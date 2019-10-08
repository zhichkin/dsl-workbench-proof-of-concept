﻿using OneCSharp.Metadata.Shared;
using OneCSharp.Persistence.Shared;
using System;
using System.Data;
using System.Data.SqlClient;

namespace OneCSharp.Metadata.Server
{
    public sealed class EntityDataPersister : MetadataObject.Persister, IDataPersister
    {
        public EntityDataPersister(IPersistentContext context) { this.Context = context; }
        public IPersistentContext Context { get; private set; }
        
        #region " SQL "
        private const string SelectCommandText =
            "SELECT e.[version], e.[name], e.[alias], e.[code], " +
            "e.[table], ISNULL(t.[name], N'') AS [_table], " +
            "e.[namespace], ISNULL(n.[name], N'') AS [_namespace], " +
            "e.[owner], ISNULL(o.[name], N'') AS [_owner], " +
            "e.[parent], ISNULL(p.[name], N'') AS [_parent], " +
            "e.[is_sealed], e.[is_abstract] " +
            "FROM (SELECT * FROM [ocs].[entities] WHERE [key] = @key) AS e " +
            "LEFT JOIN [ocs].[tables] AS t ON t.[key] = e.[table] " +
            "LEFT JOIN [ocs].[namespaces] AS n ON n.[key] = e.[namespace] " +
            "LEFT JOIN [ocs].[entities] AS o ON o.[key] = e.[owner] " +
            "LEFT JOIN [ocs].[entities] AS p ON p.[key] = e.[parent];";
        private const string InsertCommandText =
            @"DECLARE @result table([version] binary(8)); " +
            @"INSERT [ocs].[entities] ([key], [namespace], [table], [owner], [parent], [name], [code], [alias], [is_abstract], [is_sealed]) " +
            @"OUTPUT inserted.[version] INTO @result " +
            @"VALUES (@key, @namespace, @table, @owner, @parent, @name, @code, @alias, @is_abstract, @is_sealed); " +
            @"IF @@ROWCOUNT > 0 SELECT [version] FROM @result;";
        private const string UpdateCommandText =
            @"DECLARE @rows_affected int; DECLARE @result table([version] binary(8)); " +
            @"UPDATE [ocs].[entities] SET [namespace] = @namespace, [table] = @table, [owner] = @owner, [parent] = @parent, " +
            @"[name] = @name, [code] = @code, [alias] = @alias, [is_abstract] = @is_abstract, [is_sealed] = @is_sealed " +
            @"OUTPUT inserted.[version] INTO @result" +
            @" WHERE [key] = @key AND [version] = @version; " +
            @"SET @rows_affected = @@ROWCOUNT; " +
            @"IF (@rows_affected = 0) " +
            @"BEGIN " +
            @"  INSERT @result ([version]) SELECT [version] FROM [ocs].[entities] WHERE [key] = @key; " +
            @"END " +
            @"SELECT @rows_affected, [version] FROM @result;";
        private const string DeleteCommandText =
            @"DELETE [ocs].[entities] WHERE [key] = @key " +
            @"   AND ([version] = @version OR @version = 0x00000000); " + // taking into account deletion of the entities having virtual state
            @"SELECT @@ROWCOUNT;";
        #endregion

