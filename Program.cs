using Microsoft.CSharp;
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
            /*
            string testString = @"  

                              
            ";

            Console.WriteLine("-----PARSER TEST-----");
            SExprParser parser = new SExprParser(new StringReader(testString));
            SExpr expr;

            while ((expr = parser.GetSExpression()) != null) {
                var evald = Evaluator.Evaluate(expr);
                if(evald != null)
                {
                    if(evald != null)
                        Console.WriteLine("Evaluated: " + evald.GetText());
                }
            };
            */



            int mode = 0; //0 - REPL, 1 - from file, 2 - compile?..
            if(args.Length > 0)
            {
                if(args[0] == "-f")
                    mode = 1;
                else if(args[0] == "-c")
                    mode = 2;
                else
                    Console.Error.WriteLine("Wrong first argument, running in an interactive REPL mode");
            }

            if (mode == 0)
                StartREPL();
                //ParseAndPrintFromReader(new StreamReader(Console.OpenStandardInput()));

            else if (mode == 1)
            {
                if(args.Length < 2)
                    throw new ArgumentException("Wrong argument count, second argument should be a file name/location");
                string fileName = args[1];
                StreamReader fileReader = new StreamReader(fileName);

                ParseAndPrintFromReader(fileReader);
            }
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


        private static void ParseAndPrintFromReader(TextReader reader)
        {
            SExprParser replParser;
            try 
                { replParser = new SExprParser(reader); }
            catch (LexerException) 
                { return; }

            SExpr replExpr;

            try
            {
                SExpr evaluated = null;
                while ((replExpr = replParser.GetSExpression()) != null) {
                    evaluated = Evaluator.Evaluate(replExpr);  
                }
                //оцениваем только последнее выражение из серии, если надо все - внести в цикл
                if(evaluated != null)
                    Console.WriteLine("Evaluated: " + evaluated.GetText());
                else
                    Console.WriteLine("null");

            }
            catch (LexerException e)
            {
                Console.WriteLine($"Lexer error: {e.Message}");
            }
            catch (ParserException e)
            {
                Console.WriteLine($"Can't parse: {e.Message}");
            }
            catch (MacroException e)
            {
                Console.WriteLine($"Macro expansion error: {e.Message}");
            }
            catch (EvaluationException e)
            {
                Console.WriteLine($"Can't evaluate: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Some other error: {e}");
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

                ParseAndPrintFromReader(new StringReader(lineToParse));
            }
        }
    }

}
