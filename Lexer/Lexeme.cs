using System;
using System.Collections.Generic;

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

        public Lexeme(LexemeType type, String text)
        {
            Type = type;
            Text = text;
        }

        public override bool Equals(object obj)
        {
            return obj is Lexeme lexeme &&
                   Type == lexeme.Type &&
                   Text == lexeme.Text;
        }

        public override int GetHashCode()
        {
            int hashCode = -1204305965;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            return hashCode;
        }

    }
}
