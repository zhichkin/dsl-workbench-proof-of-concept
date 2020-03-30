using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace OneCSharp.AST.UI
{
    public sealed class NodePosition
    {
        public int Line { get; set; } = 0;
        public int Position { get; set; } = 0;
    }
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
        public static NodePosition GetPosition(this ISyntaxNodeViewModel @this, ISyntaxNodeViewModel node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            NodePosition position = new NodePosition();
            for (int l = 0; l < @this.Lines.Count; l++)
            {
                var line = @this.Lines[l];
                for (int p = 0; p < line.Nodes.Count; p++)
                {
                    if (line.Nodes[p] == node)
                    {
                        position.Line = l;
                        position.Position = p;
                        return position;
                    }
                }
            }
            return position;
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
        public static ConceptNodeViewModel Concept(this ConceptNodeViewModel @this)
        {
            ICodeLineViewModel codeLine = @this.NewLine().BottomCodeLine();
            codeLine.Nodes.Add(new ConceptNodeViewModel(@this, null));
            return @this;
        }
        public static ConceptNodeViewModel Identifier(this ConceptNodeViewModel @this)
        {
            ICodeLineViewModel codeLine = @this.BottomCodeLine();
            codeLine.Nodes.Add(new IdentifierNodeViewModel(@this, @this.SyntaxNode));
            return @this;
        }
        public static ConceptNodeViewModel Keyword(this ConceptNodeViewModel @this, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentNullException(nameof(keyword));

            ISyntaxNodeViewModel syntaxNode = @this.LastSyntaxNode();
            if (syntaxNode is PropertyViewModel property)
            {
                property.Nodes.Add(new KeywordNodeViewModel(property)
                {
                    Keyword = keyword,
                    PropertyBinding = property.PropertyBinding
                });
            }
            else
            {
                ICodeLineViewModel codeLine = @this.BottomCodeLine();
                codeLine.Nodes.Add(new KeywordNodeViewModel(@this) { Keyword = keyword });
            }
            return @this;
            //ICodeLineViewModel codeLine = @this.BottomCodeLine();
            //codeLine.Nodes.Add(new KeywordNodeViewModel(@this) { Keyword = keyword });
        }
        public static ConceptNodeViewModel Literal(this ConceptNodeViewModel @this, string literal)
        {
            if (string.IsNullOrWhiteSpace(literal)) throw new ArgumentNullException(nameof(literal));

            ISyntaxNodeViewModel syntaxNode = @this.LastSyntaxNode();
            if (syntaxNode is PropertyViewModel property)
            {
                property.Nodes.Add(new LiteralNodeViewModel(property)
                {
                    Literal = literal,
                    PropertyBinding = property.PropertyBinding
                });
            }
            else
            {
                ICodeLineViewModel codeLine = @this.BottomCodeLine();
                codeLine.Nodes.Add(new LiteralNodeViewModel(@this) { Literal = literal });
            }
            return @this;

            //ICodeLineViewModel codeLine = @this.BottomCodeLine();
            //codeLine.Nodes.Add(new LiteralNodeViewModel(@this) { Literal = literal });
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
        public static ConceptNodeViewModel Selector(this ConceptNodeViewModel @this)
        {
            ISyntaxNodeViewModel syntaxNode = @this.LastSyntaxNode();
            if (syntaxNode is PropertyViewModel property)
            {
                property.Nodes.Add(new SelectorViewModel(property)
                {
                    PropertyBinding = property.PropertyBinding
                });
            }
            else
            {
                ICodeLineViewModel codeLine = @this.BottomCodeLine();
                codeLine.Nodes.Add(new SelectorViewModel(@this));
            }
            return @this;

            //ICodeLineViewModel codeLine = @this.BottomCodeLine();
            //codeLine.Nodes.Add(new SelectorViewModel(@this));
        }
        public static ConceptNodeViewModel Property(this ConceptNodeViewModel @this, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            ICodeLineViewModel codeLine = @this.BottomCodeLine();
            codeLine.Nodes.Add(new PropertyViewModel(@this) { PropertyBinding = propertyName });
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
        public static TreeNodeViewModel BuildReferenceSelectorTree(IEnumerable<ISyntaxNode> references)
        {
            TreeNodeViewModel tree = new TreeNodeViewModel();

            Type baseType;
            TreeNodeViewModel currentNode;
            Stack<Type> stack = new Stack<Type>();
            
            foreach (ISyntaxNode node in references)
            {
                currentNode = tree;
                baseType = node.GetType();

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
            DescriptionAttribute attribute = baseType.GetCustomAttribute<DescriptionAttribute>(false);
            string description = (attribute == null ? baseType.Name : attribute.Description);
            TreeNodeViewModel child = new TreeNodeViewModel()
            {
                NodeText = description,
                NodePayload = baseType
            };
            root.TreeNodes.Add(child);
            return child;
        }
        public static TreeNodeViewModel BuildAssemblySelectorTree(IEnumerable<ISyntaxNode> references)
        {
            TreeNodeViewModel tree = new TreeNodeViewModel();
            foreach (ISyntaxNode node in references)
            {
                tree.TreeNodes.Add(new TreeNodeViewModel()
                {
                    IsExpanded = true,
                    NodePayload = node,
                    NodeText = node.ToString()
                });
            }
            return tree;
        }
        public static TreeNodeViewModel BuildEnumerationSelectorTree(Type enumType)
        {
            TreeNodeViewModel tree = new TreeNodeViewModel();
            foreach (var value in Enum.GetValues(enumType))
            {
                tree.TreeNodes.Add(new TreeNodeViewModel()
                {
                    IsExpanded = true,
                    NodePayload = value,
                    NodeText = Enum.GetName(enumType, value)
                });
            }
            return tree;
        }
        public static TreeNodeViewModel BuildTypeSelectionTree(IEnumerable<Type> types)
        {
            TreeNodeViewModel root = new TreeNodeViewModel();
            foreach (Type type in types)
            {
                TreeNodeViewModel namespaceNode = GetNamespaceNode(root, type.Namespace);
                if (namespaceNode == null)
                {
                    return root;
                }
                TreeNodeViewModel typeNode = new TreeNodeViewModel()
                {
                    IsExpanded = false,
                    NodePayload = type,
                    NodeText = type.Name
                };
                foreach (Type nestedType in type.GetNestedTypes())
                {
                    typeNode.TreeNodes.Add(new TreeNodeViewModel()
                    {
                        IsExpanded = false,
                        NodePayload = nestedType,
                        NodeText = nestedType.Name
                    });
                }
                namespaceNode.TreeNodes.Add(typeNode);
            }
            return root;
        }
        private static TreeNodeViewModel GetNamespaceNode(TreeNodeViewModel root, string namespacePath)
        {
            if (string.IsNullOrWhiteSpace(namespacePath)) throw new NullReferenceException(nameof(namespacePath));

            string[] namespaces = namespacePath.Split('.');

            TreeNodeViewModel parentNode = root;
            TreeNodeViewModel currentNode = null;
            foreach (string name in namespaces)
            {
                currentNode = parentNode.TreeNodes.Where(n => n.NodeText == name).FirstOrDefault();
                if (currentNode == null)
                {
                    TreeNodeViewModel namespaceNode = new TreeNodeViewModel()
                    {
                        IsExpanded = true,
                        NodePayload = null,
                        NodeText = name
                    };
                    parentNode.TreeNodes.Add(namespaceNode);
                    parentNode = namespaceNode;
                }
                else
                {
                    parentNode = currentNode;
                }
            }
            return parentNode;
        }
    }
}