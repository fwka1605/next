﻿
IF EXISTS (SELECT * FROM sys.objects
            WHERE object_id = object_id(N'uspCollationCustomerId')
              AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[uspCollationCustomerId];
END;
GO

SET ANSI_NULLS ON
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
         , ForceMatchingIndividually
         , MinReceiptRecordedAt
         , MaxReceiptRecordedAt
         , MinReceiptId
         , MaxReceiptId )
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
         , wr.MinRecordedAt
         , wr.MaxRecordedAt
         , wr.MinReceiptId
         , wr.MaxReceiptId
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[Customer] csr
        ON wr.ClientKey         = @ClientKey
       AND csr.Id               = wr.CustomerId
       AND csr.IsParent         = 0
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
        ON csb.Id               = wb.CustomerId
      LEFT JOIN [dbo].[WorkCollation] wc
        ON wc.ClientKey         = wr.ClientKey
       AND wc.CompanyId         = wr.CompanyId
       AND wc.CurrencyId        = wr.CurrencyId
       AND wc.PayerName         = wr.PayerName
       AND wc.PayerCode         = wr.PayerCode
       AND wc.BankCode          = wr.BankCode
       AND wc.BranchCode        = wr.BranchCode
       AND wc.SourceBankName    = wr.SourceBankName
       AND wc.SourceBranchName  = wr.SourceBranchName
       AND wc.CollationKey      = wr.CollationKey
       AND wc.CustomerId        = wr.CustomerId
     WHERE wc.ClientKey         IS NULL;

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
         , ForceMatchingIndividually
         , MinReceiptRecordedAt
         , MaxReceiptRecordedAt
         , MinReceiptId
         , MaxReceiptId )
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
         , wr.MinRecordedAt
         , wr.MaxRecordedAt
         , wr.MinReceiptId
         , wr.MaxReceiptId
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[WorkBilling] wb
        ON wb.ClientKey         = @ClientKey
       AND wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND wb.CustomerId        = wr.CustomerId
     INNER JOIN [dbo].[Customer] csb
        ON csb.Id               = wb.CustomerId
      LEFT JOIN [dbo].[WorkCollation] wc
        ON wc.ClientKey         = wr.ClientKey
       AND wc.CompanyId         = wr.CompanyId
       AND wc.CurrencyId        = wr.CurrencyId
       AND wc.PayerName         = wr.PayerName
       AND wc.PayerCode         = wr.PayerCode
       AND wc.BankCode          = wr.BankCode
       AND wc.BranchCode        = wr.BranchCode
       AND wc.SourceBankName    = wr.SourceBankName
       AND wc.SourceBranchName  = wr.SourceBranchName
       AND wc.CollationKey      = wr.CollationKey
       AND wc.CustomerId        = wr.CustomerId 
     WHERE wc.ClientKey         IS NULL;

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
