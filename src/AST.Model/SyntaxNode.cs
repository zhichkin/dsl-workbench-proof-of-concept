using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OneCSharp.AST.Model
{
    public interface IIdentifiable
    {
        string Identifier { get; set; }
    }
    public interface ISyntaxNode
    {
        ISyntaxNode Parent { get; set; }
    }
    [Description("Language concepts")]
    public abstract class SyntaxNode : ISyntaxNode
    {
        public ISyntaxNode Parent { get; set; }
    }
}

#region " Syntax node children "

//private readonly List<ISyntaxNode> _children = new List<ISyntaxNode>();

//public IEnumerable<ISyntaxNode> Children { get { return _children; } }
//public void AddChild(ISyntaxNode child)
//{
//    if (child == null) throw new ArgumentNullException(nameof(child));

//    child.Parent = this;
//    _children.Add(child);
//}
//public void RemoveChild(ISyntaxNode child)
//{
//    if (child == null) throw new ArgumentNullException(nameof(child));

//    child.Parent = null;
//    _children.Remove(child);
//}
//public void ReplaceChild(ISyntaxNode child, ISyntaxNode replacer)
//{
//    if (child == null) throw new ArgumentNullException(nameof(child));
//    if (replacer == null) throw new ArgumentNullException(nameof(replacer));

//    child.Parent = null;
//    replacer.Parent = this;
//    int index = _children.IndexOf(child);
//    if (index == -1)
//    {
//        _children.Add(replacer);
//    }
//    else
//    {
//        _children[index] = replacer;
//    }
//}

#endregion