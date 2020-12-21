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

                    Macro macro;
                    if((macro = MacroTable[value]) != null)
                        return ExpandMacro(macro, args);
                }       

                return expr;
            }
            else
            {
                return expr;
            }
        }

        public SExpr ExpandMacro(Macro macro, List<SExpr> args)
        {
            var expr = macro.Body;
            var expanded = ExpandExprRec(expr, macro.MacroArguments, args);
            return expanded;
        }

        private SExpr ExpandExprRec(SExpr expr, List<SExprSymbol> argNames, List<SExpr> macroArgs)
        {
            if(argNames.Count != macroArgs.Count)
                throw new MacroException($"Wrong parameter count: {macroArgs.Count} instead of {argNames.Count}");

            if (expr is SExprList list)     
            {
                list = new SExprList(list.GetElements());
                var args = list.GetArgs();
                var head = list[0];

                if (head is SExprSymbol listHeadSymbol)
                {
                    var value = listHeadSymbol.Value;
                    for (int i = 1; i < list.GetElements().Count; i++)
                    {
                        var bodyExpr = list[i];
                        var expanded = ExpandExprRec(bodyExpr, argNames, macroArgs);
                        list[i] = expanded;
                    }
                    return list;


                    /*if(value == "let") //нужно обработать особенно 
                    {
                        if (args[0] is SExprList letBindings)
                        {
                            var letBindingsList = letBindings.GetElements();
                            if(letBindingsList.Count % 2 != 0)
                                throw new MacroException("There should be an even number of elements in list of bindings");

                            for (int i = 0; i < letBindingsList.Count; i+=2)
                            {
                                var symbolIndex = i;
                                var valueIndex = i + 1;
                                letBindingsList[valueIndex] = ExpandExprRec(letBindingsList[valueIndex], argNames, macroArgs);
                            }
                            list[1] = new SExprList(letBindingsList);

                            SExpr ret = null;

                            if(args.Count == 1)
                                return new SExprObject(null);

                            for (int i = 2; i < list.GetElements().Count; i++)
                            {
                                var bodyExpr = list[i];
                                var expanded = ExpandExprRec(bodyExpr, argNames, macroArgs);
                                list[i] = expanded;
                            }

                            return list;
                        }
                        else
                            throw new MacroException("Second argument of let should be a list of bindings");
                    }
                    //todo

                    else 
                    {
                        for (int i = 1; i < list.GetElements().Count; i++)
                        {
                            var bodyExpr = list[i];
                            var expanded = ExpandExprRec(bodyExpr, argNames, macroArgs);
                            list[i] = expanded;
                        }
                        return list;
                    }
                    */
                }       

                return expr;
            }
            else if (expr is SExprSymbol symbol)
            {
                var value = symbol.Value;
                for (int i = 0; i < argNames.Count; i++)
                {
                    var argName = argNames[i];
                    if(argName.Value == value)
                        return macroArgs[i];
                }
                return expr;
            }

            return expr;
        }
    }

    public class MacroException : Exception
    {
        public MacroException() { }

        public MacroException(string message) : base(message) { }

        public MacroException(string message, Exception innerException) : base(message, innerException) { }
    }

}