using OneCSharp.OQL.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OneCSharp.OQL.UI.Dialogs
{
    public partial class TypeSelectionDialog : UserControl, ISelectionDialog
    {
        public TypeSelectionDialog()
        {
            InitializeComponent();
        }
        public TypeSelectionDialog(TreeNodeViewModel viewModel) : this()
        {
            this.DataContext = viewModel;
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeNodeViewModel viewModel = this.DataContext as TreeNodeViewModel;
            if (viewModel == null) return;
            viewModel.SelectedNode = (TreeNodeViewModel)e.NewValue;
            SelectedItem = ((TreeNodeViewModel)e.NewValue).Payload;
            this.OnSelectionChanged?.Invoke(viewModel.SelectedNode);
            e.Handled = true;
        }
        public Action<TreeNodeViewModel> OnSelectionChanged;
        public object SelectedItem { get; set; }
    }
}
