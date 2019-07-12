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
    public class ImporterSettingDetailQueryProcessor :
        IImporterSettingDetailQueryProcessor,
        IAddImporterSettingDetailQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ImporterSettingDetailQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<ImporterSettingDetail> SaveAsync(ImporterSettingDetail detail, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO ImporterSettingDetail AS target
USING ( 
    SELECT @ImporterSettingId   [ImporterSettingId]
         , @Sequence            [Sequence]
) AS source 
ON ( 
     target.ImporterSettingId  = source.[ImporterSettingId]
 AND target.[Sequence]         = source.[Sequence]
) 
WHEN MATCHED THEN 
    UPDATE SET ImportDivision       = @ImportDivision
             , FieldIndex           = COALESCE(@FieldIndex, 0)
             , FixedValue           = @FixedValue
             , Caption              = @Caption
             , IsUnique             = @IsUnique
             , DoOverwrite          = @DoOverwrite
             , AttributeDivision    = @AttributeDivision
             , ItemPriority         = @ItemPriority
             , UpdateKey            = @UpdateKey
             , UpdateBy             = @UpdateBy
             , UpdateAt             = @UpdateAt
WHEN NOT MATCHED THEN 
    INSERT (ImporterSettingId, Sequence, ImportDivision, FieldIndex, Caption, IsUnique, AttributeDivision, DoOverwrite, FixedValue, ItemPriority, UpdateKey, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@ImporterSettingId, @Sequence, @ImportDivision, COALESCE(@FieldIndex, 0), @Caption, @IsUnique, @AttributeDivision, @DoOverwrite, @FixedValue, @ItemPriority, @UpdateKey, @CreateBy, @CreateAt, @UpdateBy,@UpdateAt)  
OUTPUT inserted.*;";
            return dbHelper.ExecuteAsync<ImporterSettingDetail>(query, detail, token);
        }

        public async Task<IEnumerable<ImporterSettingDetail>> GetAsync(ImporterSetting setting, CancellationToken token = default(CancellationToken))
        {
            var byId = setting.Id != 0;
            var hasValue = await HasValueAsync(setting, token);
            var query = "";
            if (hasValue)
            {
                query = @"
SELECT      d.*
          , s.[UpdateAt]             [ImporterSettingUpdateAt]
          , b.[FieldName]
          , b.[ImportDivision]       [BaseImportDivision]
          , b.[AttributeDivision]    [BaseAttributeDivision]
          , b.[TargetColumn]
FROM        [dbo].[ImporterSettingDetail] d
INNER JOIN  [dbo].[ImporterSetting] s           ON s.[Id]       = d.[ImporterSettingId]
INNER JOIN  [dbo].[ImporterSettingBase] b       ON b.[Sequence] = d.[Sequence]
                                               AND b.[FormatId] = s.[FormatId]
WHERE       d.[ImporterSettingId]       = d.[ImporterSettingId]";
                if (byId) query += @"
AND         s.[Id]                      = @Id";
                else query += @"
AND         s.[CompanyId]               = @CompanyId
AND         s.[FormatId]                = @FormatId
AND         s.[Code]                    = @Code";
                query += @"
ORDER BY    d.[ImporterSettingId]       ASC
          , d.[Sequence]                ASC";
            }
            else
            {
                query = @"
SELECT      0 [ImporterSettingId]
          , b.[Sequence]
          , b.[FieldName]
          , b.[ImportDivision]       [BaseImportDivision]
          , b.[AttributeDivision]    [BaseAttributeDivision]
          , b.[TargetColumn]
FROM        [dbo].[ImporterSettingBase] b
WHERE       b.[FormatId]            = @FormatId
ORDER BY    b.[FormatId]            ASC
          , b.[Sequence]            ASC";
            }

            return await dbHelper.GetItemsAsync<ImporterSettingDetail>(query, setting, token);
        }

        private async Task<bool> HasValueAsync(ImporterSetting setting, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      1
WHERE EXISTS (
            SELECT      *
            FROM        [dbo].[ImporterSetting] s
            WHERE       s.Id                    = s.Id";
            if (setting.Id != 0) query += @"
            AND         s.Id                    = @Id";
            else query += @"
            AND         s.CompanyId             = @CompanyId
            AND         s.FormatId              = @FormatId
            AND         s.Code                  = @Code";
            query += @"
            )";
            return (await dbHelper.ExecuteAsync<int?>(query, setting, token)).HasValue;
        }



    }
}
