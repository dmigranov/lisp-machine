using Microsoft.VisualStudio.TestTools.UnitTesting;
using LispMachine;
using System.IO;

namespace lisp_tests
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void PrimitiveTest()
        {
            var reader = new StringReader("5");
            var parser = new SExprParser(reader);

            var sexpr = parser.GetSExpression();
            Assert.IsTrue(sexpr is SExprAbstractValueAtom);

            sexpr = parser.GetSExpression();
            Assert.IsNull(sexpr);

            parser = new SExprParser(new StringReader(" \"fdf\" 5 "));
            sexpr = parser.GetSExpression();
            Assert.IsTrue(sexpr is SExprString);

            sexpr = parser.GetSExpression();
            Assert.IsTrue(sexpr is SExprInt);
            
            if(sexpr is SExprInt atom)
                Assert.AreEqual(5, atom.Value);
        }

        [TestMethod]
        public void ListTest()
        {
            var reader = new StringReader("( 5 )");
            var parser = new SExprParser(reader);

            var sexpr = parser.GetSExpression();
            Assert.IsTrue(sexpr is SExprList);

            sexpr = parser.GetSExpression();
            Assert.IsNull(sexpr);

        }

    }

}