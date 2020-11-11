
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;
using System.Reflection;

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
            //CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            string fileName = "Out"; //TODO: parse name from args

            //System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();

            //parameters.GenerateExecutable = true;
            //parameters.OutputAssembly = fileName;

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

            /*CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, programText);

            if (results.Errors.Count > 0)
            {
                Console.WriteLine("There were errors");
                //https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/compile-code-using-compiler
            }

            else
                Console.WriteLine("Successfully compiled!");*/




            var references = MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location);

            Console.WriteLine(references.Display);

            var compilation = CSharpCompilation.Create(fileName)
                                .WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication))
                                .AddReferences(references)
                                .AddSyntaxTrees(CSharpSyntaxTree.ParseText(programText));
            var compilationResult = compilation.Emit(fileName + ".exe");

            if (compilationResult.Success)
            {
                Console.WriteLine("Successfully compiled!");
            }
            else
            {
                Console.Write(string.Join(
                    Environment.NewLine,
                    compilationResult.Diagnostics.Select(diagnostic => diagnostic.ToString())
                ));
            }

        }

        
    }
}
