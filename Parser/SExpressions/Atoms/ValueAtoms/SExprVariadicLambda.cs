using System;
using System.Collections.Generic;

namespace LispMachine
{
    public class SExprVariadicLambda : SExprLambda 
    {
        
        public SExprSymbol ArgListSymbol { get; }

        public SExprVariadicLambda(SExprSymbol symbol, List<SExpr> body, EvaluationEnvironment env) : base(null, body, env)
        {
            ArgListSymbol = symbol;
        }

        public override string GetText()
        {
            return $"Variadic lambda of argument {ArgListSymbol.Value}" ;
        }


        public override string ToString()
        {
            return GetText();
        }
    }

}