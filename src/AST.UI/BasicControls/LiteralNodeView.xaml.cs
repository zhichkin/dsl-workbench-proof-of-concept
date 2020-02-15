using System;
using System.Windows.Controls;

namespace OneCSharp.AST.UI
{
    public partial class LiteralNodeView : TextBlock
    {
        public LiteralNodeView()
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