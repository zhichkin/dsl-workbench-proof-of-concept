using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [Schema("metadata")]
    [PrimaryKey("key")]
    [Version("version", "rowversion")]
    public abstract class MetadataObject : ReferenceObject, IOptimisticConcurrencyObject
    {
        private byte[] _version = new byte[8]; // Optimistic concurrency implementation
        public MetadataObject(int typeCode)
        {
            this._typeCode = typeCode;
            this._primaryKey = Guid.NewGuid();
            this._state = PersistentState.New;
        }
        public MetadataObject(int typeCode, Guid primaryKey)
        {
            this._typeCode = typeCode;
            this._primaryKey = primaryKey;
            this._state = PersistentState.New;
        }
        byte[] IOptimisticConcurrencyObject.Version { get { return _version; } set { _version = value; } }

        private string _name = string.Empty;
        [Field("name", "nvarchar(100)")]
        public string Name { set { Set(value, ref _name); } get { return _name; } }

        private string _alias = string.Empty;
        [Field("alias", "nvarchar(100)")]
        public string Alias { set { Set(value, ref _alias); } get { return _alias; } }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(this.Alias) ? this.Name : this.Alias;
        }
    }
}
