using System.Collections.Generic;

namespace OneCSharp.Metadata.Model
{
    public interface IDomain
    {
        public IServer Server { get; set; }
        public string Name { get; set; }
        public string Database { get; set; }
        public List<INamespace> Namespaces { get; }
    }
    public sealed class Domain : IDomain
    {
        public IServer Server { get; set; }
        public string Name { get; set; }
        public string Database { get; set; }
        public List<INamespace> Namespaces { get; } = new List<INamespace>();
    }
}
