
IF EXISTS (SELECT * FROM sys.objects
            WHERE object_id = object_id(N'uspCollationPayerCode')
              AND type IN (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[uspCollationPayerCode];
END;
GO

SET ANSI_NULLS ON
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
         , wr.MinRecordedAt
         , wr.MaxRecordedAt
         , wr.MinReceiptId
         , wr.MaxReceiptId
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
            INNER JOIN [dbo].[CustomerGroup] csg
               ON csg.ChildCustomerId       = cs.Id
              AND csg.ParentCustomerId     <> csg.ChildCustomerId
             LEFT JOIN [dbo].[WorkCollation] wc
               ON wc.ClientKey              = wr.ClientKey
              AND wc.CompanyId              = wr.CompanyId
              AND wc.CurrencyId             = wr.CurrencyId
              AND wc.PayerName              = wr.PayerName
              AND wc.PayerCode              = wr.PayerCode
              AND wc.BankCode               = wr.BankCode
              AND wc.BranchCode             = wr.BranchCode
              AND wc.SourceBankName         = wr.SourceBankName
              AND wc.SourceBranchName       = wr.SourceBranchName
              AND wc.CollationKey           = wr.CollationKey
              AND wc.CustomerId             = wr.CustomerId
            WHERE wc.ClientKey              IS NULL
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
         , ForceMatchingIndividually
         , MinReceiptRecordedAt
         , MaxReceiptRecordedAt
         , MinReceiptId
         , MaxReceiptId )
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
         , wr.MinRecordedAt
         , wr.MaxRecordedAt
         , wr.MinReceiptId
         , wr.MaxReceiptId
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

END;
GO
