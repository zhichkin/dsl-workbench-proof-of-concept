using System.Collections.Generic;

namespace OneCSharp.Metadata.Model
{
    public interface INamespace
    {
        public IDomain Domain { get; set; }
        public INamespace Parent { get; set; }
        public string Name { get; set; }
        public List<INamespace> Namespaces { get; }
        public List<IEntity> Entities { get; }
        public List<IRequest> Requests { get; }
    }
    public sealed class Namespace : INamespace
    {
        public IDomain Domain { get; set; }
        public INamespace Parent { get; set; }
        public string Name { get; set; }
        public List<INamespace> Namespaces { get; } = new List<INamespace>();
        public List<IEntity> Entities { get; } = new List<IEntity>();
        public List<IRequest> Requests { get; set; } = new List<IRequest>();
    }
}
