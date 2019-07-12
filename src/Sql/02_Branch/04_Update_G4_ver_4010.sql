--USE [ScarletBranch01]
--GO

BEGIN TRANSACTION
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdvanceReceivedBackup]') AND type in (N'U'))
BEGIN
CREATE TABLE dbo.AdvanceReceivedBackup
    (
    Id bigint NOT NULL,
    CompanyId int NOT NULL,
    CurrencyId int NOT NULL,
    ReceiptHeaderId bigint NULL,
    ReceiptCategoryId int NOT NULL,
    CustomerId int NULL,
    SectionId int NULL,
    InputType int NOT NULL,
    Apportioned int NOT NULL,
    Approved int NOT NULL,
    Workday date NOT NULL,
    RecordedAt date NOT NULL,
    OriginalRecordedAt date NULL,
    ReceiptAmount numeric(18, 5) NOT NULL,
    AssignmentAmount numeric(18, 5) NOT NULL,
    RemainAmount numeric(18, 5) NOT NULL,
    AssignmentFlag int NOT NULL,
    PayerCode nvarchar(10) NOT NULL,
    PayerName nvarchar(140) NOT NULL,
    PayerNameRaw nvarchar(140) NOT NULL,
    SourceBankName nvarchar(140) NOT NULL,
    SourceBranchName nvarchar(15) NOT NULL,
    OutputAt datetime2(3) NULL,
    DueAt date NULL,
    MailedAt date NULL,
    OriginalReceiptId bigint NULL,
    ExcludeFlag int NOT NULL,
    ExcludeCategoryId int NULL,
    ExcludeAmount numeric(18, 5) NOT NULL,
    ReferenceNumber nvarchar(20) NOT NULL,
    RecordNumber nvarchar(40) NOT NULL,
    DensaiRegisterAt date NULL,
    Note1 nvarchar(140) NOT NULL,
    Note2 nvarchar(140) NOT NULL,
    Note3 nvarchar(140) NOT NULL,
    Note4 nvarchar(140) NOT NULL,
    BillNumber nvarchar(20) NOT NULL,
    BillBankCode nvarchar(4) NOT NULL,
    BillBranchCode nvarchar(3) NOT NULL,
    BillDrawAt date NULL,
    BillDrawer nvarchar(48) NOT NULL,
    DeleteAt datetime2(3) NULL,
    CreateBy int NOT NULL,
    CreateAt datetime2(3) NOT NULL,
    Memo nvarchar(MAX) NOT NULL,
    TransferOutputAt datetime2(3) NULL,
    StaffId int NULL
CONSTRAINT [PK_AdvanceReceivedBackup] PRIMARY KEY CLUSTERED 
(
    Id
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

DECLARE @v sql_variant 
SET @v = N'入金データID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'Id'
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'CompanyId'
SET @v = N'通貨ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'CurrencyId'
SET @v = N'入金ヘッダーID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'ReceiptHeaderId'
SET @v = N'入金区分'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'ReceiptCategoryId'
SET @v = N'得意先ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'CustomerId'
SET @v = N'入金部門ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'SectionId'
SET @v = N'入力区分'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'InputType'
SET @v = N'入金振分フラグ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'Apportioned'
SET @v = N'※未使用 承認フラグ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'Approved'
SET @v = N'作成日(当日営業日)'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'Workday'
SET @v = N'入金日(計上日)'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'RecordedAt'
SET @v = N'前回入金日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'OriginalRecordedAt'
SET @v = N'入金額'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'ReceiptAmount'
SET @v = N'消込額'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'AssignmentAmount'
SET @v = N'入金残'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'RemainAmount'
SET @v = N'消込フラグ 0:未消込 1:一部消込 2:消込済'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'AssignmentFlag'
SET @v = N'振込依頼人コード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'PayerCode'
SET @v = N'振込依頼人名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'PayerName'
SET @v = N'振込依頼人名（未編集）'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'PayerNameRaw'
SET @v = N'仕向先銀行'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'SourceBankName'
SET @v = N'仕向先支店名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'SourceBranchName'
SET @v = N'仕訳日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'OutputAt'
SET @v = N'入金期日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'DueAt'
SET @v = N'メール配信日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'MailedAt'
SET @v = N'元入金データID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'OriginalReceiptId'
SET @v = N'対象外フラグ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'ExcludeFlag'
SET @v = N'対象外区分'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'ExcludeCategoryId'
SET @v = N'対象外金額'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'ExcludeAmount'
SET @v = N'依頼人Ref.No'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'ReferenceNumber'
SET @v = N'記録番号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'RecordNumber'
SET @v = N'電子記録年月日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'DensaiRegisterAt'
SET @v = N'摘要 / 備考1'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'Note1'
SET @v = N'備考2'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'Note2'
SET @v = N'備考3'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'Note3'
SET @v = N'備考4'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'Note4'
SET @v = N'手形番号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'BillNumber'
SET @v = N'券面銀行コード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'BillBankCode'
SET @v = N'券面支店コード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'BillBranchCode'
SET @v = N'振出日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'BillDrawAt'
SET @v = N'振出人'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'BillDrawer'
SET @v = N'削除日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'DeleteAt'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'CreateAt'
SET @v = N'入金メモ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'Memo'
SET @v = N'振替仕訳日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'TransferOutputAt'
SET @v = N'営業担当者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'AdvanceReceivedBackup', N'COLUMN', N'StaffId'
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EBExcludeAccountSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE dbo.EBExcludeAccountSetting
    (
    CompanyId int NOT NULL,
    BankCode nchar(4) NOT NULL,
    BranchCode nchar(3) NOT NULL,
    AccountTypeId int NOT NULL,
    PayerCode nchar(10) NOT NULL,
    CreateBy int NOT NULL,
    CreateAt datetime2(3) NOT NULL,
    UpdateBy int NOT NULL,
    UpdateAt datetime2(3) NOT NULL
CONSTRAINT [PK_EBExcludeAccountSetting] PRIMARY KEY CLUSTERED 
(
    CompanyId,
    BankCode,
    BranchCode,
    AccountTypeId,
    PayerCode
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)  ON [PRIMARY]

DECLARE @v sql_variant 
SET @v = N'EBデータ取込対象外口座設定'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'EBExcludeAccountSetting', NULL, NULL
SET @v = N'会社ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'EBExcludeAccountSetting', N'COLUMN', N'CompanyId'
SET @v = N'銀行コード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'EBExcludeAccountSetting', N'COLUMN', N'BankCode'
SET @v = N'支店コード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'EBExcludeAccountSetting', N'COLUMN', N'BranchCode'
SET @v = N'預金種別'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'EBExcludeAccountSetting', N'COLUMN', N'AccountTypeId'
SET @v = N'振込依頼人コード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'EBExcludeAccountSetting', N'COLUMN', N'PayerCode'
SET @v = N'登録者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'EBExcludeAccountSetting', N'COLUMN', N'CreateBy'
SET @v = N'登録日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'EBExcludeAccountSetting', N'COLUMN', N'CreateAt'
SET @v = N'更新者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'EBExcludeAccountSetting', N'COLUMN', N'UpdateBy'
SET @v = N'更新日時'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'EBExcludeAccountSetting', N'COLUMN', N'UpdateAt'
END
GO


IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'ProcessingAt' AND object_id = OBJECT_ID(N'Receipt'))
BEGIN
ALTER TABLE dbo.Receipt ADD ProcessingAt date NULL
DECLARE @v sql_variant
SET @v = N'処理予定日'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receipt', N'COLUMN', N'ProcessingAt'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'StaffId' AND object_id = OBJECT_ID(N'Receipt'))
BEGIN
ALTER TABLE dbo.Receipt ADD StaffId int NULL
DECLARE @v sql_variant
SET @v = N'営業担当者ID'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Receipt', N'COLUMN', N'StaffId'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'DisplayOrder' AND object_id = OBJECT_ID(N'PaymentFileFormat'))
BEGIN
ALTER TABLE dbo.PaymentFileFormat ADD DisplayOrder int NOT NULL DEFAULT 0
DECLARE @v sql_variant
SET @v = N'表示順'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PaymentFileFormat', N'COLUMN', N'DisplayOrder'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'Available' AND object_id = OBJECT_ID(N'PaymentFileFormat'))
BEGIN
ALTER TABLE dbo.PaymentFileFormat ADD Available int NOT NULL DEFAULT 1
DECLARE @v sql_variant
SET @v = N'使用可否'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PaymentFileFormat', N'COLUMN', N'Available'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'IsNeedYear' AND object_id = OBJECT_ID(N'PaymentFileFormat'))
BEGIN
ALTER TABLE dbo.PaymentFileFormat ADD IsNeedYear int NOT NULL DEFAULT 0
DECLARE @v sql_variant
SET @v = N'引落年の要否'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'PaymentFileFormat', N'COLUMN', N'IsNeedYear'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'DataType' AND object_id = OBJECT_ID(N'ExportFieldSettingBase'))
BEGIN
ALTER TABLE dbo.ExportFieldSettingBase ADD DataType int NOT NULL CONSTRAINT DF_ExportFieldSettingBase_DataType DEFAULT 0
DECLARE @v sql_variant
SET @v = N'データ種別'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ExportFieldSettingBase', N'COLUMN', N'DataType'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'DataFormat' AND object_id = OBJECT_ID(N'ExportFieldSetting'))
BEGIN
ALTER TABLE dbo.ExportFieldSetting ADD DataFormat int NOT NULL CONSTRAINT DF_ExportFieldSetting_DataFormat DEFAULT 0
DECLARE @v sql_variant
SET @v = N'データ書式'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ExportFieldSetting', N'COLUMN', N'DataFormat'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'BillingReceiptDisplayOrder' AND object_id = OBJECT_ID(N'CollationSetting'))
BEGIN
ALTER TABLE CollationSetting ADD BillingReceiptDisplayOrder int NOT NULL DEFAULT 0
DECLARE @v sql_variant
SET @v = N'消込時の請求情報・入金情報表示設定'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CollationSetting', N'COLUMN', N'BillingReceiptDisplayOrder'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'ExternalCode' AND object_id = OBJECT_ID(N'Category'))
BEGIN
ALTER TABLE dbo.Category ADD ExternalCode nvarchar(20) NOT NULL CONSTRAINT DF_Category_ExternalCode DEFAULT ''
DECLARE @v sql_variant
SET @v = N'外部コード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Category', N'COLUMN', N'ExternalCode'
END
GO

/* 会社マスター 口座振替用 口座集約用オプション追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Company', N'COLUMN',N'TransferAggregate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'TransferAggregate'
GO
IF NOT EXISTS (select * from sys.columns
 where object_id = object_id(N'[dbo].[Company]')
 and name = N'TransferAggregate' )
ALTER TABLE [dbo].[Company] ADD
 [TransferAggregate]        INT                 NOT NULL    DEFAULT 0
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Company', N'COLUMN',N'TransferAggregate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替の作成/取込時に口座単位で集計する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'TransferAggregate'
GO

/* 得意先マスター 口座振替用カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'TransferCustomerCode'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferCustomerCode'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'TransferNewCode'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferNewCode'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'TransferAccountName'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferAccountName'
GO
IF NOT EXISTS (select * from sys.columns
 where object_id = object_id(N'[dbo].[Customer]')
 and name = N'TransferCustomerCode' )
ALTER TABLE [dbo].[Customer] ADD
 [TransferCustomerCode]             NVARCHAR(20)       NOT NULL    DEFAULT N''
,[TransferNewCode]                  NVARCHAR(1)        NOT NULL    DEFAULT N''
,[TransferAccountName]              NVARCHAR(30)       NOT NULL    DEFAULT N''
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'TransferCustomerCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用顧客コード'          , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferCustomerCode'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'TransferNewCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用新規コード'          , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferNewCode'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'TransferAccountName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用預金者名'          , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferAccountName'
GO

/* 決済代行会社 口座振替用カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'PaymentAgency', N'COLUMN',N'OutputFileName'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'OutputFileName'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'PaymentAgency', N'COLUMN',N'AppendDate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'AppendDate'
GO
IF NOT EXISTS (select * from sys.columns
 where object_id = object_id(N'[dbo].[PaymentAgency]')
 and name = N'OutputFileName' )
ALTER TABLE [dbo].[PaymentAgency] ADD
 [OutputFileName]           NVARCHAR(100)       NOT NULL    DEFAULT N''
,[AppendDate]               INT                 NOT NULL    DEFAULT 0
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'PaymentAgency', N'COLUMN',N'OutputFileName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力ファイル名'          , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'OutputFileName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'PaymentAgency', N'COLUMN',N'AppendDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自動的に日付を付与する'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'AppendDate'
GO


/* 口座振替依頼データ 履歴テーブル追加 */

IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'Id'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'Id'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'CompanyId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'CollectCategoryId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CollectCategoryId'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'PaymentAgencyId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'RequestDate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'RequestDate'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'DueAt'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'DueAt'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'OutputCount'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'OutputCount'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'OutputAmount'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'OutputAmount'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'CreateBy'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'CreateAt'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[AccountTransferLog]'))
CREATE TABLE [dbo].[AccountTransferLog]
([Id]                       BIGINT              NOT NULL    IDENTITY(1,1)
,[CompanyId]                INT                 NOT NULL
,[CollectCategoryId]        INT                 NOT NULL
,[PaymentAgencyId]          INT                 NOT NULL
,[RequestDate]              DATETIME2(3)        NOT NULL
,[DueAt]                    DATE                NOT NULL
,[OutputCount]              INT                 NOT NULL
,[OutputAmount]             NUMERIC(18, 5)      NOT NULL
,[CreateBy]                 INT                 NOT NULL
,[CreateAt]                 DATETIME2(3)        NOT NULL
,CONSTRAINT [PkAccountTransferLog] PRIMARY KEY CLUSTERED
([Id]                   ASC )
)
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkAccountTransferLogCompany]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[AccountTransferLog]')
       )
