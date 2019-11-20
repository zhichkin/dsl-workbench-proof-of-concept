using System.Windows;
using System.Windows.Controls;

namespace OneCSharp.OQL.UI
{
    public class SyntaxNodeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HintSyntaxNodeTemplate { get; set; }
        public DataTemplate BooleanOperatorTemplate { get; set; }
        public DataTemplate ComparisonOperatorTemplate { get; set; }
        public DataTemplate ParameterReferenceTemplate { get; set; }
        public DataTemplate PropertyReferenceTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            if (item is HintSyntaxNodeViewModel) return HintSyntaxNodeTemplate;
            else if (item is BooleanOperatorViewModel) return BooleanOperatorTemplate;
            else if (item is ComparisonOperatorViewModel) return ComparisonOperatorTemplate;
            else if (item is PropertyReferenceViewModel) return PropertyReferenceTemplate;
            else if (item is ParameterReferenceViewModel) return ParameterReferenceTemplate;
            return null;
            //return (container as FrameworkElement).FindResource("ComparisonOperatorTemplate") as DataTemplate;
        }
    }
}
