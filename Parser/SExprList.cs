using System;
using System.Collections.Generic;

namespace LispMachine
{
    class SExprList : SExpr
    {
        private List<SExpr> elements;

        public SExprList()
        {
            elements = new List<SExpr>();
        }

        public void AddSExprToList(SExpr elem)
        {
            elements.Add(elem);
        }

        public override void PrintSExpr()
        {
            Console.WriteLine("(");

            foreach (var elem in elements)
                elem.PrintSExpr();

            Console.WriteLine(")");

        }
    }
}
