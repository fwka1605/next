﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.DBService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DBService.IDBService")]
    public interface IDBService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDBService/GetClientKey", ReplyAction="http://tempuri.org/IDBService/GetClientKeyResponse")]
        Rac.VOne.Web.Models.ClientKeyResult GetClientKey(string SessionKey, string ProgramId, string ClientName, string CompanyCode, string LoginUserCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDBService/GetClientKey", ReplyAction="http://tempuri.org/IDBService/GetClientKeyResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ClientKeyResult> GetClientKeyAsync(string SessionKey, string ProgramId, string ClientName, string CompanyCode, string LoginUserCode);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDBServiceChannel : Rac.VOne.Client.Screen.DBService.IDBService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DBServiceClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.DBService.IDBService>, Rac.VOne.Client.Screen.DBService.IDBService {
        
        public DBServiceClient() {
        }
        
        public DBServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DBServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DBServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DBServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.ClientKeyResult GetClientKey(string SessionKey, string ProgramId, string ClientName, string CompanyCode, string LoginUserCode) {
            return base.Channel.GetClientKey(SessionKey, ProgramId, ClientName, CompanyCode, LoginUserCode);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ClientKeyResult> GetClientKeyAsync(string SessionKey, string ProgramId, string ClientName, string CompanyCode, string LoginUserCode) {
            return base.Channel.GetClientKeyAsync(SessionKey, ProgramId, ClientName, CompanyCode, LoginUserCode);
        }
    }
}
