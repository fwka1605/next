﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.LoginUserPasswordMasterService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LoginUserPasswordMasterService.ILoginUserPasswordMaster")]
    public interface ILoginUserPasswordMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoginUserPasswordMaster/Change", ReplyAction="http://tempuri.org/ILoginUserPasswordMaster/ChangeResponse")]
        Rac.VOne.Web.Models.LoginPasswordChangeResult Change(string SessionKey, int CompanyId, int LoginUserId, string OldPassword, string NewPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoginUserPasswordMaster/Change", ReplyAction="http://tempuri.org/ILoginUserPasswordMaster/ChangeResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.LoginPasswordChangeResult> ChangeAsync(string SessionKey, int CompanyId, int LoginUserId, string OldPassword, string NewPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoginUserPasswordMaster/Login", ReplyAction="http://tempuri.org/ILoginUserPasswordMaster/LoginResponse")]
        Rac.VOne.Web.Models.LoginProcessResult Login(string SessionKey, int CompanyId, int LoginUserId, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoginUserPasswordMaster/Login", ReplyAction="http://tempuri.org/ILoginUserPasswordMaster/LoginResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.LoginProcessResult> LoginAsync(string SessionKey, int CompanyId, int LoginUserId, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoginUserPasswordMaster/Save", ReplyAction="http://tempuri.org/ILoginUserPasswordMaster/SaveResponse")]
        Rac.VOne.Web.Models.LoginPasswordChangeResult Save(string SessionKey, int CompanyId, int LoginUserId, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoginUserPasswordMaster/Save", ReplyAction="http://tempuri.org/ILoginUserPasswordMaster/SaveResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.LoginPasswordChangeResult> SaveAsync(string SessionKey, int CompanyId, int LoginUserId, string Password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILoginUserPasswordMasterChannel : Rac.VOne.Client.Screen.LoginUserPasswordMasterService.ILoginUserPasswordMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoginUserPasswordMasterClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.LoginUserPasswordMasterService.ILoginUserPasswordMaster>, Rac.VOne.Client.Screen.LoginUserPasswordMasterService.ILoginUserPasswordMaster {
        
        public LoginUserPasswordMasterClient() {
        }
        
        public LoginUserPasswordMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LoginUserPasswordMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoginUserPasswordMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoginUserPasswordMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.LoginPasswordChangeResult Change(string SessionKey, int CompanyId, int LoginUserId, string OldPassword, string NewPassword) {
            return base.Channel.Change(SessionKey, CompanyId, LoginUserId, OldPassword, NewPassword);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.LoginPasswordChangeResult> ChangeAsync(string SessionKey, int CompanyId, int LoginUserId, string OldPassword, string NewPassword) {
            return base.Channel.ChangeAsync(SessionKey, CompanyId, LoginUserId, OldPassword, NewPassword);
        }
        
        public Rac.VOne.Web.Models.LoginProcessResult Login(string SessionKey, int CompanyId, int LoginUserId, string Password) {
            return base.Channel.Login(SessionKey, CompanyId, LoginUserId, Password);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.LoginProcessResult> LoginAsync(string SessionKey, int CompanyId, int LoginUserId, string Password) {
            return base.Channel.LoginAsync(SessionKey, CompanyId, LoginUserId, Password);
        }
        
        public Rac.VOne.Web.Models.LoginPasswordChangeResult Save(string SessionKey, int CompanyId, int LoginUserId, string Password) {
            return base.Channel.Save(SessionKey, CompanyId, LoginUserId, Password);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.LoginPasswordChangeResult> SaveAsync(string SessionKey, int CompanyId, int LoginUserId, string Password) {
            return base.Channel.SaveAsync(SessionKey, CompanyId, LoginUserId, Password);
        }
    }
}
