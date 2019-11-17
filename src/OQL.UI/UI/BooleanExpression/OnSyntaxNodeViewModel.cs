using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class OnSyntaxNodeViewModel : SyntaxNodeViewModel
    {
        private SyntaxNodeViewModel _Expression;
        public OnSyntaxNodeViewModel(ISyntaxNodeViewModel parent, OnSyntaxNode model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            OnSyntaxNode _model = Model as OnSyntaxNode;
            if (_model.Expression is BooleanOperator)
            {
                Expression = new BooleanOperatorViewModel(this, (BooleanOperator)_model.Expression);
            }
            else if (_model.Expression is ComparisonOperator)
            {
                Expression = new ComparisonOperatorViewModel(this, (ComparisonOperator)_model.Expression);
            }
        }
        public string Keyword { get { return ((OnSyntaxNode)Model).Keyword; } }
        public SyntaxNodeViewModel Expression
        {
            get { return _Expression; }
            set { _Expression = value; OnPropertyChanged(nameof(Expression)); }
        }
        public void OpenContextMenu()
        {
            IsContextMenuOpened = true;
        }
        private bool _IsContextMenuOpened;
        public bool IsContextMenuOpened
        {
            get { return _IsContextMenuOpened; }
            set { _IsContextMenuOpened = value; OnPropertyChanged(nameof(IsContextMenuOpened)); }
        }
        public void AddNewCondition()
        {
            if (Expression == null)
            {
                ComparisonOperator expression = new ComparisonOperator(_model);
                ((OnSyntaxNode)Model).Expression = expression;
                Expression = new ComparisonOperatorViewModel(this, expression);
            }
            else if (Expression is ComparisonOperatorViewModel currentVM)
            {
                BooleanOperator substitute = new BooleanOperator(_model);
                ((OnSyntaxNode)Model).Expression = substitute;
                substitute.AddChild(currentVM.Model);
                
                ComparisonOperator child = new ComparisonOperator(substitute);
                substitute.AddChild(child);

                Expression = new BooleanOperatorViewModel(this, substitute);
            }
        }
    }
}
