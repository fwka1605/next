using Rac.VOne.Web.Models;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IReportSettingMaster
    {
        [OperationContract]
        Task<ReportSettingsResult> GetItemsAsync(string SessionKey, int CompanyId, string ReportId);

        [OperationContract]
        Task<ReportSettingsResult> SaveAsync(string SessionKey, ReportSetting[] ReportSetting);
    }
}
