using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public class Domain : Entity, IHaveChildren
    {
        public List<Namespace> Namespaces { get; } = new List<Namespace>();
        public IEnumerable Children
        {
            get
            {
                return Namespaces;
            }
        }
        public void AddChild(Entity child)
        {
            if (!(child is Namespace)) throw new ArgumentOutOfRangeException(nameof(child));
            Add((Namespace)child);
        }
        public void Add(Namespace child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (Namespaces.Contains(child)) return;
            if (Namespaces.Where(i => i.Name == child.Name).FirstOrDefault() != null) return;
            child.Domain = this;
            Namespaces.Add(child);
        }
        public Namespace AddNamespace(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name));
            Namespace child = Namespaces.Where(i => i.Name == name).FirstOrDefault();
            if (child != null)
            {
                return child;
            }
            child = new Namespace()
            {
                Name = name,
                Domain = this
            };
            Namespaces.Add(child);
            return child;
        }
    }
}