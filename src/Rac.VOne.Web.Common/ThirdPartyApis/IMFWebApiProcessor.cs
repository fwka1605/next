using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MFModels = Rac.VOne.Web.Models.MFModels;

namespace Rac.VOne.Web.Common.ThirdPartyApis
{
    public interface IMFWebApiProcessor
    {
        Task<int> SaveSettingAsync(MFWebApiOption option, CancellationToken token = default(CancellationToken));

        Task<bool> ValidateToken(MFWebApiOption option, CancellationToken token = default(CancellationToken));

        Task<MFModels.Office> GetOfficesAsync(MFWebApiOption option, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<MFModels.billing>> GetBillingsAsync(MFWebApiOption option, CancellationToken token = default(CancellationToken));

        Task<bool> PatchBillings(MFWebApiOption option, CancellationToken token = default(CancellationToken));

        Task<MFModels.partner> GetPartnersAsync(MFWebApiOption option, CancellationToken token = default(CancellationToken));
    }
}
