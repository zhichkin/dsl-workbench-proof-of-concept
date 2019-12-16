using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public interface ISemanticConnection
    {
        public ISyntaxElement Owner { get; set; }
        public string Name { get; set; }
        bool IsOptional { get; set; }
        bool IsOneToMany { get; set; }
        public List<IType> Targets { get; }
    }
    public sealed class SemanticConnection : ISemanticConnection
    {
        public ISyntaxElement Owner { get; set; }
        public string Name { get; set; }
        public bool IsOptional { get; set; } // [0...1] | [0...n]
        public bool IsOneToMany { get; set; } // [1] | [1...n]
        public List<IType> Targets { get; } = new List<IType>();
    }
}