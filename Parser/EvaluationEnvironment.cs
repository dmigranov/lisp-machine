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


        private EvaluationEnvironment()
        {
            if(!IsInitialized)
            {
                //заполнить rootEnvironent?
                IsInitialized = true;
            }
        }
    }
}
