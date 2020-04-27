using System.Collections.Generic;

namespace OneCSharp.Integrator.Model
{
    public sealed class MetadataSettings
    {
        public string Catalog { get; set; }
        public List<DatabaseServer> Servers { get; set; } = new List<DatabaseServer>();
    }
    public sealed class DatabaseServer
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Database> Databases { get; set; } = new List<Database>();
    }
    public sealed class Database
    {
        public string Name { get; set; }
        public string MetadataFile { get; set; }
    }
}