USE [Stocks]
GO

/****** Object:  Table [dbo].[Script]    Script Date: 04-09-2016 13:21:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Script](
	[ScriptId] [int] IDENTITY(1,1) NOT NULL,
	[ScriptCode] [nvarchar](255) NOT NULL,
	[ScriptName] [nvarchar](500) NULL,
	[CompanyName] [nvarchar](500) NULL,
	[ScriptMarketExchangeId] [int] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Script_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Script2] PRIMARY KEY CLUSTERED 
(
	[ScriptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

