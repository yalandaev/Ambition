IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Migrations]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
	CREATE TABLE [dbo].[Migrations](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[FileName] [varchar](255) NOT NULL,
		[Hash] [varchar](255) NOT NULL,
		[ExecutionDate] [datetime2](7) NOT NULL,
		[Duration] [int] NOT NULL,
	 CONSTRAINT [PK_Migrations] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]