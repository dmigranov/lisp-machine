using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispMachine
{

    // Lexer = Tokenizer
    public class Lexer
    {
        private TextReader reader;
        private int currentChar;


        public Lexer(TextReader reader)
        {
            this.reader = reader;

            int temp;
            if ((temp = reader.Read()) == -1)
                throw new Exception();
            currentChar = Convert.ToChar(temp);
        }

        public int GetLexeme()
        {
            while (Char.IsWhiteSpace(currentChar))
            {
                currentChar = Convert.ToChar(reader.Read());
            }

            if (currentChar == -1)
            {
                Console.WriteLine("EOF");
                return -1;
            }

            switch (currentChar)
            {
                case ('('):
                    Console.WriteLine("(");
                    break;
                case (')'):
                    Console.WriteLine(")");
                    break;
                default:
                    Console.Write(currentChar);
                    break;
            }

            currentChar = Convert.ToChar(reader.Read());
            return 0;

        }

    }
}
