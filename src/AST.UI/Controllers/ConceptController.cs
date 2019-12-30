using OneCSharp.AST.Model;
using OneCSharp.Core;
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
                DataContext = BuildConceptSyntaxNode(element)
            };
            _module.Shell.AddTabItem(element.Name, editor);
        }
        
        
        
        private SyntaxNode BuildConceptSyntaxNode(Concept element)
        {
            SyntaxNode rootElement = new SyntaxNode(null, element);
            
            SyntaxNodeLine line1 = new SyntaxNodeLine(rootElement);
            if (element.IsAncestor)
            {
                line1.Nodes.Add(new KeywordNode(rootElement) { Keyword = "ROOT" });
            }
            var node1 = new KeywordNode(rootElement) { Keyword = "CONCEPT" };
            var node2 = new NameNode(rootElement, element);
            line1.Nodes.Add(node1);
            line1.Nodes.Add(node2);
            rootElement.Lines.Add(line1);

            SyntaxNodeLine line2 = new SyntaxNodeLine(rootElement);
            var node3 = new LiteralNode(rootElement) { Literal = "{" };
            line2.Nodes.Add(node3);
            rootElement.Lines.Add(line2);

            SyntaxNodeLine line3 = new SyntaxNodeLine(rootElement);
            var node4 = new IndentNode(rootElement);
            var node5 = new NameNode(rootElement, element);
            var node6 = new KeywordNode(rootElement) { Keyword = " : " };
            line3.Nodes.Add(node4);
            line3.Nodes.Add(node5);
            line3.Nodes.Add(node6);
            rootElement.Lines.Add(line3);

            SyntaxNodeLine line4 = new SyntaxNodeLine(rootElement);
            var node7 = new LiteralNode(rootElement) { Literal = "}" };
            line4.Nodes.Add(node7);
            rootElement.Lines.Add(line4);

            BuildConceptContextMenu(node1);

            return rootElement;
        }
        private void BuildConceptContextMenu(KeywordNode node)
        {
            node.ContextMenu.Clear();

            if (node.Owner.Model.IsAncestor)
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
        }
        private void RootConceptCommand(object parameter)
        {
            KeywordNode node = (KeywordNode)parameter;
            node.Owner.Model.IsAncestor = true;

            foreach (SyntaxNodeLine line in node.Owner.Lines)
            {
                if (line.Nodes.Contains(node))
                {
                    var existingNode = line.Nodes.Where(node =>
                        node.GetType() == typeof(KeywordNode)
                        && ((KeywordNode)node).Keyword == "ROOT").FirstOrDefault();
                    if (existingNode == null)
                    {
                        var newNode = new KeywordNode(node.Owner) { Keyword = "ROOT" };
                        line.Nodes.Insert(0, newNode);
                        _module.Persist(node.Owner.Model.Namespace.Owner);
                        BuildConceptContextMenu(node);
                    }
                    break;
                }
            }
        }
        private void UnRootConceptCommand(object parameter)
        {
            KeywordNode node = (KeywordNode)parameter;

            node.Owner.Model.IsAncestor = false;
            foreach (SyntaxNodeLine line in node.Owner.Lines)
            {
                if (line.Nodes.Contains(node))
                {
                    var existingNode = line.Nodes.Where(node =>
                        node.GetType() == typeof(KeywordNode)
                        && ((KeywordNode)node).Keyword == "ROOT").FirstOrDefault();
                    if (existingNode != null)
                    {
                        line.Nodes.Remove(existingNode);
                        _module.Persist(node.Owner.Model.Namespace.Owner);
                        BuildConceptContextMenu(node);
                    }
                    break;
                }
            }
        }
    }
}