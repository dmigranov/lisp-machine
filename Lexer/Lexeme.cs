using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispMachine
{
    public class Lexeme
    {
        public LexemeType Type { get; }
        public string Text { get; }

        public Lexeme(LexemeType type)
        {
            this.Type = type;
        }

        public Lexeme(String text)
        {
            this.Type = LexemeType.SYMBOL;
            this.Text = text;
        }

        //todo: equals?
    }
}
