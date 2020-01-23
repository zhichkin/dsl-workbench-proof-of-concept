using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public abstract class SyntaxElement : ISyntaxElement
    {
        public int Ordinal { get; set; }
        public ISyntaxConcept Parent { get; set; }
        public bool UseIndent { get; set; } = false;
        public bool IsOptional { get; set; } = false;
        public SyntaxElementPlacement Placement { get; set; } = SyntaxElementPlacement.OneLine;
    }
    public sealed class NameSyntaxElement : SyntaxElement
    {
        public string Name { get; set; } = string.Empty;
    }
    public sealed class LiteralSyntaxElement : SyntaxElement
    {
        public string Literal { get; set; } = string.Empty;
    }
    public sealed class KeywordSyntaxElement : SyntaxElement
    {
        public string Keyword { get; set; } = string.Empty;
    }
    public class SyntaxElementSelector : SyntaxConcept, ISyntaxElementSelector
    {
        
    }
    public sealed class RepeatableSyntaxElement : SyntaxElement, IRepeatableSyntaxElement
    {
        public ISyntaxElementSelector Selector { get; } = new SyntaxElementSelector();
        public string OpeningLiteral { get; set; } = string.Empty;
        public string ClosingLiteral { get; set; } = string.Empty;
        public string DelimiterLiteral { get; set; } = string.Empty;
        public SyntaxElementOrientation Orientation { get; set; } = SyntaxElementOrientation.Vertical;
    }

    public class SyntaxConcept : SyntaxElement, ISyntaxConcept
    {
        public List<ISyntaxElement> Elements { get; } = new List<ISyntaxElement>();
        public void AddElement(ISyntaxElement child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            child.Parent = this;
            child.Ordinal = Elements.Count;
            Elements.Add(child);
        }
        public void RemoveElement(ISyntaxElement child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            int index = 0;
            while (index < Elements.Count)
            {
                if (Elements[index] == child)
                {
                    Elements.RemoveAt(index);
                }
                else
                {
                    Elements[index].Ordinal = index;
                    index++;
                }
            }
        }
    }
}