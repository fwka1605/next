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
    public interface IBankAccountTypeMaster
    {
        [OperationContract] Task<BankAccountTypesResult> GetItemsAsync(string SessionKey);
    }
}
