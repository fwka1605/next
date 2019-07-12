--USE [VOneG4]
--GO

BEGIN TRANSACTION
GO

/* タイムスケジューラ設定 インポート方法 カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'TaskSchedule', N'COLUMN',N'ImportMode'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'ImportMode'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[TaskSchedule]')
 and name = N'ImportMode' )
ALTER TABLE [dbo].[TaskSchedule] ADD
 [ImportMode]     INT                NOT NULL DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'TaskSchedule', N'COLUMN',N'ImportMode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'インポート方法'    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'ImportMode'
GO

/* タイムスケジューラー 得意先マスターインポート既存データのインポート方法更新 ImportSubType → ImportMode*/
UPDATE d
  SET d.[ImportSubType] = 1
     ,d.[ImportMode]    = COALESCE((CASE WHEN d.[ImportMode] = 0 THEN d.[ImportSubType] END),d.[ImportMode])
FROM [dbo].[TaskSchedule] d
  WHERE d.[ImportType]  = 0

/* タイムスケジューラ実行履歴 得意先マスターインポート既存データの取込パターン更新 ImportSubType*/
UPDATE d
  SET d.[ImportSubType] = 1
FROM [dbo].[TaskScheduleHistory] d
  WHERE d.[ImportType] = 0
    AND NOT EXISTS (SELECT 1 FROM [dbo].[TaskSchedule] t WHERE t.[ImportType] = 0 AND t.[ImportSubType] > 1)
GO

--口座振替オプション化対応
IF NOT EXISTS (SELECT * FROM sys.columns
WHERE object_id = object_id(N'[dbo].[ApplicationControl]')
AND name = N'UseAccountTransfer')
ALTER TABLE [dbo].[ApplicationControl]
ADD [UseAccountTransfer]   INT NOT NULL CONSTRAINT DfApplicationControlUseAccountTransfer DEFAULT 0;
GO

