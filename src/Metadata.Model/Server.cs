using System.Collections.Generic;

namespace OneCSharp.Metadata.Model
{
    public interface IServer
    {
        public string Address { get; set; }
        public List<IDomain> Domains { get; }
    }
    public sealed class Server : IServer
    {
        public string Address { get; set; }
        public List<IDomain> Domains { get; } = new List<IDomain>();
    }
}
