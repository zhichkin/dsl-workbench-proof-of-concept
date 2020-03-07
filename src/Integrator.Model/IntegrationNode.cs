using System.Collections.Generic;

namespace OneCSharp.Integrator.Model
{
    public sealed class IntegrationNode
    {
        public IntegrationNode Parent { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public List<IntegrationNode> Nodes { get; } = new List<IntegrationNode>();
    }
}