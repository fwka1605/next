﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.SettingMasterService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SettingMasterService.ISettingMaster")]
    public interface ISettingMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISettingMaster/GetItems", ReplyAction="http://tempuri.org/ISettingMaster/GetItemsResponse")]
        Rac.VOne.Web.Models.SettingsResult GetItems(string SessionKey, string[] ItemId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISettingMaster/GetItems", ReplyAction="http://tempuri.org/ISettingMaster/GetItemsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.SettingsResult> GetItemsAsync(string SessionKey, string[] ItemId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISettingMasterChannel : Rac.VOne.Client.Screen.SettingMasterService.ISettingMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SettingMasterClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.SettingMasterService.ISettingMaster>, Rac.VOne.Client.Screen.SettingMasterService.ISettingMaster {
        
        public SettingMasterClient() {
        }
        
        public SettingMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SettingMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SettingMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SettingMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.SettingsResult GetItems(string SessionKey, string[] ItemId) {
            return base.Channel.GetItems(SessionKey, ItemId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.SettingsResult> GetItemsAsync(string SessionKey, string[] ItemId) {
            return base.Channel.GetItemsAsync(SessionKey, ItemId);
        }
    }
}