ALTER TABLE [dbo].[AccountTransferLog] ADD CONSTRAINT
 [FkAccountTransferLogCompany] FOREIGN KEY
([CompanyId]) REFERENCES [dbo].[Company] ([Id])
GO


IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼データログID'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'Id'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'CompanyId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID'                    , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'CollectCategoryId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回収区分ID'                , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CollectCategoryId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'PaymentAgencyId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決算代行会社ID'            , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'RequestDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼作成日'        , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'RequestDate'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'DueAt'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'引落日（入金予定日）'      , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'DueAt'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'OutputCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力件数'                  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'OutputCount'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'OutputAmount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力金額'                  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'OutputAmount'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'CreateBy'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID'                  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferLog', N'COLUMN',N'CreateAt'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時'                  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO


/* 請求データ 口座振替依頼履歴ID カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Billing', N'COLUMN',N'AccountTransferLogId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'AccountTransferLogId'
GO

IF NOT EXISTS (SELECT * FROM sys.columns
 where object_id = object_id(N'[dbo].[Billing]')
 and name = N'AccountTransferLogId' )
ALTER TABLE [dbo].[Billing] ADD
 [AccountTransferLogId]     BIGINT              NULL
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkBillingAccountTransferLog]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[Billing]')
       )
ALTER TABLE [dbo].[Billing] ADD CONSTRAINT
 [FkBillingAccountTransferLog] FOREIGN KEY
([AccountTransferLogId]) REFERENCES [dbo].[AccountTransferLog] ([Id])
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'BIlling', N'COLUMN',N'AccountTransferLogId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼データログID'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'AccountTransferLogId'
GO
/*口座振替 */


