using Microsoft.VisualStudio.TestTools.UnitTesting;
using LispMachine;
using System.IO;

namespace lisp_tests
{

    [TestClass]
    public class EvaluatorTest
    {
        [TestMethod]
        public void PrimitiveTest()
        {
            var parser = new SExprParser(new StringReader("5"));
            var res = Evaluator.Evaluate(parser.GetSExpression());
            Assert.AreEqual(5, ((SExprAbstractValueAtom)res).GetCommonValue());

            parser = new SExprParser(new StringReader("(+ 3 5)"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            Assert.AreEqual(8, (double)((SExprAbstractValueAtom)res).GetCommonValue(), 0.001);

            parser = new SExprParser(new StringReader("(+ 3.5 5)"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            Assert.AreEqual(8.5, ((SExprAbstractValueAtom)res).GetCommonValue());

            parser = new SExprParser(new StringReader("(define x 5)"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            parser = new SExprParser(new StringReader("x"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            Assert.AreEqual(5, ((SExprAbstractValueAtom)res).GetCommonValue());

            parser = new SExprParser(new StringReader("((lambda (a) (+ a a)) 3)"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            Assert.AreEqual(6, (double)((SExprAbstractValueAtom)res).GetCommonValue(), 0.001);

            parser = new SExprParser(new StringReader("(let (y 1) y)"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            Assert.AreEqual(1, ((SExprAbstractValueAtom)res).GetCommonValue());

            parser = new SExprParser(new StringReader("(if true 10 100)"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            Assert.AreEqual(10, ((SExprAbstractValueAtom)res).GetCommonValue());

            parser = new SExprParser(new StringReader("(if false 10 100)"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            Assert.AreEqual(100, ((SExprAbstractValueAtom)res).GetCommonValue());

            parser = new SExprParser(new StringReader("(.CompareTo 55 67)"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            Assert.AreEqual(-1, ((SExprAbstractValueAtom)res).GetCommonValue());

            parser = new SExprParser(new StringReader("(defmacro print2 (x) (quote (let (str1 x str2 x) (println str1) (println str2)))) "));
            res = Evaluator.Evaluate(parser.GetSExpression());
            parser = new SExprParser(new StringReader("(print2 (.Next (new System.Random) 0 100))  "));
            res = Evaluator.Evaluate(parser.GetSExpression());

            parser = new SExprParser(new StringReader("(try (throw (new System.ApplicationException \"MY MESSAGE!!!\")) (catch System.Exception e (println \"Exception caught\") -1))"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            Assert.AreEqual(-1, ((SExprAbstractValueAtom)res).GetCommonValue());

            parser = new SExprParser(new StringReader("(define arg-count (lambda args (count args)))"));
            res = Evaluator.Evaluate(parser.GetSExpression());

            parser = new SExprParser(new StringReader("(apply ++ (list 1 2 3 4 5))"));
            res = Evaluator.Evaluate(parser.GetSExpression());
            
            parser = new SExprParser(new StringReader("(System.String\\Concat 653 6)"));      
            res = Evaluator.Evaluate(parser.GetSExpression());

            
            parser = new SExprParser(new StringReader("(macroexpand (quote (print2 \"hello\")))"));      
            res = Evaluator.Evaluate(parser.GetSExpression());



        }


    }

}