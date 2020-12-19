using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class Macro   //от кого он должен наслеоваться?
    {
        public List<SExprSymbol> MacroArguments { get; }
        public List<SExpr> Body { get; } //тело может состоять из нескольких выражений, возвращаем последнее
        
        
        public Macro(List<SExprSymbol> args, List<SExpr> body)
        {
            Body = body;
            MacroArguments = args;
        }
    }
 }