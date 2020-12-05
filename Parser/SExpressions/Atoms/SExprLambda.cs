using System;


namespace LispMachine
{
    class SExprLambda : SExprAtom   //от кого он должен наслеоваться?
    {
        private List<SExprSymbol> LambdaArguments;
        private List<SExpr> Body; //тело может состоять из нескольких выражений, возвращаем последнее

    }
}
