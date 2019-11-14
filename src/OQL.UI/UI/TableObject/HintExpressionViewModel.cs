using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class HintExpressionViewModel : SyntaxNodeViewModel
    {
        private readonly HintExpression _model;
        public HintExpressionViewModel(HintExpression model)
        {
            _model = model;
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            _model.HintType = _model.HintType ?? HintTypes.ReadCommited;

            if (_model.Expression is TableObject)
            {
                Expression = new TableObjectViewModel((TableObject)_model.Expression) { Parent = this };
            }
            Expression.InitializeViewModel();
        }
        public ISyntaxNode Model { get { return _model; } }
        public string HintType
        {
            get { return _model.HintType; }
            set { _model.HintType = value; OnPropertyChanged(nameof(HintType)); }
        }
        public SyntaxNodeViewModel Expression { get; set; }
    }
}
