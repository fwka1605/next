--USE [VOneG4]
--GO

BEGIN TRANSACTION
GO

/* 得意先マスター 督促状発行対象外 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'ExcludeReminderPublish'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ExcludeReminderPublish'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[Customer]')
 and name = N'ExcludeReminderPublish' )
ALTER TABLE [dbo].[Customer] ADD
 [ExcludeReminderPublish]       INT     NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'ExcludeReminderPublish'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'督促状発行対象外'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ExcludeReminderPublish'
GO

/* 得意先マスター 督促状発行対象外、相手先部署、敬称 項目追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[ImporterSettingBase] b
     WHERE b.[FormatId]     = 4
       AND b.[Sequence]     = 61
       AND b.[FieldName]    = N'督促状発行対象外' )
BEGIN
    INSERT INTO [dbo].[ImporterSettingBase]
    SELECT 4 [FormatId], u.[seq], u.[fname], u.[target], u.[idev], u.[adev]
      FROM (
                  SELECT 61 [seq], 12 [idev], 0 [adev], N'ExcludeReminderPublish'       [target], N'督促状発行対象外'   [fname]
        UNION ALL SELECT 62 [seq],  1 [idev], 3 [adev], N'DestinationDepartmentName'    [target], N'相手先部署'         [fname]
        UNION ALL SELECT 63 [seq], 11 [idev], 3 [adev], N'Honorific'                    [target], N'敬称'               [fname]
           ) u
     WHERE NOT EXISTS (
           SELECT *
             FROM [dbo].[ImporterSettingBase] b
            WHERE b.[FormatId]      = 4
              AND b.[Sequence]      = u.[seq] )

    INSERT INTO [dbo].[ImporterSettingDetail]
    SELECT s.[Id] [ImporterSettingId]
         , b.[Sequence]
         , 0 [ImportDivision]
         , 0 [FieldIndex]
         , N'' [Caption]
         , 0 [AttributeDivision]
         , 0 [ItemPriority]
         , 0 [DoOVerwrite]
         , 0 [IsUnique]
         , CASE WHEN b.[ImportDivision] = 12 AND b.[AttributeDivision] = 0  THEN N'0'
                                                                            ELSE N'' END [FixedValue]
         , 0 [UpdateKey]
         , 0 [CreateBy]
         , GETDATE() [CreateAt]
         , 0 [UpdateBy]
         , GETDATE() [UpdateAt]
      FROM [dbo].[ImporterSetting] s
     INNER JOIN [dbo].[ImporterSettingBase] b   ON b.[FormatId] = s.[FormatId]  AND s.[FormatId] = 4
     WHERE NOT EXISTS (
           SELECT 1
             FROM [dbo].[ImporterSettingDetail] d
            WHERE d.[ImporterSettingId]     = s.[Id]
              AND d.[Sequence]              = b.[Sequence] )

END
GO

/* 送付先マスターに送付先名を追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[Destination]')
 and name = N'Name' )
BEGIN
    ALTER TABLE [dbo].[Destination]
      ADD [Name]    NVARCHAR(40) NOT NULL DEFAULT N''
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'送付先名', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Destination', @level2type=N'COLUMN',@level2name=N'Name'
END
GO

/* PDF出力設定テーブル 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[PdfOutputSetting]'))
BEGIN
    CREATE TABLE [dbo].[PdfOutputSetting]
    ([Id]                INT            NOT NULL IDENTITY(1,1)
    ,[CompanyId]         INT            NOT NULL
    ,[ReportType]        INT            NOT NULL
    ,[OutputUnit]        INT            NOT NULL
    ,[FileName]          NVARCHAR(50)   NOT NULL
    ,[UseCompression]    INT            NOT NULL
    ,[MaximumSize]       numeric(18, 5)     NULL
    ,[CreateBy]          INT            NOT NULL
    ,[CreateAt]          DATETIME2(3)   NOT NULL
    ,[UpdateBy]          INT            NOT NULL
    ,[UpdateAt]          DATETIME2(3)   NOT NULL
    ,CONSTRAINT [PkPdfOutputSetting] PRIMARY KEY NONCLUSTERED
    ([Id] ASC 
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ,CONSTRAINT [UqPdfOutputSetting] UNIQUE CLUSTERED 
    ([CompanyId]  ASC
    ,[ReportType] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[PdfOutputSetting] ADD CONSTRAINT
     [FkPdfOutputSettingCompany] FOREIGN KEY
    ([CompanyId]) REFERENCES [dbo].[Company] ([Id])

    DECLARE @v sql_variant
    SET @v = N'PDF出力設定ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'Id'
    SET @v = N'会社ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
    SET @v = N'レポート区分'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'ReportType'
    SET @v = N'出力単位'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'OutputUnit'
    SET @v = N'ファイル名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'FileName'
    SET @v = N'ファイルを圧縮する'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'UseCompression'
    SET @v = N'圧縮時最大バイト数'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'MaximumSize'
    SET @v = N'登録者ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
    SET @v = N'登録日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
    SET @v = N'更新者ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
    SET @v = N'更新日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PdfOutputSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
END
GO

/* 督促状集計設定ベース 送付先コード追加 */
INSERT INTO ReminderSummarySettingBase
(ColumnName, ColumnNameJp, DisplayOrder, Available)
SELECT u.*
  FROM (
          SELECT 'DestinationCode'   [ColumnName], '送付先コード' [ColumnNameJp], 7     [DisplayOrder], 1    [Available]
) u
WHERE NOT EXISTS (
    SELECT 1
      FROM [dbo].[ReminderSummarySettingBase] b
     WHERE u.[ColumnName] = b.[ColumnName]
       AND u.[DisplayOrder] = b.[DisplayOrder]
)
     ORDER BY u.[ColumnName], u.[DisplayOrder]
