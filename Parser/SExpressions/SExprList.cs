using System;
using System.Collections.Generic;

namespace LispMachine
{
    class SExprList : SExpr
    {
        private List<SExpr> elements { get; }

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

        //indexer
        public SExpr this[int index]
        {
            get { return elements[index]; }
            //set { elements[index] = value }
        }

        public List<SExpr> GetArgs()
        {
            var list = new List<SExpr>(elements);
            list.RemoveAt(0);
            return list;
        }
    }
}
