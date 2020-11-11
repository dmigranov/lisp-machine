using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;

class LispMachine
{ 
    static void Main(string[] args)
    {
        bool isREPL = true; //TODO: parse args: REPL or build

        if (isREPL)
        {
            for(; ; )
            {

            }
        }
        else
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            string fileName = "Out.exe"; //TODO: parse name from args

            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
        }
        Console.WriteLine("Hello World!");
    }
}
