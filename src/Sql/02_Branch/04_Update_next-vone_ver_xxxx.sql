--USE [VOneG4]
--GO

BEGIN TRANSACTION
GO

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ImportData]'))
BEGIN
    CREATE TABLE [dbo].[ImportData]
    ([Id]                   BIGINT          NOT NULL    IDENTITY(1,1)
    ,[CompanyId]            INT             NOT NULL
    ,[FileName]             NVARCHAR(255)   NOT NULL
    ,[FileSize]             INT             NOT NULL
    ,[CreateBy]             INT             NOT NULL
    ,[CreateAt]             DATETIME2(3)    NOT NULL
    ,CONSTRAINT [PkImportData] PRIMARY KEY CLUSTERED
    ([Id] ASC )
    ,CONSTRAINT [FkImportDataCompanyId] FOREIGN KEY
    ([CompanyId]) REFERENCES [dbo].[Company] ([Id]) ON DELETE CASCADE
    )

    DECLARE @v sql_variant
    SET @v = N'インポートデータID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportData', @level2type=N'COLUMN',@level2name=N'Id'
    SET @v = N'会社ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportData', @level2type=N'COLUMN',@level2name=N'CompanyId'
    SET @v = N'ファイル名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportData', @level2type=N'COLUMN',@level2name=N'FileName'
    SET @v = N'ファイルサイズ'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportData', @level2type=N'COLUMN',@level2name=N'FileSize'
    SET @v = N'登録者ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportData', @level2type=N'COLUMN',@level2name=N'CreateBy'
    SET @v = N'登録日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportData', @level2type=N'COLUMN',@level2name=N'CreateAt'
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ImportDataDetail]'))
BEGIN
    CREATE TABLE [dbo].[ImportDataDetail]
    ([Id]                   BIGINT          NOT NULL    IDENTITY(1, 1)
    ,[ImportDataId]         BIGINT          NOT NULL
    ,[ObjectType]           INT             NOT NULL
    ,[RecordItem]           VARBINARY(MAX)  NOT NULL
    ,CONSTRAINT [PkImportDataDetail] PRIMARY KEY NONCLUSTERED
    ([Id]           ASC)
    ,CONSTRAINT [UqImportDataDetail] UNIQUE         CLUSTERED
    ([ImportDataId] ASC
    ,[ObjectType]   ASC
    ,[Id]           ASC)
    ,CONSTRAINT [FkImportDataId] FOREIGN KEY
    ([ImportDataId]) REFERENCES [dbo].[ImportData] ([Id]) ON DELETE CASCADE
    )

    DECLARE @v sql_variant
    SET @v = N'インポートデータ明細ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportDataDetail', @level2type=N'COLUMN',@level2name=N'Id'
    SET @v = N'インポートデータID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportDataDetail', @level2type=N'COLUMN',@level2name=N'ImportDataId'
    SET @v = N'オブジェクトタイプ'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportDataDetail', @level2type=N'COLUMN',@level2name=N'ObjectType'
    SET @v = N'変換したオブジェクト MessagePack で シリアライズ'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportDataDetail', @level2type=N'COLUMN',@level2name=N'RecordItem'
END
GO

COMMIT
