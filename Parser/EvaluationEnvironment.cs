using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispMachine
{
    /// <summary>
    /// Environment === Context of Evaluation.
    /// Изначально он не пуст, там есть встроенные функции - rootEnvironment
    /// </summary>
    public class EvaluationEnvironment
    {
        public static EvaluationEnvironment rootEnvironment;
        private static bool IsInitialized = false;

        //хорошо бы тут иметь два типа констант:
        //одни - лисповские
        //другие - внешние

        private Dictionary<string, SExpr> dict; //или не SExpr, а Expression?
        private EvaluationEnvironment parent;

        public EvaluationEnvironment()
        {
            if(!IsInitialized)
            {
                //заполнить rootEnvironent?
                IsInitialized = true;
            }
        }

        public SExpr Get(string symbol)
        {            
            //todo

            //возвращает значение из словаря
            //сначала смотрим в локальном контексте, потом обращаеся к родителю

            return null;
        }
    }
}
