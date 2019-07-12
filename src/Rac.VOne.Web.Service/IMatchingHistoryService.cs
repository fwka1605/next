using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using System.ServiceModel;

namespace Rac.VOne.Web.Service
{
    [ServiceContract]
    public interface IMatchingHistoryService
    {
        [OperationContract]
        Task< MatchingHistorysResult> GetAsync(string SessionKey, MatchingHistorySearch MatchingHistorySearch, string connectionId);

        [OperationContract]
        Task<MatchingHistoryResult> SaveOutputAtAsync(string SessionKey, long[] MatchingHeaderId);
    }
}
