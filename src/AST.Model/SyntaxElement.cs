using System;
using System.Collections.Generic;
using System.Text;

namespace OneCSharp.AST.Model
{
    public interface ISyntaxElement : IReferenceType
    {
        public ILanguageNamespace Namespace { get; set; }
        public List<ISemanticConnection> Semantics { get; }
    }
    public sealed class SyntaxElement : ISyntaxElement
    {
        public string Name { get; set; }
        public IReferenceType BaseType { get; set; }
        public bool IsRootElement { get; set; } // infer from BaseType property ?
        public ILanguageNamespace Namespace { get; set; }
        public List<ISemanticConnection> Semantics { get; } = new List<ISemanticConnection>();
    }
}
