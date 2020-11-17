using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LispMachine
{
    class LispMachine
    {
        static void Main(string[] args)
        {
            string testString = @"  (defn 
                                    dsfdfsdf
                                    fdfsdfdf
                                    445345
                                    5345
                                    (ff fff ffff)

                                    5343345.54)

                            (another other)";

            Console.WriteLine("-----LEXER-----");
            Lexer lexer = new Lexer(new StringReader(testString));
            Lexeme lexeme;
            while ((lexeme = lexer.GetLexeme()).Type != LexemeType.EOF)
            {
                if(lexeme.Type == LexemeType.LBRACE)
                    Console.WriteLine('(');
                else if (lexeme.Type == LexemeType.RBRACE)
                    Console.WriteLine(')');
                if (lexeme.Type == LexemeType.SYMBOL)
                    Console.WriteLine(lexeme.Text);
            }

            Console.WriteLine("-----PARSER-----");
            SExprParser parser = new SExprParser(new StringReader(testString));
            SExpr expr;
            while ((expr = parser.GetSExpression()) != null) {
                expr.PrintSExpr();
            };




            bool isREPL = true; //TODO: parse args: REPL or build

            if (isREPL)
            {
                while (true)
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
    }

}
