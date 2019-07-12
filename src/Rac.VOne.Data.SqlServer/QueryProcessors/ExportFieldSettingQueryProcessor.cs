using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ExportFieldSettingQueryProcessor :
        IAddExportFieldSettingQueryProcessor,
        IExportFieldSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ExportFieldSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<ExportFieldSetting>> GetAsync(ExportFieldSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT es.CompanyId
     , es.ColumnName
     , esb.Caption
     , es.ColumnOrder [ColumnOrder]
     , es.AllowExport
     , es.ExportFileType
     , esb.DataType
     , es.DataFormat
  FROM ExportFieldSetting es
 INNER JOIN ExportFieldSettingBase esb
    ON es.CompanyId         = @CompanyId
   AND es.ExportFileType    = @ExportFileType
   AND es.ExportFileType    = esb.ExportFileType
   AND es.ColumnName        = esb.ColumnName
 UNION ALL
SELECT @CompanyId [CompanyId]
     , esb.ColumnName
     , esb.Caption
     , esb.ColumnOrder [ColumnOrder]
     , esb.AllowExport
     , esb.ExportFileType
     , esb.DataType
     , 0 AS DataFormat
  FROM ExportFieldSettingBase esb
 WHERE esb.ExportFileType = @ExportFileType
   AND NOT EXISTS (
       SELECT 1
         FROM ExportFieldSetting es
        WHERE es.CompanyId      = @CompanyId
          AND es.ExportFileType = @ExportFileType
          AND es.ColumnName     = esb.ColumnName )
 ORDER BY ColumnOrder; ";
            return dbHelper.GetItemsAsync<ExportFieldSetting>(query, setting, token);
        }

        public Task<ExportFieldSetting> SaveAsync(ExportFieldSetting setting, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO ExportFieldSetting target
USING (
    SELECT @CompanyId       CompanyId
         , @ExportFileType  ExportFileType
         , @ColumnName      ColumnName
) source ON (
        target.CompanyId        = source.CompanyId
    AND target.ExportFileType   = source.ExportFileType
    AND target.ColumnName       = source.ColumnName
)
WHEN MATCHED THEN
    UPDATE SET
           ColumnOrder  = @ColumnOrder
         , AllowExport  = @AllowExport
         , DataFormat   = @DataFormat
         , UpdateBy     = @UpdateBy
         , UpdateAt     = GETDATE()
WHEN NOT MATCHED THEN
    INSERT
         ( CompanyId
         , ExportFileType
         , ColumnName
         , ColumnOrder
         , AllowExport
         , CreateBy
         , CreateAt
         , UpdateBy
         , UpdateAt
         , DataFormat )
    VALUES
         ( @CompanyId
         , @ExportFileType
         , @ColumnName
         , @ColumnOrder
         , @AllowExport
         , @UpdateBy
         , GETDATE()
         , @UpdateBy
         , GETDATE()
         , @DataFormat )
    OUTPUT inserted.*;";
            #endregion
            return dbHelper.ExecuteAsync<ExportFieldSetting>(query, setting, token);
        }

    }
}
