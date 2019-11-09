using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class FromClauseViewModel : SyntaxNodesViewModel
    {
        private readonly FromClauseSyntaxNode _model;
        public FromClauseViewModel(SelectStatementViewModel parent)
        {
            Parent = parent;
            _model = ((SelectStatement)parent.Model).FROM;
            InitializeViewModel();
        }
        public ISyntaxNode Model { get { return _model; } }
        private void InitializeViewModel()
        {
            foreach (var table in _model)
            {
                //TODO: abstract TableViewModel
            }
        }
        public string Keyword { get { return _model.Keyword; } }
    }
}
