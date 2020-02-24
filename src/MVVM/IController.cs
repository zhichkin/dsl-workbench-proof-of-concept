namespace OneCSharp.MVVM
{
    public interface IController
    {
        void BuildTreeNode(object model, out TreeNodeViewModel treeNode);
    }
}