USE [Stocks]
GO

/****** Object:  Table [dbo].[MarketData]    Script Date: 04-09-2016 13:20:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MarketData](
	[StockId] [int] IDENTITY(1,1) NOT NULL,
	[Exchange] [nvarchar](255) NULL,
	[Scrip Name] [nvarchar](255) NULL,
	[% Change] [nvarchar](255) NULL,
	[Current] [nvarchar](255) NULL,
	[Last Traded Qty] [nvarchar](255) NULL,
	[Bid Qty] [nvarchar](255) NULL,
	[Bid Price] [nvarchar](255) NULL,
	[Offer Price] [nvarchar](255) NULL,
	[Offer Qty] [nvarchar](255) NULL,
	[Open] [nvarchar](255) NULL,
	[High] [nvarchar](255) NULL,
	[Low] [nvarchar](255) NULL,
	[Close] [nvarchar](255) NULL,
	[Last Updated Time] [nvarchar](255) NULL,
	[Last Traded Time] [nvarchar](255) NULL,
	[Last Traded Date] [nvarchar](255) NULL,
	[Qty] [nvarchar](255) NULL,
	[Total Buy Qty] [nvarchar](255) NULL,
	[Scrip Code] [nvarchar](255) NULL,
	[Total Sell Qty] [nvarchar](255) NULL,
	[OI Difference] [nvarchar](255) NULL,
	[OI Difference Percentage] [nvarchar](255) NULL,
	[Company Name] [nvarchar](255) NULL,
	[P#Open] [nvarchar](255) NULL,
	[P#High] [nvarchar](255) NULL,
	[P#Low] [nvarchar](255) NULL,
	[P#Close] [nvarchar](255) NULL,
	[P#Quantity] [nvarchar](255) NULL,
	[Pivot Res 3] [nvarchar](255) NULL,
	[Pivot Res 2] [nvarchar](255) NULL,
	[Pivot Res 1] [nvarchar](255) NULL,
	[Pivot] [nvarchar](255) NULL,
	[Pivot Sup 1] [nvarchar](255) NULL,
	[Pivot Sup 2] [nvarchar](255) NULL,
	[Pivot Sup 3] [nvarchar](255) NULL,
	[CreateDate] [date] NOT NULL CONSTRAINT [DF_MarketData_CreateDate]  DEFAULT (getdate())
) ON [PRIMARY]

GO

