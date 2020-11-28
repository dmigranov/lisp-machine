using System;
using System.Collections.Generic;


namespace LispMachine
{
    class StandardEnvironment
    {
        //содержит встроенные функции, которые не являются лисповскими
        //лямбды
        private Dictionary<string, object> dict;

        public StandardEnvironment()
        {
            Func<IComparable, IComparable, object> func = (x, y) => x > y;
            dict = new Dictionary<string, object>
            {
                {">",  }
            };
        }


        public SExpr this[string index]
        {
            get => null; //todo
        }
    }
}
