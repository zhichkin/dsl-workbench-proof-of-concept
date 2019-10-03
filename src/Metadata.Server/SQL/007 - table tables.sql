USE [one-c-sharp];

IF NOT EXISTS(SELECT 1 FROM sys.tables AS t
              INNER JOIN sys.schemas AS s
			  ON t.schema_id = s.schema_id
			  WHERE s.name = N'ocs' AND t.name = N'tables' AND t.type = N'U')
BEGIN
	CREATE TABLE [ocs].[tables]
	(
		[key] [uniqueidentifier] NOT NULL,
		[version] [rowversion] NOT NULL,
		[name] [nvarchar](100) NOT NULL,
		[alias] [nvarchar](100) NOT NULL,
		[schema] [nvarchar](100) NOT NULL,
		[purpose] [int] NOT NULL,
		CONSTRAINT [pk_tables] PRIMARY KEY CLUSTERED ([key] ASC)
	)
END