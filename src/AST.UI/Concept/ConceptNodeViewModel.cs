using OneCSharp.AST.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public sealed class ConceptNodeViewModel : SyntaxNodeViewModel
    {
        public ConceptNodeViewModel(ISyntaxNodeViewModel owner, ISyntaxNode model) : base(owner, model) { }
        public void ShowOptions()
        {
            Type metadata = SyntaxNode.GetType();
            
            foreach (PropertyInfo property in metadata
                .GetProperties().Where(p => p.IsOptional()))
            {
                IOptional optional = (IOptional)property.GetValue(SyntaxNode);
                if (!property.IsRepeatable() && optional.HasValue)
                {
                    continue;
                }

                List<ISyntaxNodeViewModel> nodes = this.GetNodesByPropertyName(property.Name);
                if (nodes.Count == 0) return;

                foreach (var node in nodes)
                {
                    if (node is PropertyViewModel)
                    {
                        ShowProperty(node);
                    }
                    else if (node is RepeatableViewModel)
                    {
                        ShowRepeatableOption(node);
                    }
                    else if (node is ConceptNodeViewModel)
                    {
                        ShowConceptOption(node);
                    }
                    // Indents, Identifiers, Keywords, Literals and Selectors
                    else if (node.IsTemporallyVisible)
                    {
                        node.ResetHideOptionAnimation();
                    }
                    else
                    {
                        node.StartHideOptionAnimation();
                    }
                }
            }

            foreach (PropertyInfo property in metadata
                .GetProperties().Where(p => !p.IsOptional() && p.IsRepeatable()))
            {
                List<ISyntaxNodeViewModel> nodes = this.GetNodesByPropertyName(property.Name);
                if (nodes.Count == 0) return;
                foreach (var node in nodes)
                {
                    if (node is RepeatableViewModel)
                    {
                        ShowRepeatableOption(node);
                    }
                }
            }
        }
        private void ShowProperty(ISyntaxNodeViewModel propertyNode)
        {
            if (!(propertyNode is PropertyViewModel property)) return;

            if (property.IsTemporallyVisible)
            {
                property.ResetHideOptionAnimation();
            }
            else
            {
                property.StartHideOptionAnimation();
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
        private void ShowConceptOption(ISyntaxNodeViewModel conceptNode)
        {
            int lineIndex = 0;
            int nodeIndex = 0;
            ConceptOptionViewModel option = null;
            for (int l = 0; l < conceptNode.Owner.Lines.Count; l++)
            {
                var line = conceptNode.Owner.Lines[l];
                for (int n = 0; n < line.Nodes.Count; n++)
                {
                    var node = line.Nodes[n];
                    if (node == conceptNode)
                    {
                        if (n > 0)
                        {
                            if (line.Nodes[n - 1] is ConceptOptionViewModel)
                            {
                                option = (ConceptOptionViewModel)line.Nodes[n - 1];
                                option.ResetHideOptionAnimation();
                            }
                        }
                        else
                        {
                            lineIndex = l;
                            nodeIndex = n;
                        }
                    }
                }
            }
            if (option != null) { return; }

            option = new ConceptOptionViewModel((ConceptNodeViewModel)conceptNode.Owner)
            {
                PropertyBinding = conceptNode.PropertyBinding
            };
            conceptNode.Owner.Lines[lineIndex].Nodes.Insert(nodeIndex, option);
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

            PropertyInfo property = SyntaxNode.GetPropertyInfo(propertyName);
            if (!property.IsOptional()) return;
            IOptional optional = (IOptional)property.GetValue(SyntaxNode);
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
        public void RemoveOption(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return;
            
            PropertyInfo property = SyntaxNode.GetPropertyInfo(propertyName);
            if (!property.IsOptional()) return;
            IOptional optional = (IOptional)property.GetValue(SyntaxNode);
            if (!optional.HasValue) return;

            optional.HasValue = false;
            var nodes = this.GetNodesByPropertyName(propertyName);
            foreach (var node in nodes)
            {
                node.IsVisible = false;
            }
        }
        protected override void OnMouseEnter(object parameter)
        {
            if (parameter == null) return;
            MouseEventArgs args = (MouseEventArgs)parameter;
            args.Handled = true;

            IsMouseOver = true;
        }
        protected override void OnMouseLeave(object parameter)
        {
            if (parameter == null) return;
            MouseEventArgs args = (MouseEventArgs)parameter;
            args.Handled = true;

            IsMouseOver = false;
            if (Owner is ConceptNodeViewModel)
            {
                PropertyInfo property = Owner.SyntaxNode.GetPropertyInfo(PropertyBinding);
                if (property.IsOptional())
                {
                    HideCommands();
                }
            }
            if (!(Owner is RepeatableViewModel repeatable)) return;
            HideCommands();
        }
        public void ShowCommands()
        {
            if (Lines.Count == 0) return;
            foreach (var node in Lines[0].Nodes)
            {
                if (node is RemoveConceptViewModel) return;
            }
            Lines[0].Nodes.Insert(0, new RemoveConceptViewModel(this)
            {
                PropertyBinding = Owner.PropertyBinding
            });
        }
        public void HideCommands()
        {
            if (Lines.Count == 0) return;
            if (Lines[0].Nodes[0] is RemoveConceptViewModel)
            {
                Lines[0].Nodes.RemoveAt(0);
            }
        }
        public void RemoveConcept(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return;

            // first check if own property reset to null action is requested ...
            PropertyInfo p = SyntaxNode.GetPropertyInfo(propertyName);
            if (p != null)
            {
                if (p.IsOptional())
                {
                    IOptional o = (IOptional)p.GetValue(SyntaxNode);
                    o.HasValue = false;
                }
                else
                {
                    p.SetValue(SyntaxNode, null);
                }
                // 
                return;
            }

            // second check if remove repeatable concept command is requested
            if (!(Owner is RepeatableViewModel repeatable)) return;

            // TODO: move the code below to SyntaxTreeManager
            PropertyInfo property = repeatable.Owner.SyntaxNode.GetPropertyInfo(propertyName);
            IList list;
            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(repeatable.Owner.SyntaxNode);
                if (!optional.HasValue) return;
                list = (IList)optional.Value;
            }
            else
            {
                list = (IList)property.GetValue(repeatable.Owner.SyntaxNode);
            }
            list.Remove(SyntaxNode);

            // TODO: move the code below to SyntaxNodeExtentions
            for (int l = 0; l < repeatable.Lines.Count; l++)
            {
                var line = repeatable.Lines[l];
                for (int i = 0; i < line.Nodes.Count; i++)
                {
                    if (line.Nodes[i] == this)
                    {
                        repeatable.Lines.RemoveAt(l);
                        return;
                    }
                }
            }
        }
    }
}