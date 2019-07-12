
IF EXISTS (SELECT * FROM sys.objects
            WHERE object_id = object_id(N'uspCollationHistory')
              AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[uspCollationHistory];
END;
GO

SET ANSI_NULLS ON
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
         , wr.MinRecordedAt
         , wr.MaxRecordedAt
         , wr.MinReceiptId
         , wr.MaxReceiptId
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[Customer] cs
        ON wr.ClientKey         = @ClientKey
       AND wr.SourceBankName    > N''
       AND wr.SourceBranchName  > N''
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
         , ForceMatchingIndividually
         , MinReceiptRecordedAt
         , MaxReceiptRecordedAt
         , MinReceiptId
         , MaxReceiptId )
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
         , wr.MinRecordedAt
         , wr.MaxRecordedAt
         , wr.MinReceiptId
         , wr.MaxReceiptId
      FROM [dbo].[WorkReceipt] wr
     INNER JOIN [dbo].[PaymentAgency] pa
        ON wr.ClientKey             = @ClientKey
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
         , ForceMatchingIndividually
         , MinReceiptRecordedAt
         , MaxReceiptRecordedAt
         , MinReceiptId
         , MaxReceiptId )
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
         , wr.MinRecordedAt
         , wr.MaxRecordedAt
         , wr.MinReceiptId
         , wr.MaxReceiptId
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
     INNER JOIN [dbo].[WorkBilling] wb
        ON wb.ClientKey         = @ClientKey
       AND wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND cs.Id                = wb.CustomerId
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
         , ForceMatchingIndividually
         , MinReceiptRecordedAt
         , MaxReceiptRecordedAt
         , MinReceiptId
         , MaxReceiptId )
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
         , wr.MinRecordedAt
         , wr.MaxRecordedAt
         , wr.MinReceiptId
         , wr.MaxReceiptId
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
     INNER JOIN [dbo].[WorkBankTransfer] wb
        ON wb.ClientKey         = wr.ClientKey
       AND wb.CompanyId         = wr.CompanyId
       AND wb.CurrencyId        = wr.CurrencyId
       AND pa.Id                = wb.PaymentAgencyId
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
