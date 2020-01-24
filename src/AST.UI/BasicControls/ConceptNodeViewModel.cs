using OneCSharp.Core.Model;

namespace OneCSharp.AST.UI
{
    public sealed class ConceptNodeViewModel : SyntaxNodeViewModel
    {
        public ConceptNodeViewModel(ISyntaxNodeViewModel owner, Entity model) : base(owner, model) { }
    }
}