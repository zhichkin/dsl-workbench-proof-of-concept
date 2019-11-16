namespace OneCSharp.OQL.Model
{
    public sealed class SelectStatement : SyntaxNode, IKeyword
    {
        public SelectStatement()
        {
            FROM = new FromClauseSyntaxNode(this);
            WHERE = new WhereClauseSyntaxNode(this);
            SELECT = new SelectClauseSyntaxNode(this);
        }
        public SelectStatement(ISyntaxNode parent) : this() { _parent = parent; }
        public string Keyword { get { return Keywords.SELECT; } }
        public string Alias { get; set; }
        public FromClauseSyntaxNode FROM { get; set; }
        public WhereClauseSyntaxNode WHERE { get; set; }
        public SelectClauseSyntaxNode SELECT { get; set; }
    }
    public sealed class FromClauseSyntaxNode : SyntaxNodeList, IKeyword
    {
        public string Keyword { get { return Keywords.FROM; } }
        public FromClauseSyntaxNode(SelectStatement parent) { Parent = parent; }
    }
    public sealed class WhereClauseSyntaxNode : SyntaxNodeList, IKeyword
    {
        public string Keyword { get { return Keywords.WHERE; } }
        public WhereClauseSyntaxNode(SelectStatement parent) { Parent = parent; }
    }
    public sealed class SelectClauseSyntaxNode : SyntaxNodeList, IKeyword
    {
        public string Keyword { get { return Keywords.SELECT; } }
        public SelectClauseSyntaxNode(SelectStatement parent) { Parent = parent; }
    }
}
