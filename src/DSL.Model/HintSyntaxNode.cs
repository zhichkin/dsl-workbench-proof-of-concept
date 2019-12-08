namespace OneCSharp.DSL.Model
{
    public sealed class HintSyntaxNode : SyntaxNode, IKeyword
    {
        public HintSyntaxNode(ISyntaxNode parent) : base(parent) { }
        public string Keyword { get { return Keywords.WITH; } }
        public string HintType { get; set; }
        public ISyntaxNode Expression { get; set; }
    }
}
