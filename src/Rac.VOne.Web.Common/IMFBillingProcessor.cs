using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;


namespace Rac.VOne.Web.Common
{
    public interface IMFBillingProcessor
    {
        Task<IEnumerable<Billing>> SaveAsync(MFBillingSource source, CancellationToken token = default(CancellationToken));

        /// <summary><see cref="MFBilling"/>の取得</summary>
        /// <param name="source">
        /// <see cref="MFBillingSource.CompanyId"/> 会社ID
        /// <see cref="MFBillingSource.Ids"/> 請求データの ID配列
        /// <see cref="MFBillingSource.IsMatched"/> 消込済かどうか すべて取得する場合は指定しない
        /// </param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<MFBilling>> GetAsync(MFBillingSource source, CancellationToken token = default(CancellationToken));
    }
}
