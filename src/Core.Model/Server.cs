using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public class Server : Entity, IHierarchy
    {
        public string Address { get; set; }
        [Hierarchy] public List<Domain> Domains { get; } = new List<Domain>();
        public void AddChild(Entity child)
        {
            if (!(child is Domain)) throw new ArgumentOutOfRangeException(nameof(child));
            Add((Domain)child);
        }
        private void Add(Domain child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (Domains.Contains(child)) return;
            if (Domains.Where(i => i.Name == child.Name).FirstOrDefault() != null) return;
            child.Server = this;
            Domains.Add(child);
        }
    }
}