﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.LogDataService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LogDataService.ILogDataService")]
    public interface ILogDataService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogDataService/GetItems", ReplyAction="http://tempuri.org/ILogDataService/GetItemsResponse")]
        Rac.VOne.Web.Models.LogDataResult GetItems(string SessionKey, int CompanyId, System.Nullable<System.DateTime> LoggedAtFrom, System.Nullable<System.DateTime> LoggedAtTo, string LoginUserCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogDataService/GetItems", ReplyAction="http://tempuri.org/ILogDataService/GetItemsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.LogDataResult> GetItemsAsync(string SessionKey, int CompanyId, System.Nullable<System.DateTime> LoggedAtFrom, System.Nullable<System.DateTime> LoggedAtTo, string LoginUserCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogDataService/Log", ReplyAction="http://tempuri.org/ILogDataService/LogResponse")]
        Rac.VOne.Web.Models.CountResult Log(string SessionKey, Rac.VOne.Web.Models.LogData LogData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogDataService/Log", ReplyAction="http://tempuri.org/ILogDataService/LogResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> LogAsync(string SessionKey, Rac.VOne.Web.Models.LogData LogData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogDataService/GetStats", ReplyAction="http://tempuri.org/ILogDataService/GetStatsResponse")]
        Rac.VOne.Web.Models.LogDatasResult GetStats(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogDataService/GetStats", ReplyAction="http://tempuri.org/ILogDataService/GetStatsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.LogDatasResult> GetStatsAsync(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogDataService/DeleteAll", ReplyAction="http://tempuri.org/ILogDataService/DeleteAllResponse")]
        Rac.VOne.Web.Models.CountResult DeleteAll(string SessionKey, int CompanyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILogDataService/DeleteAll", ReplyAction="http://tempuri.org/ILogDataService/DeleteAllResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteAllAsync(string SessionKey, int CompanyId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILogDataServiceChannel : Rac.VOne.Client.Screen.LogDataService.ILogDataService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LogDataServiceClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.LogDataService.ILogDataService>, Rac.VOne.Client.Screen.LogDataService.ILogDataService {
        
        public LogDataServiceClient() {
        }
        
        public LogDataServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LogDataServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LogDataServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LogDataServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.LogDataResult GetItems(string SessionKey, int CompanyId, System.Nullable<System.DateTime> LoggedAtFrom, System.Nullable<System.DateTime> LoggedAtTo, string LoginUserCode) {
            return base.Channel.GetItems(SessionKey, CompanyId, LoggedAtFrom, LoggedAtTo, LoginUserCode);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.LogDataResult> GetItemsAsync(string SessionKey, int CompanyId, System.Nullable<System.DateTime> LoggedAtFrom, System.Nullable<System.DateTime> LoggedAtTo, string LoginUserCode) {
            return base.Channel.GetItemsAsync(SessionKey, CompanyId, LoggedAtFrom, LoggedAtTo, LoginUserCode);
        }
        
        public Rac.VOne.Web.Models.CountResult Log(string SessionKey, Rac.VOne.Web.Models.LogData LogData) {
            return base.Channel.Log(SessionKey, LogData);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> LogAsync(string SessionKey, Rac.VOne.Web.Models.LogData LogData) {
            return base.Channel.LogAsync(SessionKey, LogData);
        }
        
        public Rac.VOne.Web.Models.LogDatasResult GetStats(string SessionKey, int CompanyId) {
            return base.Channel.GetStats(SessionKey, CompanyId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.LogDatasResult> GetStatsAsync(string SessionKey, int CompanyId) {
            return base.Channel.GetStatsAsync(SessionKey, CompanyId);
        }
        
        public Rac.VOne.Web.Models.CountResult DeleteAll(string SessionKey, int CompanyId) {
            return base.Channel.DeleteAll(SessionKey, CompanyId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteAllAsync(string SessionKey, int CompanyId) {
            return base.Channel.DeleteAllAsync(SessionKey, CompanyId);
        }
    }
}