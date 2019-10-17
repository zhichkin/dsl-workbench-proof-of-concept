using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [Schema("md")] [PrimaryKey("key", "uniqueidentifier")] [Version("version")]
    public abstract class MetadataObject : ReferenceObject, IVersion
    {
        private byte[] _version = new byte[8];
        public MetadataObject(int typeCode) : base(typeCode, Guid.NewGuid()) { }
        public MetadataObject(int typeCode, Guid primaryKey) : base(typeCode, primaryKey) { }
        byte[] IVersion.Version { get { return _version; } set { _version = value; } }
        [Field("name", "nvarchar(100)")] public string Name { get; set; }
        [Field("alias", "nvarchar(100)")] public string Alias { get; set; }
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(this.Alias) ? this.Name : this.Alias;
        }
    }
}
