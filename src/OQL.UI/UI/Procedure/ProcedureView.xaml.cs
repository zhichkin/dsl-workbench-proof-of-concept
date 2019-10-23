using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OneCSharp.OQL.UI
{
    public partial class ProcedureView : UserControl
    {
        public ProcedureView()
        {
            InitializeComponent();
        }
        public ProcedureView(object viewModel) : this()
        {
            this.DataContext = viewModel;
        }
        private void ContextMenu_AddParameter_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as ProcedureViewModel;
            if (vm == null) return;
            vm.AddParameter();
        }

        private void Keyword_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            if (tb == null) return;
            tb.ContextMenu.PlacementTarget = tb;
            tb.ContextMenu.IsOpen = true;
            e.Handled = true;
        }

        private void ContextMenu_AddSelectStatement_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as ProcedureViewModel;
            if (vm == null) return;
            vm.AddSelectStatement();
        }
    }
}
