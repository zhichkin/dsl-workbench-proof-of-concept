using System.Windows;

namespace OneCSharp.AST.UI
{
    public static class SyntaxTreeOptionBehavior
    {
        private const string IS_TEMPORALLY_VISIBLE = "IsTemporallyVisible";
        public static bool GetIsTemporallyVisible(DependencyObject view)
        {
            return (bool)view.GetValue(IsTemporallyVisibleProperty);
        }
        public static void SetIsTemporallyVisible(DependencyObject view, bool value)
        {
            view.SetValue(IsTemporallyVisibleProperty, value);
        }
        public static readonly DependencyProperty IsTemporallyVisibleProperty =
            DependencyProperty.RegisterAttached(
                IS_TEMPORALLY_VISIBLE,
                typeof(bool),
                typeof(SyntaxTreeOptionBehavior),
                new PropertyMetadata(false, OnIsTemporallyVisibleChanged));

        private static void OnIsTemporallyVisibleChanged(object view, DependencyPropertyChangedEventArgs args)
        {
            // args.OldValue args.NewValue
        }
    }

    public static class VisualStateApplier
    {

        public static string GetVisualState(DependencyObject target)
        {
            return target.GetValue(VisualStateProperty) as string;
        }
        public static void SetVisualState(DependencyObject target, string value)
        {
            target.SetValue(VisualStateProperty, value);
        }

        public static readonly DependencyProperty VisualStateProperty =
            DependencyProperty.RegisterAttached("VisualState", typeof(string), typeof(VisualStateApplier), new PropertyMetadata(VisualStatePropertyChangedCallback));

        private static void VisualStatePropertyChangedCallback(DependencyObject target, DependencyPropertyChangedEventArgs args)
        {
            VisualStateManager.GoToElementState((FrameworkElement)target, args.NewValue as string, true); // <- for UIElements
            //VisualStateManager.GoToState((FrameworkElement)target, args.NewValue as string, true); // <- for Controls
        }
    }
}

