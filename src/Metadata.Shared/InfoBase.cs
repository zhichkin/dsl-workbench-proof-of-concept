using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(1)] [Table("infobases")]
    public sealed class InfoBase : MetadataObject
    {
        public InfoBase() : base(1) { }
        public InfoBase(Guid key) : base(1, key) { }
        [Field("server", "nvarchar(100)")] public string Server { get; set; }
        [Field("database", "nvarchar(100)")] public string Database { get; set; }
        [Field("username", "nvarchar(100)")] public string UserName { get; set; }
        [Field("password", "nvarchar(100)")] public string Password { get; set; }
    }
}
