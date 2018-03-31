USE [Stocks]
GO

/****** Object:  Table [dbo].[ScriptTrackingStatus]    Script Date: 04-09-2016 13:22:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScriptTrackingStatus](
	[ScriptTrackingStatusId] [int] NOT NULL,
	[ScriptTrackingStatusDescription] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

