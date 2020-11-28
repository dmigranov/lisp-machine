using System;

namespace LispMachine
{

    public class SExprAtom : SExpr
    {
        public override void PrintSExpr()
        {
            Console.WriteLine("ATOM");
        }
    }
}
