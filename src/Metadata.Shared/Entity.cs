using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(3)]
    public sealed class Entity : MetadataObject
    {
        public Entity() : base(3) { }
        public Entity(Guid key) : base(3, key) { }
        ///<summary>Type code of the entity</summary>
        public int Code { get; set; }
        public ReferenceObject Table { get; set; }
        public ReferenceObject Namespace { get; set; }
        ///<summary>Nesting entity reference</summary>
        public ReferenceObject Owner { get; set; }
        ///<summary>Inheritance: base entity reference</summary>
        public ReferenceObject Parent { get; set; }
        public bool IsSealed { get; set; }
        public bool IsAbstract { get; set; }
    }
}
