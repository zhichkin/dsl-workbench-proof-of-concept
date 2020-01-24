using OneCSharp.AST.Model;
using OneCSharp.Core.Model;
using OneCSharp.MVVM;
using System;
using System.Linq;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class ConceptController : IController
    {
        private readonly IModule _module;
        public ConceptController(IModule module)
        {
            _module = module;
        }

        #region " Metadata tree "
        public void BuildTreeNode(Entity model, out TreeNodeViewModel treeNode)
        {
            treeNode = new TreeNodeViewModel()
            {
                NodeText = model.Name,
                NodePayload = model,
                NodeIcon = new BitmapImage(new Uri(Module.CS_FILE))
            };
            BuildContextMenu(treeNode);
        }
        public void BuildContextMenu(TreeNodeViewModel treeNode)
        {
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Edit concept...",
                MenuItemPayload = treeNode,
                MenuItemCommand = new RelayCommand(OpenInEditor),
                MenuItemIcon = new BitmapImage(new Uri(Module.EDIT_WINDOW)),
            });
        }
        private void OpenInEditor(object parameter)
        {
            TreeNodeViewModel treeNode = (TreeNodeViewModel)parameter;
            Concept element = (Concept)treeNode.NodePayload;
            CodeEditor editor = new CodeEditor()
            {
                DataContext = BuildConceptSyntaxNodeViewModel(element)
            };
            _module.Shell.AddTabItem(element.Name, editor);
        }
        #endregion

        #region " Syntax editor "
        private SyntaxNodeViewModel BuildConceptSyntaxNodeViewModel(Concept concept)
        {
            SyntaxNodeViewModel rootElement = new ConceptNodeViewModel(null, concept);
            
            CodeLineViewModel line1 = new CodeLineViewModel(rootElement);
            if (concept.IsAncestor)
            {
                KeywordNodeViewModel keyword = new KeywordNodeViewModel(rootElement)
                {
                    Keyword = "ROOT",
                    IsContextMenuEnabled = false
                };
                line1.Nodes.Add(keyword);
            }
            var node1 = new KeywordNodeViewModel(rootElement, concept) { Keyword = "CONCEPT" };
            var node2 = new NameNodeViewModel(rootElement, concept);
            line1.Nodes.Add(node1);
            line1.Nodes.Add(node2);
            rootElement.Lines.Add(line1);

            CodeLineViewModel line2 = new CodeLineViewModel(rootElement);
            var node3 = new LiteralNodeViewModel(rootElement) { Literal = "{" };
            line2.Nodes.Add(node3);
            rootElement.Lines.Add(line2);

            foreach (Property property in concept.Properties)
            {
                BuildConceptProperty(rootElement, property);
            }

            CodeLineViewModel line3 = new CodeLineViewModel(rootElement);
            var node4 = new LiteralNodeViewModel(rootElement) { Literal = "}" };
            line3.Nodes.Add(node4);
            rootElement.Lines.Add(line3);

            BuildConceptContextMenu(node1);

            return rootElement;
        }
        private void BuildConceptProperty(ISyntaxNodeViewModel rootElement, Property property)
        {
            int lineIndex = 0;
            ISyntaxNodeViewModel closingBracket = null;
            for (lineIndex = 0; lineIndex < rootElement.Lines.Count; lineIndex++)
            {
                ICodeLineViewModel line = rootElement.Lines[lineIndex];
                closingBracket = line.Nodes.Where(node => node.GetType() == typeof(LiteralNodeViewModel) && ((LiteralNodeViewModel)node).Literal == "}").FirstOrDefault();
                if (closingBracket != null)
                {
                    break;
                }
            }
            if (lineIndex == 0) return;
            
            CodeLineViewModel newLine = new CodeLineViewModel(rootElement);
            var node1 = new IndentNodeViewModel(rootElement);
            var node2 = new NameNodeViewModel(rootElement, property);
            var node3 = new KeywordNodeViewModel(rootElement, property) { Keyword = " : " };
            newLine.Nodes.Add(node1);
            newLine.Nodes.Add(node2);
            newLine.Nodes.Add(node3);
            rootElement.Lines.Insert(lineIndex, newLine);

            //TODO: build context menu for the property
        }
        private void BuildConceptContextMenu(KeywordNodeViewModel node)
        {
            Concept concept = (Concept)node.Owner.Model;

            node.ContextMenu.Clear();

            if (concept.IsAncestor)
            {
                node.ContextMenu.Add(new MenuItemViewModel()
                {
                    MenuItemHeader = "ROOT = false",
                    MenuItemPayload = node,
                    MenuItemCommand = new RelayCommand(UnRootConceptCommand),
                    MenuItemIcon = new BitmapImage(new Uri(Module.DELETE_PROPERTY)),
                });
            }
            else
            {
                node.ContextMenu.Add(new MenuItemViewModel()
                {
                    MenuItemHeader = "ROOT = true",
                    MenuItemPayload = node,
                    MenuItemCommand = new RelayCommand(RootConceptCommand),
                    MenuItemIcon = new BitmapImage(new Uri(Module.ADD_PROPERTY)),
                });
            }
            node.ContextMenu.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "ADD PROPERTY",
                MenuItemPayload = node,
                MenuItemCommand = new RelayCommand(AddConceptPropertyCommand),
                MenuItemIcon = new BitmapImage(new Uri(Module.ADD_PROPERTY)),
            });
        }
        private void RootConceptCommand(object parameter)
        {
            KeywordNodeViewModel node = (KeywordNodeViewModel)parameter;
            Concept concept = (Concept)node.Owner.Model;
            concept.IsAncestor = true;

            foreach (CodeLineViewModel line in node.Owner.Lines)
            {
                if (line.Nodes.Contains(node))
                {
                    var existingNode = line.Nodes.Where(node =>
                        node.GetType() == typeof(KeywordNodeViewModel)
                        && ((KeywordNodeViewModel)node).Keyword == "ROOT").FirstOrDefault();
                    if (existingNode == null)
                    {
                        var newNode = new KeywordNodeViewModel(node.Owner) { Keyword = "ROOT" };
                        line.Nodes.Insert(0, newNode);
                        _module.Persist(concept);
                        BuildConceptContextMenu(node);
                    }
                    break;
                }
            }
        }
        private void UnRootConceptCommand(object parameter)
        {
            KeywordNodeViewModel node = (KeywordNodeViewModel)parameter;
            Concept concept = (Concept)node.Owner.Model;
            concept.IsAncestor = false;

            foreach (CodeLineViewModel line in node.Owner.Lines)
            {
                if (line.Nodes.Contains(node))
                {
                    var existingNode = line.Nodes.Where(node =>
                        node.GetType() == typeof(KeywordNodeViewModel)
                        && ((KeywordNodeViewModel)node).Keyword == "ROOT").FirstOrDefault();
                    if (existingNode != null)
                    {
                        line.Nodes.Remove(existingNode);
                        _module.Persist(concept);
                        BuildConceptContextMenu(node);
                    }
                    break;
                }
            }
        }
        private void AddConceptPropertyCommand(object parameter)
        {
            KeywordNodeViewModel node = (KeywordNodeViewModel)parameter;
            Concept concept = (Concept)node.Owner.Model;
            Property property = new Property()
            {
                Name = $"Property_{concept.Properties.Count}"
            };
            property.Owner = concept;
            concept.Properties.Add(property);
            _module.Persist(concept);

            BuildConceptProperty(node.Owner, property);
        }
        #endregion
    }
}