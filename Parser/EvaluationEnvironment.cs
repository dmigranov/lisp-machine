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
        private static BuiltInEnvironment BuiltInEnv = new BuiltInEnvironment();
        private static bool IsInitialized = false;

        //хорошо бы тут иметь два типа констант:
        //одни - лисповские
        //другие - внешние

        private static void InitializeRootEnvironment()
        {
            //заполнить rootEnvironent?
            Dictionary<string, object> lambdaDictionary;

            IsInitialized = true;
        }

        private Dictionary<string, SExpr> dict; //или не SExpr, а Expression?
        private EvaluationEnvironment parent = null;

        public EvaluationEnvironment()
        {
            if(!IsInitialized)
            {
                InitializeRootEnvironment();
            }
        }

        //конструктор только для Глобала
        private EvaluationEnvironment(Dictionary<string, SExpr> currentScope)
        {
            dict = currentScope;
        }

        public EvaluationEnvironment(Dictionary<string, SExpr> currentScope, EvaluationEnvironment parent)
        {
            //todo
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
