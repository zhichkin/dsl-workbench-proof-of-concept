using OneCSharp.AST.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneCSharp.AST.UI
{
    public sealed class ConceptNodeViewModel : SyntaxNodeViewModel
    {
        public ConceptNodeViewModel(ISyntaxNodeViewModel owner, ISyntaxNode model) : base(owner, model) { }
        public void ShowOptions()
        {
            Type metadata = Model.GetType();
            foreach (PropertyInfo property in metadata
                .GetProperties().Where(p => p.IsOptional()))
            {
                IOptional optional = (IOptional)property.GetValue(Model);
                if (!property.IsRepeatable() && optional.HasValue) { continue; }

                List<ISyntaxNodeViewModel> nodes = this.GetNodesByPropertyName(property.Name);
                if (nodes.Count == 0) return;

                foreach (var node in nodes)
                {
                    if (node.IsTemporallyVisible)
                    {
                        node.ResetHideOptionAnimation();
                        continue;
                    }

                    if (node is RepeatableViewModel)
                    {
                        ShowRepeatableOption(node);
                    }
                    else
                    {
                        // Selectors, Concepts, Keywords, Literals, Indents and Identifiers
                        node.StartHideOptionAnimation();
                        
                    }
                    if (node is SelectorViewModel) { /* TODO */ }
                    if (node is ConceptNodeViewModel) { /* TODO */ }
                }
            }
        }
        private void ShowRepeatableOption(ISyntaxNodeViewModel repetableNode)
        {
            if (!repetableNode.IsVisible)
            {
                repetableNode.IsVisible = true;
            }

            RepeatableOptionViewModel option = null;
            foreach (var line in repetableNode.Lines)
            {
                foreach (var node in line.Nodes)
                {
                    if (node is RepeatableOptionViewModel)
                    {
                        option = (RepeatableOptionViewModel)node;
                        option.ResetHideOptionAnimation();
                    }
                }
            }
            if (option != null) { return; }

            option = new RepeatableOptionViewModel((RepeatableViewModel)repetableNode)
            {
                Presentation = $"[{repetableNode.PropertyBinding}]"
            };
            repetableNode.Add(option);
            option.StartHideOptionAnimation();
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
                    node.StopHideOptionAnimation();
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
            }
        }
    }
}