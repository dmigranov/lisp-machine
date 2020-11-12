﻿
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
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
                        Parser.TestMethod();
                    }
                }";

            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, programText);

            if (results.Errors.Count > 0)
            {
                Console.WriteLine("There were errors");

                foreach (CompilerError CompErr in results.Errors)
                {
                    string errorText = "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                    Console.WriteLine(errorText);

                }

                //https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/compile-code-using-compiler
            }

            else
                Console.WriteLine("Successfully compiled!");




            /*var refPaths = new[] {
                typeof(object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll")
            };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

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
            }*/

        }

        
    }
}
