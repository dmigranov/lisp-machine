using System;

namespace LispMachine
{
    public class SExprValueAtom<T> : SExprAtom
    {
        public T Value { get; }

        protected SExprValueAtom(T value)
        {
            Value = value;
        }

        public override void PrintSExpr()
        {
            Console.WriteLine(Value);
        }
    }
}

