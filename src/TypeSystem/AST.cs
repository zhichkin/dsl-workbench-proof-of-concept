using OneCSharp.TypeSystem;
using System.Collections.Generic;

namespace OneCSharp.AST
{
    public interface IDomainLanguage
    {
        string Name { get; set; }
        List<INamespace> Grammar { get; }
    }
    public interface INamespace
    {
        public IDomainLanguage Domain { get; set; }
        public INamespace Parent { get; set; }
        public string Name { get; set; }
        public List<INamespace> Namespaces { get; }
        public List<ISyntaxElement> Elements { get; }
    }
    public interface ISyntaxElement : IType
    {
        public INamespace Namespace { get; set; }
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