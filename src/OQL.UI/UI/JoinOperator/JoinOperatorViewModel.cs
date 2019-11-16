using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Services;
using System;

namespace OneCSharp.OQL.UI
{
    public sealed class JoinOperatorViewModel : SyntaxNodeViewModel
    {
        private readonly JoinOperator _model;
        public JoinOperatorViewModel(JoinOperator model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            _model.JoinType = _model.JoinType ?? JoinTypes.Left;

            if (_model.Expression is TableObject)
            {
                Expression = new TableObjectViewModel((TableObject)_model.Expression) { Parent = this };
            }
            else if (_model.Expression is AliasSyntaxNode)
            {
                Expression = new AliasSyntaxNodeViewModel((AliasSyntaxNode)_model.Expression) { Parent = this };
            }
            else if (_model.Expression is HintSyntaxNode)
            {
                Expression = new HintSyntaxNodeViewModel((HintSyntaxNode)_model.Expression) { Parent = this };
            }

            OnExpression = new OnSyntaxNodeViewModel(_model.ON);
        }
        public ISyntaxNode Model { get { return _model; } }
        public string Keyword
        {
            get { return $"{_model.JoinType} {_model.Keyword}"; }
            set { _model.JoinType = value; OnPropertyChanged(nameof(Keyword)); }
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
