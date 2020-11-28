

namespace LispMachine
{
    public class SExprSymbol : SExprAtom<string>
    {
        public SExprSymbol(string symbol) : base(symbol)
        { }
    }
}
