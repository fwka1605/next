using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Rac.VOne.Web.Models;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    [ServiceContract]
    public interface ILogDataService
    {
        [OperationContract]
        Task<LogDataResult> GetItemsAsync(string SessionKey, int CompanyId, DateTime? LoggedAtFrom, DateTime? LoggedAtTo, string LoginUserCode);

        [OperationContract]
        Task<CountResult> LogAsync(string SessionKey, LogData LogData);

        [OperationContract]
        Task<LogDatasResult> GetStatsAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<CountResult> DeleteAllAsync(string SessionKey, int CompanyId);

    }
}
