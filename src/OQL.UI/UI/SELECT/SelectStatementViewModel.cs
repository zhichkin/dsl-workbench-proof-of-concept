using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class SelectStatementViewModel : SyntaxNodeViewModel
    {
        private const string ALIAS_PLACEHOLDER = "<alias>";
        public SelectStatementViewModel(ISyntaxNodeViewModel parent, SelectStatement model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            SelectStatement model = Model as SelectStatement;
            this.FROM = new FromClauseViewModel(this, model.FROM);
            this.WHERE = new WhereClauseViewModel(this, model.WHERE);
            this.SELECT = new SelectClauseViewModel(this, model.SELECT);
        }
        public string Keyword { get { return ((SelectStatement)Model).Keyword; } }
        public string Alias
        {
            get { return string.IsNullOrEmpty(((SelectStatement)Model).Alias) ? ALIAS_PLACEHOLDER : ((SelectStatement)Model).Alias; }
            set { ((SelectStatement)Model).Alias = value; OnPropertyChanged(nameof(Alias)); }
        }
        public FromClauseViewModel FROM { get; private set; }
        public WhereClauseViewModel WHERE { get; private set; }
        public SelectClauseViewModel SELECT { get; private set; }
    }
}
