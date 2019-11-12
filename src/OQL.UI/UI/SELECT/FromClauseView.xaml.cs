using System.Windows.Controls;

namespace OneCSharp.OQL.UI
{
    public partial class FromClauseView : UserControl
    {
        public FromClauseView()
        {
            InitializeComponent();
            //this.DataContextChanged += FromClauseView_DataContextChanged;
        }
        //private void FromClauseView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        //{
        //    ((FromClauseViewModel)DataContext).InitializeViewModel();
        //}
        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((FromClauseViewModel)DataContext).SelectTableSource();
        }
    }
}
