using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine(expr.GetType());

            if (expr is SExprSymbol symbol)
            {
                return env[symbol.Value];
            }
            else if (expr.GetType().IsSubclassOf(typeof(SExprValueAtom<>)))
            {
                Console.WriteLine("HERE");
            }
            else if (expr is SExprList list)
            {
                // тут мы рассматриваем различные специальные формы,
                // которые НЕ являются функциями, так как 
                // параметры в них оцениваются по другому, нежели в случае функций
                // (простейший пример - if)
                var head = list[0];

                if (head is SExprList firstElemList)
                    ; //todo

                var operation = head as SExprAtom;



                //в конце, если ничего не найдено - вызов функции с именем operation.Symbol
            }

            return null;
        }
    }
}
