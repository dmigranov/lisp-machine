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

            }
            else if (expr is SExprList list)
            {

            }

            return 0;
        }
    }
}
