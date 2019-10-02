namespace OneCSharp.Query.Shared
{
    public abstract class QueryExpression
    {
        public QueryExpression() { }
        public QueryExpression Parent { get; set; }
        public string Keyword { get; set; } = Keywords.NONE;
    }
}
