using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class Evaluator
    {
        private static StandardEvaluationEnvironment GlobalEnv = new StandardEvaluationEnvironment();

        private static bool GlobalEnvInitialized = false;

        static public SExpr Evaluate(SExpr expr)
        {
            if(!GlobalEnvInitialized)
            {
                GlobalEnv.Init();
                GlobalEnvInitialized = true;
            }

            return Evaluate(expr, GlobalEnv);
        }

        /*
            как сделать хвостовую рекурсию (принудительно, а не как в Clojure):
            в этой функции Evaluate всё обернуть в цикл while(true)
            ну и некоторое придется поменять
            например, пусть: 
            define func (lambda (x) (if (...) (func (- x 1)) 1))
            и надо оценить вызов функции (и сразу вернуть результат!): 
            (func 5) - Evaluate(SExpr(func 5), env)
            что мы делали раньше? мы создавали новый env, 
            помещали туда имя аргумента со значением 5 и вызывали Evaluate 
            (то есть рекурсия!)
            теперь же мы заменяем env на новый и expr на новый -тело лямбды

            то есть evaluate( (if (...) (func (- x 1)) 1) ,  env = {x : 5; parent = oldEnv}) 
            в if: смотрим условие, и в зависимости от условия также меняем expr
            но это можно делать, только если if последний?
            а вообще - надо так. Все, кроме последнего expr в body оцениваем по старому 
            (аргументы в функциях - тоже по старому).
            а последнее выражение тела - по новому!

            пусть так. пусть условие верно и надо оценить (func (- x 1)).
            тут легко: оцениваем аргумент с помощью обычного, старого Evaluate 
            (с аргументами функций ничего уж не поделать - они априори не последние)
            и далее снова меняем env!

            еще пример: пусть было (if (...) (+ (func (- x 1)) 1) 1)
            то есть func - не последняя, это аргумент, и в этом случае stackoverflow будет
            но вызова evaluate для плюса не будет, это будет в цикле

            таким образом, на самом деле если последнее возвращаемое значение -
            результат вызова функции (неважно даже, этой или другой)
            мы не создаём новый стек фрейм
        */

        static public SExpr Evaluate(SExpr expr, EvaluationEnvironment env)
        {
            while(true)
            {
                if (expr is SExprSymbol symbol)
                {
                    string symbolValue = symbol.Value;

                    var ret = env[symbolValue]; 
                    if(ret == null)
                        throw new EvaluationException($"Symbol {symbolValue} not found");
                    return ret;
                }
                else if (expr is SExprAbstractValueAtom)
                {
                    return expr;
                }
                else if (expr is SExprList list)
                {
                    var args = list.GetArgs();

                    // тут мы рассматриваем различные специальные формы (!),
                    // которые НЕ являются функциями, так как 
                    // параметры в них оцениваются по другому, нежели в случае функций
                    // (простейший пример - if)
                    var head = list[0];

                    if (head is SExprSymbol listHeadSymbol)
                    {

                        var value = listHeadSymbol.Value;
                        if (value == "if")
                        {
                            if(args.Count != 3)
                                throw new EvaluationException("There should be three arguments in if statement");

                            var cond = list[1];
                            var ifTrue = list[2];
                            var ifFalse = list[3];

                            /*SExpr condTrue = Evaluate(cond, env);
                            //всё, что не ложь - правда
                            if(condTrue is SExprAbstractValueAtom atom && atom.GetCommonValue() is bool condTrueBool && !condTrueBool)
                            {
                                return Evaluate(ifFalse, env);
                            }
                            return Evaluate(ifTrue, env);*/
                            var ifList = new SExprList();
                            ifList.AddSExprToList(new SExprSymbol("cond"));
                            ifList.AddSExprToList(cond);
                            ifList.AddSExprToList(ifTrue);
                            ifList.AddSExprToList(new SExprBool(true));
                            ifList.AddSExprToList(ifFalse);
                            
                            //return Evaluate(ifList, env);
                            expr = ifList;
                            continue;
                        }
                        else if (value == "cond")
                        {
                            //(cond (cond expr)*)
                            if(args.Count % 2 != 0)
                                throw new EvaluationException("There should be an even number of arguments in cond statement");
                            
                            SExpr trueExpr = null;
                            for (int i = 0; i < args.Count; i += 2)
                            {
                                var cond = args[i];
                                var condExpr = args[i + 1];

                                SExpr evaluatedCond = Evaluate(cond, env);

                                if(evaluatedCond is SExprAbstractValueAtom atom && (atom.GetCommonValue() == null || (atom.GetCommonValue() is bool condBool && !condBool)))
                                    continue;
                                //return Evaluate(condExpr, env); 
                                trueExpr = condExpr;
                                break;
                            }

                            if(trueExpr == null)
                                return new SExprObject(null);
                            else {
                                expr = trueExpr;
                                continue;
                            }
                        }
                        else if (value == "define")
                        {
                            //синтаксис: define symbol exp
                            if(args.Count != 2)
                                throw new EvaluationException($"Wrong parameter count in definintion, should be 2 instead of {args.Count}");
                            if(args[0] is SExprSymbol defineSymbol)
                            {
                                var ret = Evaluate(args[1], env);
                                GlobalEnv[defineSymbol.Value] = ret;
                                return ret;
                            }
                            else 
                                throw new EvaluationException("First argument in definition should be a symbol!");
                        }
                        else if (value == "lambda")
                        {
                            //синтаксис: (lambda (symbol...) exp)
                            //пример (lambda (r) (* r (* r r)))
                            if (args[0] is SExprList lambdaArguments)
                            {
                                List<SExprSymbol> symbolArguments = new List<SExprSymbol>();
                                foreach (var arg in lambdaArguments.GetElements())
                                {
                                    if(arg is SExprSymbol symbolArg) 
                                        symbolArguments.Add(symbolArg);
                                    else
                                        throw new EvaluationException("Parameter in lambda definition is not symbolic");
                                } 


                                args.RemoveAt(0);
                                //сюда мы пришли с некоторым окружением, возможно неглобальным. Но мы ничего с ним не делаем, мы его теряем
                                var body = args;

                                return new SExprLambda(symbolArguments, body, env);

                            }
                            else
                                throw new EvaluationException("Lambda definition should have a list of symbol parameters");
                        }
                        else if (value == "let")
                        {
                            //(let (bindings)* exprs*); //exprs* - это body
                            if (args[0] is SExprList letBindings)
                            {
                                var letBindingsList = letBindings.GetElements();
                                if(letBindingsList.Count % 2 != 0)
                                    throw new EvaluationException("There should be an even number of elements in list of bindings");

                                var letEnvironment = new EvaluationEnvironment(env); 
                                for (int i = 0; i < letBindingsList.Count; i+=2)
                                {
                                    var symbolIndex = i;
                                    var valueIndex = i + 1;
                                    if(letBindingsList[symbolIndex] is SExprSymbol symbolArg) 
                                    {
                                        letEnvironment[symbolArg.Value] = Evaluate(letBindingsList[valueIndex], letEnvironment);
                                    }
                                    else
                                        throw new EvaluationException($"Parameter №{i} in let is not a symbol");
                                }

                                args.RemoveAt(0);
                                var body = args;
                                SExpr ret = null;

                                if(body.Count == 0)
                                    return new SExprObject(null);

                                for (int i = 0; i < body.Count - 1; i++)
                                {
                                    var bodyExpr = body[i];
                                    ret = Evaluate(bodyExpr, letEnvironment);
                                }

                                expr = body[body.Count - 1];
                                env = letEnvironment;
                                continue;

                            }
                            else
                                throw new EvaluationException("Second argument of let should be a list of bindings");
                        }
                        else if (value == "quote")
                        {
                            //(quote exp)
                            if(args.Count != 1)
                                throw new EvaluationException($"Wrong parameter count in quotation, should be 1 instead of {args.Count}");
                            //todo: check unquote?
                            return args[0];
                        }
                        else if (value == "throw")
                        {
                            //(throw expr), where expr should evaluate to Exception (analogue of Throwable in Java)

                            if(args.Count != 1)
                                throw new EvaluationException($"Throw should only have one argument, not {args.Count}");

                            var throwExpr = args[0];
                            var evaluatedThrowExpr = Evaluate(throwExpr, env);
                            if(evaluatedThrowExpr is SExprAbstractValueAtom valueExpr && valueExpr.GetCommonValue() is Exception e)
                                throw e;
                            throw new EvaluationException("Argument of throw is not an exception!");
                        }
                        else if (value == "try")
                        {
                            // (try expr* catches* finally?)
                            // catch is (catch Exception e exprs*)
                            int i;
                            List<SExpr> tryBody = new List<SExpr>();
                            for (i = 0; i < args.Count; i++)
                            {
                                var tryExpr = args[i];
                                if (tryExpr is SExprList tryList && tryList[0] is SExprSymbol trySymbol
                                    && trySymbol.Value == "catch")
                                    break;
                                else
                                    tryBody.Add(tryExpr);
                            }
                            //после этого должны быть только catch (может, 0?) и, опционально, finally  
                            //словарь для типов Exception, здесь список имеет особый вид: первый элемент - SExprSymbol - имя переменной (для нее создадим контекст внутренний)
                            //Dictionary<Type, List<SExpr>> exceptionDict = new Dictionary<Type, List<SExpr>>();
                            ExceptionDictionary exceptionDict = new ExceptionDictionary();
                            List<SExpr> finallyBody = new List<SExpr>();
                            for (; i < args.Count; i++)
                            {
                                var tryExpr = args[i];
                                if (tryExpr is SExprList tryList && tryList[0] is SExprSymbol trySymbol)
                                {
                                    if (trySymbol.Value == "catch")
                                    {
                                        //(catch ExceptionType e expr * )

                                        var catchArgs = tryList.GetArgs();
                                        if (catchArgs.Count < 2)
                                            throw new EvaluationException("Not enough elements in catch clause");
                                        
                                        var exceptionClassName = catchArgs[0].GetText();
                                        var exceptionType = Type.GetType(exceptionClassName);
                                        if (exceptionType == null)
                                            throw new EvaluationException("Exception type not found. Perhaps you should specify the namespace.");

                                        bool isExceptionType = exceptionType.IsSubclassOf(typeof(Exception)) || exceptionType == typeof(Exception);

                                        if(!isExceptionType)
                                            throw new EvaluationException($"{exceptionClassName} is not an exception type!");

                                        if(!(catchArgs[1] is SExprSymbol))
                                            throw new EvaluationException("Exception name in catch clause is not a symbol");

                                        catchArgs.RemoveAt(0);
                                        var catchBody = catchArgs;
                                        exceptionDict[exceptionType] = catchBody;
                                        continue;
                                    }
                                    else if (trySymbol.Value == "finally")
                                    {
                                        //(finally expr*); expr* is body
                                        finallyBody = tryList.GetArgs();
                                        if(i != args.Count - 1)
                                            throw new EvaluationException("There shouldn't be any other clauses after finally in try-catch");
                                        break; //после finally ничего нет
                                    }
                                    else
                                        throw new EvaluationException("After first catch, only catch and finally clauses are allowed in try-catch");
                                }
                                else
                                    throw new EvaluationException("After first catch, only catch and finally clauses are allowed in try-catch");
                            }

                            SExpr ret = null;
                            try
                            {
                                if(tryBody.Count == 0)
                                    return new SExprObject(null);

                                //foreach (var bodyExpr in tryBody)
                                for (i = 0; i < tryBody.Count - 1; i++)
                                {
                                    var bodyExpr = tryBody[i];
                                    Evaluate(bodyExpr, env);
                                }

                                //return ret;

                                expr = tryBody[tryBody.Count - 1];
                                continue;
                            }
                            catch (Exception e)
                            {
                                ret = null;
                                
                                //если бросали из внешнего метода, то там System.Reflection.TargetInvocationException, но это решается в коде для вызова методов C#

                                var exceptionType = e.GetType();
                                
                                List<SExpr> bodyForExceptionType = exceptionDict[exceptionType];
                                if(bodyForExceptionType != null)
                                {
                                    var exceptionSymbol = (SExprSymbol)bodyForExceptionType[0];
                                    bodyForExceptionType.RemoveAt(0);

                                    var catchEnvironment = new EvaluationEnvironment(env); 
                                    catchEnvironment[exceptionSymbol.Value] = new SExprObject(e);
                                
                                    //foreach (var bodyExpr in bodyForExceptionType)
                                    if(bodyForExceptionType.Count == 0)
                                        return new SExprObject(e);

                                    for (i = 0; i < bodyForExceptionType.Count - 1; i++)
                                    {
                                        var bodyExpr = bodyForExceptionType[i];
                                        Evaluate(bodyExpr, catchEnvironment);
                                    }

                                    expr = bodyForExceptionType[bodyForExceptionType.Count - 1];
                                    env = catchEnvironment;
                                    continue;

                                    //return ret; //goes to finally
                                }
                                else
                                    throw e;

                                //(try (LispMachine.StandardLibrary\ThrowsException) (catch System.ApplicationException e (.ToUpper (.ToString e))))                        
                            }
                            finally
                            {
                                //тут плохо тем, что после "внутренних" исключений (EvaluationException) тоже выполнится
                                //todo: проверять на EvaluationException И если это оно, то ничего не делать
                                //или как вариант можно все выше сделать, без finally,
                                //но тогда EvaluateException может выкинуться и finally не выполнится?
                                //это правильное поведение?

                                foreach (var finallyExpr in finallyBody)
                                    Evaluate(finallyExpr, env);
                                //оцениваются (вдруг сайд эффекты), но не возвращаются

                            }
                        }
                        else if (value == "new")
                        {
                            //(new Classname args*); classname should be full

                            string className = null;
                            if(args[0] is SExprSymbol classNameSymbol)
                                className = classNameSymbol.Value;
                            else
                                throw new EvaluationException("First argument of new is not a symbol!");
                            var type = Type.GetType(className);
                            if (type == null)
                                throw new EvaluationException("No such class found, maybe you should use the full name with namespace?");
                            
                            var arguments = new List<object>();
                            args.RemoveAt(0);
                            foreach(var arg in args)
                            {
                                var evaluatedArg = Evaluate(arg, env);
                                if(evaluatedArg is SExprLambda)
                                    throw new EvaluationException("Wrong parameter in native call, lambdas can't be passed");
                                arguments.Add(CreateObjectFromSExpr(evaluatedArg));
                            } 

                            var constructor = type.GetConstructor(arguments.Select (x => x.GetType()).ToArray());
                            if(constructor == null)
                                throw new EvaluationException("No constructor with such arguments found");

                            try {
                                var instance = constructor.Invoke(arguments.ToArray()); 
                                return CreateSExprFromObject(instance);  
                            }
                            catch (System.Reflection.TargetInvocationException e) {
                                throw e.InnerException;
                            }
                        }
                        else if (value[0] == '.')
                        {
                            //(.methodName instance parameters*)
                            if(args.Count < 1)
                                throw new EvaluationException($"Wrong parameter count in native method call: an instance should be provided after method name");
                            var methodName = value.Substring(1);
                            var instance = args[0]; //todo: надо Evaluate
                            var evaluatedInstance = Evaluate(instance, env);

                            args.RemoveAt(0);
                            var arguments = new List<object>();
                            foreach(var arg in args)
                            {
                                var evaluatedArg = Evaluate(arg, env);
                                if(evaluatedArg is SExprLambda)
                                    throw new EvaluationException("Wrong parameter in native call, lambdas can't be passed");
                                arguments.Add(CreateObjectFromSExpr(evaluatedArg));
                            }

                            var evaluatedInstanceObject = CreateObjectFromSExpr(evaluatedInstance);
                            var type = evaluatedInstanceObject.GetType();
                            var method = type.GetMethod(methodName, arguments.Select (x => x.GetType()).ToArray());
                            try {
                                var returnedObj = method.Invoke(evaluatedInstanceObject, arguments.ToArray()); 
                                return CreateSExprFromObject(returnedObj);     
                            }
                            catch (System.Reflection.TargetInvocationException e) {
                                throw e.InnerException;
                            }
                    
                        }
                        else if (value.Contains('\\'))
                        {
                            var splat = value.Split('\\');
                            var className = splat[0];
                            var methodName = splat[1];

                            var arguments = new List<object>();
                            foreach(var arg in args)
                            {
                                var evaluatedArg = Evaluate(arg, env);
                                if(evaluatedArg is SExprLambda)
                                    throw new EvaluationException("Wrong parameter in native call, lambdas can't be passed");
                                arguments.Add(CreateObjectFromSExpr(evaluatedArg));
                            } 

                            var method = Type.GetType(className).GetMethod(methodName, arguments.Select(x => x.GetType()).ToArray());
                            try {
                                var returnedObj = method.Invoke(null, arguments.ToArray());
                                return CreateSExprFromObject(returnedObj);  
                            }
                            catch (System.Reflection.TargetInvocationException e) {
                                throw e.InnerException;
                            }
                        
                        }
                    }

                    //var call = new FunctionCall(head, args); 
                    //return call.Evaluate(env);

                    var Arguments = args.Select(x => Evaluator.Evaluate(x, env)).ToList();
                    var evaluatedHead = Evaluator.Evaluate(head, env);
            
                    if(evaluatedHead is SExprLambda lambda)
                    {
                        var lambdaSymbolArguments = lambda.LambdaArguments;

                        if(lambdaSymbolArguments.Count != Arguments.Count)
                            throw new EvaluationException("Wrong argument count passed");

                        //EvaluationEnvironment lambdaEnv = new EvaluationEnvironment(env);
                        EvaluationEnvironment lambdaEnv = new EvaluationEnvironment(lambda.Environment);    //для замыканий

                        for (int i = 0; i < Arguments.Count; i++)
                        {
                            lambdaEnv[lambdaSymbolArguments[i].Value] = Arguments[i];
                        }

                        if(lambda.Body.Count == 0)
                            return new SExprObject(null);

                        for (int i = 0; i < lambda.Body.Count - 1; i++)
                        {
                            var bodyExpr = lambda.Body[i];
                            Evaluator.Evaluate(bodyExpr, lambdaEnv);
                        }

                        var lastExpr = lambda.Body[lambda.Body.Count - 1];
                        expr = lastExpr;
                        env = lambdaEnv;
                        continue;
                    }

                    throw new EvaluationException("Not built-in function or lambda");

                }
            }

            return null; //unreachable
        }

        private static SExpr CreateSExprFromObject(object obj)
        {
            if (!(obj is string) && obj is IEnumerable enumerable)
            {
                SExprList ret = new SExprList();
                foreach (var elem in enumerable)
                    ret.AddSExprToList(CreateSExprFromObject(elem));
                return ret;
            }
            if(obj is SExpr expr)
                return expr;
            return new SExprObject(obj);
        }

        private static object CreateObjectFromSExpr(SExpr expr)
        {
            if (expr is SExprList list)
                return list.GetElements().Select(x => CreateObjectFromSExpr(x)).Cast<object>().ToList();  //todo: recursive
            if (expr is SExprAbstractValueAtom value)
                return value.GetCommonValue();
                //дополнительно можно лоцировать встроенные типы типа int и приводить
            throw new EvaluationException("Wrong argument in native call");        
        }


        public static SExpr Evaluate(string str)
        {
            var parser = new SExprParser(new StringReader(str));
            SExpr expr, evald = null;

            while ((expr = parser.GetSExpression()) != null) {
                evald = Evaluate(expr);
            };

            return evald;
        }
    }

    public class EvaluationException : Exception
    {
        public EvaluationException() { }

        public EvaluationException(string message) : base(message) { }

        public EvaluationException(string message, Exception innerException) : base(message, innerException) { }
    }
}