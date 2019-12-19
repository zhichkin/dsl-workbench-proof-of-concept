using System.Collections.Generic;

namespace OneCSharp.Core
{
    public class Server : Entity
    {
        public string Address { get; set; }
        public List<Domain> Domains { get; } = new List<Domain>();
    }
}