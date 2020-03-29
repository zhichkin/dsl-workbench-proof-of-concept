using OneCSharp.AST.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxTreeController
    {
        private readonly Dictionary<Type, IConceptLayout> _layouts = new Dictionary<Type, IConceptLayout>();
        private SyntaxTreeController()
        {
            InitializeLayouts();
        }
        public static SyntaxTreeController Current { get; } = new SyntaxTreeController();
        private void InitializeLayouts()
        {
            //_layouts.Add(typeof(FunctionConcept), new FunctionConceptLayout());
            //_layouts.Add(typeof(ParameterConcept), new ParameterConceptLayout());
            //_layouts.Add(typeof(VariableConcept), new VariableConceptLayout());
            //_layouts.Add(typeof(SelectConcept), new SelectConceptLayout());
            //_layouts.Add(typeof(SelectExpression), new SelectExpressionLayout());
            //_layouts.Add(typeof(FromConcept), new FromConceptLayout());
            //_layouts.Add(typeof(WhereConcept), new WhereConceptLayout());
            //_layouts.Add(typeof(TableConcept), new TableConceptLayout());
        }
        public void RegisterConceptLayout(Type conceptType, IConceptLayout conceptLayout)
        {
            if (conceptType == null) throw new ArgumentNullException(nameof(conceptType));
            if (conceptLayout == null) throw new ArgumentNullException(nameof(conceptLayout));
            _layouts.Add(conceptType, conceptLayout);
        }
        private IConceptLayout GetLayout(ISyntaxNode concept)
        {
            if (concept == null) throw new ArgumentNullException(nameof(concept));
            Type type = concept.GetType();
            if (_layouts.TryGetValue(type, out IConceptLayout layout))
            {
                return layout;
            }
            return null;
        }
        public ConceptNodeViewModel CreateSyntaxNode(ISyntaxNodeViewModel parentNode, ISyntaxNode model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            IConceptLayout layout = GetLayout(model);
            if (layout == null) return null;

            ConceptNodeViewModel node = layout.Layout(model) as ConceptNodeViewModel;
            node.Owner = parentNode;
            InitializeConceptViewModel(node);

            return node;
        }
        private void InitializeConceptViewModel(ISyntaxNodeViewModel conceptViewModel)
        {
            foreach (ICodeLineViewModel line in conceptViewModel.Lines)
            {
                // PropertyViewModel does not have Lines collection
                InitializeChildrenSyntaxNodes(conceptViewModel, line.Nodes);
            }
        }
        private void InitializeChildrenSyntaxNodes(ISyntaxNodeViewModel conceptViewModel, ObservableCollection<ISyntaxNodeViewModel> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                ISyntaxNodeViewModel currentNode = nodes[i];
                if (currentNode is ConceptNodeViewModel) // recursion
                {
                    ISyntaxNode concept = GetConceptFromPropertyBinding(currentNode);
                    if (concept == null)
                    {
                        continue;
                    }
                    string propertyBinding = currentNode.PropertyBinding;
                    currentNode = CreateSyntaxNode(conceptViewModel, concept);
                    if (currentNode != null)
                    {
                        // replace layout node with newly created initialized node
                        nodes[i] = currentNode;
                        currentNode.PropertyBinding = propertyBinding;
                    }
                }
                else if (currentNode is PropertyViewModel)
                {
                    InitializeChildrenSyntaxNodes(currentNode, ((PropertyViewModel)currentNode).Nodes);
                }
                else if (currentNode is RepeatableViewModel)
                {
                    InitializeRepeatableViewModel(currentNode);
                }
                else if (currentNode is SelectorViewModel)
                {
                    InitializeSelectorViewModel(currentNode);
                }
            }
        }
        private ISyntaxNode GetConceptFromPropertyBinding(ISyntaxNodeViewModel conceptViewModel)
        {
            if (conceptViewModel.Owner == null)
            {
                return conceptViewModel.SyntaxNode; // it is syntax tree root node
            }
            ISyntaxNodeViewModel parentNode = conceptViewModel.Owner;
            string propertyBinding = conceptViewModel.PropertyBinding;
            ISyntaxNode parentConcept = parentNode.SyntaxNode;

            PropertyInfo property = parentConcept.GetPropertyInfo(propertyBinding);
            IOptional optional = null;
            if (property.IsOptional())
            {
                optional = (IOptional)property.GetValue(parentConcept);
            }
            ISyntaxNode concept = null;
            if (optional == null)
            {
                concept = (ISyntaxNode)property.GetValue(parentConcept);
            }
            else if (optional.HasValue)
            {
                concept = (ISyntaxNode)optional.Value;
            }
            return concept;
        }
        private void InitializeRepeatableViewModel(ISyntaxNodeViewModel repeatableNode)
        {
            ISyntaxNodeViewModel parentNode = repeatableNode.Owner;
            ISyntaxNode concept = parentNode.SyntaxNode;
            string propertyBinding = repeatableNode.PropertyBinding;

            IList conceptChildren;
            PropertyInfo property = concept.GetPropertyInfo(propertyBinding);
            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(concept);
                if (optional == null || !optional.HasValue)
                {
                    return;
                }
                conceptChildren = (IList)optional.Value;
            }
            else
            {
                conceptChildren = (IList)property.GetValue(concept);
            }
            if (conceptChildren == null || conceptChildren.Count == 0)
            {
                return;
            }
            foreach (var child in conceptChildren)
            {
                if (!(child is SyntaxNode))
                {
                    continue;
                }
                ConceptNodeViewModel conceptViewModel = CreateSyntaxNode(repeatableNode, (ISyntaxNode)child);
                if (conceptViewModel == null)
                {
                    continue;
                }
                repeatableNode.Add(conceptViewModel);
            }
        }
        private void InitializeSelectorViewModel(ISyntaxNodeViewModel selectorViewModel)
        {
            ISyntaxNodeViewModel parentNode = selectorViewModel.Owner;
            string propertyBinding = selectorViewModel.PropertyBinding;
            ISyntaxNode parentConcept = parentNode.SyntaxNode;
            PropertyInfo property = parentConcept.GetPropertyInfo(propertyBinding);

            IOptional optional = null;
            if (property.IsOptional())
            {
                optional = (IOptional)property.GetValue(parentConcept);
            }
            object propertyValue = null;
            if (optional == null)
            {
                propertyValue = property.GetValue(parentConcept);
            }
            else if (optional.HasValue)
            {
                propertyValue = optional.Value;
            }
            if (propertyValue is Assembly)
            {
                selectorViewModel.SyntaxNode = parentConcept;
                selectorViewModel.SyntaxNodeType = null;
            }
            else if (propertyValue is Type)
            {
                selectorViewModel.SyntaxNode = null;
                selectorViewModel.SyntaxNodeType = (Type)propertyValue;
            }
            else if (propertyValue is SyntaxNode)
            {
                selectorViewModel.SyntaxNode = (ISyntaxNode)propertyValue;
                selectorViewModel.SyntaxNodeType = null;
            }
            else
            {
                selectorViewModel.SyntaxNode = null;
                selectorViewModel.SyntaxNodeType = null;
            }
            _ = ((SelectorViewModel)selectorViewModel).Presentation;
        }
    }
}