using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace OneCSharp.Metadata
{
    public sealed class MetadataServer
    {
        private readonly string _serverAddress;
        private ILogger _logger;
        private Dictionary<string, DbObject> _UUIDs = new Dictionary<string, DbObject>();
        private Dictionary<string, DBNameEntry> _DBNames = new Dictionary<string, DBNameEntry>();
        
        public MetadataServer(string serverAddress) { _serverAddress = serverAddress; }
        public string ServerAddress { get { return _serverAddress; } }
        public void UseLogger(ILogger logger) { _logger = logger; }
        public List<InfoBase> GetInfoBases()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder()
            {
                DataSource = this.ServerAddress,
                IntegratedSecurity = true,
                PersistSecurityInfo = false
            };
            MetadataReader reader = new MetadataReader()
            {
                ConnectionString = csb.ConnectionString
            };
            return reader.GetInfoBases();
        }
        internal Dictionary<string, DbObject> UUIDs { get { return _UUIDs; } }
        internal Dictionary<string, DBNameEntry> DBNames { get { return _DBNames; } }
        public void ImportMetadata(InfoBase infoBase) { ImportMetadata(infoBase, false); }
        public void ImportMetadata(InfoBase infoBase, bool saveFiles)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder()
            {
                DataSource = this.ServerAddress,
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
                DataSource = this.ServerAddress,
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
    }
}
