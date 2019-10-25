using OneCSharp.OQL.UI.Dialogs;
using OneCSharp.OQL.UI.Services;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OneCSharp.OQL.UI
{
    public partial class ParametersListView : UserControl
    {
        public ParametersListView()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.MyListView.ItemsSource = this.DataContext as IEnumerable;
        }

        private object senderElement = null;
        private void TypeName_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var target = sender as UIElement;
            if (target == null) return;
            senderElement = target;
            UIServices.OpenTypeSelectionPopup(target, TypeChanged);
            e.Handled = true;
        }
        private void TypeChanged(TreeNodeViewModel selectedItem)
        {
            UIServices.CloseTypeSelectionPopup();

            if (selectedItem == null) return;
            if (senderElement == null) return;
            FrameworkElement sender = senderElement as FrameworkElement;
            if (sender == null) return;

            ParameterViewModel parameter = sender.DataContext as ParameterViewModel;
            senderElement = null;
            if (parameter == null) return;
            if (selectedItem.Payload == null) return;

            parameter.Type = (Type)selectedItem.Payload;
            
            var view = this.MyListView.View as GridView;
            if (view == null) return;
            AutoResizeGridViewColumns(view);
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            var view = this.MyListView.View as GridView;
            if (view == null) return;
            AutoResizeGridViewColumns(view);
        }
        private void AutoResizeGridViewColumns(GridView view)
        {
            foreach (var column in view.Columns)
            {
                if (double.IsNaN(column.Width))
                {
                    column.Width = 1;
                }
                column.Width = double.NaN;
            }
        }

        private void MyListView_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item == null) return;

            ParameterViewModel parameter = item.DataContext as ParameterViewModel;
            if (parameter == null) return;

            parameter.IsRemoveButtonVisible = true;
        }
        private void MyListView_MouseLeave(object sender, MouseEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item == null) return;

            ParameterViewModel parameter = item.DataContext as ParameterViewModel;
            if (parameter == null) return;

            parameter.IsRemoveButtonVisible = true;
        }
        private void MyListView_MouseEnter(object sender, MouseEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item == null) return;

            ParameterViewModel parameter = item.DataContext as ParameterViewModel;
            if (parameter == null) return;

            parameter.IsRemoveButtonVisible = false;
        }
        private void RemoveParameter_Click(object sender, RoutedEventArgs e)
        {
            Button item = sender as Button;
            if (item == null) return;

            ParameterViewModel parameter = item.DataContext as ParameterViewModel;
            if (parameter == null) return;

            parameter.RemoveParameter();
        }
    }
}
