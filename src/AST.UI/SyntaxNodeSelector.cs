using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace OneCSharp.AST.UI
{
    public static class SyntaxNodeSelector
    {
        public static TreeNodeViewModel BuildTypeConstraintsTree(TypeConstraint constraints)
        {
            TreeNodeViewModel tree = new TreeNodeViewModel();

            BuildConstantSelectorTree(tree, constraints.DotNetTypes);
            BuildTypeSelectorTree(tree, constraints.DataTypes, typeof(DataType));
            BuildTypeSelectorTree(tree, constraints.Concepts, typeof(SyntaxNode));

            // expand only top level nodes
            foreach (TreeNodeViewModel treeNode in tree.TreeNodes)
            {
                treeNode.IsExpanded = true;
            }

            return tree;
        }
        private static void BuildConstantSelectorTree(TreeNodeViewModel root, List<Type> types)
        {
            TreeNodeViewModel groupingNode = new TreeNodeViewModel()
            {
                NodeText = "Constants"
            };
            root.TreeNodes.Add(groupingNode);

            foreach (Type type in types)
            {
                TreeNodeViewModel typeNode = new TreeNodeViewModel()
                {
                    NodeText = type.ToString(),
                    NodePayload = type
                };
                groupingNode.TreeNodes.Add(typeNode);
            }
        }
        private static void BuildTypeSelectorTree(TreeNodeViewModel root, List<Type> types, Type rootType)
        {
            Type baseType;
            TreeNodeViewModel currentNode;
            Stack<Type> stack = new Stack<Type>();

            foreach (Type type in types)
            {
                baseType = type;
                currentNode = root;

                while (baseType != rootType || baseType != null)
                {
                    stack.Push(baseType);
                    baseType = baseType.BaseType;
                }

                while (stack.Count > 0)
                {
                    baseType = stack.Pop();
                    currentNode = AddNodeToTypeSelectorTree(currentNode, baseType);
                }
            }
        }
        private static TreeNodeViewModel AddNodeToTypeSelectorTree(TreeNodeViewModel treeNode, Type baseType)
        {
            foreach (TreeNodeViewModel node in treeNode.TreeNodes)
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
            treeNode.TreeNodes.Add(child);
            return child;
        }
    }
}