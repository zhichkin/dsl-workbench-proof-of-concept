using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(5)]
    public sealed class Table : MetadataObject
    {
        public Table() : base(5) { }
        public Table(Guid key) : base(5, key) { }
        public string Schema { get; set; }
        public TablePurpose Purpose { get; set; } = TablePurpose.Main;
    }
}
