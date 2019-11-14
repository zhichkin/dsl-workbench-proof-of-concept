namespace OneCSharp.OQL.Model
{
    public sealed class JoinOperator : SyntaxNode
    {
        public JoinOperator(ISyntaxNode parent) : base(parent) { }
        public string JoinType { get; set; }
        public ISyntaxNode Expression { get; set; }
        public ISyntaxNode ON { get; set; }
    }
}
