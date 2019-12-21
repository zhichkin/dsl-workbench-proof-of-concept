namespace OneCSharp.MVVM
{
    public interface IShell
    {
        IService GetService<IService>();
        void AddMenuItem(MenuItemViewModel menuItem);
        void AddTreeNode(TreeNodeViewModel treeNode);
        void AddTabItem(string header, object content);
        void RemoveTabItem(TabViewModel tab);
    }
}