using System.Collections.Generic;
using System.Windows;

namespace OneCSharp.OQL.UI
{
    public sealed class EventCommandsCollection: List<object>
    {
        //public static DependencyProperty CommandsProperty = DependencyProperty.RegisterAttached(
        //    "Commands",
        //    typeof(IList<object>),
        //    typeof(object),
        //    new PropertyMetadata(null, OnCommandsChanged));

        //public static void SetCommands(DependencyObject element, ICollection<EventToCommand> value)
        //{
        //    element.SetValue(CommandsProperty, value);
        //}
        //public static ICollection<EventToCommand> GetCommands(DependencyObject element)
        //{
        //    return (ICollection<EventToCommand>)element.GetValue(CommandsProperty);
        //}
        //private static void OnCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    // Attach/Detach event handlers
        //}
    }
}
