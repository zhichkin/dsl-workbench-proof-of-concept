using System.Collections.Generic;
using System.Reflection;

namespace OneCSharp.AST.Model
{
    public sealed class ScriptConcept : SyntaxNode
    {
        public List<LanguageConcept> Languages { get; } = new List<LanguageConcept>();
        public List<SyntaxRoot> Statements { get; } = new List<SyntaxRoot>();
    }
    public sealed class LanguageConcept : SyntaxNode
    {
        private const string PLACEHOLDER = "<language assembly>";
        public Assembly Assembly { get; set; }
        public override string ToString()
        {
            return (Assembly == null ? PLACEHOLDER : Assembly.ToString());
        }
    }
}