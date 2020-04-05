namespace OneCSharp.MVVM
{
    public interface IController
    {
        void AttachTreeNodes(TreeNodeViewModel parentNode);
        void BuildTreeNode(object model, out TreeNodeViewModel treeNode);
    }
}