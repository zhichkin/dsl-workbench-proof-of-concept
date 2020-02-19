using System.Windows.Controls;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public partial class RemoveOptionView : UserControl
    {
        public RemoveOptionView()
        {
            InitializeComponent();
        }
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }
    }
}