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
                    
                    case "+":
                        var ints = Arguments.Select(x => ((SExprInt)x).Value);
                        return new SExprFloat(ints.Sum());


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
