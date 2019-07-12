
IF EXISTS (SELECT * FROM sys.objects
            WHERE object_id = object_id(N'uspCollation')
              AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[uspCollation];
END;
GO

SET ANSI_NULLS ON
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
