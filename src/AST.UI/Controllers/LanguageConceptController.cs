using OneCSharp.AST.Model;
using OneCSharp.Core.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class LanguageConceptController // TODO: ISyntaxTreeController !
    {
        public ConceptNode CreateConceptNode(ISyntaxNode parentNode, LanguageConcept syntaxNode)
        {
            ConceptNode node = new ConceptNode(parentNode, syntaxNode);

            SyntaxNodeLine codeLine = new SyntaxNodeLine(node);
            int lineNumber = node.Lines.Count;
            node.Lines.Add(codeLine);

            foreach (Property element in syntaxNode.Properties)
            {
                ((ConceptElement)element).LineNumber = lineNumber;
                CreateConceptElement(node, codeLine, (ConceptElement)element);
            }

            return node;
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
            else if (element is RepeatableElement)
            {
                BuildRepeatableElement(rootNode, codeLine, (RepeatableElement)element);
            }
        }


        private void BuildRepeatableElement(ISyntaxNode rootNode, ISyntaxNodeLine codeLine, RepeatableElement repeatable)
        {
            //LiteralNode node = new LiteralNode(rootNode, literal)
            //{
            //    Literal = literal.Name
            //};
            //literal.LineOrdinal = codeLine.Nodes.Count;
            //codeLine.Nodes.Add(node);

            if (repeatable.Value == null) return;

            

            //TODO ?
        }
        private void AddRepeatableElement(ISyntaxNode syntaxNode, RepeatableElement template, LanguageConcept elementType)
        {
            LanguageConcept syntaxTree = (LanguageConcept)syntaxNode.Model;
            RepeatableElement repeatable = (RepeatableElement)GetConceptElementFromSyntaxTree(syntaxTree, template);
            if (repeatable.Value == null)
            {
                repeatable.Value = new List<LanguageConcept>();
            }

            LanguageConcept concept = (LanguageConcept)Activator.CreateInstance(elementType.GetType());
            concept.PrepareForEditing(); // remove optional elements
            concept.Owner = syntaxTree;
            ((List<LanguageConcept>)repeatable.Value).Add(concept);

            SyntaxNodeLine codeLine = new SyntaxNodeLine(syntaxNode);
            codeLine.Nodes.Add(new IndentNode(syntaxNode));
            syntaxNode.Lines.Add(codeLine);

            ConceptNode conceptNode = CreateConceptNode(syntaxNode, concept);
            codeLine.Nodes.Add(conceptNode);
        }
        private ConceptElement GetConceptElementFromSyntaxTree(LanguageConcept syntaxTree, ConceptElement template)
        {
            ConceptElement element = (ConceptElement)syntaxTree.Properties
                    .Where(e => e.Ordinal == template.Ordinal)
                    .FirstOrDefault();
            if (element != null) return element;

            element = (RepeatableElement)template.Clone();
            element.Owner = syntaxTree;

            bool inserted = false;
            for (int i = 0; i < syntaxTree.Properties.Count; i++)
            {
                if (syntaxTree.Properties[i].Ordinal > template.Ordinal)
                {
                    syntaxTree.Properties.Insert(i, element);
                    inserted = true;
                    break;
                }
            }
            
            if (!inserted)
            {
                syntaxTree.Properties.Add(element);
            }

            return element;
        }



        private void AddNameElement(ISyntaxNode rootNode, ISyntaxNodeLine codeLine, NameElement name)
        {
            NameNode node = new NameNode(rootNode, name);
            name.LineOrdinal = codeLine.Nodes.Count;
            codeLine.Nodes.Add(node);
        }
        private void AddLiteralElement(ISyntaxNode rootNode, ISyntaxNodeLine codeLine, LiteralElement literal)
        {
            LiteralNode node = new LiteralNode(rootNode, literal)
            {
                Literal = literal.Name
            };
            literal.LineOrdinal = codeLine.Nodes.Count;
            codeLine.Nodes.Add(node);
        }
        private void AddKeywordElement(ISyntaxNode rootNode, ISyntaxNodeLine codeLine, KeywordElement keyword)
        {
            KeywordNode node = new KeywordNode(rootNode, keyword)
            {
                Keyword = keyword.Name
            };
            keyword.LineOrdinal = codeLine.Nodes.Count;
            codeLine.Nodes.Add(node);

            if (keyword.ValueType != SimpleType.NULL)
            {
                // TODO: add keyword parameter - ConstantNode : string, int and etc.
            }
            CreateContextMenu(node, keyword);
        }
        private void InsertKeywordElement(ISyntaxNode rootNode, KeywordElement keyword)
        {
            KeywordNode node = new KeywordNode(rootNode, keyword)
            {
                Keyword = keyword.Name
            };
            ISyntaxNodeLine codeLine = rootNode.Lines[keyword.LineNumber];
            codeLine.Nodes.Insert(keyword.LineOrdinal, node);

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
                        if (template is RepeatableElement)
                        {
                            foreach (DataType dataType in ((MultipleType)((ListType)template.ValueType).Type).Types)
                            {
                                node.ContextMenu.Add(new MenuItemViewModel()
                                {
                                    MenuItemHeader = $"ADD {dataType.Name}",
                                    MenuItemPayload = new object[] { node.Owner, template, dataType },
                                    MenuItemCommand = new RelayCommand(AddConceptElementCommand),
                                    MenuItemIcon = new BitmapImage(new Uri(Module.ADD_PROPERTY)),
                                });
                            }
                        }
                        else
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
                }
            }
        }



        private void GetLineNumberAndOrdinal(ISyntaxNode node, ConceptElement element, ref int lineNumber, ref int lineOrdinal)
        {
            for (int number = 0; number < node.Lines.Count; number++)
            {
                ISyntaxNodeLine line = node.Lines[number];
                for (int ordinal = 0; ordinal < line.Nodes.Count; ordinal++)
                {
                    if (line.Nodes[ordinal].Model == element)
                    {
                        lineNumber = number;
                        lineOrdinal = ordinal;
                        return;
                    }
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
                ConceptElement element = (ConceptElement)syntaxTree.Properties
                    .Where(e => ((ConceptElement)e).Ordinal == template.Ordinal)
                    .FirstOrDefault();

                if (element != null)
                {
                    _ = MessageBox.Show($"Keyword \"{template.Name}\" is already in use.", "ONE-C-SHARP", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                KeywordElement newElement = (KeywordElement)template.Clone();
                newElement.Owner = syntaxTree;

                int elementIndex = -1;
                for (elementIndex = 0; elementIndex < syntaxTree.Properties.Count; elementIndex++)
                {
                    element = (ConceptElement)syntaxTree.Properties[elementIndex];
                    if (element.Ordinal > template.Ordinal) break;
                }
                if (element == null)
                {
                    newElement.LineNumber = 0; // ???
                    newElement.LineOrdinal = 0; // ???
                    syntaxTree.Properties.Add(newElement);
                    AddKeywordElement(syntaxNode, syntaxNode.Lines[0], newElement);
                }
                else
                {
                    int lineNumber = 0;
                    int lineOrdinal = 0;
                    GetLineNumberAndOrdinal(syntaxNode, element, ref lineNumber, ref lineOrdinal);
                    if (element.Ordinal < template.Ordinal)
                    {
                        newElement.LineNumber = lineNumber;
                        newElement.LineOrdinal = syntaxNode.Lines[lineNumber].Nodes.Count;
                        syntaxTree.Properties.Add(newElement);
                        AddKeywordElement(syntaxNode, syntaxNode.Lines[lineNumber], newElement);
                    }
                    else
                    {
                        syntaxTree.Properties.Insert(elementIndex, newElement);
                        newElement.LineNumber = lineNumber;
                        newElement.LineOrdinal = lineOrdinal;
                        element.LineOrdinal += 1;
                        InsertKeywordElement(syntaxNode, newElement);
                    }
                }
            }

            if (template is RepeatableElement)
            {
                LanguageConcept conceptToAdd = (LanguageConcept)parametersArray[2];
                AddRepeatableElement(syntaxNode, (RepeatableElement)template, conceptToAdd);
            }
        }
    }
}