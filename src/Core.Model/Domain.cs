using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.Core
{
    public class Domain : Entity
    {
        public List<Namespace> Namespaces { get; } = new List<Namespace>();
        public void Add(Namespace child)
        {
            if (Namespaces.Contains(child)) return;
            if (Namespaces.Where(i => i.Name == child.Name).FirstOrDefault() != null) return;
            child.Domain = this;
            Namespaces.Add(child);
        }
    }
}