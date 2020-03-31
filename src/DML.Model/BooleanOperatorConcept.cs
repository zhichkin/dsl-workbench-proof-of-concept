using OneCSharp.AST.Model;

namespace OneCSharp.DML.Model
{
    public sealed class BooleanOperatorConcept : SyntaxNode
    {
        public BooleanOperatorConcept()
        {
            Expression = new ComparisonOperatorConcept() { Parent = this };
        }
        public BooleanOperatorType Operator { get; set; }
        public ComparisonOperatorConcept Expression { get; set; }

        // TODO: [TypeConstraint(typeof(ComparisonOperatorConcept))] public SyntaxNode Expression { get; set; }
        // See SelectorViewModel.SelectSyntaxNodeReference function
    }
    public enum BooleanOperatorType
    {
        AND, OR, NOT, IN, IS_NULL
    }
}