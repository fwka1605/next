﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.WebApiSettingMasterService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WebApiSettingMasterService.IWebApiSettingMaster")]
    public interface IWebApiSettingMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebApiSettingMaster/Save", ReplyAction="http://tempuri.org/IWebApiSettingMaster/SaveResponse")]
        Rac.VOne.Web.Models.CountResult Save(string SessionKey, Rac.VOne.Web.Models.WebApiSetting setting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebApiSettingMaster/Save", ReplyAction="http://tempuri.org/IWebApiSettingMaster/SaveResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> SaveAsync(string SessionKey, Rac.VOne.Web.Models.WebApiSetting setting);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebApiSettingMaster/Delete", ReplyAction="http://tempuri.org/IWebApiSettingMaster/DeleteResponse")]
        Rac.VOne.Web.Models.CountResult Delete(string SessionKey, int CompanyId, System.Nullable<int> ApiTypeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebApiSettingMaster/Delete", ReplyAction="http://tempuri.org/IWebApiSettingMaster/DeleteResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteAsync(string SessionKey, int CompanyId, System.Nullable<int> ApiTypeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebApiSettingMaster/GetById", ReplyAction="http://tempuri.org/IWebApiSettingMaster/GetByIdResponse")]
        Rac.VOne.Web.Models.WebApiSettingResult GetById(string SessionKey, int CompanyId, int ApiTypeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebApiSettingMaster/GetById", ReplyAction="http://tempuri.org/IWebApiSettingMaster/GetByIdResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.WebApiSettingResult> GetByIdAsync(string SessionKey, int CompanyId, int ApiTypeId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWebApiSettingMasterChannel : Rac.VOne.Client.Screen.WebApiSettingMasterService.IWebApiSettingMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebApiSettingMasterClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.WebApiSettingMasterService.IWebApiSettingMaster>, Rac.VOne.Client.Screen.WebApiSettingMasterService.IWebApiSettingMaster {
        
        public WebApiSettingMasterClient() {
        }
        
        public WebApiSettingMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebApiSettingMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebApiSettingMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebApiSettingMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.CountResult Save(string SessionKey, Rac.VOne.Web.Models.WebApiSetting setting) {
            return base.Channel.Save(SessionKey, setting);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> SaveAsync(string SessionKey, Rac.VOne.Web.Models.WebApiSetting setting) {
            return base.Channel.SaveAsync(SessionKey, setting);
        }
        
        public Rac.VOne.Web.Models.CountResult Delete(string SessionKey, int CompanyId, System.Nullable<int> ApiTypeId) {
            return base.Channel.Delete(SessionKey, CompanyId, ApiTypeId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteAsync(string SessionKey, int CompanyId, System.Nullable<int> ApiTypeId) {
            return base.Channel.DeleteAsync(SessionKey, CompanyId, ApiTypeId);
        }
        
        public Rac.VOne.Web.Models.WebApiSettingResult GetById(string SessionKey, int CompanyId, int ApiTypeId) {
            return base.Channel.GetById(SessionKey, CompanyId, ApiTypeId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.WebApiSettingResult> GetByIdAsync(string SessionKey, int CompanyId, int ApiTypeId) {
            return base.Channel.GetByIdAsync(SessionKey, CompanyId, ApiTypeId);
        }
    }
}
