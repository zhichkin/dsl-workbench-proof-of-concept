using OneCSharp.DSL.Model;
using OneCSharp.DSL.UI.Services;
using OneCSharp.MVVM;
using System.Windows.Input;

namespace OneCSharp.DSL.UI
{
    public sealed class WhereClauseViewModel : SyntaxNodeViewModel
    {
        private ISyntaxNodeViewModel _Expression;
        public WhereClauseViewModel(ISyntaxNodeViewModel parent, WhereSyntaxNode model) : base(parent, model)
        {
            AddNewConditionCommand = new RelayCommand(AddNewCondition);
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            WhereSyntaxNode model = Model as WhereSyntaxNode;
            Expression = UIServices.CreateViewModel(this, model.Expression);
        }
        public string Keyword { get { return ((WhereSyntaxNode)Model).Keyword; } }
        public ISyntaxNodeViewModel Expression
        {
            get { return _Expression; }
            set { _Expression = value; OnPropertyChanged(nameof(Expression)); }
        }
        public ICommand AddNewConditionCommand { get; private set; }
        public void AddNewCondition(object parameter)
        {
            if (Expression == null)
            {
                ComparisonOperator expression = new ComparisonOperator(_model);
                ((WhereSyntaxNode)Model).Expression = expression;
                Expression = new ComparisonOperatorViewModel(this, expression);
            }
            else if (Expression is ComparisonOperatorViewModel currentVM)
            {
                BooleanOperator substitute = new BooleanOperator(_model);
                ((WhereSyntaxNode)Model).Expression = substitute;
                substitute.AddChild(currentVM.Model);

                ComparisonOperator child = new ComparisonOperator(substitute);
                substitute.AddChild(child);

                Expression = new BooleanOperatorViewModel(this, substitute);
            }
        }
    }
}
