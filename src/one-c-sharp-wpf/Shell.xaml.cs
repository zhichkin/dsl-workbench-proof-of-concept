using System.Windows;

namespace OneCSharp.Shell
{
    public partial class Shell : Window
    {
        public Shell(ShellViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
