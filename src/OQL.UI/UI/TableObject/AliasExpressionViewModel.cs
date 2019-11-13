using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class AliasExpressionViewModel : SyntaxNodeViewModel
    {
        private readonly AliasExpression _model;
        public AliasExpressionViewModel(AliasExpression model)
        {
            _model = model;
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            if (_model.Expression is TableObject)
            {
                Expression = new TableObjectViewModel((TableObject)_model.Expression) { Parent = this };
            }
            Expression.InitializeViewModel();
        }
        public ISyntaxNode Model { get { return _model; } }
        public string Alias
        {
            get { return _model.Alias; }
            set { _model.Alias = value; OnPropertyChanged(nameof(Alias)); }
        }
        public SyntaxNodeViewModel Expression { get; set; }
    }
}
