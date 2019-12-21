using OneCSharp.AST.Model;
using OneCSharp.Core;
using OneCSharp.MVVM;
using System;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class NamespaceController : IController
    {
        private readonly IModule _module;
        public NamespaceController(IModule module)
        {
            _module = module;
        }
        public void BuildTreeNode(Entity model, out TreeNodeViewModel treeNode)
        {
            treeNode = new TreeNodeViewModel()
            {
                NodeText = model.Name,
                NodePayload = model,
                NodeIcon = new BitmapImage(new Uri(Module.NAMESPACE_PUBLIC))
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

            Namespace child = new Namespace()
            {
                Name = (string)dialog.Result
            };
            if (treeNode.NodePayload is Language language)
            {
                language.Add(child);
            }
            else if (treeNode.NodePayload is Namespace parent)
            {
                parent.Add(child);
            }

            IController controller = _module.GetController<Namespace>();
            controller.BuildTreeNode(child, out TreeNodeViewModel childNode);

            _module.Persist(child.Domain);
            treeNode.TreeNodes.Add(childNode);
        }
    }
}