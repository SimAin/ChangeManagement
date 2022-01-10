/****** Object:  Table [dbo].[users]    Script Date: 29/07/2020 16:23:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[users](
	[userId] [int] IDENTITY(1,1) NOT NULL,
	[forename] [nvarchar](50) NOT NULL,
	[surname] [nvarchar](50) NOT NULL,
	[role] [nvarchar](50) NULL,
	[admin] [bit] NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

