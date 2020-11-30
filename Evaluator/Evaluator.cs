﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class Evaluator
    {
        public SExpr Evaluate(SExpr expr)
        {
            return Evaluate(expr, new EvaluationEnvironment());
        }

        public SExpr Evaluate(SExpr expr, EvaluationEnvironment env)
        {

            if (expr is SExprSymbol symbol)
            {
                return env[symbol.Value]; 
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
                        var cond = list[1];
                        var ifTrue = list[2];
                        var ifFalse = list[3];

                        SExpr condTrue = Evaluate(cond, env);
                        //todo
                    }
                    else if (value == "lambda")
                    {
                        //синтаксис: (lambda (symbol...) exp)
                        //пример (lambda (r) (* pi (* r r)))
                        if (args[0] is SExprList lambdaArguments
                            && lambdaArguments.GetElements().All(x => x is SExprSymbol))
                        {
                            args.RemoveAt(0);
                        }
                        else
                            throw new EvaluationException("lambda definition should have a list of symbol parameters");
                    }
                }


                //в конце, если ничего не найдено - считаем, что на первом месте - функция
                //TODO: в будущем, голову тоже надо оценивать, если это лябмда, но пока закомментриую
                
                
                
                var call = new FunctionCall(head, args.Select(x => Evaluate(x, env)).ToList()); //todo

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
