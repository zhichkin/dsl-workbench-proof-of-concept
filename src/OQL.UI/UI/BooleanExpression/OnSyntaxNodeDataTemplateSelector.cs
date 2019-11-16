using System.Windows;
using System.Windows.Controls;

namespace OneCSharp.OQL.UI
{
    public class OnSyntaxNodeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BooleanOperatorTemplate { get; set; }
        public DataTemplate ComparisonOperatorTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            if (item is BooleanOperatorViewModel)
            {
                return BooleanOperatorTemplate;
            }
            else if (item is ComparisonOperatorViewModel)
            {
                return ComparisonOperatorTemplate;
            }
            return null;
            //return (container as FrameworkElement).FindResource("ComparisonOperatorTemplate") as DataTemplate;
        }
    }
}
