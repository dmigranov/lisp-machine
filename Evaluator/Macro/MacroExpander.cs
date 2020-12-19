using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class MacroExpander
    {
        private static Dictionary<string, Macro> MacroTable = new Dictionary<string, Macro>();


        private Macro Get(string symbol)
        {            
            Macro ret;
            MacroTable.TryGetValue(symbol, out ret);
            return ret;
        }

        private void Set(string symbol, Macro value)
        {
            MacroTable[symbol] = value;
        }

        public Macro this[string index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public SExpr Expand(SExpr expr)
        {
            if (expr is SExprList list)     
            {
                var args = list.GetArgs();

                var head = list[0];

                if (head is SExprSymbol listHeadSymbol)
                {
                    var value = listHeadSymbol.Value;

                    if (value == "if")
                    {

                    }

                    if(MacroTable[value] != null)
                    {
                        Console.WriteLine("Here!");
                        

                        //но это неправильноЁ надо рекурсивно...
                    }
                }       

                return null;
            }
            else
            {
                return expr;
            }
        }
    }
}