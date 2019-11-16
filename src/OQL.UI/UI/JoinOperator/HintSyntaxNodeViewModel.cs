using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Services;

namespace OneCSharp.OQL.UI
{
    public sealed class HintSyntaxNodeViewModel : SyntaxNodeViewModel
    {
        private readonly HintSyntaxNode _model;
        public HintSyntaxNodeViewModel(HintSyntaxNode model)
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
        public string Keyword { get { return _model.Keyword; } }
        public string HintType
        {
            get { return _model.HintType; }
            set { _model.HintType = value; OnPropertyChanged(nameof(HintType)); }
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
