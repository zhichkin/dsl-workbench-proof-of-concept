using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OneCSharp.AST.UI
{
    public sealed class ReferenceViewModel : SyntaxNodeViewModel
    {
        private Brush _textColor = Brushes.Black;
        private Brush _selectedValueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2B91AF"));
        public ReferenceViewModel(ISyntaxNodeViewModel owner) : base(owner) { }
        public string Presentation
        {
            get { return $"{{{PropertyBinding}}}"; }
        }
        public Brush TextColor
        {
            get { return _textColor; }
            set { _textColor = value; OnPropertyChanged(nameof(TextColor)); }
        }
        protected override void OnMouseDown(object parameter)
        {
            if (!(parameter is MouseButtonEventArgs args)) return;
            if (args.ChangedButton == MouseButton.Left || args.ChangedButton == MouseButton.Right)
            {
                args.Handled = true;

                var ancestor = this.Ancestor<ConceptNodeViewModel>();
                if (ancestor == null) return;

                IScopeProvider scopeProvider = (ancestor.Owner == null)
                    ? ancestor.Model as IScopeProvider
                    : ancestor.Owner.Model as IScopeProvider;
                if (scopeProvider == null) return;

                Type scopeType;
                PropertyInfo property = ancestor.Model.GetPropertyInfo(PropertyBinding);
                if (property.IsOptional())
                {
                    property = property.PropertyType.GetProperty("Value");
                }

                if (property.IsRepeatable())
                {
                    scopeType = property.GetRepeatableTypes()[0];
                }
                else
                {
                    scopeType = property.PropertyType;
                }

                var scope = scopeProvider.Scope(scopeType);
                //TODO: open selection dialog

                // Get absolute location on screen of upper left corner of the TextBlock
                Visual control = args.Source as Visual;
                Point locationFromScreen = control.PointToScreen(new Point(0, 0));
                locationFromScreen.Y += 18; // control's height
                locationFromScreen.X -= 6;  // correction
                // Transform screen point to WPF device independent point
                PresentationSource source = PresentationSource.FromVisual(control);
                Point targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);

                PopupWindow dialog = new PopupWindow();
                // Set coordinates
                dialog.Top = locationFromScreen.Y;
                dialog.Left = targetPoints.X;
                _ = dialog.ShowDialog();
                if (dialog.Result == null) { return; }

                TextColor = _selectedValueBrush;
            }
        }
    }
}