using OneCSharp.Metadata.Shared;
using OneCSharp.Persistence.Shared;
using System;
using System.Data;
using System.Data.SqlClient;

namespace OneCSharp.Metadata.Server
{
    public sealed class NamespaceDataPersister : MetadataObject.Persister, IDataPersister
    {
        #region "SQL commands"
        private const string SelectCommandText =
            "SELECT ns.[owner_], ns.[owner], " +
            "CASE WHEN NOT i.[key] IS NULL THEN i.[name] " +
            "WHEN NOT n.[key] IS NULL THEN n.[name] " +
            "END AS[_owner], " +
            "ns.[name], ns.[alias], ns.[version] " +
            "FROM (SELECT * FROM [ocs].[namespaces] WHERE [key] = @key) AS ns " +
            "LEFT JOIN [ocs].[infobases] AS i ON ns.[owner] = i.[key] " +
            "LEFT JOIN [ocs].[namespaces] AS n ON ns.[owner] = n.[key];";
        private const string InsertCommandText =
            @"DECLARE @result table([version] binary(8)); " +
            @"INSERT [ocs].[namespaces] ([key], [owner_], [owner], [name], [alias]) " +
            @"OUTPUT inserted.[version] INTO @result " +
            @"VALUES (@key, @owner_, @owner, @name, @alias); " +
            @"IF @@ROWCOUNT > 0 SELECT [version] FROM @result;";
        private const string UpdateCommandText =
            @"DECLARE @rows_affected int; DECLARE @result table([version] binary(8)); " +
            @"UPDATE [ocs].[namespaces]" +
            @" SET [owner_] = @owner_, [owner] = @owner, [name] = @name, [alias] = @alias " +
            @"OUTPUT inserted.[version] INTO @result" +
            @" WHERE [key] = @key AND [version] = @version; " +
            @"SET @rows_affected = @@ROWCOUNT; " +
            @"IF (@rows_affected = 0) " +
            @"BEGIN " +
            @"  INSERT @result ([version]) SELECT [version] FROM [ocs].[namespaces] WHERE [key] = @key; " +
            @"END " +
            @"SELECT @rows_affected, [version] FROM @result;";
        private const string DeleteCommandText =
            @"DELETE [ocs].[namespaces] WHERE [key] = @key " +
            @"   AND ([version] = @version OR @version = 0x00000000); " + // taking into account deletion of the entities having virtual state
            @"SELECT @@ROWCOUNT;";
        #endregion

        public NamespaceDataPersister(IPersistentContext context) { this.Context = context; }
        public IPersistentContext Context { get; private set; }
        public void Select(IPersistentObject entity)
        {
            Namespace e = (Namespace)entity;
            
            bool ok = false;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = SelectCommandText;

                SqlParameter parameter = null;

                parameter = new SqlParameter("key", SqlDbType.UniqueIdentifier);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = e.PrimaryKey;
                command.Parameters.Add(parameter);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int code = reader.GetInt32(0);
                    Guid key = reader.GetGuid(1);
                    string presentation = reader.GetString(2);
                    //var factory = this.Context.GetObjectFactory(code);
                    //var owner = (MetadataObject)factory.New(code, key);
                    //this.Context.Load(owner);
                    //presentation = owner.ToString();
                    e.Owner = new ObjectReference(code, key, presentation);
                    e.Name = (string)reader[3];
                    e.Alias = (string)reader[4];
                    this.SetVersion(e, (byte[])reader[5]);
                    this.SetState(e, PersistentState.Original);
                    ok = true;
                }

                reader.Close(); connection.Close();
            }

            if (!ok) throw new ApplicationException("Error executing select command.");
        }
        public void Insert(IPersistentObject entity)
        {
            Namespace e = (Namespace)entity;

            bool ok = false;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = InsertCommandText;

                SqlParameter parameter = null;

                parameter = new SqlParameter("key", SqlDbType.UniqueIdentifier);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = e.PrimaryKey;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("owner_", SqlDbType.Int);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = (e.Owner == null) ? 0 : e.Owner.TypeCode;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("owner", SqlDbType.UniqueIdentifier);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = (e.Owner == null) ? Guid.Empty : e.Owner.PrimaryKey;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("name", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = (e.Name == null) ? string.Empty : e.Name;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("alias", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = (e.Alias == null) ? string.Empty : e.Alias;
                command.Parameters.Add(parameter);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    this.SetVersion(e, (byte[])reader[0]);
                    this.SetState(e, PersistentState.Original);
                    ok = true;
                }

                reader.Close(); connection.Close();
            }

            if (!ok) throw new ApplicationException("Error executing insert command.");
        }
        public void Update(IPersistentObject entity)
        {
            Namespace e = (Namespace)entity;

            bool ok = false; int rows_affected = 0;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = UpdateCommandText;

                SqlParameter parameter = null;

                parameter = new SqlParameter("key", SqlDbType.UniqueIdentifier);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = e.PrimaryKey;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("version", SqlDbType.Timestamp);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = this.GetVersion(e);
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("owner_", SqlDbType.Int);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = (e.Owner == null) ? 0 : e.Owner.TypeCode;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("owner", SqlDbType.UniqueIdentifier);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = (e.Owner == null) ? Guid.Empty : e.Owner.PrimaryKey;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("name", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = e.Name ?? string.Empty;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("alias", SqlDbType.NVarChar);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = e.Alias ?? string.Empty;
                command.Parameters.Add(parameter);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rows_affected = reader.GetInt32(0);
                        this.SetVersion(e, (byte[])reader[1]);
                        if (rows_affected == 0)
                        {
                            this.SetState(e, PersistentState.Changed);
                        }
                        else
                        {
                            this.SetState(e, PersistentState.Original);
                            ok = true;
                        }
                    }
                    else
                    {
                        this.SetState(e, PersistentState.Deleted);
                    }
                }
            }

            if (!ok) throw new OptimisticConcurrencyException(e.State.ToString());
        }
        public void Delete(IPersistentObject entity)
        {
            Namespace e = (Namespace)entity;

            bool ok = false;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = DeleteCommandText;

                SqlParameter parameter = null;

                parameter = new SqlParameter("key", SqlDbType.UniqueIdentifier);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = e.PrimaryKey;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("version", SqlDbType.Timestamp);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = this.GetVersion(e);
                command.Parameters.Add(parameter);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ok = (int)reader[0] > 0;
                    this.SetState(e, PersistentState.Deleted);
                }

                reader.Close(); connection.Close();
            }

            if (!ok) throw new ApplicationException("Error executing delete command.");
        }
    }
}