/* 入金テーブルスキーマ変更 被振込口座情報を ReceiptHeader から Receipt へ移動 */
IF EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = object_id(N'[dbo].[ReceiptHeader]') AND c.Name = N'BankCode')
BEGIN
    ALTER TABLE [dbo].[Receipt] ADD
     [BankCode]         NVARCHAR(4)         NOT NULL CONSTRAINT DfReceiptBankCode       DEFAULT N''
    ,[BankName]         NVARCHAR(30)        NOT NULL CONSTRAINT DfReceiptBankName       DEFAULT N''
    ,[BranchCode]       NVARCHAR(3)         NOT NULL CONSTRAINT DfReceiptBranchCode     DEFAULT N''
    ,[BranchName]       NVARCHAR(30)        NOT NULL CONSTRAINT DfReceiptBranchName     DEFAULT N''
    ,[AccountTypeId]    INT                     NULL
    ,[AccountNumber]    NVARCHAR(7)         NOT NULL CONSTRAINT DfReceiptAccountNumber  DEFAULT N''
    ,[AccountName]      NVARCHAR(140)       NOT NULL CONSTRAINT DfReceiptAccountName    DEFAULT N''

    ALTER TABLE [dbo].[AdvanceReceivedBackup] ADD
     [CollationKey]     NVARCHAR(48)        NOT NULL CONSTRAINT DfAdvanceReceivedBackupCollationKey     DEFAULT N''
    ,[BankCode]         NVARCHAR(4)         NOT NULL CONSTRAINT DfAdvanceReceivedBackupBankCode         DEFAULT N''
    ,[BankName]         NVARCHAR(30)        NOT NULL CONSTRAINT DfAdvanceReceivedBackupBankName         DEFAULT N''
    ,[BranchCode]       NVARCHAR(3)         NOT NULL CONSTRAINT DfAdvanceReceivedBackupBranchCode       DEFAULT N''
    ,[BranchName]       NVARCHAR(30)        NOT NULL CONSTRAINT DfAdvanceReceivedBackupBranchName       DEFAULT N''
    ,[AccountTypeId]    INT                     NULL
    ,[AccountNumber]    NVARCHAR(7)         NOT NULL CONSTRAINT DfAdvanceReceivedBackupAccountNumber    DEFAULT N''
    ,[AccountName]      NVARCHAR(140)       NOT NULL CONSTRAINT DfAdvanceReceivedBackupAccountName      DEFAULT N''

    DECLARE @v sql_variant
    SET @v = N'照合キー'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup' , @level2type=N'COLUMN',@level2name=N'CollationKey'
    SET @v = N'銀行コード'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt'               , @level2type=N'COLUMN',@level2name=N'BankCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup' , @level2type=N'COLUMN',@level2name=N'BankCode'
    SET @v = N'銀行名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt'               , @level2type=N'COLUMN',@level2name=N'BankName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup' , @level2type=N'COLUMN',@level2name=N'BankName'
    SET @v = N'支店コード'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt'               , @level2type=N'COLUMN',@level2name=N'BranchCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup' , @level2type=N'COLUMN',@level2name=N'BranchCode'
    SET @v = N'支店名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt'               , @level2type=N'COLUMN',@level2name=N'BranchName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup' , @level2type=N'COLUMN',@level2name=N'BranchName'
    SET @v = N'預金種別'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt'               , @level2type=N'COLUMN',@level2name=N'AccountTypeId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup' , @level2type=N'COLUMN',@level2name=N'AccountTypeId'
    SET @v = N'口座番号'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt'               , @level2type=N'COLUMN',@level2name=N'AccountNumber'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup' , @level2type=N'COLUMN',@level2name=N'AccountNumber'
    SET @v = N'口座名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt'               , @level2type=N'COLUMN',@level2name=N'AccountName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v, @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup' , @level2type=N'COLUMN',@level2name=N'AccountName'

    DECLARE @query NVARCHAR(2048);
    SET @query = N'
    UPDATE r
       SET r.[BankCode]         = rh.[BankCode]
         , r.[BankName]         = rh.[BankName]
         , r.[BranchCode]       = rh.[BranchCode]
         , r.[BranchName]       = rh.[BranchName]
         , r.[AccountTypeId]    = rh.[AccountTypeId]
         , r.[AccountNumber]    = rh.[AccountNumber]
         , r.[AccountName]      = rh.[AccountName]
      FROM [dbo].[Receipt] r
     INNER JOIN [dbo].[ReceiptHeader] rh        ON rh.[Id]  = r.[ReceiptHeaderId]'
    EXEC sp_executesql @query;
    SET @query = N'
    UPDATE ar
       SET ar.[BankCode]        = rh.[BankCode]
         , ar.[BankName]        = rh.[BankName]
         , ar.[BranchCode]      = rh.[BranchCode]
         , ar.[BranchName]      = rh.[BranchName]
         , ar.[AccountTypeId]   = rh.[AccountTypeId]
         , ar.[AccountNumber]   = rh.[AccountNumber]
         , ar.[AccountName]     = rh.[AccountName]
      FROM [dbo].[AdvanceReceivedBackup] ar
     INNER JOIN [dbo].[ReceiptHeader] rh        ON rh.[Id]  = ar.[ReceiptHeaderId]'
    EXEC sp_executesql @query;

    ALTER TABLE [dbo].[ReceiptHeader] DROP COLUMN
      [WorkDay]
     ,[BankCode]
     ,[BankName]
     ,[BranchCode]
     ,[BranchName]
     ,[AccountTypeId]
     ,[AccountNumber]
     ,[AccountName]
END
GO

/* 入金フリーインポーター 銀行情報の追加 */
IF EXISTS (
    SELECT  *
    FROM    [dbo].[ImporterSettingBase] b
    WHERE   b.[FormatId]        = 2
    AND     b.[Sequence]        = 13
    AND     b.[TargetColumn]    = N'SourceBankName' )
