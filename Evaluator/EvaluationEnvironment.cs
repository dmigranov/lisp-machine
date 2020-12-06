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

        private Dictionary<string, SExpr> EnvDictionary;
        private EvaluationEnvironment Parent = null;

        public EvaluationEnvironment()
        {
            EnvDictionary = new Dictionary<string, SExpr>();
        }



        public EvaluationEnvironment(EvaluationEnvironment parent)
        {
            EnvDictionary = new Dictionary<string, SExpr>();
            Parent = parent;
        }

        public SExpr Get(string symbol)
        {            
            SExpr ret = EnvDictionary[symbol];
            if(ret == null)
                if(Parent != null)
                    ret = Parent[symbol];

            return ret;
        }

        public SExpr this[string index]
        {
            get => Get(index);
        }
    }
}
