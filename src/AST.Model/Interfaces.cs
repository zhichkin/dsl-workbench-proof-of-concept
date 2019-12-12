using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public interface ISyntaxElement : IType
    {
        public ILanguageNamespace Namespace { get; set; }
        public List<ISemanticConnection> Semantics { get; }
    }
    public interface ISemanticConnection
    {
        public ISyntaxElement Owner { get; set; }
        public string Name { get; set; }
        bool IsOneToMany { get; set; }
        public List<IType> Targets { get; }
        bool IsOptional { get; set; }
        bool IsXOR { get; set; }
    }
    public interface ICodeBlock : IType // ??? target of the semantic connection, ex. if operator's else body
    {
        public List<ISyntaxElement> Elements { get; }
    }
}