BEGIN
    UPDATE d
       SET d.[Sequence] = d.[Sequence] + 7
      FROM [dbo].[ImporterSettingDetail] d
     INNER JOIN [dbo].[ImporterSetting] s       ON s.[Id]       = d.[ImporterSettingId]
     INNER JOIN [dbo].[ImporterSettingBase] b   ON s.[FormatId] = b.[FormatId]              AND d.[Sequence] = b.[Sequence]
     WHERE b.[FormatId]  = 2
       AND b.[Sequence] >= 13

    UPDATE b
       SET b.[Sequence] = b.[Sequence] + 7
      FROM [dbo].[ImporterSettingBase] b
     WHERE b.[FormatId]  = 2
       AND b.[Sequence] >= 13

    INSERT INTO [dbo].[ImporterSettingBase]
    SELECT 2 [FormatId]
         , u.[seq] [Sequence]
         , u.[fn]  [FieldName]
         , u.[tc]  [TargetColumn]
         , 11 [ImportDivision]
         , CASE WHEN u.[seq] IN (13, 15, 17, 18) THEN 0 ELSE 3 END [AttributeDivision]
      FROM (
                SELECT 13 [seq], N'銀行コード' [fn], N'BankCode'      [tc]
    UNION ALL   SELECT 14 [seq], N'銀行名'     [fn], N'BankName'      [tc]
    UNION ALL   SELECT 15 [seq], N'支店コード' [fn], N'BranchCode'    [tc]
    UNION ALL   SELECT 16 [seq], N'支店名'     [fn], N'BranchName'    [tc]
    UNION ALL   SELECT 17 [seq], N'預金種別'   [fn], N'AccountTypeId' [tc]
    UNION ALL   SELECT 18 [seq], N'口座番号'   [fn], N'AccountNumber' [tc]
    UNION ALL   SELECT 19 [seq], N'口座名'     [fn], N'AccountName'   [tc]
     ) u

    INSERT INTO [dbo].[ImporterSettingDetail]
    SELECT s.[Id] [ImporterSettingId]
         , b.[Sequence]
         , 0 [ImportDivision]
         , 0 [FieldIndex]
         , N'' [Caption]
         , 0 [AttributeDivision]
         , 0 [ItemPriority]
         , 0 [DoOverwrite]
         , 0 [IsUnique]
         , N'' [FixedValue]
         , 0 [UpdateKey]
         , 0 [CreateBy]
         , GETDATE() [CreateAt]
         , 0 [UpdateBy]
         , GETDATE() [UpdateAt]
      FROM [dbo].[ImporterSetting] s
     INNER JOIN [dbo].[ImporterSettingBase] b   ON b.[FormatId] = s.[FormatId]  AND s.[FormatId] = 2
     WHERE NOT EXISTS (
           SELECT 1
             FROM [dbo].[ImporterSettingDetail] d
            WHERE d.[ImporterSettingId]     = s.[Id]
              AND d.[Sequence]              = b.[Sequence] )

    DROP TYPE [dbo].[ReceiptImportDuplication];
    CREATE TYPE [dbo].[ReceiptImportDuplication] AS TABLE
    (
        [RowNumber]             int             NOT NULL
      , [CustomerId]            int                 NULL
      , [RecordedAt]            date                NULL
      , [ReceiptCategoryId]     int                 NULL
      , [ReceiptAmount]         numeric(18,5)       NULL
      , [DueAt]                 date                NULL
      , [Note1]                 nvarchar(100)       NULL
      , [SectionId]             int                 NULL
      , [CurrencyId]            int                 NULL
      , [Note2]                 nvarchar(100)       NULL
      , [Note3]                 nvarchar(100)       NULL
      , [Note4]                 nvarchar(100)       NULL
      , [PayerName]             nvarchar(140)       NULL
      , [BankCode]              nvarchar(4)         NULL
      , [BankName]              nvarchar(30)        NULL
      , [BranchCode]            nvarchar(3)         NULL
      , [BranchName]            nvarchar(30)        NULL
      , [AccountTypeId]         int                 NULL
      , [AccountNumber]         nvarchar(7)         NULL
      , [AccountName]           nvarchar(140)       NULL
      , [SourceBankName]        nvarchar(140)       NULL
      , [SourceBranchName]      nvarchar(15)        NULL
      , [BillNumber]            nvarchar(20)        NULL
      , [BillBankCode]          nvarchar(4)         NULL
      , [BillBranchCode]        nvarchar(3)         NULL
      , [BillDrawAt]            date                NULL
      , [BillDrawer]            nvarchar(48)        NULL
    ,   PRIMARY KEY ([RowNumber])
    );

