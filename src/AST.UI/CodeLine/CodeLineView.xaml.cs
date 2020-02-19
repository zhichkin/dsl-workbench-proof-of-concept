using System.Windows.Controls;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public partial class CodeLineView : UserControl
    {
        public CodeLineView()
        {
            InitializeComponent();
        }
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }
    }
}