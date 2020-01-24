using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public abstract class SyntaxNode : ISyntaxNode
    {
        public int Ordinal { get; set; }
        public ISyntaxConcept Parent { get; set; }
        public bool UseIndent { get; set; } = false;
        public bool IsOptional { get; set; } = false;
        public SyntaxNodePlacement Placement { get; set; } = SyntaxNodePlacement.OneLine;
        public object Value { get; set; }
    }
    public sealed class NameSyntaxNode : SyntaxNode
    {
        public string Name { get; set; } = string.Empty;
    }
    public sealed class LiteralSyntaxNode : SyntaxNode
    {
        public string Literal { get; set; } = string.Empty;
    }
    public sealed class KeywordSyntaxNode : SyntaxConcept
    {
        public string Keyword { get; set; } = string.Empty;
    }
    public class SyntaxNodeSelector : SyntaxNode, ISyntaxNodeSelector
    {
        public List<ISyntaxNode> Constraints { get; } = new List<ISyntaxNode>();
        public void AddConstraint(ISyntaxNode constraint)
        {
            if (constraint == null) throw new ArgumentNullException(nameof(constraint));
            if (Constraints.Contains(constraint)) return;
            Constraints.Add(constraint);
        }
        public void RemoveConstraint(ISyntaxNode constraint)
        {
            if (constraint == null) throw new ArgumentNullException(nameof(constraint));
            Constraints.Remove(constraint);
        }
    }
    public class SyntaxConcept : SyntaxNode, ISyntaxConcept
    {
        public List<ISyntaxNode> Nodes { get; } = new List<ISyntaxNode>();
        public void Add(ISyntaxNode child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            child.Parent = this;
            child.Ordinal = Nodes.Count;
            Nodes.Add(child);
        }
        public void Remove(ISyntaxNode child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            int index = 0;
            while (index < Nodes.Count)
            {
                if (Nodes[index] == child)
                {
                    Nodes.RemoveAt(index);
                }
                else
                {
                    Nodes[index].Ordinal = index;
                    index++;
                }
            }
        }
    }
    public sealed class RepeatableSyntaxNode : SyntaxConcept, IRepeatableSyntaxNode
    {
        public ISyntaxNodeSelector Selector { get; } = new SyntaxNodeSelector();
        public string OpeningLiteral { get; set; } = string.Empty;
        public string ClosingLiteral { get; set; } = string.Empty;
        public string DelimiterLiteral { get; set; } = string.Empty;
        public SyntaxNodeOrientation Orientation { get; set; } = SyntaxNodeOrientation.Vertical;
    }
}