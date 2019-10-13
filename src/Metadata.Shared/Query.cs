using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(8)]
    public sealed class Query : MetadataObject
    {
        public Query() : base(8) { }
        public Query(Guid key) : base(8, key) { }

        private Namespace _namespace = null;
        private Entity entity = null;
        private string parseTree = string.Empty;
        private Entity requestType = null;
        private Entity responseType = null;

        /// <summary>
        /// Namespace owning this query
        /// </summary>
        public Namespace Namespace { set { Set(value, ref _namespace); } get { return _namespace; } }
        /// <summary>
        /// Entity owning this query (can be null if owner is a namespace)
        /// </summary>
        public Entity Entity { set { Set(value, ref entity); } get { return entity; } }
        /// <summary>
        /// JSON serialized abstract syntax tree of the query
        /// </summary>
        public string ParseTree { set { Set(value, ref parseTree); } get { return parseTree; } }
        /// <summary>
        /// Data type of query (input data)
        /// </summary>
        public Entity RequestType { set { Set(value, ref requestType); } get { return requestType; } }
        /// <summary>
        /// Data type of query (output data)
        /// </summary>
        public Entity ResponseType { set { Set(value, ref responseType); } get { return responseType; } }
    }
}
