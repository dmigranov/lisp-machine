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
        private int currentCharAsInt;


        public Lexer(TextReader reader)
        {
            this.reader = reader;

            int temp;
            if ((temp = reader.Read()) == -1)
                throw new Exception();
            currentCharAsInt = temp;
        }

        public int GetLexeme()
        {
            while (Char.IsWhiteSpace((char)currentCharAsInt))
            {
                currentCharAsInt = reader.Read();
            }

            if (currentCharAsInt == -1)
            {
                Console.WriteLine("EOF");
                return -1;
            }

            if(Char.IsDigit((char)currentCharAsInt))
            {
                //значит, это цифра (имена переменных не могут начинаться с цифр)
                StringBuilder builder = new StringBuilder();
                while (Char.IsDigit((char)currentCharAsInt))
                {
                    builder.Append((char)currentCharAsInt);
                    currentCharAsInt = reader.Read();
                }
                Console.WriteLine(builder.ToString());
            }

            switch (currentCharAsInt)
            {
                case ('('):
                    Console.WriteLine("(");
                    break;
                case (')'):
                    Console.WriteLine(")");
                    break;
                //todo: quotes (') and unquotes? (,)


                default:
                    //имя
                    break;
            }

            currentCharAsInt = reader.Read();
            return 0;

        }

    }
}
