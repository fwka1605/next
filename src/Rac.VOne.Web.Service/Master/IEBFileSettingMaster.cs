using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service.Master
{
    [ServiceContract]
    public interface IEBFileSettingMaster
    {
        [OperationContract]
        Task<EBFileSettingsResult> GetItemsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<EBFileSettingResult> GetItemAsync(string SessionKey, int Id);


        [OperationContract]
        Task<EBFileSettingResult> SaveAsync(string SessionKey, EBFileSetting setting);

        [OperationContract]
        Task<CountResult> DeleteAsync(string SessionKey, int Id);

        [OperationContract]
        Task<CountResult> UpdateIsUseableAsync(string SessionKey, int CompanyId, int LoginUserId, int[] ids);

    }
}
