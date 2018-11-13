SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

DROP TABLE IF EXISTS [dbo].[SearchResults]
DROP TABLE IF EXISTS [dbo].[SearchQueries]
DROP TABLE IF EXISTS [dbo].[SearchEngines]

CREATE TABLE [dbo].[SearchEngines](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[URL] [nvarchar](250) NOT NULL,
	[Domain] [nvarchar](250) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[SearchQueries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Query] [nvarchar](2048) NOT NULL,
	[SearchDate] [datetime] NOT NULL,
	[SearchEngineId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[SearchQueries]  WITH CHECK ADD  CONSTRAINT [FK_SearchQueries_ToSearchEngine] FOREIGN KEY([SearchEngineId])
REFERENCES [dbo].[SearchEngines] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[SearchQueries] CHECK CONSTRAINT [FK_SearchQueries_ToSearchEngine]

CREATE TABLE [dbo].[SearchResults](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[URL] [nvarchar](2048) NOT NULL,
	[Text] [nvarchar](2048) NOT NULL,
	[SearchQueryId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[SearchResults]  WITH CHECK ADD  CONSTRAINT [FK_SearchResults_ToSearchQueries] FOREIGN KEY([SearchQueryId])
REFERENCES [dbo].[SearchQueries] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[SearchResults] CHECK CONSTRAINT [FK_SearchResults_ToSearchQueries]
