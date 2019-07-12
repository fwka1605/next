
IF EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id
WHERE st.name = N'Ids' AND ss.name = N'dbo')
BEGIN
    DROP TYPE [dbo].[Ids];
END;
GO
-- master 系 Id を格納するテーブル型
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id
WHERE st.name = N'Ids' AND ss.name = N'dbo')
CREATE TYPE [dbo].[Ids] AS TABLE 
(
    [Id]    INT             NOT NULL
,   PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id
WHERE st.name = N'BigIds' AND ss.name = N'dbo')
BEGIN
    DROP TYPE [dbo].[BigIds];
END;
GO
-- transaction 系 Id を格納するテーブル型
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id
WHERE st.name = N'BigIds' AND ss.name = N'dbo')
CREATE TYPE [dbo].[BigIds] AS TABLE
(
    [Id]    BIGINT          NOT NULL
,   PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id
WHERE st.name = N'Codes' AND ss.name = N'dbo')
BEGIN
    DROP TYPE [dbo].[Codes];
END;
GO
-- master 系 Code を格納するテーブル型
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id
WHERE st.name = N'Codes' AND ss.name = N'dbo')
CREATE TYPE [dbo].[Codes] AS TABLE
(
    [Code]  nvarchar(50)    NOT NULL
,   PRIMARY KEY ([Code])
);
GO


IF EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id
WHERE st.name = N'BillingImportDuplication' AND ss.name = N'dbo')
BEGIN
    DROP TYPE [dbo].[BillingImportDuplication];
END;
GO
-- 請求インポート 重複チェックに使用するテーブル型
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id
WHERE st.name = N'BillingImportDuplication' AND ss.name = N'dbo')
CREATE TYPE [dbo].[BillingImportDuplication] AS TABLE
(
    [RowNumber]             int           NOT NULL
  , [CustomerId]            int               NULL
  , [BilledAt]              date              NULL
  , [BillingAmount]         numeric(18,5)     NULL
  , [TaxAmount]             numeric(18,5)     NULL
  , [DueAt]                 date              NULL
  , [DepartmentId]          int               NULL
  , [DebitAccountTitleId]   int               NULL
  , [SalesAt]               date              NULL
  , [InvoiceCode]           nvarchar(100)     NULL
  , [ClosingAt]             date              NULL
  , [StaffId]               int               NULL
  , [Note1]                 nvarchar(100)     NULL
  , [BillingCategoryId]     int               NULL
  , [CollectCategoryId]     int               NULL
  , [Price]                 numeric(18,5)     NULL
  , [TaxClassId]            int               NULL
  , [Note2]                 nvarchar(100)     NULL
  , [Note3]                 nvarchar(100)     NULL
  , [Note4]                 nvarchar(100)     NULL
  , [Note5]                 nvarchar(100)     NULL
  , [Note6]                 nvarchar(100)     NULL
  , [Note7]                 nvarchar(100)     NULL
  , [Note8]                 nvarchar(100)     NULL
  , [CurrencyId]            int               NULL
,   PRIMARY KEY ([RowNumber])
);
GO


IF EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id
WHERE st.name = N'ReceiptImportDuplication' AND ss.name = N'dbo')
BEGIN
    DROP TYPE [dbo].[ReceiptImportDuplication];
END;
GO
-- 入金インポート 重複チェックに使用するテーブル型
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id
WHERE st.name = N'ReceiptImportDuplication' AND ss.name = N'dbo')
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
GO