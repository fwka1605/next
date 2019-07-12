using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class ReminderCommonSettingQueryProcessor :
        IAddReminderCommonSettingQueryProcessor
    {
        private readonly IDbHelper dbHelper;
        public ReminderCommonSettingQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<ReminderCommonSetting> SaveAsync(ReminderCommonSetting ReminderCommonSetting, CancellationToken token = default(CancellationToken))
        {
            #region merge query
            var query = @"
MERGE INTO ReminderCommonSetting target
USING (
    SELECT @CompanyId   [CompanyId]
) source
ON    (
        target.CompanyId    = source.CompanyId
)
WHEN MATCHED THEN
    UPDATE SET 
         OwnDepartmentName      = @OwnDepartmentName
        ,AccountingStaffName    = @AccountingStaffName
        ,OutputDetail           = @OutputDetail
        ,OutputDetailItem       = @OutputDetailItem
        ,ReminderManagementMode = @ReminderManagementMode
        ,DepartmentSummaryMode  = @DepartmentSummaryMode
        ,CalculateBaseDate      = @CalculateBaseDate
        ,IncludeOnTheDay        = @IncludeOnTheDay
        ,DisplayArrearsInterest = @DisplayArrearsInterest
        ,ArrearsInterestRate    = @ArrearsInterestRate
        ,UpdateBy = @UpdateBy
        ,UpdateAt = GETDATE()
WHEN NOT MATCHED THEN 
    INSERT (CompanyId, OwnDepartmentName, AccountingStaffName, ReminderManagementMode, DepartmentSummaryMode, OutputDetail, OutputDetailItem, CalculateBaseDate, IncludeOnTheDay, DisplayArrearsInterest, ArrearsInterestRate, CreateBy, CreateAt, UpdateBy, UpdateAt) 
    VALUES (@CompanyId, @OwnDepartmentName, @AccountingStaffName, @ReminderManagementMode, @DepartmentSummaryMode, @OutputDetail, @OutputDetailItem, @CalculateBaseDate, @IncludeOnTheDay, @DisplayArrearsInterest, @ArrearsInterestRate, @UpdateBy, GETDATE(), @UpdateBy, GETDATE()) 
OUTPUT inserted.*; ";
            #endregion

            return dbHelper.ExecuteAsync<ReminderCommonSetting>(query, ReminderCommonSetting, token);
        }

    }
}
