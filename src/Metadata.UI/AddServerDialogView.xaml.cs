using System.Windows.Controls;
using System.Windows.Input;

namespace OneCSharp.Metadata.UI
{
    public partial class AddServerDialogView : UserControl
    {
        public AddServerDialogView(AddServerDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Keyboard.Focus(ServerNameTextBox);
        }
    }
}
