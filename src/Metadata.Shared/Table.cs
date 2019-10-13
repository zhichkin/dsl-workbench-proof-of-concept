using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(5)]
    public sealed class Table : MetadataObject
    {
        public Table() : base(5) { }
        public Table(Guid key) : base(5, key) { }

        private string _schema = string.Empty;
        private TablePurpose _purpose = TablePurpose.Main; // purpose of the table

        public string Schema { set { Set(value, ref _schema); } get { return _schema; } }
        public TablePurpose Purpose { set { Set(value, ref _purpose); } get { return _purpose; } }
    }
}