GO

/* 督促状発行履歴 送付先ID追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[ReminderOutputed]')
 and name = N'DestinationId' )
ALTER TABLE [dbo].[ReminderOutputed] ADD
 [DestinationId]    INT     NULL
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ReminderOutputed', N'COLUMN',N'DestinationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'送付先ID'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReminderOutputed', @level2type=N'COLUMN',@level2name=N'DestinationId'
GO

/* 管理マスターベース 請求データ入力明細数追加 */
INSERT INTO [dbo].[GeneralSettingBase]
     ( Code
     , Value
     , Length
     , Precision
     , Description )
SELECT u.*
  FROM (
           SELECT N'請求入力明細件数'     [Code], N'10'   [Value], 2   [Length], 0    [Precision], N'請求データ入力時に、新規登録可能な明細件数'    [Description]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[GeneralSettingBase] b
        WHERE b.[Code]  = u.[Code] );
GO

INSERT INTO [dbo].[GeneralSetting]
     ( [CompanyId]
     , [Code]
     , [Value]
     , [Length]
     , [Precision]
     , [Description]
     , [CreateBy]
     , [CreateAt]
     , [UpdateBy]
     , [UpdateAt])
SELECT cm.[Id]          [CompanyId]
     , gsb.Code         [Code]
     , gsb.Value        [Value]
     , gsb.Length       [Length]
     , gsb.Precision    [Precision]
     , gsb.Description  [Description]
     , 0                [CreateBy]
     , getdate()        [CreateAt]
     , 0                [UpdateBy]
     , getdate()        [UpdateAt]
FROM GeneralSettingBase gsb
  INNER JOIN [dbo].[Company] cm
      ON cm.Id = cm.Id
    AND NOT EXISTS (
       SELECT 1
         FROM [dbo].[GeneralSetting] gs
        WHERE gs.CompanyId  = cm.Id
          AND gs.Code       = gsb.Code)
GO

