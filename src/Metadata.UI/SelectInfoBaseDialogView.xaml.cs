using System.Windows.Controls;

namespace OneCSharp.Metadata.UI
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