END
GO

/* EBフォーマット 新規追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[EBFormat]'))
BEGIN
    CREATE TABLE [dbo].[EBFormat]
    ([Id]                       INT                 NOT NULL
    ,[Name]                     NVARCHAR(100)       NOT NULL
    ,[DisplayOrder]             INT                 NOT NULL
    ,[RequireBankCode]          INT                 NOT NULL
    ,[RequireYear]              INT                 NOT NULL
    ,[IsDateSelectable]         INT                 NOT NULL
    ,[FileFieldTypes]           INT                 NOT NULL
    ,[ImportableValues]         NVARCHAR(100)       NOT NULL
    ,CONSTRAINT [PkEBFormat] PRIMARY KEY CLUSTERED
    ([Id]                       ASC )
    );

    DECLARE @v sql_variant
    SET @v = N'EBフォーマットID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFormat', @level2type=N'COLUMN',@level2name=N'Id'
    SET @v = N'EBフォーマット名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFormat', @level2type=N'COLUMN',@level2name=N'Name'
    SET @v = N'表示順'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFormat', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
    SET @v = N'銀行コード必須'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFormat', @level2type=N'COLUMN',@level2name=N'RequireBankCode'
    SET @v = N'年情報必須'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFormat', @level2type=N'COLUMN',@level2name=N'RequireYear'
    SET @v = N'入金日 選択可能'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFormat', @level2type=N'COLUMN',@level2name=N'IsDateSelectable'
    SET @v = N'ファイルフィールド形式 の 取り得る値'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFormat', @level2type=N'COLUMN',@level2name=N'FileFieldTypes'
    SET @v = N'取込可能文字列'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFormat', @level2type=N'COLUMN',@level2name=N'ImportableValues'
END
GO

INSERT INTO [dbo].[EBFormat]
SELECT u.[id], u.[name], u.[dispOrder], u.[rqBank], u.[rqYear], u.[slDate], u.[types], u.[values]
  FROM (
            /*     Id, DisplayOrder, RequireBankCode, RequireYear, SelectableDate, FileFieldTypes(flag), ImportableValues,  Name */
            SELECT  0 [id],  1 [dispOrder], 0 [rqBank], 0 [rqYear], 1 [slDate], 15 [types], N''                   [values], N'全銀 振込入金明細'           [name]
UNION ALL   SELECT  1 [id],  2 [dispOrder], 0 [rqBank], 0 [rqYear], 1 [slDate], 15 [types], N'11'                 [values], N'全銀 入出金明細'             [name]
UNION ALL   SELECT  2 [id],  3 [dispOrder], 0 [rqBank], 1 [rqYear], 1 [slDate],  3 [types], N'振込,振込入金,入金' [values], N'ANSER'                       [name]
UNION ALL   SELECT  3 [id],  4 [dispOrder], 1 [rqBank], 0 [rqYear], 0 [slDate],  1 [types], N''                   [values], N'BizSTATION 全明細'           [name]
UNION ALL   SELECT  4 [id],  5 [dispOrder], 1 [rqBank], 0 [rqYear], 1 [slDate],  1 [types], N''                   [values], N'BizSTATION 振込入金明細'     [name]
UNION ALL   SELECT  5 [id],  6 [dispOrder], 0 [rqBank], 0 [rqYear], 0 [slDate],  4 [types], N''                   [values], N'ゆうちょ 受入明細通知'       [name]
UNION ALL   SELECT  6 [id],  7 [dispOrder], 1 [rqBank], 0 [rqYear], 1 [slDate],  1 [types], N'振込,振込入金'      [values], N'地方銀行系 入出金明細'       [name]
UNION ALL   SELECT  7 [id],  8 [dispOrder], 0 [rqBank], 1 [rqYear], 0 [slDate],  1 [types], N'1'                  [values], N'マネーシューター 入出金明細' [name]
UNION ALL   SELECT  8 [id],  9 [dispOrder], 0 [rqBank], 0 [rqYear], 0 [slDate],  1 [types], N''                   [values], N'U-LINE Xtra Ver.2'           [name]
UNION ALL   SELECT  9 [id], 10 [dispOrder], 0 [rqBank], 0 [rqYear], 0 [slDate],  1 [types], N''                   [values], N'北日本銀行BizNet'            [name]
UNION ALL   SELECT 10 [id], 11 [dispOrder], 0 [rqBank], 0 [rqYear], 0 [slDate],  1 [types], N''                   [values], N'第四銀行 入出金明細'         [name]
UNION ALL   SELECT 11 [id], 12 [dispOrder], 1 [rqBank], 0 [rqYear], 0 [slDate],  1 [types], N''                   [values], N'常陽銀行 振込入金明細'       [name]
) u
WHERE NOT EXISTS (
      SELECT 1 FROM [dbo].[EBFormat] ef
       WHERE ef.[Id]    = u.[id] )
