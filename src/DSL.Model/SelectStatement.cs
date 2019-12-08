namespace OneCSharp.DSL.Model
{
    public sealed class SelectStatement : SyntaxNode, IKeyword
    {
        public SelectStatement()
        {
            FROM = new FromClauseSyntaxNode(this);
            SELECT = new SelectClauseSyntaxNode(this);
        }
        public SelectStatement(ISyntaxNode parent) : this() { _parent = parent; }
        public string Keyword { get { return Keywords.SELECT; } }
        public string Alias { get; set; }
        public FromClauseSyntaxNode FROM { get; set; }
        public SelectClauseSyntaxNode SELECT { get; set; }
        public WhereSyntaxNode WHERE { get; set; }
        public HavingSyntaxNode HAVING { get; set; }
    }
    public sealed class FromClauseSyntaxNode : SyntaxNodeList, IKeyword
    {
        public string Keyword { get { return Keywords.FROM; } }
        public FromClauseSyntaxNode(SelectStatement parent) { Parent = parent; }
    }
    public sealed class SelectClauseSyntaxNode : SyntaxNodeList, IKeyword
    {
        public string Keyword { get { return Keywords.SELECT; } }
        public SelectClauseSyntaxNode(SelectStatement parent) { Parent = parent; }
    }
}
