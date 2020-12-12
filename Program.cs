﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LispMachine
{
    class LispMachine
    {
        static void Main(string[] args)
        {
            string testString = @"  

                (+ (+ -10 20) (+ 10 23))
                (if true 10 100)
                (define x 5)
                ((lambda (a) (+ a a)) 3)
                (let (y 4 z (+ y 1)) (+ x z))
                (let (y 1) (let (z (+ y 1)) (+ z 1)))

                (let (y 1) (define z (+ y x)))
                (LispMachine.StandardLibrary\And false true)

                (define apply (lambda (fn x y) (fn x y)))
                (apply * 5 (apply * 2 2))
                (define arithm (lambda (x) (if (> x 0) (+ x (arithm (- x 1))) 0)))

                (define addA (lambda (a) (lambda (y) (+ a y))))
                (define dec (addA -1)) 
                
";
            Console.WriteLine("-----PARSER TEST-----");
            SExprParser parser = new SExprParser(new StringReader(testString));
            SExpr expr;

            while ((expr = parser.GetSExpression()) != null) {
                //Console.WriteLine($"Expression {expr.GetText()}:");
                var evald = Evaluator.Evaluate(expr);
                if(evald != null)
                {
                    if(evald != null)
                        Console.WriteLine("Evaluated: " + evald.GetText());
                }
            };




            bool isREPL = !(args.Length > 0 && args[0] == "-c");
            if (isREPL)
                StartREPL();
            
            else    //todo: only net framework, not works in Core
            {
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                string fileName = "Out.exe"; //TODO: parse name from args

                System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();

                parameters.GenerateExecutable = true;
                parameters.OutputAssembly = fileName;
                parameters.ReferencedAssemblies.Add(Assembly.GetEntryAssembly().Location);

                string programText = @"
                using System;
            
                using LispMachine;

                class CompiledProgram
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
                    Console.WriteLine("There were errors:");

                    foreach (CompilerError CompErr in results.Errors)
                    {
                        string errorText = "Line number " + CompErr.Line +
                                    ", Error Number: " + CompErr.ErrorNumber +
                                    ", '" + CompErr.ErrorText + ";" +
                                    Environment.NewLine + Environment.NewLine;
                        Console.WriteLine(errorText);

                    }
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


        private static void StartREPL()
        {
            string line;
            StringBuilder builder = new StringBuilder();
            while ((line = Console.ReadLine()) != "exit")
            {                
                if(line != "")
                {
                    builder.Append(line);
                    continue;
                }
                string lineToParse = builder.ToString();
                builder.Clear();

                SExprParser replParser;
                try 
                    { replParser = new SExprParser(new StringReader(lineToParse)); }
                catch (LexerException) 
                    { continue; }

                SExpr replExpr;

                try
                {
                    while ((replExpr = replParser.GetSExpression()) != null) {
                        var evaluated = Evaluator.Evaluate(replExpr);
                        if(evaluated != null)
                            Console.WriteLine("Evaluated: " + evaluated.GetText());
                        else
                            Console.WriteLine("Can't evaluate (yet)");            
                    }
                }
                catch (ParserException e)
                {
                    //Console.WriteLine($"Can't parse: {e}");
                    Console.WriteLine($"Can't parse: {e.Message}");
                }
                catch (EvaluationException e)
                {
                    //Console.WriteLine($"Can't evaluate: {e}");
                    Console.WriteLine($"Can't evaluate: {e.Message}");
                }
                catch (Exception e)
                {
                    //Console.WriteLine($"Can't evaluate: {e}");
                    Console.WriteLine($"Some other error: {e.Message}");
                }
            }
        }


//                (define addA (lambda (a) (lambda (y) (+ a y)))) 


    }

}
