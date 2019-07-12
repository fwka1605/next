using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class IgnoreKanaQueryProcessor :
        IIgnoreKanasByCompanyIdQueryProcessor,
        IIgnoreKanaByCodeQueryProcessor,
        IAddIgnoreKanaQueryProcessor,
        IDeleteIgnoreKanaQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public IgnoreKanaQueryProcessor(IDbHelper helper)
        {
            dbHelper = helper;
        }
        public Task<IEnumerable<IgnoreKana>> GetAsync(IgnoreKana kana, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      ik.*
          , ec.Code     ExcludeCategoryCode
          , ec.Name     ExcludeCategoryName
FROM        IgnoreKana ik
LEFT JOIN   Category ec     ON ec.Id    = ik.ExcludeCategoryId
WHERE       ik.CompanyId        = @CompanyId";
            if (!string.IsNullOrWhiteSpace(kana.Kana)) query += @"
AND         ik.Kana             = @Kana";

            query += @"
ORDER BY    ik.CompanyId        ASC
          , ik.Kana             ASC
";

            return dbHelper.GetItemsAsync<IgnoreKana>(query, kana, token);
        }


        public Task<IgnoreKana> SaveAsync(IgnoreKana kana, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO IgnoreKana AS Org 
USING ( 
       SELECT 
          @CompanyId AS CompanyId 
         ,@Kana AS Kana 
         ,@ExcludeCategoryId AS ExcludeCategoryId 
         ) AS Target 
         ON ( 
           Org.CompanyId = @CompanyId 
           AND Org.Kana = @Kana COLLATE JAPANESE_BIN
       ) 
       WHEN MATCHED THEN 
           UPDATE SET 
               ExcludeCategoryId = @ExcludeCategoryId 
           ,UpdateBy = @UpdateBy 
           ,UpdateAt = GETDATE() 
       WHEN NOT MATCHED THEN 
           INSERT (CompanyId, Kana, ExcludeCategoryId, CreateBy, CreateAt, UpdateBy, UpdateAt) 
           VALUES (@CompanyId, @Kana, @ExcludeCategoryId, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
       OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<IgnoreKana>(query, kana, token);
        }

        public Task<int> DeleteAsync(int CompanyId, string Kana, CancellationToken token = default(CancellationToken))
        {
            var query = @"
DELETE          IgnoreKana
WHERE           CompanyId   = @CompanyId
AND             Kana        = @Kana; ";
            return dbHelper.ExecuteAsync(query, new { CompanyId, Kana }, token);
        }

        public async Task<bool> ExistCategoryAsync(int ExcludeCategoryId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      TOP(1) 1
FROM        IgnoreKana 
WHERE       ExcludeCategoryId = @ExcludeCategoryId";
            return (await dbHelper.ExecuteAsync<int?>(query, new { ExcludeCategoryId })).HasValue;
        }

    }
}
