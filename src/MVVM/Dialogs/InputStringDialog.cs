using System.Windows;

namespace OneCSharp.MVVM
{
    public sealed class InputStringDialog : Window
    {
        private readonly InputStringDialogViewModel viewModel;
        public InputStringDialog()
        {
            //this.HasMaximizeButton = true;
            //this.HasMinimizeButton = true;
            this.Title = "Input string";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            viewModel = new InputStringDialogViewModel();
            viewModel.OnCancel = OnCancel;
            viewModel.OnConfirm = OnConfirm;
            this.Content = new InputStringDialogView(viewModel);
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
