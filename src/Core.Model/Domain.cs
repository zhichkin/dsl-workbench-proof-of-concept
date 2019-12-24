using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public class Domain : Entity, IHierarchy
    {
        public Server Server { get; set; }
        [Hierarchy] public List<Namespace> Namespaces { get; } = new List<Namespace>();
        public void AddChild(Entity child)
        {
            if (!(child is Namespace)) throw new ArgumentOutOfRangeException(nameof(child));
            Add((Namespace)child);
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