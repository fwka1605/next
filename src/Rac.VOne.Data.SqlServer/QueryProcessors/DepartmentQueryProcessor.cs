using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class DepartmentQueryProcessor :
        IAddDepartmentQueryProcessor,
        IDepartmentByCodeQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public DepartmentQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }


        public Task<IEnumerable<Department>> GetAsync(DepartmentSearch option, CancellationToken token = default(CancellationToken))
        {
            if (!option.CompanyId.HasValue
                && (option.WithSectionId.HasValue
                 || option.LoginUserId.HasValue
                 || (option.Codes?.Any() ?? false)))
                throw new ArgumentException($"{nameof(option.CompanyId)} is required.");

            var query = @"
SELECT      dp.*
          , st.Code     StaffCode
          , st.Name     StaffName
FROM        Department dp
LEFT JOIN   Staff st            ON st.Id        = dp.StaffId
WHERE       dp.Id               = dp.Id";
            if (option.CompanyId.HasValue) query += @"
AND         dp.CompanyId        = @CompanyId";
            if (option.Ids?.Any() ?? false) query += @"
AND         dp.Id               IN (SELECT Id   FROM @Ids)";
            if (option.SkipIds?.Any() ?? false) query += @"
AND         dp.Id           NOT IN (SELECT Id   FROM @SkipIds)";
            if (option.Codes?.Any() ?? false) query += @"
AND         dp.Code             IN (SELECT Code FROM @Codes)";
            if (option.WithSectionId.HasValue) query += @"
AND         dp.Id           NOT IN (
            SELECT      DISTINCT swd.DepartmentId
            FROM        SectionWithDepartment swd
            WHERE       swd.SectionId <> @WithSectionId
            )";
            if (option.LoginUserId.HasValue) query += @"
AND         dp.Id               IN (
            SELECT      DISTINCT swd.DepartmentId
            FROM        SectionWithDepartment swd
            INNER JOIN  SectionWithLoginUser swl    ON swd.SectionId    = swl.SectionId
                                                   AND swl.LoginUserId  = @LoginUserId
            )";
            query += @"
ORDER BY    dp.CompanyId        ASC
          , dp.Code             ASC";
            return dbHelper.GetItemsAsync<Department>(query, new {
                            option.CompanyId,
                            option.LoginUserId,
                            option.WithSectionId,
                Ids     =   option.Ids.GetTableParameter(),
                SkipIds =   option.SkipIds.GetTableParameter(),
                Codes   =   option.Codes.GetTableParameter(),
            }, token);
        }


        public Task<Department> SaveAsync(Department Department, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO Department AS Org 
USING ( 
    SELECT 
        @CompanyId AS CompanyId 
        ,@Code AS Code 
        ,@StaffId AS StaffId 
) AS Target 
ON ( 
    Org.CompanyId = @CompanyId 
    AND Org.Code = @Code COLLATE JAPANESE_BIN
) 
WHEN MATCHED THEN 
    UPDATE SET 
         Code = @Code
        ,Name = @Name
        ,StaffId = @StaffId 
        ,Note = @Note
        ,UpdateBy = @UpdateBy
        ,UpdateAt = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, Code, Name, StaffId, Note, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @Code, @Name, @StaffId, @Note, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*; ";
            #endregion
            return dbHelper.ExecuteAsync<Department>(query, Department, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsStaffAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM Department d
WHERE d.CompanyId = @CompanyId
  AND EXISTS (
      SELECT *
        FROM Staff s
       WHERE s.CompanyId = @CompanyId
         AND s.DepartmentId = d.Id )
  AND d.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsSectionWithDepartmentAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM Department d
WHERE d.CompanyId = @CompanyId
  AND EXISTS (
      SELECT *
        FROM SectionWithDepartment swd
       WHERE swd.DepartmentId = d.Id )
  AND d.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }

        public Task<IEnumerable<MasterData>> GetImportItemsBillingAsync(int CompanyId, IEnumerable<string> Code, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT * FROM Department d
WHERE d.CompanyId = @CompanyId
  AND EXISTS (
      SELECT *
        FROM Billing b
       WHERE b.CompanyId = @CompanyId
         AND b.DepartmentId = d.Id )
  AND d.Code NOT IN (SELECT Code FROM @Code) ";
            return dbHelper.GetItemsAsync<MasterData>(query, new { CompanyId, Code = Code.GetTableParameter() }, token);
        }


    }
}
