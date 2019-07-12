using System;
using System.Collections.Generic;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ImportSettingQueryProcessor :
        IUpdateImportSettingQueryProcessor,
        IImportSettingQueryProcessor,
        IInitializeImportSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ImportSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<ImportSetting>> GetAsync(ImportSettingSearch option, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      * 
FROM        MasterImportSetting 
WHERE       CompanyId       = @CompanyId ";
            if (option.ImportFileType.HasValue) query += @"
AND         ImportFileType  = @ImportFileType";
            return dbHelper.GetItemsAsync<ImportSetting>(query, option, token);
        }

        public Task<ImportSetting> SaveAsync(ImportSetting ImportSetting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO MasterImportSetting AS target
USING (
    SELECT
         @CompanyId         AS CompanyId
        ,@ImportFileType    AS ImportFileType
) AS source
ON (
        target.CompanyId        = source.CompanyId
    AND target.ImportFileType   = source.ImportFileType
)
WHEN MATCHED THEN
    UPDATE SET
      ImportFileName        = @ImportFileName
    , ImportMode            = @ImportMode
    , ExportErrorLog        = @ExportErrorLog
    , ErrorLogDestination   = @ErrorLogDestination
    , Confirm               = @Confirm
    , UpdateBy              = @UpdateBy 
    , UpdateAt              = GETDATE() 
WHEN NOT MATCHED THEN 
    INSERT ( CompanyId,  ImportFileType,  ImportFileName,  ImportMode,  ExportErrorLog,  ErrorLogDestination,  Confirm,  CreateBy,  CreateAt,  UpdateBy,  UpdateAt)
    VALUES (@CompanyId, @ImportFileType, @ImportFileName, @ImportMode, @ExportErrorLog, @ErrorLogDestination, @Confirm, @UpdateBy, GETDATE(), @UpdateBy, GETDATE())
OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<ImportSetting>(query, ImportSetting, token);
        }

        public Task<int> InitialzieAsync(int companyId, int loginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
INSERT INTO MasterImportSetting
(CompanyId, ImportFileType, ImportFileName, ImportMode, ExportErrorLog, ErrorLogDestination, Confirm, CreateBy, CreateAt, UpdateBy, UpdateAt)
SELECT
  @CompanyId
, ImportFileType
, ImportFileName
, ImportMode
, ExportErrorLog
, ErrorLogDestination
, Confirm
, @LoginUserId
, GETDATE()
, @LoginUserId
, GETDATE()
FROM MasterImportSettingBase";
            return dbHelper.ExecuteAsync(query, new { CompanyId = companyId, LoginUserId = loginUserId }, token);
        }
    }
}