GO


/* EBファイル設定 新規追加 既存 テーブル削除 新規テーブル追加*/
IF EXISTS (
    SELECT 1
      FROM sys.columns c
     WHERE c.object_id  = object_id(N'[dbo].[EBFileSetting]')
       AND c.name       = N'LoginUserId' )
BEGIN
    DROP TABLE [dbo].[EBFileSetting];
END
GO

IF NOT EXISTS (
    SELECT 1
      FROM sys.tables t
     WHERE t.object_id  = object_id(N'[dbo].[EBFileSetting]') )
BEGIN
    CREATE TABLE [dbo].[EBFileSetting]
    ([Id]                       INT                 NOT NULL   IDENTITY(1, 1)
    ,[CompanyId]                INT                 NOT NULL
    ,[Name]                     NVARCHAR(100)       NOT NULL
    ,[DisplayOrder]             INT                 NOT NULL
    ,[IsUseable]                INT                 NOT NULL
    ,[EBFormatId]               INT                 NOT NULL
    ,[FileFieldType]            INT                 NOT NULL
    ,[BankCode]                 NVARCHAR(4)         NOT NULL
    ,[UseValueDate]             INT                 NOT NULL
    ,[ImportableValues]         NVARCHAR(100)       NOT NULL
    ,[FilePath]                 NVARCHAR(255)       NOT NULL
    ,[CreateBy]                 INT                 NOT NULL
    ,[CreateAt]                 DATETIME2(3)        NOT NULL
    ,[UpdateBy]                 INT                 NOT NULL
    ,[UpdateAt]                 DATETIME2(3)        NOT NULL
    ,CONSTRAINT [PkEBFileSetting] PRIMARY KEY NONCLUSTERED
    ([Id]                       ASC )
    );

    DECLARE @v sql_variant
    SET @v = N'EBファイル設定ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'Id'
    SET @v = N'会社ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
    SET @v = N'EBファイル設定名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'Name'
    SET @v = N'表示順'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
    SET @v = N'利用可否'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'IsUseable'
    SET @v = N'EBフォーマットID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'FileFieldType'
    SET @v = N'ファイルフィールド形式'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'BankCode'
    SET @v = N'銀行コード'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'UseValueDate'
    SET @v = N'起算日を入金日とする'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'ImportableValues'
    SET @v = N'取込可能文字列'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'FilePath'
    SET @v = N'登録者ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
    SET @v = N'登録日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
    SET @v = N'更新者ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
    SET @v = N'更新日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[EBFileSetting])
INSERT INTO [dbo].[EBFileSetting]
SELECT c.Id [CompanyId]
     , f.[Name] + N' '
     + CASE f.[FileFieldTypes]
            WHEN  4 THEN N'固定長'
                    ELSE N'カンマ区切' END [Name]
     , f.[DisplayOrder]
     , 1 [IsUseable]
     , f.[Id] [EBFormatId]
     , CASE f.[FileFieldTypes] WHEN 4 THEN 3 ELSE 1 END [FileFieldType]
     , CASE WHEN f.[Id] IN (3, 4)   THEN N'0005'
                                    ELSE N'' END [BankCode]
     , 0 [UseValueDate]
     , f.[ImportableValues]
     , N'' [FilePath]
     , 0 [CreateBy]
     , GETDATE() [CreateAt]
     , 0 [UpdateBy]
     , GETDATE() [UpdateAt]
  FROM [dbo].[EBFormat] f
 INNER JOIN [dbo].[Company] c
    ON  c.Id    = c.Id
   AND NOT f.[Id]   IN (6, 11)

