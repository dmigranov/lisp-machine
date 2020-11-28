using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    class FunctionCall
    {
        private SExpr Function;
        private List<SExpr> Arguments;

        public FunctionCall(SExpr function, List<SExpr> arguments)
        {
            Function = function;
            Arguments = arguments;
        }

        public SExpr Evaluate(EvaluationEnvironment env)
        {
            if (Function is SExprSymbol symbol)
            {
                string operation = symbol.Value;
                switch (operation)
                {
                    //все эти операторы много-арные
                    case "+":
                        //todo: разобраться с типами (как можно сделать: если есть хоть один float, то суммируем так, иначе...
                        var ints = Arguments.Select(x => ((SExprInt)x).Value);
                        return new SExprFloat(ints.Sum());
                    case "-":
                        return null;
                    case "*":
                        ints = Arguments.Select(x => ((SExprInt)x).Value);
                        return new SExprFloat(ints.Sum());
                    case "/":
                        return null;

                    case ">":
                        break;
                    case "<":
                        break;
                    case "=":
                        break;



                    default:
                        break;
                }

            }




            //если ничего во встроенных функциях не нашли
            //var function = Evaluate(head, env);
            //var arguments = [eval(arg, env) for arg in x[1:]]
            //return proc(*args)
            //вызов функции реализуется через замыкание - добавляем все параметры в контекст

            return null;
        }
    }
}
