using OneCSharp.AST.Model;
using OneCSharp.Core.Model;
using OneCSharp.MVVM;
using System;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class LanguageController : IController
    {
        private readonly IModule _module;
        public LanguageController(IModule module)
        {
            _module = module;
        }
        public void BuildTreeNode(Entity model, out TreeNodeViewModel treeNode)
        {
            treeNode = new TreeNodeViewModel()
            {
                NodeText = model.Name,
                NodePayload = model,
                NodeIcon = new BitmapImage(new Uri(Module.SOLUTION))
            };
            BuildContextMenu(treeNode);
        }
        public void BuildContextMenu(TreeNodeViewModel treeNode)
        {
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add namespace...",
                MenuItemPayload = treeNode,
                MenuItemCommand = new RelayCommand(AddNamespace),
                MenuItemIcon = new BitmapImage(new Uri(Module.ADD_NAMESPACE)),
            });
        }

        private void AddNamespace(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return; }

            TreeNodeViewModel treeNode = (TreeNodeViewModel)parameter;
            Language parent = (Language)treeNode.NodePayload;
            Namespace child = new Namespace()
            {
                Name = (string)dialog.Result
            };
            child.Owner = parent;
            parent.Namespaces.Add(child);

            IController controller = _module.GetController<Namespace>();
            controller.BuildTreeNode(child, out TreeNodeViewModel childNode);
            
            _module.Persist(parent);
            treeNode.TreeNodes.Add(childNode);
        }
    }
}