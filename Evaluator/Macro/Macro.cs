using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class Macro   //от кого он должен наслеоваться?
    {
        public List<SExprSymbol> MacroArguments { get; }
        public SExpr Body { get; } 
        
        
        public Macro(List<SExprSymbol> args, SExpr body)
        {
            Body = body;
            MacroArguments = args;
        }
    }
 }