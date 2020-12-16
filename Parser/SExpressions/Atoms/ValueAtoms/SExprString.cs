namespace LispMachine
{
    public class SExprString : SExprValueAtom<string>
    {
        public SExprString(string str) : base(str)
        { }
    }
}
