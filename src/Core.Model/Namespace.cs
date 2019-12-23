using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public sealed class Namespace : Entity, IHaveChildren
    {
        private Domain _domain;
        private Namespace _parent;
        [PropertyPurpose(PropertyPurpose.Hierarchy)] public Domain Domain
        {
            set
            {
                if (_parent != null)
                {
                    _parent = null;
                }
                _domain = value;
            }
            get
            {
                if (_parent == null) return _domain;

                Namespace test = this;
                while (test.Owner != null)
                {
                    test = test.Owner;
                }
                return test.Domain;
            }
        }
        [PropertyPurpose(PropertyPurpose.Hierarchy)] public Namespace Owner
        {
            get { return _parent; }
            set
            {
                if (_domain != null)
                {
                    _domain = null;
                }
                _parent = value;
            }
        }
        [PropertyPurpose(PropertyPurpose.Children)] public List<Namespace> Namespaces { get; } = new List<Namespace>();
        [PropertyPurpose(PropertyPurpose.Children)] public List<Entity> Entities { get; } = new List<Entity>();
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