using Microsoft.VisualStudio.PlatformUI;
using OneCSharp.OQL.Model;
using System.Windows;
using System.Windows.Input;

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
            this.SELECT = new SelectClauseViewModel(this, model.SELECT);

            this.AddIntoClauseCommand = new DelegateCommand(AddIntoClause);
            this.AddPropertyCommand = new DelegateCommand(SELECT.AddProperty);
            this.AddWhereClauseCommand = new DelegateCommand(AddWhereClause);
            this.AddHavingClauseCommand = new DelegateCommand(AddHavingClause);
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

        public ICommand AddPropertyCommand { get; private set; }
        public ICommand AddIntoClauseCommand { get; private set; }
        public ICommand AddWhereClauseCommand { get; private set; }
        public ICommand AddHavingClauseCommand { get; private set; }
        public bool IsWhereClauseVisible { get { return (WHERE != null); } }
        private void AddWhereClause()
        {
            SelectStatement model = (SelectStatement)Model;
            if (model.WHERE != null)
            {
                MessageBox.Show("WHERE clause is already present!", "ONE-C-SHARP");
                return;
            }
            model.WHERE = new WhereSyntaxNode(model);
            WHERE = new WhereClauseViewModel(this, model.WHERE);
            OnPropertyChanged(nameof(IsWhereClauseVisible));
            OnPropertyChanged(nameof(WHERE));
        }
        private void AddIntoClause()
        {
            MessageBox.Show("Sorry, under construction =)", "ONE-C-SHARP");
        }
        private void AddHavingClause()
        {
            MessageBox.Show("Sorry, under construction =)", "ONE-C-SHARP");
        }
    }
}
