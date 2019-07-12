using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    [ServiceContract]
    public interface IInvoiceSettingService
    {
        [OperationContract]
        Task<InvoiceCommonSettingResult> GetInvoiceCommonSettingAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<InvoiceCommonSettingResult> SaveInvoiceCommonSettingAsync(string SessionKey, InvoiceCommonSetting InvoiceCommonSetting);

        [OperationContract]
        Task<CategoriesResult> UpdateCollectCategoryAsync(string SessionKey, IEnumerable<Category> CollectCategories);

        [OperationContract]
        Task<InvoiceNumberHistoriesResult> GetInvoiceNumberHistoriesAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<InvoiceNumberHistoryResult> SaveInvoiceNumberHistoryAsync(string SessionKey, InvoiceNumberHistory InvoiceNumberHistory);

        [OperationContract]
        Task<CountResult> DeleteInvoiceNumberHistoriesAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<InvoiceNumberSettingResult> GetInvoiceNumberSettingAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<InvoiceNumberSettingResult> SaveInvoiceNumberSettingAsync(string SessionKey, InvoiceNumberSetting InvoiceNumberSetting);

        [OperationContract]
        Task<ExistResult> ExistCollectCategoryAtTemplateAsync(string SessionKey, int CollectCategoryId);

        [OperationContract]
        Task<InvoiceTemplateSettingResult> GetInvoiceTemplateSettingByCodeAsync(string SessionKey, int CompanyId, string Code);

        [OperationContract]
        Task<InvoiceTemplateSettingsResult> GetInvoiceTemplateSettingsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<InvoiceTemplateSettingResult> SaveInvoiceTemplateSettingAsync(string SessionKey, InvoiceTemplateSetting InvoiceTemplateSetting);

        [OperationContract]
        Task<CountResult> DeleteInvoiceTemplateSettingAsync(string SessionKey, int Id);

    }
}
