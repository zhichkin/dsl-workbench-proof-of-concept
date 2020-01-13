using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public sealed class Namespace : Entity
    {
        public Entity Owner { get; set; } // Domain | Namespace
        public List<DataType> DataTypes { get; } = new List<DataType>();
        public List<Namespace> Namespaces { get; } = new List<Namespace>();
        public List<Interface> Interfaces { get; } = new List<Interface>();
    }
}