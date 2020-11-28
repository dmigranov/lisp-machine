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

                        return Sum();
                    case "-":
                        return null;
                    case "*":
                        //ints = Arguments.Select(x => ((SExprInt)x).Value);
                        //return new SExprFloat(ints.Sum());
                        return null;
                    case "/":
                        return null;

                    //а эти - бинарные!
                    case ">":
                        break;
                    case "<":
                        break;
                    case "<=":
                        break;
                    case ">=":
                        break;
                    case "=":
                        break;



                    default:
                        break;
                }

            }



            //если же ничего во встроенных функциях не нашли, или это лямбда?
            //то это "лисповская" функция
            //включая встроенные - в глобальное (корневое) окружение при его создании в конструкторе
            //тогда ищем в окружении такую функцию
            //var function = Evaluate(head, env);
            //var arguments = [eval(arg, env) for arg in x[1:]]
            //return proc(*args)
            //вызов функции реализуется через замыкание - добавляем все параметры в контекст

            return null;
        }


        private SExpr Sum()
        {
            //todo: может, ещё с выходным типом разобраться?

            double sum = 0;
            foreach (SExprAbstractValueAtom arg in Arguments)
            {
                sum += Convert.ToDouble(arg.GetCommonValue());
            }
            return new SExprFloat(sum);
        }
    }
}
