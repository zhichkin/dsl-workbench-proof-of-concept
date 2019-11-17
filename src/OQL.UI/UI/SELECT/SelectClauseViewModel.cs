using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class SelectClauseViewModel : SyntaxNodeListViewModel
    {
        public SelectClauseViewModel(SelectStatementViewModel parent, SelectClauseSyntaxNode model) : base(parent, model) { }
    }
}
