using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    [ServiceContract]
    public interface IPeriodicBillingService
    {
        [OperationContract] Task<BillingsResult> CreateAsync(string sessionKey, IEnumerable<PeriodicBillingSetting> settings);
    }
}