/* 口座振替依頼データ明細 テーブル追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'Id'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'Id'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'AccountTransferLogId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'AccountTransferLogId'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'BillingId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'BillingId'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'CompanyId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'CustomerId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'DepartmentId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'StaffId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'BilledAt'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'BilledAt'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'SalesAt'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'SalesAt'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'ClosingAt'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'ClosingAt'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'DueAt'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'DueAt'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'BillingAmount'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'BillingAmount'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'InvoiceCode'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'InvoiceCode'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'Note1'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'Note1'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferBankCode'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBankCode'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferBankName'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBankName'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferBranchCode'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBranchCode'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferBranchName'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBranchName'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferAccountTypeId'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferAccountTypeId'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferAccountNumber'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferAccountNumber'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferAccountName'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferAccountName'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferCustomerCode'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferCustomerCode'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferNewCode'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferNewCode'
GO

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = object_id(N'[dbo].[AccountTransferDetail]'))
CREATE TABLE [dbo].[AccountTransferDetail]
([Id]                       BIGINT              NOT NULL    IDENTITY(1,1)
,[AccountTransferLogId]     BIGINT              NOT NULL
,[BillingId]                BIGINT              NOT NULL
,[CompanyId]                INT                 NOT NULL
,[CustomerId]               INT                 NOT NULL
,[DepartmentId]             INT                 NOT NULL
,[StaffId]                  INT                 NOT NULL
,[BilledAt]                 DATE                    NULL
,[SalesAt]                  DATE                    NULL
,[ClosingAt]                DATE                    NULL
,[DueAt]                    DATE                    NULL
,[BillingAmount]            NUMERIC(18, 5)      NOT NULL
,[InvoiceCode]              NVARCHAR(100)       NOT NULL
,[Note1]                    NVARCHAR(100)       NOT NULL
,[TransferBankCode]         NVARCHAR(4)         NOT NULL
,[TransferBankName]         NVARCHAR(30)        NOT NULL
,[TransferBranchCode]       NVARCHAR(3)         NOT NULL
,[TransferBranchName]       NVARCHAR(30)        NOT NULL
,[TransferAccountTypeId]    INT                 NOT NULL
,[TransferAccountNumber]    NVARCHAR(10)        NOT NULL
,[TransferAccountName]      NVARCHAR(30)        NOT NULL
,[TransferCustomerCode]     NVARCHAR(20)        NOT NULL
,[TransferNewCode]          NVARCHAR(1)         NOT NULL
,CONSTRAINT [PkAccountTransferDetail] PRIMARY KEY CLUSTERED
([Id]                   ASC )
)
GO

IF NOT EXISTS (
      SELECT * FROM sys.foreign_keys
       WHERE object_id = OBJECT_ID(N'[dbo].[FkAccountTransferDetailLog]')
         AND parent_object_id = OBJECT_ID(N'[dbo].[AccountTransferDetail]')
       )
ALTER TABLE [dbo].[AccountTransferDetail] ADD CONSTRAINT
 [FkAccountTransferDetailLog] FOREIGN KEY
([AccountTransferLogId]) REFERENCES [dbo].[AccountTransferLog] ([Id]) ON DELETE CASCADE
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼データ明細ID'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'Id'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'AccountTransferLogId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼データログID'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'AccountTransferLogId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'BillingId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求ID'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'BillingId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'CompanyId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'CustomerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'DepartmentId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求部門ID'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'StaffId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'担当者ID'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'BilledAt'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求日'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'BilledAt'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'SalesAt'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'売上日'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'SalesAt'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'ClosingAt'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求締日'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'ClosingAt'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'DueAt'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定日'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'DueAt'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'BillingAmount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求金額（税込）'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'BillingAmount'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'InvoiceCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求書番号'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'InvoiceCode'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'Note1'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考１'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'Note1'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferBankCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用銀行コード'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBankCode'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferBankName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用銀行名'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBankName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferBranchCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用支店コード'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBranchCode'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferBranchName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用支店名'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBranchName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferAccountTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用預金種別'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferAccountTypeId'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferAccountNumber'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用口座番号'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferAccountNumber'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferAccountName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用預金者名'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferAccountName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferCustomerCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用顧客コード'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferCustomerCode'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AccountTransferDetail', N'COLUMN',N'TransferNewCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用新規コード'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferNewCode'
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'AutoCloseProgressDialog' AND object_id = OBJECT_ID(N'Company'))
BEGIN
ALTER TABLE dbo.Company ADD AutoCloseProgressDialog int not null default 0
DECLARE @v sql_variant
SET @v = N'処理状況ダイアログを自動で閉じる'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Company', N'COLUMN', N'AutoCloseProgressDialog'
END
GO

/* フリーインポーター 外部コード変換用 項目追加 */
INSERT INTO [dbo].[SettingDefinition]
SELECT *
  FROM (
SELECT N'ATTR6' [ItemId], N'属性区分外部コード' [Description], 1 [ItemKeyLength], 40 [ItemValueLength]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[SettingDefinition] sd
        WHERE sd.[ItemId]   = u.[ItemId] )
GO

INSERT INTO [dbo].[Setting]
SELECT N'ATTR6' [ItemId], u.[ItemKey], u.[ItemValue]
  FROM (
            SELECT N'0' [ItemKey], N''                              [ItemValue]
 UNION ALL  SELECT N'1' [ItemKey], N'1：外部コード変換を利用しない' [ItemValue]
 UNION ALL  SELECT N'2' [ItemKey], N'2：外部コード変換を利用する'   [ItemValue]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Setting] s
        WHERE s.[ItemId] = N'ATTR6'
          AND s.[ItemKey]   = u.[ItemKey] )
GO

/* フリーインポーター 取込設定 属性情報の更新 外部コード変換機能追加 */
UPDATE [dbo].[ImporterSettingBase]
   SET [AttributeDivision]        = 6
 WHERE [AttributeDivision]       <> 6
   AND [TargetColumn] IN
( N'ReceiptCategoryCode'
, N'BillingCategoryCode'
, N'CollectCategoryCode'
, N'CollectCategoryId'
, N'LessThanCollectCategoryId'
, N'GreaterThanCollectCategoryId1'
, N'GreaterThanCollectCategoryId2'
, N'GreaterThanCollectCategoryId3' )
GO

UPDATE d
   SET d.[AttributeDivision]        = 1
  FROM [dbo].[ImporterSettingDetail] d
 INNER JOIN [dbo].[ImporterSetting] s       ON s.[Id]       = d.[ImporterSettingId]
 INNER JOIN [dbo].[ImporterSettingBase] b   ON s.[FormatId] = b.[FormatId]              AND  d.[Sequence] = b.[Sequence]
 WHERE b.[AttributeDivision]        = 6
   AND d.[ImportDivision]           = 1
   AND d.[AttributeDivision]        < 1
GO


/* 得意先マスター 郵便番号 属性区分の修正 3 -> 0 */
UPDATE d
   SET d.[AttributeDivision]     = 0
  FROM [dbo].[ImporterSettingDetail] d
 INNER JOIN [dbo].[ImporterSetting] s       ON s.[Id]       = d.[ImporterSettingId]
 INNER JOIN [dbo].[ImporterSettingBase] b   ON s.[FormatId] = b.[FormatId]              AND  d.[Sequence] = b.[Sequence]
 WHERE b.[FormatId]             = 4
   AND b.[TargetColumn]         = N'PostalCode'
   AND d.[AttributeDivision]   <> 0
GO
UPDATE [dbo].[ImporterSettingBase]
   SET [AttributeDivision]      = 0
 WHERE [FormatId]               = 4
   AND [TargetColumn]           = N'PostalCode'
GO
/* 得意先マスター 取込項目修正 */
UPDATE [dbo].[ImporterSettingBase]
   SET [TargetColumn] = N'TransferBranchCode'
 WHERE [FormatId] = 4
   AND [TargetColumn] = N'TransferBrachCode'
GO
UPDATE [dbo].[ImporterSettingBase]
   SET [TargetColumn] = N'TransferBranchName'
 WHERE [FormatId] = 4
   AND [TargetColumn] = N'TransferBranchname' COLLATE Japanese_CS_AS_KS_WS
