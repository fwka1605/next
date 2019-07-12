using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class EBFileSettingQueryProcessor :
        IAddEBFileSettingQueryProcessor,
        IEBFileSettingQueryProcessor,
        IUpdateEBFileSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public EBFileSettingQueryProcessor (IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<EBFileSetting>> GetAsync(EBFileSettingSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      s.*
          , f.RequireYear
FROM        EBFileSetting s
INNER JOIN  EBFormat f          ON f.Id             = s.EBFormatId
WHERE       s.Id                = s.Id";
            if (option.CompanyId.HasValue) query += @"
AND         s.CompanyId         = @CompanyId";
            if (option.Ids?.Any() ?? false) query += @"
AND         s.Id                IN (SELECT Id   FROM @Ids)";
            query += @"
 ORDER BY s.DisplayOrder    ASC";

            return dbHelper.GetItemsAsync<EBFileSetting>(query, new {
                            option.CompanyId,
                Ids     =   option.Ids.GetTableParameter(),
            }, token);
        }

        public Task<EBFileSetting> SaveAsync(EBFileSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO EBFileSetting target
USING (
    SELECT @Id [Id]
) source
ON    (
    target.Id   = source.Id
)
WHEN MATCHED THEN
    UPDATE SET
           [Name]               = @Name
         , [DisplayOrder]       = @DisplayOrder
         , [IsUseable]          = @IsUseable
         , [EBFormatId]         = @EBFormatId
         , [FileFieldType]      = @FileFieldType
         , [BankCode]           = @BankCode
         , [UseValueDate]       = @UseValueDate
         , [ImportableValues]   = @ImportableValues
         , [FilePath]           = @FilePath
         , [UpdateBy]           = @UpdateBy
         , [UpdateAt]           = GETDATE()
WHEN NOT MATCHED THEN
    INSERT ( CompanyId,  Name,  DisplayOrder,  IsUseable,  EBFormatId,  FileFieldType,  BankCode,  UseValueDate,  ImportableValues,  FilePath,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt )
    VALUES (@CompanyId, @Name, @DisplayOrder, @IsUseable, @EBFormatId, @FileFieldType, @BankCode, @UseValueDate, @ImportableValues, @FilePath, @CreateBy, GETDATE(), @UpdateBy, GETDATE() )
OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<EBFileSetting>(query, setting, token);
        }

        public Task<int> UpdateIsUseableAsync(int companyId, int loginUserId, IEnumerable<int> ids, CancellationToken token = default(CancellationToken))
        {
            var query = @"
UPDATE s
    SET
       s.[IsUseable]          = CASE WHEN EXISTS (SELECT 1 FROM @Ids id WHERE id.Id = s.Id) THEN 1 ELSE 0 END
     , s.[UpdateBy]           = @loginUserId
     , s.[UpdateAt]           = GETDATE()
  FROM EBFileSetting s
 WHERE s.Companyid             = @companyId
;";
            return dbHelper.ExecuteAsync(query, new {
                companyId,
                loginUserId,
                ids = ids.GetTableParameter()
            }, token);
        }
    }
}