IF EXISTS (
    SELECT 1
      FROM sys.columns c
     WHERE c.object_id  = object_id(N'[dbo].[BankAccount]')
       AND c.name       = N'UseValueDate' )
BEGIN
    ALTER TABLE [dbo].[BankAccount]
    DROP COLUMN [UseValueDate];
END
GO


/* 請求フリーインポーターの取込パターンの消費税項目に属性情報を追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[Setting] d
     WHERE d.[ItemId]     = N'TRKM3'
       AND d.[ItemKey]    = N'2' )
BEGIN
    INSERT INTO [dbo].[Setting]
     ( [ItemId]
     , [ItemKey]
     , [ItemValue] )
    SELECT u.[iItemId]       [ItemId]
         , u.[iItemKey]      [ItemKey]
         , u.[iItemValue]    [ItemValue]
    FROM (
            SELECT N'TRKM3'    [iItemId], N'2'    [iItemKey], N'2：取込有'    [iItemValue]
         ) u
     WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Setting] s
       WHERE s.[ItemId]     = u.[iItemId] AND s.[ItemKey] = u.[iItemKey] )

/* 既存属性名称変更 1：取込有 ⇒ 1：取込無(消費税率より計算) */
UPDATE d
  SET d.[ItemValue] = N'1：取込無(消費税率より計算)'
FROM [dbo].[Setting] d
  WHERE d.[ItemId]     = N'TRKM3'
    AND d.[ItemKey]    = N'1'

END
GO

/* 汎用設定項目定義 消費税率用属性追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[SettingDefinition] d
     WHERE d.[ItemId]     = N'ATTR7' )
BEGIN
    UPDATE d
      SET d.[ImportDivision] = 2
    FROM [dbo].[ImporterSettingDetail] d
    WHERE d.[Sequence] = 5
      AND d.[ImporterSettingId] IN
     (
      SELECT d.[ImporterSettingId]
      FROM [dbo].[ImporterSettingDetail] d
      INNER JOIN [dbo].[ImporterSetting] s ON s.[Id] = d.[ImporterSettingId]
      INNER JOIN [dbo].[ImporterSettingBase] b   ON s.[FormatId] = b.[FormatId]    AND d.[Sequence] = b.[Sequence]
      WHERE b.[FormatId] = 1
        AND d.[Sequence] = 4
        AND d.[ImportDivision] IN (0,3,4)
     )

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
            SELECT N'ATTR7'    [iItemId], N'属性区分消費税率'    [iDescription], 1    [iItemKeyLength], 40    [iItemValueLength]
         ) u
     WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[SettingDefinition] s
       WHERE s.[ItemId]     = u.[iItemId] )

END
GO

/* 請求フリーインポーターの取込パターンに消費税率を追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[Setting] d
     WHERE d.[ItemId]     = N'ATTR7')
BEGIN
    INSERT INTO [dbo].[Setting]
     ( [ItemId]
     , [ItemKey]
     , [ItemValue] )
    SELECT u.[ItemId]
         , u.[ItemKey]
         , u.[ItemValue]
    FROM (
          SELECT N'ATTR7'    [ItemId], N'0'    [ItemKey], N''                                [ItemValue]
UNION ALL SELECT N'ATTR7'    [ItemId], N'1'    [ItemKey], N'1：小数点形式 例）0.08、0.10'    [ItemValue]
UNION ALL SELECT N'ATTR7'    [ItemId], N'2'    [ItemKey], N'2：ﾊﾟｰｾﾝﾄ形式 例）8、8%、8.0%'   [ItemValue]
         ) u
     WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Setting] s
       WHERE s.[ItemId]     = u.[ItemId] )

END
GO

/* フリーインポーターベースデータ 消費税率　項目追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[ImporterSettingBase] b
     WHERE b.[FormatId]     = 1
       AND b.[Sequence]     = 38
       AND b.[FieldName]    = N'消費税率' )
BEGIN
    INSERT INTO [dbo].[ImporterSettingBase]
    SELECT 1 [FormatId]
         , u.[seq] [Sequence]
         , u.[fn]  [FieldName]
         , u.[tc]  [TargetColumn]
         , 1 [ImportDivision]
         , 7 [AttributeDivision]
      FROM (
                SELECT 38 [seq], N'消費税率' [fn], N'TaxRate' [tc]
           ) u

    INSERT INTO [dbo].[ImporterSettingDetail]
    SELECT s.[Id] [ImporterSettingId]
         , b.[Sequence]
         , 0 [ImportDivision]
         , 0 [FieldIndex]
         , N'' [Caption]
         , 0 [AttributeDivision]
         , 0 [ItemPriority]
         , 0 [DoOverwrite]
         , 0 [IsUnique]
         , N'' [FixedValue]
         , 0 [UpdateKey]
         , 0 [CreateBy]
         , GETDATE() [CreateAt]
         , 0 [UpdateBy]
         , GETDATE() [UpdateAt]
      FROM [dbo].[ImporterSetting] s
     INNER JOIN [dbo].[ImporterSettingBase] b   ON b.[FormatId] = s.[FormatId]  AND s.[FormatId] = 1
     WHERE NOT EXISTS (
           SELECT 1
             FROM [dbo].[ImporterSettingDetail] d
            WHERE d.[ImporterSettingId]     = s.[Id]
              AND d.[Sequence]              = b.[Sequence] )

END
GO

/* 帳票設定ベース 債権総額管理表 帳票設定追加 */
INSERT INTO [dbo].[ReportSettingBase]
     ( [ReportId]
     , [DisplayOrder]
     , [ItemId]
     , [ItemKey]
     , [Alias]
     , [IsText] )
