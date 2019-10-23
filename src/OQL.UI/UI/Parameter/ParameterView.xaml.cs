using System.Windows.Controls;

namespace OneCSharp.OQL.UI
{
    public partial class ParameterView : UserControl
    {
        public ParameterView()
        {
            InitializeComponent();
        }
        public ParameterView(object viewModel) : this()
        {
            this.DataContext = viewModel;
        }

    }
}
