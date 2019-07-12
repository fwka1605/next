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
    public interface IEBExcludeAccountSettingMaster
    {
        [OperationContract]
        Task<EBExcludeAccountSettingListResult> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<EBExcludeAccountSettingResult> SaveAsync(string SessionKey, EBExcludeAccountSetting ebExcludeAccountSetting);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, EBExcludeAccountSetting ebExcludeAccountSetting);
    }
}
