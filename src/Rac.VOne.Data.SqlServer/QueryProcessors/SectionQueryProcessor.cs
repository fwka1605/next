using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class SectionQueryProcessor :
        ISectionQueryProcessor,
        IAddSectionQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public SectionQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<Section>> GetAsync(SectionSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      sc.*
FROM        Section sc
WHERE       sc.Id               = sc.Id";
            if (option.CompanyId.HasValue) query += @"
AND         sc.CompanyId        = @CompanyId";
            if (option.Ids?.Any() ?? false) query += @"
AND         sc.Id               IN (SELECT Id   FROM @Ids)";
            if (option.Codes?.Any() ?? false) query += @"
AND         sc.Code             IN (SELECT Code FROM @Codes)";
            if (option.PayerCodes?.Any() ?? false) query += @"
AND         sc.PayerCode        IN (SELECT Code FROM @PayerCodes)";
            if (option.LoginUserId.HasValue) query += @"
AND         sc.Id               IN (
            SELECT      DISTINCT swl.SectionId
            FROM        SectionWithLoginUser swl
            WHERE       swl.LoginUserId = @LoginUserId
            )";
            if (option.CustomerId.HasValue) query += @"
AND         sc.Id               IN (
            SELECT      DISTINCT swd.SectionId
            FROM        Customer cs
            INNER JOIN  Staff st                    ON  st.Id           = cs.StaffId
            INNER JOIN  SectionWithDepartment swd   ON swd.DepartmentId = st.DepartmentId
            WHERE       cs.Id           = @CustomerId
            )";
            query += @"
ORDER BY      sc.CompanyId      ASC
            , sc.Code           ASC";
            return dbHelper.GetItemsAsync<Section>(query, new {
                                option.CompanyId,
                                option.LoginUserId,
                                option.CustomerId,
                Ids         =   option.Ids.GetTableParameter(),
                Codes       =   option.Codes.GetTableParameter(),
                PayerCodes  =   option.PayerCodes.GetTableParameter(),
            } , token);
        }


        public Task<Section> SaveAsync(Section Section, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO Section AS target
USING (
    SELECT
            @Id           As Id
) AS source
ON (
        target.Id   = source.Id
  )
WHEN MATCHED THEN
    UPDATE SET
            Name = @Name,
            Note = @Note,
            PayerCode = @PayerCodeLeft+@PayerCodeRight,
            UpdateBy = @UpdateBy,
            UpdateAt = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (CompanyId,Code,Name,Note,PayerCode,CreateBy,CreateAt,UpdateBy,UpdateAt)
    VALUES(@CompanyId,@Code,@Name,@Note,@PayerCodeLeft + @PayerCodeRight,@CreateBy,GETDATE(),@UpdateBy,GETDATE())
    OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<Section>(query, Section, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsBankAccountAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM Section sc
 WHERE sc.CompanyId = @CompanyId
   AND EXISTS (
       SELECT *
         FROM BankAccount ba
         WHERE ba.SectionId = sc.Id )
  AND sc.Code NOT IN (SELECT Code From @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsSectionWithDepartmentAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM Section sc
 WHERE sc.CompanyId = @CompanyId
   AND EXISTS (
       SELECT *
         FROM SectionWithDepartment sd
        WHERE sd.SectionId = sc.Id )
   AND sc.Code NOT IN (SELECT Code From @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsSectionWithLoginUserAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM Section sc
 WHERE sc.CompanyId = @CompanyId
   AND EXISTS (
       SELECT *
         FROM SectionWithLoginUser sl
        WHERE sl.SectionId = sc.Id )
   AND sc.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsReceiptAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM Section sc
 WHERE sc.CompanyId = @CompanyId
   AND EXISTS (
        SELECT *
          FROM Receipt r
         WHERE r.SectionId = sc.Id )
   AND sc.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsNettingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM Section sc
 WHERE sc.CompanyId = @CompanyId
   AND EXISTS (
        SELECT *
          FROM Netting nt
         WHERE nt.SectionId = sc.Id )
   AND sc.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }
    }
}
