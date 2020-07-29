/****** Object:  Table [dbo].[changeStages]    Script Date: 29/07/2020 16:22:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[changeStages](
	[stageId] [int] NOT NULL,
	[changeId] [int] NOT NULL,
	[teamResponsibleId] [int] NOT NULL,
	[userResponsibleId] [int] NULL,
	[description] [nvarchar](max) NOT NULL,
	[acceptanceCriteria] [nvarchar](max) NOT NULL,
	[isDependant] [bit] NOT NULL,
 CONSTRAINT [PK_changeStages] PRIMARY KEY CLUSTERED 
(
	[stageId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[changeStages]  WITH CHECK ADD  CONSTRAINT [FK_changeStages_changeStages] FOREIGN KEY([changeId])
REFERENCES [dbo].[changes] ([changeId])
GO

ALTER TABLE [dbo].[changeStages] CHECK CONSTRAINT [FK_changeStages_changeStages]
GO

ALTER TABLE [dbo].[changeStages]  WITH CHECK ADD  CONSTRAINT [FK_changeStages_teams] FOREIGN KEY([teamResponsibleId])
REFERENCES [dbo].[teams] ([teamId])
GO

ALTER TABLE [dbo].[changeStages] CHECK CONSTRAINT [FK_changeStages_teams]
GO

ALTER TABLE [dbo].[changeStages]  WITH CHECK ADD  CONSTRAINT [FK_changeStages_users] FOREIGN KEY([userResponsibleId])
REFERENCES [dbo].[users] ([userId])
GO

ALTER TABLE [dbo].[changeStages] CHECK CONSTRAINT [FK_changeStages_users]
GO

