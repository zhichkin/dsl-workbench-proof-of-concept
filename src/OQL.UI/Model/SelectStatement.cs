namespace OneCSharp.OQL.Model
{
    public sealed class SelectStatement : SyntaxNode, IKeyword
    {
        public SelectStatement()
        {
            FROM = new SyntaxNodes(this);
            WHERE = new SyntaxNodes(this);
            SELECT = new SyntaxNodes(this);
        }
        public SelectStatement(ISyntaxNode parent) : this() { _parent = parent; }
        public string Keyword { get { return "SELECT"; } }
        public string Alias { get; set; }
        public SyntaxNodes FROM { get; set; }
        public SyntaxNodes WHERE { get; set; }
        public SyntaxNodes SELECT { get; set; }
    }
}
