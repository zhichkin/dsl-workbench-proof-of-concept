using OneCSharp.OQL.Model;
using System.Windows;

namespace OneCSharp.OQL.UI
{
    public sealed class OnSyntaxNodeViewModel : SyntaxNodeViewModel
    {
        private readonly OnSyntaxNode _model;
        private SyntaxNodeViewModel _Expression;
        public OnSyntaxNodeViewModel(OnSyntaxNode model)
        {
            _model = model;
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            if (_model.Expression is BooleanOperator)
            {
                //Expression = new TableObjectViewModel((TableObject)_model.Expression) { Parent = this };
            }
            else if (_model.Expression is ComparisonOperator)
            {
                //Expression = new HintSyntaxNodeViewModel((HintSyntaxNode)_model.Expression) { Parent = this };
            }
            //if (Expression != null)
            //{
            //    Expression.InitializeViewModel(); ???
            //}
        }
        public ISyntaxNode Model { get { return _model; } }
        public string Keyword { get { return _model.Keyword; } }
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
                _model.Expression = expression;
                Expression = new ComparisonOperatorViewModel(this, expression);
            }
            else if (Expression is ComparisonOperatorViewModel currentVM)
            {
                BooleanOperator substitute = new BooleanOperator(_model);
                _model.Expression = substitute;
                substitute.AddChild(currentVM.Model);
                
                ComparisonOperator child = new ComparisonOperator(substitute);
                substitute.AddChild(child);

                Expression = new BooleanOperatorViewModel(this, substitute);
            }
        }
    }
}
