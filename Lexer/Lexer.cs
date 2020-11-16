using System;
using System.IO;
using System.Text;

namespace LispMachine
{

    // Lexer === Tokenizer
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

        public Lexeme GetLexeme()
        {
            while (Char.IsWhiteSpace((char)currentCharAsInt))
            {
                currentCharAsInt = reader.Read();
            }

            if (currentCharAsInt == -1)
            {
                return new Lexeme(LexemeType.EOF);
            }

            if(Char.IsDigit((char)currentCharAsInt))
            {
                //значит, это цифра (имена переменных не могут начинаться с цифр)

            }

            switch (currentCharAsInt)
            {
                case ('('):
                    currentCharAsInt = reader.Read();
                    return new Lexeme(LexemeType.LBRACE);
                case (')'):
                    currentCharAsInt = reader.Read();
                    return new Lexeme(LexemeType.RBRACE);

                //todo: quotes (') and unquotes? (,)


                default:
                    StringBuilder builder = new StringBuilder();
                    while (!Char.IsWhiteSpace((char)currentCharAsInt) && currentCharAsInt != '(' && currentCharAsInt != ')')
                    {
                        builder.Append((char)currentCharAsInt);
                        currentCharAsInt = reader.Read();
                    }
                    var str = builder.ToString();
                    //todo: разделение между символами и числами? первые не могут начинаться с цифры

                    return new Lexeme(str);
            }



        }

    }
}
