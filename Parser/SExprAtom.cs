
namespace LispMachine
{

    public class SExprAtom : SExpr
    {
        public string Symbol { get; }


        public SExprAtom(string symbol)
        {
            Symbol = symbol;
        }
    }
}
