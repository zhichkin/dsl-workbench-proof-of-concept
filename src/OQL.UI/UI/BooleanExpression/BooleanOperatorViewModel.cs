using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class BooleanOperatorViewModel : SyntaxNodeViewModel
    {
        private readonly BooleanOperator _model;
        public BooleanOperatorViewModel(SyntaxNodeViewModel parent, BooleanOperator model)
        {
            _model = model;
            Parent = parent;
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            Operands = new SyntaxNodeListViewModel(this);

            foreach (var operand in _model.Operands)
            {
                if (operand is BooleanOperator)
                {
                    Operands.Add(new BooleanOperatorViewModel(this, (BooleanOperator)operand));
                }
                else if (operand is ComparisonOperator)
                {
                    Operands.Add(new ComparisonOperatorViewModel(this, (ComparisonOperator)operand));
                }
            }
        }
        public ISyntaxNode Model { get { return _model; } }
        public string Keyword
        {
            get { return _model.Keyword; }
            set { _model.Keyword = value; OnPropertyChanged(nameof(Keyword)); }
        }
        public SyntaxNodeListViewModel Operands { get; set; }
    }
}
