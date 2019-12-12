using System.Windows.Controls;
using System.Windows.Input;

namespace OneCSharp.MVVM
{
    public partial class InputStringDialogView : UserControl
    {
        public InputStringDialogView(InputStringDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Keyboard.Focus(NameTextBox);
        }
    }
}
