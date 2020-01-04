using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public class Server : Entity
    {
        public string Address { get; set; }
        public List<Domain> Domains { get; } = new List<Domain>();
    }
}