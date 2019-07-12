using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IWebApiSettingMaster
    {
        [OperationContract]
        Task<CountResult> SaveAsync(string SessionKey, WebApiSetting setting);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int CompanyId, int? ApiTypeId);

        [OperationContract]
        Task<WebApiSettingResult> GetByIdAsync(string SessionKey, int CompanyId, int ApiTypeId);
    }
}
