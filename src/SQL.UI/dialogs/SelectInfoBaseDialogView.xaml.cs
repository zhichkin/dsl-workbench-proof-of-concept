using System.Windows.Controls;

namespace OneCSharp.SQL.UI
{
    public partial class SelectInfoBaseDialogView : UserControl
    {
        public SelectInfoBaseDialogView(SelectInfoBaseDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
