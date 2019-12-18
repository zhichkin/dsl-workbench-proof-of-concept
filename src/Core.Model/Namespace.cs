using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public sealed class Namespace : Entity
    {
        public Domain Domain { get; set; }
        public Namespace Parent { get; set; }
        public List<Namespace> Children { get; } = new List<Namespace>();
        public List<Entity> Entities { get; } = new List<Entity>();
        public void Add(Entity entity)
        {
            if (Entities.Contains(entity)) return;
            if (Entities.Where(i => i.Name == entity.Name).FirstOrDefault() != null) return;
            if (entity is SimpleEntity simple)
            {
                simple.Namespace = this;
            }
            else if (entity is ComplexEntity complex)
            {
                complex.Namespace = this;
            }
            Entities.Add(entity);
        }
        public void Add(Namespace child)
        {
            if (Children.Contains(child)) return;
            if (Children.Where(i => i.Name == child.Name).FirstOrDefault() != null) return;
            child.Parent = this;
            child.Domain = Domain;
            Children.Add(child);
        }
    }
}