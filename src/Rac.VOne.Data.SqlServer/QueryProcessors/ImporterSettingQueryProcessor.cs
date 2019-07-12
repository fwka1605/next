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
    public class ImporterSettingQueryProcessor :
        IImporterSettingQueryProcessor,
        IAddImporterSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ImporterSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<ImporterSetting>> GetAsync(ImporterSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        ImporterSetting
WHERE       Id              = Id";
            if (setting.Id != 0) query += @"
AND         Id              = @Id";
            else query += @"
AND         CompanyId       = @CompanyId";

            if (setting.FormatId != 0) query += @"
AND         FormatId        = @FormatId";
            if (!string.IsNullOrWhiteSpace(setting.Code)) query += @"
AND         Code            = @Code";
            query += @"
ORDER BY    CompanyId       ASC
          , FormatId        ASC
          , Code            ASC";
            return dbHelper.GetItemsAsync<ImporterSetting>(query, setting, token);
        }


        public Task<ImporterSetting> SaveAsync(ImporterSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO ImporterSetting AS target
USING (SELECT
        @CompanyId  CompanyId
      , @FormatId   FormatId
      , @Code       Code
   ) As source
ON (
        target.CompanyId   = source.CompanyId
    AND target.FormatId    = source.FormatId
    AND target.Code        = source.Code
) 
WHEN MATCHED THEN
    UPDATE SET
     Code                   = @Code
    ,Name                   = @Name
    ,InitialDirectory       = @InitialDirectory
    ,StartLineCount         = @StartLineCount
    ,IgnoreLastLine         = @IgnoreLastLine
    ,AutoCreationCustomer   = @AutoCreationCustomer
    ,PostAction             = @PostAction
    ,UpdateBy               = @UpdateBy
    ,UpdateAt               = GETDATE()
WHEN NOT MATCHED THEN 
 INSERT ( CompanyId,  FormatId,  Code,  Name,  InitialDirectory,  EncodingCodePage,  StartLineCount,  IgnoreLastLine,  AutoCreationCustomer,  PostAction,  CreateBy,  CreateAt, UpdateBy, UpdateAt)
 VALUES (@CompanyId, @FormatId, @Code, @Name, @InitialDirectory, @EncodingCodePage, @StartLineCount, @IgnoreLastLine, @AutoCreationCustomer, @PostAction, @CreateBy, GETDATE(),@UpdateBy, GETDATE())
 OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<ImporterSetting>(query, setting, token);
        }


    }
}