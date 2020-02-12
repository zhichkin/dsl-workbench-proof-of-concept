using OneCSharp.AST.Model;
using System;
using System.Collections;
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

                    if (node is RepeatableViewModel)
                    {
                        ShowRepeatableOption(node);
                        //node.IsTemporallyVisible = true;
                    }
                    else
                    {
                        // References, Selectors, Concepts, Keywords, Literals, Indents and Identifiers
                        node.IsTemporallyVisible = true;
                    }
                    if (node is SelectorViewModel) { /* TODO */ }
                    if (node is ConceptNodeViewModel) { /* TODO */ }
                }
            }
        }
        private void ShowRepeatableOption(ISyntaxNodeViewModel repetableNode)
        {
            //if (repetableNode.IsVisible) return; // TODO: add command view model to add new repeatable item

            CreateRepeatableOption option = null;
            foreach (var line in repetableNode.Lines)
            {
                foreach (var node in line.Nodes)
                {
                    if (node is CreateRepeatableOption)
                    {
                        option = (CreateRepeatableOption)node;
                    }
                }
            }
            if (option == null)
            {
                option = new CreateRepeatableOption((RepeatableViewModel)repetableNode)
                {
                    Presentation = $"{repetableNode.PropertyBinding} (click to add item)"
                };
                repetableNode.Add(option);
            }
            
            //TODO: repeatable view model does not have event handlers to reset visibility after animation is over
            repetableNode.IsVisible = true;
            option.IsVisible = false;
            option.IsTemporallyVisible = true;

            //PropertyInfo property = Model.GetPropertyInfo(repetableNode.PropertyBinding);
            //if (property == null) return;
            //if (!property.IsRepeatable()) return;

            //TypeConstraint constraints = SyntaxTreeManager.GetTypeConstraints(Model, repetableNode.PropertyBinding);

            //if (constraints.DataTypes.Count == 0
            //    && constraints.Concepts.Count == 1
            //    && constraints.DotNetTypes.Count == 0)
            //{
            //    //TODO: replace with SyntaxTreeManager !?
            //    SyntaxTreeController controller = new SyntaxTreeController();

            //    ISyntaxNode model = CreateRepeatableConcept(constraints.Concepts[0], Model, property.Name);
            //    ConceptNodeViewModel repeatable = controller.CreateSyntaxNode(repetableNode, model);
            //    repetableNode.Add(repeatable);
            //    repetableNode.IsTemporallyVisible = true;
            //    repeatable.IsTemporallyVisible = true;
            //}
            //else
            //{
            //    // TODO: handle this repeatable as repeatable selector
            //}
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