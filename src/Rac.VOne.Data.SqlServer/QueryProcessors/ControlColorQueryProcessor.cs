using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.Entities;
using Rac.VOne.Data.QueryProcessors;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ControlColorQueryProcessor :
        IControlColorQueryProcessor,
        IAddControlColorQueryProcessor
    {
        private readonly IDbHelper dbHelper;

        public ControlColorQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<ControlColor> GetAsync(int CompanyId, int LoginUserId, CancellationToken token = default(CancellationToken))
        {
            var query = @"
SELECT      *
FROM        ControlColor
WHERE       CompanyId = @CompanyId
AND         LoginUserId = @LoginUserId";
            return dbHelper.ExecuteAsync<ControlColor>(query, new { CompanyId, LoginUserId }, token);
        }

        public Task<ControlColor> SaveAsync(ControlColor ControlColor, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO ControlColor AS Org
 USING (
     SELECT
     @CompanyId AS CompanyId
     ,@LoginUserId AS LoginUserId
 ) AS Target
 ON (
     Org.LoginUserId = @LoginUserId
 )
 WHEN MATCHED THEN
     UPDATE SET
      FormBackColor = @FormBackColor
     ,FormForeColor = @FormForeColor
     ,ControlEnableBackColor = @ControlEnableBackColor
     ,ControlDisableBackColor = @ControlDisableBackColor
     ,ControlForeColor = @ControlForeColor
     ,ControlRequiredBackColor = @ControlRequiredBackColor
     ,ControlActiveBackColor = @ControlActiveBackColor
     ,ButtonBackColor = @ButtonBackColor
     ,GridRowBackColor = @GridRowBackColor
     ,GridAlternatingRowBackColor = @GridAlternatingRowBackColor
     ,GridLineColor = @GridLineColor
     ,InputGridBackColor = @InputGridBackColor
     ,InputGridAlternatingBackColor = @InputGridAlternatingBackColor
     ,MatchingGridBillingBackColor = @MatchingGridBillingBackColor
     ,MatchingGridReceiptBackColor = @MatchingGridReceiptBackColor
     ,MatchingGridBillingSelectedRowBackColor = @MatchingGridBillingSelectedRowBackColor
     ,MatchingGridBillingSelectedCellBackColor = @MatchingGridBillingSelectedCellBackColor
     ,MatchingGridReceiptSelectedRowBackColor = @MatchingGridReceiptSelectedRowBackColor
     ,MatchingGridReceiptSelectedCellBackColor = @MatchingGridReceiptSelectedCellBackColor
     ,UpdateBy = @UpdateBy
     ,UpdateAt = GETDATE()
 WHEN NOT MATCHED THEN
    INSERT 
         ( CompanyId
         ,LoginUserId
         ,FormBackColor
         ,FormForeColor
         ,ControlEnableBackColor
         ,ControlDisableBackColor
         ,ControlForeColor
         ,ControlRequiredBackColor
         ,ControlActiveBackColor
         ,ButtonBackColor
         ,GridRowBackColor
         ,GridAlternatingRowBackColor
         ,GridLineColor
         ,InputGridBackColor
         ,InputGridAlternatingBackColor
         ,MatchingGridBillingBackColor
         ,MatchingGridReceiptBackColor
         ,MatchingGridBillingSelectedRowBackColor
         ,MatchingGridBillingSelectedCellBackColor
         ,MatchingGridReceiptSelectedRowBackColor
         ,MatchingGridReceiptSelectedCellBackColor
         ,CreateBy
         ,CreateAt
         ,UpdateBy
         ,UpdateAt
         )
         VALUES
         (@CompanyId
         ,@LoginUserId
         ,@FormBackColor
         ,@FormForeColor
         ,@ControlEnableBackColor
         ,@ControlDisableBackColor
         ,@ControlForeColor
         ,@ControlRequiredBackColor
         ,@ControlActiveBackColor
         ,@ButtonBackColor
         ,@GridRowBackColor
         ,@GridAlternatingRowBackColor
         ,@GridLineColor
         ,@InputGridBackColor
         ,@InputGridAlternatingBackColor
         ,@MatchingGridBillingBackColor
         ,@MatchingGridReceiptBackColor
         ,@MatchingGridBillingSelectedRowBackColor
         ,@MatchingGridBillingSelectedCellBackColor
         ,@MatchingGridReceiptSelectedRowBackColor
         ,@MatchingGridReceiptSelectedCellBackColor
         ,@CreateBy
         ,GETDATE()
         ,@UpdateBy
         ,GETDATE())
 OUTPUT inserted.*; ";
            #endregion
            return dbHelper.ExecuteAsync<ControlColor>(query, ControlColor, token);
        }
    }
}