SELECT u.[rid]       [ReportId]
     , u.[dorder]    [DisplayOrder]
     , u.[iid]       [ItemId]
     , u.[ikey]      [ItemKey]
     , u.[alias]     [Alias]
     , u.[istxt]     [IsText]
  FROM (
            SELECT N'PF0201'    [rid], 9    [dorder], N'ReportCreditLimitType'    [iid], N'0'    [ikey], N'与信限度額集計方法'    [alias], 0    [istxt]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[ReportSettingBase] rsb
        WHERE rsb.[ReportId]     = u.[rid]
          AND rsb.[DisplayOrder] = u.[dorder] )
GO

INSERT INTO [dbo].[ReportSetting]
     ( [CompanyId]
     , [ReportId]
     , [DisplayOrder]
     , [ItemId]
     , [ItemKey])
SELECT cm.[Id]             [CompanyId]
     , rsb.ReportId        [ReportId]
     , rsb.DisplayOrder    [DisplayOrder]
     , rsb.ItemId          [ItemId]
     , rsb.ItemKey         [ItemKey]
FROM ReportSettingBase rsb
  INNER JOIN [dbo].[Company] cm
      ON cm.Id = cm.Id
    AND NOT EXISTS (
       SELECT 1
         FROM [dbo].[ReportSetting] rs
        WHERE rs.CompanyId     = cm.Id
          AND rs.ReportId      = rsb.ReportId
          AND rs.DisplayOrder  = rsb.DisplayOrder)
GO

