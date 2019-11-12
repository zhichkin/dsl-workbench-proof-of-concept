using System.Windows.Input;
using System.Windows;
using System.Collections.Generic;
using System.Collections;

namespace OneCSharp.OQL.UI
{
    public sealed class EventToCommand
    {
        #region Command

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(EventToCommand), new UIPropertyMetadata(null));

        #endregion

        #region CommandParameter

        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(EventToCommand), new UIPropertyMetadata(null));

        #endregion

        #region Event

        public static RoutedEvent GetEvent(DependencyObject obj)
        {
            return (RoutedEvent)obj.GetValue(EventProperty);
        }

        public static void SetEvent(DependencyObject obj, RoutedEvent value)
        {
            obj.SetValue(EventProperty, value);
        }

        public static readonly DependencyProperty EventProperty =
            DependencyProperty.RegisterAttached("Event", typeof(RoutedEvent), typeof(EventToCommand), new UIPropertyMetadata(null, EventChanged));

        #endregion

        private static void EventChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var element = sender as UIElement;
            if (element != null)
            {
                element.AddHandler((RoutedEvent)args.NewValue, new RoutedEventHandler(ExecuteCommand));
            }
        }

        private static void ExecuteCommand(object sender, RoutedEventArgs args)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                var command = (ICommand)element.GetValue(EventToCommand.CommandProperty);
                if (command != null)
                {
                    var parameter = element.GetValue(EventToCommand.CommandParameterProperty);
                    parameter = parameter ?? args;
                    command.Execute(parameter);
                }
            }
        }
        
        public static DependencyProperty CommandsProperty = DependencyProperty.RegisterAttached(
            "Commands",
            typeof(IList), //<EventToCommand>
            typeof(object),
            new PropertyMetadata(null, OnCommandsChanged));

        public static void SetCommands(DependencyObject element, IList<object> value)
        {
            element.SetValue(CommandsProperty, value);
        }
        public static IList<object> GetCommands(DependencyObject element)
        {
            return (IList<object>)element.GetValue(CommandsProperty);
        }
        private static void OnCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Attach/Detach event handlers
        }
    }
}
