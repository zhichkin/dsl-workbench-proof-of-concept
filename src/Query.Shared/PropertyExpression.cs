namespace OneCSharp.Query.Shared
{
    public class PropertyExpression : QueryExpression
    {
        public PropertyExpression() { }
        public string Alias { get; set; }
        public TableExpression Table { get; set; }
        public QueryExpression Expression { get; set; }
    }
}
