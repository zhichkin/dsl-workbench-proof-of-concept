using System.Windows;

namespace OneCSharp.MVVM
{
    public sealed class PopupWindow : Window
    {
        private readonly InputStringDialogViewModel viewModel;
        public PopupWindow()
        {
            AllowsTransparency = false;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.CanResize;
            SizeToContent = SizeToContent.WidthAndHeight;
            WindowStartupLocation = WindowStartupLocation.Manual;
            //Top = 0;
            //Left = 0;

            viewModel = new InputStringDialogViewModel();
            viewModel.OnCancel = OnCancel;
            viewModel.OnConfirm = OnConfirm;
            Content = new InputStringDialogView(viewModel);
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