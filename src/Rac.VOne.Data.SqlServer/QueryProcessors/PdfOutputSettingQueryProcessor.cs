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
    public class PdfOutputSettingQueryProcessor :
        IPdfOutputSettingQueryProcessor,
        IAddPdfOutputSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public PdfOutputSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public Task<PdfOutputSetting> GetAsync(int CompanyId, int ReportType, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT *
  FROM PdfOutputSetting p
 WHERE p.CompanyId  = @CompanyId
   AND p.ReportType = @ReportType";

           return dbHelper.ExecuteAsync<PdfOutputSetting>(query, new { CompanyId, ReportType }, token);
        }
        public Task<PdfOutputSetting> SaveAsync(PdfOutputSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO PdfOutputSetting AS target
USING ( 
    SELECT 
     @CompanyId  AS CompanyId
    ,@ReportType AS ReportType
) AS source 
ON ( 
        target.CompanyId  = source.CompanyId
    AND target.ReportType = source.ReportType
) 
WHEN MATCHED THEN 
    UPDATE SET
         OutputUnit     = @OutputUnit
        ,FileName       = @FileName
        ,UseCompression = @UseCompression
        ,MaximumSize    = @MaximumSize
        ,UpdateBy       = @UpdateBy
        ,UpdateAt       = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (
         CompanyId
        ,ReportType
        ,OutputUnit
        ,FileName
        ,UseCompression
        ,MaximumSize
        ,CreateBy
        ,CreateAt
        ,UpdateBy
        ,UpdateAt
    ) VALUES (
         @CompanyId
        ,@ReportType
        ,@OutputUnit
        ,@FileName
        ,@UseCompression
        ,@MaximumSize
        ,@CreateBy
        ,GETDATE()
        ,@UpdateBy
        ,GETDATE()
        )
OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<PdfOutputSetting>(query, setting, token);
        }

    }
}
