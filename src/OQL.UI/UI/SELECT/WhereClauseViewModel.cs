using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class WhereClauseViewModel : SyntaxNodeListViewModel
    {
        public WhereClauseViewModel(SelectStatementViewModel parent, WhereClauseSyntaxNode model) : base(parent, model) { }
    }
}
