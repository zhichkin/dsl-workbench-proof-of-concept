using System;
using System.Windows;
using System.Windows.Controls;

namespace OneCSharp.OQL.UI.Dialogs
{
    public partial class TypeSelectionDialog : UserControl
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
            this.OnSelectionChanged?.Invoke(viewModel.SelectedNode);
        }
        public Action<TreeNodeViewModel> OnSelectionChanged;
    }
}
