using OneCSharp.Metadata.Model;

namespace OneCSharp.DSL.Model
{
    public sealed class PropertyObject : SyntaxNode
    {
        public PropertyObject(ISyntaxNode parent) : base(parent) { }
        public Property Property { get; set; }
        public string Name { get { return Property.Name; } }
    }
    public sealed class PropertyReference : SyntaxNode
    {
        public PropertyReference(ISyntaxNode parent) : base(parent) { }
        public ISyntaxNode TableSource { get; set; }
        public ISyntaxNode PropertySource { get; set; }
    }
}
