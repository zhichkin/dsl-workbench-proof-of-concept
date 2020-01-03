using Microsoft.Data.SqlClient;
using OneCSharp.Core;
using OneCSharp.SQL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OneCSharp.SQL.Services
{
    internal sealed class DBNameEntry
    {
        internal Table Table = new Table();
        internal List<DBName> DBNames = new List<DBName>();
    }
    internal sealed class DBName
    {
        internal string Token;
        internal int TypeCode;
        internal bool IsMainTable;
    }

    internal delegate void SpecialParser(StreamReader reader, string line, Table table);

    public interface IMetadataReader
    {
        bool CheckServerConnection(Server server);
        List<Database> GetDatabases(Server server);
        void ReadMetadata(Database infoBase);
    }
    public sealed class MetadataReader : IMetadataReader
    {
        private readonly ILogger _logger;
        private readonly Dictionary<string, Table> _UUIDs = new Dictionary<string, Table>();
        private readonly Dictionary<string, DBNameEntry> _DBNames = new Dictionary<string, DBNameEntry>();
        public MetadataReader()
        {
            _logger = null; //new TextFileLogger();

            _SpecialParsers.Add("cf4abea7-37b2-11d4-940f-008048da11f9", ParseTableProperties); // Catalogs properties collection
            _SpecialParsers.Add("932159f9-95b2-4e76-a8dd-8849fe5c5ded", ParseNestedObjects); // Catalogs nested objects collection
            _SpecialParsers.Add("45e46cbc-3e24-4165-8b7b-cc98a6f80211", ParseTableProperties); // Documents properties collection
            _SpecialParsers.Add("21c53e09-8950-4b5e-a6a0-1054f1bbc274", ParseNestedObjects); // Documents nested objects collection

            _SpecialParsers.Add("13134203-f60b-11d5-a3c7-0050bae0a776", ParseTableProperties); // Коллекция измерений регистра сведений
            _SpecialParsers.Add("13134202-f60b-11d5-a3c7-0050bae0a776", ParseTableProperties); // Коллекция ресурсов регистра сведений
            _SpecialParsers.Add("a2207540-1400-11d6-a3c7-0050bae0a776", ParseTableProperties); // Коллекция реквизитов регистра сведений

            _SpecialParsers.Add("b64d9a43-1642-11d6-a3c7-0050bae0a776", ParseTableProperties); // Коллекция измерений регистра накопления
            _SpecialParsers.Add("b64d9a41-1642-11d6-a3c7-0050bae0a776", ParseTableProperties); // Коллекция ресурсов регистра накопления
            _SpecialParsers.Add("b64d9a42-1642-11d6-a3c7-0050bae0a776", ParseTableProperties); // Коллекция реквизитов регистра накопления
        }
        internal string ConnectionString { get; set; }
        public bool CheckServerConnection(Server server)
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
        private void WriteBinaryDataToFile(Stream binaryData, string fileName)
        {
            string filePath = Path.Combine(_logger.CatalogPath, fileName);
            using (FileStream output = File.Create(filePath))
            {
                binaryData.CopyTo(output);
            }
        }
        public List<Database> GetDatabases(Server server)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder()
            {
                DataSource = server.Address,
                IntegratedSecurity = true,
                PersistSecurityInfo = false
            };
            ConnectionString = csb.ConnectionString;

            List<Database> list = new List<Database>();

            { // limited scope for variables declared in it - using statement does like that - used here to get control over catch block
                SqlConnection connection = new SqlConnection(ConnectionString);
                SqlCommand command = connection.CreateCommand();
                SqlDataReader reader = null;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT database_id, name FROM sys.databases WHERE name NOT IN ('master', 'model', 'msdb', 'tempdb', 'Resource', 'distribution', 'reportserver', 'reportservertempdb');";
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new Database()
                        {
                            Name = reader.GetString(1),
                            Alias = string.Empty
                        });
                    }
                }
                catch (Exception error)
                {
                    // TODO: log error
                    _ = error.Message;
                }
                finally
                {
                    if (reader != null)
                    {
                        if (reader.HasRows) command.Cancel();
                        reader.Dispose();
                    }
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Dispose();
                }
            } // end of limited scope

            return list;
        }
        public void ReadMetadata(Database infoBase)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder()
            {
                DataSource = infoBase.Server.Address,
                InitialCatalog = infoBase.Name,
                IntegratedSecurity = true,
                PersistSecurityInfo = false
            };
            ConnectionString = csb.ConnectionString;
            
            ReadDBNames();
            if (_DBNames.Count > 0)
            {
                foreach (var item in _DBNames)
                {
                    ReadConfig(item.Key, infoBase);
                }
                MakeSecondPass(infoBase);
                ReadSQLMetadata(infoBase);
            }
        }
        
        # region " Read DBNames "
        internal void ReadDBNames()
        {
            SqlBytes binaryData = GetDBNamesFromDatabase();
            if (binaryData == null) return;

            DeflateStream stream = new DeflateStream(binaryData.Stream, CompressionMode.Decompress);

            if (_logger == null)
            {
                ParseDBNames(stream);
            }
            else
            {
                MemoryStream memory = new MemoryStream();
                stream.CopyTo(memory);
                memory.Seek(0, SeekOrigin.Begin);
                WriteBinaryDataToFile(memory, "DBNames.txt");
                memory.Seek(0, SeekOrigin.Begin);
                ParseDBNames(memory);
            }
        }
        private SqlBytes GetDBNamesFromDatabase()
        {
            SqlBytes binaryData = null;

            { // limited scope for variables declared in it - using statement does like that - used here to get control over catch block
                SqlConnection connection = new SqlConnection(ConnectionString);
                SqlCommand command = connection.CreateCommand();
                SqlDataReader reader = null;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT BinaryData FROM Params WHERE FileName = N'DBNames'";
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        binaryData = reader.GetSqlBytes(0);
                    }
                }
                catch (Exception error)
                {
                    // TODO: log error
                    _ = error.Message;
                }
                finally
                {
                    if (reader != null)
                    {
                        if (reader.HasRows) command.Cancel();
                        reader.Dispose();
                    }
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Dispose();
                }
            } // end of limited scope

            return binaryData;
        }
        private void ParseDBNames(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    int capacity = GetDBNamesCapacity(line);
                    while ((line = reader.ReadLine()) != null)
                    {
                        ParseDBNameLine(line);
                    }
                }
            }
        }
        private int GetDBNamesCapacity(string line)
        {
            return int.Parse(line.Replace("{", string.Empty).Replace(",", string.Empty));
        }
        private void ParseDBNameLine(string line)
        {
            string[] items = line.Split(',');
            if (items.Length < 3) return;

            string FileName = items[0].Replace("{", string.Empty);
            DBName dbname = new DBName()
            {
                Token = items[1].Replace("\"", string.Empty),
                TypeCode = int.Parse(items[2].Replace("}", string.Empty))
            };
            dbname.IsMainTable = IsMainTable(dbname.Token);

            if (_DBNames.TryGetValue(FileName, out DBNameEntry entry))
            {
                entry.DBNames.Add(dbname);
            }
            else
            {
                entry = new DBNameEntry();
                entry.DBNames.Add(dbname);
                _DBNames.Add(FileName, entry);
            }
        }
        private bool IsMainTable(string token)
        {
            switch (token)
            {
                case DBToken.VT: return true;
                case DBToken.Enum: return true;
                case DBToken.Const: return true;
                case DBToken.InfoRg: return true;
                case DBToken.AccumRg: return true;
                case DBToken.Document: return true;
                case DBToken.Reference: return true;
            }
            return false;
        }
        #endregion

        #region " Read Config "
        private readonly Regex rxUUID = new Regex("[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}"); // Example: eb3dfdc7-58b8-4b1f-b079-368c262364c9
        private readonly Regex rxSpecialUUID = new Regex("^{[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12},\\d+(?:})?,$"); // Example: {3daea016-69b7-4ed4-9453-127911372fe6,0}, | {cf4abea7-37b2-11d4-940f-008048da11f9,5,
        private readonly Regex rxOCSName = new Regex("^{\\d,\\d,[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}},\"\\w+\",$");
        private readonly Regex rxOCSType = new Regex("^{\"[#BSDN]\""); // Example: {"#",1aaea747-a4ba-4fb2-9473-075b1ced620c}, | {"B"}, | {"S",10,0}, | {"D","T"}, | {"N",10,0,1}
        private readonly Regex rxNestedProperties = new Regex("^{888744e1-b616-11d4-9436-004095e12fc7,\\d+[},]$"); // look rxSpecialUUID
        private readonly Dictionary<string, SpecialParser> _SpecialParsers = new Dictionary<string, SpecialParser>();
        public void ReadConfig(string fileName, Database infoBase)
        {
            if (new Guid(fileName) == Guid.Empty) return;

            SqlBytes binaryData = ReadConfigFromDatabase(fileName);
            if (binaryData == null) return;

            DeflateStream stream = new DeflateStream(binaryData.Stream, CompressionMode.Decompress);
            if (_logger == null)
            {
                ParseMetadataObject(stream, fileName, infoBase);
            }
            else
            {
                MemoryStream memory = new MemoryStream();
                stream.CopyTo(memory);
                memory.Seek(0, SeekOrigin.Begin);
                WriteBinaryDataToFile(memory, $"{fileName}.txt");
                memory.Seek(0, SeekOrigin.Begin);
                ParseMetadataObject(memory, fileName, infoBase);
            }
        }
        private SqlBytes ReadConfigFromDatabase(string fileName)
        {
            SqlBytes binaryData = null;

            { // limited scope for variables declared in it - using statement does like that - used here to get control over catch block
                SqlConnection connection = new SqlConnection(ConnectionString);
                SqlCommand command = connection.CreateCommand();
                SqlDataReader reader = null;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT BinaryData FROM Config WHERE FileName = @FileName ORDER BY PartNo ASC";
                command.Parameters.AddWithValue("FileName", fileName);
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        binaryData = reader.GetSqlBytes(0);
                    }
                }
                catch (Exception error)
                {
                    // TODO: log error
                    _ = error.Message;
                }
                finally
                {
                    if (reader != null)
                    {
                        if (reader.HasRows) command.Cancel();
                        reader.Dispose();
                    }
                    if (command != null) command.Dispose();
                    if (connection != null) connection.Dispose();
                }
            } // end of limited scope

            return binaryData;
        }
        private void ParseMetadataObject(Stream stream, string fileName, Database infoBase)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    line = reader.ReadLine();
                }

                Table table;
                string token = string.Empty;
                if (_DBNames.TryGetValue(fileName, out DBNameEntry entry))
                {
                    table = entry.Table;
                    DBName dbname = entry.DBNames.Where(i => i.Token == DBToken.Enum).FirstOrDefault();
                    if (dbname != null)
                    {
                        token = DBToken.Enum;
                    }
                }
                else
                {
                    table = new Table();
                }
                try
                {
                    ParseInternalIdentifier(line, table, token);
                }
                catch (Exception error)
                {
                    _ = error.Message;
                    return;
                }

                _ = reader.ReadLine();
                _ = reader.ReadLine();
                line = reader.ReadLine();
                if (line != null)
                {
                    ParseTableNames(line, fileName, table);
                    SetTableNamespace(table, infoBase, token);
                }
                if (token == DBToken.Reference)
                {
                    ParseReferenceOwner(reader, table);
                }

                int count = 0;
                string UUID = null;
                Match match = null;
                while ((line = reader.ReadLine()) != null)
                {
                    match = rxSpecialUUID.Match(line);
                    if (!match.Success) continue;

                    string[] lines = line.Split(',');
                    UUID = lines[0].Replace("{", string.Empty);
                    count = int.Parse(lines[1].Replace("}", string.Empty));
                    if (count == 0) continue;

                    if (_SpecialParsers.ContainsKey(UUID))
                    {
                        _SpecialParsers[UUID](reader, line, table);
                    }
                }
            }
        }
        private void ParseInternalIdentifier(string line, Table table, string token)
        {
            string[] lines = line.Split(',');
            string UUID = (token == DBToken.Enum ? lines[1] : lines[3]);
            _UUIDs.Add(UUID, table);
        }
        private void ParseTableNames(string line, string fileName, Table table)
        {
            string[] lines = line.Split(',');
            string FileName = lines[2].Replace("}", string.Empty);
            if (fileName != FileName)
            {
                // TODO: error ?
            }
            table.Alias = lines[3].Replace("\"", string.Empty);
            if (_DBNames.TryGetValue(fileName, out DBNameEntry entry))
            {
                DBName dbname = entry.DBNames.Where(i => i.IsMainTable).FirstOrDefault();
                if (dbname != null)
                {
                    table.TypeCode = dbname.TypeCode;
                    table.Name = CreateTableName(table, dbname);
                }
            }
        }
        private string CreateTableName(Table table, DBName dbname)
        {
            if (dbname.Token == DBToken.VT)
            {
                if (table.Owner == null)
                {
                    return string.Empty;
                    // TODO: error ?
                }
                else
                {
                    return $"{table.Owner.Name}_{dbname.Token}{dbname.TypeCode}";
                }
            }
            else
            {
                return $"_{dbname.Token}{dbname.TypeCode}";
            }
        }
        private void SetTableNamespace(Table table, Database infoBase, string token)
        {
            if (table.Namespace != null) return;

            Namespace ns = infoBase.Namespaces.Where(n => n.Name == token).FirstOrDefault();
            if (ns == null)
            {
                if (string.IsNullOrEmpty(token))
                {
                    ns = infoBase.Namespaces.Where(n => n.Name == "Unknown").FirstOrDefault();
                    if (ns == null)
                    {
                        ns = new Namespace() { Name = "Unknown" };
                        infoBase.AddChild(ns);
                    }
                }
                else
                {
                    ns = new Namespace() { Name = token };
                    infoBase.AddChild(ns);
                }
            }
            ns.AddChild(table);
        }
        private void ParseReferenceOwner(StreamReader reader, Table table)
        {
            int count = 0;
            string[] lines;

            _ = reader.ReadLine(); // строка описания - "Синоним" в терминах 1С
            _ = reader.ReadLine();
            string line = reader.ReadLine();
            if (line != null)
            {
                lines = line.Split(',');
                count = int.Parse(lines[1].Replace("}", string.Empty));
            }
            if (count == 0) return;

            Match match;
            List<TypeInfo> types = new List<TypeInfo>();
            for (int i = 0; i < count; i++)
            {
                _ = reader.ReadLine();
                line = reader.ReadLine();
                if (line == null) return;

                match = rxUUID.Match(line);
                if (match.Success)
                {
                    if (_DBNames.TryGetValue(match.Value, out DBNameEntry entry))
                    {
                        types.Add(new TypeInfo()
                        {
                            TypeCode = entry.Table.TypeCode,
                            Name = entry.Table.Name,
                            Entity = entry.Table
                        });
                    }
                }
                _ = reader.ReadLine();
            }

            if (types.Count > 0)
            {
                TableProperty property = new TableProperty
                {
                    Name = "Владелец",
                    DbName = "OwnerID" // [_OwnerIDRRef] | [_OwnerID_TYPE] + [_OwnerID_RTRef] + [_OwnerID_RRRef]
                    // TODO: add Field at once ?
                };
                property.Types.AddRange(types);
                table.AddChild(property);
            }
        }
        private void ParseTableProperties(StreamReader reader, string line, Table table)
        {
            string[] lines = line.Split(',');
            int count = int.Parse(lines[1].Replace("}", string.Empty));
            Match match;
            string nextLine;
            for (int i = 0; i < count; i++)
            {
                while ((nextLine = reader.ReadLine()) != null)
                {
                    match = rxOCSName.Match(nextLine);
                    if (match.Success)
                    {
                        ParseTableProperty(reader, nextLine, table);
                        break;
                    }
                }
            }
        }
        private void ParseTableProperty(StreamReader reader, string line, Table table)
        {
            string[] lines = line.Split(',');
            string fileName = lines[2].Replace("}", string.Empty);
            string objectName = lines[3].Replace("\"", string.Empty);

            TableProperty property = new TableProperty { Name = objectName };
            table.AddChild(property);

            if (_DBNames.TryGetValue(fileName, out DBNameEntry entry))
            {
                if (entry.DBNames.Count == 1)
                {
                    property.DbName = CreateTableFieldName(entry.DBNames[0]);
                }
                else if (entry.DBNames.Count > 1)
                {
                    foreach (var dbn in entry.DBNames.Where(dbn => dbn.Token == DBToken.Fld))
                    {
                        property.DbName = CreateTableFieldName(dbn);
                    }
                }
            }
            ParseTablePropertyTypes(reader, property);
        }
        private string CreateTableFieldName(DBName dbname)
        {
            return $"{dbname.Token}{dbname.TypeCode}";
        }
        private void ParseTablePropertyTypes(StreamReader reader, TableProperty property)
        {
            string line = reader.ReadLine();
            if (line == null) return;

            while (line != "{\"Pattern\",")
            {
                line = reader.ReadLine();
                if (line == null) return;
            }

            Match match;
            while ((line = reader.ReadLine()) != null)
            {
                match = rxOCSType.Match(line);
                if (!match.Success) break;

                int typeCode = 0;
                string typeName = string.Empty;
                string token = match.Value.Replace("{", string.Empty).Replace("\"", string.Empty);
                switch (token)
                {
                    case DBToken.B: { typeCode = -1; typeName = "Boolean"; break; }
                    case DBToken.S: { typeCode = -2; typeName = "String"; break; }
                    case DBToken.D: { typeCode = -3; typeName = "DateTime"; break; }
                    case DBToken.N: { typeCode = -4; typeName = "Numeric"; break; }
                }
                if (typeCode != 0)
                {
                    property.Types.Add(new TypeInfo()
                    {
                        Name = typeName,
                        TypeCode = typeCode
                    });
                }
                else
                {
                    string[] lines = line.Split(',');
                    string UUID = lines[1].Replace("}", string.Empty);

                    if (UUID == "e199ca70-93cf-46ce-a54b-6edc88c3a296")
                    {
                        // ХранилищеЗначения - varbinary(max)
                        property.Types.Add(new TypeInfo()
                        {
                            Name = "BLOB",
                            TypeCode = -5
                        });
                    }
                    else if (UUID == "fc01b5df-97fe-449b-83d4-218a090e681e")
                    {
                        // УникальныйИдентификатор - binary(16)
                        property.Types.Add(new Model.TypeInfo()
                        {
                            Name = "UUID",
                            TypeCode = -6
                        });
                    }
                    else if (_UUIDs.TryGetValue(UUID, out Table table))
                    {
                        property.Types.Add(new TypeInfo()
                        {
                            Name = table.Name,
                            TypeCode = table.TypeCode,
                            UUID = UUID,
                            Entity = table
                        });
                    }
                    else // UUID is not loaded yet - leave it for second pass
                    {
                        property.Types.Add(new TypeInfo()
                        {
                            UUID = UUID
                        });
                    }
                }
            }
        }
        private void ParseNestedObjects(StreamReader reader, string line, Table table)
        {
            string[] lines = line.Split(',');
            int count = int.Parse(lines[1]);
            Match match;
            string nextLine;
            for (int i = 0; i < count; i++)
            {
                while ((nextLine = reader.ReadLine()) != null)
                {
                    match = rxOCSName.Match(nextLine);
                    if (match.Success)
                    {
                        ParseNestedObject(reader, nextLine, table);
                        break;
                    }
                }
            }
        }
        private void ParseNestedObject(StreamReader reader, string line, Table table)
        {
            string[] lines = line.Split(',');
            string fileName = lines[2].Replace("}", string.Empty);
            string objectName = lines[3].Replace("\"", string.Empty);

            Table nested = new Table()
            {
                Alias = objectName,
                Namespace = table.Namespace
            };
            table.AddChild(nested);

            if (_DBNames.TryGetValue(fileName, out DBNameEntry entry))
            {
                DBName dbname = entry.DBNames.Where(i => i.IsMainTable).FirstOrDefault();
                if (dbname != null)
                {
                    nested.TypeCode = dbname.TypeCode;
                    nested.Name = CreateTableName(nested, dbname);
                }
            }
            ParseNestedObjectProperties(reader, nested);
        }
        private void ParseNestedObjectProperties(StreamReader reader, Table table)
        {
            string line;
            Match match;
            while ((line = reader.ReadLine()) != null)
            {
                match = rxNestedProperties.Match(line);
                if (match.Success)
                {
                    ParseTableProperties(reader, line, table);
                    break;
                }
            }
        }
        #endregion
        public void MakeSecondPass(Database infoBase)
        {
            foreach (var ns in infoBase.Namespaces)
            {
                foreach (var entity in ns.Entities)
                {
                    Table table = (Table)entity;
                    foreach (var property in table.Properties)
                    {
                        TableProperty tp = (TableProperty)property;
                        foreach (var type in tp.Types)
                        {
                            if (type.TypeCode == 0)
                            {
                                if (type.Entity == null)
                                {
                                    if (!string.IsNullOrEmpty(type.UUID))
                                    {
                                        if (_UUIDs.TryGetValue(type.UUID, out Table dbObject))
                                        {
                                            type.Name = dbObject.Name;
                                            type.Entity = dbObject;
                                            type.TypeCode = dbObject.TypeCode;
                                        }
                                    }
                                }
                                else
                                {
                                    type.Name = type.Entity.Name;
                                    type.TypeCode = ((Table)type.Entity).TypeCode;
                                }
                            }
                        }
                    }
                    foreach (var nested in table.Tables)
                    {
                        foreach (var property in nested.Properties)
                        {
                            TableProperty tp = (TableProperty)property;
                            foreach (var type in tp.Types)
                            {
                                if (type.TypeCode == 0)
                                {
                                    if (type.Entity == null)
                                    {
                                        if (!string.IsNullOrEmpty(type.UUID))
                                        {
                                            if (_UUIDs.TryGetValue(type.UUID, out Table dbObject))
                                            {
                                                type.Name = dbObject.Name;
                                                type.Entity = dbObject;
                                                type.TypeCode = dbObject.TypeCode;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        type.Name = type.Entity.Name;
                                        type.TypeCode = ((Table)type.Entity).TypeCode;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void ReadSQLMetadata(Database infoBase)
        {
            SQLHelper SQL = new SQLHelper();
            SQL.ConnectionString = ConnectionString;
            SQL.Load(infoBase);
        }
    

        internal void SaveTableMetadataToFile(Table table)
        {
            if (_logger == null) return;

            var kv = _DBNames
                .Where(i => i.Value.Table == table)
                .FirstOrDefault();
            string fileName = kv.Key;

            if (new Guid(fileName) == Guid.Empty) return;

            SqlBytes binaryData = ReadConfigFromDatabase(fileName);
            if (binaryData == null) return;

            DeflateStream stream = new DeflateStream(binaryData.Stream, CompressionMode.Decompress);
            WriteBinaryDataToFile(stream, $"{fileName}.txt");
        }
    }
}