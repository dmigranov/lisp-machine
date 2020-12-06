﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class Evaluator
    {
        static public SExpr Evaluate(SExpr expr)
        {
            return Evaluate(expr, new EvaluationEnvironment());
        }

        static public SExpr Evaluate(SExpr expr, EvaluationEnvironment env)
        {

            if (expr is SExprSymbol symbol)
            {
                return env[symbol.Value]; 
            }
            else if (expr is SExprAbstractValueAtom)
            {
                return expr;
            }
            else if (expr is SExprLambda lamda)
            {
                Console.WriteLine("HEEEEEEEEEEEEEREEE");
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
                        var cond = list[1];
                        var ifTrue = list[2];
                        var ifFalse = list[3];

                        SExpr condTrue = Evaluate(cond, env);
                        //todo
                    }
                    else if (value == "define")
                    {
                        //синтаксис: define symbol exp
                        if(args.Count != 2)
                            throw new EvaluationException($"Wrong parameter count in definintion, shold be 2 instead of {args.Count}");
                    }
                    else if (value == "lambda")
                    {
                        //синтаксис: (lambda (symbol...) exp)
                        //пример (lambda (r) (* pi (* r r)))
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
                            var body = args;

                            return new SExprLambda(symbolArguments, body);
                        }
                        else
                            throw new EvaluationException("Lambda definition should have a list of symbol parameters");
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
