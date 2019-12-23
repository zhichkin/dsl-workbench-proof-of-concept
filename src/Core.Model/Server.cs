using System.Collections.Generic;

namespace OneCSharp.Core
{
    public class Server : Entity
    {
        public string Address { get; set; }
        [PropertyPurpose(PropertyPurpose.Children)] public List<Domain> Domains { get; } = new List<Domain>();
    }
}