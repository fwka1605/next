﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rac.VOne.Client.Screen.SectionWithDepartmentMasterService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SectionWithDepartmentMasterService.ISectionWithDepartmentMaster")]
    public interface ISectionWithDepartmentMaster {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/Save", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/SaveResponse")]
        Rac.VOne.Web.Models.SectionWithDepartmentResult Save(string sessionKey, Rac.VOne.Web.Models.SectionWithDepartment[] AddList, Rac.VOne.Web.Models.SectionWithDepartment[] DeleteList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/Save", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/SaveResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.SectionWithDepartmentResult> SaveAsync(string sessionKey, Rac.VOne.Web.Models.SectionWithDepartment[] AddList, Rac.VOne.Web.Models.SectionWithDepartment[] DeleteList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/GetBySection", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/GetBySectionResponse")]
        Rac.VOne.Web.Models.SectionWithDepartmentsResult GetBySection(string SessionKey, int CompanyId, int SectionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/GetBySection", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/GetBySectionResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.SectionWithDepartmentsResult> GetBySectionAsync(string SessionKey, int CompanyId, int SectionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/GetByDepartment", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/GetByDepartmentResponse")]
        Rac.VOne.Web.Models.SectionWithDepartmentResult GetByDepartment(string sessionKey, int CompanyId, int DepartmentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/GetByDepartment", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/GetByDepartmentResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.SectionWithDepartmentResult> GetByDepartmentAsync(string sessionKey, int CompanyId, int DepartmentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/GetItems", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/GetItemsResponse")]
        Rac.VOne.Web.Models.SectionWithDepartmentsResult GetItems(string SessionKey, int CompanyId, Rac.VOne.Web.Models.SectionWithDepartmentSearch SectionWithDepartmentSearch);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/GetItems", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/GetItemsResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.SectionWithDepartmentsResult> GetItemsAsync(string SessionKey, int CompanyId, Rac.VOne.Web.Models.SectionWithDepartmentSearch SectionWithDepartmentSearch);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/Import", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/ImportResponse")]
        Rac.VOne.Web.Models.ImportResult Import(string sessionKey, Rac.VOne.Web.Models.SectionWithDepartment[] insertList, Rac.VOne.Web.Models.SectionWithDepartment[] updateList, Rac.VOne.Web.Models.SectionWithDepartment[] deleteList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/Import", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/ImportResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ImportResult> ImportAsync(string sessionKey, Rac.VOne.Web.Models.SectionWithDepartment[] insertList, Rac.VOne.Web.Models.SectionWithDepartment[] updateList, Rac.VOne.Web.Models.SectionWithDepartment[] deleteList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/ExistSection", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/ExistSectionResponse")]
        Rac.VOne.Web.Models.ExistResult ExistSection(string SessionKey, int SectionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/ExistSection", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/ExistSectionResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExistResult> ExistSectionAsync(string SessionKey, int SectionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/ExistDepartment", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/ExistDepartmentResponse")]
        Rac.VOne.Web.Models.ExistResult ExistDepartment(string SessionKey, int DepartmentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISectionWithDepartmentMaster/ExistDepartment", ReplyAction="http://tempuri.org/ISectionWithDepartmentMaster/ExistDepartmentResponse")]
        System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExistResult> ExistDepartmentAsync(string SessionKey, int DepartmentId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISectionWithDepartmentMasterChannel : Rac.VOne.Client.Screen.SectionWithDepartmentMasterService.ISectionWithDepartmentMaster, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SectionWithDepartmentMasterClient : System.ServiceModel.ClientBase<Rac.VOne.Client.Screen.SectionWithDepartmentMasterService.ISectionWithDepartmentMaster>, Rac.VOne.Client.Screen.SectionWithDepartmentMasterService.ISectionWithDepartmentMaster {
        
        public SectionWithDepartmentMasterClient() {
        }
        
        public SectionWithDepartmentMasterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SectionWithDepartmentMasterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SectionWithDepartmentMasterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SectionWithDepartmentMasterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Rac.VOne.Web.Models.SectionWithDepartmentResult Save(string sessionKey, Rac.VOne.Web.Models.SectionWithDepartment[] AddList, Rac.VOne.Web.Models.SectionWithDepartment[] DeleteList) {
            return base.Channel.Save(sessionKey, AddList, DeleteList);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.SectionWithDepartmentResult> SaveAsync(string sessionKey, Rac.VOne.Web.Models.SectionWithDepartment[] AddList, Rac.VOne.Web.Models.SectionWithDepartment[] DeleteList) {
            return base.Channel.SaveAsync(sessionKey, AddList, DeleteList);
        }
        
        public Rac.VOne.Web.Models.SectionWithDepartmentsResult GetBySection(string SessionKey, int CompanyId, int SectionId) {
            return base.Channel.GetBySection(SessionKey, CompanyId, SectionId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.SectionWithDepartmentsResult> GetBySectionAsync(string SessionKey, int CompanyId, int SectionId) {
            return base.Channel.GetBySectionAsync(SessionKey, CompanyId, SectionId);
        }
        
        public Rac.VOne.Web.Models.SectionWithDepartmentResult GetByDepartment(string sessionKey, int CompanyId, int DepartmentId) {
            return base.Channel.GetByDepartment(sessionKey, CompanyId, DepartmentId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.SectionWithDepartmentResult> GetByDepartmentAsync(string sessionKey, int CompanyId, int DepartmentId) {
            return base.Channel.GetByDepartmentAsync(sessionKey, CompanyId, DepartmentId);
        }
        
        public Rac.VOne.Web.Models.SectionWithDepartmentsResult GetItems(string SessionKey, int CompanyId, Rac.VOne.Web.Models.SectionWithDepartmentSearch SectionWithDepartmentSearch) {
            return base.Channel.GetItems(SessionKey, CompanyId, SectionWithDepartmentSearch);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.SectionWithDepartmentsResult> GetItemsAsync(string SessionKey, int CompanyId, Rac.VOne.Web.Models.SectionWithDepartmentSearch SectionWithDepartmentSearch) {
            return base.Channel.GetItemsAsync(SessionKey, CompanyId, SectionWithDepartmentSearch);
        }
        
        public Rac.VOne.Web.Models.ImportResult Import(string sessionKey, Rac.VOne.Web.Models.SectionWithDepartment[] insertList, Rac.VOne.Web.Models.SectionWithDepartment[] updateList, Rac.VOne.Web.Models.SectionWithDepartment[] deleteList) {
            return base.Channel.Import(sessionKey, insertList, updateList, deleteList);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ImportResult> ImportAsync(string sessionKey, Rac.VOne.Web.Models.SectionWithDepartment[] insertList, Rac.VOne.Web.Models.SectionWithDepartment[] updateList, Rac.VOne.Web.Models.SectionWithDepartment[] deleteList) {
            return base.Channel.ImportAsync(sessionKey, insertList, updateList, deleteList);
        }
        
        public Rac.VOne.Web.Models.ExistResult ExistSection(string SessionKey, int SectionId) {
            return base.Channel.ExistSection(SessionKey, SectionId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExistResult> ExistSectionAsync(string SessionKey, int SectionId) {
            return base.Channel.ExistSectionAsync(SessionKey, SectionId);
        }
        
        public Rac.VOne.Web.Models.ExistResult ExistDepartment(string SessionKey, int DepartmentId) {
            return base.Channel.ExistDepartment(SessionKey, DepartmentId);
        }
        
        public System.Threading.Tasks.Task<Rac.VOne.Web.Models.ExistResult> ExistDepartmentAsync(string SessionKey, int DepartmentId) {
            return base.Channel.ExistDepartmentAsync(SessionKey, DepartmentId);
        }
    }
}