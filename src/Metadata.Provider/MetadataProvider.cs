using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace OneCSharp.Metadata
{
    public sealed class MetadataProvider
    {
        private readonly List<DbServer> _servers = new List<DbServer>();
        private ILogger _logger;
        private Dictionary<string, DbObject> _UUIDs = new Dictionary<string, DbObject>();
        private Dictionary<string, DBNameEntry> _DBNames = new Dictionary<string, DBNameEntry>();
        
        public MetadataProvider() { }
        
        public void UseLogger(ILogger logger) { _logger = logger; }
        
        internal Dictionary<string, DbObject> UUIDs { get { return _UUIDs; } }
        internal Dictionary<string, DBNameEntry> DBNames { get { return _DBNames; } }
        public void ImportMetadata(InfoBase infoBase) { ImportMetadata(infoBase, false); }
        public void ImportMetadata(InfoBase infoBase, bool saveFiles)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder()
            {
                DataSource = infoBase.Server.Address,
                InitialCatalog = infoBase.Database,
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
                    reader.ReadConfig(this, item.Key, infoBase);
                }
                reader.MakeSecondPass(infoBase, this);
                reader.ReadSQLMetadata(infoBase);
            }
        }
        public InfoBase GetInfoBase(DbObject dbo)
        {
            Type type;
            object parent = GetParent(dbo);
            while (parent != null)
            {
                type = parent.GetType();
                if (type == typeof(DbObject))
                {
                    parent = GetParent((DbObject)parent);
                }
                else if (type == typeof(Namespace))
                {
                    parent = GetParent((Namespace)parent);
                }
                else if (type == typeof(InfoBase))
                {
                    return (InfoBase)parent;
                }
            }
            return null;
        }
        private object GetParent(Namespace child)
        {
            object parent;
            if (child.Parent == null)
            {
                parent = child.InfoBase;
            }
            else
            {
                parent = child.Parent;
            }
            return parent;
        }
        private object GetParent(DbObject child)
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
        public void SaveMetadataToFile(DbObject dbo)
        {
            InfoBase infoBase = GetInfoBase(dbo);
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder()
            {
                DataSource = infoBase.Server.Address,
                InitialCatalog = infoBase.Database,
                IntegratedSecurity = true,
                PersistSecurityInfo = false
            };
            MetadataReader reader = new MetadataReader()
            {
                ConnectionString = csb.ConnectionString
            };
            reader.UseLogger(_logger);
            reader.SaveDbObjectToFile(dbo, this);
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



        public List<DbServer> Servers { get { return _servers; } }
        public void AddServer(DbServer server) { _servers.Add(server); }
        public List<InfoBase> GetInfoBases(DbServer server)
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
            return reader.GetInfoBases();
            //server.InfoBases = reader.GetInfoBases();
            //foreach (var ib in server.InfoBases)
            //{
            //    ib.Server = server;
            //}
            //return server.InfoBases;
        }
        public bool CheckServerConnection(DbServer server)
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
