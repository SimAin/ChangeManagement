/****** Object:  Table [dbo].[teamMembers]    Script Date: 29/07/2020 16:23:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[teamMembers](
	[teamMemberId] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[teamId] [int] NOT NULL,
	[throughput] [int] NOT NULL,
 CONSTRAINT [PK_teamMembers] PRIMARY KEY CLUSTERED 
(
	[teamMemberId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[teamMembers]  WITH CHECK ADD  CONSTRAINT [FK_teamMembers_teams] FOREIGN KEY([teamId])
REFERENCES [dbo].[teams] ([teamId])
GO

ALTER TABLE [dbo].[teamMembers] CHECK CONSTRAINT [FK_teamMembers_teams]
GO

ALTER TABLE [dbo].[teamMembers]  WITH CHECK ADD  CONSTRAINT [FK_teamMembers_users] FOREIGN KEY([userId])
REFERENCES [dbo].[users] ([userId])
GO

ALTER TABLE [dbo].[teamMembers] CHECK CONSTRAINT [FK_teamMembers_users]
GO

