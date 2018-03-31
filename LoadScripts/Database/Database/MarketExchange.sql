USE [Stocks]
GO

/****** Object:  Table [dbo].[MarketExchange]    Script Date: 04-09-2016 13:20:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MarketExchange](
	[MarketExchangeId] [int] IDENTITY(1,1) NOT NULL,
	[ExchangeCode] [nvarchar](50) NOT NULL,
	[ExchangeDescription] [nvarchar](50) NULL,
	[Exchange] [nvarchar](50) NULL,
 CONSTRAINT [PK_MarketExchange] PRIMARY KEY CLUSTERED 
(
	[MarketExchangeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

