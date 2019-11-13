using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class TableObjectViewModel : SyntaxNodeViewModel
    {
        private readonly TableObject _model;
        public TableObjectViewModel(TableObject model)
        {
            _model = model;
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            
        }
        public ISyntaxNode Model { get { return _model; } }
        public string Name { get { return _model.Name; } }
    }
}
