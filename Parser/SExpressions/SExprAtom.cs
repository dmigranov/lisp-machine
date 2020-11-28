using System;

namespace LispMachine
{

    public class SExprAtom<T> : SExpr
    {
        public T Symbol { get; }

        protected SExprAtom(T symbol)
        {
            Symbol = symbol;
        }

        public override void PrintSExpr()
        {
            Console.WriteLine(Symbol);
        }
    }
}
