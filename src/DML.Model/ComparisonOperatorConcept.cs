using OneCSharp.AST.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace OneCSharp.DML.Model
{
    public sealed class ComparisonOperatorConcept : SyntaxNode
    {
        public ComparisonType Operator { get; set; }
        [TypeConstraint(typeof(ColumnConcept), typeof(ParameterConcept), typeof(VariableConcept))]
        public SyntaxNode LeftExpression{ get; set; }
        [TypeConstraint(typeof(ColumnConcept), typeof(ParameterConcept), typeof(VariableConcept))]
        public SyntaxNode RightExpression { get; set; }
    }
    public enum ComparisonType
    {
        [Description("=")] Equal,
        [Description("<>")] NotEqual,
        [Description("<")] LessThen,
        [Description(">")] GreaterThen,
        [Description("<=")] LessThenOrEqual,
        [Description(">=")] GreaterThenOrEqual
    }
}