using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;

class LispMachine
{ 
    static void Main(string[] args)
    {
        bool isREPL = false; //TODO: parse args: REPL or build

        if (isREPL)
        {
            for(; ; )
            {
                break;
            }
        }
        else
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            string fileName = "Out.exe"; //TODO: parse name from args

            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();

            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = fileName;

            string programText = @"
                using System;

                class HelloWorldClass
                {
                    static void Main(string[] args)
                    {
                        //TODO: parse и eval текст файла
                        Console.WriteLine(""Test Test Test"");
                    }
                }";

            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, programText);

            if (results.Errors.Count > 0)
            {
                Console.WriteLine("There were errors");
                //https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/compile-code-using-compiler
            }

            else
                Console.WriteLine("Successfully compiled!");
        }

        
    }
}
