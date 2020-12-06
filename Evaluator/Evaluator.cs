using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class Evaluator
    {
        static EvaluationEnvironment Env = new EvaluationEnvironment();

        static public SExpr Evaluate(SExpr expr)
        {
            return Evaluate(expr, Env);
        }

        static public SExpr Evaluate(SExpr expr, EvaluationEnvironment env)
        {

            if (expr is SExprSymbol symbol)
            {
                var ret = env[symbol.Value]; 
                if(ret == null)
                    throw new EvaluationException($"Symbol {symbol.Value} not found");
                return ret;
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
                            throw new EvaluationException($"Wrong parameter count in definintion, should be 2 instead of {args.Count}");
                        if(args[0] is SExprSymbol defineSymbol)
                        {
                            env[defineSymbol.Value] = args[1];
                            //todo?
                        }
                        else throw new EvaluationException("First argument in definition should be a symbol!");
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
                    else if (value == "let")
                    {
                        
                    }
                    else if (value == "quote")
                    {
                        //(quote exp)
                        if(args.Count != 1)
                            throw new EvaluationException($"Wrong parameter count in quotation, should be 1 instead of {args.Count}");
                        return args[0];
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
