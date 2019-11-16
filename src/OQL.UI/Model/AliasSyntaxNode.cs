namespace OneCSharp.OQL.Model
{
    public sealed class AliasSyntaxNode : SyntaxNode
    {
        public AliasSyntaxNode(ISyntaxNode parent) : base(parent) { }
        public string Alias { get; set; } = string.Empty;
        public ISyntaxNode Expression { get; set; }
    }
}