/* 汎用設定項目定義 消費税率用属性追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[SettingDefinition] d
     WHERE d.[ItemId]     = N'ReportCreditLimitType' )
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
            SELECT N'ReportCreditLimitType'    [iItemId], N'与信限度額集計方法'    [iDescription], 1    [iItemKeyLength], 40    [iItemValueLength]
         ) u
     WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[SettingDefinition] s
       WHERE s.[ItemId]     = u.[iItemId] )

END
GO

/* 債権総額管理表 帳票設定に属性情報を追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[Setting] d
     WHERE d.[ItemId]     = N'ReportCreditLimitType' )
BEGIN
    INSERT INTO [dbo].[Setting]
     ( [ItemId]
     , [ItemKey]
     , [ItemValue] )
    SELECT u.[iItemId]       [ItemId]
         , u.[iItemKey]      [ItemKey]
         , u.[iItemValue]    [ItemValue]
    FROM (
            SELECT N'ReportCreditLimitType'    [iItemId], N'0'    [iItemKey], N'得意先の与信限度額を集計する'            [iItemValue]
 UNION ALL  SELECT N'ReportCreditLimitType'    [iItemId], N'1'    [iItemKey], N'債権代表者の与信限度額を使用する'        [iItemValue]
         ) u
     WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Setting] s
       WHERE s.[ItemId]     = u.[iItemId] )

END
GO

/* 管理マスターベース 有効桁数更新 */
BEGIN
    UPDATE gsb
      SET gsb.[Length] = 10
    FROM [dbo].[GeneralSettingBase] gsb
    WHERE gsb.[Code] IN(
      SELECT u.[iCode]
      FROM (
          SELECT N'借方消費税誤差科目コード'    [iCode], 10    [iLength]
UNION ALL SELECT N'振込手数料科目コード'        [iCode], 10    [iLength]
UNION ALL SELECT N'貸方消費税誤差科目コード'    [iCode], 10    [iLength]
UNION ALL SELECT N'仮受補助コード'              [iCode], 10    [iLength]
UNION ALL SELECT N'振込手数料補助コード'        [iCode], 10    [iLength]
           ) u
     WHERE EXISTS (
       SELECT 1
         FROM [dbo].[GeneralSettingBase] gb
       WHERE gb.[Code]     IN (u.[iCode]) AND gb.[Length] <> u.[iLength]))

/* 管理マスター 有効桁数更新 */
    UPDATE gs
      SET gs.[Length] = 10
    FROM [dbo].[GeneralSetting] gs
    WHERE gs.[Code] IN(
      SELECT u.[iCode]
      FROM (
          SELECT N'借方消費税誤差科目コード'    [iCode], 10    [iLength]
UNION ALL SELECT N'振込手数料科目コード'        [iCode], 10    [iLength]
UNION ALL SELECT N'貸方消費税誤差科目コード'    [iCode], 10    [iLength]
UNION ALL SELECT N'仮受補助コード'              [iCode], 10    [iLength]
UNION ALL SELECT N'振込手数料補助コード'        [iCode], 10    [iLength]
           ) u
     WHERE EXISTS (
       SELECT 1
         FROM [dbo].[GeneralSetting] gs
       WHERE gs.[Code]     IN (u.[iCode]) AND gs.[Length] <> u.[iLength]))

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
            SELECT N'PH1301' [Id], N'PH' [Category], 6130 [Sequence], N'EBファイル設定一覧'       [Name]
 --UNION ALL  SELECT N'PX9999' [Id], N'PX' [Category], 9999 [Sequence], N''                 [Name]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Menu] m
        WHERE m.MenuId      = u.Id )
 ORDER BY
       u.Id;
GO

/* 新規口座振替フォーマット追加 */
INSERT INTO [dbo].[PaymentFileFormat]
     ( [Id]
      ,[Name]
      ,[DisplayOrder]
      ,[Available]
      ,[IsNeedYear] )
SELECT * FROM (
SELECT 9 [Id], N'インターネット伝送ゆうちょ形式' [Name],  9 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
) f
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[PaymentFileFormat] p
        WHERE p.[Id] = 9)
GO

INSERT INTO [dbo].[PaymentFileFormat]
     ( [Id]
      ,[Name]
      ,[DisplayOrder]
      ,[Available]
      ,[IsNeedYear] )
SELECT * FROM (
SELECT 10 [Id], N'りそなネット'                   [Name], 10 [DisplayOrder], 1 [Available], 1 [IsNeedYear]
) f
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[PaymentFileFormat] p
        WHERE p.[Id] = 10 )
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[PaymentAgency]')
 and name = N'ContractCode' )
ALTER TABLE [dbo].[PaymentAgency] ADD
 [ContractCode]     nvarchar(2)                NOT NULL DEFAULT ''
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[AccountTransferDetail]')
 and name = N'DueAt2nd' )
ALTER TABLE [dbo].[AccountTransferDetail] ADD
 [DueAt2nd]     date                NULL
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[CollationSetting]')
 and name = N'UseFromToNarrowing' )
ALTER TABLE [dbo].[CollationSetting] ADD
 [UseFromToNarrowing]    int NOT NULL DEFAULT 0
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
GO