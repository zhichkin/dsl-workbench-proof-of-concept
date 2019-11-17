using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class AliasSyntaxNodeViewModel : SyntaxNodeViewModel
    {
        public AliasSyntaxNodeViewModel(ISyntaxNodeViewModel parent, AliasSyntaxNode model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            AliasSyntaxNode model = Model as AliasSyntaxNode;
            if (model.Expression is TableObject)
            {
                Expression = new TableObjectViewModel(this, (TableObject)model.Expression);
            }
            else if (model.Expression is HintSyntaxNode)
            {
                Expression = new HintSyntaxNodeViewModel(this, (HintSyntaxNode)model.Expression);
            }
            Expression.InitializeViewModel();
        }
        public string Alias
        {
            get { return ((AliasSyntaxNode)Model).Alias; }
            set { ((AliasSyntaxNode)Model).Alias = value; OnPropertyChanged(nameof(Alias)); }
        }
        public SyntaxNodeViewModel Expression { get; set; }
    }
}
