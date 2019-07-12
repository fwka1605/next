--USE [VOneG4]
--GO

BEGIN TRANSACTION
GO

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[Closing]'))
BEGIN
    CREATE TABLE [dbo].[Closing] (
        [CompanyId]            INT          NOT NULL
      , [ClosingMonth]         DATE         NOT NULL
      , [UpdateBy]             INT          NOT NULL
      , [UpdateAt]             DATETIME2(3) NOT NULL
      , CONSTRAINT [PkClosing] PRIMARY KEY NONCLUSTERED
     ( [CompanyId] ASC )
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[Closing] ADD CONSTRAINT
     [FkClosingCompany] FOREIGN KEY
    ([CompanyId]) REFERENCES [dbo].[Company] ([Id])

    DECLARE @v sql_variant
    SET @v = N'会社ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Closing', @level2type=N'COLUMN',@level2name=N'CompanyId'
    SET @v = N'締め年月'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Closing', @level2type=N'COLUMN',@level2name=N'ClosingMonth'
    SET @v = N'更新者ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Closing', @level2type=N'COLUMN',@level2name=N'UpdateBy'
    SET @v = N'更新日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Closing', @level2type=N'COLUMN',@level2name=N'UpdateAt'
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[ClosingSetting]'))
BEGIN
    CREATE TABLE [dbo].[ClosingSetting] (
        [CompanyId]                   INT          NOT NULL
      , [BaseDate]                    INT          NOT NULL
      , [AllowReceptJournalPending]   INT          NOT NULL
      , [AllowMutchingPending]        INT          NOT NULL
      , [UpdateBy]                    INT          NOT NULL
      , [UpdateAt]                    DATETIME2(3) NOT NULL
      , CONSTRAINT [PkClosingSetting] PRIMARY KEY NONCLUSTERED
     ( [CompanyId] ASC )
    ) ON [PRIMARY]

    ALTER TABLE [dbo].[ClosingSetting] ADD CONSTRAINT
     [FkClosingSettingCompany] FOREIGN KEY
    ([CompanyId]) REFERENCES [dbo].[Company] ([Id])

    DECLARE @v sql_variant
    SET @v = N'会社ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClosingSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
    SET @v = N'基準日'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClosingSetting', @level2type=N'COLUMN',@level2name=N'BaseDate'
    SET @v = N'入金仕訳未出力を許可する'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClosingSetting', @level2type=N'COLUMN',@level2name=N'AllowReceptJournalPending'
    SET @v = N'未消込入金データを許可する'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClosingSetting', @level2type=N'COLUMN',@level2name=N'AllowMutchingPending'
    SET @v = N'更新者ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClosingSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
    SET @v = N'更新日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ClosingSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
END
GO

/* 基本設定用 締め処理利用　カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseClosing'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseClosing'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[ApplicationControl]')
 and name = N'UseClosing' )
ALTER TABLE [dbo].[ApplicationControl] ADD
 [UseClosing]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseClosing'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'締め処理利用'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseClosing'
GO

/* 基本設定用 ファクタリング対応　カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseFactoring'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseFactoring'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[ApplicationControl]')
 and name = N'UseFactoring' )
ALTER TABLE [dbo].[ApplicationControl] ADD
 [UseFactoring]     INT                 NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseClosing'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ファクタリング対応'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseFactoring'
GO

/* 基本設定用 奉行クラウド対応　カラム追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
WHERE object_id = object_id(N'[dbo].[ApplicationControl]')
AND name = N'UseBugyoWebApi')
ALTER TABLE [dbo].[ApplicationControl] ADD
 [UseBugyoWebApi]   INT         NOT NULL CONSTRAINT DfApplicationControlUseBugyoWebApi DEFAULT 0;
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ApplicationControl', N'COLUMN',N'UseBugyoWebApi'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奉行クラウドWebAPI利用'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseBugyoWebApi'
GO

/* WebAPI接続設定 奉行クラウド TenantId   カラム追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[WebApiSetting]')
 and name = N'TenantId' )
BEGIN
ALTER TABLE [dbo].[WebApiSetting]
 ADD [TenantId]     NVARCHAR(100)                 NOT NULL DEFAULT N'';

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'テナントID'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WebApiSetting', @level2type=N'COLUMN',@level2name=N'TenantId'

END
GO

/* 汎用設定項目定義 奉行クラウド用 サブスクリプションキー 追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[SettingDefinition] d
     WHERE d.[ItemId]     = N'BugyoSubscriptionKey' )
BEGIN
    INSERT INTO [dbo].[SettingDefinition]
     ( [ItemId]
     , [Description]
     , [ItemKeyLength]
     , [ItemValueLength] )
    SELECT u.[iItemId]             [ItemId]
         , u.[iDescription]        [Description]
         , u.[iItemKeyLength]      [ItemKeyLength]
         , u.[iItemValueLength]    [ItemValueLength]
    FROM (
            SELECT N'BugyoSubscriptionKey'    [iItemId], N'奉行クラウドAPI接続キー'    [iDescription], 1    [iItemKeyLength], 40    [iItemValueLength]
         ) u
     WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[SettingDefinition] s
       WHERE s.[ItemId]     = u.[iItemId] )

END
GO

/* 汎用設定項目 奉行クラウド用 サブスクリプションキー 追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[Setting] d
     WHERE d.[ItemId]     = N'BugyoSubscriptionKey')
BEGIN
    INSERT INTO [dbo].[Setting]
     ( [ItemId]
     , [ItemKey]
     , [ItemValue] )
    SELECT u.[ItemId]
         , u.[ItemKey]
         , u.[ItemValue]
    FROM (
          SELECT N'BugyoSubscriptionKey'    [ItemId], N'0'    [ItemKey], N'2f5e47ad7e6a4276aceffa6d2331f6a4'    [ItemValue]
UNION ALL SELECT N'BugyoSubscriptionKey'    [ItemId], N'1'    [ItemKey], N'2e04e562a8be4f9fa5f583051650096e'    [ItemValue]
         ) u
     WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Setting] s
       WHERE s.[ItemId]     = u.[ItemId] )

END
GO

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
                SELECT N'PE1101' [Id], N'PE' [Category], 3110 [Sequence], N'奉行クラウド消込結果連携'   [Name]
    UNION ALL   SELECT N'PH1501' [Id], N'PH' [Category], 6120 [Sequence], N'締め処理'                   [Name]
    UNION ALL   SELECT N'PH1601' [Id], N'PH' [Category], 6330 [Sequence], N'奉行クラウド連携設定'       [Name]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Menu] m
        WHERE m.MenuId      = u.Id )
 ORDER BY
       u.Id;
GO

   /* 対象外処理日追加 */
IF NOT EXISTS (SELECT * FROM sys.columns
 WHERE object_id = object_id(N'[dbo].[ReceiptExclude]')
 AND name = N'RecordedAt' )
BEGIN
    ALTER TABLE [dbo].[ReceiptExclude] ADD
    [RecordedAt] Date NULL

    EXECUTE sp_executesql N'
    UPDATE re
       SET re.[RecordedAt] = r.RecordedAt
      FROM [ReceiptExclude] re
     INNER JOIN Receipt r
        ON r.Id = re.ReceiptId
    '

    ALTER TABLE [dbo].[ReceiptExclude] ALTER COLUMN
    [RecordedAt] Date NOT NULL
END
GO

--グリッド表示設定　対象外伝票日付の追加
DELETE FROM [dbo].[GridSetting]
 WHERE GridId = 2;
GO
DELETE FROM [dbo].[GridSettingBase]
 WHERE GridId = 2;
GO

INSERT INTO [dbo].[GridSettingBase]
     ( GridId
     , ColumnName
     , ColumnNameJp
     , DisplayOrder
     , DisplayWidth )
SELECT u.GridId
     , u.ColumnName
     , u.ColumnNameJp
     , u.DisplayOrder
     , u.DIsplayWidth
  FROM (
--入金データ検索
            SELECT 2 [GridId], 1  [DisplayOrder], 65  [DisplayWidth], N'ExcludeFlag'                [ColumnName], N'対象外'                   [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 2  [DisplayOrder], 150 [DisplayWidth], N'ExcludeCategory'            [ColumnName], N'対象外区分'               [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 3  [DisplayOrder], 110 [DisplayWidth], N'ExcludeRecordedAt'          [ColumnName], N'対象外伝票日付'           [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 4  [DisplayOrder], 90  [DisplayWidth], N'Id'                         [ColumnName], N'入金ID'                   [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 5  [DisplayOrder], 75  [DisplayWidth], N'AssignmentState'            [ColumnName], N'消込区分'                 [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 6  [DisplayOrder], 80  [DisplayWidth], N'RecordedAt'                 [ColumnName], N'入金日'                   [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 7  [DisplayOrder], 105 [DisplayWidth], N'CustomerCode'               [ColumnName], N'得意先コード'             [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 8  [DisplayOrder], 110 [DisplayWidth], N'CustomerName'               [ColumnName], N'得意先名'                 [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 9  [DisplayOrder], 150 [DisplayWidth], N'PayerName'                  [ColumnName], N'振込依頼人名'             [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 10 [DisplayOrder], 75  [DisplayWidth], N'CurrencyCode'               [ColumnName], N'通貨コード'               [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 11 [DisplayOrder], 80  [DisplayWidth], N'ReceiptAmount'              [ColumnName], N'入金額'                   [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 12 [DisplayOrder], 80  [DisplayWidth], N'RemainAmount'               [ColumnName], N'入金残'                   [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 13 [DisplayOrder], 100 [DisplayWidth], N'ExcludeAmount'              [ColumnName], N'対象外金額'               [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 14 [DisplayOrder], 85  [DisplayWidth], N'ReceiptCategoryName'        [ColumnName], N'入金区分'                 [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 15 [DisplayOrder], 100 [DisplayWidth], N'InputType'                  [ColumnName], N'入力区分'                 [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 16 [DisplayOrder], 150 [DisplayWidth], N'Note1'                      [ColumnName], N'備考'                     [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 17 [DisplayOrder], 150 [DisplayWidth], N'Memo'                       [ColumnName], N'メモ'                     [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 18 [DisplayOrder], 80  [DisplayWidth], N'DueAt'                      [ColumnName], N'入金期日'                 [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 19 [DisplayOrder], 110 [DisplayWidth], N'SectionCode'                [ColumnName], N'入金部門コード'           [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 20 [DisplayOrder], 150 [DisplayWidth], N'SectionName'                [ColumnName], N'入金部門名'               [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 21 [DisplayOrder], 80  [DisplayWidth], N'BankCode'                   [ColumnName], N'銀行コード'               [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 22 [DisplayOrder], 80  [DisplayWidth], N'BankName'                   [ColumnName], N'銀行名'                   [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 23 [DisplayOrder], 80  [DisplayWidth], N'BranchCode'                 [ColumnName], N'支店コード'               [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 24 [DisplayOrder], 80  [DisplayWidth], N'BranchName'                 [ColumnName], N'支店名'                   [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 25 [DisplayOrder], 80  [DisplayWidth], N'AccountNumber'              [ColumnName], N'口座番号'                 [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 26 [DisplayOrder], 80  [DisplayWidth], N'SourceBankName'             [ColumnName], N'仕向銀行'                 [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 27 [DisplayOrder], 80  [DisplayWidth], N'SourceBranchName'           [ColumnName], N'仕向支店'                 [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 28 [DisplayOrder], 100 [DisplayWidth], N'VirtualBranchCode'          [ColumnName], N'仮想支店コード'           [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 29 [DisplayOrder], 100 [DisplayWidth], N'VirtualAccountNumber'       [ColumnName], N'仮想口座番号'             [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 30 [DisplayOrder], 110 [DisplayWidth], N'OutputAt'                   [ColumnName], N'入金仕訳日'               [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 31 [DisplayOrder], 150 [DisplayWidth], N'Note2'                      [ColumnName], N'備考2'                    [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 32 [DisplayOrder], 150 [DisplayWidth], N'Note3'                      [ColumnName], N'備考3'                    [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 33 [DisplayOrder], 150 [DisplayWidth], N'Note4'                      [ColumnName], N'備考4'                    [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 34 [DisplayOrder], 0   [DisplayWidth], N'BillNumber'                 [ColumnName], N'手形番号'                 [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 35 [DisplayOrder], 0   [DisplayWidth], N'BillBankCode'               [ColumnName], N'券面銀行コード'           [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 36 [DisplayOrder], 0   [DisplayWidth], N'BillBranchCode'             [ColumnName], N'券面支店コード'           [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 37 [DisplayOrder], 0   [DisplayWidth], N'BillDrawAt'                 [ColumnName], N'振出日'                   [ColumnNameJp]
UNION ALL   SELECT 2 [GridId], 38 [DisplayOrder], 0   [DisplayWidth], N'BillDrawer'                 [ColumnName], N'振出人'                   [ColumnNameJp]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[GridSettingBase] b
        WHERE b.[GridId]        = u.[GridId]
          AND b.[ColumnName]    = u.[ColumnName] )
 ORDER BY
       u.GridId     ASC
     , u.ColumnName ASC;
GO

--管理マスター　ファクタリング関連項目追加
IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'UseFactoring' AND object_id = OBJECT_ID(N'Category'))
ALTER TABLE [Category] ADD [UseFactoring] int NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'FactoringCustomerId' AND object_id = OBJECT_ID(N'Category'))
ALTER TABLE [Category] ADD [FactoringCustomerId] int NULL
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkCategoryFactoringCustomer]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[Category]')
       )
ALTER TABLE [dbo].[Category] ADD CONSTRAINT
 [FkCategoryFactoringCustomer] FOREIGN KEY
([FactoringCustomerId]) REFERENCES [dbo].[Customer] ([Id])
GO

INSERT INTO GeneralSettingBase
SELECT *
  FROM (
        SELECT 'ファクタリング請求区分コード' AS Code
             , '' AS Value
             , 2 AS [Length]
             , 0 AS [Precision]
             , 'ファクタリングで自動生成される債権の請求区分コード' AS [Description]
        ) tbl
 WHERE NOT EXISTS(
                  SELECT *
                    FROM GeneralSettingBase b
                   WHERE b.Code = tbl.Code
                 )
GO

INSERT INTO GeneralSetting
SELECT *
  FROM (
        SELECT c.Id AS CompanyId
             , 'ファクタリング請求区分コード' AS Code
             , '' AS Value
             , 2 AS [Length]
             , 0 AS [Precision]
             , 'ファクタリングで自動生成される債権の請求区分コード' AS [Description]
             , MIN(u.Id) AS CreateBy
             , GETDATE() AS CreateAt
             , MIN(u.Id) AS UpdateBy
             , GETDATE() AS UpdateAt
          FROM Company c
         INNER JOIN LoginUser u
            ON u.CompanyId = c.Id
         GROUP BY c.Id
        ) tbl
 WHERE NOT EXISTS(
                  SELECT *
                    FROM GeneralSetting g
                   WHERE g.CompanyId = tbl.CompanyId
                     AND g.Code      = tbl.Code
                 )
GO

INSERT INTO GeneralSettingBase
SELECT *
  FROM (
        SELECT 'ファクタリング債権科目コード' AS Code
             , '' AS Value
             , 10 AS [Length]
             , 0 AS [Precision]
             , 'ファクタリングで自動生成される債権の科目コード' AS [Description]
        ) tbl
 WHERE NOT EXISTS(
                  SELECT *
                    FROM GeneralSettingBase b
                   WHERE b.Code = tbl.Code
                 )
GO

INSERT INTO GeneralSetting
SELECT *
  FROM (
        SELECT c.Id AS CompanyId
             , 'ファクタリング債権科目コード' AS Code
             , '' AS Value
             , 10 AS [Length]
             , 0 AS [Precision]
             , 'ファクタリングで自動生成される債権の科目コード' AS [Description]
             , MIN(u.Id) AS CreateBy
             , GETDATE() AS CreateAt
             , MIN(u.Id) AS UpdateBy
             , GETDATE() AS UpdateAt
          FROM Company c
         INNER JOIN LoginUser u
            ON u.CompanyId = c.Id
         GROUP BY c.Id
        ) tbl
 WHERE NOT EXISTS(
                  SELECT *
                    FROM GeneralSetting g
                   WHERE g.CompanyId = tbl.CompanyId
                     AND g.Code      = tbl.Code
                 )
GO

--ファクタリング入金＆請求データの紐づけ管理テーブル追加
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[BillingFactoring]'))
CREATE TABLE [dbo].[BillingFactoring](
    [BillingId] [bigint] NOT NULL,
    [ReceiptId] [bigint] NOT NULL,
    [CreateBy] [int] NOT NULL,
    [CreateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkBillingFactoring] PRIMARY KEY CLUSTERED 
(
    [BillingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkBillingFactoringBilling]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[BillingFactoring]')
       )
ALTER TABLE [dbo].[BillingFactoring] ADD CONSTRAINT
 [FkBillingFactoringBilling] FOREIGN KEY
([BillingId]) REFERENCES [dbo].[Billing] ([Id]) ON DELETE CASCADE
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkBillingFactoringReceipt]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[BillingFactoring]')
       )
ALTER TABLE [dbo].[BillingFactoring] ADD CONSTRAINT
 [FkBillingFactoringReceipt] FOREIGN KEY
([ReceiptId]) REFERENCES [dbo].[Receipt] ([Id]) ON DELETE CASCADE
GO

--口座振替フォーマット「しんきん情報サービス」を追加
INSERT INTO [dbo].[PaymentFileFormat]
     ( Id
     , Name
     , DisplayOrder
     , Available
     , IsNeedYear)
SELECT u.Id
     , u.Name
     , u.DisplayOrder
     , u.Available
     , u.IsNeedYear
  FROM (
            SELECT  1 [Id], N'全銀（口座振替 カンマ区切り）'   [Name],  1 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  2 [Id], N'全銀（口座振替 固定長）'         [Name],  2 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  3 [Id], N'みずほファクター（Web伝送）'     [Name],  3 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  4 [Id], N'三菱UFJファクター'               [Name],  5 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  5 [Id], N'SMBC（口座振替 固定長）'         [Name],  7 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  6 [Id], N'三菱UFJニコス'                   [Name],  6 [DisplayOrder], 1 [Available], 0 [IsNeedYear]
 UNION ALL  SELECT  7 [Id], N'みずほファクター（ASPサービス）' [Name],  4 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT  8 [Id], N'リコーリースコレクト！'          [Name],  8 [DisplayOrder], 1 [Available], 0 [IsNeedYear]
 UNION ALL  SELECT  9 [Id], N'インターネット伝送ゆうちょ形式'  [Name],  9 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT 10 [Id], N'りそなネット'                    [Name], 10 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
 UNION ALL  SELECT 11 [Id], N'しんきん情報サービス'            [Name], 11 [DisplayOrder], 1 [Available], 0 [IsNeedYear]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[PaymentFileFormat] t
        WHERE t.Id      = u.Id )
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
