﻿using System;
using System.IO;

namespace LispMachine
{
    public class Parser
    {
        //todo: построения синтаксического дерева

        private Lexer lexer;
        private Lexeme currentLexeme;
        public static void TestMethod()
        {
            Console.WriteLine("Parser message");
        }

        public Parser(TextReader reader)
        {
            lexer = new Lexer(reader);
        }

        /// <summary>
        /// The method reads and returns next S-Expression from the reader passed to the constructor
        /// </summary>
        /// <returns>SExpr - next SExpression</returns>
        public SExpr GetSExpression()
        {
            currentLexeme = lexer.GetLexeme(); // first token === lexeme
            return GetSExpressionRecursive();
        }

        private SExpr GetSExpressionRecursive()
        {
            var lexemeType = currentLexeme.Type;

            if (lexemeType == LexemeType.EOF)
                return null;

            if (lexemeType == LexemeType.RBRACE) 
                throw new ParserException("Unexpected right brace!");

            if (lexemeType == LexemeType.LBRACE)
            {
                SExprList list = new SExprList();

                while ((currentLexeme = lexer.GetLexeme()).Type != LexemeType.RBRACE)
                {
                    if (currentLexeme.Type == LexemeType.EOF)
                        throw new ParserException("Not enough right braces!");
                    list.AddSExprToList(GetSExpressionRecursive());
                }

                return list;
            }

            //Атомарное S-выражение
            string text = currentLexeme.Text;
            return new SExprAtom(text);
        }
    }

    public class ParserException : Exception
    {
        public ParserException() { }

        public ParserException(string message) : base(message) { }

        public ParserException(string message, Exception innerException) : base(message, innerException) { }
    }
}

