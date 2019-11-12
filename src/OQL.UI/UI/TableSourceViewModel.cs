using OneCSharp.OQL.Model;

namespace OneCSharp.OQL.UI
{
    public sealed class TableSourceViewModel : SyntaxNodeViewModel
    {
        private readonly TableSource _model;
        public TableSourceViewModel(TableSource model)
        {
            _model = model;
            InitializeViewModel();
        }
        private void InitializeViewModel()
        {
            
        }
        public ISyntaxNode Model { get { return _model; } }
        public string Name { get { return _model.Name; } }
    }
}
