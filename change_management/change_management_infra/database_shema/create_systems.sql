/****** Object:  Table [dbo].[systems]    Script Date: 29/07/2020 16:22:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[systems](
	[systemId] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[code] [nvarchar](50) NULL,
	[description] [nvarchar](max) NULL,
	[techStack] [nvarchar](max) NULL,
	[pointOfContact] [int] NULL,
	[owningTeam] [int] NULL,
 CONSTRAINT [PK_system_1] PRIMARY KEY CLUSTERED 
(
	[systemId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[systems]  WITH CHECK ADD  CONSTRAINT [FK_system_teams] FOREIGN KEY([owningTeam])
REFERENCES [dbo].[teams] ([teamId])
GO

ALTER TABLE [dbo].[systems] CHECK CONSTRAINT [FK_system_teams]
GO

ALTER TABLE [dbo].[systems]  WITH CHECK ADD  CONSTRAINT [FK_system_users] FOREIGN KEY([pointOfContact])
REFERENCES [dbo].[users] ([userId])
GO

ALTER TABLE [dbo].[systems] CHECK CONSTRAINT [FK_system_users]
GO

