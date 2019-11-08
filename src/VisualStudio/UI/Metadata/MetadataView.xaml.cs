using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OneCSharp.VisualStudio.UI
{
    public partial class MetadataView : UserControl
    {
        public MetadataView()
        {
            InitializeComponent();
            DataContext = new MetadataViewModel();
        }
        private void ImportInfoBase_Click(object sender, RoutedEventArgs e)
        {
            ((MetadataViewModel)DataContext).ImportInfoBase();
        }
        

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ((MetadataViewModel)DataContext).SelectedItem = e.NewValue;
        }
        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }
        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

        private void AddWebService_Click(object sender, RoutedEventArgs e)
        {
            ((MetadataViewModel)DataContext).AddWebService();
        }
        private void AddNamespace_Click(object sender, RoutedEventArgs e)
        {
            ((MetadataViewModel)DataContext).AddNamespace();
        }
        private void AddProcedure_Click(object sender, RoutedEventArgs e)
        {
            ((MetadataViewModel)DataContext).AddProcedure();
        }
        private void RenameProcedure_Click(object sender, RoutedEventArgs e)
        {
            ((MetadataViewModel)DataContext).RenameProcedure();
        }
        private void EditProcedure_Click(object sender, RoutedEventArgs e)
        {
            ((MetadataViewModel)DataContext).EditProcedure();
        }
    }
}
