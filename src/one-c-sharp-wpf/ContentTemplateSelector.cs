using OneCSharp.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace OneCSharp.Shell
{
    internal sealed class ContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MenuItemTemplate { get; set; }
        public DataTemplate LeftRegionTemplate { get; set; }
        public DataTemplate RightRegionTemplate { get; set; }
        public DataTemplate StatusBarTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;
            //if (item is MenuViewModel) return MenuItemTemplate;
            if (item is TreeNodeViewModel) return LeftRegionTemplate;
            //else if (item is MetadataViewModel) return LeftRegionTemplate;
            //else if (item is StatusBarViewModel) return StatusBarTemplate;
            //else if (item is TabViewModel) return RightRegionTemplate;
            return null;
            //return (container as FrameworkElement).FindResource("ComparisonOperatorTemplate") as DataTemplate;
        }
    }
}
