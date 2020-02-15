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
                        // References, Selectors, Concepts, Keywords, Literals, Indents and Identifiers
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
        private ISyntaxNode CreateRepeatableConcept(Type repeatable, ISyntaxNode parent, string propertyName)
        {
            ISyntaxNode concept = (ISyntaxNode)Activator.CreateInstance(repeatable);
            concept.Parent = parent;

            IList list;
            PropertyInfo property = parent.GetPropertyInfo(propertyName);
            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(parent);
                list = (IList)optional.Value;
                if (list == null)
                {
                    Type listType = property.PropertyType.GetProperty("Value").PropertyType;
                    list = (IList)Activator.CreateInstance(listType);
                    optional.Value = list;
                }
            }
            else
            {
                list = (IList)property.GetValue(parent);
                if (list == null)
                {
                    Type listType = property.PropertyType;
                    list = (IList)Activator.CreateInstance(listType);
                    property.SetValue(parent, list);
                }
            }
            list.Add(concept);
            return concept;
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