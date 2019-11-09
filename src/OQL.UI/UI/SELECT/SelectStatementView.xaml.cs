using System.Windows.Controls;

namespace OneCSharp.OQL.UI
{
    public partial class SelectStatementView : UserControl
    {
        public SelectStatementView()
        {
            InitializeComponent();
        }
        public SelectStatementView(object viewModel) : this()
        {
            this.DataContext = viewModel;
        }
    }
}
