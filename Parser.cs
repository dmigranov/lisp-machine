using System;
using System.IO;

namespace LispMachine
{
    public class Parser
    {
        //todo: построения синтаксического дерева

        Lexer lexer;

        public static void TestMethod()
        {
            Console.WriteLine("Parser message");
        }

        public Parser(TextReader reader)
        {
            lexer = new Lexer(reader); 
        }


        /// <summary>
        /// The method reads and returns next S-Expression from reader passed to the constructor
        /// </summary>
        /// <returns>SExpr - next SExpression</returns>
        public /*SExpr*/ void GetSExpression()
        {
            var lexeme = lexer.GetLexeme(); // first token === lexeme
            var lexemeType = lexeme.Type;
            if (lexemeType == LexemeType.RBRACE) 
                throw new Exception("Unexpected '(' character");

            if (lexemeType == LexemeType.LBRACE)
            {
                //todo

                //return список ...
            }

            //Атомарное S-выражение
            //return атомарное ...
        }

    }

}

