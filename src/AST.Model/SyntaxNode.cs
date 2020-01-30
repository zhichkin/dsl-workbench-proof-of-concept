using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public interface IIdentifiable
    {
        string Identifier { get; set; }
    }
    public interface ISyntaxNode
    {
        ISyntaxNode Parent { get; set; }
        IEnumerable<ISyntaxNode> Children { get; }
        void AddChild(ISyntaxNode child);
        void RemoveChild(ISyntaxNode child);
        void ReplaceChild(ISyntaxNode child, ISyntaxNode replacer);
    }
    public abstract class SyntaxNode : ISyntaxNode
    {
        private readonly List<ISyntaxNode> _children = new List<ISyntaxNode>();
        public ISyntaxNode Parent { get; set; }
        public IEnumerable<ISyntaxNode> Children { get { return _children; } }
        public void AddChild(ISyntaxNode child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));

            child.Parent = this;
            _children.Add(child);
        }
        public void RemoveChild(ISyntaxNode child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));

            child.Parent = null;
            _children.Remove(child);
        }
        public void ReplaceChild(ISyntaxNode child, ISyntaxNode replacer)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (replacer == null) throw new ArgumentNullException(nameof(replacer));

            child.Parent = null;
            replacer.Parent = this;
            int index = _children.IndexOf(child);
            if (index == -1)
            {
                _children.Add(replacer);
            }
            else
            {
                _children[index] = replacer;
            }
        }
    }
}