namespace OneCSharp.DSL.Model
{
    public sealed class JoinOperator : SyntaxNode, IKeyword
    {
        public JoinOperator(ISyntaxNode parent) : base(parent)
        {
            ON = new OnSyntaxNode(this);
        }
        public string Keyword { get { return Keywords.JOIN; } }
        public string JoinType { get; set; }
        public OnSyntaxNode ON { get; private set; }
        public ISyntaxNode Expression { get; set; }
    }
}
