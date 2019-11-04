using Microsoft.VisualStudio.PlatformUI;
using OneCSharp.Metadata;
using System.Collections.Generic;
using System.Windows;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class SelectInfoBaseDialog : DialogWindow
    {
        private readonly SelectInfoBaseDialogViewModel viewModel;
        public SelectInfoBaseDialog(List<InfoBase> infoBases)
        {
            this.Title = "Select database...";
            this.SizeToContent = SizeToContent.WidthAndHeight;
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
