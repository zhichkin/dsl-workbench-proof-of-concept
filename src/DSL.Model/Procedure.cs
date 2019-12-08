namespace OneCSharp.DSL.Model
{
    public sealed class Procedure : SyntaxNode, IKeyword
    {
        public Procedure()
        {
            Parameters = new SyntaxNodeList(this);
            Statements = new SyntaxNodeList(this);
        }
        public Procedure(ISyntaxNode parent) : this() { _parent = parent; }
        public string Keyword { get { return Keywords.PROCEDURE; } }
        public string Name { get; set; }
        public SyntaxNodeList Parameters { get; set; }
        public SyntaxNodeList Statements { get; set; }
    }
}
