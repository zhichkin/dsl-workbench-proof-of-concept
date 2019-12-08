using OneCSharp.DSL.Model;
using OneCSharp.DSL.UI.Services;

namespace OneCSharp.DSL.UI
{
    public sealed class HintSyntaxNodeViewModel : SyntaxNodeViewModel
    {
        public HintSyntaxNodeViewModel(ISyntaxNodeViewModel parent, HintSyntaxNode model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            HintSyntaxNode model = Model as HintSyntaxNode;
            model.HintType = model.HintType ?? HintTypes.ReadCommited;

            if (model.Expression is TableObject)
            {
                Expression = new TableObjectViewModel(this, (TableObject)model.Expression);
            }
            Expression.InitializeViewModel();
        }
        public string Keyword { get { return ((HintSyntaxNode)Model).Keyword; } }
        public string HintType
        {
            get { return ((HintSyntaxNode)Model).HintType; }
            set { ((HintSyntaxNode)Model).HintType = value; OnPropertyChanged(nameof(HintType)); }
        }
        public SyntaxNodeViewModel Expression { get; set; }


        public void SelectHintType()
        {
            UIServices.OpenHintTypeSelectionPopup(HintTypeSelected);
        }
        private void HintTypeSelected(string hintType)
        {
            if (string.IsNullOrWhiteSpace(hintType)) return;
            HintType = hintType;
        }
    }
}
