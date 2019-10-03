USE [one-c-sharp];

IF NOT EXISTS(SELECT 1 FROM sys.tables AS t
              INNER JOIN sys.schemas AS s
			  ON t.schema_id = s.schema_id
              WHERE s.name = N'ocs' AND t.name = N'infobases' AND t.type = N'U')
BEGIN
	CREATE TABLE [ocs].[infobases]
	(
		[key] [uniqueidentifier] NOT NULL,
		[version] [rowversion] NOT NULL,
		[name]     [nvarchar](100) NOT NULL,
		[alias]    [nvarchar](100) NOT NULL,
		[server]   [nvarchar](100) NOT NULL,
		[database] [nvarchar](100) NOT NULL,
		[username] [nvarchar](100) NOT NULL,
		[password] [nvarchar](100) NOT NULL,
		CONSTRAINT [pk_infobases] PRIMARY KEY CLUSTERED ([key] ASC)
	)
END

IF NOT EXISTS(SELECT 1 FROM [ocs].[infobases] WHERE [key] = CAST(0x00000000000000000000000000000000 AS uniqueidentifier))
BEGIN
	INSERT [ocs].[infobases] ([key], [name], [alias], [server], [database], [username], [password])
	VALUES (CAST(0x00000000000000000000000000000000 AS uniqueidentifier), N'Metadata', N'', N'', N'', N'', N'');
END

