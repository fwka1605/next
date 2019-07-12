using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IExportFieldSettingMaster
    {
        [OperationContract]
        Task<ExportFieldSettingsResult> GetItemsByExportFileTypeAsync(string SessionKey, int CompanyId, int ExportFileType);

        [OperationContract]
        Task<ExportFieldSettingsResult> SaveAsync(string SessionKey, ExportFieldSetting[] ExportFieldSetting);
    }
}
