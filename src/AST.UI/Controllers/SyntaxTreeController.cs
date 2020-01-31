using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxTreeController
    {
        private const string ADD_PROPERTY = "pack://application:,,,/OneCSharp.AST.UI;component/images/AddProperty.png";
        public ConceptNodeViewModel CreateSyntaxNode(ISyntaxNodeViewModel parentNode, ISyntaxNode model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            IConceptLayout layout = new FunctionConceptLayout();
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
        }
    }
}