using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public class Domain : Entity
    {
        public Server Owner { get; set; }
        public List<Namespace> Namespaces { get; } = new List<Namespace>();
    }
}