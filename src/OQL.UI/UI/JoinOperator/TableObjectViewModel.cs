using Microsoft.VisualStudio.PlatformUI;
using OneCSharp.Metadata;
using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Services;
using System.Windows.Input;

namespace OneCSharp.OQL.UI
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
            SelectTableObjectCommand = new DelegateCommand(SelectTableObject);
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
        private void SelectTableObject()
        {
            OneCSharpDialog dialog = UIServices.GetTableObjectSelectionDialog(this);
            _ = dialog.ShowModal();
            if (!(dialog.Content is ISelectionDialog content)) return;
            if (!(content.SelectedItem is DbObject selectedTable)) return;

            TableObject model = (TableObject)Model;
            model.Table = selectedTable;
            OnPropertyChanged(nameof(FullName));
        }
    }
}