GO


/* 得意先マスター 照合処理用カラム追加 */
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'ForceMatchingIndividually'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ForceMatchingIndividually'
GO
IF  EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'CollationKey'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CollationKey'
GO
IF NOT EXISTS (select * from sys.columns
 where object_id = object_id(N'[dbo].[Customer]')
 and name = N'CollationKey' )
 BEGIN
ALTER TABLE [dbo].[Customer] ADD
 [ForceMatchingIndividually]        INT                 NOT NULL    DEFAULT 0
,[CollationKey]                     NVARCHAR(48)        NOT NULL    DEFAULT N''
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'ForceMatchingIndividually'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込対象外'          , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ForceMatchingIndividually'
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Customer', N'COLUMN',N'CollationKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合番号'                , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CollationKey'
END
GO

/* 得意先マスター項目追加 */
IF NOT EXISTS (
    SELECT * FROM [dbo].[ImporterSettingBase] b
     WHERE b.[FormatId]     = 4
       AND b.[Sequence]     = 17
       AND b.[FieldName]    = N'回収予定（都度請求）' )
BEGIN
    UPDATE [dbo].[ImporterSettingBase]
       SET [ImportDivision] = 11
     WHERE [FormatId]      = 4
       AND [TargetColumn] IN (N'CollectOffsetMonth', N'CollectOffsetDay')

    UPDATE d
       SET d.[ImportDivision] = 2
      FROM [dbo].[ImporterSettingDetail] d
     INNER JOIN [dbo].[ImporterSetting] s       ON s.[Id]       = d.[ImporterSettingId]
     INNER JOIN [dbo].[ImporterSettingBase] b   ON s.[FormatId] = b.[FormatId]              AND  d.[Sequence] = b.[Sequence]
     WHERE b.[FormatId]       = 4
       AND b.[ImportDivision] = 11
       AND d.[ImportDivision] = 0

    /* 既存の Sequence 移動 */
    UPDATE d
       SET d.[Sequence]     = d.[Sequence] + 3
      FROM [dbo].[ImporterSettingDetail] d
     INNER JOIN [dbo].[ImporterSetting] s       ON s.[Id]       = d.[ImporterSettingId]
     INNER JOIN [dbo].[ImporterSettingBase] b   ON s.[FormatId] = b.[FormatId]              AND  d.[Sequence] = b.[Sequence]
     WHERE b.[FormatId]     = 4
       AND b.[Sequence]    >= 37 /*約定金額以降*/

    UPDATE b
       SET b.[Sequence]     = b.[Sequence] + 3
      FROM [dbo].[ImporterSettingBase] b
     WHERE b.[FormatId]     = 4
       AND b.[Sequence]    >= 37 /*約定金額以降*/

    UPDATE d
       SET d.[Sequence]     = d.[Sequence] + 1
      FROM [dbo].[ImporterSettingDetail] d
     INNER JOIN [dbo].[ImporterSetting] s       ON s.[Id]       = d.[ImporterSettingId]
     INNER JOIN [dbo].[ImporterSettingBase] b   ON s.[FormatId] = b.[FormatId]              AND  d.[Sequence] = b.[Sequence]
     WHERE b.[FormatId]     = 4
       AND b.[Sequence]    >= 17 /*営業担当者以降*/

    UPDATE b
       SET b.[Sequence]     = b.[Sequence] + 1
      FROM [dbo].[ImporterSettingBase] b
     WHERE b.[FormatId]     = 4
       AND b.[Sequence]    >= 17 /*営業担当者以降*/

    INSERT INTO [dbo].[ImporterSettingBase]
    SELECT 4 [FormatId], u.[seq], u.[fname], u.[target], u.[idev], u.[adev]
      FROM (
                SELECT 17 [seq], 11 [idev], 0 [adev], N'CollectOffsetDayPerBilling' [target], N'回収予定（都度請求）' [fname]
     UNION ALL  SELECT 38 [seq],  1 [idev], 3 [adev], N'TransferCustomerCode'       [target], N'口座振替用顧客コード' [fname]
     UNION ALL  SELECT 39 [seq],  1 [idev], 0 [adev], N'TransferNewCode'            [target], N'口座振替用新規コード' [fname]
     UNION ALL  SELECT 40 [seq],  1 [idev], 3 [adev], N'TransferAccountName'        [target], N'口座振替用預金者名'   [fname]
     UNION ALL  SELECT 58 [seq], 12 [idev], 0 [adev], N'ForceMatchingIndividually'  [target], N'一括消込対象外'       [fname]
     UNION ALL  SELECT 59 [seq],  1 [idev], 3 [adev], N'CollationKey'               [target], N'照合番号'             [fname]
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
         , N'' [FixedValue]
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

/* 照合設定 入金データ取込オプション追加*/
IF NOT EXISTS (select * from sys.columns
 WHERE object_id = object_id(N'[dbo].[CollationSetting]')
 AND name = N'RemoveSpaceFromPayerName' )
ALTER TABLE [dbo].[CollationSetting] ADD
 [RemoveSpaceFromPayerName]         INT                 NOT NULL    DEFAULT 0
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'CollationSetting', N'COLUMN',N'RemoveSpaceFromPayerName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金ﾃﾞｰﾀ取込時、振込依頼人名のｽﾍﾟｰｽ除去'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'RemoveSpaceFromPayerName'
GO

/* 照合番号 追加 */
IF NOT EXISTS (select * from sys.columns
 WHERE object_id = object_id(N'[dbo].[Receipt]') AND name = N'CollationKey' )
ALTER TABLE [dbo].[Receipt]                 ADD [CollationKey]                     NVARCHAR(48)        NOT NULL    DEFAULT N''
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Receipt', N'COLUMN',N'CollationKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合番号'  , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'CollationKey'
GO


IF NOT EXISTS (select * from sys.columns
 WHERE object_id = object_id(N'[dbo].[WorkReceipt]') AND name = N'CollationKey' )
BEGIN
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'AdvanceReceivedCount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ForceMatchingIndividually'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptCount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptRemainAmount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptAssignmentAmount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptAmount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CustomerId'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'SourceBankName'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'PayerCode'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'BranchCode'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'BankCode'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'PayerName'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CurrencyId'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CompanyId'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ClientKey'

    ALTER TABLE [dbo].[WorkReceipt] DROP CONSTRAINT [FkWorkReceiptCompany]
    DROP TABLE [dbo].[WorkReceipt]

    CREATE TABLE [dbo].[WorkReceipt]
    ([ClientKey]                    [varbinary](20)     NOT NULL
    ,[CompanyId]                    [int]               NOT NULL
    ,[CurrencyId]                   [int]               NOT NULL
    ,[PayerName]                    [nvarchar](140)     NOT NULL
    ,[BankCode]                     [nvarchar](4)       NOT NULL
    ,[BranchCode]                   [nvarchar](3)       NOT NULL
    ,[PayerCode]                    [nvarchar](10)      NOT NULL
    ,[SourceBankName]               [nvarchar](140)     NOT NULL
    ,[SourceBranchName]             [nvarchar](15)      NOT NULL
    ,[CustomerId]                   [int]               NOT NULL
    ,[CollationKey]                 [nvarchar](48)      NOT NULL
    ,[ReceiptAmount]                [numeric](18, 5)    NOT NULL
    ,[ReceiptAssignmentAmount]      [numeric](18, 5)    NOT NULL
    ,[ReceiptRemainAmount]          [numeric](18, 5)    NOT NULL
    ,[ReceiptCount]                 [int]               NOT NULL
    ,[ForceMatchingIndividually]    [int]               NOT NULL
    ,[AdvanceReceivedCount]         [int]               NOT NULL
    ,CONSTRAINT [PkWorkReceipt] PRIMARY KEY CLUSTERED 
    ([ClientKey] ASC
    ,[CompanyId] ASC
    ,[CurrencyId] ASC
    ,[PayerName] ASC
    ,[BankCode] ASC
    ,[BranchCode] ASC
    ,[PayerCode] ASC
    ,[SourceBankName] ASC
    ,[SourceBranchName] ASC
    ,[CustomerId] ASC
    ,[CollationKey] ASC )
    )

    ALTER TABLE [dbo].[WorkReceipt]  WITH CHECK ADD  CONSTRAINT [FkWorkReceiptCompany] FOREIGN KEY([CompanyId])
    REFERENCES [dbo].[Company] ([Id])

    ALTER TABLE [dbo].[WorkReceipt] CHECK CONSTRAINT [FkWorkReceiptCompany]

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー'   , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ClientKey'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID'             , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CompanyId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID'             , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CurrencyId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名'       , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'PayerName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード'         , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'BankCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード'         , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'BranchCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人番号'     , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'PayerCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向銀行'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'SourceBankName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向支店'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CustomerId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合番号'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CollationKey'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金額'             , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptAmount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'引当額'             , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptAssignmentAmount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金残'             , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptRemainAmount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金件数'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptCount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込対象外件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ForceMatchingIndividually'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前受件数'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'AdvanceReceivedCount'
END
GO

IF NOT EXISTS (select * from sys.columns
 WHERE object_id = object_id(N'[dbo].[WorkCollation]') AND name = N'CollationKey' )
BEGIN
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ForceMatchingIndividually'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'AdvanceReceivedCount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptCount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptRemainAmount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptAssignmentAmount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptAmount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BillingCount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BillingRemainAmount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BillingAmount'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerShareTransferFee'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerKana'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerName'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CollationType'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CustomerId'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'SourceBankName'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BranchCode'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BankCode'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'PayerCode'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'PayerName'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerId'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CurrencyId'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CompanyId'
    EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ClientKey'

    ALTER TABLE [dbo].[WorkCollation] DROP CONSTRAINT [FkWorkCollationCompany]
    DROP TABLE [dbo].[WorkCollation]

    CREATE TABLE [dbo].[WorkCollation]
    ([ClientKey]                        [varbinary](20)     NOT NULL
    ,[CompanyId]                        [int]               NOT NULL
    ,[CurrencyId]                       [int]               NOT NULL
    ,[ParentCustomerId]                 [int]               NOT NULL
    ,[PaymentAgencyId]                  [int]               NOT NULL
    ,[PayerName]                        [nvarchar](140)     NOT NULL
    ,[PayerCode]                        [nvarchar](10)      NOT NULL
    ,[BankCode]                         [nvarchar](4)       NOT NULL
    ,[BranchCode]                       [nvarchar](3)       NOT NULL
    ,[SourceBankName]                   [nvarchar](140)     NOT NULL
    ,[SourceBranchName]                 [nvarchar](15)      NOT NULL
    ,[CustomerId]                       [int]               NOT NULL
    ,[CollationKey]                     [nvarchar](48)      NOT NULL
    ,[CollationType]                    [int]               NOT NULL
    ,[ParentCustomerName]               [nvarchar](140)     NOT NULL
    ,[ParentCustomerKana]               [nvarchar](140)     NOT NULL
    ,[ParentCustomerShareTransferFee]   [int]               NOT NULL
    ,[BillingAmount]                    [numeric](18, 5)    NOT NULL
    ,[BillingRemainAmount]              [numeric](18, 5)    NOT NULL
    ,[BillingCount]                     [int]               NOT NULL
    ,[ReceiptAmount]                    [numeric](18, 5)    NOT NULL
    ,[ReceiptAssignmentAmount]          [numeric](18, 5)    NOT NULL
    ,[ReceiptRemainAmount]              [numeric](18, 5)    NOT NULL
    ,[ReceiptCount]                     [int]               NOT NULL
    ,[AdvanceReceivedCount]             [int]               NOT NULL
    ,[ForceMatchingIndividually]        [int]               NOT NULL
    ,CONSTRAINT [PkWorkCollation] PRIMARY KEY CLUSTERED
    ([ClientKey]        ASC
    ,[CompanyId]        ASC
    ,[CurrencyId]       ASC
    ,[ParentCustomerId] ASC
    ,[PaymentAgencyId]  ASC
    ,[PayerName]        ASC
    ,[PayerCode]        ASC
    ,[BankCode]         ASC
    ,[BranchCode]       ASC
    ,[SourceBankName]   ASC
    ,[SourceBranchName] ASC
    ,[CustomerId]       ASC
    ,[CollationKey]     ASC
    ,[CollationType]    ASC )
    )

    ALTER TABLE [dbo].[WorkCollation]  WITH CHECK ADD  CONSTRAINT [FkWorkCollationCompany] FOREIGN KEY([CompanyId])
    REFERENCES [dbo].[Company] ([Id])
    ALTER TABLE [dbo].[WorkCollation] CHECK CONSTRAINT [FkWorkCollationCompany]

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー'         , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ClientKey'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID'                   , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CompanyId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID'                   , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CurrencyId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'代表得意先ID'             , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社ID'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名'             , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'PayerName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人コード'         , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'PayerCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード'               , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BankCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード'               , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BranchCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向銀行名'               , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'SourceBankName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向支店名'               , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合番号'                 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CollationKey'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID'                 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CustomerId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合タイプ'               , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CollationType'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'債権代表者名'             , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'債権代表者カナ'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerKana'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'債権代表者手数料負担区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerShareTransferFee'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求額'                   , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BillingAmount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求残'                   , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BillingRemainAmount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求件数'                 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BillingCount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金額'                   , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptAmount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'引当額'                   , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptAssignmentAmount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金残'                   , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptRemainAmount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金件数'                 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptCount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前受件数'                 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'AdvanceReceivedCount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込対象外件数'       , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ForceMatchingIndividually'
END
GO

/* 複数請求部門選択 */
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = object_id(N'[dbo].[WorkDepartmentTarget]'))
BEGIN
    CREATE TABLE [dbo].[WorkDepartmentTarget]
    ([ClientKey]            [varbinary](20)         NOT NULL
    ,[CompanyId]            [int]                   NOT NULL
    ,[DepartmentId]         [int]                   NOT NULL
    ,[UseCollation]         [int]                   NOT NULL
    ,CONSTRAINT [PkWorkDepartmentTarget] PRIMARY KEY CLUSTERED
    ([ClientKey]            ASC
    ,[CompanyId]            ASC
    ,[DepartmentId]         ASC ) )

    ALTER TABLE [dbo].[WorkDepartmentTarget]  WITH CHECK ADD  CONSTRAINT [FkWorkDepartmentTargetCompany] FOREIGN KEY([CompanyId])
    REFERENCES [dbo].[Company] ([Id])
    ALTER TABLE [dbo].[WorkDepartmentTarget] CHECK CONSTRAINT [FkWorkDepartmentTargetCompany]

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkDepartmentTarget', @level2type=N'COLUMN',@level2name=N'ClientKey'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkDepartmentTarget', @level2type=N'COLUMN',@level2name=N'CompanyId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求部門ID'       , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkDepartmentTarget', @level2type=N'COLUMN',@level2name=N'DepartmentId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'絞込フラグ'       , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkDepartmentTarget', @level2type=N'COLUMN',@level2name=N'UseCollation'
END
GO

