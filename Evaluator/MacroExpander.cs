using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class MacroExpander
    {
        private static Dictionary<string, List<SExpr>> MacroTable = new Dictionary<string, List<SExpr>>();


        private List<SExpr> Get(string symbol)
        {            
            List<SExpr> ret;
            MacroTable.TryGetValue(symbol, out ret);
            return ret;
        }

        private void Set(string symbol, List<SExpr> value)
        {
            MacroTable[symbol] = value;
        }

        public List<SExpr> this[string index]
        {
            get => Get(index);
            set => Set(index, value);
        }
    }
}