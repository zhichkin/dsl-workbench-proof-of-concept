using System;
using System.Windows.Controls;

namespace OneCSharp.AST.UI
{
    public partial class ReferenceView : UserControl
    {
        public ReferenceView()
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