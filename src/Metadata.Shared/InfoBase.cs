using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(1)]
    [Schema("ocs")]
    [Table("infobases")]
    //[PrimaryKey("_IDRRef")]
    public sealed class InfoBase : MetadataObject
    {
        public InfoBase() : base(1) { }
        public InfoBase(Guid key) : base(1, key) { }

        private string _server = string.Empty;
        private string _database = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        public string Server { set { Set(value, ref _server); } get { return _server; } }
        public string Database { set { Set(value, ref _database); } get { return _database; } }
        public string UserName { set { Set(value, ref _username); } get { return _username; } }
        public string Password { set { Set(value, ref _password); } get { return _password; } }
    }
}
