﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.MenuAuthorityMasterService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MenuAuthorityMasterService.IMenuAuthorityMaster")]
    public interface IMenuAuthorityMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMenuAuthorityMaster/Delete", ReplyAction="http://tempuri.org/IMenuAuthorityMaster/DeleteResponse")]
        Rac.VOne.Web.Models.CountResult Delete(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMenuAuthorityMaster/Delete", ReplyAction="http://tempuri.org/IMenuAuthorityMaster/DeleteResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteAsync(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMenuAuthorityMaster/GetItems", ReplyAction="http://tempuri.org/IMenuAuthorityMaster/GetItemsResponse")]
        Rac.VOne.Web.Models.MenuAuthoritiesResult GetItems(string SessionKey, System.Nullable<int> CompanyId, System.Nullable<int> LoginUserId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMenuAuthorityMaster/GetItems", ReplyAction="http://tempuri.org/IMenuAuthorityMaster/GetItemsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.MenuAuthoritiesResult> GetItemsAsync(string SessionKey, System.Nullable<int> CompanyId, System.Nullable<int> LoginUserId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMenuAuthorityMaster/Save", ReplyAction="http://tempuri.org/IMenuAuthorityMaster/SaveResponse")]
        Rac.VOne.Web.Models.MenuAuthoritiesResult Save(string SessionKey, Rac.VOne.Web.Models.MenuAuthority[] menuAuthorities);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMenuAuthorityMaster/Save", ReplyAction="http://tempuri.org/IMenuAuthorityMaster/SaveResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.MenuAuthoritiesResult> SaveAsync(string SessionKey, Rac.VOne.Web.Models.MenuAuthority[] menuAuthorities);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMenuAuthorityMasterChannel : Rac.VOne.Client.Screen.MenuAuthorityMasterService.IMenuAuthorityMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MenuAuthorityMasterClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.MenuAuthorityMasterService.IMenuAuthorityMaster>, Rac.VOne.Client.Screen.MenuAuthorityMasterService.IMenuAuthorityMaster {
        
        public MenuAuthorityMasterClient() {
        }
        
        public MenuAuthorityMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MenuAuthorityMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MenuAuthorityMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MenuAuthorityMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.CountResult Delete(string SessionKey, int CompanyId) {
            return base.Channel.Delete(SessionKey, CompanyId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteAsync(string SessionKey, int CompanyId) {
            return base.Channel.DeleteAsync(SessionKey, CompanyId);
        }
        
        public Rac.VOne.Web.Models.MenuAuthoritiesResult GetItems(string SessionKey, System.Nullable<int> CompanyId, System.Nullable<int> LoginUserId) {
            return base.Channel.GetItems(SessionKey, CompanyId, LoginUserId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.MenuAuthoritiesResult> GetItemsAsync(string SessionKey, System.Nullable<int> CompanyId, System.Nullable<int> LoginUserId) {
            return base.Channel.GetItemsAsync(SessionKey, CompanyId, LoginUserId);
        }
        
        public Rac.VOne.Web.Models.MenuAuthoritiesResult Save(string SessionKey, Rac.VOne.Web.Models.MenuAuthority[] menuAuthorities) {
            return base.Channel.Save(SessionKey, menuAuthorities);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.MenuAuthoritiesResult> SaveAsync(string SessionKey, Rac.VOne.Web.Models.MenuAuthority[] menuAuthorities) {
            return base.Channel.SaveAsync(SessionKey, menuAuthorities);
        }
    }
}
