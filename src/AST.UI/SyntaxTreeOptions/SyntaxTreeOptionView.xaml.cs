using System;
using System.Windows.Controls;

namespace OneCSharp.AST.UI
{
    public partial class SyntaxTreeOptionView : UserControl
    {
        public SyntaxTreeOptionView()
        {
            InitializeComponent();
        }
        private void HideOptionAnimation_Completed(object sender, EventArgs args)
        {
            if (!(DataContext is SyntaxTreeOptionViewModel option)) return;
            if (!(option.Owner is ISyntaxNodeViewModel owner)) return;

            option.IsVisible = false;
            foreach (var line in owner.Lines)
            {
                for (int i = 0; i < line.Nodes.Count; i++)
                {
                    if (this == line.Nodes[i])
                    {
                        line.Nodes.Remove((ISyntaxNodeViewModel)this);
                        return;
                    }
                }
            }
        }
    }
}