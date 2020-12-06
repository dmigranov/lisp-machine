using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    class FunctionCall
    {
        private SExpr Function;
        private List<SExpr> Arguments;

        //function и arguments - не-evaluated
        public FunctionCall(SExpr function, List<SExpr> arguments)
        {
            Function = function;
            Arguments = arguments;
        }

        public SExpr Evaluate(EvaluationEnvironment env)
        {
            Arguments = Arguments.Select(x => Evaluator.Evaluate(x, env)).ToList();

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
            //тогда сначала оцениваем нашу функцию (вдруг это лямбда), потом ищем в окружении такую функцию
            //var function = Evaluate(head, env);
            //var arguments = [eval(arg, env) for arg in x[1:]]
            //return proc(*args)
            //вызов функции реализуется через замыкание - добавляем все параметры в контекст
            //если мы хотим реализовать переопределение встроенных функций, надо переместить код в начало


            var evaluatedHead = Evaluator.Evaluate(Function, env);
            if(evaluatedHead is SExprLambda lambda)
            {
                Console.WriteLine("lambda!!!");
                if(lambda.LambdaArguments.Count != Arguments.Count)
                    throw new EvaluationException("Wrong argument count passed");
            }

            //todo: вызвать функцию с evaluatedHead и аргументами и оценить её
            //реализовать класс Closure?


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
