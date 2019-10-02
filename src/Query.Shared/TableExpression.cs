using OneCSharp.Metadata.Shared;

namespace OneCSharp.Query.Shared
{
    public abstract class TableExpression : QueryExpression
    {
        public TableExpression() 
        {
            this.Hint = HintTypes.NoneHint;
        }
        public string Name { get { return (this.Entity == null) ? string.Empty : this.Entity.Name; } }
        public string Alias { get; set; }
        public Entity Entity { get; set; }
        public string Hint { get; set; }
    }
}
