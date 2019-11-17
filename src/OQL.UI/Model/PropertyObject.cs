using OneCSharp.Metadata;

namespace OneCSharp.OQL.Model
{
    public sealed class PropertyObject : SyntaxNode
    {
        public PropertyObject(ISyntaxNode parent) : base(parent) { }
        public DbProperty Property { get; set; }
        public string Name { get { return Property.Name; } }
    }
    public sealed class PropertyReference : SyntaxNode
    {
        public PropertyReference(ISyntaxNode parent) : base(parent) { }
        public ISyntaxNode TableSource { get; set; }
        public ISyntaxNode PropertySource { get; set; }
    }
}
