using System;
using System.Windows.Controls;

namespace OneCSharp.AST.UI
{
    public partial class KeywordNodeView : TextBlock
    {
        public KeywordNodeView()
        {
            InitializeComponent();
        }
        private void HideOptionsAnimation_Completed(object sender, EventArgs args)
        {
            if (DataContext is ISyntaxNodeViewModel vm)
            {
                vm.HideOptionsCommand.Execute(args);
            }
        }
    }
}