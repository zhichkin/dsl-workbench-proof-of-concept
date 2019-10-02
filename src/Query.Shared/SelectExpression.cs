using System.Collections.ObjectModel;

namespace OneCSharp.Query.Shared
{
    public sealed class SelectExpression : TableExpression
    {
        public SelectExpression() { this.Keyword = Keywords.SELECT; }
        public ObservableCollection<TableExpression> FROM { get; set; }
        public BooleanFunction WHERE { get; set; }
        public BooleanFunction HAVING { get; set; }
        public ObservableCollection<PropertyExpression> SELECT { get; set; }
    }
}