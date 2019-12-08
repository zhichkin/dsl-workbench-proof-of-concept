using Microsoft.Data.SqlClient;
using OneCSharp.Metadata.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OneCSharp.Metadata.Services
{
    internal sealed class DBNameEntry
    {
        internal IEntity OCSObject = new Entity();
        internal List<DBName> DBNames = new List<DBName>();
    }
    internal sealed class DBName
    {
        internal string Token;
        internal int TypeCode;
        internal bool IsMainTable;
    }

    internal delegate void SpecialParser(StreamReader reader, string line, Entity dbo, MetadataProvider server);

    public interface IMetadataReader
    {
        public List<IDomain> GetDomains();
    }
    public sealed class MetadataReader : IMetadataReader
    {
        private ILogger _logger;
        public MetadataReader()
        {
            _SpecialParsers.Add("cf4abea7-37b2-11d4-940f-008048da11f9", ParseOCSProperties); // Catalogs properties collection
            _SpecialParsers.Add("932159f9-95b2-4e76-a8dd-8849fe5c5ded", ParseNestedObjects); // Catalogs nested objects collection
            _SpecialParsers.Add("45e46cbc-3e24-4165-8b7b-cc98a6f80211", ParseOCSProperties); // Documents properties collection
            _SpecialParsers.Add("21c53e09-8950-4b5e-a6a0-1054f1bbc274", ParseNestedObjects); // Documents nested objects collection

            _SpecialParsers.Add("13134203-f60b-11d5-a3c7-0050bae0a776", ParseOCSProperties); // Коллекция измерений регистра сведений
            _SpecialParsers.Add("13134202-f60b-11d5-a3c7-0050bae0a776", ParseOCSProperties); // Коллекция ресурсов регистра сведений
            _SpecialParsers.Add("a2207540-1400-11d6-a3c7-0050bae0a776", ParseOCSProperties); // Коллекция реквизитов регистра сведений

            _SpecialParsers.Add("b64d9a43-1642-11d6-a3c7-0050bae0a776", ParseOCSProperties); // Коллекция измерений регистра накопления
            _SpecialParsers.Add("b64d9a41-1642-11d6-a3c7-0050bae0a776", ParseOCSProperties); // Коллекция ресурсов регистра накопления
            _SpecialParsers.Add("b64d9a42-1642-11d6-a3c7-0050bae0a776", ParseOCSProperties); // Коллекция реквизитов регистра накопления
        }
        internal string ConnectionString { get; set; }
        private void WriteBinaryDataToFile(Stream binaryData, string fileName)
        {
            string filePath = Path.Combine(_logger.CatalogPath, fileName);
            using (FileStream output = File.Create(filePath))
            {
                binaryData.CopyTo(output);
            }
        }
        internal void UseLogger(ILogger logger) { _logger = logger; }
        public List<IDomain> GetDomains()
        {
            List<IDomain> list = new List<IDomain>();

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
                        list.Add(new Domain()
                        {
                            Name = string.Empty,
                            Database = reader.GetString(1)
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

        # region " Read DBNames "
        internal void ReadDBNames(Dictionary<string, DBNameEntry> DBNames)
        {
            SqlBytes binaryData = GetDBNamesFromDatabase();
            if (binaryData == null) return;

            DeflateStream stream = new DeflateStream(binaryData.Stream, CompressionMode.Decompress);

            if (_logger == null)
            {
                ParseDBNames(stream, DBNames);
            }
            else
            {
                MemoryStream memory = new MemoryStream();
                stream.CopyTo(memory);
                memory.Seek(0, SeekOrigin.Begin);
                WriteBinaryDataToFile(memory, "DBNames.txt");
                memory.Seek(0, SeekOrigin.Begin);
                ParseDBNames(memory, DBNames);
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
        private void ParseDBNames(Stream stream, Dictionary<string, DBNameEntry> DBNames)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    int capacity = GetDBNamesCapacity(line);
                    while ((line = reader.ReadLine()) != null)
                    {
                        ParseDBNameLine(line, DBNames);
                    }
                }
            }
        }
        private int GetDBNamesCapacity(string line)
        {
            return int.Parse(line.Replace("{", string.Empty).Replace(",", string.Empty));
        }
        private void ParseDBNameLine(string line, Dictionary<string, DBNameEntry> DBNames)
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

            if (DBNames.TryGetValue(FileName, out DBNameEntry entry))
            {
                entry.DBNames.Add(dbname);
            }
            else
            {
                entry = new DBNameEntry();
                entry.DBNames.Add(dbname);
                DBNames.Add(FileName, entry);
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

        #region " Read DBSchema "
        //internal sealed class DBObject // Regex("^{\"\\w+\",\"[NI]\",\\d+,\"\\w*\","); // Example: {"Reference42","N",42,"", | {"VT5798","I",0,"Reference228",
        //{
        //    internal int ID;
        //    internal string Name;
        //    internal string Token;
        //    internal string FileName;
        //    internal string Owner; // for table parts (I token)
        //    internal List<DBProperty> Properties = new List<DBProperty>();
        //    internal List<DBObject> NestedObjects = new List<DBObject>(); // Nested tables
        //}
        //internal sealed class DBProperty // Regex("^{\"\\w+\",0,"); // Example: {"Period",0,
        //{
        //    internal int ID;
        //    internal string Name;
        //    internal string Token;
        //    internal string FileName;
        //    internal List<DBType> Types = new List<DBType>();
        //}
        //internal sealed class DBType // Regex("^{\"[ENLVBSTR]\",\\d+,\\d+,\"\\w*\",\\d}"); // Example: {"N",5,0,"",0} | {"R",0,0,"Reference188",3}
        //{
        //    internal string Token; // [BENLVSTR]
        //    internal int Size;
        //    internal int Tail;
        //    internal string Name; // reference types only & if not compound (E token)
        //    internal int Kind; // ??? used usually with references
        //}
        //public List<DBObject> ReadDBSchema(string connectionString, DBName[] dbnames)
        //{
        //    SqlBytes binaryData = GetDBSchemaFromDatabase(connectionString);
        //    if (binaryData == null) return null;

        //    WriteBinaryDataToFile(binaryData.Stream, "DBSchema.txt");

        //    binaryData.Stream.Seek(0, SeekOrigin.Begin);
        //    return ParseDBSchema(binaryData.Stream, dbnames);
        //}
        //private SqlBytes GetDBSchemaFromDatabase(string connectionString)
        //{
        //    SqlBytes binaryData = null;

        //    { // limited scope for variables declared in it - using statement does like that - used here to get control over catch block
        //        SqlConnection connection = new SqlConnection(connectionString);
        //        SqlCommand command = connection.CreateCommand();
        //        SqlDataReader reader = null;
        //        command.CommandType = CommandType.Text;
        //        command.CommandText = "SELECT TOP 1 SerializedData FROM DBSchema";
        //        try
        //        {
        //            connection.Open();
        //            reader = command.ExecuteReader();
        //            if (reader.Read())
        //            {
        //                binaryData = reader.GetSqlBytes(0);
        //            }
        //        }
        //        catch (Exception error)
        //        {
        //            // TODO: log error
        //        }
        //        finally
        //        {
        //            if (reader != null)
        //            {
        //                if (reader.HasRows) command.Cancel();
        //                reader.Dispose();
        //            }
        //            if (command != null) command.Dispose();
        //            if (connection != null) connection.Dispose();
        //        }
        //    } // end of limited scope

        //    return binaryData;
        //}
        //private List<DBObject> ParseDBSchema(Stream stream, DBName[] dbnames)
        //{
        //    List<DBObject> result = new List<DBObject>();

        //    Match match = null;
        //    Regex DBTypeRegex = new Regex("^{\"[ENLVBSTR]\",\\d+,\\d+,\"\\w*\",\\d(?:,\\d)?}"); // Example: {"N",5,0,"",0} | {"N",10,0,"",0,1} | {"R",0,0,"Reference188",3}
        //    Regex DBObjectRegex = new Regex("^{\"\\w+\",\"[NI]\",\\d+,\"\\w*\","); // Example: {"Reference42","N",42,"", | {"VT5798","I",0,"Reference228",
        //    Regex DBPropertyRegex = new Regex("^{\"\\w{2,}\",\\d,$"); // Example: {"Period",0, | {"Fld1795",1,

        //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        //    {
        //        string line = null;
        //        int state = 0;
        //        int typesCount = 0;
        //        int propertiesCount = 0;
        //        DBObject currentObject = null;
        //        DBObject currentTable = null;
        //        DBProperty currentProperty = null;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            if (state == 0) // waiting for the next DBObject
        //            {
        //                match = DBObjectRegex.Match(line);
        //                if (match.Success)
        //                {
        //                    DBObject dbo = ParseDBObject(line, dbnames);
        //                    if (dbo.Owner == string.Empty)
        //                    {
        //                        currentObject = dbo;
        //                        result.Add(currentObject);
        //                        state = 1;
        //                    }
        //                    else // Nested object - table part
        //                    {
        //                        currentTable = dbo;
        //                        currentObject.NestedObjects.Add(currentTable);
        //                        state = 3;
        //                    }
        //                }
        //            }
        //            else if (state == 1) // reading DBObject's properties
        //            {
        //                propertiesCount = int.Parse(line.Replace("{", string.Empty).Replace(",", string.Empty));
        //                state = 2;
        //            }
        //            else if (state == 2) // waiting for the next DBProperty
        //            {
        //                if (propertiesCount == 0)
        //                {
        //                    state = 0; // waiting for the next DBObject
        //                }
        //                else
        //                {
        //                    match = DBPropertyRegex.Match(line);
        //                    if (match.Success)
        //                    {
        //                        currentProperty = ParseDBProperty(line, dbnames);
        //                        currentObject.Properties.Add(currentProperty);
        //                        propertiesCount--;
        //                        state = 5;
        //                    }
        //                }
        //            }
        //            else if (state == 3) // reading DBObject's properties
        //            {
        //                propertiesCount = int.Parse(line.Replace("{", string.Empty).Replace(",", string.Empty));
        //                state = 4;
        //            }
        //            else if (state == 4) // waiting for the next DBProperty
        //            {
        //                if (propertiesCount == 0)
        //                {
        //                    state = 0; // waiting for the next DBObject
        //                    currentTable = null;
        //                }
        //                else
        //                {
        //                    match = DBPropertyRegex.Match(line);
        //                    if (match.Success)
        //                    {
        //                        currentProperty = ParseDBProperty(line, dbnames);
        //                        currentTable.Properties.Add(currentProperty);
        //                        propertiesCount--;
        //                        state = 5;
        //                    }
        //                }
        //            }
        //            else if (state == 5) // reading DBProperty's types
        //            {
        //                typesCount = int.Parse(line.Replace("{", string.Empty).Replace(",", string.Empty));
        //                state = 6;
        //            }
        //            else if (state == 6) // waiting for the next DBType
        //            {
        //                match = DBTypeRegex.Match(line);
        //                if (match.Success)
        //                {
        //                    DBType propertyType = ParseDBType(line);
        //                    currentProperty.Types.Add(propertyType);
        //                    typesCount--;
        //                    if (typesCount == 0)
        //                    {
        //                        if (currentTable == null)
        //                        {
        //                            state = 2;
        //                        }
        //                        else
        //                        {
        //                            state = 4;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}
        //private DBObject ParseDBObject(string line, DBName[] dbnames)
        //{
        //    string[] items = line.Split(',');
        //    DBObject dbo = new DBObject()
        //    {
        //        Name = items[0].Replace("{", string.Empty).Replace("\"", string.Empty),
        //        ID = int.Parse(items[2]),
        //        Owner = items[3].Replace("\"", string.Empty),
        //    };
        //    if (dbo.Owner == string.Empty) 
        //    {
        //        dbo.Token = dbo.Name.Replace(dbo.ID.ToString(), string.Empty);
        //        DBName dbname = dbnames[dbo.ID];
        //        if (dbo.Token == dbname.Token)
        //        {
        //            dbo.FileName = dbname.FileName;
        //        }
        //    }
        //    else // nested object table part
        //    {
        //        Match match = Regex.Match(dbo.Name, "\\d+");
        //        if (match.Success)
        //        {
        //            dbo.ID = int.Parse(match.Value);
        //            dbo.Token = dbo.Name.Replace(dbo.ID.ToString(), string.Empty);
        //            DBName dbname = dbnames[dbo.ID];
        //            if (dbo.Token == dbname.Token)
        //            {
        //                dbo.FileName = dbname.FileName;
        //            }
        //        }
        //        else // system property
        //        {
        //            dbo.ID = 0;
        //            dbo.FileName = string.Empty;
        //        }
        //    }
        //    return dbo;
        //}
        //private DBProperty ParseDBProperty(string line, DBName[] dbnames)
        //{
        //    string[] items = line.Split(',');
        //    DBProperty dbp = new DBProperty()
        //    {
        //        Name = items[0].Replace("{", string.Empty).Replace("\"", string.Empty)
        //    };
        //    Match match = Regex.Match(dbp.Name, "\\d+");
        //    if (match.Success && !dbp.Name.Contains("DimUse"))
        //    {
        //        dbp.ID = int.Parse(match.Value);
        //        dbp.Token = dbp.Name.Replace(dbp.ID.ToString(), string.Empty);
        //        DBName dbname = dbnames[dbp.ID];
        //        if (dbp.Token == dbname.Token)
        //        {
        //            dbp.FileName = dbname.FileName;
        //        }
        //    }
        //    else // system property
        //    {
        //        dbp.ID = 0;
        //        dbp.FileName = string.Empty;
        //    }
        //    return dbp;
        //}
        //private DBType ParseDBType(string line)
        //{
        //    string[] items = line.Split(',');
        //    DBType dbt = new DBType()
        //    {
        //        Token = items[0].Replace("{", string.Empty).Replace("\"", string.Empty),
        //        Size = (items[1].Length == 10) ? int.MaxValue : int.Parse(items[1]),
        //        Tail = int.Parse(items[2]),
        //        Name = items[3].Replace("\"", string.Empty),
        //        Kind = int.Parse(items[4].Replace("}",string.Empty))
        //    };
        //    return dbt;
        //}
        #endregion

        #region " Read Config "
        private readonly Regex rxUUID = new Regex("[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}"); // Example: eb3dfdc7-58b8-4b1f-b079-368c262364c9
        private readonly Regex rxSpecialUUID = new Regex("^{[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12},\\d+(?:})?,$"); // Example: {3daea016-69b7-4ed4-9453-127911372fe6,0}, | {cf4abea7-37b2-11d4-940f-008048da11f9,5,
        private readonly Regex rxOCSName = new Regex("^{\\d,\\d,[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}},\"\\w+\",$");
        private readonly Regex rxOCSType = new Regex("^{\"[#BSDN]\""); // Example: {"#",1aaea747-a4ba-4fb2-9473-075b1ced620c}, | {"B"}, | {"S",10,0}, | {"D","T"}, | {"N",10,0,1}
        private readonly Regex rxNestedProperties = new Regex("^{888744e1-b616-11d4-9436-004095e12fc7,\\d+[},]$"); // look rxSpecialUUID
        private readonly Dictionary<string, SpecialParser> _SpecialParsers = new Dictionary<string, SpecialParser>();
        public void ReadConfig(MetadataProvider server, string fileName, Domain infoBase)
        {
            if (new Guid(fileName) == Guid.Empty) return;

            SqlBytes binaryData = ReadConfigFromDatabase(fileName);
            if (binaryData == null) return;

            DeflateStream stream = new DeflateStream(binaryData.Stream, CompressionMode.Decompress);
            if (_logger == null)
            {
                ParseMetadataObject(stream, server, fileName, infoBase);
            }
            else
            {
                MemoryStream memory = new MemoryStream();
                stream.CopyTo(memory);
                memory.Seek(0, SeekOrigin.Begin);
                WriteBinaryDataToFile(memory, $"{fileName}.txt");
                memory.Seek(0, SeekOrigin.Begin);
                ParseMetadataObject(memory, server, fileName, infoBase);
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
        private void ParseMetadataObject(Stream stream, MetadataProvider server, string fileName, Domain infoBase)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    line = reader.ReadLine();
                }

                Entity dbo;
                if (server.DBNames.TryGetValue(fileName, out DBNameEntry entry))
                {
                    dbo = (Entity)entry.OCSObject;
                    DBName dbname = entry.DBNames.Where(i => i.Token == DBToken.Enum).FirstOrDefault();
                    if (dbname != null)
                    {
                        dbo.Token = DBToken.Enum;
                    }
                }
                else
                {
                    dbo = new Entity();
                }
                try
                {
                    ParseInternalIdentifier(line, dbo, server);
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
                    ParseOCSObjectNames(line, fileName, dbo, server);
                    SetOCSObjecNamespace(dbo, infoBase);
                }
                if (dbo.Token == DBToken.Reference)
                {
                    ParseReferenceOwner(reader, dbo, server);
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
                        _SpecialParsers[UUID](reader, line, dbo, server);
                    }
                }
            }
        }
        private void ParseInternalIdentifier(string line, Entity dbo, MetadataProvider server)
        {
            string[] lines = line.Split(',');
            string UUID = (dbo.Token == DBToken.Enum ? lines[1] : lines[3]);
            server.UUIDs.Add(UUID, dbo);
        }
        private void ParseOCSObjectNames(string line, string fileName, Entity dbo, MetadataProvider server)
        {
            string[] lines = line.Split(',');
            string FileName = lines[2].Replace("}", string.Empty);
            if (fileName != FileName)
            {
                // TODO: error ?
            }
            dbo.Name = lines[3].Replace("\"", string.Empty);
            if (server.DBNames.TryGetValue(fileName, out DBNameEntry entry))
            {
                DBName dbname = entry.DBNames.Where(i => i.IsMainTable).FirstOrDefault();
                if (dbname != null)
                {
                    dbo.Token = dbname.Token;
                    dbo.TypeCode = dbname.TypeCode;
                    dbo.TableName = CreateTableName(dbo, dbname);
                }
            }
        }
        private string CreateTableName(Entity dbo, DBName dbname)
        {
            if (dbname.Token == DBToken.VT)
            {
                if (dbo.Owner == null)
                {
                    return string.Empty;
                    // TODO: error ?
                }
                else
                {
                    return $"{dbo.Owner.TableName}_{dbname.Token}{dbname.TypeCode}";
                }
            }
            else
            {
                return $"_{dbname.Token}{dbname.TypeCode}";
            }
        }
        private void SetOCSObjecNamespace(Entity dbo, Domain domain)
        {
            if (dbo.Parent != null) return;

            Namespace ns = (Namespace)domain.Namespaces.Where(n => n.Name == dbo.Token).FirstOrDefault();
            if (ns == null)
            {
                if (string.IsNullOrEmpty(dbo.Token))
                {
                    ns = (Namespace)domain.Namespaces.Where(n => n.Name == "Unknown").FirstOrDefault();
                    if (ns == null)
                    {
                        ns = new Namespace() { Domain = domain };
                        ns.Name = "Unknown";
                        domain.Namespaces.Add(ns);
                    }
                }
                else
                {
                    ns = new Namespace() { Domain = domain };
                    ns.Name = dbo.Token;
                    domain.Namespaces.Add(ns);
                }
            }
            dbo.Parent = ns;
            ns.Entities.Add(dbo);
        }
        private void ParseReferenceOwner(StreamReader reader, Entity dbo, MetadataProvider server)
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
            List<ITypeInfo> types = new List<ITypeInfo>();
            for (int i = 0; i < count; i++)
            {
                _ = reader.ReadLine();
                line = reader.ReadLine();
                if (line == null) return;

                match = rxUUID.Match(line);
                if (match.Success)
                {
                    if (server.DBNames.TryGetValue(match.Value, out DBNameEntry entry))
                    {
                        types.Add(new Model.TypeInfo()
                        {
                            TypeCode = entry.OCSObject.TypeCode,
                            Name = entry.OCSObject.TableName,
                            Entity = entry.OCSObject
                        });
                    }
                }
                _ = reader.ReadLine();
            }

            if (types.Count > 0)
            {
                Property property = new Property
                {
                    Parent = dbo,
                    Name = "Владелец",
                    DbName = "OwnerID" // [_OwnerIDRRef] | [_OwnerID_TYPE] + [_OwnerID_RTRef] + [_OwnerID_RRRef]
                    // TODO: add OCSField at once ?
                };
                property.Types.AddRange(types);
                dbo.Properties.Add(property);
            }
        }
        private void ParseOCSProperties(StreamReader reader, string line, Entity dbo, MetadataProvider server)
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
                        ParseOCSProperty(reader, nextLine, dbo, server);
                        break;
                    }
                }
            }
        }
        private void ParseOCSProperty(StreamReader reader, string line, Entity dbo, MetadataProvider server)
        {
            string[] lines = line.Split(',');
            string fileName = lines[2].Replace("}", string.Empty);
            string objectName = lines[3].Replace("\"", string.Empty);

            Property property = new Property
            {
                Parent = dbo,
                Name = objectName
            };
            dbo.Properties.Add(property);

            if (server.DBNames.TryGetValue(fileName, out DBNameEntry entry))
            {
                if (entry.DBNames.Count == 1)
                {
                    property.DbName = CreateOCSFieldName(entry.DBNames[0]);
                }
                else if (entry.DBNames.Count > 1)
                {
                    foreach (var dbn in entry.DBNames.Where(dbn => dbn.Token == DBToken.Fld))
                    {
                        property.DbName = CreateOCSFieldName(dbn);
                    }
                }
            }
            ParseOCSPropertyTypes(reader, property, server);
        }
        private string CreateOCSFieldName(DBName dbname)
        {
            return $"{dbname.Token}{dbname.TypeCode}";
        }
        private void ParseOCSPropertyTypes(StreamReader reader, Property property, MetadataProvider server)
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
                    property.Types.Add(new Model.TypeInfo()
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
                        property.Types.Add(new Model.TypeInfo()
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
                    else if (server.UUIDs.TryGetValue(UUID, out Entity dbo))
                    {
                        property.Types.Add(new Model.TypeInfo()
                        {
                            Name = dbo.TableName,
                            TypeCode = dbo.TypeCode,
                            UUID = UUID,
                            Entity = dbo
                        });
                    }
                    else // UUID is not loaded yet - leave it for second pass
                    {
                        property.Types.Add(new Model.TypeInfo()
                        {
                            UUID = UUID
                        });
                    }
                }
            }
        }
        private void ParseNestedObjects(StreamReader reader, string line, Entity dbo, MetadataProvider server)
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
                        ParseNestedObject(reader, nextLine, dbo, server);
                        break;
                    }
                }
            }
        }
        private void ParseNestedObject(StreamReader reader, string line, Entity dbo, MetadataProvider server)
        {
            string[] lines = line.Split(',');
            string fileName = lines[2].Replace("}", string.Empty);
            string objectName = lines[3].Replace("\"", string.Empty);

            Entity nested = new Entity()
            {
                Owner = dbo,
                Name = objectName,
                Parent = dbo.Parent
            };
            dbo.NestedEntities.Add(nested);

            if (server.DBNames.TryGetValue(fileName, out DBNameEntry entry))
            {
                DBName dbname = entry.DBNames.Where(i => i.IsMainTable).FirstOrDefault();
                if (dbname != null)
                {
                    nested.Token = dbname.Token;
                    nested.TypeCode = dbname.TypeCode;
                    nested.TableName = CreateTableName(nested, dbname);
                }
            }
            ParseNestedOCSProperties(reader, nested, server);
        }
        private void ParseNestedOCSProperties(StreamReader reader, Entity dbo, MetadataProvider server)
        {
            string line;
            Match match;
            while ((line = reader.ReadLine()) != null)
            {
                match = rxNestedProperties.Match(line);
                if (match.Success)
                {
                    ParseOCSProperties(reader, line, dbo, server);
                    break;
                }
            }
        }
        #endregion
        public void MakeSecondPass(Domain infoBase, MetadataProvider server)
        {
            foreach (var ns in infoBase.Namespaces)
            {
                foreach (var dbo in ns.Entities)
                {
                    foreach (var property in dbo.Properties)
                    {
                        foreach (var type in property.Types)
                        {
                            if (type.TypeCode == 0)
                            {
                                if (type.Entity == null)
                                {
                                    if (!string.IsNullOrEmpty(type.UUID))
                                    {
                                        if (server.UUIDs.TryGetValue(type.UUID, out Entity dbObject))
                                        {
                                            type.Name = dbObject.TableName;
                                            type.Entity = dbObject;
                                            type.TypeCode = dbObject.TypeCode;
                                        }
                                    }
                                }
                                else
                                {
                                    type.Name = type.Entity.TableName;
                                    type.TypeCode = type.Entity.TypeCode;
                                }
                            }
                        }
                    }
                    foreach (var nested in dbo.NestedEntities)
                    {
                        foreach (var property in nested.Properties)
                        {
                            foreach (var type in property.Types)
                            {
                                if (type.TypeCode == 0)
                                {
                                    if (type.Entity == null)
                                    {
                                        if (!string.IsNullOrEmpty(type.UUID))
                                        {
                                            if (server.UUIDs.TryGetValue(type.UUID, out Entity dbObject))
                                            {
                                                type.Name = dbObject.TableName;
                                                type.Entity = dbObject;
                                                type.TypeCode = dbObject.TypeCode;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        type.Name = type.Entity.TableName;
                                        type.TypeCode = type.Entity.TypeCode;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void ReadSQLMetadata(Domain infoBase)
        {
            SQLHelper SQL = new SQLHelper();
            SQL.ConnectionString = ConnectionString;
            SQL.Load(infoBase);
        }
    

        internal void SaveOCSObjectToFile(Entity dbo, MetadataProvider server)
        {
            if (_logger == null) return;

            var kv = server.DBNames
                .Where(i => i.Value.OCSObject == dbo)
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
