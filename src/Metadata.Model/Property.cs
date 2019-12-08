using System.Collections.Generic;

namespace OneCSharp.Metadata.Model
{
    public interface IProperty
    {
        public IEntity Parent { get; set; }
        public string Name { get; set; }
        public string DbName { get; set; }
        public List<ITypeInfo> Types { get; }
        public List<IField> Fields { get; }
    }
    public sealed class Property : IProperty
    {
        public IEntity Parent { get; set; }
        public string Name { get; set; }
        public string DbName { get; set; }
        public List<ITypeInfo> Types { get; } = new List<ITypeInfo>();
        public List<IField> Fields { get; } = new List<IField>();
    }
}
