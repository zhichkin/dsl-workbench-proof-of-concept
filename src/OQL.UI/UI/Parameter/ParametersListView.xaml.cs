using OneCSharp.OQL.UI.Dialogs;
using OneCSharp.OQL.UI.Services;
using System;
using System.Collections;
using System.Collections.Specialized;
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
            INotifyCollectionChanged collectionTracker = this.DataContext as INotifyCollectionChanged;
            if (collectionTracker == null) return;
            collectionTracker.CollectionChanged += ItemsSource_CollectionChanged;
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Remove)
            {
                var view = this.MyListView.View as GridView;
                if (view == null) return;
                AutoResizeGridViewColumns(view);
            }
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
                if ((string)column.Header == "Commands") continue;
                if (double.IsNaN(column.Width))
                {
                    column.Width = 1;
                }
                column.Width = double.NaN;
            }
        }

        private void ListBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item == null) return;

            ParameterViewModel parameter = item.DataContext as ParameterViewModel;
            if (parameter == null) return;

            parameter.IsRemoveButtonVisible = true;
        }
        private void ListBoxItem_MouseLeave(object sender, MouseEventArgs e)
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
        private void MoveParameterUp_Click(object sender, RoutedEventArgs e)
        {
            Button item = sender as Button;
            if (item == null) return;

            ParameterViewModel parameter = item.DataContext as ParameterViewModel;
            if (parameter == null) return;

            parameter.MoveParameterUp();
        }
        private void MoveParameterDown_Click(object sender, RoutedEventArgs e)
        {
            Button item = sender as Button;
            if (item == null) return;

            ParameterViewModel parameter = item.DataContext as ParameterViewModel;
            if (parameter == null) return;

            parameter.MoveParameterDown();
        }
    }
}
