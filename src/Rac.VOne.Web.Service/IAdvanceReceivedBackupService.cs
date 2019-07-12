using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    [ServiceContract]
    public interface IAdvanceReceivedBackupService
    {
        [OperationContract]
        Task<AdvanceReceivedBackupResult> GetByOriginalReceiptIdAsync(string SessionKey, long OriginalReceiptId);

        [OperationContract] Task<AdvanceReceivedBackupsResult> GetByIdsAsync(string SessionKey, long[] ids);
    }
}
