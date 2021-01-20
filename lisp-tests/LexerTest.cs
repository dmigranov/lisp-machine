using Microsoft.VisualStudio.TestTools.UnitTesting;
using LispMachine;
using System.IO;


namespace lisp_tests
{
    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        public void PrimitiveTest()
        {
            var reader = new StringReader("    4  7   ");
            var lexer = new Lexer(reader);

            var lexeme = lexer.GetLexeme();
            Assert.AreEqual(LexemeType.SYMBOL, lexeme.Type);
            Assert.AreEqual("4", lexeme.Text);

            lexeme = lexer.GetLexeme();
            Assert.AreEqual(LexemeType.SYMBOL, lexeme.Type);
            Assert.AreEqual("7", lexeme.Text);

            lexeme = lexer.GetLexeme();
            Assert.AreEqual(LexemeType.EOF, lexeme.Type);


            reader = new StringReader("");
            Assert.ThrowsException<LexerException>( () => new Lexer(reader));
        }

        [TestMethod]
        public void ListTest()
        {
            var reader = new StringReader("  (   4  )   ");
            var lexer = new Lexer(reader);


            var lexeme = lexer.GetLexeme();
            Assert.AreEqual(LexemeType.LBRACE, lexeme.Type);

            lexeme = lexer.GetLexeme();
            Assert.AreEqual(LexemeType.SYMBOL, lexeme.Type);
            Assert.AreEqual("4", lexeme.Text);

            lexeme = lexer.GetLexeme();
            Assert.AreEqual(LexemeType.RBRACE, lexeme.Type);

            lexeme = lexer.GetLexeme();
            Assert.AreEqual(LexemeType.EOF, lexeme.Type);
        }
    }
}
