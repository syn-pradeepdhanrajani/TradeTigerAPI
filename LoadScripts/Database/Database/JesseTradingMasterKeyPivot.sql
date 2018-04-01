USE [Stocks]
GO

/****** Object:  Table [dbo].[JesseTradingMasterKeyPivot]    Script Date: 4/1/2018 11:36:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[JesseTradingMasterKeyPivot](
	[JesseTradingMasterKeyPivotId] [int] IDENTITY(1,1) NOT NULL,
	[ScriptId] [int] NOT NULL,
	[SecondaryRallyPrice] [float] NULL,
	[NaturalRallyPrice] [float] NULL,
	[UptrendPrice] [float] NULL,
	[DowntrendPrice] [float] NULL,
	[NaturalReactionPrice] [float] NULL,
	[SecondaryReactionPrice] [float] NULL,
	[IsPivot] [bit] NOT NULL,
 CONSTRAINT [PK_JesseTradingMasterKeyPivot] PRIMARY KEY CLUSTERED 
(
	[JesseTradingMasterKeyPivotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

