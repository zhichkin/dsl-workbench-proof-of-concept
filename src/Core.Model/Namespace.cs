using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public sealed class Namespace : Entity, IHierarchy
    {
        private Entity _owner;
        public Entity Owner
        {
            get { return _owner; }
            set
            {
                if (value != null
                    && !(value is Domain)
                    && !(value is Namespace))
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _owner = value;
            }
        }
        [Hierarchy] public List<Namespace> Namespaces { get; } = new List<Namespace>();
        [Hierarchy] public List<Entity> Entities { get; } = new List<Entity>();
        public void AddChild(Entity child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (child is Namespace)
            {
                Add((Namespace)child);
            }
            else
            {
                Add(child);
            }
        }
        private void Add(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
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
        private void Add(Namespace child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (Namespaces.Contains(child)) return;
            if (Namespaces.Where(i => i.Name == child.Name).FirstOrDefault() != null) return;
            child.Owner = this;
            Namespaces.Add(child);
        }
    }
}