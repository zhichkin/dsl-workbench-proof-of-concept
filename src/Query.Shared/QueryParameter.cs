using OneCSharp.Metadata.Shared;

namespace OneCSharp.Query.Shared
{
    public sealed class QueryParameter : QueryExpression
    {
        public QueryParameter() { }
        public string Name { get; set; }
        public Entity Type { get; set; }
        public object Value { get; set; }
    }
}
