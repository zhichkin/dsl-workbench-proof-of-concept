namespace OneCSharp.OQL.Model
{
    public sealed class HintExpression : SyntaxNode
    {
        public HintExpression(ISyntaxNode parent) : base(parent) { }
        public string HintType { get; set; }
        public ISyntaxNode Expression { get; set; }
    }
}
