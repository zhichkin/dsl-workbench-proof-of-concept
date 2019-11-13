using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class SelectStatementViewModel : SyntaxNodeViewModel
    {
        private const string ALIAS_PLACEHOLDER = "<alias>";
        private readonly SelectStatement _model;
        public SelectStatementViewModel()
        {
            _model = new SelectStatement();
            InitializeViewModel();
        }
        public SelectStatementViewModel(SelectStatement model)
        {
            _model = model;
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            this.FROM = new FromClauseViewModel(this);
            this.WHERE = new WhereClauseViewModel(this);
            this.SELECT = new SelectClauseViewModel(this);
        }
        public ISyntaxNode Model { get { return _model; } }
        public string Keyword { get { return _model.Keyword; } }
        public string Alias
        {
            get { return string.IsNullOrEmpty(_model.Alias) ? ALIAS_PLACEHOLDER : _model.Alias; }
            set { _model.Alias = value; OnPropertyChanged(nameof(Alias)); }
        }
        public FromClauseViewModel FROM { get; private set; }
        public WhereClauseViewModel WHERE { get; private set; }
        public SelectClauseViewModel SELECT { get; private set; }
    }
}
