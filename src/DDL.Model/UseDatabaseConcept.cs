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
            return (Assembly == null ? PLACEHOLDER : Assembly.ToString());
        }
    }
}