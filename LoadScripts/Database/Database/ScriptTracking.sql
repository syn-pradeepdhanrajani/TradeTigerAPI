USE [Stocks]
GO

/****** Object:  Table [dbo].[ScriptTracking]    Script Date: 04-09-2016 13:22:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScriptTracking](
	[ScriptTrackingId] [int] IDENTITY(1,1) NOT NULL,
	[ScriptId] [int] NOT NULL,
	[ScriptTrackingStatus] [int] NOT NULL,
	[IsWeeklyTrackingStatus] [bit] NOT NULL CONSTRAINT [DF_Table_1_IsWeeklyPrice]  DEFAULT ((0)),
	[IsMonthlyTrackingStatus] [bit] NOT NULL CONSTRAINT [DF_Table_1_IsMonthlyPrice]  DEFAULT ((0)),
	[IsQuarterlyTrackingStatus] [bit] NOT NULL CONSTRAINT [DF_Table_1_IsQuarterlyPrice]  DEFAULT ((0)),
	[IsDailyTrackingStatus] [bit] NOT NULL CONSTRAINT [DF_Table_1_IsDailyPrice]  DEFAULT ((0)),
	[IsOpenLowSamePrice] [bit] NOT NULL CONSTRAINT [DF_Table_1_IsOpenLowSamePrice]  DEFAULT ((0)),
	[IsOpenHighSamePrice] [bit] NOT NULL CONSTRAINT [DF_Table_1_IsOpenHighSamePrice]  DEFAULT ((0)),
	[TradeDate] [date] NOT NULL,
	[ClosingPrice] [float] NULL,
	[TrackingDetails] [nvarchar](max) NULL,
 CONSTRAINT [PK_ScriptTracking] PRIMARY KEY CLUSTERED 
(
	[ScriptTrackingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

