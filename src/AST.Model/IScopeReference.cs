namespace OneCSharp.AST.Model
{
    public interface IScopeReference
    {
        string Presentation { get; set; }
        ISyntaxNode SyntaxNode { get; set; }
    }
    public sealed class ScopeReference : IScopeReference
    {
        public string Presentation { get; set; }
        public ISyntaxNode SyntaxNode { get; set; }
    }
}