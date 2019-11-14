using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class JoinOperatorViewModel : SyntaxNodeViewModel
    {
        private readonly JoinOperator _model;
        public JoinOperatorViewModel(JoinOperator model)
        {
            _model = model;
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            _model.JoinType = _model.JoinType ?? JoinTypes.LeftJoin;

            if (_model.Expression is TableObject)
            {
                Expression = new TableObjectViewModel((TableObject)_model.Expression) { Parent = this };
            }
            else if (_model.Expression is AliasExpression)
            {
                Expression = new AliasExpressionViewModel((AliasExpression)_model.Expression) { Parent = this };
            }
            else if (_model.Expression is HintExpression)
            {
                Expression = new HintExpressionViewModel((HintExpression)_model.Expression) { Parent = this };
            }
            Expression.InitializeViewModel();
        }
        public ISyntaxNode Model { get { return _model; } }
        public string JoinType
        {
            get { return _model.JoinType; }
            set { _model.JoinType = value; OnPropertyChanged(nameof(JoinType)); }
        }
        public SyntaxNodeViewModel Expression { get; set; }
    }
}
