using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    /// <summary>
    /// 入金予定入力系
    /// 画面からの入力
    /// インポート処理
    /// </summary>
    public interface IBillingScheduledPaymentProcessor
    {

        Task<IEnumerable<Billing>> SaveAsync(IEnumerable<Billing> billings, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<Billing>> GetAsync(BillingScheduledPaymentImportSource source, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<ScheduledPaymentImport>> ImportAsync(BillingScheduledPaymentImportSource source, CancellationToken token = default(CancellationToken));

    }
}
