using System;

namespace LispMachine
{
    public class Lexeme
    {
        public LexemeType Type { get; }
        public string Text { get; }

        public Lexeme(LexemeType type)
        {
            Type = type;
        }

        public Lexeme(String text)
        {
            Type = LexemeType.SYMBOL;
            Text = text;
        }

        //todo: equals?
    }
}
