using System.Collections.ObjectModel;

namespace OneCSharp.OQL.Model
{
    public interface ISyntaxNode
    {
        ISyntaxNode Parent { get; set; }
        // TODO: add functions to add and remove children, so as children could ask parent to remove them from parent's collections
    }
    public abstract class SyntaxNode : ISyntaxNode
    {
        protected ISyntaxNode _parent = null;
        public SyntaxNode() { }
        public SyntaxNode(ISyntaxNode parent) { _parent = parent; }
        public ISyntaxNode Parent { get { return _parent; } set { _parent = value; } }
    }
    public class SyntaxNodes : ObservableCollection<ISyntaxNode>, ISyntaxNode
    {
        protected ISyntaxNode _parent = null;
        public SyntaxNodes() { }
        public SyntaxNodes(ISyntaxNode parent) { _parent = parent; }
        public ISyntaxNode Parent { get { return _parent; } set { _parent = value; } }
    }
    public interface IKeyword
    {
        string Keyword { get; }
    }
    public interface IOperator
    {
        string Literal { get; }
    }
}
