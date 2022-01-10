/****** Object:  Table [dbo].[systemChanges]    Script Date: 29/07/2020 16:22:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[systemChanges](
	[systemChangeId] [int] NOT NULL,
	[systemId] [int] NOT NULL,
	[changeId] [int] NOT NULL,
 CONSTRAINT [PK_systemChanges] PRIMARY KEY CLUSTERED 
(
	[systemChangeId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[systemChanges]  WITH CHECK ADD  CONSTRAINT [FK_systemChanges_changes] FOREIGN KEY([changeId])
REFERENCES [dbo].[changes] ([changeId])
GO

ALTER TABLE [dbo].[systemChanges] CHECK CONSTRAINT [FK_systemChanges_changes]
GO

ALTER TABLE [dbo].[systemChanges]  WITH CHECK ADD  CONSTRAINT [FK_systemChanges_system] FOREIGN KEY([systemId])
REFERENCES [dbo].[systems] ([systemId])
GO

ALTER TABLE [dbo].[systemChanges] CHECK CONSTRAINT [FK_systemChanges_system]
GO

