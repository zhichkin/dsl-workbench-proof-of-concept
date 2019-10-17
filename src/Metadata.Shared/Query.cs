using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(8)]
    public sealed class Query : MetadataObject
    {
        public Query() : base(8) { }
        public Query(Guid key) : base(8, key) { }
        /// <summary>
        /// Namespace owning this query
        /// </summary>
        public Namespace Namespace { get; set; }
        /// <summary>
        /// Entity owning this query (can be null if owner is a namespace)
        /// </summary>
        public Entity Entity { get; set; }
        /// <summary>
        /// JSON serialized abstract syntax tree of the query
        /// </summary>
        public string ParseTree { get; set; }
        /// <summary>
        /// Data type of query (input data)
        /// </summary>
        public Entity RequestType { get; set; }
        /// <summary>
        /// Data type of query (output data)
        /// </summary>
        public Entity ResponseType { get; set; }
    }
}
