using System;
using System.Collections.Generic;

namespace LispMachine
{
    class SExprList : SExpr
    {
        private List<SExpr> Elements { get; }

        public SExprList()
        {
            Elements = new List<SExpr>();
        }

        public void AddSExprToList(SExpr elem)
        {
            Elements.Add(elem);
        }

        public override void PrintSExpr()
        {
            Console.WriteLine("(");

            foreach (var elem in Elements)
                elem.PrintSExpr();

            Console.WriteLine(")");

        }

        //indexer
        public SExpr this[int index]
        {
            get { return Elements[index]; }
            //set { elements[index] = value }
        }

        public List<SExpr> GetArgs()
        {
            var list = new List<SExpr>(Elements);
            list.RemoveAt(0);
            return list;
        }

        public List<SExpr> GetElements()
        {
            var list = new List<SExpr>(Elements);
            return list;
        }
    }
}
