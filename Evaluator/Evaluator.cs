﻿using System;
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
                        return args[0];
                    }
                    else if(value.Contains('\\'))
                    {
                        var splat = value.Split('\\');
                        var className = splat[0];
                        var methodName = splat[1];

                        var arguments = new List<object>();
                        foreach(var arg in args)
                        {
                            var evaluatedArg = Evaluate(arg, env);
                            if (evaluatedArg is SExprAbstractValueAtom valueArg)
                                arguments.Add(valueArg.GetCommonValue());
                            else if (evaluatedArg is SExprList listArg)
                                arguments.Add(listArg.GetElements().Cast<object>().ToList());
                            else
                                throw new EvaluationException("Wrong argument in native call");
                        
                            /*//todo: переделать: сначала всё эвалуэйт
                            if (arg is SExprAbstractValueAtom valueArg)
                                arguments.Add(valueArg.GetCommonValue());
                            else if (arg is SExprSymbol symbolArg && Evaluator.Evaluate(symbolArg, env) is SExprAbstractValueAtom evaluatedArg)
                                arguments.Add(evaluatedArg.GetCommonValue());
                            else if (arg is SExprList listArg)
                                ; //todo: просто возвращать список
                            else
                                throw new EvaluationException("Wrong argument in native call");*/
                        } 

                        var obj = Type.GetType(className).GetMethod(methodName).Invoke(null, arguments.ToArray());
                        return new SExprObject(obj);
                    }
                }

                
                var call = new FunctionCall(head, args); 
                return call.Evaluate(env);


            }

            return null;
        }

    }

    public class EvaluationException : Exception
    {
        public EvaluationException() { }

        public EvaluationException(string message) : base(message) { }

        public EvaluationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
