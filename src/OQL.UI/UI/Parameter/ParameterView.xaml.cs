using OneCSharp.OQL.UI.Dialogs;
using OneCSharp.OQL.UI.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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
        private void TypeName_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var target = sender as UIElement;
            if (target == null) return;
            UIServices.OpenTypeSelectionPopup(target, TypeChanged);
            e.Handled = true;
        }
        private void TypeChanged(TreeNodeViewModel selectedItem)
        {
            UIServices.CloseTypeSelectionPopup();
            if (selectedItem == null) return;
            var parameter = this.DataContext as ParameterViewModel;
            if (parameter == null) return;
            if (selectedItem.Payload == null) return;
            parameter.Type = (Type)selectedItem.Payload;
        }
    }
}
