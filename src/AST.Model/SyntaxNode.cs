using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public enum SyntaxNodePlacement { OneLine, NewLine }
    public enum SyntaxNodeOrientation { Vertical, Horizontal }
    public interface ISyntaxNode
    {
        int Ordinal { get; set; }
        ISyntaxConcept Parent { get; set; }
        bool UseIndent { get; set; }
        bool IsOptional { get; set; }
        SyntaxNodePlacement Placement { get; set; }
        object Value { get; set; }
    }
    public interface ISyntaxNodeSelector : ISyntaxNode
    {
        List<ISyntaxNode> Constraints { get; }
        void AddConstraint(ISyntaxNode constraint);
        void RemoveConstraint(ISyntaxNode constraint);
    }
    public interface ISyntaxConcept : ISyntaxNode
    {
        List<ISyntaxNode> Nodes { get; }
        void Add(ISyntaxNode child);
        void Remove(ISyntaxNode child);
    }
    public interface IRepeatableSyntaxNode : ISyntaxConcept
    {
        ISyntaxNodeSelector Selector { get; }
        string OpeningLiteral { get; set; }
        string ClosingLiteral { get; set; }
        string DelimiterLiteral { get; set; }
        SyntaxNodeOrientation Orientation { get; set; }
    }
}