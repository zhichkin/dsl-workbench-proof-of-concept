using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OneCSharp.MVVM
{
    public sealed class PopupWindow : Window
    {
        public PopupWindow(Visual parentControl, TreeNodeViewModel viewModel)
        {
            if (parentControl != null)
            {
                // Get absolute location on screen of upper left corner of the TextBlock
                Point locationFromScreen = parentControl.PointToScreen(new Point(0, 0));
                locationFromScreen.Y += 18; // SystemParameters.WindowCaptionHeight;
                locationFromScreen.X -= 6;  // correction
                                            // Transform screen point to WPF device independent point
                PresentationSource source = PresentationSource.FromVisual(parentControl); // this
                Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
                Top = targetPoints.Y;
                Left = targetPoints.X;

                AllowsTransparency = false;
                WindowStyle = WindowStyle.None;
                ResizeMode = ResizeMode.CanResize;
                WindowStartupLocation = WindowStartupLocation.Manual;
            }
            else
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            
            SizeToContent = SizeToContent.Manual;
            Width = 200;
            Height = 300;

            viewModel.SelectedItemChanged = new RelayCommand(SelectedItemChangedHandler);
            Content = new TreeNodeView() { DataContext = viewModel };
        }
        public TreeNodeViewModel Result { get; private set; }
        private void SelectedItemChangedHandler(object parameter)
        {
            var args = parameter as RoutedPropertyChangedEventArgs<object>;
            if (args == null) return;
            args.Handled = true;
            Result = args.NewValue as TreeNodeViewModel;
            this.Close();
        }
        protected override void OnActivated(EventArgs e)
        {
            Mouse.Capture(this, CaptureMode.SubTree);
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs args)
        {
            OnMouseButtonUp(args);
        }
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs args)
        {
            OnMouseButtonUp(args);
        }
        private void OnMouseButtonUp(MouseButtonEventArgs args)
        {
            Point clickPoint = args.GetPosition(this);
            if (clickPoint.X < 0
                || clickPoint.X > Width
                || clickPoint.Y + 25 < 0
                || clickPoint.Y + 25 > Height)
            {
                args.Handled = true;
                this.Close();
            }
        }
    }
}