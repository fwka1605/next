
IF EXISTS (SELECT * FROM sys.objects
            WHERE object_id = object_id(N'uspCollationInitialize')
              AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[uspCollationInitialize];
END;
GO

SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[uspCollationInitialize]
 @ClientKey         VARBINARY(20)  /* クライアントキー */
,@CompanyId         INT            /* 会社ID           */
,@CurrencyId        INT            /* 通貨ID           */
,@RecordedAtFrom    DATE           /* 入金日From       */
,@RecordedAtTo      DATE           /* 入金日To         */
,@DueAtFrom         DATE           /* 入金予定日From   */
,@DueAtTo           DATE           /* 入金予定日To     */
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
       AND b.DueAt                  >= COALESCE(@DueAtFrom, b.DueAt)
       AND b.DueAt                  <= @DueAtTo
       AND b.Approved                = 1
       AND b.RemainAmount           <> b.OffsetAmount
       AND b.DeleteAt               IS NULL
       AND (
                @BillingType = 0
            OR (@BillingType = 1 AND b.InputType <> 3 AND b.InputType <> 5)
            OR (@BillingType = 2 AND b.InputType  = 3)
            OR (@BillingType = 3 AND b.InputType  = 5)
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
       AND b.DueAt                  >= COALESCE(@DueAtFrom, b.DueAt)
       AND b.DueAt                  <= @DueAtTo
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
         , r.[BankCode]
         , r.[BranchCode]
         , r.[PayerCode]
         , r.[SourceBankName]
         , r.[SourceBranchName]
         , r.[CollationKey]
         , COALESCE(r.[CustomerId], 0) [CustomerId]
         , 0            [CollationType]
      FROM [dbo].[Receipt] r
      LEFT JOIN [dbo].[ReceiptHeader] rh    ON rh.[Id]  = r.[ReceiptHeaderId]
     WHERE r.CompanyId                              = @CompanyId
       AND r.CurrencyId                             = COALESCE(@CurrencyId, r.CurrencyId)
       AND COALESCE(r.ProcessingAt, r.RecordedAt)  >= COALESCE(@RecordedAtFrom, r.RecordedAt)
       AND COALESCE(r.ProcessingAt, r.RecordedAt)  <= @RecordedAtTo
       AND r.Approved                               = 1
       AND r.Apportioned                            = 1
       AND r.RemainAmount                          <> 0
       AND r.DeleteAt                              IS NULL
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
       AND n.RecordedAt        >= COALESCE(@RecordedAtFrom, n.RecordedAt)
       AND n.RecordedAt        <= @RecordedAtTo
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
         , ForceMatchingIndividually
         , MinRecordedAt
         , MaxRecordedAt
         , MinReceiptId
         , MaxReceiptId )

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
         , MIN(u.RecordedAt)                    [MinRecordedAt]
         , MAX(u.RecordedAt)                    [MaxRecordedAt]
         , MIN(u.ReceiptId)                     [MinReceiptId]
         , MAX(u.ReceiptId)                     [MaxReceiptId]

      FROM (

            /* 通常入金 */
            SELECT r.CompanyId
                 , r.CurrencyId
                 , r.PayerName
                 , r.BankCode
                 , r.BranchCode
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
                 , r.RecordedAt
                 , r.Id                         [ReceiptId]
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
                 , n.RecordedAt
                 , 0                    [ReceiptId]
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
