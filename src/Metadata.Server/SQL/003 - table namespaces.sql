USE [one-c-sharp];

IF NOT EXISTS(SELECT 1 FROM sys.tables AS t
              INNER JOIN sys.schemas AS s
			  ON t.schema_id = s.schema_id
			  WHERE s.name = N'ocs' AND t.name = N'namespaces' AND t.type = N'U')
BEGIN
	CREATE TABLE [ocs].[namespaces]
	(
		[key] [uniqueidentifier] NOT NULL,
		[version] [rowversion] NOT NULL,
		[owner] [uniqueidentifier] NOT NULL,
		[owner_] [int] NOT NULL,
		[name] [nvarchar](100) NOT NULL,
		[alias] [nvarchar](100) NOT NULL,
		CONSTRAINT [pk_namespaces] PRIMARY KEY CLUSTERED ([key] ASC)
	)
END

IF NOT EXISTS(SELECT 1 FROM [ocs].[namespaces]
			   WHERE [key]    = CAST(0x00000000000000000000000000000000 AS uniqueidentifier)
			     AND [owner_] = 1
				 AND [owner]  = CAST(0x00000000000000000000000000000000 AS uniqueidentifier))
BEGIN
	INSERT [ocs].[namespaces] ([key],[owner_],[owner],[name],[alias])
	VALUES
	(
		CAST(0x00000000000000000000000000000000 AS uniqueidentifier),
		1, -- InfoBase
		CAST(0x00000000000000000000000000000000 AS uniqueidentifier),
		N'TypeSystem',
		N''
	);
END