using System;
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
                // тут мы рассматриваем различные специальные формы,
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
                }


                //в конце, если ничего не найдено - считаем, что на первом месте - функция
                //TODO: в будущем, голову тоже надо оценивать, если это лябмда, но пока закомментриую
                
                var args = list.GetArgs();
                
                var call = new FunctionCall(head, args.Select(x => Evaluate(x, env)).ToList()); //todo

                return call.Evaluate(env);


            }

            return null;
        }

    }
}
