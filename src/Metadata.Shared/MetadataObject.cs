using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    public abstract class MetadataObject : ReferenceObject
    {
        public MetadataObject() : base() { this._key = Guid.NewGuid(); }
        public MetadataObject(Guid key) : base(key) { }

        private byte[] _version = new byte[8]; // Optimistic concurrency implementation
        private string _name = string.Empty;
        private string _alias = string.Empty;
        
        public string Name { set { Set<string>(value, ref _name); } get { return Get<string>(ref _name); } }
        public string Alias { set { Set<string>(value, ref _alias); } get { return Get<string>(ref _alias); } }
        
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(this.Alias) ? this.Name : this.Alias;
        }

        public abstract class Persister : StateObject.Insider
        {
            protected void SetVersion(MetadataObject target, byte[] version)
            {
                target._version = version;
            }
            protected byte[] GetVersion(MetadataObject target)
            {
                return target._version;
            }
        }
    }
}
