using System.Collections.Generic;

namespace OneCSharp.Integrator.Model
{
    public sealed class IntegratorModuleSettings
    {
        public List<WebServerSettings> Nodes { get; set; } = new List<WebServerSettings>();
    }
    public class WebServerSettings
    {
        public string Name { get; set; } = "";
        public string HttpHost { get; set; } = "http://localhost:5000";
        public string DataHost { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? HttpHost : Name;
        }
    }
}