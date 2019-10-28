using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OneCSharp.Metadata
{
    internal sealed class DBName
    {
        internal string Token;
        internal string FileName;
    }
    internal sealed class DBObject // Regex("^{\"\\w+\",\"[NI]\",\\d+,\"\\w*\","); // Example: {"Reference42","N",42,"", | {"VT5798","I",0,"Reference228",
    {
        internal int ID;
        internal string Name;
        internal string Token;
        internal string FileName;
        internal string Owner; // for table parts (I token)
        internal List<DBProperty> Properties = new List<DBProperty>();
        internal List<DBObject> NestedObjects = new List<DBObject>(); // Nested tables
    }
    internal sealed class DBProperty // Regex("^{\"\\w+\",0,"); // Example: {"Period",0,
    {
        internal int ID;
        internal string Name;
        internal string Token;
        internal string FileName;
        internal List<DBType> Types = new List<DBType>();
    }
    internal sealed class DBType // Regex("^{\"[ENLVBSTR]\",\\d+,\\d+,\"\\w*\",\\d}"); // Example: {"N",5,0,"",0} | {"R",0,0,"Reference188",3}
    {
        internal string Token; // [BENLVSTR]
        internal int Size;
        internal int Tail;
        internal string Name; // reference types only & if not compound (E token)
        internal int Kind; // ??? used usually with references
    }

    internal static class QueryHelper
    {
        public static Stream ReadConfigFile(string connectionString, string fileName)
        {
            SqlBytes binaryData = null;

            { // limited scope for variables declared in it - using statement does like that - used here to get control over catch block
                SqlConnection connection = new SqlConnection(connectionString);
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

            return binaryData?.Stream;
        }


        public static DBName[] ReadDBNames(string connectionString)
        {
            SqlBytes binaryData = GetDBNamesFromDatabase(connectionString);
            if (binaryData == null) return null;
            DeflateStream stream = new DeflateStream(binaryData.Stream, CompressionMode.Decompress);
            return ParseDBNames(stream);
        }
        private static SqlBytes GetDBNamesFromDatabase(string connectionString)
        {
            SqlBytes binaryData = null;

            { // limited scope for variables declared in it - using statement does like that - used here to get control over catch block
                SqlConnection connection = new SqlConnection(connectionString);
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
        private static DBName[] ParseDBNames(Stream stream)
        {
            DBName[] dbnames = null;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    int capacity = GetDBNamesCapacity(line);
                    dbnames = new DBName[capacity + 1];
                    
                    while ((line = reader.ReadLine()) != null)
                    {
                        ParseDBNameLine(line, dbnames);
                    }
                }
            }
            return dbnames;
        }
        private static int GetDBNamesCapacity(string line)
        {
            return int.Parse(line.Replace("{", string.Empty).Replace(",", string.Empty));
        }
        private static void ParseDBNameLine(string line, DBName[] dbnames)
        {
            string[] items = line.Split(',');
            if (items.Length < 3) return;

            DBName dbname = new DBName()
            {
                FileName = items[0].Replace("{", string.Empty),
                Token = items[1].Replace("\"", string.Empty)
            };
            int index = int.Parse(items[2].Replace("}", string.Empty));
            dbnames[index] = dbname;
        }


        public static List<DBObject> ReadDBSchema(string connectionString, DBName[] dbnames)
        {
            SqlBytes binaryData = GetDBSchemaFromDatabase(connectionString);
            if (binaryData == null) return null;
            return ParseDBSchema(binaryData.Stream, dbnames);
        }
        public static SqlBytes GetDBSchemaFromDatabase(string connectionString)
        {
            SqlBytes binaryData = null;

            { // limited scope for variables declared in it - using statement does like that - used here to get control over catch block
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = connection.CreateCommand();
                SqlDataReader reader = null;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT TOP 1 SerializedData FROM DBSchema";
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
        private static List<DBObject> ParseDBSchema(Stream stream, DBName[] dbnames)
        {
            List<DBObject> result = new List<DBObject>();

            Match match = null;
            Regex DBTypeRegex = new Regex("^{\"[ENLVBSTR]\",\\d+,\\d+,\"\\w*\",\\d(?:,\\d)?}"); // Example: {"N",5,0,"",0} | {"N",10,0,"",0,1} | {"R",0,0,"Reference188",3}
            Regex DBObjectRegex = new Regex("^{\"\\w+\",\"[NI]\",\\d+,\"\\w*\","); // Example: {"Reference42","N",42,"", | {"VT5798","I",0,"Reference228",
            Regex DBPropertyRegex = new Regex("^{\"\\w{2,}\",\\d,$"); // Example: {"Period",0, | {"Fld1795",1,

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line = null;
                int state = 0;
                int typesCount = 0;
                int propertiesCount = 0;
                DBObject currentObject = null;
                DBObject currentTable = null;
                DBProperty currentProperty = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (state == 0) // waiting for the next DBObject
                    {
                        match = DBObjectRegex.Match(line);
                        if (match.Success)
                        {
                            DBObject dbo = ParseDBObject(line, dbnames);
                            if (dbo.Owner == string.Empty)
                            {
                                currentObject = dbo;
                                result.Add(currentObject);
                                state = 1;
                            }
                            else // Nested object - table part
                            {
                                currentTable = dbo;
                                currentObject.NestedObjects.Add(currentTable);
                                state = 3;
                            }
                        }
                    }
                    else if (state == 1) // reading DBObject's properties
                    {
                        propertiesCount = int.Parse(line.Replace("{", string.Empty).Replace(",", string.Empty));
                        state = 2;
                    }
                    else if (state == 2) // waiting for the next DBProperty
                    {
                        if (propertiesCount == 0)
                        {
                            state = 0; // waiting for the next DBObject
                        }
                        else
                        {
                            match = DBPropertyRegex.Match(line);
                            if (match.Success)
                            {
                                currentProperty = ParseDBProperty(line, dbnames);
                                currentObject.Properties.Add(currentProperty);
                                propertiesCount--;
                                state = 5;
                            }
                        }
                    }
                    else if (state == 3) // reading DBObject's properties
                    {
                        propertiesCount = int.Parse(line.Replace("{", string.Empty).Replace(",", string.Empty));
                        state = 4;
                    }
                    else if (state == 4) // waiting for the next DBProperty
                    {
                        if (propertiesCount == 0)
                        {
                            state = 0; // waiting for the next DBObject
                            currentTable = null;
                        }
                        else
                        {
                            match = DBPropertyRegex.Match(line);
                            if (match.Success)
                            {
                                currentProperty = ParseDBProperty(line, dbnames);
                                currentTable.Properties.Add(currentProperty);
                                propertiesCount--;
                                state = 5;
                            }
                        }
                    }
                    else if (state == 5) // reading DBProperty's types
                    {
                        typesCount = int.Parse(line.Replace("{", string.Empty).Replace(",", string.Empty));
                        state = 6;
                    }
                    else if (state == 6) // waiting for the next DBType
                    {
                        match = DBTypeRegex.Match(line);
                        if (match.Success)
                        {
                            DBType propertyType = ParseDBType(line);
                            currentProperty.Types.Add(propertyType);
                            typesCount--;
                            if (typesCount == 0)
                            {
                                if (currentTable == null)
                                {
                                    state = 2;
                                }
                                else
                                {
                                    state = 4;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        private static DBObject ParseDBObject(string line, DBName[] dbnames)
        {
            string[] items = line.Split(',');
            DBObject dbo = new DBObject()
            {
                Name = items[0].Replace("{", string.Empty).Replace("\"", string.Empty),
                ID = int.Parse(items[2]),
                Owner = items[3].Replace("\"", string.Empty),
            };
            if (dbo.Owner == string.Empty) 
            {
                dbo.Token = dbo.Name.Replace(dbo.ID.ToString(), string.Empty);
                DBName dbname = dbnames[dbo.ID];
                if (dbo.Token == dbname.Token)
                {
                    dbo.FileName = dbname.FileName;
                }
            }
            else // nested object table part
            {
                Match match = Regex.Match(dbo.Name, "\\d+");
                if (match.Success)
                {
                    dbo.ID = int.Parse(match.Value);
                    dbo.Token = dbo.Name.Replace(dbo.ID.ToString(), string.Empty);
                    DBName dbname = dbnames[dbo.ID];
                    if (dbo.Token == dbname.Token)
                    {
                        dbo.FileName = dbname.FileName;
                    }
                }
                else // system property
                {
                    dbo.ID = 0;
                    dbo.FileName = string.Empty;
                }
            }
            return dbo;
        }
        private static DBProperty ParseDBProperty(string line, DBName[] dbnames)
        {
            string[] items = line.Split(',');
            DBProperty dbp = new DBProperty()
            {
                Name = items[0].Replace("{", string.Empty).Replace("\"", string.Empty)
            };
            Match match = Regex.Match(dbp.Name, "\\d+");
            if (match.Success && !dbp.Name.Contains("DimUse"))
            {
                dbp.ID = int.Parse(match.Value);
                dbp.Token = dbp.Name.Replace(dbp.ID.ToString(), string.Empty);
                DBName dbname = dbnames[dbp.ID];
                if (dbp.Token == dbname.Token)
                {
                    dbp.FileName = dbname.FileName;
                }
            }
            else // system property
            {
                dbp.ID = 0;
                dbp.FileName = string.Empty;
            }
            return dbp;
        }
        private static DBType ParseDBType(string line)
        {
            string[] items = line.Split(',');
            DBType dbt = new DBType()
            {
                Token = items[0].Replace("{", string.Empty).Replace("\"", string.Empty),
                Size = (items[1].Length == 10) ? int.MaxValue : int.Parse(items[1]),
                Tail = int.Parse(items[2]),
                Name = items[3].Replace("\"", string.Empty),
                Kind = int.Parse(items[4].Replace("}",string.Empty))
            };
            return dbt;
        }
    }
}
