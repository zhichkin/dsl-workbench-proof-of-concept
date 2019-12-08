using OneCSharp.DSL.Model;
using OneCSharp.DSL.UI.Services;
using OneCSharp.Metadata.Model;
using OneCSharp.MVVM;
using System.Windows.Input;

namespace OneCSharp.DSL.UI
{
    public sealed class TableObjectViewModel : SyntaxNodeViewModel
    {
        private const string EMPTY_TABLE_PLACEHOLDER = "<table source>";
        public TableObjectViewModel(ISyntaxNodeViewModel parent, TableObject model) : base(parent, model)
        {
            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            SelectTableObjectCommand = new RelayCommand(SelectTableObject);
        }
        public string FullName
        {
            get
            {
                TableObject model = (TableObject)Model;
                if (model.Table == null)
                {
                    return EMPTY_TABLE_PLACEHOLDER;
                }
                else
                {
                    return model.FullName;
                }
            }
        }

        public ICommand SelectTableObjectCommand { get; private set; }
        private void SelectTableObject(object parameter)
        {
            OneCSharpDialog dialog = UIServices.GetTableObjectSelectionDialog(this);
            _ = dialog.ShowDialog();
            if (!(dialog.Content is ISelectionDialog content)) return;
            if (!(content.SelectedItem is Entity selectedTable)) return;

            TableObject model = (TableObject)Model;
            model.Table = selectedTable;
            OnPropertyChanged(nameof(FullName));
        }
    }
}
