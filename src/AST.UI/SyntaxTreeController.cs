using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxTreeController
    {
        private const string ADD_PROPERTY = "pack://application:,,,/OneCSharp.AST.UI;component/images/AddProperty.png";
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

            foreach (ICodeLineViewModel line in node.Lines)
            {
                for (int i = 0; i < line.Nodes.Count; i++)
                {
                    ISyntaxNodeViewModel currentNode = line.Nodes[i];
                    if (currentNode is ConceptNodeViewModel)
                    {
                        InitializeConceptViewModel(currentNode);
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
            return node;
        }
        private void InitializeConceptViewModel(ISyntaxNodeViewModel conceptViewModel)
        {
            ISyntaxNodeViewModel parentNode = conceptViewModel.Owner;
            string propertyBinding = conceptViewModel.PropertyBinding;
            ISyntaxNode parentConcept = parentNode.SyntaxNode;

            PropertyInfo property = parentNode.SyntaxNode.GetPropertyInfo(propertyBinding);
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
            if (concept != null)
            {
                ConceptNodeViewModel conceptNode = CreateSyntaxNode(parentNode, concept);
                if (conceptNode != null)
                {
                    conceptViewModel = conceptNode; // TODO: why I did it like that ???
                    conceptNode.PropertyBinding = propertyBinding;
                }
            }
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
            //TODO
        }
    }
}