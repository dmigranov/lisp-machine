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
            //todo: сначала обращаемся непосредственно к словарю, нашли - возвращаем
            //иначе проверка на наследование: итерируемся по всем и проверяем, не наследуется ли

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
