namespace OneCSharp.OQL.Model
{
    public sealed class Procedure : SyntaxNode, IKeyword
    {
        public Procedure()
        {
            Parameters = new SyntaxNodes(this);
            Statements = new SyntaxNodes(this);
        }
        public Procedure(ISyntaxNode parent) : this() { _parent = parent; }
        public string Keyword { get { return Keywords.PROCEDURE; } }
        public string Name { get; set; }
        public SyntaxNodes Parameters { get; set; }
        public SyntaxNodes Statements { get; set; }
    }
}
