using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispMachine
{
    
    public class ExceptionDictionary 
    {
        private Dictionary<Type, List<SExpr>> Dict = new Dictionary<Type, List<SExpr>>();
    
        private List<SExpr> Get(Type type)
        {            
            List<SExpr> ret;
            //todo: проверка на наследование

            return null;
        }

        private void Set(Type type, List<SExpr> value)
        {
            Dict[type] = value;
        }
    

        public List<SExpr> this[Type index]
        {
            get => Get(index);
            set => Set(index, value);
        }
    }


    
}
