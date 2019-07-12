using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;


namespace Rac.VOne.Data.QueryProcessors
{
    public interface ICustomerLedgerQueryProcessor
    {

        Task<IEnumerable<CustomerLedger>> GetAsync(CustomerLedgerSearch searchOption, CancellationToken token, IProgressNotifier notifier);

        /// <summary>消込記号用　消込ヘッダー取得</summary>
        /// <param name="searchOption"></param>
        /// <returns>債権代表者ID, 消込ヘッダーID</returns>
        Task<IEnumerable<KeyValuePair<int, long>>> GetKeysAsync(CustomerLedgerSearch searchOption, CancellationToken token, IProgressNotifier notifier);

        /// <summary>繰越用 残高取得</summary>
        /// <param name="searchOption"></param>
        /// <returns>債権代表者ID, 繰越残高</returns>
        Task<IEnumerable<KeyValuePair<int, decimal>>> GetBalancesAsync(CustomerLedgerSearch searchOption, CancellationToken token, IProgressNotifier notifier);
    }
}
