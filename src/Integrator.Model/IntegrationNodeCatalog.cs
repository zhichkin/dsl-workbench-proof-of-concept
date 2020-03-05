using System.Collections.Generic;

namespace OneCSharp.Integrator.Model
{
    public sealed class IntegrationNodeCatalog
    {
        public string Name { get; set; }
        public List<IntegrationNode> Nodes { get; } = new List<IntegrationNode>();
        public List<IntegrationNodeCatalog> Catalogs { get; } = new List<IntegrationNodeCatalog>();
    }
}