﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.BankAccountTypeMasterService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BankAccountTypeMasterService.IBankAccountTypeMaster")]
    public interface IBankAccountTypeMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBankAccountTypeMaster/GetItems", ReplyAction="http://tempuri.org/IBankAccountTypeMaster/GetItemsResponse")]
        Rac.VOne.Web.Models.BankAccountTypesResult GetItems(string SessionKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBankAccountTypeMaster/GetItems", ReplyAction="http://tempuri.org/IBankAccountTypeMaster/GetItemsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.BankAccountTypesResult> GetItemsAsync(string SessionKey);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBankAccountTypeMasterChannel : Rac.VOne.Client.Screen.BankAccountTypeMasterService.IBankAccountTypeMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BankAccountTypeMasterClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.BankAccountTypeMasterService.IBankAccountTypeMaster>, Rac.VOne.Client.Screen.BankAccountTypeMasterService.IBankAccountTypeMaster {
        
        public BankAccountTypeMasterClient() {
        }
        
        public BankAccountTypeMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BankAccountTypeMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BankAccountTypeMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BankAccountTypeMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.BankAccountTypesResult GetItems(string SessionKey) {
            return base.Channel.GetItems(SessionKey);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.BankAccountTypesResult> GetItemsAsync(string SessionKey) {
            return base.Channel.GetItemsAsync(SessionKey);
        }
    }
}
