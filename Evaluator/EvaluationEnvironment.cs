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
        //private static StandardEnvironment BuiltInEnv = new StandardEnvironment();
        // static bool IsInitialized = false;

        private Dictionary<string, SExpr> dict;
        private EvaluationEnvironment parent = null;


        /*private static void InitializeRootEnvironment()
        {
            //заполнить rootEnvironent?
            Dictionary<string, object> lambdaDictionary;

            IsInitialized = true;
        }
        */
        public EvaluationEnvironment()
        {

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

        public SExpr this[string index]
        {
            get => Get(index);
        }
    }
}
