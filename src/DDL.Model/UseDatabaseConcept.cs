using OneCSharp.AST.Model;
using System.Reflection;

namespace OneCSharp.DDL.Model
{
    public sealed class UseDatabaseConcept : SyntaxRoot, IAssemblyConcept
    {
        private const string PLACEHOLDER = "<database assembly>";
        public Assembly Assembly { get; set; }
        public override string ToString()
        {
            if (Assembly == null) return PLACEHOLDER;
            AssemblyName asm = Assembly.GetName();
            return $"{asm.Name} ({asm.Version.ToString()})";
        }
    }
}