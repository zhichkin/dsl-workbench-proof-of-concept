using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxNodeTemplateSelector : DataTemplateSelector
    {
        private readonly Dictionary<Type, Type> _map_models_to_views = new Dictionary<Type, Type>()
        {
            { typeof(IdentifierNodeViewModel), typeof(IdentifierNodeView) },
            { typeof(IndentNodeViewModel), typeof(IndentNodeView) },
            { typeof(KeywordNodeViewModel), typeof(KeywordNodeView) },
            { typeof(LiteralNodeViewModel), typeof(LiteralNodeView) },
            { typeof(ConceptNodeViewModel), typeof(ConceptNodeView) },
            { typeof(RepeatableViewModel), typeof(RepeatableView) },
            { typeof(RepeatableOptionViewModel), typeof(RepeatableOptionView) },
            { typeof(ReferenceViewModel), typeof(ReferenceView) },
            { typeof(SelectorViewModel), typeof(SelectorView) }
        };
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            Type modelType = item.GetType();
            if (_map_models_to_views.TryGetValue(modelType, out Type viewType))
            {
                return new DataTemplate()
                {
                    DataType = modelType,
                    VisualTree = new FrameworkElementFactory(viewType)
                };
            }
            return null;
        }
    }
}