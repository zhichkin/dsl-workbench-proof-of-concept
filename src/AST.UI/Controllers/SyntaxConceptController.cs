using OneCSharp.AST.Model;
using OneCSharp.Core.Model;
using OneCSharp.MVVM;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxConceptController // TODO: ISyntaxController !?
    {
        public SyntaxNode CreateSyntaxNode(LanguageConcept syntaxTree)
        {
            if (syntaxTree == null) throw new ArgumentNullException(nameof(syntaxTree));

            LanguageConcept concept = (LanguageConcept)Activator.CreateInstance(syntaxTree.GetType());

            SyntaxNode rootNode = new SyntaxNode(null, syntaxTree);
            SyntaxNodeLine codeLine = new SyntaxNodeLine(rootNode);
            rootNode.Lines.Add(codeLine);
            foreach (Property element in syntaxTree.Properties)
            {
                CreateConceptElement(rootNode, codeLine, (ConceptElement)element);
            }
            return rootNode;
        }
        private void CreateConceptElement(ISyntaxNode rootNode, ISyntaxNodeLine codeLine, ConceptElement element)
        {
            if (element is KeywordElement)
            {
                AddKeywordElement(rootNode, codeLine, (KeywordElement)element);
            }
            else if (element is LiteralElement)
            {
                AddLiteralElement(rootNode, codeLine, (LiteralElement)element);
            }
            else if (element is NameElement)
            {
                AddNameElement(rootNode, codeLine, (NameElement)element);
            }
            else if (element is RepeatableElement repeatable)
            {
                // TODO: 
            }
            //var indent = new IndentNode(rootNode);
        }
        private void AddNameElement(ISyntaxNode rootNode, ISyntaxNodeLine codeLine, NameElement name)
        {
            NameNode node = new NameNode(rootNode, name);
            codeLine.Nodes.Add(node);
        }
        private void AddLiteralElement(ISyntaxNode rootNode, ISyntaxNodeLine codeLine, LiteralElement literal)
        {
            LiteralNode node = new LiteralNode(rootNode, literal)
            {
                Literal = literal.Name
            };
            codeLine.Nodes.Add(node);
        }
        private void AddKeywordElement(ISyntaxNode rootNode, ISyntaxNodeLine codeLine, KeywordElement keyword)
        {
            KeywordNode node = new KeywordNode(rootNode, keyword)
            {
                Keyword = keyword.Name
            };
            codeLine.Nodes.Add(node);

            if (keyword.ValueType != SimpleType.NULL)
            {
                // TODO: add ConstantNode : string, int and etc.
            }
            CreateContextMenu(node, keyword);
        }
        private void CreateContextMenu(KeywordNode node, KeywordElement keyword)
        {
            if (keyword.IsOptional)
            {
                node.IsContextMenuEnabled = false;
                return;
            }
            node.ContextMenu.Clear();

            LanguageConcept concept = (LanguageConcept)Activator.CreateInstance(keyword.Owner.GetType());

            LanguageConcept syntaxTree = (LanguageConcept)keyword.Owner;
            foreach (Property property in concept.Properties)
            {
                ConceptElement template = (ConceptElement)property;
                ConceptElement element = (ConceptElement)syntaxTree.Properties.Where(e =>
                                                                                e.Name == template.Name
                                                                                && e.GetType() == template.GetType())
                                                                                .FirstOrDefault();
                if (template.IsOptional)
                {
                    if (element == null) // the element is not present at syntax tree - can be added
                    {
                        node.ContextMenu.Add(new MenuItemViewModel()
                        {
                            MenuItemHeader = $"ADD {template.Name}",
                            MenuItemPayload = new object[] { node.Owner, template },
                            MenuItemCommand = new RelayCommand(AddConceptElementCommand),
                            MenuItemIcon = new BitmapImage(new Uri(Module.ADD_PROPERTY)),
                        });
                    }
                }
                
                if (template is RepeatableElement)
                {
                    node.ContextMenu.Add(new MenuItemViewModel()
                    {
                        MenuItemHeader = $"ADD {((ListType)template.ValueType).Type.Name}",
                        MenuItemPayload = new object[] { node.Owner, template },
                        MenuItemCommand = new RelayCommand(AddConceptElementCommand),
                        MenuItemIcon = new BitmapImage(new Uri(Module.ADD_PROPERTY)),
                    });
                }
            }
        }
        private void AddConceptElementCommand(object parameter)
        {
            object[] parametersArray = (object[])parameter;
            SyntaxNode syntaxNode = (SyntaxNode)parametersArray[0];
            ConceptElement template = (ConceptElement)parametersArray[1];
            LanguageConcept syntaxTree = (LanguageConcept)syntaxNode.Model;

            if (template is KeywordElement)
            {
                ConceptElement element = (ConceptElement)syntaxTree.Properties.Where(e =>
                                                                                e.Name == template.Name
                                                                                && e.GetType() == template.GetType())
                                                                                .FirstOrDefault();
                if (element != null)
                {
                    _ = MessageBox.Show($"Keyword \"{template.Name}\" is already in use.", "ONE-C-SHARP", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                KeywordElement newElement = (KeywordElement)template.Clone();
                newElement.Owner = syntaxTree;

                // TODO:
                // 1. find line where the test element is placed
                // 2. find index in that line where the test element is placed
                // 3. use this index to place new element in this line

                if (syntaxTree.Properties.Count == 0)
                {
                    syntaxTree.Properties.Add(newElement);
                    AddKeywordElement(syntaxNode, syntaxNode.Lines[0], newElement);
                    return;
                }

                LanguageConcept concept = (LanguageConcept)Activator.CreateInstance(syntaxNode.Model.GetType());
                int anchorIndex = concept.Properties.FindIndex(e => e.Name == template.Name && e.GetType() == template.GetType());
                if (anchorIndex == -1) return;

                int testIndex = -1;
                int currentIndex = 0;
                ConceptElement testElement;

                while (testIndex < anchorIndex && currentIndex < syntaxTree.Properties.Count)
                {
                    testElement = (ConceptElement)syntaxTree.Properties[currentIndex];
                    testIndex = concept.Properties.FindIndex(e => e.Name == testElement.Name && e.GetType() == testElement.GetType());
                    currentIndex++;
                }

                if (currentIndex == syntaxTree.Properties.Count)
                {
                    if (testIndex < anchorIndex)
                    {
                        syntaxTree.Properties.Add(newElement);
                        AddKeywordElement(syntaxNode, syntaxNode.Lines[0], newElement);
                        return;
                    }
                    else
                    {
                        currentIndex--; // ???
                    }
                }

                currentIndex--;
                syntaxTree.Properties.Insert(currentIndex, newElement);
                AddKeywordElement(syntaxNode, syntaxNode.Lines[0], newElement);
                return;
            }
        }
    }
}