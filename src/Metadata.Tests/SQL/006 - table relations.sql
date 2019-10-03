USE [one-c-sharp];

IF NOT EXISTS(SELECT 1 FROM sys.tables AS t
              INNER JOIN sys.schemas AS s
			  ON t.schema_id = s.schema_id
			  WHERE s.name = N'ocs' AND t.name = N'relations' AND t.type = N'U')
BEGIN
	CREATE TABLE [ocs].[relations]
	(
		[key] [uniqueidentifier] NOT NULL,
		[version] [rowversion] NOT NULL,
		[name] [nvarchar](128) NOT NULL,
		[alias] [nvarchar](128) NOT NULL,
		[property] [uniqueidentifier] NOT NULL,
		[entity] [uniqueidentifier] NOT NULL, -- allowed data type for the property
		[on_delete] [int] NOT NULL,
		CONSTRAINT [pk_relations] PRIMARY KEY CLUSTERED ([key] ASC),
		CONSTRAINT [nux_property_entity] UNIQUE ([property] ASC, [entity] ASC)
	)
END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = N'nnx_relations_entity' AND object_id = OBJECT_ID(N'ocs.relations'))
BEGIN
	CREATE NONCLUSTERED INDEX nnx_relations_entity ON [ocs].[relations] ([entity]);
END