USE [one-c-sharp];

IF NOT EXISTS(SELECT 1 FROM sys.tables AS t
              INNER JOIN sys.schemas AS s
			  ON t.schema_id = s.schema_id
			  WHERE s.name = N'ocs' AND t.name = N'queries' AND t.type = N'U')
BEGIN
	CREATE TABLE [ocs].[queries]
	(
		[key] [uniqueidentifier] NOT NULL,
		[version] [rowversion] NOT NULL,
		[name] [nvarchar](100) NOT NULL,
		[alias] [nvarchar](100) NOT NULL,
		[namespace] [uniqueidentifier] NOT NULL,
		[entity] [uniqueidentifier] NOT NULL,
		[parse_tree] [nvarchar](max) NOT NULL,
		[request_type] [uniqueidentifier] NOT NULL,
		[response_type] [uniqueidentifier] NOT NULL,
		CONSTRAINT [pk_queries] PRIMARY KEY CLUSTERED ([key] ASC)
	)
END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = N'nnx_queries_namespace' AND object_id = OBJECT_ID(N'ocs.queries'))
BEGIN
	CREATE NONCLUSTERED INDEX nnx_queries_namespace ON [ocs].[queries] ([namespace]);
END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = N'nnx_queries_entity' AND object_id = OBJECT_ID(N'ocs.queries'))
BEGIN
	CREATE NONCLUSTERED INDEX nnx_queries_entity ON [ocs].[queries] ([entity]);
END