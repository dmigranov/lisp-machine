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

            (define println (lambda (x) (LispMachine.StandardLibrary\Println x)))
            (define readln (lambda () (LispMachine.StandardLibrary\Readln)))

            (define count (lambda (x) (LispMachine.StandardLibrary\Count x)))
            (define cons (lambda (x seq) (LispMachine.StandardLibrary\Cons x seq)))
            (define conj (lambda (seq x) (LispMachine.StandardLibrary\Conj seq x)))
            (define first (lambda (seq) (LispMachine.StandardLibrary\First seq)))
            (define second (lambda (seq) (LispMachine.StandardLibrary\Second seq)))

            
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