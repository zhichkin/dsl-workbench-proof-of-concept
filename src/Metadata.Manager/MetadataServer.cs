using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace OneCSharp.Metadata
{
    public sealed class MetadataServer
    {
        private readonly string _serverAddress;
        private ILogger _logger;
        private Dictionary<string, List<DBName>> _DBNames = new Dictionary<string, List<DBName>>();
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
        public void ImportMetadata(InfoBase infoBase)
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
            reader.UseLogger(_logger);
            reader.ReadDBNames(_DBNames);

            if (_DBNames.Count > 0)
            {
                //reader.ReadConfig(infoBase);
                //reader.ReadSQLMetadata(infoBase);
            }
        }
    }
}
