
IF EXISTS (SELECT * FROM sys.objects
            WHERE object_id = object_id(N'uspCollationMissinge')
              AND type IN (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[uspCollationMissinge];
END;
GO

IF EXISTS (SELECT * FROM sys.objects
            WHERE object_id = object_id(N'uspCollationMissing')
              AND type IN (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[uspCollationMissing];
END;
GO

SET ANSI_NULLS ON
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
      LEFT JOIN [dbo].[WorkCollation] wc
        ON wc.ClientKey         = wb.ClientKey
       AND wc.CompanyId         = wb.CompanyId
       AND wc.CurrencyId        = wb.CurrencyId
       AND wc.ParentCustomerId  = wb.CustomerId
       AND wc.PaymentAgencyId   = 0
     WHERE wc.ClientKey         IS NULL
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
      LEFT JOIN [dbo].[WorkCollation] wc
        ON wc.ClientKey         = wb.ClientKey
       AND wc.CompanyId         = wb.CompanyId
       AND wc.CurrencyId        = wb.CurrencyId
       AND wc.ParentCustomerId  = 0
       AND wc.PaymentAgencyId   = wb.PaymentAgencyId
     WHERE wc.ClientKey         IS NULL

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
         , ForceMatchingIndividually
         , MinReceiptRecordedAt
         , MaxReceiptRecordedAt
         , MinReceiptId
         , MaxReceiptId )
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
         , wr.MinRecordedAt
         , wr.MaxRecordedAt
         , wr.MinReceiptId
         , wr.MaxReceiptId
      FROM [dbo].[WorkReceipt] wr
      LEFT JOIN [dbo].[WorkCollation] wc
        ON wc.ClientKey         = wr.ClientKey
       AND wc.CurrencyId        = wr.CurrencyId
       AND wc.PayerName         = wr.PayerName
       AND wc.PayerCode         = wr.PayerCode
       AND wc.BankCode          = wr.BankCode
       AND wc.BranchCode        = wr.BranchCode
       AND wc.SourceBankName    = wr.SourceBankName
       AND wc.SourceBranchName  = wr.SourceBranchName
       AND wc.CollationKey      = wr.CollationKey
       AND wc.CustomerId        = wr.CustomerId
     WHERE wr.ClientKey         = @ClientKey
       AND wc.ClientKey         IS NULL;

END;
GO
