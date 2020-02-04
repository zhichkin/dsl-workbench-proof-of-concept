using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace OneCSharp.AST.UI
{
    public static class SyntaxNodeExtensions
    {
        public static ISyntaxNodeViewModel Ancestor<T>(this ISyntaxNodeViewModel @this)
        {
            Type ancestorType = typeof(T);
            ISyntaxNodeViewModel ancestor = @this.Owner;
            while (ancestor != null)
            {
                if (ancestor.GetType() != ancestorType)
                {
                    ancestor = ancestor.Owner;
                }
                else
                {
                    break;
                }
            }
            return ancestor;
        }
        public static ICodeLineViewModel BottomCodeLine(this ISyntaxNodeViewModel @this)
        {
            ICodeLineViewModel codeLine;
            int count = @this.Lines.Count;
            if (count == 0)
            {
                codeLine = new CodeLineViewModel(@this);
                @this.Lines.Add(codeLine);
            }
            else
            {
                codeLine = @this.Lines[count - 1];
            }
            return codeLine;
        }
        public static ISyntaxNodeViewModel LastSyntaxNode(this ISyntaxNodeViewModel @this)
        {
            ICodeLineViewModel codeLine = @this.BottomCodeLine();
            int count = codeLine.Nodes.Count;
            if (count == 0) { return null; }
            return codeLine.Nodes[count - 1];
        }
        public static ConceptNodeViewModel Bind(this ConceptNodeViewModel @this, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            ISyntaxNodeViewModel syntaxNode = @this.LastSyntaxNode();
            if (syntaxNode == null) throw new ArgumentNullException(nameof(syntaxNode));

            syntaxNode.PropertyBinding = propertyName;
            return @this;
        }
        public static List<ISyntaxNodeViewModel> GetNodesByPropertyName(this ConceptNodeViewModel @this, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            List<ISyntaxNodeViewModel> nodes = new List<ISyntaxNodeViewModel>();
            foreach (ICodeLineViewModel line in @this.Lines)
            {
                foreach (ISyntaxNodeViewModel node in line.Nodes)
                {
                    if (node.PropertyBinding == propertyName)
                    {
                        nodes.Add(node);
                    }
                }
            }
            return nodes;
        }
        public static void ShowSyntaxNodes(this ConceptNodeViewModel @this, string propertyName)
        {
            SetSyntaxNodesVisibility(@this, propertyName, true);
        }
        public static void HideSyntaxNodes(this ConceptNodeViewModel @this, string propertyName)
        {
            SetSyntaxNodesVisibility(@this, propertyName, false);
        }
        private static void SetSyntaxNodesVisibility(ConceptNodeViewModel @this, string propertyName, bool isVisible)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return;

            foreach (ISyntaxNodeViewModel node in @this.GetNodesByPropertyName(propertyName))
            {
                node.IsVisible = isVisible;
            }
        }
        public static ConceptNodeViewModel Identifier(this ConceptNodeViewModel @this)
        {
            ICodeLineViewModel codeLine = @this.BottomCodeLine();
            codeLine.Nodes.Add(new IdentifierNodeViewModel(@this, @this.Model));
            return @this;
        }
        public static ConceptNodeViewModel Literal(this ConceptNodeViewModel @this, string literal)
        {
            if (string.IsNullOrWhiteSpace(literal)) throw new ArgumentNullException(nameof(literal));

            ICodeLineViewModel codeLine = @this.BottomCodeLine();
            codeLine.Nodes.Add(new LiteralNodeViewModel(@this) { Literal = literal });
            return @this;
        }
        public static ConceptNodeViewModel Keyword(this ConceptNodeViewModel @this, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentNullException(nameof(keyword));

            ICodeLineViewModel codeLine = @this.BottomCodeLine();
            codeLine.Nodes.Add(new KeywordNodeViewModel(@this) { Keyword = keyword });
            return @this;
        }
        public static ConceptNodeViewModel NewLine(this ConceptNodeViewModel @this)
        {
            @this.Lines.Add(new CodeLineViewModel(@this));
            return @this;
        }
        public static ConceptNodeViewModel Indent(this ConceptNodeViewModel @this)
        {
            ICodeLineViewModel codeLine = @this.BottomCodeLine();
            codeLine.Nodes.Add(new IndentNodeViewModel(@this));
            return @this;
        }
        public static ConceptNodeViewModel Repeatable(this ConceptNodeViewModel @this)
        {
            ICodeLineViewModel codeLine = @this
                .NewLine()
                .BottomCodeLine();
            codeLine.Nodes.Add(new RepeatableViewModel(@this));
            return @this;
        }
        public static ConceptNodeViewModel Decorate(this ConceptNodeViewModel @this, string openingLiteral, string closingLiteral)
        {
            ISyntaxNodeViewModel syntaxNode = @this.LastSyntaxNode();
            if (syntaxNode == null) throw new ArgumentNullException(nameof(syntaxNode));
            if (!(syntaxNode is RepeatableViewModel repeatable)) return @this;

            repeatable.OpeningLiteral = openingLiteral;
            repeatable.ClosingLiteral = closingLiteral;
            return @this;
        }
        public static ConceptNodeViewModel Delimiter(this ConceptNodeViewModel @this, string delimiterLiteral)
        {
            ISyntaxNodeViewModel syntaxNode = @this.LastSyntaxNode();
            if (syntaxNode == null) throw new ArgumentNullException(nameof(syntaxNode));
            if (!(syntaxNode is RepeatableViewModel repeatable)) return @this;

            repeatable.Delimiter = delimiterLiteral;
            return @this;
        }
        public static ConceptNodeViewModel Reference(this ConceptNodeViewModel @this)
        {
            ICodeLineViewModel codeLine = @this.BottomCodeLine();
            codeLine.Nodes.Add(new ReferenceViewModel(@this));
            return @this;
        }
        public static TreeNodeViewModel BuildReferenceSelectorTree(IEnumerable<ISyntaxNode> references)
        {
            TreeNodeViewModel tree = new TreeNodeViewModel();

            Type baseType;
            TreeNodeViewModel currentNode;
            Stack<Type> stack = new Stack<Type>();
            
            foreach (ISyntaxNode node in references)
            {
                currentNode = tree;
                baseType = node.GetType().BaseType;

                while (baseType != typeof(SyntaxNode))
                {
                    stack.Push(baseType);
                    baseType = baseType.BaseType;
                }
                while (stack.Count > 0)
                {
                    baseType = stack.Pop();
                    currentNode = AddNodeToReferenceSelectorTree(currentNode, baseType);
                }
                if (currentNode != tree)
                {
                    currentNode.TreeNodes.Add(new TreeNodeViewModel()
                    {
                        NodeText = node.ToString(),
                        NodePayload = node
                    });
                }
            }

            // expand only top level nodes
            foreach (TreeNodeViewModel treeNode in tree.TreeNodes)
            {
                treeNode.IsExpanded = true;
            }

            return tree;
        }
        private static TreeNodeViewModel AddNodeToReferenceSelectorTree(TreeNodeViewModel root, Type baseType)
        {
            foreach (TreeNodeViewModel node in root.TreeNodes)
            {
                if ((Type)node.NodePayload == baseType) return node;
            }
            DescriptionAttribute attribute = baseType.GetCustomAttribute<DescriptionAttribute>();
            string description = (attribute == null ? baseType.Name : attribute.Description);
            TreeNodeViewModel child = new TreeNodeViewModel()
            {
                NodeText = description,
                NodePayload = baseType
            };
            root.TreeNodes.Add(child);
            return child;
        }
    }
}