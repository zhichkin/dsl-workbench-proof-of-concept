using OneCSharp.Metadata.Shared;

namespace OneCSharp.Query.Shared
{
    public class PropertyReference : QueryExpression
    {
        public PropertyReference() { }
        public string Name { get { return this.Property.Name; } }
        public Property Property { get; set; }
        public TableExpression Table { get; set; }
    }
}
