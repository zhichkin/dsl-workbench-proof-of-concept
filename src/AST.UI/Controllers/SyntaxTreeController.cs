using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxTreeController
    {
        private const string ADD_PROPERTY = "pack://application:,,,/OneCSharp.AST.UI;component/images/AddProperty.png";
        private IConceptLayout GetLayout(ISyntaxNode model)
        {
            if (model is FunctionConcept)
            {
                return new FunctionConceptLayout();
            }
            else if (model is ParameterConcept)
            {
                return new ParameterConceptLayout();
            }
            else if (model is SelectConcept)
            {
                return new SelectConceptLayout();
            }
            else if (model is SelectExpression)
            {
                return new SelectExpressionLayout();
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

            if (node.Lines.Count > 0)
            {
                KeywordNodeViewModel keyword = (KeywordNodeViewModel)node.Lines[0].Nodes
                    .Where(n => n.GetType() == typeof(KeywordNodeViewModel))
                    .FirstOrDefault();
                if (keyword != null)
                {
                    CreateContextMenu(keyword, model);
                }
            }

            return node;
        }
        private void CreateContextMenu(KeywordNodeViewModel node, ISyntaxNode model)
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
            }
            node.IsContextMenuEnabled = (node.ContextMenu.Count > 0);
        }
        private void ShowSyntaxNode(object parameter)
        {
            ValueTuple<ISyntaxNodeViewModel, string> tuple = (ValueTuple<ISyntaxNodeViewModel, string>)parameter;
            if (!(tuple.Item1 is ConceptNodeViewModel node)) return;
            node.ShowSyntaxNodes(tuple.Item2);

            PropertyInfo property = node.Model.GetPropertyInfo(tuple.Item2);
            if (property == null) return;
            if (property.IsRepeatable())
            {
                List<Type> types = property.GetRepeatableTypes();
                if (types.Count == 1)
                {
                    CreateConceptCommand command = new CreateConceptCommand();
                    ISyntaxNode model = command.Create(node.Model, property.Name, types[0]);
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