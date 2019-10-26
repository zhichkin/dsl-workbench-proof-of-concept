using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;

namespace OneCSharp.Metadata
{
    public sealed class MetadataManager
    {
        private readonly string _logPath;
        public MetadataManager(string logPath)
        {
            _logPath = logPath;
        }
        private void WriteToLog(string entry)
        {
            using (StreamWriter writer = new StreamWriter(_logPath, true))
            {
                writer.WriteLine(entry);
                writer.Close();
            }
        }
        private string GetErrorText(Exception ex)
        {
            string errorText = string.Empty;
            Exception error = ex;
            while (error != null)
            {
                errorText += (errorText == string.Empty) ? error.Message : Environment.NewLine + error.Message;
                error = error.InnerException;
            }
            return errorText;
        }
        public void ImportMetadataToFiles(string connectionString, string calalogPath)
        {
            string RootPath = Path.Combine(calalogPath, "root.txt");
            string ConfigPath = Path.Combine(calalogPath, "config.txt");
            string DBNamesPath = Path.Combine(calalogPath, "dbnames.txt");
            string DBSchemaPath = Path.Combine(calalogPath, "dbschema.txt");
            string IBVersionPath = Path.Combine(calalogPath, "ibversion.txt");
            
            ReadDBNames(connectionString, DBNamesPath);
            ReadDBSchema(connectionString, DBSchemaPath);
            ReadIBVersion(connectionString, IBVersionPath);
            ReadConfig(connectionString, "root", RootPath); // + version + versions
            ReadConfig(connectionString, "e0666db2-45d6-49b4-a200-061c6ba7d569", ConfigPath);

            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.0", Path.Combine(calalogPath, "root-0.txt"));
            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.2", Path.Combine(calalogPath, "root-2.txt"));
            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.4", Path.Combine(calalogPath, "root-4.txt"));
            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.5", Path.Combine(calalogPath, "root-5.txt"));
            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.6", Path.Combine(calalogPath, "root-6.txt"));
            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.7", Path.Combine(calalogPath, "root-7.txt"));
            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.8", Path.Combine(calalogPath, "root-8.txt"));
            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.9", Path.Combine(calalogPath, "root-9.txt"));
            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.a", Path.Combine(calalogPath, "root-a.txt"));
            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.b", Path.Combine(calalogPath, "root-b.txt"));
            ReadConfig(connectionString, "f0ba0954-a66b-4085-9df1-b8a4283bdbd3.c", Path.Combine(calalogPath, "root-c.txt"));
        }
        public void ReadIBVersion(string connectionString, string filePath)
        {
            int infoBaseVersion = 0;
            int platformVersion = 0;

            { // limited scope for variables declared in it - using statement does like that - used here to get control over catch block
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = connection.CreateCommand();
                SqlDataReader reader = null;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT TOP 1 [IBVersion], [PlatformVersionReq] FROM [IBVersion]";
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        infoBaseVersion = reader.GetInt32(0);
                        platformVersion = reader.GetInt32(1);
                    }
                }
                catch (Exception error)
                {
                    WriteToLog(GetErrorText(error));
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

            if (platformVersion == 0)
            {
                WriteToLog("IBVersion info is not found or empty.");
            }
            else
            {
                WritePaltformVersion(infoBaseVersion, platformVersion, filePath);
            }
        }
        public void ReadDBSchema(string connectionString, string filePath)
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
                catch(Exception error)
                {
                    WriteToLog(GetErrorText(error));
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

            if (binaryData == null)
            {
                WriteToLog("DBSchema info is not found or empty.");
            }
            else
            {
                WriteBinaryDataToFile(binaryData, filePath);
            }
        }
        public void ReadDBNames(string connectionString, string filePath)
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
                    WriteToLog(GetErrorText(error));
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

            if (binaryData == null)
            {
                WriteToLog("DBNames info is not found or empty.");
            }
            else
            {
                DecompressBinaryData(binaryData, filePath);
            }
        }
        private void DecompressBinaryData(SqlBytes binaryData, string filePath)
        {
            DeflateStream stream = new DeflateStream(binaryData.Stream, CompressionMode.Decompress);
            using (FileStream output = File.Create(filePath))
            {
                stream.CopyTo(output);
            }
            WriteToLog($"Metadata is saved to {filePath}");
        }
        private void WriteBinaryDataToFile(SqlBytes binaryData, string filePath)
        {
            using (FileStream output = File.Create(filePath))
            {
                binaryData.Stream.CopyTo(output);
            }
            WriteToLog($"Metadata is saved to {filePath}");
        }
        private void WritePaltformVersion(int infoBaseVersion, int platformVersion, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(infoBaseVersion);
                writer.Write(platformVersion);
                writer.Close();
            }
            WriteToLog($"Metadata is saved to {filePath}");
        }

        public void ReadConfig(string connectionString, string fileName, string filePath)
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
                    WriteToLog(GetErrorText(error));
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

            if (binaryData == null)
            {
                WriteToLog("DBNames info is not found or empty.");
            }
            else
            {
                DecompressBinaryData(binaryData, filePath);
            }
        }
    
    

    }
}
