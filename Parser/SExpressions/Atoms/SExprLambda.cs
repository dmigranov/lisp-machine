using System;


namespace LispMachine
{
    public class SExprLambda : SExprAtom   //от кого он должен наслеоваться?
    {
        private List<SExprSymbol> LambdaArguments;
        private List<SExpr> Body; //тело может состоять из нескольких выражений, возвращаем последнее


        public SExprLambda(List<SExprSymbol> lambdaArgs, List<SExpr> body)
        {
            LambdaArguments = lambdaArgs;
            Body = body;
        }
    }
}
