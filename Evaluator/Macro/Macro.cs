using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class Macro   //от кого он должен наслеоваться?
    {
        private List<SExprSymbol> macroArguments;
        public List<SExprSymbol> MacroArguments { 
            get {
                return new List<SExprSymbol>(macroArguments);
            }
        }
        public SExpr Body { get; } //тело может состоять из нескольких выражений, возвращаем последнее
        
        
        public Macro(List<SExprSymbol> args, SExpr body)
        {
            Body = body;
            macroArguments = args;
        }
    }
 }