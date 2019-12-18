using System.Collections.Generic;

namespace OneCSharp.Core
{
    public class Property : Entity
    {
        public ComplexEntity Owner { get; set; }
        public bool IsOptional { get; set; }
        public bool IsOneToMany { get; set; }
        public List<Entity> Types { get; } = new List<Entity>();
        public void Add(Entity type)
        {
            if (Types.Contains(type)) return;
            Types.Add(type);
        }
    }
}