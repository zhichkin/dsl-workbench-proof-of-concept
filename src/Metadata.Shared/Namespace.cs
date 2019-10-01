using OneCSharp.Persistence.Shared;
using System;
using System.Collections.Generic;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(2)]
    public sealed class Namespace : MetadataObject
    {
        public Namespace() : base() { }
        public Namespace(Guid key) : base(key) { }
        public override int TypeCode { get { return 2; } }

        private MetadataObject _owner = null; // InfoBase|Namespace
        private List<Namespace> _namespaces = new List<Namespace>();
        //private List<Entity> _entities = new List<Entity>();
        //private List<Queries> _queries = new List<Queries>();
        public MetadataObject Owner { set { Set<MetadataObject>(value, ref _owner); } get { return Get<MetadataObject>(ref _owner); } }
        public List<Namespace> Namespaces { get { return _namespaces; } }
        public InfoBase GetInfoBase()
        {
            Namespace ns = this;
            while (ns.Owner.GetType() != typeof(InfoBase))
            {
                ns = (Namespace)ns.Owner;
            }
            return (InfoBase)ns.Owner;
        }
    }
}
