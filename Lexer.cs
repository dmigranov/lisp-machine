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

        public Lexer(TextReader reader)
        {
            this.reader = reader;
        }

    }
}
