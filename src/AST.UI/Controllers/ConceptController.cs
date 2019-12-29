using OneCSharp.AST.Model;
using OneCSharp.Core;
using OneCSharp.MVVM;
using System;
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
                MenuItemHeader = "Edit element...",
                MenuItemPayload = treeNode,
                MenuItemCommand = new RelayCommand(OpenInEditor),
                MenuItemIcon = new BitmapImage(new Uri(Module.EDIT_WINDOW)),
            });
        }
        private void OpenInEditor(object parameter)
        {
            TreeNodeViewModel treeNode = (TreeNodeViewModel)parameter;
            Concept element = (Concept)treeNode.NodePayload;

            SyntaxNode rootElement = new SyntaxNode(null, element);
            SyntaxNodeLine line = new SyntaxNodeLine(rootElement);
            KeywordNode keyword1 = new KeywordNode(rootElement) { Keyword = "SELECT" };
            KeywordNode keyword2 = new KeywordNode(rootElement) { Keyword = "FROM" };
            KeywordNode keyword3 = new KeywordNode(rootElement) { Keyword = "WHERE" };
            line.Nodes.Add(keyword1);
            line.Nodes.Add(keyword2);
            line.Nodes.Add(keyword3);
            rootElement.Lines.Add(line);

            CodeEditor editor = new CodeEditor()
            {
                DataContext = rootElement
            };
            _module.Shell.AddTabItem(element.Name, editor);
        }
    }
}