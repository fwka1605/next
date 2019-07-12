using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class PeriodicBillingSettingQueryProcessor :
        IPeriodicBillingSettingQueryProcessor,
        IAddPeriodicBillingSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public PeriodicBillingSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<PeriodicBillingSetting> SaveAsync(PeriodicBillingSetting setting, CancellationToken token = default(CancellationToken))
        {
            #region query
            var query = @"
MERGE INTO [PeriodicBillingSetting] target
USING (
    SELECT @Id  [Id]
      ) source
ON    (target.Id   = source.Id )
WHEN MATCHED THEN
    UPDATE SET
           [Name]               = @Name
         , CurrencyId           = @CurrencyId
         , CustomerId           = @CustomerId
         , DestinationId        = @DestinationId
         , DepartmentId         = @DepartmentId
         , StaffId              = @StaffId
         , CollectCategoryId    = @CollectCategoryId
         , BilledCycle          = @BilledCycle
         , BilledDay            = @BilledDay
         , StartMonth           = @StartMonth
         , EndMonth             = @EndMonth
         , InvoiceCode          = @InvoiceCode
         , SetBillingNote1      = @SetBillingNote1
         , SetBillingNote2      = @SetBillingNote2
         , UpdateBy             = @UpdateBy
         , UpdateAt             = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (  CompanyId,  [Name]
           ,  CurrencyId,  CustomerId,  DestinationId,  DepartmentId,  StaffId,  CollectCategoryId
           ,  BilledCycle,  BilledDay,  StartMonth,  EndMonth
           ,  InvoiceCode,  SetBillingNote1,  SetBillingNote2
           ,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt )
    VALUES ( @CompanyId, @Name
           , @CurrencyId,  @CustomerId, @DestinationId, @DepartmentId, @StaffId, @CollectCategoryId
           , @BilledCycle, @BilledDay, @StartMonth, @EndMonth
           , @InvoiceCode, @SetBillingNote1, @SetBillingNote2
           , @CreateBy, GETDATE(), @UpdateBy, GETDATE() )
OUTPUT inserted.*
;";
            #endregion
            return dbHelper.ExecuteAsync<PeriodicBillingSetting>(query, setting, token);
        }

        public Task<IEnumerable<PeriodicBillingSetting>> GetAsync(PeriodicBillingSettingSearch option, CancellationToken token = default(CancellationToken))
        {
            #region query
            var query = new StringBuilder(@"
SELECT s.*
     , pc.[UpdateAt]    [LastUpdateAt]
     , lu.[Name]        [LastUpdatedBy]
     , cc.[Name]        [CollectCategoryName]
     , dp.[Code]        [DepartmentCode]
     , dp.[Name]        [DepartmentName]
     , st.[Code]        [StaffCode]
     , st.[Name]        [StaffName]
     , ds.[Code]        [DestinationCode]
     , cs.[Code]        [CustomerCode]
     , cs.[Name]        [CustomerName]
     , CASE ds.[Addressee] WHEN N'' THEN ds.[DepartmentName] ELSE ds.[Addressee]        END
     + CASE ds.[Honorific] WHEN N'' THEN N''                 ELSE N' ' + ds.[Honorific] END
                        [DestinationName]
     , ds.[Addressee]
     , pbc.[LastCreateYearMonth]
  FROM PeriodicBillingSetting s
 INNER JOIN Customer cs                     ON cs.Id    = s.CustomerId
  LEFT JOIN Destination ds                  ON ds.Id    = s.DestinationId
  LEFT JOIN Category cc                     ON cc.Id    = s.CollectCategoryId
  LEFT JOIN Staff st                        ON st.Id    = s.StaffId
  LEFT JOIN Department dp                   ON dp.Id    = s.DepartmentId
  LEFT JOIN PeriodicBillingCreated pc       ON  s.Id    = pc.PeriodicBillingSettingId AND pc.CreateYearMonth = @BaseDate
  LEFT JOIN LoginUser lu                    ON lu.Id    = pc.UpdateBy
  LEFT JOIN (
       SELECT pbc.PeriodicBillingSettingId
            , MAX(CreateYearMonth) [LastCreateYearMonth]
         FROM PeriodicBillingCreated pbc
        GROUP BY
              pbc.PeriodicBillingSettingId
       ) pbc                                ON  s.Id    = pbc.PeriodicBillingSettingId
 WHERE s.CompanyId              = @CompanyId
");

            if (option.Ids?.Any() ?? false) query.Append(@"
   AND s.Id                     IN (SELECT Id FROM @Ids)");
            if (!string.IsNullOrEmpty(option.Name))
            {
                option.Name = Sql.GetWrappedValue(option.Name);
                query.Append(@"
   AND (s.[Name]                LIKE @Name
    OR  cs.[Name]               LIKE @Name
    OR  ds.[Addressee]          LIKE @Name
       )");
            }
            if (option.BaseDate.HasValue)
            {
                query.Append(@"
   AND s.StartMonth             <= @BaseDate
   AND @BaseDate                <= COALESCE( s.EndMonth, CAST('9999-12-01' AS DATE) )
   AND (s.BilledCycle           = 1
     OR s.StartMonth            = @BaseDate
     OR DateAdd(Month, s.BilledCycle * (DATEDIFF(Month, s.StartMonth, @BaseDate) / s.BilledCycle), s.StartMonth ) = @BaseDate )");

                if (option.ReCreate) query.Append(@"
   AND EXISTS (
       SELECT 1
         FROM PeriodicBillingCreated pbc
        WHERE pbc.PeriodicBillingSettingId  = s.Id
          AND pbc.CreateYearMonth           = @BaseDate )");
                else query.Append(@"
   AND NOT EXISTS (
       SELECT 1
         FROM PeriodicBillingCreated pbc
        WHERE pbc.PeriodicBillingSettingId  = s.Id
          AND pbc.CreateYearMonth           = @BaseDate )");
            }

            query.Append(@"
 ORDER BY cs.[Code], s.[StartMonth]");
            #endregion
            return dbHelper.GetItemsAsync<PeriodicBillingSetting>(query.ToString(), new {
                        option.CompanyId,
                        option.Name,
                        option.BaseDate,
                Ids =   option.Ids?.GetTableParameter(),
            }, token);
        }

    }
}
