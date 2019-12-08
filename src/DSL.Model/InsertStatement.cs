namespace OneCSharp.DSL.Model
{
    public sealed class InsertStatement : SyntaxNode, IKeyword
    {
        public string Keyword { get { return Keywords.INSERT; } }
        public InsertStatement(ISyntaxNode parent) : base(parent) { }
        public TableObject Table { get; set; }
        public ISyntaxNode DataSource { get; set; } // SelectStatement | ExecuteStatement | Parameters list
    }
}