using OneCSharp.Core.Model;

namespace OneCSharp.AST.UI
{
    public sealed class ConceptNode : SyntaxNode
    {
        public ConceptNode(ISyntaxNode owner, Entity model) : base(owner, model) { }
    }
}