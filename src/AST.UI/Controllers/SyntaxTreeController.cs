using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxTreeController
    {
        private const string ADD_PROPERTY = "pack://application:,,,/OneCSharp.AST.UI;component/images/AddProperty.png";
        private readonly Dictionary<Type, IConceptLayout> _layouts = new Dictionary<Type, IConceptLayout>();
        public SyntaxTreeController()
        {
            InitializeLayouts();
        }
        private void InitializeLayouts()
        {
            _layouts.Add(typeof(FunctionConcept), new FunctionConceptLayout());
            _layouts.Add(typeof(ParameterConcept), new ParameterConceptLayout());
            _layouts.Add(typeof(VariableConcept), new VariableConceptLayout());
            _layouts.Add(typeof(SelectConcept), new SelectConceptLayout());
            _layouts.Add(typeof(SelectExpression), new SelectExpressionLayout());
            _layouts.Add(typeof(FromConcept), new FromConceptLayout());
            _layouts.Add(typeof(WhereConcept), new WhereConceptLayout());
            _layouts.Add(typeof(TableConcept), new TableConceptLayout());
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
                    if (line.Nodes[i] is ConceptNodeViewModel)
                    {
                        string propertyBinding = line.Nodes[i].PropertyBinding;
                        PropertyInfo property = node.SyntaxNode.GetPropertyInfo(propertyBinding);
                        IOptional optional = null;
                        if (property.IsOptional())
                        {
                            optional = (IOptional)property.GetValue(node.SyntaxNode);
                        }
                        ISyntaxNode concept = null;
                        if (optional == null)
                        {
                            concept = (ISyntaxNode)property.GetValue(node.SyntaxNode);
                        }
                        else if (optional.HasValue)
                        {
                            concept = (ISyntaxNode)optional.Value;
                        }
                        if (concept != null)
                        {
                            ConceptNodeViewModel conceptNode = CreateSyntaxNode(node, concept);
                            if (conceptNode != null)
                            {
                                line.Nodes[i] = conceptNode;
                                conceptNode.PropertyBinding = propertyBinding;
                            }
                        }
                    }
                }
            }
            return node;
        }


        
        [Obsolete] private void CreateContextMenu(KeywordNodeViewModel node, ISyntaxNode model)
        {
            Type metadata = model.GetType();

            foreach (PropertyInfo property in metadata.GetProperties())
            {
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(Optional<>))
                {
                    IOptional propertyValue = (IOptional)property.GetValue(model);
                    if (!propertyValue.HasValue)
                    {
                        //propertyValue.Value
                        node.ContextMenu.Add(new MenuItemViewModel()
                        {
                            MenuItemHeader = $"Add {property.Name}",
                            MenuItemPayload = (node.Owner, property.Name), // ValueTuple<ISyntaxNodeViewModel, string>
                            MenuItemCommand = new RelayCommand(ShowSyntaxNode),
                            MenuItemIcon = new BitmapImage(new Uri(ADD_PROPERTY)),
                        });
                    }
                }
                else if (property.IsRepeatable())
                {
                    node.ContextMenu.Add(new MenuItemViewModel()
                    {
                        MenuItemHeader = $"Add {property.Name}",
                        MenuItemPayload = (node.Owner, property.Name), // ValueTuple<ISyntaxNodeViewModel, string>
                        MenuItemCommand = new RelayCommand(ShowSyntaxNode),
                        MenuItemIcon = new BitmapImage(new Uri(ADD_PROPERTY)),
                    });
                }
            }
            node.IsContextMenuEnabled = (node.ContextMenu.Count > 0);
        }
        [Obsolete] private void ShowSyntaxNode(object parameter)
        {
            ValueTuple<ISyntaxNodeViewModel, string> tuple = (ValueTuple<ISyntaxNodeViewModel, string>)parameter;
            if (!(tuple.Item1 is ConceptNodeViewModel node)) return;
            node.ShowSyntaxNodes(tuple.Item2);

            PropertyInfo property = node.SyntaxNode.GetPropertyInfo(tuple.Item2);
            if (property == null) return;
            if (property.IsRepeatable())
            {
                List<Type> types = property.GetRepeatableTypes();
                if (types.Count == 1)
                {
                    CreateConceptCommand command = new CreateConceptCommand();
                    ISyntaxNode model = command.Create(node.SyntaxNode, property.Name, types[0]);
                    ConceptNodeViewModel child = CreateSyntaxNode(node, model);
                    List<ISyntaxNodeViewModel> list = node.GetNodesByPropertyName(property.Name);
                    if (list != null && list.Count == 1)
                    {
                        list[0].Add(child);
                    }
                }
            }
        }
    }
}