using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using OneCSharp.DDL.Attributes;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace OneCSharp.CodeGenerator
{
    public sealed class OneCSharpCodeGenerator
    {
        private string filePath = @"C:\Users\User\Desktop\GitHub\TestCode.cs";
        public Assembly Generate()
        {
            //Runner runner = new Runner();
            Compiler compiler = new Compiler();
            byte[] buffer = compiler.Compile(filePath);
            return Assembly.Load(buffer);
            //runner.Execute(assembly);
        }
    }
    internal sealed class Compiler
    {
        public byte[] Compile(string filepath)
        {
            var sourceCode = File.ReadAllText(filepath);

            using (var stream = new MemoryStream())
            {
                var result = GenerateCode(sourceCode).Emit(stream);

                if (!result.Success)
                {
                    var failures = result.Diagnostics
                        .Where(diagnostic => diagnostic.IsWarningAsError
                        || diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                    return null;
                }
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ToArray();
            }
        }

        private static CSharpCompilation GenerateCode(string sourceCode)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp7_3);

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);

            //var dotNetCoreDir = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);

            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(EntityAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(RuntimeReflectionExtensions).Assembly.Location)
                //MetadataReference.CreateFromFile(Path.Combine(dotNetCoreDir, "System.Runtime.dll"))
            };

            return CSharpCompilation.Create("Hello.dll",
                new[] { parsedSyntaxTree },                
                references: references,
                options: new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release));
        }
    }
    internal class Runner
    {
        public void Execute(byte[] compiledAssembly) // , string[] args
        {
            var assemblyLoadContextWeakRef = LoadAndExecute(compiledAssembly); // , args

            for (var i = 0; i < 8 && assemblyLoadContextWeakRef.IsAlive; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static WeakReference LoadAndExecute(byte[] compiledAssembly) // , string[] args
        {
            using (var asm = new MemoryStream(compiledAssembly))
            {
                var assemblyLoadContext = new SimpleUnloadableAssemblyLoadContext();

                var assembly = assemblyLoadContext.LoadFromStream(asm);

                //var entry = assembly.EntryPoint;

                //_ = entry != null && entry.GetParameters().Length > 0
                //    ? entry.Invoke(null, new object[] { args })
                //    : entry.Invoke(null, null);

                assemblyLoadContext.Unload();

                return new WeakReference(assemblyLoadContext);
            }
        }
    }
    internal class SimpleUnloadableAssemblyLoadContext : AssemblyLoadContext
    {
        public SimpleUnloadableAssemblyLoadContext() : base(true) { }
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}