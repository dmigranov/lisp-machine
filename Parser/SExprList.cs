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
    }
}
