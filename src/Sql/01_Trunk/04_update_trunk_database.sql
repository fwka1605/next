USE [ScarletTrunk]

IF ((SELECT COLUMNPROPERTY(object_id(N'SessionStorage'), 'CompanyId', 'AllowsNull')) IS NULL)
BEGIN
    ALTER TABLE [dbo].[SessionStorage] ADD CompanyId INT NULL
END
GO
IF ((SELECT COLUMNPROPERTY(object_id(N'SessionStorage'), 'LoginUserId', 'AllowsNull')) IS NULL)
BEGIN
    ALTER TABLE [dbo].[SessionStorage] ADD LoginUserId INT NULL
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.tables where object_id = object_id(N'[dbo].[ConnectionInfo]'))
BEGIN
    CREATE TABLE [dbo].[ConnectionInfo](
        [CompanyCode] [varchar](100) COLLATE Japanese_CS_AS_KS_WS NOT NULL,
        [ConnectionString] [varchar](300) COLLATE Japanese_CI_AS_KS_WS NOT NULL,
        CONSTRAINT [PKConnectionInfo] PRIMARY KEY CLUSTERED 
        (
            [CompanyCode] ASC
        )
    ) ON [PRIMARY]


    INSERT INTO [dbo].[ConnectionInfo]
    VALUES
    ('RAC999'
    ,'Data Source=(local);Initial Catalog=ScarletBranch01;Integrated Security=False;User Id=Scarlet;Password=Scarlet;'
    )
END
GO
