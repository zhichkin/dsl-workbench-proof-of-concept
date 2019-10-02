using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(8)]
    public sealed class Query : MetadataObject
    {
        public Query() : base() { }
        public Query(Guid key) : base(key) { }
        public override int TypeCode { get { return 8; } }

        private Namespace _namespace = null;
        private Entity owner = null;
        private string parseTree = string.Empty;
        private Entity requestType = null;
        private Entity responseType = null;

        /// <summary>
        /// Namespace owning this query
        /// </summary>
        public Namespace Namespace { set { Set<Namespace>(value, ref _namespace); } get { return Get<Namespace>(ref _namespace); } }
        /// <summary>
        /// Entity owning this query (can be null if owner is a namespace)
        /// </summary>
        public Entity Owner { set { Set<Entity>(value, ref owner); } get { return Get<Entity>(ref owner); } }
        /// <summary>
        /// JSON serialized abstract syntax tree of the query
        /// </summary>
        public string ParseTree { set { Set<string>(value, ref parseTree); } get { return Get<string>(ref parseTree); } }
        /// <summary>
        /// Data type of query (input data)
        /// </summary>
        public Entity RequestType { set { Set<Entity>(value, ref requestType); } get { return Get<Entity>(ref requestType); } }
        /// <summary>
        /// Data type of query (output data)
        /// </summary>
        public Entity ResponseType { set { Set<Entity>(value, ref responseType); } get { return Get<Entity>(ref responseType); } }
    }
}
