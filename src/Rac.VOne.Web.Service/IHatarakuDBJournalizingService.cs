using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Service
{
    /// <summary>働くDB 消込結果連携用（請求データフラグ更新）</summary>
    [ServiceContract]
    public interface IHatarakuDBJournalizingService
    {
        [OperationContract]
        Task<JournalizingSummariesResult> GetSummaryAsync(string SessionKey, JournalizingOption option);
        [OperationContract]
        Task<HatarakuDBDataResult> ExtractAsync(string SessionKey, JournalizingOption option);
        [OperationContract]
        Task<CountResult> UpdateAsync(string SessionKey, JournalizingOption option);
        [OperationContract]
        Task<CountResult> CancelAsync(string SessionKey, JournalizingOption option);
    }
}
