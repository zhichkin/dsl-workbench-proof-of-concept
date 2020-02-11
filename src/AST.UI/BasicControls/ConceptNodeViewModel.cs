using OneCSharp.AST.Model;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

namespace OneCSharp.AST.UI
{
    public sealed class ConceptNodeViewModel : SyntaxNodeViewModel
    {
        public ConceptNodeViewModel(ISyntaxNodeViewModel owner, ISyntaxNode model) : base(owner, model) { }
        public void ShowOptions()
        {
            foreach (var line in this.Lines)
            {
                foreach (var node in line.Nodes)
                {
                    if (node.IsTemporallyVisible)
                    {
                        node.ResetHideOptionsAnimation = true;
                        node.ResetHideOptionsAnimation = false;
                        continue;
                    }

                    
                    node.IsTemporallyVisible = true;
                    
                    if (node is RepeatableViewModel)
                    {
                        // TODO: if repeatable has no items - nothing will be shown !
                    }
                }
            }
        }
        public void ProcessOptionSelection(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return;
            List<ISyntaxNodeViewModel> nodes = this.GetNodesByPropertyName(propertyName);
            if (nodes.Count == 0) return;

            foreach (var node in nodes)
            {
                if (node.IsTemporallyVisible)
                {
                    node.StopHideOptionsAnimation = true;
                    node.HideOptionsCommand.Execute(propertyName);
                }
            }

            PropertyInfo property = Model.GetPropertyInfo(propertyName);
            if (!property.IsOptional()) return;
            IOptional optional = (IOptional)property.GetValue(Model);
            if (optional.HasValue) return;

            optional.HasValue = true;
            if (optional.Value != null && optional.Value.GetType() == typeof(bool))
            {
                optional.Value = true;
            }

            foreach (var node in nodes)
            {
                node.IsVisible = true;
                if (node is KeywordNodeViewModel keyword)
                {
                    keyword.TextBrush = Brushes.Blue;
                }
            }
        }
    }
}