        public void Select(IPersistentObject persistentObject)
        {
            Entity entity = (Entity)persistentObject;

            bool ok = false;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command  = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = SelectCommandText;

                command.Parameters.Add(new SqlParameter("key", SqlDbType.UniqueIdentifier)
                {
                    Value = entity.PrimaryKey,
                    Direction = ParameterDirection.Input
                });

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    this.SetVersion(entity, (byte[])reader[0]);
                    entity.Name       = reader.GetString(1);
                    entity.Alias      = reader.GetString(2);
                    entity.Code       = reader.GetInt32(3);
                    entity.Table      = new ObjectReference(5, reader.GetGuid(4),  reader.GetString(5));
                    entity.Namespace  = new ObjectReference(2, reader.GetGuid(6),  reader.GetString(7));
                    entity.Owner      = new ObjectReference(3, reader.GetGuid(8),  reader.GetString(9));
                    entity.Parent     = new ObjectReference(3, reader.GetGuid(10), reader.GetString(11));
                    entity.IsSealed   = reader.GetBoolean(12);
                    entity.IsAbstract = reader.GetBoolean(13);
                    this.SetState(entity, PersistentState.Original);
                    ok = true;
                }
                reader.Close();
                connection.Close();
            }
            if (!ok) throw new ApplicationException("Error executing select command.");
        }
        public void Insert(IPersistentObject persistentObject)
        {
            Entity entity = (Entity)persistentObject;

            bool ok = false;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command  = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = InsertCommandText;

                this.InitializeParameters(command, entity);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    this.SetVersion(entity, (byte[])reader[0]);
                    this.SetState(entity, PersistentState.Original);
                    ok = true;
                }
                reader.Close();
                connection.Close();
            }
            if (!ok) throw new ApplicationException("Error executing insert command.");
        }
        public void Update(IPersistentObject persistentObject)
        {
            Entity entity = (Entity)persistentObject;

            bool ok = false; int rows_affected = 0;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = UpdateCommandText;

                this.InitializeParameters(command, entity);

                SqlParameter parameter = new SqlParameter("version", SqlDbType.Timestamp);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = this.GetVersion(entity);
                command.Parameters.Add(parameter);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rows_affected = reader.GetInt32(0);
                        this.SetVersion(entity, (byte[])reader[1]);
                        if (rows_affected == 0)
                        {
                            this.SetState(entity, PersistentState.Changed);
                        }
                        else
                        {
                            this.SetState(entity, PersistentState.Original);
                            ok = true;
                        }
                    }
                    else
                    {
                        this.SetState(entity, PersistentState.Deleted);
                    }
                }
            }
            if (!ok) throw new OptimisticConcurrencyException(entity.State.ToString());
        }
        private void InitializeParameters(SqlCommand command, Entity entity)
        {
            command.Parameters.Add(new SqlParameter("key", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Input,
                Value = entity.PrimaryKey
            });
            command.Parameters.Add(new SqlParameter("namespace", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Input,
                Value = (entity.Namespace == null ? Guid.Empty : entity.Namespace.PrimaryKey)
            });
            command.Parameters.Add(new SqlParameter("table", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Input,
                Value = (entity.Table == null ? Guid.Empty : entity.Table.PrimaryKey)
            });
            command.Parameters.Add(new SqlParameter("owner", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Input,
                Value = (entity.Owner == null ? Guid.Empty : entity.Owner.PrimaryKey)
            });
            command.Parameters.Add(new SqlParameter("parent", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Input,
                Value = (entity.Parent == null ? Guid.Empty : entity.Parent.PrimaryKey)
            });
            command.Parameters.Add(new SqlParameter("name", SqlDbType.NVarChar)
            {
                Direction = ParameterDirection.Input,
                Value = (entity.Name ?? string.Empty)
            });
            command.Parameters.Add(new SqlParameter("alias", SqlDbType.NVarChar)
            {
                Direction = ParameterDirection.Input,
                Value = (entity.Alias ?? string.Empty)
            });
            command.Parameters.Add(new SqlParameter("code", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = entity.Code
            });
            command.Parameters.Add(new SqlParameter("is_sealed", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = entity.IsSealed
            });
            command.Parameters.Add(new SqlParameter("is_abstract", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = entity.IsAbstract
            });
        }
        public void Delete(IPersistentObject persistentObject)
        {
            Entity entity = (Entity)persistentObject;

            bool ok = false;

            using (SqlConnection connection = new SqlConnection(this.Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command  = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = DeleteCommandText;

                SqlParameter parameter = null;

                parameter = new SqlParameter("key", SqlDbType.UniqueIdentifier);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = entity.PrimaryKey;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("version", SqlDbType.Timestamp);
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = this.GetVersion(entity);
                command.Parameters.Add(parameter);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ok = (int)reader[0] > 0;
                    this.SetState(entity, PersistentState.Deleted);
                }
                reader.Close();
                connection.Close();
            }
            if (!ok) throw new ApplicationException("Error executing delete command.");
        }
    }
}
