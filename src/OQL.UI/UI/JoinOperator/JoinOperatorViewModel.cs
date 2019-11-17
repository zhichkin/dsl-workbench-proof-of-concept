using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Services;

namespace OneCSharp.OQL.UI
{
    public sealed class JoinOperatorViewModel : SyntaxNodeViewModel
    {
        public JoinOperatorViewModel(ISyntaxNodeViewModel parent, JoinOperator model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            JoinOperator model = Model as JoinOperator;
            model.JoinType = model.JoinType ?? JoinTypes.Left;

            if (model.Expression is TableObject)
            {
                Expression = new TableObjectViewModel(this, (TableObject)model.Expression);
            }
            else if (model.Expression is AliasSyntaxNode)
            {
                Expression = new AliasSyntaxNodeViewModel(this, (AliasSyntaxNode)model.Expression);
            }
            else if (model.Expression is HintSyntaxNode)
            {
                Expression = new HintSyntaxNodeViewModel(this, (HintSyntaxNode)model.Expression);
            }

            OnExpression = new OnSyntaxNodeViewModel(this, model.ON);
        }
        public string Keyword
        {
            get { return $"{((JoinOperator)Model).JoinType} {((JoinOperator)Model).Keyword}"; }
            set { ((JoinOperator)Model).JoinType = value; OnPropertyChanged(nameof(Keyword)); }
        }
        public SyntaxNodeViewModel Expression { get; set; }
        public SyntaxNodeViewModel OnExpression { get; set; }


        public void SelectJoinType()
        {
            UIServices.OpenJoinTypeSelectionPopup(JoinTypeSelected);
        }
        private void JoinTypeSelected(string joinType)
        {
            if (string.IsNullOrWhiteSpace(joinType)) return;
            Keyword = joinType;
        }
    }
}
