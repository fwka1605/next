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
    [ServiceContract] public interface IPeriodicBillingSettingMaster
    {

        [OperationContract] Task<PeriodicBillingSettingsResult> GetItemsAsync(string sessionKey, PeriodicBillingSettingSearch option);

        [OperationContract] Task<PeriodicBillingSettingResult> SaveAsync(string sessionKey, PeriodicBillingSetting setting);

        [OperationContract] Task<CountResult> DeleteAsync(string sessionKey, long id);
    }
}
