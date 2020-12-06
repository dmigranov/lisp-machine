using System;
using System.Text;
using System.IO;

namespace LispMachine
{
    public class StandardEvaluationEnvironment : EvaluationEnvironment
    {
        private string predefined = @"

            (define + (lambda (x y) (LispMachine.StandardLibrary/Plus x y)))

            ";

        public StandardEvaluationEnvironment() : base()
        {
        
        }

        public void Init()
        {
            SExprParser parser = new SExprParser(new StringReader(predefined));
            SExpr expr;

            while ((expr = parser.GetSExpression()) != null) {
                expr.PrintSExpr();
                var evald = Evaluator.Evaluate(expr, this);
                if(evald != null)
                {
                    evald.PrintSExpr();
                }
            };
        }


    }
}