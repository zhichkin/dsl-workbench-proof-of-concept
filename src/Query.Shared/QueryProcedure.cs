using System.Collections.ObjectModel;

namespace OneCSharp.Query.Shared
{
    public sealed class QueryProcedure : QueryExpression
    {
        public QueryProcedure()
        {
            this.Keyword = Keywords.PROCEDURE;
        }
        public string Name { get; set; }
        public ObservableCollection<QueryParameter> Parameters { get; set; }
        public ObservableCollection<QueryExpression> Expressions { get; set; }
    }
}
