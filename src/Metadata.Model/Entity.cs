using System.Collections.Generic;

namespace OneCSharp.Metadata.Model
{
    public interface IEntity
    {
        public INamespace Parent { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public int TypeCode { get; set; }
        public string TableName { get; set; }
        public List<IProperty> Properties { get; }
        public IEntity Owner { get; set; }
        public List<IEntity> NestedEntities { get; }
    }
    public sealed class Entity : IEntity
    {
        public INamespace Parent { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public int TypeCode { get; set; }
        public string TableName { get; set; }
        public List<IProperty> Properties { get; } = new List<IProperty>();
        public IEntity Owner { get; set; }
        public List<IEntity> NestedEntities { get; } = new List<IEntity>();
    }
}
