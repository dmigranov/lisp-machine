using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispMachine
{
    public enum LexemeType
    {
        SYMBOL,                 //including number?
        LBRACE, RBRACE,
        EOF
    }
}
