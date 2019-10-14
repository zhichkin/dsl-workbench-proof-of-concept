using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(1)]
    [Table("infobases")]
    public sealed class InfoBase : MetadataObject
    {
        public InfoBase() : base(1) { }
        public InfoBase(Guid key) : base(1, key) { }

        private string _server = string.Empty;
        [Field("server", "nvarchar(100)")]
        public string Server { set { Set(value, ref _server); } get { return _server; } }
        
        private string _database = string.Empty;
        [Field("database", "nvarchar(100)")]
        public string Database { set { Set(value, ref _database); } get { return _database; } }
        
        private string _username = string.Empty;
        [Field("username", "nvarchar(100)")]
        public string UserName { set { Set(value, ref _username); } get { return _username; } }

        private string _password = string.Empty;
        [Field("password", "nvarchar(100)")]
        public string Password { set { Set(value, ref _password); } get { return _password; } }
    }
}
