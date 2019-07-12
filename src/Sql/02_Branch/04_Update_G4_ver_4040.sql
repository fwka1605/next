--USE [VOneG4]
--GO

BEGIN TRANSACTION
GO 

UPDATE [dbo].[Menu]
   SET [Sequence] = 6300
 WHERE [MenuId]   = N'PH1101'

 UPDATE [dbo].[Menu]
   SET [Sequence] = 6310
 WHERE [MenuId]   = N'PH1201'

 UPDATE [dbo].[Menu]
   SET [Sequence] = 6110
 WHERE [MenuId]   = N'PH1301'

/* 新規画面追加 */
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
            SELECT N'PH1401' [Id], N'PH' [Category], 6320 [Sequence], N'MFクラウド請求書 Web API 連携設定'  [Name]
 UNION ALL  SELECT N'PC1801' [Id], N'PC' [Category], 1160 [Sequence], N'MFクラウド請求書 データ抽出'    [Name]
 UNION ALL  SELECT N'PE1001' [Id], N'PE' [Category], 3100 [Sequence], N'MFクラウド会計 消込結果連携'        [Name]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Menu] m
        WHERE m.MenuId      = u.Id )
 ORDER BY
       u.Id;
GO

/* MFクラウド請求書テーブル 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[MFBilling]'))
BEGIN
    CREATE TABLE [dbo].[MFBilling]
    ( 
      [BillingId] bigint        NOT NULL
    , [CompanyId] int           NOT NULL
    , [Id]        nvarchar(100) NOT NULL
    , CONSTRAINT [PkMFBilling] PRIMARY KEY CLUSTERED
    ( [BillingId]                       ASC )
    , CONSTRAINT [UqMFBilling]  UNIQUE     ([Id])
    , CONSTRAINT [FkMFBillingBilling] FOREIGN KEY ([BillingId])
      REFERENCES [dbo].[Billing] ([Id])
      ON DELETE CASCADE
    , CONSTRAINT [FkMFBillingCompany] FOREIGN KEY ([CompanyId])
      REFERENCES [dbo].[Company] ([Id])
    );

    DECLARE @v sql_variant
    SET @v = N'MFクラウド請求書ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MFBilling', @level2type=N'COLUMN',@level2name=N'Id'
    SET @v = N'請求ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MFBilling', @level2type=N'COLUMN',@level2name=N'BillingId'
END
GO

/* 基本設定用 MFクラウド請求書 WebAPI利用 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseMFWebApi'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseMFWebApi'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[ApplicationControl]')
 and name = N'UseMFWebApi' )
ALTER TABLE [dbo].[ApplicationControl] ADD
 [UseMFWebApi]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseMFWebApi'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MFクラウド請求書 WebAPI利用'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseMFWebApi'
GO

/* 照合設定 一括消込入金情報表示順コラム 追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 WHERE object_id = object_id(N'[dbo].[CollationSetting]')
 AND name = N'SortOrderColumn' )
BEGIN
    ALTER TABLE [dbo].[CollationSetting]
     ADD [SortOrderColumn]  INT NOT NULL CONSTRAINT DfCollationSettingSortOrderColumn   DEFAULT 0
       , [SortOrder]        INT NOT NULL CONSTRAINT DfCollationSettingSortOrder         DEFAULT 0;

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込入金情報表示順コラム'          , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'SortOrderColumn'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込入金情報表示順 0:昇順・1:降順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'SortOrder'

    TRUNCATE TABLE WorkCollation;
    TRUNCATE TABLE WorkReceipt;

END
GO

/* 消込候補表示用 一括消込入金情報表示順用古い入金日 追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[WorkCollation]')
 and name = N'MinReceiptRecordedAt' )
BEGIN
    ALTER TABLE [dbo].[WorkCollation]
      ADD [MinReceiptRecordedAt]    DATE    NULL
        , [MaxReceiptRecordedAt]    DATE    NULL
        , [MinReceiptId]            BigInt  NULL
        , [MaxReceiptId]            BigInt  NULL;

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込入金情報表示順用古い入金日', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'MinReceiptRecordedAt'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込入金情報表示順用最近入金日', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'MaxReceiptRecordedAt'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込入金情報表示順用最小入金ID', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'MinReceiptId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込入金情報表示順用最大入金ID', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'MaxReceiptId'

END
GO

/* 消込候補入金データ作成用 一括消込入金情報表示順用最小入金日 追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[WorkReceipt]')
 and name = N'MinRecordedAt' )
BEGIN
    ALTER TABLE [dbo].[WorkReceipt]
      ADD [MinRecordedAt]   DATE    NULL
        , [MaxRecordedAt]   DATE    NULL
        , [MinReceiptId]    BigInt  NULL
        , [MaxReceiptId]    BigInt  NULL;

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最小入金日', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'MinRecordedAt'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大入金日', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'MaxRecordedAt'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最小入金ID', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'MinReceiptId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大入金ID', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'MaxReceiptId'

END
GO

/* グリッド表示設定・入金予定入力 項目追加 */
INSERT INTO [dbo].[GridSettingBase]
SELECT u.*
    FROM (
            SELECT 5 [id], N'Note5' [name], N'備考5' [jp], 29 [od], 0 [wd]
UNION ALL   SELECT 5 [id], N'Note6' [name], N'備考6' [jp], 30 [od], 0 [wd]
UNION ALL   SELECT 5 [id], N'Note7' [name], N'備考7' [jp], 31 [od], 0 [wd]
UNION ALL   SELECT 5 [id], N'Note8' [name], N'備考8' [jp], 32 [od], 0 [wd]
        ) u
WHERE NOT EXISTS (
        SELECT *
        FROM [dbo].[GridSettingBase] b
        WHERE    b.[GridId]     = u.[id]
            AND  b.[ColumnName] = u.[name] )
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 WHERE object_id = object_id(N'[dbo].[CollationSetting]')
 AND name = N'SetSystemDateToCreateAtFilter' )
BEGIN
    ALTER TABLE [dbo].[CollationSetting] ADD
     [SetSystemDateToCreateAtFilter]    int NOT NULL DEFAULT 0

    DECLARE @v sql_variant
    SET @v = N'消込済データ表示時、消込日時にシステム日時を設定'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CollationSetting', N'COLUMN', N'SetSystemDateToCreateAtFilter'
END
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