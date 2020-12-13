using System;
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


        static public SExpr Evaluate(SExpr expr, EvaluationEnvironment env)
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
                        //todo: проверка на правильное число аргументов
                        var cond = list[1];
                        var ifTrue = list[2];
                        var ifFalse = list[3];

                        SExpr condTrue = Evaluate(cond, env);
                        //всё, что не ложь - правда
                        if(condTrue is SExprAbstractValueAtom atom && atom.GetCommonValue() is bool condTrueBool && !condTrueBool)
                        {
                            return Evaluate(ifFalse, env);
                        }
                        return Evaluate(ifTrue, env);
                    }
                    else if (value == "cond")
                    {
                        //(cond (cond expr)*)
                        if(args.Count % 2 != 0)
                            throw new EvaluationException("There should be an even number of arguments in cond statement");
                        for (int i = 0; i < letBindingsList.Count; i+=2)
                        {
                            
                        }

                        return null;
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
                            foreach (var bodyExpr in body)
                            {
                                ret = Evaluate(bodyExpr, letEnvironment);
                            }
                            return ret;
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
                    else if (value == "new")
                    {
                        //(new Classname args*)
                        return null;
                    }
                    else if (value == "throw")
                    {
                        //(throw expr), where expr should evaluate to throwable
                        return null;
                    }
                    else if (value == "try")
                    {
                        // (try expr* catches* finally?)
                        // catch is (catch Exception e exprs*)
                        //todo: try catch...
                        return null;
                    }
                    else if (value == "new")
                    {
                        //todo
                        return null;
                    }
                    else if (value[0] == '.')
                    {
                        //(.methodName instance parameters*)
                        if(args.Count < 1)
                            throw new EvaluationException($"Wrong parameter count in native method call: an instance should be provided after method name");
                        var methodName = value.Substring(1);
                        var instance = args[0]; //todo: надо Evaluate
                        var evaluatedInstance = Evaluate(instance);

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
                        var returnedObj = method.Invoke(evaluatedInstanceObject, arguments.ToArray()); 
                        return CreateSExprFromObject(returnedObj);                      
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
                        var returnedObj = method.Invoke(null, arguments.ToArray());
                        return CreateSExprFromObject(returnedObj);                      
                    }
                }

                var call = new FunctionCall(head, args); 
                return call.Evaluate(env);
            }

            return null;
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
    }

    public class EvaluationException : Exception
    {
        public EvaluationException() { }

        public EvaluationException(string message) : base(message) { }

        public EvaluationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
