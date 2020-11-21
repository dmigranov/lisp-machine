using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispMachine
{
    public class Evaluator
    {
        public int Evaluate(SExpr expr)
        {
            return Evaluate(expr, new EvaluationEnvironment());
        }

        public int Evaluate(SExpr expr, EvaluationEnvironment env)
        {
            if (expr is SExprAtom atom)
            {
                //рассматриваем тут различные варианты:
                //число, строка (начинается и кончается на кавычку), булева константа
                //и в конце концов else - идентификатор переменой
            }
            else if (expr is SExprList list)
            {
                var head = list[0];

                if (head is SExprList firstElemList)
                    ; //todo

                var operation = head as SExprAtom;



                //в конце, если ничего не найдено - вызов функции с именем operation.Symbol
            }

            return 0;
        }
    }
}
