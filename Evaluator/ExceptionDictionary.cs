using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace LispMachine
{
    

    public class ExceptionDictionary 
    {

        //private Dictionary<Type, List<SExpr>> Dict = new Dictionary<Type, List<SExpr>>();
        private OrderedDictionary Dict = new OrderedDictionary();


        private List<SExpr> Get(Type type)
        {            
            
            //todo: сначала обращаемся непосредственно к словарю, нашли - возвращаем
            //иначе проверка на наследование: итерируемся по всем и проверяем, не наследуется ли

            List<SExpr> ret = (List<SExpr>)Dict[type];
            if(ret != null)
                return ret;
            foreach (DictionaryEntry pair in Dict)
            {
                var key = (Type)pair.Key;
                var value = (List<SExpr>)pair.Value;
            }

            //проверка наслеования

            return null;
        }

        private void Set(Type type, List<SExpr> value)
        {
            Dict.Add(type, value);
        }

        public List<SExpr> this[Type index]
        {
            get => Get(index);
            set => Set(index, value);
        }
    }


    
}
