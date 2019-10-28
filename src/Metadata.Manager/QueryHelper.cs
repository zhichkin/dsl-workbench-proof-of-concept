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
    internal sealed class DBObject
    {
        internal int ID;
        internal string Name;
        internal string Token;
        internal string FileName;
        internal List<DBProperty> Properties = new List<DBProperty>();
    }
    internal sealed class DBProperty
    {
        internal int ID;
        internal string Name;
        internal List<string> Types = new List<string>();
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
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    ParseDBObjectLine(line, dbnames, result);
                }
            }
            return result;
        }
        private static void ParseDBObjectLine(string line, DBName[] dbnames, List<DBObject> result)
        {
            Match match = Regex.Match(line, "^{\"\\w+\",\"N\",\\d+,\"\","); // Example: {"Reference42","N",42,"",
            if (match.Success)
            {
                string[] items = line.Split(',');
                DBObject dbo = new DBObject()
                {
                    Name = items[0].Replace("{", string.Empty).Replace("\"", string.Empty),
                    ID = int.Parse(items[2])
                };
                dbo.Token = dbo.Name.Replace(dbo.ID.ToString(), string.Empty);
                DBName dbname = dbnames[dbo.ID];
                if (dbo.Token == dbname.Token)
                {
                    dbo.FileName = dbname.FileName;
                }
                result.Add(dbo);
            }
        }
    }
}
