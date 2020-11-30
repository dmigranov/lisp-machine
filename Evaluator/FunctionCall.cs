using System;
using System.Collections.Generic;

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
                        return null;
                    case "/":
                        return null;
                    //вот в случае этих проверки надо как-то проводить, и в случае double выкидывать исключения?
                    case "&":
                        return null;
                    case "|":
                        return null;

                    //а эти - бинарные!
                    case ">":
                        return null;
                    case "<":
                        return null; ;
                    case "<=":
                        return null;
                    case ">=":
                        break;
                    case "=":
                        return null;
                    case "!=":
                        return null;

                    case "abs":
                        return null;

                    //операции над списками:
                    case "cons":
                        return null;
                    case "conj":
                        return null;
                    case "length":
                        return null;
                    case "list":
                        return null;

                    case "print":
                        return null;
                    case "println":
                        return null;


                    default:
                        return null; ;
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
            //если мы хотим реализовать переопределение встроенных функций, надо переместить код в начало

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
