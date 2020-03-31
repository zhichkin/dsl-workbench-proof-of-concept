using OneCSharp.AST.Model;
using System.Collections.Generic;

namespace OneCSharp.DML.Model
{
    public sealed class WhereConcept : SyntaxNode
    {
        [TypeConstraint(typeof(BooleanOperatorConcept), typeof(ComparisonOperatorConcept))]
        public List<SyntaxNode> Expressions { get; } = new List<SyntaxNode>();
    }
}