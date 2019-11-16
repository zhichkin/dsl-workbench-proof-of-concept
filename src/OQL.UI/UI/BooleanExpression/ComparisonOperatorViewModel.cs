using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class ComparisonOperatorViewModel : SyntaxNodeViewModel
    {
        private readonly ComparisonOperator _model;
        private SyntaxNodeViewModel _LeftExpression;
        private SyntaxNodeViewModel _RightExpression;
        public ComparisonOperatorViewModel(SyntaxNodeViewModel parent, ComparisonOperator model)
        {
            _model = model;
            Parent = parent;
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            //if (_model.LeftExpression is PropertyReference)
            //{
            //    //Expression = new TableObjectViewModel((TableObject)_model.Expression) { Parent = this };
            //}
            //else if (_model.RightExpression is ParameterExpression)
            //{
            //    //Expression = new HintSyntaxNodeViewModel((HintSyntaxNode)_model.Expression) { Parent = this };
            //}
            if (LeftExpression != null) LeftExpression.InitializeViewModel();
            if (RightExpression != null) RightExpression.InitializeViewModel();
        }
        public ISyntaxNode Model { get { return _model; } }
        public string Literal { get { return _model.Literal; } }
        public SyntaxNodeViewModel LeftExpression
        {
            get { return _LeftExpression; }
            set { _LeftExpression = value; OnPropertyChanged(nameof(LeftExpression)); }
        }
        public SyntaxNodeViewModel RightExpression
        {
            get { return _RightExpression; }
            set { _RightExpression = value; OnPropertyChanged(nameof(RightExpression)); }
        }
    }
}
