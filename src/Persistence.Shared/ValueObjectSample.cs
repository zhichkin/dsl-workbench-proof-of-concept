using System;

namespace OneCSharp.Persistence.Shared
{
    public interface ITablePartKey
    {
        ReferenceObject Owner { get; set; }
        int RowNumber { get; set; }
    }
    public class TablePart : IPersistentObject<ITablePartKey>, ITablePartKey
    {
        private ITablePartKey _primaryKey;
        public int TypeCode { get; set; }
        public ITablePartKey PrimaryKey { get { return this; } }
        ITablePartKey IPersistentObject<ITablePartKey>.PrimaryKey { get { return this; } set { _primaryKey = value; } }
        public ReferenceObject Owner { get; set; }
        public int RowNumber { get; set; }
    }
}
