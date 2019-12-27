using OneCSharp.AST.Model;
using OneCSharp.Core;
using OneCSharp.MVVM;
using System;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxElementController : IController
    {
        private readonly IModule _module;
        public SyntaxElementController(IModule module)
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
            SyntaxElement element = (SyntaxElement)treeNode.NodePayload;

            ConceptViewModel rootElement = new ConceptViewModel(null);
            LineViewModel line = new LineViewModel(rootElement);
            KeywordViewModel keyword1 = new KeywordViewModel(rootElement) { Keyword = "SELECT" };
            KeywordViewModel keyword2 = new KeywordViewModel(rootElement) { Keyword = "FROM" };
            KeywordViewModel keyword3 = new KeywordViewModel(rootElement) { Keyword = "WHERE" };
            line.Items.Add(keyword1);
            line.Items.Add(keyword2);
            line.Items.Add(keyword3);
            rootElement.Lines.Add(line);

            ConceptView editor = new ConceptView();
            editor.DataContext = rootElement;

            _module.Shell.AddTabItem(element.Name, editor);
        }
    }
}