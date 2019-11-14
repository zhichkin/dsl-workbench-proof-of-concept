namespace OneCSharp.OQL.Model
{
    public sealed class AliasExpression : SyntaxNode
    {
        public AliasExpression(ISyntaxNode parent) : base(parent) { }
        public string Alias { get; set; } = string.Empty;
        public ISyntaxNode Expression { get; set; }
    }
}
