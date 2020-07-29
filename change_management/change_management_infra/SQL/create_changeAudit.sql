/****** Object:  Table [dbo].[changeAudit]    Script Date: 29/07/2020 16:21:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[changeAudit](
	[changeAuditId] [int] IDENTITY(1,1) NOT NULL,
	[auditType] [nvarchar](50) NOT NULL,
	[changeId] [int] NULL,
	[systemId] [int] NULL,
	[updateUserId] [int] NULL,
	[type] [nvarchar](50) NULL,
	[description] [nvarchar](max) NULL,
	[criticality] [bit] NULL,
	[deadline] [datetime] NULL,
	[priority] [int] NULL,
	[approverId] [int] NULL,
	[stakeholderId] [int] NULL,
	[teamResponsibleId] [int] NULL,
	[userResponsibleId] [int] NULL,
	[processingTimeDays] [int] NULL,
	[statusId] [int] NULL,
	[dateCreated] [datetime] NULL,
	[dateStarted] [datetime] NULL,
	[comment] [nvarchar](max) NULL,
	[auditDate] [datetime] NULL,
 CONSTRAINT [PK_audit] PRIMARY KEY CLUSTERED 
(
	[changeAuditId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[changeAudit]  WITH CHECK ADD  CONSTRAINT [FK_changeAudit_changes] FOREIGN KEY([changeId])
REFERENCES [dbo].[changes] ([changeId])
GO

ALTER TABLE [dbo].[changeAudit] CHECK CONSTRAINT [FK_changeAudit_changes]
GO

ALTER TABLE [dbo].[changeAudit]  WITH CHECK ADD  CONSTRAINT [FK_changeAudit_users] FOREIGN KEY([updateUserId])
REFERENCES [dbo].[users] ([userId])
GO

ALTER TABLE [dbo].[changeAudit] CHECK CONSTRAINT [FK_changeAudit_users]
GO

