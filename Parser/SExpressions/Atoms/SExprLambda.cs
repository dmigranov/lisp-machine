using System;
using System.Collections.Generic;
using System.Linq;

namespace LispMachine
{
    public class SExprLambda : SExprAtom   //от кого он должен наслеоваться?
    {
        public List<SExprSymbol> LambdaArguments { get; }
        public List<SExpr> Body { get; } //тело может состоять из нескольких выражений, возвращаем последнее
        //но исполнить надо все (вдруг там принтлны или дефайны)

        public SExprLambda(List<SExprSymbol> lambdaArgs, List<SExpr> body)
        {
            LambdaArguments = lambdaArgs;
            Body = body;

            /*Console.WriteLine("Lambda body:");
            foreach (var elem in Body)
                elem.PrintSExpr();*/
        }

        public override string GetText()
        {
            return $"Lambda of arguments {String.Join(", ", LambdaArguments.Select(x => x.GetText()))}";
        }
    }
}
