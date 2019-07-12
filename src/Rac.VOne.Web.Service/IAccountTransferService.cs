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
    public interface IAccountTransferService
    {
        [OperationContract]
        Task<AccountTransferLogsResult> GetAsync(string SessionKey, int CompanyId);

        [OperationContract]
        Task<AccountTransferDetailsResult> ExtractAsync(string SessionKey, AccountTransferSearch SearchOption);

        [OperationContract]
        Task<CountResult> CancelAsync(string SessionKey, long[] AccountTransferLogIds, int LoginUserId);

        [OperationContract]
        Task<AccountTransferDetailsResult> SaveAsync(string SessionKey, AccountTransferDetail[] AccountTransferDetails, bool Aggregate);
    }
}
