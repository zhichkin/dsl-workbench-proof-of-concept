using OneCSharp.Core.Model;
using System;
using System.Windows.Media.Imaging;

namespace OneCSharp.MVVM
{
    public sealed class NamespaceController : IController
    {
        public const string NAMESPACE = "pack://application:,,,/OneCSharp.MVVM;component/images/Namespace.png";
        public const string ADD_NAMESPACE = "pack://application:,,,/OneCSharp.MVVM;component/images/AddNamespace.png";
        public NamespaceController() { }
        public void BuildTreeNode(Entity model, out TreeNodeViewModel treeNode)
        {
            treeNode = new TreeNodeViewModel()
            {
                NodeText = model.Name,
                NodePayload = model,
                NodeIcon = new BitmapImage(new Uri(NAMESPACE))
            };
            BuildContextMenu(treeNode);
        }
        public void BuildContextMenu(TreeNodeViewModel treeNode)
        {
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add namespace",
                MenuItemPayload = treeNode,
                MenuItemCommand = new RelayCommand(AddNamespaceNode),
                MenuItemIcon = new BitmapImage(new Uri(ADD_NAMESPACE)),
            });
        }
        private void AddNamespaceNode(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return; }

            TreeNodeViewModel treeNode = (TreeNodeViewModel)parameter;

            Namespace child = new Namespace()
            {
                Name = (string)dialog.Result
            };
            if (treeNode.NodePayload is Domain domain)
            {
                child.Owner = domain;
                domain.Namespaces.Add(child);
            }
            else if (treeNode.NodePayload is Namespace parent)
            {
                child.Owner = parent;
                parent.Namespaces.Add(child);
            }
            BuildTreeNode(child, out TreeNodeViewModel childNode);
            treeNode.TreeNodes.Add(childNode);
        }
        public TreeNodeViewModel CreateNamespaceNode(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return null; }

            TreeNodeViewModel treeNode = (TreeNodeViewModel)parameter;

            Namespace child = new Namespace()
            {
                Name = (string)dialog.Result
            };
            if (treeNode.NodePayload is Domain domain)
            {
                child.Owner = domain;
                domain.Namespaces.Add(child);
            }
            else if (treeNode.NodePayload is Namespace parent)
            {
                child.Owner = parent;
                parent.Namespaces.Add(child);
            }
            BuildTreeNode(child, out TreeNodeViewModel childNode);
            treeNode.TreeNodes.Add(childNode);

            return childNode;
        }
    }
}