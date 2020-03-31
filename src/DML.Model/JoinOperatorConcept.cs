using OneCSharp.AST.Model;

namespace OneCSharp.DML.Model
{
    public enum JoinType { INNER, LEFT, RIGHT, FULL, CROSS }
    public sealed class JoinOperatorConcept : SyntaxNode
    {
        public JoinOperatorConcept()
        {
            TableReference = new TableConcept() { Parent = this };
            Predicate = new WhereConcept() { Parent = this };
        }
        public JoinType Operator { get; set; }
        public TableConcept TableReference { get; set; }
        public WhereConcept Predicate { get; set; }
    }
}