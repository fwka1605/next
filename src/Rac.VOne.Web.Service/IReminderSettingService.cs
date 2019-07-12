using Rac.VOne.Web.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IReminderSettingService" を変更できます。
    [ServiceContract]
    public interface IReminderSettingService
    {
        [OperationContract]
        Task<ReminderCommonSettingResult> GetReminderCommonSettingAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ReminderCommonSettingResult> SaveReminderCommonSettingAsync(string SessionKey, ReminderCommonSetting ReminderCommonSetting);

        [OperationContract]
        Task<ReminderTemplateSettingsResult> GetReminderTemplateSettingsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ReminderTemplateSettingResult> GetReminderTemplateSettingByCodeAsync(string SessionKey, int CompanyId, string Code);

        [OperationContract]
        Task<ReminderTemplateSettingResult> SaveReminderTemplateSettingAsync(string SessionKey, ReminderTemplateSetting ReminderTemplateSetting);

        [OperationContract]
        Task<CountResult> DeleteReminderTemplateSettingAsync(string SessionKey, int Id);

        [OperationContract]
        Task<ExistResult> ExistReminderTemplateSettingAsync(string SessionKey, int ReminderTemplateId);

        [OperationContract]
        Task<ReminderLevelSettingsResult> GetReminderLevelSettingsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ReminderLevelSettingResult> GetReminderLevelSettingByLevelAsync(string SessionKey, int CompanyId, int ReminderLevel);

        [OperationContract]
        Task<ExistResult> ExistTemplateAtReminderLevelAsync(string SessionKey, int ReminderTemplateId);

        [OperationContract]
        Task<MaxReminderLevelResult> GetMaxReminderLevelAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ReminderLevelSettingResult> SaveReminderLevelSettingAsync(string SessionKey, ReminderLevelSetting ReminderLevelSetting);

        [OperationContract]
        Task<CountResult> DeleteReminderLevelSettingAsync(string SessionKey, int CompanyId, int ReminderLevel);

        [OperationContract]
        Task<ReminderSummarySettingsResult> GetReminderSummarySettingsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<ReminderSummarySettingsResult> SaveReminderSummarySettingAsync(string SessionKey, ReminderSummarySetting[] ReminderSummarySettings);
    }
}
