using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class BooleanOperatorViewModel : SyntaxNodeViewModel
    {
        public BooleanOperatorViewModel(ISyntaxNodeViewModel parent, BooleanOperator model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            Operands = new SyntaxNodeListViewModel(this, ((BooleanOperator)Model).Operands);

            foreach (var operand in ((BooleanOperator)Model).Operands)
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
        public string Keyword
        {
            get { return ((BooleanOperator)Model).Keyword; }
            set { ((BooleanOperator)Model).Keyword = value; OnPropertyChanged(nameof(Keyword)); }
        }
        public SyntaxNodeListViewModel Operands { get; set; }
    }
}