/* WorkBillingTarget 追加 */
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = object_id(N'[dbo].[WorkBillingTarget]'))
BEGIN
    DROP TABLE [dbo].[WorkBillingTarget];
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = object_id(N'[dbo].[WorkBillingTarget]'))
BEGIN
    CREATE TABLE [dbo].[WorkBillingTarget]
    ([ClientKey]        [varbinary](20)             NOT NULL
    ,[BillingId]        [bigint]                    NOT NULL
    ,[CustomerId]       [int]                       NOT NULL
    ,[PaymentAgencyId]  [int]                       NOT NULL
    ,[BillingAmount]    [numeric](18, 5)            NOT NULL
    ,[RemainAmount]     [numeric](18, 5)            NOT NULL
    ,CONSTRAINT [PkWorkBillingTarget] PRIMARY KEY CLUSTERED
    ([ClientKey]        ASC
    ,[BillingId]        ASC ) )

    CREATE NONCLUSTERED INDEX [IdxWorkBillingTarget] ON [dbo].[WorkBillingTarget]
    ([ClientKey]
    ,[CustomerId]
    ,[PaymentAgencyId])
    INCLUDE
    ([BillingId]
    ,[BillingAmount]
    ,[RemainAmount])

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー'      , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'ClientKey'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求ID'                , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'BillingId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先（債権代表者）ID', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'CustomerId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社ID'        , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求額'                , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'BillingAmount'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求残'                , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'RemainAmount'
END

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CustomerGroup]') AND name = N'IdxCustomerGroupChild')
DROP INDEX [IdxCustomerGroupChild] ON [dbo].[CustomerGroup]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Matching]') AND name = N'IdxMatchingBilling')
DROP INDEX [IdxMatchingBilling] ON [dbo].[Matching]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Matching]') AND name = N'IdxMatchingReceipt')
DROP INDEX [IdxMatchingReceipt] ON [dbo].[Matching]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Matching]') AND name = N'IdxMatchingMatchingHeaderId')
DROP INDEX [IdxMatchingMatchingHeaderId] ON [dbo].[Matching]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CustomerGroup]') AND name = N'IdxCustomerGroupChild')
CREATE NONCLUSTERED INDEX [IdxCustomerGroupChild] ON [dbo].[CustomerGroup]
([ChildCustomerId]  ASC)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Matching]') AND name = N'IdxMatchingBilling')
CREATE NONCLUSTERED INDEX [IdxMatchingBilling] ON [dbo].[Matching]
([BillingId]        ASC
,[MatchingHeaderId] ASC)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Matching]') AND name = N'IdxMatchingReceipt')
CREATE NONCLUSTERED INDEX [IdxMatchingReceipt] ON [dbo].[Matching]
([ReceiptId]        ASC
,[MatchingHeaderId] ASC)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Matching]') AND name = N'IdxMatchingMatchingHeaderId')
CREATE NONCLUSTERED INDEX [IdxMatchingMatchingHeaderId] ON [dbo].[Matching]
([MatchingHeaderId] ASC)
GO

