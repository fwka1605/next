﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.ReminderSettingService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ReminderSettingService.IReminderSettingService")]
    public interface IReminderSettingService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderCommonSetting", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderCommonSettingResponse")]
        Rac.VOne.Web.Models.ReminderCommonSettingResult GetReminderCommonSetting(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderCommonSetting", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderCommonSettingResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderCommonSettingResult> GetReminderCommonSettingAsync(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/SaveReminderCommonSetting", ReplyAction="http://tempuri.org/IReminderSettingService/SaveReminderCommonSettingResponse")]
        Rac.VOne.Web.Models.ReminderCommonSettingResult SaveReminderCommonSetting(string SessionKey, Rac.VOne.Web.Models.ReminderCommonSetting ReminderCommonSetting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/SaveReminderCommonSetting", ReplyAction="http://tempuri.org/IReminderSettingService/SaveReminderCommonSettingResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderCommonSettingResult> SaveReminderCommonSettingAsync(string SessionKey, Rac.VOne.Web.Models.ReminderCommonSetting ReminderCommonSetting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderTemplateSettings", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderTemplateSettingsResponse")]
        Rac.VOne.Web.Models.ReminderTemplateSettingsResult GetReminderTemplateSettings(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderTemplateSettings", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderTemplateSettingsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderTemplateSettingsResult> GetReminderTemplateSettingsAsync(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderTemplateSettingByCode", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderTemplateSettingByCodeRespon" +
            "se")]
        Rac.VOne.Web.Models.ReminderTemplateSettingResult GetReminderTemplateSettingByCode(string SessionKey, int CompanyId, string Code);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderTemplateSettingByCode", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderTemplateSettingByCodeRespon" +
            "se")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderTemplateSettingResult> GetReminderTemplateSettingByCodeAsync(string SessionKey, int CompanyId, string Code);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/SaveReminderTemplateSetting", ReplyAction="http://tempuri.org/IReminderSettingService/SaveReminderTemplateSettingResponse")]
        Rac.VOne.Web.Models.ReminderTemplateSettingResult SaveReminderTemplateSetting(string SessionKey, Rac.VOne.Web.Models.ReminderTemplateSetting ReminderTemplateSetting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/SaveReminderTemplateSetting", ReplyAction="http://tempuri.org/IReminderSettingService/SaveReminderTemplateSettingResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderTemplateSettingResult> SaveReminderTemplateSettingAsync(string SessionKey, Rac.VOne.Web.Models.ReminderTemplateSetting ReminderTemplateSetting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/DeleteReminderTemplateSetting", ReplyAction="http://tempuri.org/IReminderSettingService/DeleteReminderTemplateSettingResponse")]
        Rac.VOne.Web.Models.CountResult DeleteReminderTemplateSetting(string SessionKey, int Id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/DeleteReminderTemplateSetting", ReplyAction="http://tempuri.org/IReminderSettingService/DeleteReminderTemplateSettingResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteReminderTemplateSettingAsync(string SessionKey, int Id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/ExistReminderTemplateSetting", ReplyAction="http://tempuri.org/IReminderSettingService/ExistReminderTemplateSettingResponse")]
        Rac.VOne.Web.Models.ExistResult ExistReminderTemplateSetting(string SessionKey, int ReminderTemplateId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/ExistReminderTemplateSetting", ReplyAction="http://tempuri.org/IReminderSettingService/ExistReminderTemplateSettingResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExistResult> ExistReminderTemplateSettingAsync(string SessionKey, int ReminderTemplateId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderLevelSettings", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderLevelSettingsResponse")]
        Rac.VOne.Web.Models.ReminderLevelSettingsResult GetReminderLevelSettings(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderLevelSettings", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderLevelSettingsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderLevelSettingsResult> GetReminderLevelSettingsAsync(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderLevelSettingByLevel", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderLevelSettingByLevelResponse" +
            "")]
        Rac.VOne.Web.Models.ReminderLevelSettingResult GetReminderLevelSettingByLevel(string SessionKey, int CompanyId, int ReminderLevel);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderLevelSettingByLevel", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderLevelSettingByLevelResponse" +
            "")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderLevelSettingResult> GetReminderLevelSettingByLevelAsync(string SessionKey, int CompanyId, int ReminderLevel);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/ExistTemplateAtReminderLevel", ReplyAction="http://tempuri.org/IReminderSettingService/ExistTemplateAtReminderLevelResponse")]
        Rac.VOne.Web.Models.ExistResult ExistTemplateAtReminderLevel(string SessionKey, int ReminderTemplateId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/ExistTemplateAtReminderLevel", ReplyAction="http://tempuri.org/IReminderSettingService/ExistTemplateAtReminderLevelResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExistResult> ExistTemplateAtReminderLevelAsync(string SessionKey, int ReminderTemplateId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetMaxReminderLevel", ReplyAction="http://tempuri.org/IReminderSettingService/GetMaxReminderLevelResponse")]
        Rac.VOne.Web.Models.MaxReminderLevelResult GetMaxReminderLevel(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetMaxReminderLevel", ReplyAction="http://tempuri.org/IReminderSettingService/GetMaxReminderLevelResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.MaxReminderLevelResult> GetMaxReminderLevelAsync(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/SaveReminderLevelSetting", ReplyAction="http://tempuri.org/IReminderSettingService/SaveReminderLevelSettingResponse")]
        Rac.VOne.Web.Models.ReminderLevelSettingResult SaveReminderLevelSetting(string SessionKey, Rac.VOne.Web.Models.ReminderLevelSetting ReminderLevelSetting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/SaveReminderLevelSetting", ReplyAction="http://tempuri.org/IReminderSettingService/SaveReminderLevelSettingResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderLevelSettingResult> SaveReminderLevelSettingAsync(string SessionKey, Rac.VOne.Web.Models.ReminderLevelSetting ReminderLevelSetting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/DeleteReminderLevelSetting", ReplyAction="http://tempuri.org/IReminderSettingService/DeleteReminderLevelSettingResponse")]
        Rac.VOne.Web.Models.CountResult DeleteReminderLevelSetting(string SessionKey, int CompanyId, int ReminderLevel);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/DeleteReminderLevelSetting", ReplyAction="http://tempuri.org/IReminderSettingService/DeleteReminderLevelSettingResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteReminderLevelSettingAsync(string SessionKey, int CompanyId, int ReminderLevel);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderSummarySettings", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderSummarySettingsResponse")]
        Rac.VOne.Web.Models.ReminderSummarySettingsResult GetReminderSummarySettings(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/GetReminderSummarySettings", ReplyAction="http://tempuri.org/IReminderSettingService/GetReminderSummarySettingsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderSummarySettingsResult> GetReminderSummarySettingsAsync(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/SaveReminderSummarySetting", ReplyAction="http://tempuri.org/IReminderSettingService/SaveReminderSummarySettingResponse")]
        Rac.VOne.Web.Models.ReminderSummarySettingsResult SaveReminderSummarySetting(string SessionKey, Rac.VOne.Web.Models.ReminderSummarySetting[] ReminderSummarySettings);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReminderSettingService/SaveReminderSummarySetting", ReplyAction="http://tempuri.org/IReminderSettingService/SaveReminderSummarySettingResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderSummarySettingsResult> SaveReminderSummarySettingAsync(string SessionKey, Rac.VOne.Web.Models.ReminderSummarySetting[] ReminderSummarySettings);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IReminderSettingServiceChannel : Rac.VOne.Client.Screen.ReminderSettingService.IReminderSettingService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ReminderSettingServiceClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.ReminderSettingService.IReminderSettingService>, Rac.VOne.Client.Screen.ReminderSettingService.IReminderSettingService {
        
        public ReminderSettingServiceClient() {
        }
        
        public ReminderSettingServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ReminderSettingServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReminderSettingServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReminderSettingServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.ReminderCommonSettingResult GetReminderCommonSetting(string SessionKey, int CompanyId) {
            return base.Channel.GetReminderCommonSetting(SessionKey, CompanyId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderCommonSettingResult> GetReminderCommonSettingAsync(string SessionKey, int CompanyId) {
            return base.Channel.GetReminderCommonSettingAsync(SessionKey, CompanyId);
        }
        
        public Rac.VOne.Web.Models.ReminderCommonSettingResult SaveReminderCommonSetting(string SessionKey, Rac.VOne.Web.Models.ReminderCommonSetting ReminderCommonSetting) {
            return base.Channel.SaveReminderCommonSetting(SessionKey, ReminderCommonSetting);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderCommonSettingResult> SaveReminderCommonSettingAsync(string SessionKey, Rac.VOne.Web.Models.ReminderCommonSetting ReminderCommonSetting) {
            return base.Channel.SaveReminderCommonSettingAsync(SessionKey, ReminderCommonSetting);
        }
        
        public Rac.VOne.Web.Models.ReminderTemplateSettingsResult GetReminderTemplateSettings(string SessionKey, int CompanyId) {
            return base.Channel.GetReminderTemplateSettings(SessionKey, CompanyId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderTemplateSettingsResult> GetReminderTemplateSettingsAsync(string SessionKey, int CompanyId) {
            return base.Channel.GetReminderTemplateSettingsAsync(SessionKey, CompanyId);
        }
        
        public Rac.VOne.Web.Models.ReminderTemplateSettingResult GetReminderTemplateSettingByCode(string SessionKey, int CompanyId, string Code) {
            return base.Channel.GetReminderTemplateSettingByCode(SessionKey, CompanyId, Code);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderTemplateSettingResult> GetReminderTemplateSettingByCodeAsync(string SessionKey, int CompanyId, string Code) {
            return base.Channel.GetReminderTemplateSettingByCodeAsync(SessionKey, CompanyId, Code);
        }
        
        public Rac.VOne.Web.Models.ReminderTemplateSettingResult SaveReminderTemplateSetting(string SessionKey, Rac.VOne.Web.Models.ReminderTemplateSetting ReminderTemplateSetting) {
            return base.Channel.SaveReminderTemplateSetting(SessionKey, ReminderTemplateSetting);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderTemplateSettingResult> SaveReminderTemplateSettingAsync(string SessionKey, Rac.VOne.Web.Models.ReminderTemplateSetting ReminderTemplateSetting) {
            return base.Channel.SaveReminderTemplateSettingAsync(SessionKey, ReminderTemplateSetting);
        }
        
        public Rac.VOne.Web.Models.CountResult DeleteReminderTemplateSetting(string SessionKey, int Id) {
            return base.Channel.DeleteReminderTemplateSetting(SessionKey, Id);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteReminderTemplateSettingAsync(string SessionKey, int Id) {
            return base.Channel.DeleteReminderTemplateSettingAsync(SessionKey, Id);
        }
        
        public Rac.VOne.Web.Models.ExistResult ExistReminderTemplateSetting(string SessionKey, int ReminderTemplateId) {
            return base.Channel.ExistReminderTemplateSetting(SessionKey, ReminderTemplateId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExistResult> ExistReminderTemplateSettingAsync(string SessionKey, int ReminderTemplateId) {
            return base.Channel.ExistReminderTemplateSettingAsync(SessionKey, ReminderTemplateId);
        }
        
        public Rac.VOne.Web.Models.ReminderLevelSettingsResult GetReminderLevelSettings(string SessionKey, int CompanyId) {
            return base.Channel.GetReminderLevelSettings(SessionKey, CompanyId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderLevelSettingsResult> GetReminderLevelSettingsAsync(string SessionKey, int CompanyId) {
            return base.Channel.GetReminderLevelSettingsAsync(SessionKey, CompanyId);
        }
        
        public Rac.VOne.Web.Models.ReminderLevelSettingResult GetReminderLevelSettingByLevel(string SessionKey, int CompanyId, int ReminderLevel) {
            return base.Channel.GetReminderLevelSettingByLevel(SessionKey, CompanyId, ReminderLevel);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderLevelSettingResult> GetReminderLevelSettingByLevelAsync(string SessionKey, int CompanyId, int ReminderLevel) {
            return base.Channel.GetReminderLevelSettingByLevelAsync(SessionKey, CompanyId, ReminderLevel);
        }
        
        public Rac.VOne.Web.Models.ExistResult ExistTemplateAtReminderLevel(string SessionKey, int ReminderTemplateId) {
            return base.Channel.ExistTemplateAtReminderLevel(SessionKey, ReminderTemplateId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExistResult> ExistTemplateAtReminderLevelAsync(string SessionKey, int ReminderTemplateId) {
            return base.Channel.ExistTemplateAtReminderLevelAsync(SessionKey, ReminderTemplateId);
        }
        
        public Rac.VOne.Web.Models.MaxReminderLevelResult GetMaxReminderLevel(string SessionKey, int CompanyId) {
            return base.Channel.GetMaxReminderLevel(SessionKey, CompanyId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.MaxReminderLevelResult> GetMaxReminderLevelAsync(string SessionKey, int CompanyId) {
            return base.Channel.GetMaxReminderLevelAsync(SessionKey, CompanyId);
        }
        
        public Rac.VOne.Web.Models.ReminderLevelSettingResult SaveReminderLevelSetting(string SessionKey, Rac.VOne.Web.Models.ReminderLevelSetting ReminderLevelSetting) {
            return base.Channel.SaveReminderLevelSetting(SessionKey, ReminderLevelSetting);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderLevelSettingResult> SaveReminderLevelSettingAsync(string SessionKey, Rac.VOne.Web.Models.ReminderLevelSetting ReminderLevelSetting) {
            return base.Channel.SaveReminderLevelSettingAsync(SessionKey, ReminderLevelSetting);
        }
        
        public Rac.VOne.Web.Models.CountResult DeleteReminderLevelSetting(string SessionKey, int CompanyId, int ReminderLevel) {
            return base.Channel.DeleteReminderLevelSetting(SessionKey, CompanyId, ReminderLevel);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteReminderLevelSettingAsync(string SessionKey, int CompanyId, int ReminderLevel) {
            return base.Channel.DeleteReminderLevelSettingAsync(SessionKey, CompanyId, ReminderLevel);
        }
        
        public Rac.VOne.Web.Models.ReminderSummarySettingsResult GetReminderSummarySettings(string SessionKey, int CompanyId) {
            return base.Channel.GetReminderSummarySettings(SessionKey, CompanyId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderSummarySettingsResult> GetReminderSummarySettingsAsync(string SessionKey, int CompanyId) {
            return base.Channel.GetReminderSummarySettingsAsync(SessionKey, CompanyId);
        }
        
        public Rac.VOne.Web.Models.ReminderSummarySettingsResult SaveReminderSummarySetting(string SessionKey, Rac.VOne.Web.Models.ReminderSummarySetting[] ReminderSummarySettings) {
            return base.Channel.SaveReminderSummarySetting(SessionKey, ReminderSummarySettings);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ReminderSummarySettingsResult> SaveReminderSummarySettingAsync(string SessionKey, Rac.VOne.Web.Models.ReminderSummarySetting[] ReminderSummarySettings) {
            return base.Channel.SaveReminderSummarySettingAsync(SessionKey, ReminderSummarySettings);
        }
    }
}
