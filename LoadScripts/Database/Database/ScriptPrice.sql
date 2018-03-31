USE [Stocks]
GO

/****** Object:  Table [dbo].[ScriptPrice]    Script Date: 04-09-2016 13:21:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScriptPrice](
	[ScriptPriceId] [int] IDENTITY(1,1) NOT NULL,
	[ScriptId] [int] NOT NULL,
	[ClosingPrice] [float] NOT NULL,
	[DayOpen] [float] NOT NULL,
	[DayHigh] [float] NOT NULL,
	[DayLow] [float] NOT NULL,
	[TradeDate] [date] NOT NULL,
	[DayVolume] [numeric](18, 0) NULL,
	[OpenInterestPercentage] [float] NULL,
	[OpenInterestDifference] [numeric](18, 0) NULL,
	[IsWeeklyPrice] [bit] NOT NULL CONSTRAINT [DF_ScriptPrice_IsWeeklyPrice]  DEFAULT ((0)),
	[IsMonthlyPrice] [bit] NOT NULL CONSTRAINT [DF_ScriptPrice_IsMonthlyPrice]  DEFAULT ((0)),
	[IsQuarterlyPrice] [bit] NOT NULL CONSTRAINT [DF_ScriptPrice_IsQuarterlyPrice]  DEFAULT ((0)),
	[IsDailyPrice] [bit] NOT NULL CONSTRAINT [DF_ScriptPrice_IsDailyPrice]  DEFAULT ((0)),
 CONSTRAINT [PK_ScriptPrice] PRIMARY KEY CLUSTERED 
(
	[ScriptPriceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