/* WorkReceiptTarget 修正 */
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = object_id(N'[dbo].[WorkReceiptTarget]'))
BEGIN
    DROP TABLE [dbo].[WorkReceiptTarget];
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = object_id(N'[dbo].[WorkReceiptTarget]'))
BEGIN
    CREATE TABLE [dbo].[WorkReceiptTarget]
    ([ClientKey]        [varbinary](20)             NOT NULL
    ,[ReceiptId]        [bigint]                    NOT NULL
    ,[CompanyId]        [int]                       NOT NULL
    ,[CurrencyId]       [int]                       NOT NULL
    ,[PayerName]        [nvarchar](140)             NOT NULL
    ,[BankCode]         [nvarchar](4)               NOT NULL
    ,[BranchCode]       [nvarchar](3)               NOT NULL
    ,[PayerCode]        [nvarchar](10)              NOT NULL
    ,[SourceBankName]   [nvarchar](140)             NOT NULL
    ,[SourceBranchName] [nvarchar](15)              NOT NULL
    ,[CollationKey]     [nvarchar](48)              NOT NULL
    ,[CustomerId]       [int]                       NOT NULL
    ,[CollationType]    [int]                       NOT NULL
    ,CONSTRAINT [PkWorkReceiptTarget] PRIMARY KEY CLUSTERED
    ([ClientKey]        ASC
    ,[ReceiptId]        ASC ) )

    CREATE NONCLUSTERED INDEX [IdxWorkReceiptTarget] ON [dbo].[WorkReceiptTarget]
    ([ClientKey]
    ,[CompanyId]
    ,[CurrencyId]
    ,[PayerName]
    ,[BankCode]
    ,[BranchCode]
    ,[PayerCode]
    ,[SourceBankName]
    ,[SourceBranchName]
    ,[CollationKey]
    ,[CustomerId])
    INCLUDE
    ([ReceiptId]
    ,[CollationType])

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー'      , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'ClientKey'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金ID'                , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'ReceiptId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID'                , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'CompanyId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID'                , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'CurrencyId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名'          , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'PayerName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード'            , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'BankCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード'            , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'BranchCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人コード'      , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'PayerCode'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向銀行'              , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'SourceBankName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向支店'              , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合番号'              , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'CollationKey'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID'              , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'CustomerId'
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合タイプ'            , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'CollationType'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'LimitAccessFolder' AND object_id = OBJECT_ID(N'ApplicationControl'))
BEGIN
ALTER TABLE dbo.ApplicationControl ADD LimitAccessFolder int NOT NULL DEFAULT 0
DECLARE @v sql_variant
SET @v = N'フォルダ選択の制限'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ApplicationControl', N'COLUMN', N'LimitAccessFolder'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = N'RootPath' AND object_id = OBJECT_ID(N'ApplicationControl'))
BEGIN
ALTER TABLE dbo.ApplicationControl ADD RootPath nvarchar(50) NOT NULL DEFAULT N''
DECLARE @v sql_variant
SET @v = N'ルートフォルダのパス'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'ApplicationControl', N'COLUMN', N'RootPath'
END

/* 請求フリーインポーター 得意先自動生成用 照合番号追加 */
IF EXISTS (SELECT * FROM ImporterSettingBase b WHERE b.FormatId = 1 AND b.[Sequence] = 27 AND b.TargetColumn = N'UseDiscount')
BEGIN
    UPDATE d
       SET d.[Sequence]     = d.[Sequence] + 1
      FROM [dbo].[ImporterSettingDetail] d
     INNER JOIN [dbo].[ImporterSetting] s       ON s.[Id]       = d.[ImporterSettingId]
     INNER JOIN [dbo].[ImporterSettingBase] b   ON s.[FormatId] = b.[FormatId]              AND  d.[Sequence] = b.[Sequence]
     WHERE b.[FormatId]     = 1
       AND b.[Sequence]    >= 27
    ;
    UPDATE b
       SET b.[Sequence]     = b.[Sequence] + 1
      FROM [dbo].[ImporterSettingBase]  b
     WHERE b.FormatId       = 1
       AND b.[Sequence]    >= 27
    ;

    INSERT INTO [dbo].[ImporterSettingBase]
    VALUES (1, 27, N'照合番号', N'CollationKey', 1, 3)
    ;

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

/* 照合ロジック設定 項目追加 カラム修正 */
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id('[dbo].[Customer]') AND name = N'ForceMatchingIndividually')
EXEC sp_rename N'dbo.Customer.ForceMatchingIndividually', N'PrioritizeMatchingIndividually', 'COLUMN'
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[CollationSetting]') AND name = N'ForceSequentialMatchingSingleReceipt')
EXEC sp_rename N'dbo.CollationSetting.ForceSequentialMatchingSingleReceipt', N'PrioritizeMatchingIndividuallyMultipleReceipts', 'COLUMN'
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[CollationSetting]') AND name = N'PrioritizeMatchingIndividuallyTaxTolerance')
BEGIN
    ALTER TABLE [dbo].[CollationSetting]
    ADD [PrioritizeMatchingIndividuallyTaxTolerance] INT NOT NULL DEFAULT (0)
    ;
    DECLARE @v sql_variant
    SET @v = N'差額が消費税誤差の範囲内でも一括消込対象外から外す（消費税誤差時に、個別消込優先）'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CollationSetting', N'COLUMN', N'PrioritizeMatchingIndividuallyTaxTolerance'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = object_id(N'[dbo].[CollationSetting]') AND name = N'JournalizingPattern')
