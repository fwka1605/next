using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ColumnNameSettingQueryProcessor :
        IColumnNameSettingQueryProcessor,
        IAddColumnNameSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ColumnNameSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<IEnumerable<ColumnNameSetting>> GetAsync(ColumnNameSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      cns.*
FROM        ColumnNameSetting cns
WHERE       cns.CompanyId       = @CompanyId";
            if (!string.IsNullOrEmpty(setting.TableName)) query += @"
AND         cns.TableName       = @TableName";
            if (!string.IsNullOrEmpty(setting.ColumnName)) query += @"
AND         cns.ColumnName      = @ColumnName";
            query += @"
ORDER BY    cns.CompanyId       ASC
          , cns.TableName       ASC
          , cns.ColumnName      ASC";
            return dbHelper.GetItemsAsync<ColumnNameSetting>(query, setting, token);
        }

        public Task<ColumnNameSetting> SaveAsync(ColumnNameSetting ColumnName, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO ColumnNameSetting AS Org 
USING ( 
    SELECT 
        @CompanyId AS CompanyId 
       ,@TableName AS TableName
       ,@ColumnName AS ColumnName
) AS Target 
ON ( 
    Org.CompanyId = @CompanyId 
    AND Org.TableName = @TableName
    AND Org.ColumnName = @ColumnName
) 
WHEN MATCHED THEN 
    UPDATE SET
     Alias = @Alias
     ,UpdateBy = @UpdateBy
     ,UpdateAt = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, TableName, ColumnName, OriginalName, Alias,CreateBy,CreateAt,UpdateBy,UpdateAt) 
    VALUES (@CompanyId, @TableName, @ColumnName, @OriginalName, @Alias,@CreateBy,GETDATE(),@UpdateBy,GETDATE()) 
OUTPUT inserted.*; ";
            #endregion
            return dbHelper.ExecuteAsync<ColumnNameSetting>(query, ColumnName, token);
        }
    }
}
