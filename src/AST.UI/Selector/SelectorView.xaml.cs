using System;
using System.Windows.Controls;

namespace OneCSharp.AST.UI
{
    public partial class SelectorView : UserControl
    {
        public SelectorView()
        {
            InitializeComponent();
        }
        private void HideOptionAnimation_Completed(object sender, EventArgs args)
        {
            if (DataContext is ISyntaxNodeViewModel vm)
            {
                vm.StopHideOptionAnimation();
            }
        }
    }
}