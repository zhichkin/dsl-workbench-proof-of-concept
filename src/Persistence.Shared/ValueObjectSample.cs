using System;

namespace OneCSharp.Persistence.Shared
{
    public interface ITablePartKey
    {
        ReferenceObject Owner { get; set; }
        int RowNumber { get; set; }
    }
    public class TablePart : IValueObject<ITablePartKey>, ITablePartKey
    {
        public ITablePartKey PrimaryKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TypeCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ReferenceObject Owner { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int RowNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
