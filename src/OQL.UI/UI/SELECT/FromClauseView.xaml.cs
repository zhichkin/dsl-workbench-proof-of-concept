using System.Windows.Controls;

namespace OneCSharp.OQL.UI
{
    public partial class FromClauseView : UserControl
    {
        public FromClauseView()
        {
            InitializeComponent();
        }
        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((FromClauseViewModel)DataContext).SelectTableSource();
        }
    }
}
