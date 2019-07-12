--USE [VOneG4]
--GO

/****** Object:  User [Scarlet]    Script Date: 2017/12/14 18:13:15 ******/
CREATE USER [Scarlet] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [Scarlet]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [Scarlet]
GO
ALTER ROLE [db_datareader] ADD MEMBER [Scarlet]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [Scarlet]
GO
/****** Object:  UserDefinedTableType [dbo].[BigIds]    Script Date: 2017/12/14 18:13:15 ******/
CREATE TYPE [dbo].[BigIds] AS TABLE(
	[Id] [bigint] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[BillingImportDuplication]    Script Date: 2017/12/14 18:13:15 ******/
CREATE TYPE [dbo].[BillingImportDuplication] AS TABLE(
	[RowNumber] [int] NOT NULL,
	[CustomerId] [int] NULL,
	[BilledAt] [date] NULL,
	[BillingAmount] [numeric](18, 5) NULL,
	[TaxAmount] [numeric](18, 5) NULL,
	[DueAt] [date] NULL,
	[DepartmentId] [int] NULL,
	[DebitAccountTitleId] [int] NULL,
	[SalesAt] [date] NULL,
	[InvoiceCode] [nvarchar](100) NULL,
	[ClosingAt] [date] NULL,
	[StaffId] [int] NULL,
	[Note1] [nvarchar](100) NULL,
	[BillingCategoryId] [int] NULL,
	[CollectCategoryId] [int] NULL,
	[Price] [numeric](18, 5) NULL,
	[TaxClassId] [int] NULL,
	[Note2] [nvarchar](100) NULL,
	[Note3] [nvarchar](100) NULL,
	[Note4] [nvarchar](100) NULL,
	[CurrencyId] [int] NULL,
	PRIMARY KEY CLUSTERED 
(
	[RowNumber] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[Codes]    Script Date: 2017/12/14 18:13:15 ******/
CREATE TYPE [dbo].[Codes] AS TABLE(
	[Code] [nvarchar](50) NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[Ids]    Script Date: 2017/12/14 18:13:15 ******/
CREATE TYPE [dbo].[Ids] AS TABLE(
	[Id] [int] NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[ReceiptImportDuplication]    Script Date: 2017/12/14 18:13:15 ******/
CREATE TYPE [dbo].[ReceiptImportDuplication] AS TABLE(
	[RowNumber] [int] NOT NULL,
	[CustomerId] [int] NULL,
	[RecordedAt] [date] NULL,
	[ReceiptCategoryId] [int] NULL,
	[ReceiptAmount] [numeric](18, 5) NULL,
	[DueAt] [date] NULL,
	[Note1] [nvarchar](100) NULL,
	[SectionId] [int] NULL,
	[CurrencyId] [int] NULL,
	[Note2] [nvarchar](100) NULL,
	[Note3] [nvarchar](100) NULL,
	[Note4] [nvarchar](100) NULL,
	[PayerName] [nvarchar](140) NULL,
	[SourceBankName] [nvarchar](140) NULL,
	[SourceBranchName] [nvarchar](15) NULL,
	[BillNumber] [nvarchar](20) NULL,
	[BillBankCode] [nvarchar](4) NULL,
	[BillBranchCode] [nvarchar](3) NULL,
	[BillDrawAt] [date] NULL,
	[BillDrawer] [nvarchar](48) NULL,
	PRIMARY KEY CLUSTERED 
(
	[RowNumber] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedFunction [dbo].[GetClientKey]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetClientKey]
(@ProgramId             NVARCHAR(6)
,@ClientName            NVARCHAR(30)
,@CompanyCode           NVARCHAR(20)
,@LoginUserCode         NVARCHAR(20))
RETURNS VARBINARY(20)
AS
BEGIN
 RETURN CAST(
    HASHBYTES
        (N'SHA1'
        ,CAST(@ProgramId + @ClientName + @CompanyCode + @LoginUserCode AS NVARCHAR)
        ) AS VARBINARY(20));
END


GO
/****** Object:  Table [dbo].[AccountTitle]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountTitle](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[ContraAccountCode] [nvarchar](20) NOT NULL,
	[ContraAccountName] [nvarchar](40) NOT NULL,
	[ContraAccountSubCode] [nvarchar](20) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkAccountTitle] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqAccountTitle] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AccountTransferDetail]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountTransferDetail](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountTransferLogId] [bigint] NOT NULL,
	[BillingId] [bigint] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[StaffId] [int] NOT NULL,
	[BilledAt] [date] NULL,
	[SalesAt] [date] NULL,
	[ClosingAt] [date] NULL,
	[DueAt] [date] NULL,
	[BillingAmount] [numeric](18, 5) NOT NULL,
	[InvoiceCode] [nvarchar](100) NOT NULL,
	[Note1] [nvarchar](100) NOT NULL,
	[TransferBankCode] [nvarchar](4) NOT NULL,
	[TransferBankName] [nvarchar](30) NOT NULL,
	[TransferBranchCode] [nvarchar](3) NOT NULL,
	[TransferBranchName] [nvarchar](30) NOT NULL,
	[TransferAccountTypeId] [int] NOT NULL,
	[TransferAccountNumber] [nvarchar](10) NOT NULL,
	[TransferAccountName] [nvarchar](30) NOT NULL,
	[TransferCustomerCode] [nvarchar](20) NOT NULL,
	[TransferNewCode] [nvarchar](1) NOT NULL,
 CONSTRAINT [PkAccountTransferDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AccountTransferLog]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountTransferLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CollectCategoryId] [int] NOT NULL,
	[PaymentAgencyId] [int] NOT NULL,
	[RequestDate] [datetime2](3) NOT NULL,
	[DueAt] [date] NOT NULL,
	[OutputCount] [int] NOT NULL,
	[OutputAmount] [numeric](18, 5) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkAccountTransferLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AdvanceReceivedBackup]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdvanceReceivedBackup](
	[Id] [bigint] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[ReceiptHeaderId] [bigint] NULL,
	[ReceiptCategoryId] [int] NOT NULL,
	[CustomerId] [int] NULL,
	[SectionId] [int] NULL,
	[InputType] [int] NOT NULL,
	[Apportioned] [int] NOT NULL,
	[Approved] [int] NOT NULL,
	[Workday] [date] NOT NULL,
	[RecordedAt] [date] NOT NULL,
	[OriginalRecordedAt] [date] NULL,
	[ReceiptAmount] [numeric](18, 5) NOT NULL,
	[AssignmentAmount] [numeric](18, 5) NOT NULL,
	[RemainAmount] [numeric](18, 5) NOT NULL,
	[AssignmentFlag] [int] NOT NULL,
	[PayerCode] [nvarchar](10) NOT NULL,
	[PayerName] [nvarchar](140) NOT NULL,
	[PayerNameRaw] [nvarchar](140) NOT NULL,
	[SourceBankName] [nvarchar](140) NOT NULL,
	[SourceBranchName] [nvarchar](15) NOT NULL,
	[OutputAt] [datetime2](3) NULL,
	[DueAt] [date] NULL,
	[MailedAt] [date] NULL,
	[OriginalReceiptId] [bigint] NULL,
	[ExcludeFlag] [int] NOT NULL,
	[ExcludeCategoryId] [int] NULL,
	[ExcludeAmount] [numeric](18, 5) NOT NULL,
	[ReferenceNumber] [nvarchar](20) NOT NULL,
	[RecordNumber] [nvarchar](40) NOT NULL,
	[DensaiRegisterAt] [date] NULL,
	[Note1] [nvarchar](140) NOT NULL,
	[Note2] [nvarchar](140) NOT NULL,
	[Note3] [nvarchar](140) NOT NULL,
	[Note4] [nvarchar](140) NOT NULL,
	[BillNumber] [nvarchar](20) NOT NULL,
	[BillBankCode] [nvarchar](4) NOT NULL,
	[BillBranchCode] [nvarchar](3) NOT NULL,
	[BillDrawAt] [date] NULL,
	[BillDrawer] [nvarchar](48) NOT NULL,
	[DeleteAt] [datetime2](3) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[Memo] [nvarchar](max) NOT NULL,
	[TransferOutputAt] [datetime2](3) NULL,
	[StaffId] [int] NULL,
 CONSTRAINT [PK_AdvanceReceivedBackup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationControl]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationControl](
	[CompanyId] [int] NOT NULL,
	[DepartmentCodeLength] [int] NOT NULL,
	[DepartmentCodeType] [int] NOT NULL,
	[SectionCodeLength] [int] NOT NULL,
	[SectionCodeType] [int] NOT NULL,
	[AccountTitleCodeLength] [int] NOT NULL,
	[AccountTitleCodeType] [int] NOT NULL,
	[CustomerCodeLength] [int] NOT NULL,
	[CustomerCodeType] [int] NOT NULL,
	[LoginUserCodeLength] [int] NOT NULL,
	[LoginUserCodeType] [int] NOT NULL,
	[StaffCodeLength] [int] NOT NULL,
	[StaffCodeType] [int] NOT NULL,
	[UseDepartment] [int] NOT NULL,
	[UseScheduledPayment] [int] NOT NULL,
	[UseReceiptSection] [int] NOT NULL,
	[UseAuthorization] [int] NOT NULL,
	[UseLongTermAdvanceReceived] [int] NOT NULL,
	[RegisterContractInAdvance] [int] NOT NULL,
	[UseCashOnDueDates] [int] NOT NULL,
	[UseDeclaredAmount] [int] NOT NULL,
	[UseDiscount] [int] NOT NULL,
	[UseForeignCurrency] [int] NOT NULL,
	[UseBillingFilter] [int] NOT NULL,
	[UseDistribution] [int] NOT NULL,
	[UseOperationLogging] [int] NOT NULL,
	[ApplicationEdition] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
	[LimitAccessFolder] [int] NOT NULL DEFAULT ((0)),
	[RootPath] [nvarchar](50) NOT NULL DEFAULT (N''),
 CONSTRAINT [PkApplicatoinControl] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BankAccount]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankAccount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[BankCode] [nvarchar](4) NOT NULL,
	[BranchCode] [nvarchar](3) NOT NULL,
	[AccountTypeId] [int] NOT NULL,
	[AccountNumber] [nvarchar](7) NOT NULL,
	[BankName] [nvarchar](80) NOT NULL,
	[BranchName] [nvarchar](80) NOT NULL,
	[ReceiptCategoryId] [int] NULL,
	[SectionId] [int] NULL,
	[UseValueDate] [int] NOT NULL,
	[ImportSkipping] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkBankAccount] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqBankAccount] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[BankCode] ASC,
	[BranchCode] ASC,
	[AccountTypeId] ASC,
	[AccountNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BankAccountType]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankAccountType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[UseReceipt] [int] NOT NULL,
	[UseTransfer] [int] NOT NULL,
 CONSTRAINT [PkBankAccountType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BankBranch]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankBranch](
	[CompanyId] [int] NOT NULL,
	[BankCode] [nvarchar](4) NOT NULL,
	[BranchCode] [nvarchar](3) NOT NULL,
	[BankName] [nvarchar](120) NOT NULL,
	[BankKana] [nvarchar](120) NOT NULL,
	[BranchName] [nvarchar](120) NOT NULL,
	[BranchKana] [nvarchar](120) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkBankBranch] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[BankCode] ASC,
	[BranchCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Billing]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Billing](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[StaffId] [int] NOT NULL,
	[BillingCategoryId] [int] NOT NULL,
	[InputType] [int] NOT NULL,
	[BillingInputId] [bigint] NULL,
	[BilledAt] [date] NOT NULL,
	[ClosingAt] [date] NOT NULL,
	[SalesAt] [date] NOT NULL,
	[DueAt] [date] NOT NULL,
	[BillingAmount] [numeric](18, 5) NOT NULL,
	[TaxAmount] [numeric](18, 5) NOT NULL,
	[AssignmentAmount] [numeric](18, 5) NOT NULL,
	[RemainAmount] [numeric](18, 5) NOT NULL,
	[OffsetAmount] [numeric](18, 5) NOT NULL,
	[AssignmentFlag] [int] NOT NULL,
	[Approved] [int] NOT NULL,
	[CollectCategoryId] [int] NOT NULL,
	[OriginalCollectCategoryId] [int] NULL,
	[DebitAccountTitleId] [int] NULL,
	[CreditAccountTitleId] [int] NULL,
	[OriginalDueAt] [date] NULL,
	[OutputAt] [datetime2](3) NULL,
	[PublishAt] [datetime2](3) NULL,
	[InvoiceCode] [nvarchar](100) NOT NULL,
	[TaxClassId] [int] NOT NULL,
	[Note1] [nvarchar](100) NOT NULL,
	[Note2] [nvarchar](100) NOT NULL,
	[Note3] [nvarchar](100) NOT NULL,
	[Note4] [nvarchar](100) NOT NULL,
	[DeleteAt] [datetime2](3) NULL,
	[RequestDate] [datetime2](3) NULL,
	[ResultCode] [int] NULL,
	[TransferOriginalDueAt] [date] NULL,
	[ScheduledPaymentKey] [nvarchar](100) NOT NULL,
	[Quantity] [numeric](7, 2) NULL,
	[UnitPrice] [numeric](18, 5) NULL,
	[UnitSymbol] [nvarchar](3) NULL,
	[Price] [numeric](18, 5) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
	[AccountTransferLogId] [bigint] NULL,
 CONSTRAINT [PkBilling] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqBilling] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[CurrencyId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillingBalance]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingBalance](
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[StaffId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[CarryOverAt] [date] NOT NULL,
	[BalanceCarriedOver] [numeric](18, 5) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
 CONSTRAINT [PkBillingBalance] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[CurrencyId] ASC,
	[CustomerId] ASC,
	[StaffId] ASC,
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillingBalanceBack]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingBalanceBack](
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[StaffId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[CarryOverAt] [date] NOT NULL,
	[BalanceCarriedOver] [numeric](18, 5) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
 CONSTRAINT [PkBillingBalanceBack] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[CurrencyId] ASC,
	[CustomerId] ASC,
	[StaffId] ASC,
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillingDiscount]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingDiscount](
	[BillingId] [bigint] NOT NULL,
	[DiscountType] [int] NOT NULL,
	[DiscountAmount] [numeric](18, 5) NOT NULL,
	[AssignmentFlag] [int] NOT NULL,
 CONSTRAINT [PkBillingDiscount] PRIMARY KEY CLUSTERED 
(
	[BillingId] ASC,
	[DiscountType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillingDivision]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingDivision](
	[BillingDivisionContractId] [int] NOT NULL,
	[DivisionId] [int] NOT NULL,
	[DivisionAmount] [numeric](18, 5) NOT NULL,
	[TaxClassId] [int] NOT NULL,
	[DebitAccountTitleId] [int] NULL,
	[RecordedAt] [date] NOT NULL,
	[OutputAt] [datetime2](3) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkBillingDivision] PRIMARY KEY CLUSTERED 
(
	[BillingDivisionContractId] ASC,
	[DivisionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillingDivisionContract]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingDivisionContract](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[ContractNumber] [nvarchar](40) NOT NULL,
	[FirstDateType] [int] NOT NULL,
	[Monthly] [int] NOT NULL,
	[BasisDay] [int] NOT NULL,
	[DivisionCount] [int] NOT NULL,
	[RoundingMode] [int] NOT NULL,
	[RemainsApportionment] [int] NOT NULL,
	[BillingId] [bigint] NULL,
	[BillingAmount] [numeric](18, 5) NOT NULL,
	[Comfirm] [int] NOT NULL,
	[CancelDate] [date] NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkBillingDivisionContract] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqBillingDivisionContract] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[CustomerId] ASC,
	[ContractNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillingDivisionSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingDivisionSetting](
	[CompanyId] [int] NOT NULL,
	[FirstDateType] [int] NOT NULL,
	[Monthly] [int] NOT NULL,
	[BasisDay] [int] NOT NULL,
	[DivisionCount] [int] NOT NULL,
	[RoundingMode] [int] NOT NULL,
	[RemainsApportionment] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkBillingDivisionSetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillingInput]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingInput](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PkBillingInput] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillingMemo]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingMemo](
	[BillingId] [bigint] NOT NULL,
	[Memo] [nvarchar](max) NOT NULL,
 CONSTRAINT [PkBillingMemo] PRIMARY KEY CLUSTERED 
(
	[BillingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillingScheduledIncome]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingScheduledIncome](
	[BillingId] [bigint] NOT NULL,
	[MatchingId] [bigint] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkBillingScheduledIncome] PRIMARY KEY CLUSTERED 
(
	[BillingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CategoryType] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[AccountTitleId] [int] NULL,
	[SubCode] [nvarchar](20) NOT NULL,
	[Note] [nvarchar](100) NOT NULL,
	[TaxClassId] [int] NULL,
	[UseLimitDate] [int] NOT NULL,
	[UseLongTermAdvanceReceived] [int] NOT NULL,
	[UseCashOnDueDates] [int] NOT NULL,
	[UseAccountTransfer] [int] NOT NULL,
	[PaymentAgencyId] [int] NULL,
	[UseDiscount] [int] NOT NULL,
	[UseAdvanceReceived] [int] NOT NULL,
	[ForceMatchingIndividually] [int] NOT NULL,
	[UseInput] [int] NOT NULL,
	[MatchingOrder] [int] NOT NULL,
	[StringValue1] [nvarchar](50) NULL,
	[StringValue2] [nvarchar](50) NULL,
	[StringValue3] [nvarchar](50) NULL,
	[StringValue4] [nvarchar](50) NULL,
	[StringValue5] [nvarchar](50) NULL,
	[IntValue1] [int] NULL,
	[IntValue2] [int] NULL,
	[IntValue3] [int] NULL,
	[IntValue4] [int] NULL,
	[IntValue5] [int] NULL,
	[NumericValue1] [numeric](18, 5) NULL,
	[NumericValue2] [numeric](18, 5) NULL,
	[NumericValue3] [numeric](18, 5) NULL,
	[NumericValue4] [numeric](18, 5) NULL,
	[NumericValue5] [numeric](18, 5) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
	[ExternalCode] [nvarchar](20) NOT NULL CONSTRAINT [DF_Category_ExternalCode]  DEFAULT (''),
 CONSTRAINT [PkCategory] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqCategory] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[CategoryType] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CategoryBase]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryBase](
	[CategoryType] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[TaxClassId] [int] NULL,
	[UseLimitDate] [int] NOT NULL,
	[UseAdvanceReceived] [int] NOT NULL,
	[UseInput] [int] NOT NULL,
 CONSTRAINT [PK_CategoryBase] PRIMARY KEY CLUSTERED 
(
	[CategoryType] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CollationOrder]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollationOrder](
	[CompanyId] [int] NOT NULL,
	[CollationTypeId] [int] NOT NULL,
	[ExecutionOrder] [int] NOT NULL,
	[Available] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkCollationOrder] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[CollationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CollationSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollationSetting](
	[CompanyId] [int] NOT NULL,
	[RequiredCustomer] [int] NOT NULL,
	[AutoAssignCustomer] [int] NOT NULL,
	[LearnKanaHistory] [int] NOT NULL,
	[UseApportionMenu] [int] NOT NULL,
	[ReloadCollationData] [int] NOT NULL,
	[UseAdvanceReceived] [int] NOT NULL,
	[AdvanceReceivedRecordedDateType] [int] NOT NULL,
	[AutoMatching] [int] NOT NULL,
	[AutoSortMatchingEnabledData] [int] NOT NULL,
	[PrioritizeMatchingIndividuallyMultipleReceipts] [int] NOT NULL,
	[ForceShareTransferFee] [int] NOT NULL,
	[LearnSpecifiedCustomerKana] [int] NOT NULL,
	[MatchingSilentSortedData] [int] NOT NULL,
	[BillingReceiptDisplayOrder] [int] NOT NULL,
	[RemoveSpaceFromPayerName] [int] NOT NULL DEFAULT ((0)),
	[PrioritizeMatchingIndividuallyTaxTolerance] [int] NOT NULL DEFAULT ((0)),
	[JournalizingPattern] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PkCollationSetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ColumnNameSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ColumnNameSetting](
	[CompanyId] [int] NOT NULL,
	[TableName] [nvarchar](50) NOT NULL,
	[ColumnName] [nvarchar](50) NOT NULL,
	[OriginalName] [nvarchar](50) NOT NULL,
	[Alias] [nvarchar](50) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkColumnName] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[TableName] ASC,
	[ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Company]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](140) NOT NULL,
	[Kana] [nvarchar](140) NOT NULL,
	[PostalCode] [nvarchar](10) NOT NULL,
	[Address1] [nvarchar](80) NOT NULL,
	[Address2] [nvarchar](80) NOT NULL,
	[Tel] [nvarchar](20) NOT NULL,
	[Fax] [nvarchar](20) NOT NULL,
	[ProductKey] [nvarchar](40) NOT NULL,
	[BankAccountName] [nvarchar](48) NOT NULL,
	[BankAccountKana] [nvarchar](48) NOT NULL,
	[BankName1] [nvarchar](80) NOT NULL,
	[BranchName1] [nvarchar](80) NOT NULL,
	[AccountType1] [nvarchar](10) NOT NULL,
	[AccountNumber1] [nvarchar](7) NOT NULL,
	[BankName2] [nvarchar](80) NOT NULL,
	[BranchName2] [nvarchar](80) NOT NULL,
	[AccountType2] [nvarchar](10) NOT NULL,
	[AccountNumber2] [nvarchar](7) NOT NULL,
	[BankName3] [nvarchar](80) NOT NULL,
	[BranchName3] [nvarchar](80) NOT NULL,
	[AccountType3] [nvarchar](10) NOT NULL,
	[AccountNumber3] [nvarchar](7) NOT NULL,
	[ClosingDay] [int] NOT NULL,
	[ShowConfirmDialog] [int] NOT NULL,
	[PresetCodeSearchDialog] [int] NOT NULL,
	[ShowWarningDialog] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
	[TransferAggregate] [int] NOT NULL DEFAULT ((0)),
	[AutoCloseProgressDialog] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PkCompany] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqCompany] UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompanyLogo]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CompanyLogo](
	[CompanyId] [int] NOT NULL,
	[Logo] [varbinary](max) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkCompanyLogo] PRIMARY KEY NONCLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CompanySetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanySetting](
	[CompanyId] [int] NOT NULL,
	[KeyId] [int] NOT NULL,
	[Value] [nvarchar](100) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkCompanySetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[KeyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ControlColor]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ControlColor](
	[CompanyId] [int] NOT NULL,
	[LoginUserId] [int] NOT NULL,
	[FormBackColor] [int] NOT NULL,
	[FormForeColor] [int] NOT NULL,
	[ControlEnableBackColor] [int] NOT NULL,
	[ControlDisableBackColor] [int] NOT NULL,
	[ControlForeColor] [int] NOT NULL,
	[ControlRequiredBackColor] [int] NOT NULL,
	[ControlActiveBackColor] [int] NOT NULL,
	[ButtonBackColor] [int] NOT NULL,
	[GridRowBackColor] [int] NOT NULL,
	[GridAlternatingRowBackColor] [int] NOT NULL,
	[GridLineColor] [int] NOT NULL,
	[InputGridBackColor] [int] NOT NULL,
	[InputGridAlternatingBackColor] [int] NOT NULL,
	[MatchingGridBillingBackColor] [int] NOT NULL,
	[MatchingGridReceiptBackColor] [int] NOT NULL,
	[MatchingGridBillingSelectedRowBackColor] [int] NOT NULL,
	[MatchingGridBillingSelectedCellBackColor] [int] NOT NULL,
	[MatchingGridReceiptSelectedRowBackColor] [int] NOT NULL,
	[MatchingGridReceiptSelectedCellBackColor] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkControlColor] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[LoginUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Currency]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nvarchar](3) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[Symbol] [nvarchar](1) NOT NULL,
	[Precision] [int] NOT NULL,
	[Note] [nvarchar](100) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[Tolerance] [numeric](18, 5) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkCurrency] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqCurrency] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](140) NOT NULL,
	[Kana] [nvarchar](140) NOT NULL,
	[PostalCode] [nvarchar](10) NOT NULL,
	[Address1] [nvarchar](40) NOT NULL,
	[Address2] [nvarchar](40) NOT NULL,
	[Tel] [nvarchar](20) NOT NULL,
	[Fax] [nvarchar](20) NOT NULL,
	[CustomerStaffName] [nvarchar](40) NOT NULL,
	[ExclusiveBankCode] [nvarchar](4) NOT NULL,
	[ExclusiveBankName] [nvarchar](30) NOT NULL,
	[ExclusiveBranchCode] [nvarchar](3) NOT NULL,
	[ExclusiveBranchName] [nvarchar](30) NOT NULL,
	[ExclusiveAccountNumber] [nvarchar](10) NOT NULL,
	[ExclusiveAccountTypeId] [int] NULL,
	[ShareTransferFee] [int] NOT NULL,
	[CreditLimit] [numeric](18, 5) NOT NULL,
	[ClosingDay] [int] NOT NULL,
	[CollectCategoryId] [int] NOT NULL,
	[CollectOffsetMonth] [int] NOT NULL,
	[CollectOffsetDay] [int] NOT NULL,
	[StaffId] [int] NOT NULL,
	[IsParent] [int] NOT NULL,
	[Note] [nvarchar](100) NOT NULL,
	[SightOfBill] [int] NULL,
	[DensaiCode] [nvarchar](10) NOT NULL,
	[CreditCode] [nvarchar](15) NOT NULL,
	[CreditRank] [nvarchar](2) NOT NULL,
	[TransferBankCode] [nvarchar](4) NOT NULL,
	[TransferBankName] [nvarchar](30) NOT NULL,
	[TransferBranchCode] [nvarchar](3) NOT NULL,
	[TransferBranchName] [nvarchar](30) NOT NULL,
	[TransferAccountNumber] [nvarchar](10) NOT NULL,
	[TransferAccountTypeId] [int] NULL,
	[ReceiveAccountId1] [int] NOT NULL,
	[ReceiveAccountId2] [int] NOT NULL,
	[ReceiveAccountId3] [int] NOT NULL,
	[UseFeeLearning] [int] NOT NULL,
	[UseKanaLearning] [int] NOT NULL,
	[HolidayFlag] [int] NOT NULL,
	[UseFeeTolerance] [int] NOT NULL,
	[StringValue1] [nvarchar](50) NULL,
	[StringValue2] [nvarchar](50) NULL,
	[StringValue3] [nvarchar](50) NULL,
	[StringValue4] [nvarchar](50) NULL,
	[StringValue5] [nvarchar](50) NULL,
	[IntValue1] [int] NULL,
	[IntValue2] [int] NULL,
	[IntValue3] [int] NULL,
	[IntValue4] [int] NULL,
	[IntValue5] [int] NULL,
	[NumericValue1] [numeric](18, 5) NULL,
	[NumericValue2] [numeric](18, 5) NULL,
	[NumericValue3] [numeric](18, 5) NULL,
	[NumericValue4] [numeric](18, 5) NULL,
	[NumericValue5] [numeric](18, 5) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
	[TransferCustomerCode] [nvarchar](20) NOT NULL DEFAULT (N''),
	[TransferNewCode] [nvarchar](1) NOT NULL DEFAULT (N''),
	[TransferAccountName] [nvarchar](30) NOT NULL DEFAULT (N''),
	[PrioritizeMatchingIndividually] [int] NOT NULL DEFAULT ((0)),
	[CollationKey] [nvarchar](48) NOT NULL DEFAULT (N''),
 CONSTRAINT [PkCustomer] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqCustomer] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerDiscount]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerDiscount](
	[CustomerId] [int] NOT NULL,
	[Sequence] [int] NOT NULL,
	[Rate] [numeric](5, 2) NOT NULL,
	[RoundingMode] [int] NOT NULL,
	[MinValue] [numeric](18, 5) NOT NULL,
	[DepartmentId] [int] NULL,
	[AccountTitleId] [int] NULL,
	[SubCode] [nvarchar](20) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkCustomerDiscount] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[Sequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerFee]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerFee](
	[CustomerId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[Fee] [numeric](18, 5) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkCustomerFee] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[CurrencyId] ASC,
	[Fee] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerGroup]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerGroup](
	[ParentCustomerId] [int] NOT NULL,
	[ChildCustomerId] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkCustomerGroup] PRIMARY KEY NONCLUSTERED 
(
	[ParentCustomerId] ASC,
	[ChildCustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerPaymentContract]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerPaymentContract](
	[CustomerId] [int] NOT NULL,
	[ThresholdValue] [numeric](18, 5) NOT NULL,
	[LessThanCollectCategoryId] [int] NOT NULL,
	[GreaterThanCollectCategoryId1] [int] NOT NULL,
	[GreaterThanRate1] [numeric](4, 1) NOT NULL,
	[GreaterThanRoundingMode1] [int] NOT NULL,
	[GreaterThanSightOfBill1] [int] NOT NULL,
	[GreaterThanCollectCategoryId2] [int] NULL,
	[GreaterThanRate2] [numeric](4, 1) NULL,
	[GreaterThanRoundingMode2] [int] NULL,
	[GreaterThanSightOfBill2] [int] NULL,
	[GreaterThanCollectCategoryId3] [int] NULL,
	[GreaterThanRate3] [numeric](4, 1) NULL,
	[GreaterThanRoundingMode3] [int] NULL,
	[GreaterThanSightOfBill3] [int] NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkCustomerPaymentContract] PRIMARY KEY NONCLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DensaiRemoveWord]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DensaiRemoveWord](
	[CompanyId] [int] NOT NULL,
	[Word] [nvarchar](48) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkDensaiRemoveWord] PRIMARY KEY NONCLUSTERED 
(
	[CompanyId] ASC,
	[Word] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DensaiRemoveWordBase]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DensaiRemoveWordBase](
	[Word] [nvarchar](48) NOT NULL,
 CONSTRAINT [PkDensaiRemoveWordBase] PRIMARY KEY CLUSTERED 
(
	[Word] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Department]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[StaffId] [int] NULL,
	[Note] [nvarchar](100) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkDepartment] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqDepartment] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EBExcludeAccountSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EBExcludeAccountSetting](
	[CompanyId] [int] NOT NULL,
	[BankCode] [nchar](4) NOT NULL,
	[BranchCode] [nchar](3) NOT NULL,
	[AccountTypeId] [int] NOT NULL,
	[PayerCode] [nchar](10) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PK_EBExcludeAccountSetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[BankCode] ASC,
	[BranchCode] ASC,
	[AccountTypeId] ASC,
	[PayerCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EBFileSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EBFileSetting](
	[CompanyId] [int] NOT NULL,
	[LoginUserId] [int] NOT NULL,
	[IsForeignCurrency] [int] NOT NULL,
	[FileFormat] [int] NOT NULL,
	[FilePath] [nvarchar](255) NOT NULL,
 CONSTRAINT [PkEBFileSetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[LoginUserId] ASC,
	[IsForeignCurrency] ASC,
	[FileFormat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExportFieldSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExportFieldSetting](
	[CompanyId] [int] NOT NULL,
	[ExportFileType] [int] NOT NULL,
	[ColumnName] [nvarchar](50) NOT NULL,
	[ColumnOrder] [int] NOT NULL,
	[AllowExport] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
	[DataFormat] [int] NOT NULL CONSTRAINT [DF_ExportFieldSetting_DataFormat]  DEFAULT ((0)),
 CONSTRAINT [PkExportFiledSetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[ExportFileType] ASC,
	[ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExportFieldSettingBase]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExportFieldSettingBase](
	[ExportFileType] [int] NOT NULL,
	[ColumnName] [nvarchar](50) NOT NULL,
	[Caption] [nvarchar](50) NOT NULL,
	[ColumnOrder] [int] NOT NULL,
	[AllowExport] [int] NOT NULL,
	[DataType] [int] NOT NULL CONSTRAINT [DF_ExportFieldSettingBase_DataType]  DEFAULT ((0)),
 CONSTRAINT [PkExportFiledSettingBase] PRIMARY KEY CLUSTERED 
(
	[ExportFileType] ASC,
	[ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FunctionAuthority]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FunctionAuthority](
	[CompanyId] [int] NOT NULL,
	[AuthorityLevel] [int] NOT NULL,
	[FunctionType] [int] NOT NULL,
	[Available] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkFunctionAuthority] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[AuthorityLevel] ASC,
	[FunctionType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GeneralSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](200) NOT NULL,
	[Length] [int] NOT NULL,
	[Precision] [int] NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkGeneralSetting] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqGeneralSetting] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GeneralSettingBase]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralSettingBase](
	[Code] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](200) NOT NULL,
	[Length] [int] NOT NULL,
	[Precision] [int] NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PkGeneralSettingBase] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GridSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GridSetting](
	[CompanyId] [int] NOT NULL,
	[LoginUserId] [int] NOT NULL,
	[GridId] [int] NOT NULL,
	[ColumnName] [nvarchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[DisplayWidth] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkGridSetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[LoginUserId] ASC,
	[GridId] ASC,
	[ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GridSettingBase]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GridSettingBase](
	[GridId] [int] NOT NULL,
	[ColumnName] [nvarchar](50) NOT NULL,
	[ColumnNameJp] [nvarchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[DisplayWidth] [int] NOT NULL,
 CONSTRAINT [PkGridSettingBase] PRIMARY KEY CLUSTERED 
(
	[GridId] ASC,
	[ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HolidayCalendar]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HolidayCalendar](
	[CompanyId] [int] NOT NULL,
	[Holiday] [date] NOT NULL,
 CONSTRAINT [PkHolidayCalendar] PRIMARY KEY NONCLUSTERED 
(
	[CompanyId] ASC,
	[Holiday] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IgnoreKana]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IgnoreKana](
	[CompanyId] [int] NOT NULL,
	[Kana] [nvarchar](140) NOT NULL,
	[ExcludeCategoryId] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkIgnoreKana] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[Kana] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImporterFormat]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImporterFormat](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
 CONSTRAINT [PkImporterFormat] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImporterSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImporterSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[FormatId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[InitialDirectory] [nvarchar](255) NOT NULL,
	[EncodingCodePage] [int] NOT NULL,
	[StartLineCount] [int] NOT NULL,
	[IgnoreLastLine] [int] NOT NULL,
	[AutoCreationCustomer] [int] NOT NULL,
	[PostAction] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkImporterSetting] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqImporterSetting] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[FormatId] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImporterSettingBase]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImporterSettingBase](
	[FormatId] [int] NOT NULL,
	[Sequence] [int] NOT NULL,
	[FieldName] [nvarchar](50) NOT NULL,
	[TargetColumn] [nvarchar](50) NOT NULL,
	[ImportDivision] [int] NOT NULL,
	[AttributeDivision] [int] NOT NULL,
 CONSTRAINT [PkImporterSettingBase] PRIMARY KEY NONCLUSTERED 
(
	[FormatId] ASC,
	[Sequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImporterSettingDetail]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImporterSettingDetail](
	[ImporterSettingId] [int] NOT NULL,
	[Sequence] [int] NOT NULL,
	[ImportDivision] [int] NOT NULL,
	[FieldIndex] [int] NOT NULL,
	[Caption] [nvarchar](50) NOT NULL,
	[AttributeDivision] [int] NOT NULL,
	[ItemPriority] [int] NOT NULL,
	[DoOverwrite] [int] NOT NULL,
	[IsUnique] [int] NOT NULL,
	[FixedValue] [nvarchar](50) NOT NULL,
	[UpdateKey] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkImporterSettingDetail] PRIMARY KEY CLUSTERED 
(
	[ImporterSettingId] ASC,
	[Sequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImportFileLog]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportFileLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[FileSize] [int] NOT NULL,
	[ReadCount] [int] NOT NULL,
	[ImportCount] [int] NOT NULL,
	[ImportAmount] [numeric](18, 5) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkImportFileLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InputControl]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InputControl](
	[CompanyId] [int] NOT NULL,
	[LoginUserId] [int] NOT NULL,
	[InputGridTypeId] [int] NOT NULL,
	[ColumnName] [nvarchar](50) NOT NULL,
	[ColumnNameJp] [nvarchar](50) NOT NULL,
	[ColumnOrder] [int] NOT NULL,
	[TabStop] [int] NOT NULL,
	[TabIndex] [int] NOT NULL,
 CONSTRAINT [PkInputControl] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[LoginUserId] ASC,
	[InputGridTypeId] ASC,
	[ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JuridicalPersonality]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuridicalPersonality](
	[CompanyId] [int] NOT NULL,
	[Kana] [nvarchar](48) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkJuridicalPersonality] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[Kana] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JuridicalPersonalityBase]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuridicalPersonalityBase](
	[Kana] [nvarchar](48) NOT NULL,
 CONSTRAINT [PkJuridicalPersonalityBase] PRIMARY KEY NONCLUSTERED 
(
	[Kana] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KanaHistoryCustomer]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KanaHistoryCustomer](
	[CompanyId] [int] NOT NULL,
	[PayerName] [nvarchar](140) NOT NULL,
	[SourceBankName] [nvarchar](140) NOT NULL,
	[SourceBranchName] [nvarchar](15) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[HitCount] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkKanaHistoryCustomer] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[PayerName] ASC,
	[SourceBankName] ASC,
	[SourceBranchName] ASC,
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KanaHistoryPaymentAgency]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KanaHistoryPaymentAgency](
	[CompanyId] [int] NOT NULL,
	[PayerName] [nvarchar](140) NOT NULL,
	[SourceBankName] [nvarchar](140) NOT NULL,
	[SourceBranchName] [nvarchar](15) NOT NULL,
	[PaymentAgencyId] [int] NOT NULL,
	[HitCount] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkKanaHistoryPaymentAgency] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[PayerName] ASC,
	[SourceBankName] ASC,
	[SourceBranchName] ASC,
	[PaymentAgencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LogData]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ClientName] [nvarchar](100) NOT NULL,
	[LoggedAt] [datetime2](3) NOT NULL,
	[LoginUserCode] [nvarchar](20) NOT NULL,
	[LoginUserName] [nvarchar](100) NOT NULL,
	[MenuId] [int] NULL,
	[MenuName] [nvarchar](100) NOT NULL,
	[OperationName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PkLogData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LoginUser]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[Mail] [nvarchar](254) NOT NULL,
	[MenuLevel] [int] NOT NULL,
	[FunctionLevel] [int] NOT NULL,
	[UseClient] [int] NOT NULL,
	[UseWebViewer] [int] NOT NULL,
	[AssignedStaffId] [int] NULL,
	[StringValue1] [nvarchar](50) NOT NULL,
	[StringValue2] [nvarchar](50) NOT NULL,
	[StringValue3] [nvarchar](50) NOT NULL,
	[StringValue4] [nvarchar](50) NOT NULL,
	[StringValue5] [nvarchar](50) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkLoginUser] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqLoginUser] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LoginUserLicense]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginUserLicense](
	[CompanyId] [int] NOT NULL,
	[LicenseKey] [nvarchar](200) NOT NULL,
 CONSTRAINT [PkLoginUserLicense] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[LicenseKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LoginUserPassword]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginUserPassword](
	[LoginUserId] [int] NOT NULL,
	[PasswordHash] [nvarchar](200) NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
	[PasswordHash0] [nvarchar](200) NOT NULL,
	[PasswordHash1] [nvarchar](200) NOT NULL,
	[PasswordHash2] [nvarchar](200) NOT NULL,
	[PasswordHash3] [nvarchar](200) NOT NULL,
	[PasswordHash4] [nvarchar](200) NOT NULL,
	[PasswordHash5] [nvarchar](200) NOT NULL,
	[PasswordHash6] [nvarchar](200) NOT NULL,
	[PasswordHash7] [nvarchar](200) NOT NULL,
	[PasswordHash8] [nvarchar](200) NOT NULL,
	[PasswordHash9] [nvarchar](200) NOT NULL,
 CONSTRAINT [PkLoginUserPassword] PRIMARY KEY CLUSTERED 
(
	[LoginUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MailSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailSetting](
	[CompanyId] [int] NOT NULL,
	[SmtpServerName] [nvarchar](100) NOT NULL,
	[SmtpPortNumber] [int] NOT NULL,
	[SmtpUserName] [nvarchar](200) NOT NULL,
	[SmtpPassword] [nvarchar](200) NOT NULL,
	[UseSmtpAuthentication] [int] NOT NULL,
	[UseSsL] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkMailSetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MailTemplate]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailTemplate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[MailType] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[Title] [nvarchar](40) NOT NULL,
	[Honorific] [nvarchar](10) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkMailTemplate] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqMailTemplate] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[MailType] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MasterImportSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterImportSetting](
	[CompanyId] [int] NOT NULL,
	[ImportFileType] [int] NOT NULL,
	[ImportFileName] [nvarchar](50) NOT NULL,
	[ImportMode] [int] NOT NULL,
	[ExportErrorLog] [int] NOT NULL,
	[ErrorLogDestination] [int] NOT NULL,
	[Confirm] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkMasterImportSetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[ImportFileType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MasterImportSettingBase]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterImportSettingBase](
	[ImportFileType] [int] NOT NULL,
	[ImportFileName] [nvarchar](50) NOT NULL,
	[ImportMode] [int] NOT NULL,
	[ExportErrorLog] [int] NOT NULL,
	[ErrorLogDestination] [int] NOT NULL,
	[Confirm] [int] NOT NULL,
 CONSTRAINT [PkMasterImportSettingBase] PRIMARY KEY CLUSTERED 
(
	[ImportFileType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Matching]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Matching](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReceiptId] [bigint] NOT NULL,
	[BillingId] [bigint] NOT NULL,
	[MatchingHeaderId] [bigint] NOT NULL,
	[BankTransferFee] [numeric](18, 5) NOT NULL,
	[Amount] [numeric](18, 5) NOT NULL,
	[BillingRemain] [numeric](18, 5) NOT NULL,
	[ReceiptRemain] [numeric](18, 5) NOT NULL,
	[AdvanceReceivedOccured] [int] NOT NULL,
	[RecordedAt] [date] NOT NULL,
	[TaxDifference] [numeric](18, 5) NOT NULL,
	[OutputAt] [datetime2](3) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkMatching] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MatchingBillingDiscount]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MatchingBillingDiscount](
	[MatchingId] [bigint] NOT NULL,
	[DiscountType] [int] NOT NULL,
	[DiscountAmount] [numeric](18, 5) NOT NULL,
 CONSTRAINT [PkMatchingBillingDiscount] PRIMARY KEY CLUSTERED 
(
	[MatchingId] ASC,
	[DiscountType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MatchingHeader]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MatchingHeader](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[CustomerId] [int] NULL,
	[PaymentAgencyId] [int] NULL,
	[Approved] [int] NOT NULL,
	[ReceiptCount] [int] NOT NULL,
	[BillingCount] [int] NOT NULL,
	[Amount] [numeric](18, 5) NOT NULL,
	[BankTransferFee] [numeric](18, 5) NOT NULL,
	[TaxDifference] [numeric](18, 5) NOT NULL,
	[MatchingProcessType] [int] NOT NULL,
	[Memo] [nvarchar](max) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkMatchingHeader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MatchingOrder]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MatchingOrder](
	[CompanyId] [int] NOT NULL,
	[TransactionCategory] [int] NOT NULL,
	[ItemName] [nvarchar](50) NOT NULL,
	[ExecutionOrder] [int] NOT NULL,
	[Available] [int] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkMatchingOrder] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[TransactionCategory] ASC,
	[ItemName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MatchingOrderBase]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MatchingOrderBase](
	[TransactionCategory] [int] NOT NULL,
	[ItemName] [nvarchar](50) NOT NULL,
	[ExecutionOrder] [int] NOT NULL,
	[Available] [int] NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_MatchingOrderBase] PRIMARY KEY CLUSTERED 
(
	[TransactionCategory] ASC,
	[ItemName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MatchingOutputed]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MatchingOutputed](
	[MatchingHeaderId] [bigint] NOT NULL,
	[OutputAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkMatchingOutputed] PRIMARY KEY CLUSTERED 
(
	[MatchingHeaderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Menu]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[MenuId] [nvarchar](50) NOT NULL,
	[MenuName] [nvarchar](40) NOT NULL,
	[MenuCategory] [nvarchar](2) NOT NULL,
	[Sequence] [int] NOT NULL,
	[IsStandard] [int] NOT NULL,
	[UseScheduledPayment] [int] NOT NULL,
	[UseReceiptSection] [int] NOT NULL,
	[UseAuthorization] [int] NOT NULL,
	[UseLongTermAdvanceReceived] [int] NOT NULL,
	[UseCashOnDueDates] [int] NOT NULL,
	[UseDiscount] [int] NOT NULL,
	[UseForeignCurrency] [int] NOT NULL,
	[NotUseForeignCurrency] [int] NOT NULL,
	[UseBillingFilter] [int] NOT NULL,
	[UseDistribution] [int] NOT NULL,
	[UseOperationLogging] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkMenu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MenuAuthority]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuAuthority](
	[CompanyId] [int] NOT NULL,
	[MenuId] [nvarchar](50) NOT NULL,
	[AuthorityLevel] [int] NOT NULL,
	[Available] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkMenuAuthority] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[MenuId] ASC,
	[AuthorityLevel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Netting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Netting](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[ReceiptCategoryId] [int] NOT NULL,
	[SectionId] [int] NULL,
	[ReceiptId] [bigint] NULL,
	[RecordedAt] [date] NOT NULL,
	[DueAt] [date] NULL,
	[Amount] [numeric](18, 5) NOT NULL,
	[AssignmentFlag] [int] NOT NULL,
	[Note] [nvarchar](140) NOT NULL,
	[ReceiptMemo] [nvarchar](140) NOT NULL,
 CONSTRAINT [PkNetting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OperationLoggingSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperationLoggingSetting](
	[CompanyId] [int] NOT NULL,
	[SettingKey] [nvarchar](16) NOT NULL,
	[Value] [nvarchar](512) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkOperationLoggingSetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[SettingKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PasswordPolicy]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PasswordPolicy](
	[CompanyId] [int] NOT NULL,
	[MinLength] [int] NOT NULL,
	[MaxLength] [int] NOT NULL,
	[UseAlphabet] [int] NOT NULL,
	[MinAlphabetUseCount] [int] NOT NULL,
	[UseNumber] [int] NOT NULL,
	[MinNumberUseCount] [int] NOT NULL,
	[UseSymbol] [int] NOT NULL,
	[MinSymbolUseCount] [int] NOT NULL,
	[SymbolType] [nvarchar](20) NOT NULL,
	[CaseSensitive] [int] NOT NULL,
	[MinSameCharacterRepeat] [int] NOT NULL,
	[ExpirationDays] [int] NOT NULL,
	[HistoryCount] [int] NOT NULL,
 CONSTRAINT [PkPasswordPolicy] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PaymentAgency]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentAgency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nvarchar](2) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[Kana] [nvarchar](48) NOT NULL,
	[ConsigneeCode] [nvarchar](20) NOT NULL,
	[ShareTransferFee] [int] NOT NULL,
	[UseFeeTolerance] [int] NOT NULL,
	[UseFeeLearning] [int] NOT NULL,
	[UseKanaLearning] [int] NOT NULL,
	[DueDateOffset] [int] NOT NULL,
	[BankCode] [nvarchar](4) NOT NULL,
	[BankName] [nvarchar](15) NOT NULL,
	[BranchCode] [nvarchar](3) NOT NULL,
	[BranchName] [nvarchar](15) NOT NULL,
	[AccountTypeId] [int] NOT NULL,
	[AccountNumber] [nvarchar](7) NOT NULL,
	[FileFormatId] [int] NOT NULL,
	[ConsiderUncollected] [int] NOT NULL,
	[CollectCategoryId] [int] NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
	[OutputFileName] [nvarchar](100) NOT NULL DEFAULT (N''),
	[AppendDate] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PkPaymentAgency] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqPaymentAgency] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PaymentAgencyFee]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentAgencyFee](
	[PaymentAgencyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[Fee] [numeric](18, 5) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkPaymentAgencyFee] PRIMARY KEY CLUSTERED 
(
	[PaymentAgencyId] ASC,
	[CurrencyId] ASC,
	[Fee] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PaymentFileFormat]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentFileFormat](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[DisplayOrder] [int] NOT NULL DEFAULT ((0)),
	[Available] [int] NOT NULL DEFAULT ((1)),
	[IsNeedYear] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PkPaymentFileFormat] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Receipt]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Receipt](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[ReceiptHeaderId] [bigint] NULL,
	[ReceiptCategoryId] [int] NOT NULL,
	[CustomerId] [int] NULL,
	[SectionId] [int] NULL,
	[InputType] [int] NOT NULL,
	[Apportioned] [int] NOT NULL,
	[Approved] [int] NOT NULL,
	[Workday] [date] NOT NULL,
	[RecordedAt] [date] NOT NULL,
	[OriginalRecordedAt] [date] NULL,
	[ReceiptAmount] [numeric](18, 5) NOT NULL,
	[AssignmentAmount] [numeric](18, 5) NOT NULL,
	[RemainAmount] [numeric](18, 5) NOT NULL,
	[AssignmentFlag] [int] NOT NULL,
	[PayerCode] [nvarchar](10) NOT NULL,
	[PayerName] [nvarchar](140) NOT NULL,
	[PayerNameRaw] [nvarchar](140) NOT NULL,
	[SourceBankName] [nvarchar](140) NOT NULL,
	[SourceBranchName] [nvarchar](15) NOT NULL,
	[OutputAt] [datetime2](3) NULL,
	[DueAt] [date] NULL,
	[MailedAt] [date] NULL,
	[OriginalReceiptId] [bigint] NULL,
	[ExcludeFlag] [int] NOT NULL,
	[ExcludeCategoryId] [int] NULL,
	[ExcludeAmount] [numeric](18, 5) NOT NULL,
	[ReferenceNumber] [nvarchar](20) NOT NULL,
	[RecordNumber] [nvarchar](40) NOT NULL,
	[DensaiRegisterAt] [date] NULL,
	[Note1] [nvarchar](140) NOT NULL,
	[Note2] [nvarchar](140) NOT NULL,
	[Note3] [nvarchar](140) NOT NULL,
	[Note4] [nvarchar](140) NOT NULL,
	[BillNumber] [nvarchar](20) NOT NULL,
	[BillBankCode] [nvarchar](4) NOT NULL,
	[BillBranchCode] [nvarchar](3) NOT NULL,
	[BillDrawAt] [date] NULL,
	[BillDrawer] [nvarchar](48) NOT NULL,
	[DeleteAt] [datetime2](3) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
	[ProcessingAt] [date] NULL,
	[StaffId] [int] NULL,
	[CollationKey] [nvarchar](48) NOT NULL DEFAULT (N''),
 CONSTRAINT [PkReceipt] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReceiptExclude]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceiptExclude](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReceiptId] [bigint] NOT NULL,
	[ExcludeAmount] [numeric](18, 5) NOT NULL,
	[ExcludeCategoryId] [int] NOT NULL,
	[OutputAt] [datetime2](3) NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkReceiptExclude] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReceiptHeader]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceiptHeader](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[FileType] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[ImportFileLogId] [int] NOT NULL,
	[Workday] [date] NOT NULL,
	[BankCode] [nvarchar](4) NOT NULL,
	[BankName] [nvarchar](30) NOT NULL,
	[BranchCode] [nvarchar](3) NOT NULL,
	[BranchName] [nvarchar](30) NOT NULL,
	[AccountTypeId] [int] NOT NULL,
	[AccountNumber] [nvarchar](7) NOT NULL,
	[AccountName] [nvarchar](30) NOT NULL,
	[AssignmentFlag] [int] NOT NULL,
	[ImportCount] [int] NOT NULL,
	[ImportAmount] [numeric](18, 5) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkReceiptHeader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReceiptMemo]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceiptMemo](
	[ReceiptId] [bigint] NOT NULL,
	[Memo] [nvarchar](max) NOT NULL,
 CONSTRAINT [PkReceiptMemo] PRIMARY KEY CLUSTERED 
(
	[ReceiptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReceiptSectionTransfer]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceiptSectionTransfer](
	[SourceReceiptId] [bigint] NOT NULL,
	[DestinationReceiptId] [bigint] NOT NULL,
	[SourceSectionId] [int] NOT NULL,
	[DestinationSectionId] [int] NOT NULL,
	[SourceAmount] [numeric](18, 5) NOT NULL,
	[DestinationAmount] [numeric](18, 5) NOT NULL,
	[PrintFlag] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkReceiptSectionTransfer] PRIMARY KEY CLUSTERED 
(
	[SourceReceiptId] ASC,
	[DestinationReceiptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReportSetting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportSetting](
	[CompanyId] [int] NOT NULL,
	[ReportId] [nvarchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[ItemId] [nvarchar](50) NOT NULL,
	[ItemKey] [nvarchar](50) NOT NULL,
 CONSTRAINT [PkReportSetting] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[ReportId] ASC,
	[DisplayOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReportSettingBase]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportSettingBase](
	[ReportId] [nvarchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[ItemId] [nvarchar](50) NOT NULL,
	[ItemKey] [nvarchar](50) NOT NULL,
	[Alias] [nvarchar](50) NULL,
	[IsText] [int] NOT NULL,
 CONSTRAINT [PkReportSettingBase] PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC,
	[DisplayOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Section]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Section](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[Note] [nvarchar](100) NOT NULL,
	[PayerCode] [nvarchar](10) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkSection] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqSection] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SectionWithDepartment]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SectionWithDepartment](
	[SectionId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkSectionWithDepartment] PRIMARY KEY CLUSTERED 
(
	[SectionId] ASC,
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SectionWithLoginUser]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SectionWithLoginUser](
	[LoginUserId] [int] NOT NULL,
	[SectionId] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkSectionWithLoginUser] PRIMARY KEY CLUSTERED 
(
	[LoginUserId] ASC,
	[SectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Setting]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[ItemId] [nvarchar](50) NOT NULL,
	[ItemKey] [nvarchar](50) NOT NULL,
	[ItemValue] [nvarchar](50) NOT NULL,
 CONSTRAINT [PkSetting] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC,
	[ItemKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SettingDefinition]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SettingDefinition](
	[ItemId] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[ItemKeyLength] [int] NOT NULL,
	[ItemValueLength] [int] NOT NULL,
 CONSTRAINT [PkSettingDefinition] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Staff]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[Mail] [nvarchar](254) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkStaff] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UqStaff] UNIQUE CLUSTERED 
(
	[CompanyId] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TaskSchedule]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskSchedule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ImportType] [int] NOT NULL,
	[ImportSubType] [int] NOT NULL,
	[Duration] [int] NOT NULL,
	[StartDate] [datetime2](3) NOT NULL,
	[Interval] [int] NOT NULL,
	[WeekDay] [binary](1) NOT NULL,
	[ImportDirectory] [nvarchar](255) NOT NULL,
	[SuccessDirectory] [nvarchar](255) NOT NULL,
	[FailedDirectory] [nvarchar](255) NOT NULL,
	[LogDestination] [int] NOT NULL,
	[TargetBillingAssignment] [int] NOT NULL,
	[BillingAmount] [int] NOT NULL,
	[UpdateSameCustomer] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
	[UpdateBy] [int] NOT NULL,
	[UpdateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkTaskSchedule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TaskScheduleHistory]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskScheduleHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[ImportType] [int] NOT NULL,
	[ImportSubType] [int] NOT NULL,
	[StartAt] [datetime2](3) NOT NULL,
	[EndAt] [datetime2](3) NOT NULL,
	[Result] [int] NOT NULL,
	[Errors] [nvarchar](255) NOT NULL,
 CONSTRAINT [PkTaskScheduleHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tax]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tax](
	[CompanyId] [int] NOT NULL,
	[ApplyDate] [date] NOT NULL,
	[Rate] [numeric](4, 2) NOT NULL,
	[CreateBy] [int] NOT NULL,
	[CreateAt] [datetime2](3) NOT NULL,
 CONSTRAINT [PkTax] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC,
	[ApplyDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TaxClass]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaxClass](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
 CONSTRAINT [PkTaxClass] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WorkBankTransfer]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WorkBankTransfer](
	[ClientKey] [varbinary](20) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[PaymentAgencyId] [int] NOT NULL,
	[PaymentAgencyKana] [nvarchar](140) NOT NULL,
	[BillingAmount] [numeric](18, 5) NOT NULL,
	[BillingRemainAmount] [numeric](18, 5) NOT NULL,
	[BillingCount] [int] NOT NULL,
 CONSTRAINT [PkWorkBankTransfer] PRIMARY KEY CLUSTERED 
(
	[ClientKey] ASC,
	[CompanyId] ASC,
	[CurrencyId] ASC,
	[PaymentAgencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkBilling]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WorkBilling](
	[ClientKey] [varbinary](20) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CustomerKana] [nvarchar](140) NOT NULL,
	[BillingAmount] [numeric](18, 5) NOT NULL,
	[BillingRemainAmount] [numeric](18, 5) NOT NULL,
	[BillingCount] [int] NOT NULL,
 CONSTRAINT [PkWorkBilling] PRIMARY KEY CLUSTERED 
(
	[ClientKey] ASC,
	[CompanyId] ASC,
	[CurrencyId] ASC,
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkBillingTarget]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WorkBillingTarget](
	[ClientKey] [varbinary](20) NOT NULL,
	[BillingId] [bigint] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[PaymentAgencyId] [int] NOT NULL,
	[BillingAmount] [numeric](18, 5) NOT NULL,
	[RemainAmount] [numeric](18, 5) NOT NULL,
 CONSTRAINT [PkWorkBillingTarget] PRIMARY KEY CLUSTERED 
(
	[ClientKey] ASC,
	[BillingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkCollation]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WorkCollation](
	[ClientKey] [varbinary](20) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[ParentCustomerId] [int] NOT NULL,
	[PaymentAgencyId] [int] NOT NULL,
	[PayerName] [nvarchar](140) NOT NULL,
	[PayerCode] [nvarchar](10) NOT NULL,
	[BankCode] [nvarchar](4) NOT NULL,
	[BranchCode] [nvarchar](3) NOT NULL,
	[SourceBankName] [nvarchar](140) NOT NULL,
	[SourceBranchName] [nvarchar](15) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CollationKey] [nvarchar](48) NOT NULL,
	[CollationType] [int] NOT NULL,
	[ParentCustomerName] [nvarchar](140) NOT NULL,
	[ParentCustomerKana] [nvarchar](140) NOT NULL,
	[ParentCustomerShareTransferFee] [int] NOT NULL,
	[BillingAmount] [numeric](18, 5) NOT NULL,
	[BillingRemainAmount] [numeric](18, 5) NOT NULL,
	[BillingCount] [int] NOT NULL,
	[ReceiptAmount] [numeric](18, 5) NOT NULL,
	[ReceiptAssignmentAmount] [numeric](18, 5) NOT NULL,
	[ReceiptRemainAmount] [numeric](18, 5) NOT NULL,
	[ReceiptCount] [int] NOT NULL,
	[AdvanceReceivedCount] [int] NOT NULL,
	[ForceMatchingIndividually] [int] NOT NULL,
 CONSTRAINT [PkWorkCollation] PRIMARY KEY CLUSTERED 
(
	[ClientKey] ASC,
	[CompanyId] ASC,
	[CurrencyId] ASC,
	[ParentCustomerId] ASC,
	[PaymentAgencyId] ASC,
	[PayerName] ASC,
	[PayerCode] ASC,
	[BankCode] ASC,
	[BranchCode] ASC,
	[SourceBankName] ASC,
	[SourceBranchName] ASC,
	[CustomerId] ASC,
	[CollationKey] ASC,
	[CollationType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkDepartmentTarget]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WorkDepartmentTarget](
	[ClientKey] [varbinary](20) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[UseCollation] [int] NOT NULL,
 CONSTRAINT [PkWorkDepartmentTarget] PRIMARY KEY CLUSTERED 
(
	[ClientKey] ASC,
	[CompanyId] ASC,
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkNettingTarget]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WorkNettingTarget](
	[ClientKey] [varbinary](20) NOT NULL,
	[NettingId] [bigint] NOT NULL,
	[CollationType] [int] NOT NULL,
 CONSTRAINT [PkWorkNettingTarget] PRIMARY KEY CLUSTERED 
(
	[ClientKey] ASC,
	[NettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkReceipt]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WorkReceipt](
	[ClientKey] [varbinary](20) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[PayerName] [nvarchar](140) NOT NULL,
	[BankCode] [nvarchar](4) NOT NULL,
	[BranchCode] [nvarchar](3) NOT NULL,
	[PayerCode] [nvarchar](10) NOT NULL,
	[SourceBankName] [nvarchar](140) NOT NULL,
	[SourceBranchName] [nvarchar](15) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CollationKey] [nvarchar](48) NOT NULL,
	[ReceiptAmount] [numeric](18, 5) NOT NULL,
	[ReceiptAssignmentAmount] [numeric](18, 5) NOT NULL,
	[ReceiptRemainAmount] [numeric](18, 5) NOT NULL,
	[ReceiptCount] [int] NOT NULL,
	[ForceMatchingIndividually] [int] NOT NULL,
	[AdvanceReceivedCount] [int] NOT NULL,
 CONSTRAINT [PkWorkReceipt] PRIMARY KEY CLUSTERED 
(
	[ClientKey] ASC,
	[CompanyId] ASC,
	[CurrencyId] ASC,
	[PayerName] ASC,
	[BankCode] ASC,
	[BranchCode] ASC,
	[PayerCode] ASC,
	[SourceBankName] ASC,
	[SourceBranchName] ASC,
	[CustomerId] ASC,
	[CollationKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkReceiptTarget]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WorkReceiptTarget](
	[ClientKey] [varbinary](20) NOT NULL,
	[ReceiptId] [bigint] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[PayerName] [nvarchar](140) NOT NULL,
	[BankCode] [nvarchar](4) NOT NULL,
	[BranchCode] [nvarchar](3) NOT NULL,
	[PayerCode] [nvarchar](10) NOT NULL,
	[SourceBankName] [nvarchar](140) NOT NULL,
	[SourceBranchName] [nvarchar](15) NOT NULL,
	[CollationKey] [nvarchar](48) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CollationType] [int] NOT NULL,
 CONSTRAINT [PkWorkReceiptTarget] PRIMARY KEY CLUSTERED 
(
	[ClientKey] ASC,
	[ReceiptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkSectionTarget]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WorkSectionTarget](
	[ClientKey] [varbinary](20) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[SectionId] [int] NOT NULL,
	[UseCollation] [int] NOT NULL,
 CONSTRAINT [PkWorkSectionTarget] PRIMARY KEY CLUSTERED 
(
	[ClientKey] ASC,
	[CompanyId] ASC,
	[SectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IdxCustomerGroupChild]    Script Date: 2017/12/14 18:13:15 ******/
CREATE NONCLUSTERED INDEX [IdxCustomerGroupChild] ON [dbo].[CustomerGroup]
(
	[ChildCustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IdxMatchingBilling]    Script Date: 2017/12/14 18:13:15 ******/
CREATE NONCLUSTERED INDEX [IdxMatchingBilling] ON [dbo].[Matching]
(
	[BillingId] ASC,
	[MatchingHeaderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IdxMatchingMatchingHeaderId]    Script Date: 2017/12/14 18:13:15 ******/
CREATE NONCLUSTERED INDEX [IdxMatchingMatchingHeaderId] ON [dbo].[Matching]
(
	[MatchingHeaderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IdxMatchingReceipt]    Script Date: 2017/12/14 18:13:15 ******/
CREATE NONCLUSTERED INDEX [IdxMatchingReceipt] ON [dbo].[Matching]
(
	[ReceiptId] ASC,
	[MatchingHeaderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IdxReceiptOriginalReceiptId]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IdxReceiptOriginalReceiptId] ON [dbo].[Receipt]
(
	[OriginalReceiptId] ASC
)
INCLUDE ( 	[Id]) 
WHERE ([OriginalReceiptId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IdxSectionWithDepartment]    Script Date: 2017/12/14 18:13:15 ******/
CREATE NONCLUSTERED INDEX [IdxSectionWithDepartment] ON [dbo].[SectionWithDepartment]
(
	[DepartmentId] ASC,
	[SectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IdxSectionWithLoginUser]    Script Date: 2017/12/14 18:13:15 ******/
CREATE NONCLUSTERED INDEX [IdxSectionWithLoginUser] ON [dbo].[SectionWithLoginUser]
(
	[LoginUserId] ASC,
	[SectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IdxWorkBillingTarget]    Script Date: 2017/12/14 18:13:15 ******/
CREATE NONCLUSTERED INDEX [IdxWorkBillingTarget] ON [dbo].[WorkBillingTarget]
(
	[ClientKey] ASC,
	[CustomerId] ASC,
	[PaymentAgencyId] ASC
)
INCLUDE ( 	[BillingId],
	[BillingAmount],
	[RemainAmount]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IdxWorkReceiptTarget]    Script Date: 2017/12/14 18:13:15 ******/
CREATE NONCLUSTERED INDEX [IdxWorkReceiptTarget] ON [dbo].[WorkReceiptTarget]
(
	[ClientKey] ASC,
	[CompanyId] ASC,
	[CurrencyId] ASC,
	[PayerName] ASC,
	[BankCode] ASC,
	[BranchCode] ASC,
	[PayerCode] ASC,
	[SourceBankName] ASC,
	[SourceBranchName] ASC,
	[CollationKey] ASC,
	[CustomerId] ASC
)
INCLUDE ( 	[ReceiptId],
	[CollationType]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AccountTransferDetail]  WITH CHECK ADD  CONSTRAINT [FkAccountTransferDetailLog] FOREIGN KEY([AccountTransferLogId])
REFERENCES [dbo].[AccountTransferLog] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AccountTransferDetail] CHECK CONSTRAINT [FkAccountTransferDetailLog]
GO
ALTER TABLE [dbo].[AccountTransferLog]  WITH CHECK ADD  CONSTRAINT [FkAccountTransferLogCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[AccountTransferLog] CHECK CONSTRAINT [FkAccountTransferLogCompany]
GO
ALTER TABLE [dbo].[ApplicationControl]  WITH CHECK ADD  CONSTRAINT [FkApplicationControlCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationControl] CHECK CONSTRAINT [FkApplicationControlCompany]
GO
ALTER TABLE [dbo].[BankAccount]  WITH CHECK ADD  CONSTRAINT [FkBankAccountCategory] FOREIGN KEY([ReceiptCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[BankAccount] CHECK CONSTRAINT [FkBankAccountCategory]
GO
ALTER TABLE [dbo].[BankAccount]  WITH CHECK ADD  CONSTRAINT [FkBankAccountCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[BankAccount] CHECK CONSTRAINT [FkBankAccountCompany]
GO
ALTER TABLE [dbo].[BankAccount]  WITH CHECK ADD  CONSTRAINT [FkBankAccountSection] FOREIGN KEY([SectionId])
REFERENCES [dbo].[Section] ([Id])
GO
ALTER TABLE [dbo].[BankAccount] CHECK CONSTRAINT [FkBankAccountSection]
GO
ALTER TABLE [dbo].[BankBranch]  WITH CHECK ADD  CONSTRAINT [FkBankBranchCategory] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[BankBranch] CHECK CONSTRAINT [FkBankBranchCategory]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingAccountTransferLog] FOREIGN KEY([AccountTransferLogId])
REFERENCES [dbo].[AccountTransferLog] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingAccountTransferLog]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingBillingCategory] FOREIGN KEY([BillingCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingBillingCategory]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingBillingInputId] FOREIGN KEY([BillingInputId])
REFERENCES [dbo].[BillingInput] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingBillingInputId]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingCollectCategory] FOREIGN KEY([CollectCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingCollectCategory]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingCompany]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingCreditAccountTitle] FOREIGN KEY([CreditAccountTitleId])
REFERENCES [dbo].[AccountTitle] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingCreditAccountTitle]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingCurrency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingCurrency]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingCustomer]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingDebitAccountTitle] FOREIGN KEY([DebitAccountTitleId])
REFERENCES [dbo].[AccountTitle] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingDebitAccountTitle]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingDepartment] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingDepartment]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingOriginalCollectCategory] FOREIGN KEY([OriginalCollectCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingOriginalCollectCategory]
GO
ALTER TABLE [dbo].[Billing]  WITH CHECK ADD  CONSTRAINT [FkBillingStaff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([Id])
GO
ALTER TABLE [dbo].[Billing] CHECK CONSTRAINT [FkBillingStaff]
GO
ALTER TABLE [dbo].[BillingDiscount]  WITH CHECK ADD  CONSTRAINT [FkBillingDiscountBilling] FOREIGN KEY([BillingId])
REFERENCES [dbo].[Billing] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BillingDiscount] CHECK CONSTRAINT [FkBillingDiscountBilling]
GO
ALTER TABLE [dbo].[BillingDivision]  WITH CHECK ADD  CONSTRAINT [FkBillingDivisionBillingDivisionContract] FOREIGN KEY([BillingDivisionContractId])
REFERENCES [dbo].[BillingDivisionContract] ([Id])
GO
ALTER TABLE [dbo].[BillingDivision] CHECK CONSTRAINT [FkBillingDivisionBillingDivisionContract]
GO
ALTER TABLE [dbo].[BillingDivisionContract]  WITH CHECK ADD  CONSTRAINT [FkBillingDivisionContractCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[BillingDivisionContract] CHECK CONSTRAINT [FkBillingDivisionContractCompany]
GO
ALTER TABLE [dbo].[BillingDivisionSetting]  WITH CHECK ADD  CONSTRAINT [FkBillingDivisionSettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[BillingDivisionSetting] CHECK CONSTRAINT [FkBillingDivisionSettingCompany]
GO
ALTER TABLE [dbo].[BillingMemo]  WITH CHECK ADD  CONSTRAINT [FkBillingMemoBilling] FOREIGN KEY([BillingId])
REFERENCES [dbo].[Billing] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BillingMemo] CHECK CONSTRAINT [FkBillingMemoBilling]
GO
ALTER TABLE [dbo].[BillingScheduledIncome]  WITH CHECK ADD  CONSTRAINT [FkBillingScheduledIncomeBilling] FOREIGN KEY([BillingId])
REFERENCES [dbo].[Billing] ([Id])
GO
ALTER TABLE [dbo].[BillingScheduledIncome] CHECK CONSTRAINT [FkBillingScheduledIncomeBilling]
GO
ALTER TABLE [dbo].[BillingScheduledIncome]  WITH CHECK ADD  CONSTRAINT [FkBillingScheduledIncomeMatching] FOREIGN KEY([MatchingId])
REFERENCES [dbo].[Matching] ([Id])
GO
ALTER TABLE [dbo].[BillingScheduledIncome] CHECK CONSTRAINT [FkBillingScheduledIncomeMatching]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FkCategoryAccountTitle] FOREIGN KEY([AccountTitleId])
REFERENCES [dbo].[AccountTitle] ([Id])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FkCategoryAccountTitle]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FkCategoryCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FkCategoryCompany]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FkCategoryPaymentAgency] FOREIGN KEY([PaymentAgencyId])
REFERENCES [dbo].[PaymentAgency] ([Id])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FkCategoryPaymentAgency]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FkCategoryTaxClass] FOREIGN KEY([TaxClassId])
REFERENCES [dbo].[TaxClass] ([Id])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FkCategoryTaxClass]
GO
ALTER TABLE [dbo].[CollationOrder]  WITH CHECK ADD  CONSTRAINT [FkCollationOrderCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[CollationOrder] CHECK CONSTRAINT [FkCollationOrderCompany]
GO
ALTER TABLE [dbo].[CollationSetting]  WITH CHECK ADD  CONSTRAINT [FkCollationSettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[CollationSetting] CHECK CONSTRAINT [FkCollationSettingCompany]
GO
ALTER TABLE [dbo].[ColumnNameSetting]  WITH CHECK ADD  CONSTRAINT [FkColumnNameCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[ColumnNameSetting] CHECK CONSTRAINT [FkColumnNameCompany]
GO
ALTER TABLE [dbo].[CompanyLogo]  WITH CHECK ADD  CONSTRAINT [FkCompnayLogo] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CompanyLogo] CHECK CONSTRAINT [FkCompnayLogo]
GO
ALTER TABLE [dbo].[CompanySetting]  WITH CHECK ADD  CONSTRAINT [FkCompanySettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CompanySetting] CHECK CONSTRAINT [FkCompanySettingCompany]
GO
ALTER TABLE [dbo].[ControlColor]  WITH CHECK ADD  CONSTRAINT [FkControlColorCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[ControlColor] CHECK CONSTRAINT [FkControlColorCompany]
GO
ALTER TABLE [dbo].[ControlColor]  WITH CHECK ADD  CONSTRAINT [FkControlColorLoginUser] FOREIGN KEY([LoginUserId])
REFERENCES [dbo].[LoginUser] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ControlColor] CHECK CONSTRAINT [FkControlColorLoginUser]
GO
ALTER TABLE [dbo].[Currency]  WITH CHECK ADD  CONSTRAINT [FkCurrencyCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Currency] CHECK CONSTRAINT [FkCurrencyCompany]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FkCustoemrCollectCategory] FOREIGN KEY([CollectCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FkCustoemrCollectCategory]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FkCustoemrCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FkCustoemrCompany]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FkCustoemrStaff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FkCustoemrStaff]
GO
ALTER TABLE [dbo].[CustomerDiscount]  WITH CHECK ADD  CONSTRAINT [FkCustomerDiscountAccountTitle] FOREIGN KEY([AccountTitleId])
REFERENCES [dbo].[AccountTitle] ([Id])
GO
ALTER TABLE [dbo].[CustomerDiscount] CHECK CONSTRAINT [FkCustomerDiscountAccountTitle]
GO
ALTER TABLE [dbo].[CustomerDiscount]  WITH CHECK ADD  CONSTRAINT [FkCustomerDiscountCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerDiscount] CHECK CONSTRAINT [FkCustomerDiscountCustomer]
GO
ALTER TABLE [dbo].[CustomerDiscount]  WITH CHECK ADD  CONSTRAINT [FkCustomerDiscountDepartment] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([Id])
GO
ALTER TABLE [dbo].[CustomerDiscount] CHECK CONSTRAINT [FkCustomerDiscountDepartment]
GO
ALTER TABLE [dbo].[CustomerFee]  WITH CHECK ADD  CONSTRAINT [FkCustomerFeeCurrency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerFee] CHECK CONSTRAINT [FkCustomerFeeCurrency]
GO
ALTER TABLE [dbo].[CustomerFee]  WITH CHECK ADD  CONSTRAINT [FkCustomerFeeCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerFee] CHECK CONSTRAINT [FkCustomerFeeCustomer]
GO
ALTER TABLE [dbo].[CustomerGroup]  WITH CHECK ADD  CONSTRAINT [FkCustomerGroupChild] FOREIGN KEY([ChildCustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[CustomerGroup] CHECK CONSTRAINT [FkCustomerGroupChild]
GO
ALTER TABLE [dbo].[CustomerGroup]  WITH CHECK ADD  CONSTRAINT [FkCustomerGroupParent] FOREIGN KEY([ParentCustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[CustomerGroup] CHECK CONSTRAINT [FkCustomerGroupParent]
GO
ALTER TABLE [dbo].[CustomerPaymentContract]  WITH CHECK ADD  CONSTRAINT [FkCustomerPaymentContract] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerPaymentContract] CHECK CONSTRAINT [FkCustomerPaymentContract]
GO
ALTER TABLE [dbo].[CustomerPaymentContract]  WITH CHECK ADD  CONSTRAINT [FkCustomerPaymentContractGreaterThanCollectCategoryId1] FOREIGN KEY([GreaterThanCollectCategoryId1])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[CustomerPaymentContract] CHECK CONSTRAINT [FkCustomerPaymentContractGreaterThanCollectCategoryId1]
GO
ALTER TABLE [dbo].[CustomerPaymentContract]  WITH CHECK ADD  CONSTRAINT [FkCustomerPaymentContractGreaterThanCollectCategoryId2] FOREIGN KEY([GreaterThanCollectCategoryId2])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[CustomerPaymentContract] CHECK CONSTRAINT [FkCustomerPaymentContractGreaterThanCollectCategoryId2]
GO
ALTER TABLE [dbo].[CustomerPaymentContract]  WITH CHECK ADD  CONSTRAINT [FkCustomerPaymentContractGreaterThanCollectCategoryId3] FOREIGN KEY([GreaterThanCollectCategoryId3])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[CustomerPaymentContract] CHECK CONSTRAINT [FkCustomerPaymentContractGreaterThanCollectCategoryId3]
GO
ALTER TABLE [dbo].[CustomerPaymentContract]  WITH CHECK ADD  CONSTRAINT [FkCustomerPaymentContractLessThanCollectCategory] FOREIGN KEY([LessThanCollectCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[CustomerPaymentContract] CHECK CONSTRAINT [FkCustomerPaymentContractLessThanCollectCategory]
GO
ALTER TABLE [dbo].[DensaiRemoveWord]  WITH CHECK ADD  CONSTRAINT [FkDensaiRemoveWordCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[DensaiRemoveWord] CHECK CONSTRAINT [FkDensaiRemoveWordCompany]
GO
ALTER TABLE [dbo].[Department]  WITH CHECK ADD  CONSTRAINT [FkDepartmentCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FkDepartmentCompany]
GO
ALTER TABLE [dbo].[EBFileSetting]  WITH CHECK ADD  CONSTRAINT [FkEBFileSettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[EBFileSetting] CHECK CONSTRAINT [FkEBFileSettingCompany]
GO
ALTER TABLE [dbo].[EBFileSetting]  WITH CHECK ADD  CONSTRAINT [FkEBFileSettingLoginUser] FOREIGN KEY([LoginUserId])
REFERENCES [dbo].[LoginUser] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EBFileSetting] CHECK CONSTRAINT [FkEBFileSettingLoginUser]
GO
ALTER TABLE [dbo].[ExportFieldSetting]  WITH CHECK ADD  CONSTRAINT [FkExportFiledSettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[ExportFieldSetting] CHECK CONSTRAINT [FkExportFiledSettingCompany]
GO
ALTER TABLE [dbo].[GridSetting]  WITH CHECK ADD  CONSTRAINT [FkGridSettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[GridSetting] CHECK CONSTRAINT [FkGridSettingCompany]
GO
ALTER TABLE [dbo].[GridSetting]  WITH CHECK ADD  CONSTRAINT [FkGridSettingGridSettingBase] FOREIGN KEY([GridId], [ColumnName])
REFERENCES [dbo].[GridSettingBase] ([GridId], [ColumnName])
GO
ALTER TABLE [dbo].[GridSetting] CHECK CONSTRAINT [FkGridSettingGridSettingBase]
GO
ALTER TABLE [dbo].[GridSetting]  WITH CHECK ADD  CONSTRAINT [FkGridSettingLoginUser] FOREIGN KEY([LoginUserId])
REFERENCES [dbo].[LoginUser] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GridSetting] CHECK CONSTRAINT [FkGridSettingLoginUser]
GO
ALTER TABLE [dbo].[HolidayCalendar]  WITH CHECK ADD  CONSTRAINT [FkHolidayCalendarCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HolidayCalendar] CHECK CONSTRAINT [FkHolidayCalendarCompany]
GO
ALTER TABLE [dbo].[IgnoreKana]  WITH CHECK ADD  CONSTRAINT [FkIgnoreKanaCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[IgnoreKana] CHECK CONSTRAINT [FkIgnoreKanaCompany]
GO
ALTER TABLE [dbo].[IgnoreKana]  WITH CHECK ADD  CONSTRAINT [FkIgnoreKanaExcludeCategoryId] FOREIGN KEY([ExcludeCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[IgnoreKana] CHECK CONSTRAINT [FkIgnoreKanaExcludeCategoryId]
GO
ALTER TABLE [dbo].[ImporterSetting]  WITH CHECK ADD  CONSTRAINT [FkImporterSettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[ImporterSetting] CHECK CONSTRAINT [FkImporterSettingCompany]
GO
ALTER TABLE [dbo].[ImporterSetting]  WITH CHECK ADD  CONSTRAINT [FkImporterSettingImporterFormat] FOREIGN KEY([FormatId])
REFERENCES [dbo].[ImporterFormat] ([Id])
GO
ALTER TABLE [dbo].[ImporterSetting] CHECK CONSTRAINT [FkImporterSettingImporterFormat]
GO
ALTER TABLE [dbo].[ImporterSettingDetail]  WITH CHECK ADD  CONSTRAINT [FkImporterSettingDetailImporterSetting] FOREIGN KEY([ImporterSettingId])
REFERENCES [dbo].[ImporterSetting] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImporterSettingDetail] CHECK CONSTRAINT [FkImporterSettingDetailImporterSetting]
GO
ALTER TABLE [dbo].[ImportFileLog]  WITH CHECK ADD  CONSTRAINT [FkImportFileLogCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[ImportFileLog] CHECK CONSTRAINT [FkImportFileLogCompany]
GO
ALTER TABLE [dbo].[InputControl]  WITH CHECK ADD  CONSTRAINT [FkInputControlCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[InputControl] CHECK CONSTRAINT [FkInputControlCompany]
GO
ALTER TABLE [dbo].[InputControl]  WITH CHECK ADD  CONSTRAINT [FkInputControlLoginUser] FOREIGN KEY([LoginUserId])
REFERENCES [dbo].[LoginUser] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InputControl] CHECK CONSTRAINT [FkInputControlLoginUser]
GO
ALTER TABLE [dbo].[JuridicalPersonality]  WITH CHECK ADD  CONSTRAINT [FkJuridicalPersonalityCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[JuridicalPersonality] CHECK CONSTRAINT [FkJuridicalPersonalityCompany]
GO
ALTER TABLE [dbo].[KanaHistoryCustomer]  WITH CHECK ADD  CONSTRAINT [FkKanaHistoryCustomerCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[KanaHistoryCustomer] CHECK CONSTRAINT [FkKanaHistoryCustomerCompany]
GO
ALTER TABLE [dbo].[KanaHistoryCustomer]  WITH CHECK ADD  CONSTRAINT [FkKanaHistoryCustomerCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[KanaHistoryCustomer] CHECK CONSTRAINT [FkKanaHistoryCustomerCustomer]
GO
ALTER TABLE [dbo].[KanaHistoryPaymentAgency]  WITH CHECK ADD  CONSTRAINT [FkKanaHistoryPaymentAgencyPaymentAgency] FOREIGN KEY([PaymentAgencyId])
REFERENCES [dbo].[PaymentAgency] ([Id])
GO
ALTER TABLE [dbo].[KanaHistoryPaymentAgency] CHECK CONSTRAINT [FkKanaHistoryPaymentAgencyPaymentAgency]
GO
ALTER TABLE [dbo].[KanaHistoryPaymentAgency]  WITH CHECK ADD  CONSTRAINT [PkKanaHistoryPaymentAgencyCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[KanaHistoryPaymentAgency] CHECK CONSTRAINT [PkKanaHistoryPaymentAgencyCompany]
GO
ALTER TABLE [dbo].[LogData]  WITH CHECK ADD  CONSTRAINT [FkLogDataCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[LogData] CHECK CONSTRAINT [FkLogDataCompany]
GO
ALTER TABLE [dbo].[LoginUser]  WITH CHECK ADD  CONSTRAINT [FkLoginUser] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoginUser] CHECK CONSTRAINT [FkLoginUser]
GO
ALTER TABLE [dbo].[LoginUser]  WITH CHECK ADD  CONSTRAINT [FkLoginUserStaff] FOREIGN KEY([AssignedStaffId])
REFERENCES [dbo].[Staff] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoginUser] CHECK CONSTRAINT [FkLoginUserStaff]
GO
ALTER TABLE [dbo].[LoginUserLicense]  WITH CHECK ADD  CONSTRAINT [FkLoginUserLicense] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoginUserLicense] CHECK CONSTRAINT [FkLoginUserLicense]
GO
ALTER TABLE [dbo].[LoginUserPassword]  WITH CHECK ADD  CONSTRAINT [FkLoginUserPasswordLoginUser] FOREIGN KEY([LoginUserId])
REFERENCES [dbo].[LoginUser] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoginUserPassword] CHECK CONSTRAINT [FkLoginUserPasswordLoginUser]
GO
ALTER TABLE [dbo].[MailSetting]  WITH CHECK ADD  CONSTRAINT [FkMailSettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[MailSetting] CHECK CONSTRAINT [FkMailSettingCompany]
GO
ALTER TABLE [dbo].[MailTemplate]  WITH CHECK ADD  CONSTRAINT [FkMailTemplateCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[MailTemplate] CHECK CONSTRAINT [FkMailTemplateCompany]
GO
ALTER TABLE [dbo].[MasterImportSetting]  WITH CHECK ADD  CONSTRAINT [FkMasterImportSettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[MasterImportSetting] CHECK CONSTRAINT [FkMasterImportSettingCompany]
GO
ALTER TABLE [dbo].[MasterImportSetting]  WITH CHECK ADD  CONSTRAINT [FkMasterImportSettingMasterImportSettingBase] FOREIGN KEY([ImportFileType])
REFERENCES [dbo].[MasterImportSettingBase] ([ImportFileType])
GO
ALTER TABLE [dbo].[MasterImportSetting] CHECK CONSTRAINT [FkMasterImportSettingMasterImportSettingBase]
GO
ALTER TABLE [dbo].[Matching]  WITH CHECK ADD  CONSTRAINT [FkMatchingBilling] FOREIGN KEY([BillingId])
REFERENCES [dbo].[Billing] ([Id])
GO
ALTER TABLE [dbo].[Matching] CHECK CONSTRAINT [FkMatchingBilling]
GO
ALTER TABLE [dbo].[Matching]  WITH CHECK ADD  CONSTRAINT [FkMatchingMatchingHeader] FOREIGN KEY([MatchingHeaderId])
REFERENCES [dbo].[MatchingHeader] ([Id])
GO
ALTER TABLE [dbo].[Matching] CHECK CONSTRAINT [FkMatchingMatchingHeader]
GO
ALTER TABLE [dbo].[Matching]  WITH CHECK ADD  CONSTRAINT [FkMatchingReceipt] FOREIGN KEY([ReceiptId])
REFERENCES [dbo].[Receipt] ([Id])
GO
ALTER TABLE [dbo].[Matching] CHECK CONSTRAINT [FkMatchingReceipt]
GO
ALTER TABLE [dbo].[MatchingBillingDiscount]  WITH CHECK ADD  CONSTRAINT [FkMatchingBillingDiscountMatching] FOREIGN KEY([MatchingId])
REFERENCES [dbo].[Matching] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MatchingBillingDiscount] CHECK CONSTRAINT [FkMatchingBillingDiscountMatching]
GO
ALTER TABLE [dbo].[MatchingHeader]  WITH CHECK ADD  CONSTRAINT [FkMatchingHeaderCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[MatchingHeader] CHECK CONSTRAINT [FkMatchingHeaderCompany]
GO
ALTER TABLE [dbo].[MatchingHeader]  WITH CHECK ADD  CONSTRAINT [FkMatchingHeaderCurrency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[MatchingHeader] CHECK CONSTRAINT [FkMatchingHeaderCurrency]
GO
ALTER TABLE [dbo].[MatchingHeader]  WITH CHECK ADD  CONSTRAINT [FkMatchingHeaderCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[MatchingHeader] CHECK CONSTRAINT [FkMatchingHeaderCustomer]
GO
ALTER TABLE [dbo].[MatchingHeader]  WITH CHECK ADD  CONSTRAINT [FkMatchingHeaderPaymentAgency] FOREIGN KEY([PaymentAgencyId])
REFERENCES [dbo].[PaymentAgency] ([Id])
GO
ALTER TABLE [dbo].[MatchingHeader] CHECK CONSTRAINT [FkMatchingHeaderPaymentAgency]
GO
ALTER TABLE [dbo].[MatchingOrder]  WITH CHECK ADD  CONSTRAINT [FkMatchingOrderCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[MatchingOrder] CHECK CONSTRAINT [FkMatchingOrderCompany]
GO
ALTER TABLE [dbo].[MatchingOutputed]  WITH CHECK ADD  CONSTRAINT [FkMatchingOutputedMatchingHeader] FOREIGN KEY([MatchingHeaderId])
REFERENCES [dbo].[MatchingHeader] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MatchingOutputed] CHECK CONSTRAINT [FkMatchingOutputedMatchingHeader]
GO
ALTER TABLE [dbo].[MenuAuthority]  WITH CHECK ADD  CONSTRAINT [FkMenuAuthorityCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[MenuAuthority] CHECK CONSTRAINT [FkMenuAuthorityCompany]
GO
ALTER TABLE [dbo].[MenuAuthority]  WITH CHECK ADD  CONSTRAINT [FkMenuAuthorityMenu] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menu] ([MenuId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MenuAuthority] CHECK CONSTRAINT [FkMenuAuthorityMenu]
GO
ALTER TABLE [dbo].[Netting]  WITH CHECK ADD  CONSTRAINT [FkNettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Netting] CHECK CONSTRAINT [FkNettingCompany]
GO
ALTER TABLE [dbo].[Netting]  WITH CHECK ADD  CONSTRAINT [FkNettingCurrency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[Netting] CHECK CONSTRAINT [FkNettingCurrency]
GO
ALTER TABLE [dbo].[Netting]  WITH CHECK ADD  CONSTRAINT [FkNettingReceipt] FOREIGN KEY([ReceiptId])
REFERENCES [dbo].[Receipt] ([Id])
GO
ALTER TABLE [dbo].[Netting] CHECK CONSTRAINT [FkNettingReceipt]
GO
ALTER TABLE [dbo].[Netting]  WITH CHECK ADD  CONSTRAINT [FkNettingReceiptCategory] FOREIGN KEY([ReceiptCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Netting] CHECK CONSTRAINT [FkNettingReceiptCategory]
GO
ALTER TABLE [dbo].[Netting]  WITH CHECK ADD  CONSTRAINT [FkNettingSection] FOREIGN KEY([SectionId])
REFERENCES [dbo].[Section] ([Id])
GO
ALTER TABLE [dbo].[Netting] CHECK CONSTRAINT [FkNettingSection]
GO
ALTER TABLE [dbo].[PasswordPolicy]  WITH CHECK ADD  CONSTRAINT [FkPasswordPolicyCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PasswordPolicy] CHECK CONSTRAINT [FkPasswordPolicyCompany]
GO
ALTER TABLE [dbo].[PaymentAgency]  WITH CHECK ADD  CONSTRAINT [FkPaymentAgencyCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[PaymentAgency] CHECK CONSTRAINT [FkPaymentAgencyCompany]
GO
ALTER TABLE [dbo].[PaymentAgency]  WITH CHECK ADD  CONSTRAINT [FkPaymentAgencyFileFormat] FOREIGN KEY([FileFormatId])
REFERENCES [dbo].[PaymentFileFormat] ([Id])
GO
ALTER TABLE [dbo].[PaymentAgency] CHECK CONSTRAINT [FkPaymentAgencyFileFormat]
GO
ALTER TABLE [dbo].[PaymentAgencyFee]  WITH CHECK ADD  CONSTRAINT [FkPaymentAgencyFeeCurrency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PaymentAgencyFee] CHECK CONSTRAINT [FkPaymentAgencyFeeCurrency]
GO
ALTER TABLE [dbo].[PaymentAgencyFee]  WITH CHECK ADD  CONSTRAINT [FkPaymentAgencyFeePaymentAgency] FOREIGN KEY([PaymentAgencyId])
REFERENCES [dbo].[PaymentAgency] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PaymentAgencyFee] CHECK CONSTRAINT [FkPaymentAgencyFeePaymentAgency]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FkReceiptCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FkReceiptCompany]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FkReceiptCurrency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FkReceiptCurrency]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FkReceiptCustomer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FkReceiptCustomer]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FkReceiptExcludeCategory] FOREIGN KEY([ExcludeCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FkReceiptExcludeCategory]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FkReceiptOriginalReceipt] FOREIGN KEY([OriginalReceiptId])
REFERENCES [dbo].[Receipt] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FkReceiptOriginalReceipt]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FkReceiptReceiptCategory] FOREIGN KEY([ReceiptCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FkReceiptReceiptCategory]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FkReceiptReceiptHeader] FOREIGN KEY([ReceiptHeaderId])
REFERENCES [dbo].[ReceiptHeader] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FkReceiptReceiptHeader]
GO
ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FkReceiptSection] FOREIGN KEY([SectionId])
REFERENCES [dbo].[Section] ([Id])
GO
ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FkReceiptSection]
GO
ALTER TABLE [dbo].[ReceiptExclude]  WITH CHECK ADD  CONSTRAINT [FkReceiptExcludeExcludeCategory] FOREIGN KEY([ExcludeCategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[ReceiptExclude] CHECK CONSTRAINT [FkReceiptExcludeExcludeCategory]
GO
ALTER TABLE [dbo].[ReceiptExclude]  WITH CHECK ADD  CONSTRAINT [FkReceiptExcludeReceipt] FOREIGN KEY([ReceiptId])
REFERENCES [dbo].[Receipt] ([Id])
GO
ALTER TABLE [dbo].[ReceiptExclude] CHECK CONSTRAINT [FkReceiptExcludeReceipt]
GO
ALTER TABLE [dbo].[ReceiptHeader]  WITH CHECK ADD  CONSTRAINT [FkReceiptHeaderCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[ReceiptHeader] CHECK CONSTRAINT [FkReceiptHeaderCompany]
GO
ALTER TABLE [dbo].[ReceiptHeader]  WITH CHECK ADD  CONSTRAINT [FkReceiptHeaderCurrency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[ReceiptHeader] CHECK CONSTRAINT [FkReceiptHeaderCurrency]
GO
ALTER TABLE [dbo].[ReceiptHeader]  WITH CHECK ADD  CONSTRAINT [FkReceiptHeaderImportFileLog] FOREIGN KEY([ImportFileLogId])
REFERENCES [dbo].[ImportFileLog] ([Id])
GO
ALTER TABLE [dbo].[ReceiptHeader] CHECK CONSTRAINT [FkReceiptHeaderImportFileLog]
GO
ALTER TABLE [dbo].[ReceiptMemo]  WITH CHECK ADD  CONSTRAINT [FkReceiptMemoReceipt] FOREIGN KEY([ReceiptId])
REFERENCES [dbo].[Receipt] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReceiptMemo] CHECK CONSTRAINT [FkReceiptMemoReceipt]
GO
ALTER TABLE [dbo].[ReceiptSectionTransfer]  WITH CHECK ADD  CONSTRAINT [FkReceiptSectionTransferDestinationReceipt] FOREIGN KEY([DestinationReceiptId])
REFERENCES [dbo].[Receipt] ([Id])
GO
ALTER TABLE [dbo].[ReceiptSectionTransfer] CHECK CONSTRAINT [FkReceiptSectionTransferDestinationReceipt]
GO
ALTER TABLE [dbo].[ReceiptSectionTransfer]  WITH CHECK ADD  CONSTRAINT [FkReceiptSectionTransferDestinationSection] FOREIGN KEY([DestinationSectionId])
REFERENCES [dbo].[Section] ([Id])
GO
ALTER TABLE [dbo].[ReceiptSectionTransfer] CHECK CONSTRAINT [FkReceiptSectionTransferDestinationSection]
GO
ALTER TABLE [dbo].[ReceiptSectionTransfer]  WITH CHECK ADD  CONSTRAINT [FkReceiptSectionTransferSourceReceipt] FOREIGN KEY([SourceReceiptId])
REFERENCES [dbo].[Receipt] ([Id])
GO
ALTER TABLE [dbo].[ReceiptSectionTransfer] CHECK CONSTRAINT [FkReceiptSectionTransferSourceReceipt]
GO
ALTER TABLE [dbo].[ReceiptSectionTransfer]  WITH CHECK ADD  CONSTRAINT [FkReceiptSectionTransferSourceSection] FOREIGN KEY([SourceSectionId])
REFERENCES [dbo].[Section] ([Id])
GO
ALTER TABLE [dbo].[ReceiptSectionTransfer] CHECK CONSTRAINT [FkReceiptSectionTransferSourceSection]
GO
ALTER TABLE [dbo].[ReportSetting]  WITH CHECK ADD  CONSTRAINT [FkReportSettingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[ReportSetting] CHECK CONSTRAINT [FkReportSettingCompany]
GO
ALTER TABLE [dbo].[Section]  WITH CHECK ADD  CONSTRAINT [FkSectionCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Section] CHECK CONSTRAINT [FkSectionCompany]
GO
ALTER TABLE [dbo].[SectionWithDepartment]  WITH CHECK ADD  CONSTRAINT [FkSectionWithDepartmentDepartment] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([Id])
GO
ALTER TABLE [dbo].[SectionWithDepartment] CHECK CONSTRAINT [FkSectionWithDepartmentDepartment]
GO
ALTER TABLE [dbo].[SectionWithDepartment]  WITH CHECK ADD  CONSTRAINT [FkSectionWithDepartmentSection] FOREIGN KEY([SectionId])
REFERENCES [dbo].[Section] ([Id])
GO
ALTER TABLE [dbo].[SectionWithDepartment] CHECK CONSTRAINT [FkSectionWithDepartmentSection]
GO
ALTER TABLE [dbo].[SectionWithLoginUser]  WITH CHECK ADD  CONSTRAINT [FkSectionWithLoginUserLoginUser] FOREIGN KEY([LoginUserId])
REFERENCES [dbo].[LoginUser] ([Id])
GO
ALTER TABLE [dbo].[SectionWithLoginUser] CHECK CONSTRAINT [FkSectionWithLoginUserLoginUser]
GO
ALTER TABLE [dbo].[SectionWithLoginUser]  WITH CHECK ADD  CONSTRAINT [FkSectionWithLoginUserSection] FOREIGN KEY([SectionId])
REFERENCES [dbo].[Section] ([Id])
GO
ALTER TABLE [dbo].[SectionWithLoginUser] CHECK CONSTRAINT [FkSectionWithLoginUserSection]
GO
ALTER TABLE [dbo].[Setting]  WITH CHECK ADD  CONSTRAINT [FkSettingSettingDefinition] FOREIGN KEY([ItemId])
REFERENCES [dbo].[SettingDefinition] ([ItemId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Setting] CHECK CONSTRAINT [FkSettingSettingDefinition]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FkStaffCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FkStaffCompany]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FkStaffDepartment] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([Id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FkStaffDepartment]
GO
ALTER TABLE [dbo].[TaskSchedule]  WITH CHECK ADD  CONSTRAINT [FkTaskScheduleCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[TaskSchedule] CHECK CONSTRAINT [FkTaskScheduleCompany]
GO
ALTER TABLE [dbo].[TaskScheduleHistory]  WITH CHECK ADD  CONSTRAINT [FkTaskScheduleHistoryCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[TaskScheduleHistory] CHECK CONSTRAINT [FkTaskScheduleHistoryCompany]
GO
ALTER TABLE [dbo].[Tax]  WITH CHECK ADD  CONSTRAINT [FkTaxCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Tax] CHECK CONSTRAINT [FkTaxCompany]
GO
ALTER TABLE [dbo].[WorkBankTransfer]  WITH CHECK ADD  CONSTRAINT [FkWorkBankTransferCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[WorkBankTransfer] CHECK CONSTRAINT [FkWorkBankTransferCompany]
GO
ALTER TABLE [dbo].[WorkBilling]  WITH CHECK ADD  CONSTRAINT [FkWorkBillingCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[WorkBilling] CHECK CONSTRAINT [FkWorkBillingCompany]
GO
ALTER TABLE [dbo].[WorkCollation]  WITH CHECK ADD  CONSTRAINT [FkWorkCollationCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[WorkCollation] CHECK CONSTRAINT [FkWorkCollationCompany]
GO
ALTER TABLE [dbo].[WorkDepartmentTarget]  WITH CHECK ADD  CONSTRAINT [FkWorkDepartmentTargetCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[WorkDepartmentTarget] CHECK CONSTRAINT [FkWorkDepartmentTargetCompany]
GO
ALTER TABLE [dbo].[WorkReceipt]  WITH CHECK ADD  CONSTRAINT [FkWorkReceiptCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[WorkReceipt] CHECK CONSTRAINT [FkWorkReceiptCompany]
GO
ALTER TABLE [dbo].[WorkSectionTarget]  WITH CHECK ADD  CONSTRAINT [FkWorkSectionTargetCompany] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[WorkSectionTarget] CHECK CONSTRAINT [FkWorkSectionTargetCompany]
GO
/****** Object:  StoredProcedure [dbo].[uspCollation]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspCollation]
 @ClientKey         VARBINARY(20)  /* クライアントキー */
,@CompanyId         INT            /* 会社ID           */
,@CurrencyId        INT            /* 通貨ID           */
,@RecordedAt        DATE           /* 入金日           */
,@DueAt             DATE           /* 入金予定日       */
,@BillingType       INT            /* 請求データタイプ */
,@LimitDateType     INT            /* 期日現金利用タイプ */
,@AmountType        INT            /* 請求額種別 */
,@UseDepartmentWork INT            /* 請求部門絞込利用 */
,@UseSectionWork    INT            /* 入金部門絞込利用 */

AS
DECLARE
 @dat               DATETIME2
,@collationType     INT
BEGIN
    SET @dat = getdate();

    EXECUTE [dbo].[uspCollationInitialize]
            @ClientKey
          , @CompanyId
          , @CurrencyId
          , @RecordedAt
          , @DueAt
          , @BillingType
          , @LimitDateType
          , @AmountType
          , @UseDepartmentWork
          , @UseSectionWork
          ;

    DECLARE cur CURSOR FOR
     SELECT co.CollationTypeId
       FROM [dbo].[CollationOrder] co
      WHERE co.CompanyId = @CompanyId
        AND co.Available    = 1
      ORDER BY
            co.ExecutionOrder   ASC

    OPEN cur
    FETCH NEXT FROM cur INTO @collationType

    WHILE (@@FETCH_STATUS = 0)
    BEGIN
        IF @collationType = 0
        BEGIN
            EXECUTE [dbo].[uspCollationPayerCode] @ClientKey;
        END
        ELSE IF @collationType = 1
        BEGIN
            EXECUTE [dbo].[uspCollationCustomerId] @ClientKey;
        END
        ELSE IF @collationType = 2
        BEGIN
            EXECUTE [dbo].[uspCollationHistory] @ClientKey;
        END
        ELSE IF @collationType = 3
        BEGIN
            EXECUTE [dbo].[uspCollationPayerName] @ClientKey;
        END
        ELSE IF @collationType = 4
        BEGIN
            EXECUTE [dbo].[uspCollationKey] @ClientKey;
        END

        FETCH NEXT FROM cur INTO @collationType
    END

    CLOSE cur
    DEALLOCATE cur

    EXECUTE [dbo].[uspCollationMissing] @ClientKey;
END;

GO
/****** Object:  StoredProcedure [dbo].[uspCollationCustomerId]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspCollationCustomerId]
 @ClientKey         VARBINARY(20)
AS
DECLARE
 @type              INT
BEGIN

    SET @type       = 2 /* 債権代表者グループ 子得意先 */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey           [ClientKey]
         , wb.CompanyId
         , wb.CurrencyId
         , wb.CustomerId
         , 0                    [PaymentAgencyId]
         , wr.PayerName
         , wr.BankCode
         , wr.BranchCode
         , wr.PayerCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                [CollationType]
         , csb.Name             [ParentCustomerName]
         , csb.Kana             [ParentCustomerKana]
         , csb.ShareTransferFee [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[Customer] csr
        ON wr.ClientKey         = @ClientKey
       AND csr.Id               = wr.CustomerId
       AND csr.IsParent         = 0
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.CompanyId          = wr.CompanyId
              AND wc.CurrencyId         = wr.CurrencyId
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId )
     INNER JOIN [dbo].[WorkBilling] wb
        ON wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND wb.CustomerId       <> wr.CustomerId
       AND EXISTS (
           SELECT 1
             FROM [dbo].[CustomerGroup] csg
            WHERE csg.ParentCustomerId  = wb.CustomerId
              AND csg.ChildCustomerId   = wr.CustomerId )
     INNER JOIN [dbo].[Customer] csb
        ON csb.Id               = wb.CustomerId;

    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.ParentCustomerId <> wrt.CustomerId
       AND wc.CollationType     = @type;

    UPDATE [dbo].[WorkNettingTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkNettingTarget] wnt
     INNER JOIN [dbo].[Netting] n
        ON wnt.ClientKey        = @ClientKey
       AND wnt.NettingId        = n.Id
       AND wnt.CollationType    = 0
     INNER JOIN [dbo].[Customer] cs
        ON cs.Id                = n.CustomerId
     INNER JOIN [dbo].[WorkCollation] wc
        ON wc.ClientKey         = wnt.ClientKey
       AND wc.CompanyId         = n.CompanyId
       AND wc.CurrencyId        = n.CurrencyId
       AND wc.PayerName         = cs.Kana
       AND wc.PayerCode         = N''
       AND wc.BankCode          = N''
       AND wc.BranchCode        = N''
       AND wc.SourceBankName    = N''
       AND wc.SourceBranchName  = N''
       AND wc.CollationKey      = N''
       AND wc.CustomerId        = n.CustomerId
       AND wc.ParentCustomerId <> n.CustomerId
       AND wc.CollationType     = @type;


    SET @type       = 3  /* 債権代表者ID 入金 得意先ID */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , PayerCode
         , BankCode
         , BranchCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey
         , wb.CompanyId
         , wb.CurrencyId
         , wb.CustomerId
         , 0                    [PaymentAgencyId]
         , wr.PayerName
         , wr.PayerCode
         , wr.BankCode
         , wr.BranchCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                [CollationType]
         , csb.Name             [ParentCustomerName]
         , csb.Kana             [ParentCustomerKana]
         , csb.ShareTransferFee [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[WorkBilling] wb
        ON wb.ClientKey         = @ClientKey
       AND wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND wb.CustomerId        = wr.CustomerId
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.CompanyId          = wr.CompanyId
              AND wc.CurrencyId         = wr.CurrencyId
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId )
     INNER JOIN [dbo].[Customer] csb
        ON csb.Id               = wb.CustomerId;

    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.ParentCustomerId  = wrt.CustomerId
       AND wc.CollationType     = @type;

    UPDATE [dbo].[WorkNettingTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkNettingTarget] wnt
     INNER JOIN [dbo].[Netting] n
        ON wnt.ClientKey        = @ClientKey
       AND wnt.NettingId        = n.Id
       AND wnt.CollationType    = 0
     INNER JOIN [dbo].[Customer] cs
        ON cs.Id                = n.CustomerId
     INNER JOIN [dbo].[WorkCollation] wc
        ON wc.ClientKey         = wnt.ClientKey
       AND wc.CompanyId         = n.CompanyId
       AND wc.CurrencyId        = n.CurrencyId
       AND wc.PayerName         = cs.Kana
       AND wc.PayerCode         = N''
       AND wc.BankCode          = N''
       AND wc.BranchCode        = N''
       AND wc.SourceBankName    = N''
       AND wc.SourceBranchName  = N''
       AND wc.CollationKey      = N''
       AND wc.CustomerId        = n.CustomerId
       AND wc.ParentCustomerId  = n.CustomerId
       AND wc.CollationType     = @type;

END;

GO
/****** Object:  StoredProcedure [dbo].[uspCollationHistory]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspCollationHistory]
 @ClientKey         VARBINARY(20)
AS
DECLARE
 @type              INT
BEGIN

    SET @type       = 5 /* 学習履歴照合 仕向銀行/支店 情報利用 */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey           [ClientKey]
         , wb.CompanyId
         , wb.CurrencyId
         , wb.CustomerId
         , 0                    [PaymentAgencyId]
         , wr.PayerName
         , wr.BankCode
         , wr.BranchCode
         , wr.PayerCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                [CollationType]
         , cs.Name              [ParentCustomerName]
         , cs.Kana              [ParentCustomerKana]
         , cs.ShareTransferFee  [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[Customer] cs
        ON wr.ClientKey         = @ClientKey
       AND wr.SourceBankName    > N''
       AND wr.SourceBranchName  > N''
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.CompanyId          = wr.CompanyId
              AND wc.CurrencyId         = wr.CurrencyId
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId )
       AND cs.Id = (
           SELECT TOP 1 khc.CustomerId
             FROM [dbo].[KanaHistoryCustomer] khc
            WHERE khc.CompanyId        = wr.CompanyId
              AND khc.PayerName        = wr.PayerName
              AND khc.SourceBankName   = wr.SourceBankName
              AND khc.SourceBranchName = wr.SourceBranchName
            ORDER BY
                  khc.HitCount     DESC
                , khc.UpdateAt     DESC )
     INNER JOIN [dbo].[WorkBilling] wb
        ON wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND wb.CustomerId        = cs.Id

    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.CollationType     = @type;

    SET @type       = 8  /* 口座振替 仕向銀行/支店あり */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey
         , wb.CompanyId
         , wb.CurrencyId
         , 0                    [ParentCustomerId]
         , wb.PaymentAgencyId
         , wr.PayerName
         , wr.BankCode
         , wr.BranchCode
         , wr.PayerCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                [CollationType]
         , pa.Name              [ParentCustomerName]
         , pa.Kana              [ParentCustomerKana]
         , pa.ShareTransferFee  [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[PaymentAgency] pa
        ON wr.ClientKey             = @ClientKey
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.CompanyId          = wr.CompanyId
              AND wc.CurrencyId         = wr.CurrencyId
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId )
       AND pa.Id = (
           SELECT TOP 1 khp.PaymentAgencyId
             FROM [dbo].[KanaHistoryPaymentAgency] khp
            WHERE khp.CompanyId         = wr.CompanyId
              AND khp.PayerName         = wr.PayerName
              AND khp.SourceBankName    = wr.SourceBankName
              AND khp.SourceBranchName  = wr.SourceBranchName
            ORDER BY
                  khp.HitCount      DESC
                , khp.UpdateAt      DESC )
     INNER JOIN [dbo].[WorkBankTransfer] wb
        ON wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND pa.Id                = wb.PaymentAgencyId
    ;

    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.CollationType     = @type;

    SET @type       = 6 /* 学習履歴照合 仕向銀行/支店なし */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey           [ClientKey]
         , wb.CompanyId
         , wb.CurrencyId
         , wb.CustomerId        [ParentCustomerId]
         , 0                    [PaymentAgencyId]
         , wr.PayerName
         , wr.BankCode
         , wr.BranchCode
         , wr.PayerCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                [CollationType]
         , cs.Name              [ParentCustomerName]
         , cs.Kana              [ParentCustomerKana]
         , cs.ShareTransferFee  [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[Customer] cs
        ON wr.ClientKey             = @ClientKey
       AND cs.Id = (
           SELECT TOP 1 khc.CustomerId
             FROM [dbo].[KanaHistoryCustomer] khc
            WHERE khc.CompanyId         = wr.CompanyId
              AND khc.PayerName         = wr.PayerName
              AND khc.SourceBankName    = N''
              AND khc.SourceBranchName  = N''
            ORDER BY
                  khc.HitCount     DESC
                , khc.UpdateAt     DESC )
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.CompanyId          = wr.CompanyId
              AND wc.CurrencyId         = wr.CurrencyId
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId )
     INNER JOIN [dbo].[WorkBilling] wb
        ON wb.ClientKey         = @ClientKey
       AND wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND cs.Id                = wb.CustomerId

    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.CollationType     = @type;

    UPDATE [dbo].[WorkNettingTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkNettingTarget] wnt
     INNER JOIN [dbo].[Netting] n
        ON wnt.ClientKey        = @ClientKey
       AND wnt.NettingId        = n.Id
       AND wnt.CollationType    = 0
     INNER JOIN [dbo].[Customer] cs
        ON cs.Id                = n.CustomerId
     INNER JOIN [dbo].[WorkCollation] wc
        ON wc.ClientKey         = wnt.ClientKey
       AND wc.CompanyId         = n.CompanyId
       AND wc.CurrencyId        = n.CurrencyId
       AND wc.PayerName         = cs.Kana
       AND wc.PayerCode         = N''
       AND wc.BankCode          = N''
       AND wc.BranchCode        = N''
       AND wc.SourceBankName    = N''
       AND wc.SourceBranchName  = N''
       AND wc.CollationKey      = N''
       AND wc.CustomerId        = n.CustomerId
       AND wc.CollationType     = @type;

    SET @type       = 9  /* 口座振替 仕向銀行/支店なし */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey
         , wb.CompanyId
         , wb.CurrencyId
         , 0                    [ParentCustomerId]
         , wb.PaymentAgencyId
         , wr.PayerName
         , wr.BankCode
         , wr.BranchCode
         , wr.PayerCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                [CollationType]
         , pa.Name              [ParentCustomerName]
         , pa.Kana              [ParentCustomerKana]
         , pa.ShareTransferFee  [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[PaymentAgency] pa
        ON wr.ClientKey         = @ClientKey
       AND pa.Id = (
           SELECT TOP 1 khp.PaymentAgencyId
             FROM [dbo].[KanaHistoryPaymentAgency] khp
            WHERE khp.CompanyId         = wr.CompanyId
              AND khp.PayerName         = wr.PayerName
              AND khp.SourceBankName    = N''
              AND khp.SourceBranchName  = N''
            ORDER BY
                  khp.HitCount      DESC
                , khp.UpdateAt      DESC )
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.CompanyId          = wr.CompanyId
              AND wc.CurrencyId         = wr.CurrencyId
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId )
     INNER JOIN [dbo].[WorkBankTransfer] wb
        ON wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND pa.Id                = wb.PaymentAgencyId
    ;

    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.CollationType     = @type;

    UPDATE [dbo].[WorkNettingTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkNettingTarget] wnt
     INNER JOIN [dbo].[Netting] n
        ON wnt.ClientKey        = @ClientKey
       AND wnt.NettingId        = n.Id
       AND wnt.CollationType    = 0
     INNER JOIN [dbo].[Customer] cs
        ON cs.Id                = n.CustomerId
     INNER JOIN [dbo].[WorkCollation] wc
        ON wc.ClientKey         = wnt.ClientKey
       AND wc.CompanyId         = n.CompanyId
       AND wc.CurrencyId        = n.CurrencyId
       AND wc.PayerName         = cs.Kana
       AND wc.PayerCode         = N''
       AND wc.BankCode          = N''
       AND wc.BranchCode        = N''
       AND wc.SourceBankName    = N''
       AND wc.SourceBranchName  = N''
       AND wc.CollationKey      = N''
       AND wc.CustomerId        = n.CustomerId
       AND wc.CollationType     = @type;

END;

GO
/****** Object:  StoredProcedure [dbo].[uspCollationInitialize]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspCollationInitialize]
 @ClientKey         VARBINARY(20)  /* クライアントキー */
,@CompanyId         INT            /* 会社ID           */
,@CurrencyId        INT            /* 通貨ID           */
,@RecordedAt        DATE           /* 入金日           */
,@DueAt             DATE           /* 入金予定日       */
,@BillingType       INT            /* 請求データタイプ */
,@LimitDateType     INT            /* 期日現金タイプ   */
,@AmountType        INT            /* 請求金額タイプ   */
,@UseDepartmentWork INT            /* 請求部門絞込利用 */
,@UseSectionWork    INT            /* 入金部門絞込利用 */
AS
BEGIN
    /* delete work data */
    DELETE [dbo].[WorkBilling]
     WHERE ClientKey        = @ClientKey;

    DELETE [dbo].[WorkBillingTarget]
     WHERE ClientKey        = @ClientKey;

    DELETE [dbo].[WorkBankTransfer]
     WHERE ClientKey        = @ClientKey;

    DELETE [dbo].[WorkReceipt]
     WHERE ClientKey        = @ClientKey;

    DELETE [dbo].[WorkCollation]
     WHERE ClientKey        = @ClientKey;

    DELETE [dbo].[WorkReceiptTarget]
     WHERE ClientKey        = @ClientKey;

    DELETE [dbo].[WorkNettingTarget]
     WHERE ClientKey        = @ClientKey;

    /* 通常請求ワーク登録 */
    INSERT INTO [dbo].[WorkBillingTarget]
         ( ClientKey
         , BillingId
         , CustomerId
         , PaymentAgencyId
         , BillingAmount
         , RemainAmount )
    SELECT @ClientKey   [ClientKey]
         , b.[Id]       [BillingId]
         , COALESCE(csg.ParentCustomerId, b.CustomerId) [CustomerId]
         , 0 [PaymentAgengyId]
         , ( b.BillingAmount
           - COALESCE(bd.DiscountAmount, 0)
           - CASE @AmountType WHEN 0 THEN 0 ELSE b.OffsetAmount END ) [BillingAmount]
         , ( b.RemainAmount
           - COALESCE(bd.DiscountAmount, 0)
           - CASE @AmountType WHEN 0 THEN 0 ELSE b.OffsetAmount END ) [RemainAmount]
      FROM [dbo].[Billing] b
     INNER JOIN [dbo].[Category] cc
        ON cc.Id                     = b.CollectCategoryId
       AND cc.UseAccountTransfer     = 0 /* 口座振替ではない通常請求 */
       AND b.CompanyId               = @CompanyId
       AND b.CurrencyId              = COALESCE(@CurrencyId, b.CurrencyId)
       AND b.DueAt                  <= @DueAt
       AND b.Approved                = 1
       AND b.RemainAmount           <> b.OffsetAmount
       AND b.DeleteAt               IS NULL
       AND (
                @BillingType = 0
            OR (@BillingType = 1 AND b.InputType <> 3)
            OR (@BillingType = 2 AND b.InputType  = 3)
           )
       AND (
                @UseDepartmentWork = 0
            OR (@UseDepartmentWork = 1
                AND b.DepartmentId IN (
                    SELECT wdt.DepartmentId
                      FROM [dbo].[WorkDepartmentTarget] wdt
                     WHERE wdt.ClientKey         = @ClientKey
                       AND wdt.UseCollation      = 1 )
               )
           )
      LEFT JOIN [dbo].[CustomerGroup] csg
        ON csg.ChildCustomerId       = b.CustomerId
      LEFT JOIN (
           SELECT bd.BillingId
                , SUM( bd.DiscountAmount ) [DiscountAmount]
             FROM [dbo].[BillingDiscount] bd
            WHERE bd.AssignmentFlag      = 0
            GROUP BY bd.BillingId
           ) bd
        ON bd.BillingId              = b.Id

    /* 口座振替ワーク登録 */
    INSERT INTO [dbo].[WorkBillingTarget]
         ( ClientKey
         , BillingId
         , CustomerId
         , PaymentAgencyId
         , BillingAmount
         , RemainAmount )
    SELECT @ClientKey   [ClientKey]
         , b.[Id]       [BillingId]
         , 0 [CustomerId]
         , pa.[Id] [PaymentAgengyId]
         , ( b.BillingAmount
           - COALESCE(bd.DiscountAmount, 0)
           - CASE @AmountType WHEN 0 THEN 0 ELSE b.OffsetAmount END ) [BillingAmount]
         , ( b.RemainAmount
           - COALESCE(bd.DiscountAmount, 0)
           - CASE @AmountType WHEN 0 THEN 0 ELSE b.OffsetAmount END ) [RemainAmount]
      FROM [dbo].[Billing] b
     INNER JOIN [dbo].[Category] cc
        ON cc.Id                     = b.CollectCategoryId
       AND cc.UseAccountTransfer     = 1 /* 口座振替の回収区分 */
       AND b.ResultCode              = 0 /* 口座振替の結果あり */
       AND b.CompanyId               = @CompanyId
       AND b.CurrencyId              = COALESCE(@CurrencyId, b.CurrencyId)
       AND b.DueAt                  <= @DueAt
       AND b.Approved                = 1
       AND b.RemainAmount           <> b.OffsetAmount
       AND b.DeleteAt               IS NULL
       AND (
                @BillingType = 0
            OR (@BillingType = 1 and b.InputType <> 3)
            OR (@BillingType = 2 and b.InputType  = 3)
           )
       AND (
                @UseDepartmentWork = 0
            OR (@UseDepartmentWork = 1
                AND b.DepartmentId IN (
                    SELECT wdt.DepartmentId
                      FROM [dbo].[WorkDepartmentTarget] wdt
                     WHERE wdt.ClientKey     = @ClientKey
                       AND wdt.UseCollation  = 1 )
               )
           )
     INNER JOIN [dbo].[PaymentAgency] pa
        ON pa.Id                     = cc.PaymentAgencyId
      LEFT JOIN (
           SELECT bd.BillingId
                , SUM( bd.DiscountAmount ) [DiscountAmount]
             FROM [dbo].[BillingDiscount] bd
            GROUP BY bd.BillingId
           ) bd
        ON bd.BillingId              = b.Id


    INSERT INTO [dbo].[WorkBilling]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , CustomerId
         , CustomerKana
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         )
    SELECT @ClientKey       [ClientKey]
         , b.CompanyId
         , b.CurrencyId
         , b.CustomerId
         , cs.Kana          [CustomerKana]
         , b.BillingAmount
         , b.BillingRemainAmount
         , b.BillingCount
      FROM (
           SELECT b.CompanyId
                , b.CurrencyId
                , wbt.[CustomerId]
                , SUM( wbt.BillingAmount ) [BillingAmount]
                , SUM( wbt.RemainAmount  ) [BillingRemainAmount]
                , COUNT(1) [BillingCount]
             FROM [dbo].[WorkBillingTarget] wbt
            INNER JOIN [dbo].[Billing] b
               ON wbt.[ClientKey]           = @ClientKey
              AND wbt.[Billingid]           = b.[Id]
              AND wbt.[CustomerId]          > 0
              AND wbt.[PaymentAgencyId]     = 0
            GROUP BY
                  b.CompanyId
                , b.CurrencyId
                , wbt.CustomerId
           ) b
     INNER JOIN [dbo].[Customer] cs
        ON cs.Id                = b.CustomerId;

    /* 口座振替 請求ワーク登録 */
    INSERT INTO [dbo].[WorkBankTransfer]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , PaymentAgencyId
         , PaymentAgencyKana
         , BillingAmount
         , BillingRemainAmount
         , BillingCount )
    SELECT @ClientKey       [ClientKey]
         , b.CompanyId
         , b.CurrencyId
         , b.PaymentAgencyId
         , pa.Kana          [CustomerKana]
         , b.BillingAmount
         , b.BillingRemainAmount
         , b.BillingCount
      FROM (
           SELECT b.CompanyId
                , b.CurrencyId
                , wbt.PaymentAgencyId
                , SUM( wbt.BillingAmount ) [BillingAmount]
                , SUM( wbt.RemainAmount  ) [BillingRemainAmount]
                , COUNT(1) [BillingCount]
             FROM [dbo].[WorkBillingTarget] wbt
            INNER JOIN [dbo].[Billing] b
               ON wbt.[ClientKey]           = @ClientKey
              AND wbt.[BillingId]           = b.[Id]
              AND wbt.[CustomerId]          = 0
              AND wbt.[PaymentAgencyId]     > 0
            GROUP BY
                  b.CompanyId
                , b.CurrencyId
                , wbt.PaymentAgencyId
           ) b
     INNER JOIN [dbo].[PaymentAgency] pa
        ON pa.Id                = b.PaymentAgencyId;

    /* 入金対象データ絞込登録 */
    INSERT INTO [dbo].[WorkReceiptTarget]
         ( [ClientKey]
         , [ReceiptId]
         , [CompanyId]
         , [CurrencyId]
         , [PayerName]
         , [BankCode]
         , [BranchCode]
         , [PayerCode]
         , [SourceBankName]
         , [SourceBranchName]
         , [CollationKey]
         , [CustomerId]
         , [CollationType] )
    SELECT @ClientKey   [ClientKey]
         , r.Id         [ReceiptId]
         , r.[CompanyId]
         , r.[CurrencyId]
         , r.[PayerName]
         , COALESCE(rh.[BankCode]  , N'') [BankCode]
         , COALESCE(rh.[BranchCode], N'') [BranchCode]
         , r.[PayerCode]
         , r.[SourceBankName]
         , r.[SourceBranchName]
         , r.[CollationKey]
         , COALESCE(r.[CustomerId], 0) [CustomerId]
         , 0            [CollationType]
      FROM [dbo].[Receipt] r
      LEFT JOIN [dbo].[ReceiptHeader] rh    ON rh.[Id]  = r.[ReceiptHeaderId]
     WHERE r.CompanyId          = @CompanyId
       AND r.CurrencyId         = COALESCE(@CurrencyId, r.CurrencyId)
       AND r.RecordedAt        <= @RecordedAt
       AND r.Approved           = 1
       AND r.Apportioned        = 1
       AND r.RemainAmount      <> 0
       AND r.DeleteAt          IS NULL
       AND (@UseSectionWork = 0
         OR @UseSectionWork = 1
            AND r.SectionId IN (
                SELECT wst.SectionId
                  FROM WorkSectionTarget wst
                 WHERE wst.ClientKey    = @ClientKey
                   AND wst.UseCollation = 1 )
           )
       AND (@BillingType <> 2
         OR @BillingType  = 2 AND r.DueAt IS NULL
           );

    /* 相殺対象データ絞込登録 */
    INSERT INTO [dbo].[WorkNettingTarget]
         ( ClientKey
         , NettingId
         , CollationType )
    SELECT @ClientKey       [ClientKey]
         , n.Id             [NettingId]
         , 0                [CollationType]
      FROM [dbo].[Netting] n
     WHERE n.CompanyId          = @CompanyId
       AND n.CurrencyId         = COALESCE(@CurrencyId, n.CurrencyId)
       AND n.RecordedAt        <= @RecordedAt
       AND n.AssignmentFlag     = 0
       AND (@UseSectionWork = 0
         OR @UseSectionWork = 1
            AND n.SectionId IN (
                SELECT wst.SectionId
                  FROM [dbo].[WorkSectionTarget] wst
                 WHERE wst.ClientKey            = @ClientKey
                   AND wst.UseCollation         = 1 )
           )
       AND (@BillingType <> 2
         OR @BillingType  = 2 AND n.DueAt IS NULL
           );

    /* 入金ワークデータ登録 */
    INSERT INTO [dbo].[WorkReceipt]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey                           [ClientKey]
         , u.CompanyId
         , u.CurrencyId
         , u.PayerName
         , u.BankCode
         , u.BranchCode
         , u.PayerCode
         , u.SourceBankName
         , u.SourceBranchName
         , u.CollationKey
         , u.CustomerId
         , SUM( u.ReceiptAmount )               [ReceiptAmount]
         , SUM( u.AssignmentAmount )            [ReceiptAssignmentAmount]
         , SUM( u.RemainAmount )                [ReceiptRemainAmount]
         , COUNT(1)                             [ReceiptCount]
         , SUM( u.AdvanceReceivedCount )        [AdvanceReceivedCount]
         , MAX( u.ForceMatchingIndividually )   [ForceMatchingIndividually]
      FROM (

            /* 通常入金 */
            SELECT r.CompanyId
                 , r.CurrencyId
                 , r.PayerName
                 , COALESCE(rh.BankCode  , N'') [BankCode]
                 , COALESCE(rh.BranchCode, N'') [BranchCode]
                 , r.PayerCode
                 , r.SourceBankName
                 , r.SourceBranchName
                 , r.CollationKey
                 , COALESCE(r.CustomerId, 0)    [CustomerId]
                 , r.ReceiptAmount
                 , r.AssignmentAmount
                 , r.RemainAmount
                 , 1                            [ReceiptCount]
                 , CASE WHEN r.OriginalReceiptId > 0 THEN 1 ELSE 0 END [AdvanceReceivedCount]
                 , rc.ForceMatchingIndividually
              FROM [dbo].[WorkReceiptTarget] wrt
             INNER JOIN [dbo].[Receipt] r
                ON wrt.ClientKey        = @ClientKey
               AND wrt.ReceiptId        = r.Id
             INNER JOIN [dbo].[Category] rc
                ON rc.Id                = r.ReceiptCategoryId
              LEFT JOIN [dbo].[ReceiptHeader] rh
                ON rh.Id                = r.ReceiptHeaderId

             UNION ALL

            /* 入金予定相殺入力 */
            SELECT n.CompanyId
                 , n.CurrencyId
                 , cs.Kana              [PayerName]
                 , N''                  [BankCode]
                 , N''                  [BranchCode]
                 , N''                  [PayerCode]
                 , N''                  [SourceBankName]
                 , N''                  [SourceBranchName]
                 , N''                  [CollationKey]
                 , n.CustomerId
                 , n.Amount             [ReceiptAmount]
                 , 0                    [AssignmentAmount]
                 , n.Amount             [RemainAmount]
                 , 1                    [ReceiptCount]
                 , 0                    [AdvanceReceivedCount] 
                 , rc.ForceMatchingIndividually
              FROM [dbo].[WorkNettingTarget] wnt
             INNER JOIN [dbo].[Netting] n
                ON wnt.ClientKey        = @ClientKey
               AND wnt.NettingId        = n.Id
             INNER JOIN [dbo].[Customer] cs
                ON cs.Id                = n.CustomerId
             INNER JOIN [dbo].[Category] rc
                ON rc.Id                = n.ReceiptCategoryId

           ) u
     GROUP BY
           u.CompanyId
         , u.CurrencyId
         , u.PayerName
         , u.BankCode
         , u.BranchCode
         , u.PayerCode
         , u.SourceBankName
         , u.SourceBranchName
         , u.CollationKey
         , u.CustomerId

END;

GO
/****** Object:  StoredProcedure [dbo].[uspCollationKey]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspCollationKey]
 @ClientKey         VARBINARY(20)
AS
DECLARE
 @type              INT
BEGIN

    SET @type       = 11 /* 照合番号 */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey           [ClientKey]
         , wb.CompanyId
         , wb.CurrencyId
         , wb.CustomerId
         , 0                    [PaymentAgencyId]
         , wr.PayerName
         , wr.BankCode
         , wr.BranchCode
         , wr.PayerCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                [CollationType]
         , csb.Name             [ParentCustomerName]
         , csb.Kana             [ParentCustomerKana]
         , csb.ShareTransferFee [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[WorkBilling] wb
        ON wr.ClientKey         = @ClientKey
       AND wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND wr.CollationKey      > N''
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.CompanyId          = wr.CompanyId
              AND wc.CurrencyId         = wr.CurrencyId
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId )
     INNER JOIN [dbo].[Customer] csb
        ON csb.Id               = wb.CustomerId
       AND csb.CollationKey     = wr.CollationKey;

    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.CollationType     = @type;


END;

GO
/****** Object:  StoredProcedure [dbo].[uspCollationMissing]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspCollationMissing]
 @ClientKey         VARBINARY(20)
AS
DECLARE
 @type              INT
BEGIN

    SET @type       = 0 /* 未照合 */

    /* 請求データ登録 */
    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey           [ClientKey]
         , wb.CompanyId
         , wb.CurrencyId
         , wb.CustomerId
         , 0                    [PaymentAgencyId]
         , N''                  [PayerName]
         , N''                  [BankCode]
         , N''                  [BranchCode]
         , N''                  [PayerCode]
         , N''                  [SourceBankName]
         , N''                  [SourceBranchName]
         , N''                  [CollationKey]
         , 0                    [CustomerId]
         , @type                [CollationType]
         , cs.Name              [ParentCustomerName]
         , cs.Kana              [ParentCustomerKana]
         , cs.ShareTransferFee  [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , 0                    [ReceiptAmount]
         , 0                    [ReceiptAssignmentAmount]
         , 0                    [ReceiptRemainAmount]
         , 0                    [ReceiptCount]
         , 0                    [AdvanceReceivedCount]
         , 0                    [ForceMatchingIndividually]
      FROM [dbo].[WorkBilling] wb
     INNER JOIN [dbo].[Customer] cs
        ON wb.ClientKey     = @ClientKey
       AND cs.Id            = wb.CustomerId
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey              = wb.ClientKey
              AND wc.CompanyId              = wb.CompanyId
              AND wc.CurrencyId             = wb.CurrencyId
              AND wc.ParentCustomerId       = wb.CustomerId
              AND wc.PaymentAgencyId        = 0 )
     UNION ALL
    SELECT @ClientKey           [ClientKey]
         , wb.CompanyId
         , wb.CurrencyId
         , 0                    [ParentCustomerId]
         , wb.PaymentAgencyId
         , N''                  [PayerName]
         , N''                  [BankCode]
         , N''                  [BranchCode]
         , N''                  [PayerCode]
         , N''                  [SourceBankName]
         , N''                  [SourceBranchName]
         , N''                  [CollationKey]
         , 0                    [CustomerId]
         , @type                [CollationType]
         , cs.Name              [ParentCustomerName]
         , cs.Kana              [ParentCustomerKana]
         , cs.ShareTransferFee  [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , 0                    [ReceiptAmount]
         , 0                    [ReceiptAssignmentAmount]
         , 0                    [ReceiptRemainAmount]
         , 0                    [ReceiptRemainCount]
         , 0                    [AdvanceReceivedCount]
         , 0                    [ForceMatchingIndividually]
      FROM [dbo].[WorkBankTransfer] wb
     INNER JOIN [dbo].[PaymentAgency] cs
        ON wb.ClientKey     = @ClientKey
       AND cs.Id            = wb.PaymentAgencyId
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wb.ClientKey
              AND wc.CompanyId          = wb.CompanyId
              AND wc.CurrencyId         = wb.CurrencyId
              AND wc.ParentCustomerId   = 0
              AND wc.PaymentAgencyId    = wb.PaymentAgencyId )

    /* 入金データ登録 */
    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey
         , wr.CompanyId
         , wr.CurrencyId
         , 0                        [CustomerId]
         , 0                        [PaymentAgencyId]
         , wr.PayerName
         , wr.BankCode
         , wr.BranchCode
         , wr.PayerCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                    [CollationType]
         , N''                      [ParentCustomerName]
         , N''                      [ParentCustomerKana]
         , 0                        [ParentShareTransferFee]
         , 0                        [BillingAmount]
         , 0                        [BillingRemainAmount]
         , 0                        [BillingCount]
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually

      FROM [dbo].[WorkReceipt] wr
     WHERE wr.ClientKey     = @ClientKey
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId );

END;

GO
/****** Object:  StoredProcedure [dbo].[uspCollationPayerCode]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspCollationPayerCode]
 @ClientKey         VARBINARY(20)
AS
DECLARE
 @type              INT
BEGIN

    SET @type       = 10 /* 専用口座 子得意先 */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , PayerCode
         , BankCode
         , BranchCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey           [ClientKey]
         , wb.CompanyId
         , wb.CurrencyId
         , wb.CustomerId
         , 0                    [PaymentAgencyId]
         , wr.PayerName
         , wr.PayerCode
         , wr.BankCode
         , wr.BranchCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                [CollationType]
         , cs.Name              [ParentCustomerName]
         , cs.Kana              [ParentCustomerKana]
         , cs.ShareTransferFee  [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually
      FROM (
           SELECT wr.ClientKey
                , wr.CompanyId
                , wr.CurrencyId
                , wr.PayerName
                , wr.PayerCode
                , wr.BankCode
                , wr.BranchCode
                , wr.SourceBankName
                , wr.SourceBranchName
                , wr.CollationKey
                , wr.CustomerId
                , csg.ParentCustomerId
             FROM [dbo].[WorkReceipt] wr
            INNER JOIN [dbo].[Customer] cs
               ON wr.ClientKey              = @ClientKey
              AND wr.BankCode               > N''
              AND wr.BranchCode             > N''
              AND wr.PayerCode              > N''
              AND cs.ExclusiveBankCode      = wr.BankCode
              AND cs.ExclusiveBranchCode    = wr.BranchCode
              AND cs.ExclusiveAccountNumber = wr.PayerCode
              AND NOT EXISTS (
                 SELECT 1
                   FROM [dbo].[WorkCollation] wc
                  WHERE wc.ClientKey            = wr.ClientKey
                    AND wc.CompanyId            = wr.CompanyId
                    AND wc.CurrencyId           = wr.CurrencyId
                    AND wc.PayerName            = wr.PayerName
                    AND wc.PayerCode            = wr.PayerCode
                    AND wc.BankCode             = wr.BankCode
                    AND wc.BranchCode           = wr.BranchCode
                    AND wc.SourceBankName       = wr.SourceBankName
                    AND wc.SourceBranchName     = wr.SourceBranchName
                    AND wc.CollationKey         = wr.CollationKey
                    AND wc.CustomerId           = wr.CustomerId )
            INNER JOIN [dbo].[CustomerGroup] csg
               ON csg.ChildCustomerId       = cs.Id
              AND csg.ParentCustomerId     <> csg.ChildCustomerId
            GROUP BY
                  wr.ClientKey
                , wr.CompanyId
                , wr.CurrencyId
                , wr.PayerName
                , wr.PayerCode
                , wr.BankCode
                , wr.BranchCode
                , wr.SourceBankName
                , wr.SourceBranchName
                , wr.CollationKey
                , wr.CustomerId
                , csg.ParentCustomerId
            ) wr_temp
     INNER JOIN [dbo].[WorkReceipt] wr
        ON wr.ClientKey         = wr_temp.ClientKey
       AND wr.CompanyId         = wr_temp.CompanyId
       AND wr.CurrencyId        = wr_temp.CurrencyId
       AND wr.PayerName         = wr_temp.PayerName
       AND wr.PayerCode         = wr_temp.PayerCode
       AND wr.BankCode          = wr_temp.BankCode
       AND wr.BranchCode        = wr_temp.BranchCode
       AND wr.SourceBankName    = wr_temp.SourceBankName
       AND wr.SourceBranchName  = wr_temp.SourceBranchName
       AND wr.CollationKey      = wr_temp.CollationKey
       AND wr.CustomerId        = wr_temp.CustomerId
     INNER JOIN [dbo].[WorkBilling] wb
        ON wb.ClientKey     = @ClientKey
       AND wb.ClientKey     = wr.ClientKey
       AND wb.CompanyId     = wr.CompanyId
       AND wb.CurrencyId    = wr.CurrencyId
       AND wb.CustomerId    = wr_temp.ParentCustomerId
     INNER JOIN [dbo].[Customer] cs
        ON cs.Id            = wb.CustomerId

    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.CollationType     = @type

    SET @type       = 1  /* 専用入金口座 親 */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey
         , wb.CompanyId
         , wb.CurrencyId
         , wb.CustomerId
         , 0                        [PaymentAgencyId]
         , wr.PayerName
         , wr.BankCode
         , wr.BranchCode
         , wr.PayerCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                    [CollationType]
         , cs.Name                  [ParentCustomerName]
         , cs.Kana                  [ParentCustomerKana]
         , cs.ShareTransferFee      [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually

      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[Customer] cs
        ON wr.ClientKey     = @ClientKey
       AND wr.BankCode      > N''
       AND wr.BranchCode    > N''
       AND wr.PayerCode     > N''
       AND wr.BankCode      = cs.ExclusiveBankCode
       AND wr.BranchCode    = cs.ExclusiveBranchCode
       AND wr.PayerCode     = cs.ExclusiveAccountNumber
     INNER JOIN [dbo].[WorkBilling] wb
        ON wb.ClientKey     = @ClientKey
       AND wb.ClientKey     = wr.ClientKey
       AND wb.CompanyId     = wr.CompanyId
       AND wb.CurrencyId    = wr.CurrencyId
       AND wb.CustomerId    = cs.Id
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.CompanyId          = wr.CompanyId
              AND wc.CurrencyId         = wr.CurrencyId
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId );

    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.CollationType     = @type;

END;

GO
/****** Object:  StoredProcedure [dbo].[uspCollationPayerName]    Script Date: 2017/12/14 18:13:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspCollationPayerName]
 @ClientKey         VARBINARY(20)
AS
DECLARE
 @type              INT
BEGIN

    SET @type       = 4 /* カナ完全一致 */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey           [ClientKey]
         , wb.CompanyId
         , wb.CurrencyId
         , wb.CustomerId
         , 0                    [PaymentAgencyId]
         , wr.PayerName
         , wr.BankCode
         , wr.BranchCode
         , wr.PayerCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                [CollationType]
         , csb.Name             [ParentCustomerName]
         , csb.Kana             [ParentCustomerKana]
         , csb.ShareTransferFee [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[WorkBilling] wb
        ON wr.ClientKey         = @ClientKey
       AND wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND wb.CustomerKana      = wr.PayerName
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.CompanyId          = wr.CompanyId
              AND wc.CurrencyId         = wr.CurrencyId
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId )
     INNER JOIN [dbo].[Customer] csb
        ON csb.Id               = wb.CustomerId;

    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.CollationType     = @type;

    UPDATE [dbo].[WorkNettingTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkNettingTarget] wnt
     INNER JOIN [dbo].[Netting] n
        ON wnt.ClientKey        = @ClientKey
       AND wnt.NettingId        = n.Id
       AND wnt.CollationType    = 0
     INNER JOIN [dbo].[Customer] cs
        ON cs.Id                = n.CustomerId
     INNER JOIN [dbo].[WorkCollation] wc
        ON wc.ClientKey         = wnt.ClientKey
       AND wc.CompanyId         = n.CompanyId
       AND wc.CurrencyId        = n.CurrencyId
       AND wc.PayerName         = cs.Kana
       AND wc.BankCode          = N''
       AND wc.BranchCode        = N''
       AND wc.PayerCode         = N''
       AND wc.SourceBankName    = N''
       AND wc.SourceBranchName  = N''
       AND wc.CollationKey      = N''
       AND wc.CustomerId        = n.CustomerId
       AND wc.CollationType     = @type;


    SET @type       = 7  /* カナ照合 口座振替 */

    INSERT INTO [dbo].[WorkCollation]
         ( ClientKey
         , CompanyId
         , CurrencyId
         , ParentCustomerId
         , PaymentAgencyId
         , PayerName
         , BankCode
         , BranchCode
         , PayerCode
         , SourceBankName
         , SourceBranchName
         , CollationKey
         , CustomerId
         , CollationType
         , ParentCustomerName
         , ParentCustomerKana
         , ParentCustomerShareTransferFee
         , BillingAmount
         , BillingRemainAmount
         , BillingCount
         , ReceiptAmount
         , ReceiptAssignmentAmount
         , ReceiptRemainAmount
         , ReceiptCount
         , AdvanceReceivedCount
         , ForceMatchingIndividually )
    SELECT @ClientKey
         , wb.CompanyId
         , wb.CurrencyId
         , 0                    [ParentCustomerId]
         , wb.PaymentAgencyId
         , wr.PayerName
         , wr.BankCode
         , wr.BranchCode
         , wr.PayerCode
         , wr.SourceBankName
         , wr.SourceBranchName
         , wr.CollationKey
         , wr.CustomerId
         , @type                [CollationType]
         , csb.Name             [ParentCustomerName]
         , csb.Kana             [ParentCustomerKana]
         , csb.ShareTransferFee [ParentShareTransferFee]
         , wb.BillingAmount
         , wb.BillingRemainAmount
         , wb.BillingCount
         , wr.ReceiptAmount
         , wr.ReceiptAssignmentAmount
         , wr.ReceiptRemainAmount
         , wr.ReceiptCount
         , wr.AdvanceReceivedCount
         , wr.ForceMatchingIndividually
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[WorkBankTransfer] wb
        ON wr.ClientKey         = @ClientKey
       AND wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND wb.PaymentAgencyKana = wr.PayerName
       AND NOT EXISTS (
           SELECT 1
             FROM [dbo].[WorkCollation] wc
            WHERE wc.ClientKey          = wr.ClientKey
              AND wc.CompanyId          = wr.CompanyId
              AND wc.CurrencyId         = wr.CurrencyId
              AND wc.PayerName          = wr.PayerName
              AND wc.PayerCode          = wr.PayerCode
              AND wc.BankCode           = wr.BankCode
              AND wc.BranchCode         = wr.BranchCode
              AND wc.SourceBankName     = wr.SourceBankName
              AND wc.SourceBranchName   = wr.SourceBranchName
              AND wc.CollationKey       = wr.CollationKey
              AND wc.CustomerId         = wr.CustomerId )
     INNER JOIN [dbo].[PaymentAgency] csb
        ON csb.Id               = wb.PaymentAgencyId;


    UPDATE [dbo].[WorkReceiptTarget]
       SET CollationType        = @type
      FROM [dbo].[WorkReceiptTarget] wrt
     INNER JOIN [dbo].[WorkCollation] wc
        ON wrt.ClientKey        = @ClientKey
       AND wrt.CollationType    = 0
       AND wc.ClientKey         = wrt.ClientKey
       AND wc.CompanyId         = wrt.CompanyId
       AND wc.CurrencyId        = wrt.CurrencyId
       AND wc.PayerName         = wrt.PayerName
       AND wc.BankCode          = wrt.BankCode
       AND wc.BranchCode        = wrt.BranchCode
       AND wc.PayerCode         = wrt.PayerCode
       AND wc.SourceBankName    = wrt.SourceBankName
       AND wc.SourceBranchName  = wrt.SourceBranchName
       AND wc.CollationKey      = wrt.CollationKey
       AND wc.CustomerId        = wrt.CustomerId
       AND wc.CollationType     = @type;

END;

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'勘定科目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'勘定科目コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'勘定科目名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相手科目コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'ContraAccountCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相手科目名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'ContraAccountName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相手科目補助コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'ContraAccountSubCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTitle', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼データ明細ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼データログID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'AccountTransferLogId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'BillingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'担当者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'BilledAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'売上日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'SalesAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求締日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'ClosingAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'DueAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求金額（税込）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'BillingAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求書番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'InvoiceCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考１' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'Note1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用銀行名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用支店名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferBranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用預金種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferAccountTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用口座番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferAccountNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用預金者名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferAccountName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用顧客コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferCustomerCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用新規コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferDetail', @level2type=N'COLUMN',@level2name=N'TransferNewCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼データログID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回収区分ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CollectCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決算代行会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼作成日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'RequestDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'引落日（入金予定日）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'DueAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'OutputCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'OutputAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccountTransferLog', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金ヘッダーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'ReceiptHeaderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'ReceiptCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'SectionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入力区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'InputType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金振分フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'Apportioned'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'※未使用 承認フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'Approved'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作成日(当日営業日)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'Workday'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金日(計上日)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'RecordedAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前回入金日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'OriginalRecordedAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'ReceiptAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'AssignmentAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'RemainAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込フラグ 0:未消込 1:一部消込 2:消込済' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'AssignmentFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'PayerCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'PayerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名（未編集）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'PayerNameRaw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向先銀行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'SourceBankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向先支店名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕訳日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'OutputAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金期日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'DueAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メール配信日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'MailedAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'元入金データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'OriginalReceiptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'対象外フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'ExcludeFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'対象外区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'ExcludeCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'対象外金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'ExcludeAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'依頼人Ref.No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'ReferenceNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記録番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'RecordNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'電子記録年月日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'DensaiRegisterAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'摘要 / 備考1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'Note1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'Note2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'Note3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'Note4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手形番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'BillNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'券面銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'BillBankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'券面支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'BillBranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振出日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'BillDrawAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振出人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'BillDrawer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'削除日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'DeleteAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金メモ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振替仕訳日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'TransferOutputAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AdvanceReceivedBackup', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求部門コード桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'DepartmentCodeLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求部門コード文字種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'DepartmentCodeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門コード桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'SectionCodeLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門コード文字種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'SectionCodeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'科目コード桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'AccountTitleCodeLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'科目コード文字種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'AccountTitleCodeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先コード桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'CustomerCodeLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先コード文字種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'CustomerCodeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'担当者コード桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'LoginUserCodeLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'担当者コード文字種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'LoginUserCodeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者コード桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'StaffCodeLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者コード文字種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'StaffCodeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求部門管理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseDepartment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定入力利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseScheduledPayment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門管理利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseReceiptSection'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'承認機能利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseAuthorization'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'長期前受管理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseLongTermAdvanceReceived'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'契約マスターの事前登録' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'RegisterContractInAdvance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'期日現金管理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseCashOnDueDates'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'予定額を消込対象額に使用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseDeclaredAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'歩引き対応' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseDiscount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外貨対応' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseForeignCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求絞込機能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseBillingFilter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配信機能利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseDistribution'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作ログ利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UseOperationLogging'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'スタンダード/エントリー モデル' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'ApplicationEdition'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'フォルダ選択の制限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'LimitAccessFolder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ルートフォルダのパス' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationControl', @level2type=N'COLUMN',@level2name=N'RootPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行口座ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'BankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'BranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'AccountTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'AccountNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'BankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'BranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金種別ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'ReceiptCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'SectionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金日指定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'UseValueDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込対象外' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'ImportSkipping'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccount', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預金種別ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccountType', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預金種別名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccountType', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通常入金で利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccountType', @level2type=N'COLUMN',@level2name=N'UseReceipt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替で利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankAccountType', @level2type=N'COLUMN',@level2name=N'UseTransfer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'BankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'BranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'BankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行名カナ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'BankKana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'BranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店名カナ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'BranchKana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BankBranch', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'売上部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'担当者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'BillingCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入力区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'InputType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求入力ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'BillingInputId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'BilledAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求締日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'ClosingAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'売上日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'SalesAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'DueAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'BillingAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'うち消費税' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'TaxAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'AssignmentAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'RemainAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定違算額 入金予定金額 入金予定入力(違算)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'OffsetAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'AssignmentFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'承認フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Approved'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回収区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'CollectCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回収区分退避用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'OriginalCollectCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'売掛金 借方科目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'DebitAccountTitleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'売上 貸方科目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'CreditAccountTitleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'元入金予定日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'OriginalDueAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕訳日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'OutputAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求書発行日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'PublishAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求書番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'InvoiceCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消費税属性' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'TaxClassId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品名・摘要' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Note1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Note2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Note3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Note4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'削除日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'DeleteAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼作成日 作成依頼出力済フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'RequestDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'ResultCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'元入金予定日（口座振替用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'TransferOriginalDueAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定キー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'ScheduledPaymentKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Quantity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'単価' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'UnitPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'単位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'UnitSymbol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'Price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替依頼データログID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Billing', @level2type=N'COLUMN',@level2name=N'AccountTransferLogId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalance', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalance', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalance', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalance', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalance', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'繰越日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalance', @level2type=N'COLUMN',@level2name=N'CarryOverAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'繰越残高' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalance', @level2type=N'COLUMN',@level2name=N'BalanceCarriedOver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalance', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalance', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalanceBack', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalanceBack', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalanceBack', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalanceBack', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalanceBack', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'繰越日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalanceBack', @level2type=N'COLUMN',@level2name=N'CarryOverAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'繰越残高' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalanceBack', @level2type=N'COLUMN',@level2name=N'BalanceCarriedOver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalanceBack', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingBalanceBack', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDiscount', @level2type=N'COLUMN',@level2name=N'BillingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'歩引種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDiscount', @level2type=N'COLUMN',@level2name=N'DiscountType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'歩引額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDiscount', @level2type=N'COLUMN',@level2name=N'DiscountAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDiscount', @level2type=N'COLUMN',@level2name=N'AssignmentFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'長期前受契約ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'BillingDivisionContractId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分割ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'DivisionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分割金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'DivisionAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'税区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'TaxClassId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'債権科目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'DebitAccountTitleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分割計上日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'RecordedAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕訳出力日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'OutputAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivision', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'長期前受契約ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'契約番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'ContractNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'初回計上日決定方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'FirstDateType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計上サイクル' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'Monthly'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'基準日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'BasisDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'DivisionCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小数点以下扱い' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'RoundingMode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端数処理 残余分配方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'RemainsApportionment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'BillingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'BillingAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'確定フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'Comfirm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'解約日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'CancelDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionContract', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'初回計上日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'FirstDateType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計上サイクル' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'Monthly'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'基準日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'BasisDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分割回数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'DivisionCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小数点以下扱い' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'RoundingMode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端数処理 残余分配方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'RemainsApportionment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingDivisionSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingInput', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingMemo', @level2type=N'COLUMN',@level2name=N'BillingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求メモ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingMemo', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新請求データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingScheduledIncome', @level2type=N'COLUMN',@level2name=N'BillingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingScheduledIncome', @level2type=N'COLUMN',@level2name=N'MatchingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingScheduledIncome', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BillingScheduledIncome', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'明細分類ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'明細分類識別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'CategoryType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'明細分類コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'明細分類名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'科目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'AccountTitleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'SubCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'Note'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'税区分(消費税属性)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'TaxClassId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'期日入力可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'UseLimitDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'長期前受管理使用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'UseLongTermAdvanceReceived'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'期日入金管理使用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'UseCashOnDueDates'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替使用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'UseAccountTransfer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'歩引利用フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'UseDiscount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前受フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'UseAdvanceReceived'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込対象外' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'ForceMatchingIndividually'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入力画面利用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'UseInput'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込順序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'MatchingOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'StringValue1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'StringValue2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'StringValue3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'StringValue4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'StringValue5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助整数1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'IntValue1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助整数2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'IntValue2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助整数3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'IntValue3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助整数4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'IntValue4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助整数5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'IntValue5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助金額1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'NumericValue1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助金額2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'NumericValue2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助金額3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'NumericValue3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助金額4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'NumericValue4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助金額5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'NumericValue5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外部コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'ExternalCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区分識別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CategoryBase', @level2type=N'COLUMN',@level2name=N'CategoryType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区分コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CategoryBase', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区分名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CategoryBase', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'税区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CategoryBase', @level2type=N'COLUMN',@level2name=N'TaxClassId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'期日入力可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CategoryBase', @level2type=N'COLUMN',@level2name=N'UseLimitDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前受フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CategoryBase', @level2type=N'COLUMN',@level2name=N'UseAdvanceReceived'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入力画面利用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CategoryBase', @level2type=N'COLUMN',@level2name=N'UseInput'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区分マスターベース' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CategoryBase'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationOrder', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合ロジックタイプ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationOrder', @level2type=N'COLUMN',@level2name=N'CollationTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合順序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationOrder', @level2type=N'COLUMN',@level2name=N'ExecutionOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationOrder', @level2type=N'COLUMN',@level2name=N'Available'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationOrder', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationOrder', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationOrder', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationOrder', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先コードセットを必須にする' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'RequiredCustomer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先コードを自動でセットする' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'AutoAssignCustomer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'学習履歴機能を利用する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'LearnKanaHistory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金データ振分画面を使用する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'UseApportionMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自動更新を許可する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'ReloadCollationData'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前受振替を使用する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'UseAdvanceReceived'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前受伝票日付設定方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'AdvanceReceivedRecordedDateType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込チェックONのデータを消込実行させておく' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'AutoMatching'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込チェックONのソートをあらかじめ実行しておく' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'AutoSortMatchingEnabledData'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金が複数件の場合は一括消込対象から外す' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'PrioritizeMatchingIndividuallyMultipleReceipts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金額一致していても手数料自社負担の場合には、一括消込対象から外す' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'ForceShareTransferFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金データ修正時に得意先を付与した場合、学習履歴に登録する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'LearnSpecifiedCustomerKana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'個別消込時の消込順序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'MatchingSilentSortedData'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金ﾃﾞｰﾀ取込時、振込依頼人名のｽﾍﾟｰｽ除去' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'RemoveSpaceFromPayerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'差額が消費税誤差の範囲内でも一括消込対象外から外す（消費税誤差時に、個別消込優先）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'PrioritizeMatchingIndividuallyTaxTolerance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕訳出力内容設定 0:標準, 1:汎用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CollationSetting', @level2type=N'COLUMN',@level2name=N'JournalizingPattern'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnNameSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'テーブル名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnNameSetting', @level2type=N'COLUMN',@level2name=N'TableName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'カラム名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnNameSetting', @level2type=N'COLUMN',@level2name=N'ColumnName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'元名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnNameSetting', @level2type=N'COLUMN',@level2name=N'OriginalName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ユーザー設定名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnNameSetting', @level2type=N'COLUMN',@level2name=N'Alias'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnNameSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnNameSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnNameSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ColumnNameSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社カナ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Kana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵便番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'PostalCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'住所1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Address1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'住所2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Address2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'電話番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Tel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FAX番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'Fax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'プロダクトキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'ProductKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込口座名義' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'BankAccountName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込口座カナ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'BankAccountKana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込銀行1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'BankName1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込支店1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'BranchName1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込口座種別1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'AccountType1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込口座番号1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'AccountNumber1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込銀行2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'BankName2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込支店2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'BranchName2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込口座種別2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'AccountType2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込口座番号2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'AccountNumber2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込銀行3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'BankName3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込支店3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'BranchName3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込口座種別3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'AccountType3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込口座番号3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'AccountNumber3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'締め日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'ClosingDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'確認メッセージを表示する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'ShowConfirmDialog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'検索時に全件初期表示する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'PresetCodeSearchDialog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'警告時ダイアログを表示する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'ShowWarningDialog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替の作成/取込時に口座単位で集計する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'TransferAggregate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'処理状況ダイアログを自動で閉じる' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Company', @level2type=N'COLUMN',@level2name=N'AutoCloseProgressDialog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanyLogo', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ロゴ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanyLogo', @level2type=N'COLUMN',@level2name=N'Logo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanyLogo', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanyLogo', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanyLogo', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanyLogo', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanySetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'設定キーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanySetting', @level2type=N'COLUMN',@level2name=N'KeyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'設定値' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanySetting', @level2type=N'COLUMN',@level2name=N'Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanySetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanySetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanySetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanySetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'LoginUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'フォーム背景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'FormBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'フォーム前景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'FormForeColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'コントロール有効時の背景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'ControlEnableBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'コントロール無効時の背景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'ControlDisableBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'コントロール前景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'ControlForeColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入力必須コントロールの背景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'ControlRequiredBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'コントロールフォーカス時の背景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'ControlActiveBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ボタンコントロール背景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'ButtonBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'明細行1行目背景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'GridRowBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'明細行2行目背景色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'GridAlternatingRowBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'グリッド罫線色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'GridLineColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入力1?' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'InputGridBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入力2?' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'InputGridAlternatingBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込請求側' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'MatchingGridBillingBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込残額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'MatchingGridReceiptBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'個別消込請求セル色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'MatchingGridBillingSelectedRowBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'個別消込請求選択セル色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'MatchingGridBillingSelectedCellBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'個別消込入金セル色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'MatchingGridReceiptSelectedRowBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'個別消込入金選択セル色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'MatchingGridReceiptSelectedCellBackColor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ControlColor', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨記号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'Symbol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小数点以下桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'Precision'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'Note'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'誤差許容値' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'Tolerance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Currency', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先カナ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'Kana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵便番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'PostalCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'住所1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'Address1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'住所2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'Address2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'電話番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'Tel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FAX番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'Fax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相手先担当者名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CustomerStaffName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'専用口座銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ExclusiveBankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'専用口座銀行名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ExclusiveBankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'専用口座支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ExclusiveBranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'専用口座支店名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ExclusiveBranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'専用口座番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ExclusiveAccountNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'専用入金 預金種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ExclusiveAccountTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手数料負担区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ShareTransferFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'与信限度額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CreditLimit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'締日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ClosingDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回収区分ID(支払方法)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CollectCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定日計算 月' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CollectOffsetMonth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定日計算 日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CollectOffsetDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'債権代表者フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'IsParent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'Note'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回収サイト' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'SightOfBill'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'電子手形用企業コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'DensaiCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'信用調査用企業コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CreditCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'与信ランク' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CreditRank'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferBankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用銀行名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferBankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferBranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用支店名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferBranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用口座番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferAccountNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用預金種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferAccountTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被振込口座１' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ReceiveAccountId1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被振込口座２' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ReceiveAccountId2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被振込口座３' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'ReceiveAccountId3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手数料自動学習フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'UseFeeLearning'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'カナ自動学習' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'UseKanaLearning'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'休業日設定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'HolidayFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手数料誤差利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'UseFeeTolerance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'StringValue1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'StringValue2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'StringValue3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'StringValue4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'StringValue5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助整数1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'IntValue1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助整数2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'IntValue2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助整数3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'IntValue3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助整数4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'IntValue4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助整数5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'IntValue5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助金額1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'NumericValue1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助金額2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'NumericValue2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助金額3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'NumericValue3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助金額4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'NumericValue4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助金額5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'NumericValue5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用顧客コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferCustomerCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用新規コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferNewCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座振替用預金者名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'TransferAccountName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込対象外' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'PrioritizeMatchingIndividually'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customer', @level2type=N'COLUMN',@level2name=N'CollationKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1..5 までのキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'Sequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'歩引き率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'Rate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端数処理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'RoundingMode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最低実行金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'MinValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'科目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'AccountTitleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'SubCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerDiscount', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerFee', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerFee', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手数料金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerFee', @level2type=N'COLUMN',@level2name=N'Fee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerFee', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerFee', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerFee', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerFee', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'親得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerGroup', @level2type=N'COLUMN',@level2name=N'ParentCustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerGroup', @level2type=N'COLUMN',@level2name=N'ChildCustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerGroup', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerGroup', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerGroup', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerGroup', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'約定金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'ThresholdValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'約定金額未満回収区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'LessThanCollectCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'約定金額以上回収区分１' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanCollectCategoryId1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分割１' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanRate1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端数処理１' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanRoundingMode1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回収サイト１' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanSightOfBill1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'約定金額以上回収区分２' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanCollectCategoryId2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分割２' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanRate2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端数処理２' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanRoundingMode2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回収サイト２' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanSightOfBill2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'約定金額以上回収区分３' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanCollectCategoryId3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分割３' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanRate3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端数処理３' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanRoundingMode3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回収サイト３' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'GreaterThanSightOfBill3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerPaymentContract', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DensaiRemoveWord', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'削除対象文字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DensaiRemoveWord', @level2type=N'COLUMN',@level2name=N'Word'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DensaiRemoveWord', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DensaiRemoveWord', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DensaiRemoveWord', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DensaiRemoveWord', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'削除対象文字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DensaiRemoveWordBase', @level2type=N'COLUMN',@level2name=N'Word'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部門コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部門名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回収責任担当者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Note'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBExcludeAccountSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBExcludeAccountSetting', @level2type=N'COLUMN',@level2name=N'BankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBExcludeAccountSetting', @level2type=N'COLUMN',@level2name=N'BranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預金種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBExcludeAccountSetting', @level2type=N'COLUMN',@level2name=N'AccountTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBExcludeAccountSetting', @level2type=N'COLUMN',@level2name=N'PayerCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBExcludeAccountSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBExcludeAccountSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBExcludeAccountSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBExcludeAccountSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EBデータ取込対象外口座設定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBExcludeAccountSetting'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'LoginUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外貨データフラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'IsForeignCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込フォーマット' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'FileFormat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込ファイルパス' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EBFileSetting', @level2type=N'COLUMN',@level2name=N'FilePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力ファイルタイプ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSetting', @level2type=N'COLUMN',@level2name=N'ExportFileType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力対象列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSetting', @level2type=N'COLUMN',@level2name=N'ColumnName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSetting', @level2type=N'COLUMN',@level2name=N'ColumnOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSetting', @level2type=N'COLUMN',@level2name=N'AllowExport'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'データ書式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSetting', @level2type=N'COLUMN',@level2name=N'DataFormat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力ファイルタイプ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSettingBase', @level2type=N'COLUMN',@level2name=N'ExportFileType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力対象列名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSettingBase', @level2type=N'COLUMN',@level2name=N'ColumnName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力列名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSettingBase', @level2type=N'COLUMN',@level2name=N'Caption'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSettingBase', @level2type=N'COLUMN',@level2name=N'ColumnOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSettingBase', @level2type=N'COLUMN',@level2name=N'AllowExport'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'データ種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExportFieldSettingBase', @level2type=N'COLUMN',@level2name=N'DataType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FunctionAuthority', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'権限レベル' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FunctionAuthority', @level2type=N'COLUMN',@level2name=N'AuthorityLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機能名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FunctionAuthority', @level2type=N'COLUMN',@level2name=N'FunctionType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'利用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FunctionAuthority', @level2type=N'COLUMN',@level2name=N'Available'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FunctionAuthority', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FunctionAuthority', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FunctionAuthority', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FunctionAuthority', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理マスターID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'データ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'有効桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'Length'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小数点以下桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'Precision'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'説明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSettingBase', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'データ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSettingBase', @level2type=N'COLUMN',@level2name=N'Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'有効桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSettingBase', @level2type=N'COLUMN',@level2name=N'Length'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小数点以下桁数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSettingBase', @level2type=N'COLUMN',@level2name=N'Precision'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'説明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GeneralSettingBase', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSetting', @level2type=N'COLUMN',@level2name=N'LoginUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'グリッドID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSetting', @level2type=N'COLUMN',@level2name=N'GridId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'カラム名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSetting', @level2type=N'COLUMN',@level2name=N'ColumnName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSetting', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示幅' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSetting', @level2type=N'COLUMN',@level2name=N'DisplayWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'グリッドID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSettingBase', @level2type=N'COLUMN',@level2name=N'GridId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'カラム名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSettingBase', @level2type=N'COLUMN',@level2name=N'ColumnName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'カラム名（日本語）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSettingBase', @level2type=N'COLUMN',@level2name=N'ColumnNameJp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSettingBase', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示幅' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GridSettingBase', @level2type=N'COLUMN',@level2name=N'DisplayWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HolidayCalendar', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'休業日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HolidayCalendar', @level2type=N'COLUMN',@level2name=N'Holiday'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IgnoreKana', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'除外カナ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IgnoreKana', @level2type=N'COLUMN',@level2name=N'Kana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'対象外区分ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IgnoreKana', @level2type=N'COLUMN',@level2name=N'ExcludeCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IgnoreKana', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IgnoreKana', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IgnoreKana', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IgnoreKana', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'フォーマットID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterFormat', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'フォーマット名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterFormat', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'インポーター設定ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'パターン区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'FormatId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'パターンNo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'パターン名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込フォルダ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'InitialDirectory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ファイルエンコーディング' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'EncodingCodePage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込開始行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'StartLineCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最終行を取込まない' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'IgnoreLastLine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先マスター自動作成フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'AutoCreationCustomer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込後ファイル操作' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'PostAction'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込フォーマットID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingBase', @level2type=N'COLUMN',@level2name=N'FormatId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込項目行番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingBase', @level2type=N'COLUMN',@level2name=N'Sequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingBase', @level2type=N'COLUMN',@level2name=N'FieldName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'対象列名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingBase', @level2type=N'COLUMN',@level2name=N'TargetColumn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込区分情報' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingBase', @level2type=N'COLUMN',@level2name=N'ImportDivision'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性区分情報' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingBase', @level2type=N'COLUMN',@level2name=N'AttributeDivision'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'インポーター設定ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'ImporterSettingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込項目行番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'Sequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込有無' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'ImportDivision'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'項目番号（CSVファイル何番目か）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'FieldIndex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'項目名（注釈）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'Caption'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'属性情報' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'AttributeDivision'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'項目内優先順位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'ItemPriority'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上書キー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'DoOverwrite'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重複チェック' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'IsUnique'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'固定値' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'FixedValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'UpdateKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImporterSettingDetail', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込ファイルID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportFileLog', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportFileLog', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ファイル名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportFileLog', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ファイルサイズ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportFileLog', @level2type=N'COLUMN',@level2name=N'FileSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportFileLog', @level2type=N'COLUMN',@level2name=N'ReadCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportFileLog', @level2type=N'COLUMN',@level2name=N'ImportCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportFileLog', @level2type=N'COLUMN',@level2name=N'ImportAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportFileLog', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImportFileLog', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InputControl', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InputControl', @level2type=N'COLUMN',@level2name=N'LoginUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入力制御用コントロールID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InputControl', @level2type=N'COLUMN',@level2name=N'InputGridTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InputControl', @level2type=N'COLUMN',@level2name=N'ColumnName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列名（日本語）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InputControl', @level2type=N'COLUMN',@level2name=N'ColumnNameJp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'列順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InputControl', @level2type=N'COLUMN',@level2name=N'ColumnOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'タブ移動許可' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InputControl', @level2type=N'COLUMN',@level2name=N'TabStop'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'タブ移動順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InputControl', @level2type=N'COLUMN',@level2name=N'TabIndex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JuridicalPersonality', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'法人格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JuridicalPersonality', @level2type=N'COLUMN',@level2name=N'Kana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JuridicalPersonality', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JuridicalPersonality', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JuridicalPersonality', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JuridicalPersonality', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'法人格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JuridicalPersonalityBase', @level2type=N'COLUMN',@level2name=N'Kana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryCustomer', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名（カナ）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryCustomer', @level2type=N'COLUMN',@level2name=N'PayerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向銀行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryCustomer', @level2type=N'COLUMN',@level2name=N'SourceBankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向支店' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryCustomer', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryCustomer', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込回数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryCustomer', @level2type=N'COLUMN',@level2name=N'HitCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryCustomer', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryCustomer', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryCustomer', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryCustomer', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryPaymentAgency', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名（カナ）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryPaymentAgency', @level2type=N'COLUMN',@level2name=N'PayerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向銀行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryPaymentAgency', @level2type=N'COLUMN',@level2name=N'SourceBankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向支店' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryPaymentAgency', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryPaymentAgency', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込回数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryPaymentAgency', @level2type=N'COLUMN',@level2name=N'HitCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryPaymentAgency', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryPaymentAgency', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryPaymentAgency', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KanaHistoryPaymentAgency', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogData', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogData', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端末名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogData', @level2type=N'COLUMN',@level2name=N'ClientName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogData', @level2type=N'COLUMN',@level2name=N'LoggedAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogData', @level2type=N'COLUMN',@level2name=N'LoginUserCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザー名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogData', @level2type=N'COLUMN',@level2name=N'LoginUserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'画面ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogData', @level2type=N'COLUMN',@level2name=N'MenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'画面名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogData', @level2type=N'COLUMN',@level2name=N'MenuName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogData', @level2type=N'COLUMN',@level2name=N'OperationName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザーコード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザー名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メールアドレス' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'Mail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'権限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'MenuLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機能制限レベル' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'FunctionLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアント利用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'UseClient'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Web Viewer利用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'UseWebViewer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'AssignedStaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'StringValue1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'StringValue2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'StringValue3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'StringValue4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補助文字列5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'StringValue5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUser', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserLicense', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ライセンスキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserLicense', @level2type=N'COLUMN',@level2name=N'LicenseKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'LoginUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'パスワードハッシュ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'履歴0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash0'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'履歴1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'履歴2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'履歴3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'履歴4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'履歴5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash5'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'履歴6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash6'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'履歴7' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash7'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'履歴8' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash8'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'履歴9' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LoginUserPassword', @level2type=N'COLUMN',@level2name=N'PasswordHash9'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMTPサーバー名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'SmtpServerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMTPポート番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'SmtpPortNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMTPユーザー名 encrypted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'SmtpUserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMTPパスワード encrypted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'SmtpPassword'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMTP認証の可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'UseSmtpAuthentication'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SSL通信利用の可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'UseSsL'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メールテンプレートID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メール区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'MailType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'テンプレートNo.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'テンプレート名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メール件名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'敬称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'Honorific'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メール本文' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'Body'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailTemplate', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込フォーマットID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'ImportFileType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込フォーマット名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'ImportFileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'ImportMode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'エラーログ出力要否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'ExportErrorLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'エラーログ出力先' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'ErrorLogDestination'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込時確認要否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'Confirm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込フォーマットID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSettingBase', @level2type=N'COLUMN',@level2name=N'ImportFileType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込フォーマット名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSettingBase', @level2type=N'COLUMN',@level2name=N'ImportFileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSettingBase', @level2type=N'COLUMN',@level2name=N'ImportMode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'エラーログ出力要否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSettingBase', @level2type=N'COLUMN',@level2name=N'ExportErrorLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'エラーログ出力先' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSettingBase', @level2type=N'COLUMN',@level2name=N'ErrorLogDestination'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込時確認要否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MasterImportSettingBase', @level2type=N'COLUMN',@level2name=N'Confirm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'ReceiptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'BillingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込ヘッダーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'MatchingHeaderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込手数料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'BankTransferFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込完了時の請求残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'BillingRemain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込完了時の入金残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'ReceiptRemain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前受発生フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'AdvanceReceivedOccured'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込伝票日付（入金日）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'RecordedAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消費税差異' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'TaxDifference'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕訳日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'OutputAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Matching', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingBillingDiscount', @level2type=N'COLUMN',@level2name=N'MatchingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'歩引種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingBillingDiscount', @level2type=N'COLUMN',@level2name=N'DiscountType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'歩引額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingBillingDiscount', @level2type=N'COLUMN',@level2name=N'DiscountAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込ヘッダーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'債権代表者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'承認フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'Approved'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'ReceiptCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'BillingCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込額合計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込手数料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'BankTransferFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消費税差異' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'TaxDifference'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込処理方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'MatchingProcessType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込メモ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingHeader', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrder', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'データ種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrder', @level2type=N'COLUMN',@level2name=N'TransactionCategory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'項目名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrder', @level2type=N'COLUMN',@level2name=N'ItemName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込順序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrder', @level2type=N'COLUMN',@level2name=N'ExecutionOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrder', @level2type=N'COLUMN',@level2name=N'Available'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'昇順・降順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrder', @level2type=N'COLUMN',@level2name=N'SortOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrder', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrder', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrder', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrder', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'データ種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrderBase', @level2type=N'COLUMN',@level2name=N'TransactionCategory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'項目名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrderBase', @level2type=N'COLUMN',@level2name=N'ItemName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込順序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrderBase', @level2type=N'COLUMN',@level2name=N'ExecutionOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrderBase', @level2type=N'COLUMN',@level2name=N'Available'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'昇順・降順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrderBase', @level2type=N'COLUMN',@level2name=N'SortOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込順序設定ベース' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOrderBase'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込ヘッダーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOutputed', @level2type=N'COLUMN',@level2name=N'MatchingHeaderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MatchingOutputed', @level2type=N'COLUMN',@level2name=N'OutputAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'画面ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'MenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'画面名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'MenuName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'タブ名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'MenuCategory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メニュー順序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'Sequence'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標準メニュー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'IsStandard'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定入力利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseScheduledPayment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門管理利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseReceiptSection'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'承認機能利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseAuthorization'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'長期前受管理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseLongTermAdvanceReceived'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'期日現金管理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseCashOnDueDates'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'歩引き対応' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseDiscount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外貨対応' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseForeignCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外貨対応時未使用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'NotUseForeignCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求絞込機能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseBillingFilter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配信機能利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseDistribution'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作ログ利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UseOperationLogging'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Menu', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MenuAuthority', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メニューID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MenuAuthority', @level2type=N'COLUMN',@level2name=N'MenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'権限レベル' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MenuAuthority', @level2type=N'COLUMN',@level2name=N'AuthorityLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'利用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MenuAuthority', @level2type=N'COLUMN',@level2name=N'Available'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MenuAuthority', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MenuAuthority', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MenuAuthority', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MenuAuthority', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金予定相殺ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'ReceiptCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'SectionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'ReceiptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'RecordedAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'期日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'DueAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'AssignmentFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'適用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'Note'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金メモ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Netting', @level2type=N'COLUMN',@level2name=N'ReceiptMemo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperationLoggingSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'設定項目名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperationLoggingSetting', @level2type=N'COLUMN',@level2name=N'SettingKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'設定値文字列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperationLoggingSetting', @level2type=N'COLUMN',@level2name=N'Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperationLoggingSetting', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperationLoggingSetting', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperationLoggingSetting', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperationLoggingSetting', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最小パスワード文字数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'MinLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大パスワード文字数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'MaxLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'アルファベット利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'UseAlphabet'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'アルファベット最低使用回数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'MinAlphabetUseCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数字利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'UseNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数字最低使用回数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'MinNumberUseCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記号利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'UseSymbol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記号最低使用回数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'MinSymbolUseCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記号種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'SymbolType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CaseSensitive/Insensitive' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'CaseSensitive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'同じ文字を連続して使用しない' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'MinSameCharacterRepeat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失効日数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'ExpirationDays'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'パスワード履歴保存数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PasswordPolicy', @level2type=N'COLUMN',@level2name=N'HistoryCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社カナ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'Kana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'委託者コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'ConsigneeCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手数料負担区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'ShareTransferFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手数料誤差利用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'UseFeeTolerance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手数料自動学習' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'UseFeeLearning'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'カナ自動学習' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'UseKanaLearning'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'予定日指定日数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'DueDateOffset'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'BankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'BankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'BranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'BranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預金種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'AccountTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'AccountNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'フォーマット' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'FileFormatId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振替不能時回収区分自動更新' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'ConsiderUncollected'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自動更新する区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'CollectCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出力ファイル名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'OutputFileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自動的に日付を付与する' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgency', @level2type=N'COLUMN',@level2name=N'AppendDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgencyFee', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgencyFee', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手数料金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgencyFee', @level2type=N'COLUMN',@level2name=N'Fee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgencyFee', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgencyFee', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgencyFee', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentAgencyFee', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行ファイルフォーマットID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentFileFormat', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行ファイルフォーマット名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentFileFormat', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentFileFormat', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用可否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentFileFormat', @level2type=N'COLUMN',@level2name=N'Available'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'引落年の要否' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PaymentFileFormat', @level2type=N'COLUMN',@level2name=N'IsNeedYear'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金ヘッダーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'ReceiptHeaderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'ReceiptCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'SectionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入力区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'InputType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金振分フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'Apportioned'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'※未使用 承認フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'Approved'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作成日(当日営業日)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'Workday'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金日(計上日)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'RecordedAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前回入金日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'OriginalRecordedAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'ReceiptAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'AssignmentAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'RemainAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込フラグ 0:未消込 1:一部消込 2:消込済' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'AssignmentFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'PayerCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'PayerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名（未編集）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'PayerNameRaw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向先銀行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'SourceBankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向先支店名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕訳日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'OutputAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金期日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'DueAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メール配信日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'MailedAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'元入金データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'OriginalReceiptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'対象外フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'ExcludeFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'対象外区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'ExcludeCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'対象外金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'ExcludeAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'依頼人Ref.No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'ReferenceNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記録番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'RecordNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'電子記録年月日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'DensaiRegisterAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'摘要 / 備考1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'Note1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'Note2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'Note3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'Note4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手形番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'BillNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'券面銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'BillBankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'券面支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'BillBranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振出日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'BillDrawAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振出人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'BillDrawer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'削除日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'DeleteAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'処理予定日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'ProcessingAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'StaffId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Receipt', @level2type=N'COLUMN',@level2name=N'CollationKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金対象外ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptExclude', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptExclude', @level2type=N'COLUMN',@level2name=N'ReceiptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'対象外金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptExclude', @level2type=N'COLUMN',@level2name=N'ExcludeAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'対象外区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptExclude', @level2type=N'COLUMN',@level2name=N'ExcludeCategoryId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕訳日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptExclude', @level2type=N'COLUMN',@level2name=N'OutputAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptExclude', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptExclude', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptExclude', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptExclude', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金EB取込ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ファイルタイプ EB/でんさい など' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'FileType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込ファイルID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'ImportFileLogId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作成日(当日営業日)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'Workday'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'BankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'BankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'BranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'BranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預金種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'AccountTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'AccountNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'口座名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'AccountName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消込フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'AssignmentFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'ImportCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'ImportAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptHeader', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptMemo', @level2type=N'COLUMN',@level2name=N'ReceiptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金メモ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptMemo', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振替元入金データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'SourceReceiptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振替先入金データID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'DestinationReceiptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振替元入金部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'SourceSectionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振替先入金部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'DestinationSectionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振替元金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'SourceAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振替先金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'DestinationAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'印刷フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'PrintFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReceiptSectionTransfer', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSetting', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帳票ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSetting', @level2type=N'COLUMN',@level2name=N'ReportId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSetting', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'項目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSetting', @level2type=N'COLUMN',@level2name=N'ItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'設定値(既定値)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSetting', @level2type=N'COLUMN',@level2name=N'ItemKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帳票ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSettingBase', @level2type=N'COLUMN',@level2name=N'ReportId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表示順' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSettingBase', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'項目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSettingBase', @level2type=N'COLUMN',@level2name=N'ItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'設定値(既定値)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSettingBase', @level2type=N'COLUMN',@level2name=N'ItemKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'項目名説明の代替テキスト' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSettingBase', @level2type=N'COLUMN',@level2name=N'Alias'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文字列判定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportSettingBase', @level2type=N'COLUMN',@level2name=N'IsText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Section', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Section', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Section', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Section', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備考' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Section', @level2type=N'COLUMN',@level2name=N'Note'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Section', @level2type=N'COLUMN',@level2name=N'PayerCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Section', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Section', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Section', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Section', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithDepartment', @level2type=N'COLUMN',@level2name=N'SectionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithDepartment', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithDepartment', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithDepartment', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithDepartment', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithDepartment', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ログインユーザーID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithLoginUser', @level2type=N'COLUMN',@level2name=N'LoginUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithLoginUser', @level2type=N'COLUMN',@level2name=N'SectionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithLoginUser', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithLoginUser', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithLoginUser', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SectionWithLoginUser', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'項目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'ItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'キー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'ItemKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'値' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Setting', @level2type=N'COLUMN',@level2name=N'ItemValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'項目ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SettingDefinition', @level2type=N'COLUMN',@level2name=N'ItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'説明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SettingDefinition', @level2type=N'COLUMN',@level2name=N'Description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'キー文字数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SettingDefinition', @level2type=N'COLUMN',@level2name=N'ItemKeyLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'値文字数)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SettingDefinition', @level2type=N'COLUMN',@level2name=N'ItemValueLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'営業担当者名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'メールアドレス' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'Mail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Staff', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'タスクスケジュールID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'インポート種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'ImportType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'インポート種別詳細パターン(マスター登録方法/EBファイルフォーマット/フリーインポーターパターンID)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'ImportSubType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'トリガー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'Duration'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'開始日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'StartDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'間隔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'Interval'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'曜日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'WeekDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込フォルダパス' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'ImportDirectory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成功時移動フォルダパス' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'SuccessDirectory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失敗時移動フォルダパス' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'FailedDirectory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'エラーログ出力場所' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'LogDestination'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'処理対象請求データ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'TargetBillingAssignment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'処理方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'BillingAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'同一得意先請求データ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'UpdateSameCustomer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'UpdateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskSchedule', @level2type=N'COLUMN',@level2name=N'UpdateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'タイムスケジューラー実行履歴ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskScheduleHistory', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskScheduleHistory', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'インポート種別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskScheduleHistory', @level2type=N'COLUMN',@level2name=N'ImportType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取込パターン' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskScheduleHistory', @level2type=N'COLUMN',@level2name=N'ImportSubType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'開始日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskScheduleHistory', @level2type=N'COLUMN',@level2name=N'StartAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'終了日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskScheduleHistory', @level2type=N'COLUMN',@level2name=N'EndAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'実行結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskScheduleHistory', @level2type=N'COLUMN',@level2name=N'Result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'エラーログ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaskScheduleHistory', @level2type=N'COLUMN',@level2name=N'Errors'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tax', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'適用日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tax', @level2type=N'COLUMN',@level2name=N'ApplyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'税率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tax', @level2type=N'COLUMN',@level2name=N'Rate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tax', @level2type=N'COLUMN',@level2name=N'CreateBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登録日時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tax', @level2type=N'COLUMN',@level2name=N'CreateAt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消費税属性コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxClass', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消費税属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TaxClass', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBankTransfer', @level2type=N'COLUMN',@level2name=N'ClientKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBankTransfer', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBankTransfer', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBankTransfer', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社カナ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBankTransfer', @level2type=N'COLUMN',@level2name=N'PaymentAgencyKana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBankTransfer', @level2type=N'COLUMN',@level2name=N'BillingAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBankTransfer', @level2type=N'COLUMN',@level2name=N'BillingRemainAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBankTransfer', @level2type=N'COLUMN',@level2name=N'BillingCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBilling', @level2type=N'COLUMN',@level2name=N'ClientKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBilling', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBilling', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBilling', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先/決済代行会社カナ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBilling', @level2type=N'COLUMN',@level2name=N'CustomerKana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBilling', @level2type=N'COLUMN',@level2name=N'BillingAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBilling', @level2type=N'COLUMN',@level2name=N'BillingRemainAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBilling', @level2type=N'COLUMN',@level2name=N'BillingCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'ClientKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'BillingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先（債権代表者）ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'BillingAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkBillingTarget', @level2type=N'COLUMN',@level2name=N'RemainAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ClientKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'代表得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'決済代行会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'PaymentAgencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'PayerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'PayerCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向銀行名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'SourceBankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向支店名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CollationKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合タイプ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'CollationType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'債権代表者名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'債権代表者カナ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerKana'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'債権代表者手数料負担区分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ParentCustomerShareTransferFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BillingAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BillingRemainAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'BillingCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'引当額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptAssignmentAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptRemainAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ReceiptCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前受件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'AdvanceReceivedCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込対象外件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkCollation', @level2type=N'COLUMN',@level2name=N'ForceMatchingIndividually'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkDepartmentTarget', @level2type=N'COLUMN',@level2name=N'ClientKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkDepartmentTarget', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'請求部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkDepartmentTarget', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'絞込フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkDepartmentTarget', @level2type=N'COLUMN',@level2name=N'UseCollation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkNettingTarget', @level2type=N'COLUMN',@level2name=N'ClientKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相殺ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkNettingTarget', @level2type=N'COLUMN',@level2name=N'NettingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合タイプ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkNettingTarget', @level2type=N'COLUMN',@level2name=N'CollationType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ClientKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'PayerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'BankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'BranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'PayerCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向銀行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'SourceBankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向支店' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'CollationKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'引当額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptAssignmentAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金残' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptRemainAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ReceiptCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一括消込対象外件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'ForceMatchingIndividually'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'前受件数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceipt', @level2type=N'COLUMN',@level2name=N'AdvanceReceivedCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'ClientKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'ReceiptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通貨ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'CurrencyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'PayerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銀行コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'BankCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支店コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'BranchCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'振込依頼人コード' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'PayerCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向銀行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'SourceBankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'仕向支店' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'SourceBranchName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合番号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'CollationKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'得意先ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'照合タイプ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkReceiptTarget', @level2type=N'COLUMN',@level2name=N'CollationType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'クライアントキー' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkSectionTarget', @level2type=N'COLUMN',@level2name=N'ClientKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会社ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkSectionTarget', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入金部門ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkSectionTarget', @level2type=N'COLUMN',@level2name=N'SectionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'絞込フラグ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkSectionTarget', @level2type=N'COLUMN',@level2name=N'UseCollation'
GO
