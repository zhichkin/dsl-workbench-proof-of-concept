using System.Windows.Controls;

namespace OneCSharp.VisualStudio.UI
{
    public partial class AddServerDialogView : UserControl
    {
        public AddServerDialogView(AddServerDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
