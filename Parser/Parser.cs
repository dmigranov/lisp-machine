﻿using System;
using System.IO;

namespace LispMachine
{
    public class Parser
    {
        //todo: построения синтаксического дерева

        private Lexer lexer;

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
        public SExpr GetSExpression()
        {
            var lexeme = lexer.GetLexeme(); // first token === lexeme
            var lexemeType = lexeme.Type;
            if (lexemeType == LexemeType.RBRACE) 
                throw new ParserException("Unexpected left brace");

            if (lexemeType == LexemeType.LBRACE)
            {
                var list = new SExprList();

                Lexeme listLexeme;
                while ((listLexeme = lexer.GetLexeme()).Type != LexemeType.RBRACE)
                {
                    list.AddSExprToList(GetSExpression());
                }

                return list;
            }

            //Атомарное S-выражение
            return new SExprAtom(lexeme.Text);
        }
    }



    public class ParserException : Exception
    {
        public ParserException() { }

        public ParserException(string message) : base(message) { }

        public ParserException(string message, Exception innerException) : base(message, innerException) { }
    }
}

