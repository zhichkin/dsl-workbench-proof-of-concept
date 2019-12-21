using OneCSharp.Core;

namespace OneCSharp.MVVM
{
    public interface IController
    {
        void BuildTreeNode(Entity model, out TreeNodeViewModel treeNode);
    }
}