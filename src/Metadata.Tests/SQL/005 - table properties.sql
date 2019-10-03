USE [one-c-sharp];

IF NOT EXISTS(SELECT 1 FROM sys.tables AS t
              INNER JOIN sys.schemas AS s
			  ON t.schema_id = s.schema_id
			  WHERE s.name = N'ocs' AND t.name = N'properties' AND t.type = N'U')
BEGIN
	CREATE TABLE [ocs].[properties]
	(
		[key] [uniqueidentifier] NOT NULL,
		[version] [rowversion] NOT NULL,
		[name] [nvarchar](128) NOT NULL,
		[alias] [nvarchar](128) NOT NULL,
		[entity] [uniqueidentifier] NOT NULL,
		[purpose] [int] NOT NULL,
		[ordinal] [int] NOT NULL,
		[is_abstract] [bit] NOT NULL,
		[is_read_only] [bit] NOT NULL,
		[is_primary_key] [bit] NOT NULL,
		CONSTRAINT [pk_properties] PRIMARY KEY CLUSTERED ([key] ASC)
	)
END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = N'nnx_properties_entity' AND object_id = OBJECT_ID(N'ocs.properties'))
BEGIN
	CREATE NONCLUSTERED INDEX nnx_properties_entity ON [ocs].[properties] ([entity]);
END
