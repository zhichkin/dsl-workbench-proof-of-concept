using System.Windows;
using System.Windows.Controls;

namespace OneCSharp.AST.UI
{
    public sealed class MyDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //if (item == null) return null;
            ////if (item is MenuViewModel) return MenuItemTemplate;
            //if (item is TreeNodeViewModel) return LeftRegionTemplate;
            ////else if (item is MetadataViewModel) return LeftRegionTemplate;
            //else if (item is StatusBarViewModel) return StatusBarTemplate;
            ////else if (item is TabViewModel) return RightRegionTemplate;
            return null;
            //return (container as FrameworkElement).FindResource("ComparisonOperatorTemplate") as DataTemplate;
        }
    }
}