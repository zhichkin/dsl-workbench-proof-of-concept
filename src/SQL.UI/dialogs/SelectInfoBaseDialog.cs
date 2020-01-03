using OneCSharp.SQL.Model;
using System.Collections.Generic;
using System.Windows;

namespace OneCSharp.SQL.UI
{
    public sealed class SelectInfoBaseDialog : Window
    {
        private readonly SelectInfoBaseDialogViewModel viewModel;
        public SelectInfoBaseDialog(List<Database> infoBases)
        {
            this.Title = "Select database...";
            this.SizeToContent = SizeToContent.WidthAndHeight;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            viewModel = new SelectInfoBaseDialogViewModel(infoBases);
            viewModel.OnCancel = OnCancel;
            viewModel.OnConfirm = OnConfirm;
            this.Content = new SelectInfoBaseDialogView(viewModel);
        }
        public object Result { get; private set; }
        private void OnConfirm(object result)
        {
            Result = result;
            this.Close();
        }
        private void OnCancel()
        {
            this.Close();
        }
    }
}