/* 請求書設定 備考欄印字用文字数制御コラム追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[InvoiceCommonSetting]')
 and name = N'ControlInputCharacter' )
ALTER TABLE [dbo].[InvoiceCommonSetting] ADD
 [ControlInputCharacter]    INT     NOT NULL    DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'InvoiceCommonSetting', N'COLUMN',N'ControlInputCharacter'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考欄印字用文字数制御'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InvoiceCommonSetting', @level2type=N'COLUMN',@level2name=N'ControlInputCharacter'
GO

/* 得意先マスター 送付先項目追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[Customer]')
 and name = N'DestinationDepartmentName' )
BEGIN
ALTER TABLE [dbo].[Customer] ADD
 [DestinationDepartmentName]    NVARCHAR(40)    NOT NULL    DEFAULT N''
,[Honorific]                    NVARCHAR(6)     NOT NULL    DEFAULT N''
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相手先部署', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'DestinationDepartmentName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'敬称', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'Honorific'
END
GO

/* グリッド表示設定 請求書の送付先コードを削除し送付先を追加 */
IF EXISTS (SELECT * FROM [GridSettingBase]
            WHERE [GridId]     = 6
              AND [ColumnName] = N'DestnationCode')
BEGIN
DELETE FROM [GridSetting]
 WHERE [GridId] = 6
DELETE FROM [GridSettingBase]
 WHERE [GridId]     = 6
   AND [ColumnName] = N'DestnationCode'

INSERT INTO [GridSettingBase]
([GridId], [ColumnName], [ColumnNameJp], [DisplayOrder], [DisplayWidth])
VALUES
(6, N'DestnationContent', N'送付先', 16, 400)

END

/* 新規画面追加 督促管理帳票*/
INSERT INTO [dbo].[Menu]
     ( [MenuId]
     , [MenuName]
     , [MenuCategory]
     , [Sequence]
     , [CreateBy]
     , [CreateAt]
     , [UpdateBy]
     , [UpdateAt] )
SELECT u.Id             [MenuId]
     , u.Name           [MenuName]
     , u.Category       [MenuCategory]
     , u.[Sequence]
     , 0                [CreateBy]
     , getdate()        [CreateAt]
     , 0                [UpdateBy]
     , getdate()        [UpdateAt]
  FROM (
            SELECT N'PI0301' [Id], N'PI' [Category], 7030 [Sequence], N'督促管理帳票' [Name]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Menu] m
        WHERE m.MenuId      = u.Id )
 ORDER BY
       u.Id;
GO

   /*
 既存のメニュー権限に追加 メニュー追加後、一度流せば良いので、修正クエリの最下部に配置すること
 メニューを利用したい場合は、各種設定＆セキュリティ 画面で別途有効化すること
 */
INSERT INTO [dbo].[MenuAuthority]
     ( [CompanyId]
     , [MenuId]
     , [AuthorityLevel]
     , [Available]
     , [CreateBy]
     , [CreateAt]
     , [UpdateBy]
     , [UpdateAt])
SELECT cmp.[Id] [CompanyId]
     , m.MenuId
     , lvl.AuthorityLevel
     , 0 [Available]
     , 0            [CreateBy]
     , getdate()    [CreateAt]
     , 0            [UpdateBy]
     , getdate()    [UpdateAt]
  FROM [Company] cmp
 INNER JOIN (
SELECT 1 [AuthorityLevel] UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4
       ) lvl
    ON cmp.[Id]     = cmp.[Id]
 INNER JOIN [dbo].[Menu] m
    ON m.MenuId = m.MenuId
   AND NOT EXISTS (
       SELECT 1
         FROM [dbo].[MenuAuthority] ma
        WHERE ma.CompanyId  = cmp.[Id]
          AND ma.MenuId     = m.MenuId
          AND ma.AuthorityLevel = lvl.AuthorityLevel );
GO

COMMIT
