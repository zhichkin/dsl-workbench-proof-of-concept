using System.Collections.ObjectModel;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxNodeLine : ISyntaxNodeLine
    {
        public SyntaxNodeLine(ISyntaxNode owner)
        {
            Owner = owner;
        }
        public ISyntaxNode Owner { get; }
        public ObservableCollection<ISyntaxNode> Nodes { get; } = new ObservableCollection<ISyntaxNode>();
    }
}