BEGIN
    ALTER TABLE [dbo].[CollationSetting]
    ADD [JournalizingPattern] INT NOT NULL DEFAULT (0)
    ;
    DECLARE @v sql_variant
    SET @v = N'仕訳出力内容設定 0:標準, 1:汎用'
    EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CollationSetting', N'COLUMN', N'JournalizingPattern'
END
GO

/* メニューマスター 外貨利用関係なくメニュー表示フラグ更新 */
IF NOT EXISTS (SELECT * FROM Menu
WHERE NotUseForeignCurrency = 0
AND MenuId In (N'PB1901', N'PC0701', N'PC0801'))

BEGIN

/* 決済代行会社マスター */
UPDATE d
  SET d.[NotUseForeignCurrency] = 0
FROM [dbo].[Menu] d
WHERE d.[MenuId] = N'PB1901'

/* 口座振替依頼データ作成 */
UPDATE d
  SET d.[NotUseForeignCurrency] = 0
FROM [dbo].[Menu] d
WHERE d.[MenuId] = N'PC0701'

/* 口座振替結果データ取込 */
UPDATE d
  SET d.[NotUseForeignCurrency] = 0
FROM [dbo].[Menu] d
WHERE d.[MenuId] = N'PC0801'

END
GO

/* 区分マスター Base */
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CategoryBase]') AND type in (N'U'))
BEGIN
CREATE TABLE dbo.CategoryBase
    (
    CategoryType int NOT NULL,
    Code nvarchar(20) NOT NULL,
    Name nvarchar(40) NOT NULL,
    TaxClassId int NULL,
    UseLimitDate int NOT NULL,
    UseAdvanceReceived int NOT NULL,
    UseInput int NOT NULL,
CONSTRAINT [PK_CategoryBase] PRIMARY KEY CLUSTERED 
(
    CategoryType,
    Code
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)  ON [PRIMARY]

DECLARE @v sql_variant 
SET @v = N'区分マスターベース'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CategoryBase', NULL, NULL
SET @v = N'区分識別'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CategoryBase', N'COLUMN', N'CategoryType'
SET @v = N'区分コード'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CategoryBase', N'COLUMN', N'Code'
SET @v = N'区分名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CategoryBase', N'COLUMN', N'Name'
SET @v = N'税区分'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CategoryBase', N'COLUMN', N'TaxClassId'
SET @v = N'期日入力可否'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CategoryBase', N'COLUMN', N'UseLimitDate'
SET @v = N'前受フラグ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CategoryBase', N'COLUMN', N'UseAdvanceReceived'
SET @v = N'入力画面利用可否'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'CategoryBase', N'COLUMN', N'UseInput'

INSERT INTO CategoryBase
(CategoryType, Code, Name, TaxClassId, UseLimitDate, UseAdvanceReceived, UseInput)
          SELECT 1 [CategoryType], '01' [Code], '売上'     [Name], 1    [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '01' [Code], '振込'     [Name], 4    [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '02' [Code], '手形'     [Name], 4    [TaxClassId], 1 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '03' [Code], '期日現金' [Name], 4    [TaxClassId], 1 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '04' [Code], '小切手'   [Name], 4    [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '05' [Code], '相殺'     [Name], 4    [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 2 [CategoryType], '99' [Code], '前受'     [Name], 4    [TaxClassId], 0 [UseLimitDate], 1 [UseAdvanceReceived], 0 [UseInput]
UNION ALL SELECT 3 [CategoryType], '00' [Code], '約定'     [Name], NULL [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 3 [CategoryType], '01' [Code], '振込'     [Name], NULL [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 3 [CategoryType], '02' [Code], '手形'     [Name], NULL [TaxClassId], 1 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 3 [CategoryType], '03' [Code], '期日現金' [Name], NULL [TaxClassId], 1 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]
UNION ALL SELECT 3 [CategoryType], '04' [Code], '小切手'   [Name], NULL [TaxClassId], 0 [UseLimitDate], 0 [UseAdvanceReceived], 1 [UseInput]

END
GO


ALTER TABLE [dbo].[MenuAuthority] DROP CONSTRAINT [FkMenuAuthorityMenu]
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Sequence'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'MenuCategory'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'MenuName'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'MenuId'
GO

DROP TABLE [dbo].[Menu];
GO

CREATE TABLE [dbo].[Menu]
([MenuId]                           NVARCHAR(50)            NOT NULL
,[MenuName]                         NVARCHAR(40)            NOT NULL
,[MenuCategory]                     NVARCHAR(2)             NOT NULL
,[Sequence]                         INT                     NOT NULL
,[IsStandard]                       INT                     NOT NULL
,[UseScheduledPayment]              INT                     NOT NULL
,[UseReceiptSection]                INT                     NOT NULL
,[UseAuthorization]                 INT                     NOT NULL
,[UseLongTermAdvanceReceived]       INT                     NOT NULL
,[UseCashOnDueDates]                INT                     NOT NULL
,[UseDiscount]                      INT                     NOT NULL
,[UseForeignCurrency]               INT                     NOT NULL
,[NotUseForeignCurrency]            INT                     NOT NULL
,[UseBillingFilter]                 INT                     NOT NULL
,[UseDistribution]                  INT                     NOT NULL
,[UseOperationLogging]              INT                     NOT NULL
,[CreateBy]                         INT                     NOT NULL
,[CreateAt]                         DATETIME2(3)            NOT NULL
,[UpdateBy]                         INT                     NOT NULL
,[UpdateAt]                         DATETIME2(3)            NOT NULL
,CONSTRAINT [PkMenu]    PRIMARY KEY CLUSTERED
(
    [MenuId]    ASC
)
)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'画面ID'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'MenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'画面名'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'MenuName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'タブ名'           , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'MenuCategory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メニュー順序'     , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Sequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標準メニュー'     , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'IsStandard'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定入力利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseScheduledPayment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門管理利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseReceiptSection'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'承認機能利用'     , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseAuthorization'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'長期前受管理'     , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseLongTermAdvanceReceived'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'期日現金管理'     , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseCashOnDueDates'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'歩引き対応'       , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseDiscount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外貨対応'         , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseForeignCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外貨対応時未使用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'NotUseForeignCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求絞込機能'     , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseBillingFilter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配信機能利用'     , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseDistribution'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作ログ利用'     , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseOperationLogging'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID'         , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時'         , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID'         , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時'         , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO

INSERT INTO [dbo].[Menu]
     ( [MenuId]
     , [MenuName]
     , [MenuCategory]
     , [Sequence]
     , [IsStandard]
     , [UseScheduledPayment]
     , [UseReceiptSection]
     , [UseAuthorization]
     , [UseLongTermAdvanceReceived]
     , [UseCashOnDueDates]
     , [UseDiscount]
     , [UseForeignCurrency]
     , [NotUseForeignCurrency]
     , [UseBillingFilter]
     , [UseDistribution]
     , [UseOperationLogging]
     , [CreateBy]
     , [CreateAt]
     , [UpdateBy]
     , [UpdateAt] )
SELECT u.Id             [MenuId]
     , u.Name           [MenuName]
     , u.Category       [MenuCategory]
     , u.[Sequence]
     , u.[std]          [IsStandard]
     , u.[sch]          [UseScheduledPayment]
     , u.[sec]          [UseReceiptSection]
     , u.[auth]         [UseAuthorization]
     , u.[div]          [UseLongTermAdvanceReceived]
     , u.[due]          [UseCashOnDueDates]
     , 0                [UseDiscount]
     , u.[cur]          [UseForeignCurrency]
     , u.[ncur]         [NotUseForeignCurrency]
     , 0                [UseBillingFilter]
     , u.[dis]          [UseDistribution]
     , 0                [UseOperationLogging]
     , 0                [CreateBy]
     , getdate()        [CreateAt]
     , 0                [UpdateBy]
     , getdate()        [UpdateAt]
  FROM (
            SELECT N'PB0101' [Id], N'PB' [Category],   10 [Sequence], N'会社マスター'                 [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB0201' [Id], N'PB' [Category],   40 [Sequence], N'請求部門マスター'             [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB0301' [Id], N'PB' [Category],   30 [Sequence], N'ログインユーザーマスター'     [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB0401' [Id], N'PB' [Category],   50 [Sequence], N'営業担当者マスター'           [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB0501' [Id], N'PB' [Category],   20 [Sequence], N'得意先マスター'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB0601' [Id], N'PB' [Category],   60 [Sequence], N'債権代表者マスター'           [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB0701' [Id], N'PB' [Category],   80 [Sequence], N'科目マスター'                 [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB0801' [Id], N'PB' [Category],   70 [Sequence], N'銀行口座マスター'             [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB0901' [Id], N'PB' [Category],  100 [Sequence], N'区分マスター'                 [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB1001' [Id], N'PB' [Category],  150 [Sequence], N'学習履歴データ管理'           [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB1101' [Id], N'PB' [Category],  160 [Sequence], N'入金部門マスター'             [Name], 0 [std], 1 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB1201' [Id], N'PB' [Category],  110 [Sequence], N'入金・請求部門対応マスター'   [Name], 0 [std], 1 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB1301' [Id], N'PB' [Category],  120 [Sequence], N'入金部門・担当者対応マスター' [Name], 0 [std], 1 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB1401' [Id], N'PB' [Category],  130 [Sequence], N'管理マスター'                 [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB1501' [Id], N'PB' [Category],  180 [Sequence], N'除外カナマスター'             [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB1601' [Id], N'PB' [Category],  190 [Sequence], N'カレンダーマスター'           [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB1701' [Id], N'PB' [Category],  210 [Sequence], N'法人格マスター'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB1801' [Id], N'PB' [Category],  220 [Sequence], N'銀行・支店マスター'           [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB1901' [Id], N'PB' [Category],  140 [Sequence], N'決済代行会社マスター'         [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PB2101' [Id], N'PB' [Category],  200 [Sequence], N'通貨マスター'                 [Name], 0 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 1 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PC0101' [Id], N'PC' [Category], 1010 [Sequence], N'請求フリーインポーター'       [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PC0201' [Id], N'PC' [Category], 1020 [Sequence], N'請求データ入力'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PC0301' [Id], N'PC' [Category], 1030 [Sequence], N'請求データ検索'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PC0501' [Id], N'PC' [Category], 1050 [Sequence], N'入金予定日変更'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PC0601' [Id], N'PC' [Category], 1060 [Sequence], N'請求仕訳出力'                 [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PC0701' [Id], N'PC' [Category], 1070 [Sequence], N'口座振替依頼データ作成'       [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PC0801' [Id], N'PC' [Category], 1080 [Sequence], N'口座振替結果データ取込'       [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PC0901' [Id], N'PC' [Category], 1090 [Sequence], N'入金予定入力'                 [Name], 0 [std], 0 [sec], 1 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PC1001' [Id], N'PC' [Category], 1100 [Sequence], N'入金予定フリーインポーター'   [Name], 0 [std], 0 [sec], 1 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PD0101' [Id], N'PD' [Category], 2010 [Sequence], N'入金EBデータ取込'             [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PD0201' [Id], N'PD' [Category], 2020 [Sequence], N'入金フリーインポーター'       [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PD0301' [Id], N'PD' [Category], 2030 [Sequence], N'入金データ入力'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PD0401' [Id], N'PD' [Category], 2040 [Sequence], N'入金データ振分'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PD0501' [Id], N'PD' [Category], 2050 [Sequence], N'入金データ検索'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PD0601' [Id], N'PD' [Category], 2060 [Sequence], N'前受一括計上処理'             [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PD0701' [Id], N'PD' [Category], 2070 [Sequence], N'入金仕訳出力'                 [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PD0801' [Id], N'PD' [Category], 2080 [Sequence], N'入金部門振替処理'             [Name], 0 [std], 1 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PE0101' [Id], N'PE' [Category], 3010 [Sequence], N'一括消込'                     [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PE0201' [Id], N'PE' [Category], 3020 [Sequence], N'消込仕訳出力'                 [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PE0301' [Id], N'PE' [Category], 3030 [Sequence], N'消込履歴データ検索'           [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PE0401' [Id], N'PE' [Category], 3040 [Sequence], N'消込仕訳出力取消'             [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PE0501' [Id], N'PE' [Category], 3050 [Sequence], N'未消込請求データ削除'         [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PE0601' [Id], N'PE' [Category], 3060 [Sequence], N'未消込入金データ削除'         [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PE0701' [Id], N'PE' [Category], 3070 [Sequence], N'消込データ承認'               [Name], 0 [std], 0 [sec], 0 [sch], 1 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PF0101' [Id], N'PF' [Category], 5010 [Sequence], N'請求残高年齢表'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PF0201' [Id], N'PF' [Category], 5020 [Sequence], N'債権総額管理表'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 1 [ncur], 0 [dis]
 UNION ALL  SELECT N'PF0301' [Id], N'PF' [Category], 5030 [Sequence], N'入金予定明細表'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PF0401' [Id], N'PF' [Category], 5040 [Sequence], N'滞留明細一覧表'               [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PF0501' [Id], N'PF' [Category], 5060 [Sequence], N'得意先別消込台帳'             [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PF0601' [Id], N'PF' [Category], 5070 [Sequence], N'回収予定表'                   [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 1 [ncur], 0 [dis]
 UNION ALL  SELECT N'PG0101' [Id], N'PG' [Category], 4010 [Sequence], N'回収通知メール配信'           [Name], 0 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 1 [dis]
 UNION ALL  SELECT N'PG0201' [Id], N'PG' [Category], 4020 [Sequence], N'回収遅延通知メール配信'       [Name], 0 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 1 [dis]
 UNION ALL  SELECT N'PG0301' [Id], N'PG' [Category], 4030 [Sequence], N'メール設定'                   [Name], 0 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 1 [dis]
 UNION ALL  SELECT N'PG0401' [Id], N'PG' [Category], 4040 [Sequence], N'WebViewer公開処理'            [Name], 0 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 1 [dis]
 UNION ALL  SELECT N'PH0101' [Id], N'PH' [Category], 6020 [Sequence], N'各種設定＆セキュリティ'       [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PH0201' [Id], N'PH' [Category], 6010 [Sequence], N'不要データ削除＆バックアップ' [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PH0301' [Id], N'PH' [Category], 6030 [Sequence], N'残高メンテナンス処理'         [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PH0401' [Id], N'PH' [Category], 6040 [Sequence], N'データトランスファー'         [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PH0501' [Id], N'PH' [Category], 6050 [Sequence], N'タイムスケジューラー'         [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PH0601' [Id], N'PH' [Category], 6060 [Sequence], N'色マスター'                   [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PH0701' [Id], N'PH' [Category], 6070 [Sequence], N'グリッド表示設定'             [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PH0801' [Id], N'PH' [Category], 6080 [Sequence], N'照合ロジック設定'             [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PH0901' [Id], N'PH' [Category], 6090 [Sequence], N'操作ログ管理'                 [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
 UNION ALL  SELECT N'PH1001' [Id], N'PH' [Category], 6100 [Sequence], N'項目名称設定'                 [Name], 1 [std], 0 [sec], 0 [sch], 0 [auth], 0 [div], 0 [due], 0 [cur], 0 [ncur], 0 [dis]
       ) u
 WHERE NOT EXISTS (
       SELECT 1
         FROM [dbo].[Menu] m
        WHERE m.MenuId      = u.Id )
 ORDER BY
       u.Id;
GO

DELETE ma
FROM MenuAuthority ma
WHERE NOT EXISTS (
SELECT *
FROM Menu m
WHERE m.MenuId = ma.MenuId
)
GO

ALTER TABLE [dbo].[MenuAuthority] ADD  CONSTRAINT [FkMenuAuthorityMenu] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menu] ([MenuId])
ON DELETE CASCADE
GO


COMMIT