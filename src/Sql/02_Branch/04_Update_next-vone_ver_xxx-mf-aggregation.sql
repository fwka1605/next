/* 04_update_g4_ver_4.0.10 partial */

begin transaction

/*
 検討事項：
 マスター ( tag, account, sub_account ) への 会社ID 追加
*/

/* ApplicationControl MF明細連携 オプション項目追加 */
IF NOT EXISTS (SELECT * FROM sys.columns where object_id = object_id(N'[dbo].[ApplicationControl]') and name = N'UseMfAggregation')
BEGIN
    ALTER TABLE [dbo].[ApplicationControl]
    ADD [UseMfAggregation]  INT         NOT NULL CONSTRAINT DfApplicationControlUseMfAggregation DEFAULT 0;

    DECLARE @v sql_variant
    SET @v = N'MF明細連携利用'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseMfAggregation'
END
GO


/* MF明細連携 タグテーブル追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[MfAggrTag]'))
BEGIN
    CREATE TABLE [dbo].[MfAggrTag]
    ( [Id]                      BIGINT          NOT NULL
    , [Name]                    NVARCHAR(20)    NOT NULL
    , CONSTRAINT [PkMfAggrTak] PRIMARY KEY CLUSTERED
    ( [Id]              ASC)
    );

    DECLARE @v sql_variant
    SET @v = N'MF明細連携タグID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTag', @level2type=N'COLUMN',@level2name=N'Id'
    SET @v = N'MF明細連携タグ名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTag', @level2type=N'COLUMN',@level2name=N'Name'
END
GO

/* MF明細連携 口座テーブル追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[MfAggrAccount]'))
BEGIN
    CREATE TABLE [dbo].[MfAggrAccount]
    ( [Id]                      BIGINT          NOT NULL
    , [DisplayName]             NVARCHAR(20)    NOT NULL
    , [LastAggregatedAt]        DATETIME2(3)    NOT NULL
    , [LastLoginAt]             DATETIME2(3)    NOT NULL
    , [LastSucceededAt]         DATETIME2(3)    NOT NULL
    , [AggregationStartDate]    DATE                NULL
    , [Status]                  INT             NOT NULL
    , [IsSuspended]             INT             NOT NULL
    , [BankCode]                NVARCHAR(4)     NOT NULL
    , CONSTRAINT [PkMfAggrAccount] PRIMARY KEY CLUSTERED
    ( [Id]              ASC)
    );

    DECLARE @v sql_variant
    SET @v = N'MF明細連携 口座ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrAccount', @level2type=N'COLUMN',@level2name=N'Id'
    SET @v = N'MF明細連携 口座名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrAccount', @level2type=N'COLUMN',@level2name=N'DisplayName'
    SET @v = N'MF明細連携 最終更新日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrAccount', @level2type=N'COLUMN',@level2name=N'LastAggregatedAt'
    SET @v = N'MF明細連携 最終ログイン日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrAccount', @level2type=N'COLUMN',@level2name=N'LastLoginAt'
    SET @v = N'MF明細連携 最終連携日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrAccount', @level2type=N'COLUMN',@level2name=N'LastSucceededAt'
    SET @v = N'MF明細連携 データ取得開始日'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrAccount', @level2type=N'COLUMN',@level2name=N'AggregationStartDate'
    SET @v = N'MF明細連携 連携状態 0: 成功, 1: 失敗'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrAccount', @level2type=N'COLUMN',@level2name=N'Status'
    SET @v = N'MF明細連携 0: 取得中, 1: 取得停止中'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrAccount', @level2type=N'COLUMN',@level2name=N'IsSuspended'
    SET @v = N'MF明細連携 銀行コード'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrAccount', @level2type=N'COLUMN',@level2name=N'BankCode'
END
GO


/* MF明細連携 サブアカウントテーブル追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[MfAggrSubAccount]'))
BEGIN
    CREATE TABLE [dbo].[MfAggrSubAccount]
    ( [Id]                      BIGINT          NOT NULL
    --, [CompanyId]               INT             NOT NULL
    , [AccountId]               BIGINT          NOT NULL
    , [Name]                    NVARCHAR(100)   NOT NULL
    , [AccountTypeName]         NVARCHAR(20)    NOT NULL
    , [AccountTypeId]           INT                 NULL
    , [AccountNumber]           NVARCHAR(7)     NOT NULL
    , [BranchCode]              NVARCHAR(3)     NOT NULL
    , [ReceiptCategoryId]       INT             NOT NULL
    , [SectionId]               INT                 NULL
    , CONSTRAINT [PkMfAggrSubAccount] PRIMARY KEY CLUSTERED
    ( [Id]              ASC)
    );

    DECLARE @v sql_variant
    SET @v = N'MF明細連携 サブアカウントID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrSubAccount', @level2type=N'COLUMN',@level2name=N'Id'
    SET @v = N'MF明細連携 口座ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrSubAccount', @level2type=N'COLUMN',@level2name=N'AccountId'
    SET @v = N'MF明細連携 サブアカウント名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrSubAccount', @level2type=N'COLUMN',@level2name=N'Name'
    SET @v = N'MF明細連携 預金種別名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrSubAccount', @level2type=N'COLUMN',@level2name=N'AccountTypeName'
    SET @v = N'MF明細連携 預金種別'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrSubAccount', @level2type=N'COLUMN',@level2name=N'AccountTypeId'
    SET @v = N'MF明細連携 口座番号'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrSubAccount', @level2type=N'COLUMN',@level2name=N'AccountNumber'
    SET @v = N'MF明細連携 支店コード'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrSubAccount', @level2type=N'COLUMN',@level2name=N'BranchCode'
    SET @v = N'MF明細連携 入金区分ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrSubAccount', @level2type=N'COLUMN',@level2name=N'ReceiptCategoryId'
    SET @v = N'MF明細連携 入金部門ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrSubAccount', @level2type=N'COLUMN',@level2name=N'SectionId'
END
GO

/* MF明細連携 タグ紐づけテーブル追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[MfAggrTagRel]'))
BEGIN
    CREATE TABLE [dbo].[MfAggrTagRel]
    ( [SubAccountId]            BIGINT          NOT NULL
    , [TagId]                   BIGINT          NOT NULL
    , CONSTRAINT [PkMfAggrTagRel] PRIMARY KEY CLUSTERED
    ( [SubAccountId]            ASC
    , [TagId]                   ASC)
    );

    DECLARE @v sql_variant
    SET @v = N'MF明細連携 サブアカウントID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTagRel', @level2type=N'COLUMN',@level2name=N'SubAccountId'
    SET @v = N'MF明細連携 タグID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTagRel', @level2type=N'COLUMN',@level2name=N'TagId'
END
GO

/* MF明細連携 明細テーブル追加 */
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[MfAggrTransaction]'))
BEGIN
    CREATE TABLE [dbo].[MfAggrTransaction]
    ( [Id]                      BIGINT          NOT NULL
    , [ReceiptId]               BIGINT              NULL
    , [CompanyId]               INT             NOT NULL
    , [CurrencyId]              INT             NOT NULL
    , [Amount]                  NUMERIC(18, 5)  NOT NULL
    , [AccountId]               BIGINT          NOT NULL
    , [SubAccountId]            BIGINT          NOT NULL
    , [Content]                 NVARCHAR(200)   NOT NULL
    , [PayerCode]               NVARCHAR(10)    NOT NULL
    , [PayerName]               NVARCHAR(140)   NOT NULL
    , [PayerNameRaw]            NVARCHAR(140)   NOT NULL
    , [RecordedAt]              DATE            NOT NULL
    , [MfCreatedAt]             DATETIME2(3)    NOT NULL
    , [Rate]                    NUMERIC(18, 5)  NOT NULL
    , [ConvertedAmount]         NUMERIC(18, 5)  NOT NULL
    , [ToCurrencyId]            INT             NOT NULL
    , [ExcludeCategoryId]       INT                 NULL
    , [CreateBy]                INT             NOT NULL
    , [CreateAt]                DATETIME2(3)    NOT NULL
    , CONSTRAINT [PkMfAggrTransaction] PRIMARY KEY CLUSTERED
    ( [Id]              ASC)
    , CONSTRAINT [FkMfAggrTransactionCompany] FOREIGN KEY ([CompanyId])
      REFERENCES [dbo].[Company] ([Id]) ON DELETE CASCADE
    , CONSTRAINT [FkMfAggrTransactionCurrency] FOREIGN KEY ([CurrencyId])
      REFERENCES [dbo].[Currency] ([Id]) ON DELETE CASCADE
    );

    DECLARE @v sql_variant
    SET @v = N'MF明細連携 明細ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'Id'
    SET @v = N'MF明細連携 入金ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'ReceiptId'
    SET @v = N'MF明細連携 会社ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'CompanyId'
    SET @v = N'MF明細連携 通貨ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'CurrencyId'
    SET @v = N'MF明細連携 金額'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'Amount'
    SET @v = N'MF明細連携 口座ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'AccountId'
    SET @v = N'MF明細連携 サブアカウントID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'SubAccountId'
    SET @v = N'MF明細連携 入出金履歴の内容'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'Content'
    SET @v = N'MF明細連携 振込依頼人番号'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'PayerCode'
    SET @v = N'MF明細連携 振込依頼人名'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'PayerName'
    SET @v = N'MF明細連携 振込依頼人名（全て）'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'PayerNameRaw'
    SET @v = N'MF明細連携 取引日付'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'RecordedAt'
    SET @v = N'MF明細連携 MF明細作成日付'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'MfCreatedAt'
    SET @v = N'MF明細連携 通貨レート'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'Rate'
    SET @v = N'MF明細連携 通貨換算後の金額'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'ConvertedAmount'
    SET @v = N'MF明細連携 換算するターゲット通貨ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'ToCurrencyId'
    SET @v = N'MF明細連携 対象外区分ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'ExcludeCategoryId'
    SET @v = N'MF明細連携 登録者ID'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'CreateBy'
    SET @v = N'MF明細連携 登録日時'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@v , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MfAggrTransaction', @level2type=N'COLUMN',@level2name=N'CreateAt'
END
GO


commit transaction

