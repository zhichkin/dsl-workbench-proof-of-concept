using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

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
            
            ReadConfig(connectionString, "db4a9ccb-9ef5-4b3c-8577-b6fe5db1b62e", Path.Combine(calalogPath, "db4a9ccb-9ef5-4b3c-8577-b6fe5db1b62e.txt"));
            ReadConfig(connectionString, "30ee720f-b02b-4bab-8c66-bd27d8c2e8cb", Path.Combine(calalogPath, "30ee720f-b02b-4bab-8c66-bd27d8c2e8cb.txt"));
            ReadConfig(connectionString, "cb3a5c5b-7bdc-4e12-96f1-11b1213b6853", Path.Combine(calalogPath, "cb3a5c5b-7bdc-4e12-96f1-11b1213b6853.txt"));
            ReadConfig(connectionString, "9ad3b432-5b49-44ee-9d8d-83c36458d927", Path.Combine(calalogPath, "9ad3b432-5b49-44ee-9d8d-83c36458d927.txt"));
            ReadConfig(connectionString, "c1568f1c-25ab-4328-8e77-e0e84788f10f", Path.Combine(calalogPath, "c1568f1c-25ab-4328-8e77-e0e84788f10f.txt"));

            ReadCurrentSchema(connectionString, Path.Combine(calalogPath, "SchemaStorage.txt"));
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


        public void ReadCurrentSchema(string connectionString, string filePath)
        {
            SqlBytes[] binaryData = new SqlBytes[3];

            { // limited scope for variables declared in it - using statement does like that - used here to get control over catch block
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = connection.CreateCommand();
                SqlDataReader reader = null;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT CurrentSchema, NewGenCreated, NewGenDropped FROM SchemaStorage WHERE SchemaID = 0";
                
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        binaryData[0] = reader.GetSqlBytes(0);
                        binaryData[1] = reader.GetSqlBytes(1);
                        binaryData[2] = reader.GetSqlBytes(2);
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
                WriteToLog("SchemaStorage info is not found or empty.");
            }
            else
            {
                WriteBinaryDataToFile(binaryData[0], filePath);
                WriteBinaryDataToFile(binaryData[1], filePath.Replace("SchemaStorage", "NewGenCreated"));
                WriteBinaryDataToFile(binaryData[2], filePath.Replace("SchemaStorage", "NewGenDropped"));
            }
        }
        public void ReadIBParamsInf(string connectionString, string filePath)
        {
            SqlBytes binaryData = null;

            { // limited scope for variables declared in it - using statement does like that - used here to get control over catch block
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = connection.CreateCommand();
                SqlDataReader reader = null;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT BinaryData FROM Params WHERE FileName = N'ibparams.inf' ORDER BY PartNo";

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
                WriteToLog("SchemaStorage info is not found or empty.");
            }
            else
            {
                WriteBinaryDataToFile(binaryData, filePath);
            }
        }

        public void ImportMetadata(string connectionString, string catalogPath)
        {
            DBName[] DBNames = QueryHelper.ReadDBNames(connectionString);
            List<DBObject> DBObjects = QueryHelper.ReadDBSchema(connectionString, DBNames);
        }
    }
}
