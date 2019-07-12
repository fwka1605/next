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
    public interface IImporterSettingService
    {
        [OperationContract]
        Task<ImporterSettingsResult> GetHeaderAsync(string SessionKey, int CompanyId, int FormatId);

        [OperationContract]
        Task<ImporterSettingResult> GetHeaderByCodeAsync(string SessionKey, int CompanyId, int FormatId, string Code);

        [OperationContract]
        Task<ImporterSettingAndDetailResult> SaveAsync(string SessionKey, ImporterSetting ImpSetting, ImporterSettingDetail[] ImpSettingDetail);

        [OperationContract]
        Task<ImporterSettingDetailsResult> GetDetailByCodeAsync(string SessionKey, int CompanyId, int FormatId, string Code);
        [OperationContract]
        Task<ImporterSettingDetailsResult> GetDetailByIdAsync(string SessionKey, int ImporterSettingId);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int ImporterSettingId);
    }
}
