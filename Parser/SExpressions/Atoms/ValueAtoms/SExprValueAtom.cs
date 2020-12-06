using System;

namespace LispMachine
{
    public class SExprValueAtom<T> : SExprAbstractValueAtom
    {
        public T Value { get; }

        protected SExprValueAtom(T value)
        {
            Value = value;
        }

        public override void PrintSExpr()
        {
            //Console.WriteLine(this.GetType() + " " + Value);
            Console.WriteLine(Value);
        }

        public override string GetText()
        {
            return Value.ToString();
        }

        public override object GetCommonValue()
        {
            return Value;
        }
    }
}

