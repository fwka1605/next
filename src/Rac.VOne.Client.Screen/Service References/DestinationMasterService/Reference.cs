﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.DestinationMasterService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DestinationMasterService.IDestinationMaster")]
    public interface IDestinationMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDestinationMaster/GetItems", ReplyAction="http://tempuri.org/IDestinationMaster/GetItemsResponse")]
        Rac.VOne.Web.Models.DestinationsResult GetItems(string SessionKey, Rac.VOne.Web.Models.DestinationSearch option);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDestinationMaster/GetItems", ReplyAction="http://tempuri.org/IDestinationMaster/GetItemsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.DestinationsResult> GetItemsAsync(string SessionKey, Rac.VOne.Web.Models.DestinationSearch option);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDestinationMaster/Save", ReplyAction="http://tempuri.org/IDestinationMaster/SaveResponse")]
        Rac.VOne.Web.Models.DestinationResult Save(string SessionKey, Rac.VOne.Web.Models.Destination Destination);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDestinationMaster/Save", ReplyAction="http://tempuri.org/IDestinationMaster/SaveResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.DestinationResult> SaveAsync(string SessionKey, Rac.VOne.Web.Models.Destination Destination);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDestinationMaster/Delete", ReplyAction="http://tempuri.org/IDestinationMaster/DeleteResponse")]
        Rac.VOne.Web.Models.CountResult Delete(string SessionKey, int Id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDestinationMaster/Delete", ReplyAction="http://tempuri.org/IDestinationMaster/DeleteResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteAsync(string SessionKey, int Id);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDestinationMasterChannel : Rac.VOne.Client.Screen.DestinationMasterService.IDestinationMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DestinationMasterClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.DestinationMasterService.IDestinationMaster>, Rac.VOne.Client.Screen.DestinationMasterService.IDestinationMaster {
        
        public DestinationMasterClient() {
        }
        
        public DestinationMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DestinationMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DestinationMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DestinationMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.DestinationsResult GetItems(string SessionKey, Rac.VOne.Web.Models.DestinationSearch option) {
            return base.Channel.GetItems(SessionKey, option);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.DestinationsResult> GetItemsAsync(string SessionKey, Rac.VOne.Web.Models.DestinationSearch option) {
            return base.Channel.GetItemsAsync(SessionKey, option);
        }
        
        public Rac.VOne.Web.Models.DestinationResult Save(string SessionKey, Rac.VOne.Web.Models.Destination Destination) {
            return base.Channel.Save(SessionKey, Destination);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.DestinationResult> SaveAsync(string SessionKey, Rac.VOne.Web.Models.Destination Destination) {
            return base.Channel.SaveAsync(SessionKey, Destination);
        }
        
        public Rac.VOne.Web.Models.CountResult Delete(string SessionKey, int Id) {
            return base.Channel.Delete(SessionKey, Id);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteAsync(string SessionKey, int Id) {
            return base.Channel.DeleteAsync(SessionKey, Id);
        }
    }
}