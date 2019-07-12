﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.KanaHistoryCustomerMasterService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="KanaHistoryCustomerMasterService.IKanaHistoryCustomerMaster")]
    public interface IKanaHistoryCustomerMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKanaHistoryCustomerMaster/ExistCustomer", ReplyAction="http://tempuri.org/IKanaHistoryCustomerMaster/ExistCustomerResponse")]
        Rac.VOne.Web.Models.ExistResult ExistCustomer(string SessionKey, int CustomerId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKanaHistoryCustomerMaster/ExistCustomer", ReplyAction="http://tempuri.org/IKanaHistoryCustomerMaster/ExistCustomerResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKanaHistoryCustomerMaster/GetList", ReplyAction="http://tempuri.org/IKanaHistoryCustomerMaster/GetListResponse")]
        Rac.VOne.Web.Models.KanaHistoryCustomersResult GetList(string SessionKey, int CompanyId, string PayerName, string CustomerCodeFrom, string CustomerCodeTo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKanaHistoryCustomerMaster/GetList", ReplyAction="http://tempuri.org/IKanaHistoryCustomerMaster/GetListResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.KanaHistoryCustomersResult> GetListAsync(string SessionKey, int CompanyId, string PayerName, string CustomerCodeFrom, string CustomerCodeTo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKanaHistoryCustomerMaster/Delete", ReplyAction="http://tempuri.org/IKanaHistoryCustomerMaster/DeleteResponse")]
        Rac.VOne.Web.Models.CountResult Delete(string SessionKey, int CompanyId, string PayerName, string SourceBankName, string SourceBranchName, int CustomerId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKanaHistoryCustomerMaster/Delete", ReplyAction="http://tempuri.org/IKanaHistoryCustomerMaster/DeleteResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteAsync(string SessionKey, int CompanyId, string PayerName, string SourceBankName, string SourceBranchName, int CustomerId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKanaHistoryCustomerMaster/Import", ReplyAction="http://tempuri.org/IKanaHistoryCustomerMaster/ImportResponse")]
        Rac.VOne.Web.Models.ImportResult Import(string SessionKey, Rac.VOne.Web.Models.KanaHistoryCustomer[] InsertList, Rac.VOne.Web.Models.KanaHistoryCustomer[] UpdateList, Rac.VOne.Web.Models.KanaHistoryCustomer[] DeleteList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IKanaHistoryCustomerMaster/Import", ReplyAction="http://tempuri.org/IKanaHistoryCustomerMaster/ImportResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ImportResult> ImportAsync(string SessionKey, Rac.VOne.Web.Models.KanaHistoryCustomer[] InsertList, Rac.VOne.Web.Models.KanaHistoryCustomer[] UpdateList, Rac.VOne.Web.Models.KanaHistoryCustomer[] DeleteList);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IKanaHistoryCustomerMasterChannel : Rac.VOne.Client.Screen.KanaHistoryCustomerMasterService.IKanaHistoryCustomerMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class KanaHistoryCustomerMasterClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.KanaHistoryCustomerMasterService.IKanaHistoryCustomerMaster>, Rac.VOne.Client.Screen.KanaHistoryCustomerMasterService.IKanaHistoryCustomerMaster {
        
        public KanaHistoryCustomerMasterClient() {
        }
        
        public KanaHistoryCustomerMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public KanaHistoryCustomerMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KanaHistoryCustomerMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KanaHistoryCustomerMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.ExistResult ExistCustomer(string SessionKey, int CustomerId) {
            return base.Channel.ExistCustomer(SessionKey, CustomerId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExistResult> ExistCustomerAsync(string SessionKey, int CustomerId) {
            return base.Channel.ExistCustomerAsync(SessionKey, CustomerId);
        }
        
        public Rac.VOne.Web.Models.KanaHistoryCustomersResult GetList(string SessionKey, int CompanyId, string PayerName, string CustomerCodeFrom, string CustomerCodeTo) {
            return base.Channel.GetList(SessionKey, CompanyId, PayerName, CustomerCodeFrom, CustomerCodeTo);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.KanaHistoryCustomersResult> GetListAsync(string SessionKey, int CompanyId, string PayerName, string CustomerCodeFrom, string CustomerCodeTo) {
            return base.Channel.GetListAsync(SessionKey, CompanyId, PayerName, CustomerCodeFrom, CustomerCodeTo);
        }
        
        public Rac.VOne.Web.Models.CountResult Delete(string SessionKey, int CompanyId, string PayerName, string SourceBankName, string SourceBranchName, int CustomerId) {
            return base.Channel.Delete(SessionKey, CompanyId, PayerName, SourceBankName, SourceBranchName, CustomerId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.CountResult> DeleteAsync(string SessionKey, int CompanyId, string PayerName, string SourceBankName, string SourceBranchName, int CustomerId) {
            return base.Channel.DeleteAsync(SessionKey, CompanyId, PayerName, SourceBankName, SourceBranchName, CustomerId);
        }
        
        public Rac.VOne.Web.Models.ImportResult Import(string SessionKey, Rac.VOne.Web.Models.KanaHistoryCustomer[] InsertList, Rac.VOne.Web.Models.KanaHistoryCustomer[] UpdateList, Rac.VOne.Web.Models.KanaHistoryCustomer[] DeleteList) {
            return base.Channel.Import(SessionKey, InsertList, UpdateList, DeleteList);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ImportResult> ImportAsync(string SessionKey, Rac.VOne.Web.Models.KanaHistoryCustomer[] InsertList, Rac.VOne.Web.Models.KanaHistoryCustomer[] UpdateList, Rac.VOne.Web.Models.KanaHistoryCustomer[] DeleteList) {
            return base.Channel.ImportAsync(SessionKey, InsertList, UpdateList, DeleteList);
        }
    }
}
