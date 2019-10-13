using OneCSharp.Metadata.Shared;
using OneCSharp.Persistence.Shared;
using System;
using System.Data;
using System.Data.SqlClient;

namespace OneCSharp.Metadata.Server
{
    public sealed class InfoBaseDataPersister : IDataPersister
    {
        #region "SQL commands"
        private const string SelectCommandScript = @"SELECT [name], [alias], [server], [database], [username], [password], [version] FROM [ocs].[infobases] WHERE [key] = @key";
        private const string InsertCommandScript =
                @"DECLARE @result table([version] binary(8)); " +
                @"INSERT [ocs].[infobases] ([key], [name], [alias], [server], [database], [username], [password]) " +
                @"OUTPUT inserted.[version] INTO @result " +
                @"VALUES (@key, @name, @alias, @server, @database, @username, @password); " +
                @"IF @@ROWCOUNT > 0 SELECT [version] FROM @result;";
        private const string UpdateCommandScript =
                @"DECLARE @rows_affected int; DECLARE @result table([version] binary(8)); " +
                @"UPDATE [ocs].[infobases] SET [name] = @name, [alias] = @alias, [server] = @server, [database] = @database, [username] = @username, [password] = @password " +
                @"OUTPUT inserted.[version] INTO @result" +
                @" WHERE [key] = @key AND [version] = @version; " +
                @"SET @rows_affected = @@ROWCOUNT; " +
                @"IF (@rows_affected = 0) " +
                @"BEGIN " +
                @"  INSERT @result ([version]) SELECT [version] FROM [ocs].[infobases] WHERE [key] = @key; " +
                @"END " +
                @"SELECT @rows_affected, [version] FROM @result;";
        private const string DeleteCommandScript =
            @"DELETE [ocs].[infobases] WHERE [key] = @key " +
            @"   AND ([version] = @version OR @version = 0x00000000); " + // taking into account deletion of the entities having virtual state
            @"SELECT @@ROWCOUNT;";
        #endregion

        public InfoBaseDataPersister(IPersistentContext context) { this.Context = context; }
        public IPersistentContext Context { get; set; }
        public int Select(ref ReferenceObject persistentObject)
        {
            InfoBase po = (InfoBase)persistentObject;
            bool ok = false;
            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = SelectCommandScript;

                SqlParameter parameter = new SqlParameter("key", SqlDbType.UniqueIdentifier)
                {
                    Direction = ParameterDirection.Input,
                    Value = po.PrimaryKey
                };
                command.Parameters.Add(parameter);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    po.Name = (string)reader[0];
                    po.Alias = (string)reader[1];
                    po.Server = (string)reader[2];
                    po.Database = (string)reader[3];
                    po.UserName = (string)reader[4];
                    po.Password = (string)reader[5];
                    ((IOptimisticConcurrencyObject)po).Version = (byte[])reader[6];
                    ok = true;
                }
                reader.Close();
                connection.Close();
            }
            if (ok) return 1; else return 0;
        }
        public int Insert(ref ReferenceObject persistentObject)
        {
            InfoBase po = (InfoBase)persistentObject;

            bool ok = false;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = InsertCommandScript;

                SqlParameter parameter = null;

                parameter = new SqlParameter("key", SqlDbType.UniqueIdentifier);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.PrimaryKey;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("name", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.Name;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("alias", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.Alias;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("server", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.Server;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("database", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.Database;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("username", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.UserName;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("password", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.Password;
                command.Parameters.Add(parameter);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ((IOptimisticConcurrencyObject)po).Version = (byte[])reader[0];
                    ok = true;
                }

                reader.Close();
                connection.Close();
            }
            if (ok) return 1; else return 0;
        }
        public int Update(ref ReferenceObject persistentObject)
        {
            InfoBase po = (InfoBase)persistentObject;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = UpdateCommandScript;

                SqlParameter parameter = null;

                parameter = new SqlParameter("key", SqlDbType.UniqueIdentifier);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.PrimaryKey;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("version", SqlDbType.Timestamp);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = ((IOptimisticConcurrencyObject)po).Version;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("name", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.Name;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("alias", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.Alias;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("server", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.Server;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("database", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.Database;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("username", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.UserName;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("password", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.Password;
                command.Parameters.Add(parameter);

                int result = 2; // sql exception
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int rows_affected = reader.GetInt32(0);
                        ((IOptimisticConcurrencyObject)po).Version = (byte[])reader[1];
                        if (rows_affected == 0)
                        {
                            result = 0; // changed
                        }
                        else
                        {
                            result = 1; // original
                        }
                    }
                    else
                    {
                        result = -1; // deleted
                    }
                }
                return result;
            }
        }
        public int Delete(ref ReferenceObject persistentObject)
        {
            InfoBase po = (InfoBase)persistentObject;

            bool ok = false;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = DeleteCommandScript;

                SqlParameter parameter = null;

                parameter = new SqlParameter("key", SqlDbType.UniqueIdentifier);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = po.PrimaryKey;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("version", SqlDbType.Timestamp);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = ((IOptimisticConcurrencyObject)po).Version;
                command.Parameters.Add(parameter);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ok = (int)reader[0] > 0;
                }
                reader.Close();
                connection.Close();
            }
            if (ok) { return 1; } else { return 0; }
        }
    }
}
