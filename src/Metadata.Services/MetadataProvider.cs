using Microsoft.Data.SqlClient;
using OneCSharp.Metadata.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace OneCSharp.Metadata.Services
{
    public interface IMetadataProvider
    {
        public List<IServer> Servers { get; }
        public void AddServer(IServer server);
        public List<IDomain> GetDomains(IServer server);
        public bool CheckServerConnection(IServer server);
        public void ImportMetadata(IDomain node);
    }
    public sealed class MetadataProvider : IMetadataProvider
    {
        private readonly List<IServer> _servers = new List<IServer>();
        private ILogger _logger;
        private Dictionary<string, Entity> _UUIDs = new Dictionary<string, Entity>();
        private Dictionary<string, DBNameEntry> _DBNames = new Dictionary<string, DBNameEntry>();
        
        public MetadataProvider() { }
        
        public void UseLogger(ILogger logger) { _logger = logger; }
        
        internal Dictionary<string, Entity> UUIDs { get { return _UUIDs; } }
        internal Dictionary<string, DBNameEntry> DBNames { get { return _DBNames; } }
        public void ImportMetadata(IDomain domain) { ImportMetadata(domain, false); }
        public void ImportMetadata(IDomain domain, bool saveFiles)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder()
            {
                DataSource = domain.Server.Address,
                InitialCatalog = domain.Database,
                IntegratedSecurity = true,
                PersistSecurityInfo = false
            };
            MetadataReader reader = new MetadataReader()
            {
                ConnectionString = csb.ConnectionString
            };
            if (saveFiles)
            {
                reader.UseLogger(_logger);
            }
            reader.ReadDBNames(_DBNames);

            if (_DBNames.Count > 0)
            {
                foreach (var item in _DBNames)
                {
                    reader.ReadConfig(this, item.Key, (Domain)domain);
                }
                reader.MakeSecondPass((Domain)domain, this);
                reader.ReadSQLMetadata((Domain)domain);
            }
        }
        public Domain GetDomain(Entity dbo)
        {
            Type type;
            object parent = GetParent(dbo);
            while (parent != null)
            {
                type = parent.GetType();
                if (type == typeof(Entity))
                {
                    parent = GetParent((Entity)parent);
                }
                else if (type == typeof(Namespace))
                {
                    parent = GetParent((Namespace)parent);
                }
                else if (type == typeof(Domain))
                {
                    return (Domain)parent;
                }
            }
            return null;
        }
        private object GetParent(Namespace child)
        {
            object parent;
            if (child.Parent == null)
            {
                parent = child.Domain;
            }
            else
            {
                parent = child.Parent;
            }
            return parent;
        }
        private object GetParent(Entity child)
        {
            object parent;
            if (child.Owner == null)
            {
                parent = child.Parent;
            }
            else
            {
                parent = child.Owner;
            }
            return parent;
        }
        public void SaveMetadataToFile(Entity dbo)
        {
            Domain domain = GetDomain(dbo);
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder()
            {
                DataSource = domain.Server.Address,
                InitialCatalog = domain.Database,
                IntegratedSecurity = true,
                PersistSecurityInfo = false
            };
            MetadataReader reader = new MetadataReader()
            {
                ConnectionString = csb.ConnectionString
            };
            reader.UseLogger(_logger);
            reader.SaveOCSObjectToFile(dbo, this);
        }



        public string CatalogPath { get; set; }
        public void Initialize()
        {
            if (CatalogPath == null)
            {
                CatalogPath = "C:\\one-c-sharp";
                if (!Directory.Exists(CatalogPath))
                {
                    Directory.CreateDirectory(CatalogPath);
                }
            }

            FileStream settings;
            string settingsPath = Path.Combine(CatalogPath, "settings.json");
            if (File.Exists(settingsPath))
            {
                settings = File.OpenRead(settingsPath);
            }
            else
            {
                settings = File.Create(settingsPath);
            }
            StreamReader reader = new StreamReader(settings);
        }



        public List<IServer> Servers { get { return _servers; } }
        public void AddServer(IServer server) { _servers.Add(server); }
        public List<IDomain> GetDomains(IServer server)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder()
            {
                DataSource = server.Address,
                IntegratedSecurity = true,
                PersistSecurityInfo = false
            };
            MetadataReader reader = new MetadataReader()
            {
                ConnectionString = csb.ConnectionString
            };
            return reader.GetDomains();
            //server.OCSNodes = reader.GetOCSNodes();
            //foreach (var ib in server.OCSNodes)
            //{
            //    ib.Server = server;
            //}
            //return server.OCSNodes;
        }
        public bool CheckServerConnection(IServer server)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder()
            {
                DataSource = server.Address,
                IntegratedSecurity = true,
                PersistSecurityInfo = false
            };

            bool result = false;
            {
                SqlConnection connection = new SqlConnection(csb.ToString());
                try
                {
                    connection.Open();
                    result = (connection.State == ConnectionState.Open);
                }
                catch
                {
                    // TODO: handle or log the error
                }
                finally
                {
                    if (connection != null) connection.Dispose();
                }
            }
            return result;
        }
    }
}
