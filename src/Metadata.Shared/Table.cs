using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(5)]
    public sealed class Table : MetadataObject
    {
        public Table() : base() { }
        public Table(Guid key) : base(key) { }
        public override int TypeCode { get { return 5; } }

        private string _schema = string.Empty;
        private TablePurpose _purpose = TablePurpose.Main; // purpose of the table

        public string Schema { set { Set<string>(value, ref _schema); } get { return Get<string>(ref _schema); } }
        public TablePurpose Purpose { set { Set<TablePurpose>(value, ref _purpose); } get { return Get<TablePurpose>(ref _purpose); } }
    }
}
