using System.ComponentModel;

namespace OneCSharp.AST.Model
{
    public interface IIdentifiable
    {
        string Identifier { get; set; }
    }
    public interface ISyntaxNode
    {
        ISyntaxNode Parent { get; set; }
    }
    [Description("Language concepts")]
    public abstract class SyntaxNode : ISyntaxNode
    {
        public ISyntaxNode Parent { get; set; }
    }
    [Description("Root concepts")]
    public abstract class SyntaxRoot : SyntaxNode
    {
        // marker class (interface)
    }
}