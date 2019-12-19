using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public sealed class Namespace : Entity
    {
        private Domain _domain;
        private Namespace _parent;
        public Domain Domain
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
                while (test.Parent != null)
                {
                    test = test.Parent;
                }
                return test.Domain;
            }
        }
        public Namespace Parent
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
        public List<Namespace> Children { get; } = new List<Namespace>();
        public List<Entity> Entities { get; } = new List<Entity>();
        public void Add(Entity entity)
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
        public void Add(Namespace child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (Children.Contains(child)) return;
            if (Children.Where(i => i.Name == child.Name).FirstOrDefault() != null) return;
            child.Parent = this;
            Children.Add(child);
        }
        public Namespace AddNamespace(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name));
            Namespace child = Children.Where(i => i.Name == name).FirstOrDefault();
            if (child != null)
            {
                return child;
            }
            child = new Namespace()
            {
                Name = name,
                Parent = this
            };
            Children.Add(child);
            return child;
        }
        public T AddEntity<T>(string name) where T : Entity, new()
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name));
            Entity entity = Entities.Where(i => i.Name == name).FirstOrDefault();
            if (entity != null)
            {
                return (T)entity;
            }
            entity = new T() { Name = name };
            if (entity is SimpleEntity simple)
            {
                simple.Namespace = this;
            }
            else if (entity is ComplexEntity complex)
            {
                complex.Namespace = this;
            }
            Entities.Add(entity);
            return (T)entity;
        }
    }
}