using System;
using System.Collections.Generic;

namespace OneCSharp.Core
{
    public class Property : Entity
    {
        public ComplexEntity Owner { get; set; }
        public bool IsOptional { get; set; }
        public bool IsOneToMany { get; set; }
        public List<Entity> ValueTypes { get; } = new List<Entity>();
        public void AddValueType(Entity type)
        {
            if (ValueTypes.Contains(type)) return;
            ValueTypes.Add(type);
        }
    }
}