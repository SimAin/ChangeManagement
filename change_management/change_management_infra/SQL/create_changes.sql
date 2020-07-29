/****** Object:  Table [dbo].[changes]    Script Date: 29/07/2020 16:21:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[changes](
	[changeId] [int] IDENTITY(1,1) NOT NULL,
	[systemId] [int] NOT NULL,
	[type] [nvarchar](50) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[criticality] [bit] NOT NULL,
	[deadline] [datetime] NOT NULL,
	[priority] [int] NOT NULL,
	[approverId] [int] NOT NULL,
	[stakeholderId] [int] NULL,
	[teamResponsibleId] [int] NULL,
	[userResponsibleId] [int] NULL,
	[processingTimeDays] [int] NULL,
	[statusId] [int] NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateStarted] [datetime] NULL,
 CONSTRAINT [PK_changes] PRIMARY KEY CLUSTERED 
(
	[changeId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[changes]  WITH CHECK ADD  CONSTRAINT [FK_changes_status] FOREIGN KEY([statusId])
REFERENCES [dbo].[status] ([statusId])
GO

ALTER TABLE [dbo].[changes] CHECK CONSTRAINT [FK_changes_status]
GO

ALTER TABLE [dbo].[changes]  WITH CHECK ADD  CONSTRAINT [FK_changes_systems] FOREIGN KEY([systemId])
REFERENCES [dbo].[systems] ([systemId])
GO

ALTER TABLE [dbo].[changes] CHECK CONSTRAINT [FK_changes_systems]
GO

ALTER TABLE [dbo].[changes]  WITH CHECK ADD  CONSTRAINT [FK_changes_teams] FOREIGN KEY([teamResponsibleId])
REFERENCES [dbo].[teams] ([teamId])
GO

ALTER TABLE [dbo].[changes] CHECK CONSTRAINT [FK_changes_teams]
GO

ALTER TABLE [dbo].[changes]  WITH CHECK ADD  CONSTRAINT [FK_changes_users] FOREIGN KEY([changeId])
REFERENCES [dbo].[users] ([userId])
GO

ALTER TABLE [dbo].[changes] CHECK CONSTRAINT [FK_changes_users]
GO

ALTER TABLE [dbo].[changes]  WITH CHECK ADD  CONSTRAINT [FK_changes_users1] FOREIGN KEY([stakeholderId])
REFERENCES [dbo].[users] ([userId])
GO

ALTER TABLE [dbo].[changes] CHECK CONSTRAINT [FK_changes_users1]
GO

ALTER TABLE [dbo].[changes]  WITH CHECK ADD  CONSTRAINT [FK_changes_users2] FOREIGN KEY([userResponsibleId])
REFERENCES [dbo].[users] ([userId])
GO

ALTER TABLE [dbo].[changes] CHECK CONSTRAINT [FK_changes_users2]
GO

