USE [ScarletTrunk]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuthenticationStorage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AuthenticationStorage](
    [AuthenticationKey] [varchar](200) COLLATE Japanese_CS_AS_KS_WS NOT NULL,
 CONSTRAINT [PKAuthenticationStorage] PRIMARY KEY CLUSTERED 
(
    [AuthenticationKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConnectionInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConnectionInfo](
    [CompanyCode]       [varchar](100) COLLATE Japanese_CS_AS_KS_WS NOT NULL,
    [ConnectionString]  [varchar](300) COLLATE Japanese_CI_AS_KS_WS NOT NULL,
    [DatabaseName]      [varchar](100) COLLATE Japanese_CI_AS_KS_WS NOT NULL,
 CONSTRAINT [PKConnectionInfo] PRIMARY KEY CLUSTERED 
(
    [CompanyCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SessionStorage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SessionStorage](
    [SessionKey]        [varchar](100) COLLATE Japanese_CS_AS_KS_WS NOT NULL,
    [ConnectionInfo]    [varchar](300) COLLATE Japanese_CI_AS_KS_WS NOT NULL,
    [CompanyId]         [int]                                           NULL,
    [LoginUserId]       [int]                                           NULL,
    [CreatedAt]         [datetime]                                  NOT NULL
 CONSTRAINT [PKSessionStorage] PRIMARY KEY CLUSTERED 
(
    [SessionKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Logs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Logs]
([Id]           [BIGINT]                                        NOT NULL        IDENTITY(1, 1)
,[LoggedAt]     [DATETIME2](3)                                  NOT NULL
,[Level]        [VARCHAR](5)                                    NOT NULL
,[Logger]       [VARCHAR](100)                                  NOT NULL
,[SessionKey]   [VARCHAR](100)  COLLATE Japanese_CS_AS_KS_WS        NULL
,[CompanyCode]  [VARCHAR](100)  COLLATE Japanese_CS_AS_KS_WS        NULL
,[Message]      [NVARCHAR](MAX)                                 NOT NULL
,[Exception]    [NVARCHAR](MAX)                                     NULL
,[DatabaseName] [VARCHAR](100)  COLLATE Japanese_CI_AS_KS_WS        NULL
,[Query]        [NVARCHAR](MAX)                                     NULL
,[Parameters]   [NVARCHAR](MAX)                                     NULL
,CONSTRAINT [PKLogs] PRIMARY KEY CLUSTERED
([Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

