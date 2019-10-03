USE [one-c-sharp];

IF NOT EXISTS(SELECT 1 FROM sys.tables AS t
              INNER JOIN sys.schemas AS s
			  ON t.schema_id = s.schema_id
			  WHERE s.name = N'ocs' AND t.name = N'fields' AND t.type = N'U')
BEGIN
	CREATE TABLE [ocs].[fields]
	(
		[key]            UNIQUEIDENTIFIER NOT NULL,
		[version]        ROWVERSION       NOT NULL,
		[name]           NVARCHAR (100)   NOT NULL,
		[alias]          NVARCHAR (100)   NOT NULL,
		[table]          UNIQUEIDENTIFIER NOT NULL,
		[property]       UNIQUEIDENTIFIER NOT NULL,
		[purpose]        INT              NOT NULL,
		[type_name]      NVARCHAR (16)    NOT NULL,
		[length]         INT              NOT NULL,
		[precision]      INT              NOT NULL,
		[scale]          INT              NOT NULL,
		[is_nullable]    BIT              NOT NULL,
		[is_read_only]   BIT              NOT NULL, -- auto-generated
		[is_primary_key] BIT              NOT NULL,
		[key_ordinal]    TINYINT          NOT NULL
		CONSTRAINT [pk_fields] PRIMARY KEY CLUSTERED ([key] ASC)
	)
END

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = N'nnx_fields_property' AND object_id = OBJECT_ID(N'ocs.fields'))
BEGIN
	CREATE NONCLUSTERED INDEX nnx_fields_property ON [ocs].[fields] ([property]);
END