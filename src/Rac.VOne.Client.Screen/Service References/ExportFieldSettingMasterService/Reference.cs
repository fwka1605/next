﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.ExportFieldSettingMasterService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ExportFieldSettingMasterService.IExportFieldSettingMaster")]
    public interface IExportFieldSettingMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExportFieldSettingMaster/GetItemsByExportFileType", ReplyAction="http://tempuri.org/IExportFieldSettingMaster/GetItemsByExportFileTypeResponse")]
        Rac.VOne.Web.Models.ExportFieldSettingsResult GetItemsByExportFileType(string SessionKey, int CompanyId, int ExportFileType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExportFieldSettingMaster/GetItemsByExportFileType", ReplyAction="http://tempuri.org/IExportFieldSettingMaster/GetItemsByExportFileTypeResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExportFieldSettingsResult> GetItemsByExportFileTypeAsync(string SessionKey, int CompanyId, int ExportFileType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExportFieldSettingMaster/Save", ReplyAction="http://tempuri.org/IExportFieldSettingMaster/SaveResponse")]
        Rac.VOne.Web.Models.ExportFieldSettingsResult Save(string SessionKey, Rac.VOne.Web.Models.ExportFieldSetting[] ExportFieldSetting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IExportFieldSettingMaster/Save", ReplyAction="http://tempuri.org/IExportFieldSettingMaster/SaveResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExportFieldSettingsResult> SaveAsync(string SessionKey, Rac.VOne.Web.Models.ExportFieldSetting[] ExportFieldSetting);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IExportFieldSettingMasterChannel : Rac.VOne.Client.Screen.ExportFieldSettingMasterService.IExportFieldSettingMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ExportFieldSettingMasterClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.ExportFieldSettingMasterService.IExportFieldSettingMaster>, Rac.VOne.Client.Screen.ExportFieldSettingMasterService.IExportFieldSettingMaster {
        
        public ExportFieldSettingMasterClient() {
        }
        
        public ExportFieldSettingMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ExportFieldSettingMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ExportFieldSettingMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ExportFieldSettingMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.ExportFieldSettingsResult GetItemsByExportFileType(string SessionKey, int CompanyId, int ExportFileType) {
            return base.Channel.GetItemsByExportFileType(SessionKey, CompanyId, ExportFileType);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExportFieldSettingsResult> GetItemsByExportFileTypeAsync(string SessionKey, int CompanyId, int ExportFileType) {
            return base.Channel.GetItemsByExportFileTypeAsync(SessionKey, CompanyId, ExportFileType);
        }
        
        public Rac.VOne.Web.Models.ExportFieldSettingsResult Save(string SessionKey, Rac.VOne.Web.Models.ExportFieldSetting[] ExportFieldSetting) {
            return base.Channel.Save(SessionKey, ExportFieldSetting);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExportFieldSettingsResult> SaveAsync(string SessionKey, Rac.VOne.Web.Models.ExportFieldSetting[] ExportFieldSetting) {
            return base.Channel.SaveAsync(SessionKey, ExportFieldSetting);
        }
    }
}