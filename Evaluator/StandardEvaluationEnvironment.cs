using System;
using System.Text;
using System.IO;

namespace LispMachine
{
    public class StandardEvaluationEnvironment : EvaluationEnvironment
    {
        private string predefined = @"

            (define + (lambda (x y) (LispMachine.StandardLibrary\Plus x y)))


            (define - (lambda (x y) (LispMachine.StandardLibrary\Minus x y)))


            (define * (lambda (x y) (LispMachine.StandardLibrary\Multiply x y)))

    
            (define / (lambda (x y) (LispMachine.StandardLibrary\Divide x y)))


            (define > (lambda (x y) (LispMachine.StandardLibrary\More x y)))
            (define < (lambda (x y) (LispMachine.StandardLibrary\More y x)))
            (define <= (lambda (x y) (LispMachine.StandardLibrary\MoreEqual y x)))
            (define >= (lambda (x y) (LispMachine.StandardLibrary\MoreEqual x y)))
            (define = (lambda (x y) (LispMachine.StandardLibrary\Equal x y)))

            (define & (lambda (x y) (LispMachine.StandardLibrary\And x y)))
            (define | (lambda (x y) (LispMachine.StandardLibrary\Or x y)))

            ";

//деление: / пока нельзя, надо исправить в евалуэйт

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