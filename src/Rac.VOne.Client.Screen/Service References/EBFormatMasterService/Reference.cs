﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.EBFormatMasterService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EBFormatMasterService.IEBFormatMaster")]
    public interface IEBFormatMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEBFormatMaster/GetItems", ReplyAction="http://tempuri.org/IEBFormatMaster/GetItemsResponse")]
        Rac.VOne.Web.Models.EBFormatsResult GetItems(string sessionKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEBFormatMaster/GetItems", ReplyAction="http://tempuri.org/IEBFormatMaster/GetItemsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.EBFormatsResult> GetItemsAsync(string sessionKey);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEBFormatMasterChannel : Rac.VOne.Client.Screen.EBFormatMasterService.IEBFormatMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EBFormatMasterClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.EBFormatMasterService.IEBFormatMaster>, Rac.VOne.Client.Screen.EBFormatMasterService.IEBFormatMaster {
        
        public EBFormatMasterClient() {
        }
        
        public EBFormatMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EBFormatMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EBFormatMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EBFormatMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.EBFormatsResult GetItems(string sessionKey) {
            return base.Channel.GetItems(sessionKey);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.EBFormatsResult> GetItemsAsync(string sessionKey) {
            return base.Channel.GetItemsAsync(sessionKey);
        }
    }
}
