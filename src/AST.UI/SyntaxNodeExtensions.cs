using System;
using System.Collections.Generic;

namespace OneCSharp.AST.UI
{
    public static class SyntaxNodeExtensions
    {
        public static ISyntaxNodeViewModel Ancestor<T>(this ISyntaxNodeViewModel node)
        {
            Type ancestorType = typeof(T);
            ISyntaxNodeViewModel ancestor = node.Owner;
            while (ancestor != null || ancestor.GetType() != ancestorType)
            {
                ancestor = ancestor.Owner;
            }
            return ancestor;
        }
        public static ICodeLineViewModel BottomCodeLine(this ConceptNodeViewModel node)
        {
            ICodeLineViewModel codeLine;
            int count = node.Lines.Count;
            if (count == 0)
            {
                codeLine = new CodeLineViewModel(node);
                node.Lines.Add(codeLine);
            }
            else
            {
                codeLine = node.Lines[count - 1];
            }
            return codeLine;
        }
        public static ConceptNodeViewModel Name(this ConceptNodeViewModel node)
        {
            ICodeLineViewModel codeLine = node.BottomCodeLine();
            ISyntaxNodeViewModel ancestor = node.Ancestor<ConceptNodeViewModel>();
            codeLine.Nodes.Add(new NameNodeViewModel(node, ancestor.Model));
            return node;
        }
        public static ConceptNodeViewModel Literal(this ConceptNodeViewModel node, string literal)
        {
            if (string.IsNullOrWhiteSpace(literal)) throw new ArgumentNullException(nameof(literal));

            ICodeLineViewModel codeLine = node.BottomCodeLine();
            codeLine.Nodes.Add(new LiteralNodeViewModel(node) { Literal = literal });
            return node;
        }
        public static ConceptNodeViewModel Keyword(this ConceptNodeViewModel node, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentNullException(nameof(keyword));

            ICodeLineViewModel codeLine = node.BottomCodeLine();
            codeLine.Nodes.Add(new KeywordNodeViewModel(node) { Keyword = keyword });
            return node;
        }
    }
}