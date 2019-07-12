using System;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class SectionWithDepartmentQueryProcessor :
        ISectionWithDepartmentQueryProcessor,
        IAddSectionWithDepartmentQueryProcessor,
        IDeleteSectionWithDepartmentQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public SectionWithDepartmentQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<SectionWithDepartment>> GetAsync(SectionWithDepartmentSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT        swd.*
            , sc.Code As SectionCode
            , sc.Name As SectionName
            , dp.Code As DepartmentCode
            , dp.Name As DepartmentName
FROM        SectionWithDepartment swd
INNER JOIN  Department dp               ON dp.Id            = swd.DepartmentId
INNER JOIN  Section sc                  ON sc.Id            = swd.SectionId
WHERE       swd.SectionId       = swd.SectionId";
            if (option.SectionId.HasValue) query += @"
AND         swd.SectionId       = @SectionId";
            if (option.CompanyId.HasValue) query += @"
AND         dp.CompanyId        = @CompanyId";
            if (option.DepartmentId.HasValue) query += @"
AND         swd.DepartmentId    = @DepartmentId";
            query += @"
ORDER BY      sc.Code           ASC
            , dp.Code           ASC";

            return dbHelper.GetItemsAsync<SectionWithDepartment>(query, option, token);
        }

        public async Task<bool> ExistDepartmentAsync(int DepartmentId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      1
WHERE EXISTS(
            SELECT      1
            FROM        SectionWithDepartment
            WHERE       DepartmentId    = @DepartmentId
            )";
            return (await dbHelper.ExecuteAsync<int?>(query, new { DepartmentId }, token)).HasValue;
        }

        public async Task<bool> ExistSectionAsync(int SectionId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT TOP 1 1 FROM SectionWithDepartment 
WHERE SectionId = @SectionId";
            return (await dbHelper.ExecuteAsync<int?>(query, new { SectionId }, token)).HasValue;
        }

        public Task<SectionWithDepartment> SaveAsync(SectionWithDepartment SectionWithDepartment, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO SectionWithDepartment AS target 
USING (SELECT
              @SectionId    As SectionId
            , @DepartmentId As DepartmentId
   ) As source
ON ( 
            target.SectionId    = source.SectionId
        AND target.DepartmentId = source.DepartmentId
) 
WHEN MATCHED THEN 
    UPDATE SET 
     UpdateBy = @UpdateBy
    ,UpdateAt = GETDATE()
WHEN NOT MATCHED THEN 
 INSERT (SectionId, DepartmentId, CreateBy, CreateAt, UpdateBy, UpdateAt)
 VALUES (SectionId, DepartmentId, @UpdateBy, GETDATE(),@UpdateBy, GETDATE())
 OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<SectionWithDepartment>(query, SectionWithDepartment, token);
        }

        public Task<int> DeleteAsync(int SectionId, int DepartmentId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE SectionWithDepartment
OUTPUT deleted.*
 WHERE SectionId = @SectionId
   AND DepartmentId = @DepartmentId";
            return dbHelper.ExecuteAsync(query, new { SectionId, DepartmentId }, token);
        }

    }
}
