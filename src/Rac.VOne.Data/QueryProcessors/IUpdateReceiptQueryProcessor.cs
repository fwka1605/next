using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Data.QueryProcessors
{
    public interface IUpdateReceiptQueryProcessor
    {
        Task<Receipt> UpdateOriginalRemainAsync(long Id, int UpdateBy, DateTime UpdateAt, CancellationToken token = default(CancellationToken));
        Task<Receipt> UpdateCancelAdvancedReceivedAsync(long receiptId, long originalReceiptId, int updateBy, DateTime updateAt, CancellationToken token = default(CancellationToken));

        /// <summary>対象外 更新処理 対象外処理の実行 および 対象外の戻し処理を行う
        /// </summary>
        /// <param name="receipt">
        /// <see cref="Receipt.ExcludeFlag"/>の値によって動作が異なる
        /// 対象外にする/戻す場合に、<see cref="Receipt.ExcludeAmount"/> に対象となる 値を設定する
        /// </param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<int> UpdateExcludeAmountAsync(Receipt receipt, CancellationToken token = default(CancellationToken));

        Task<ReceiptInput> UpdateReceiptInputAsync(ReceiptInput receipt, CancellationToken token = default(CancellationToken));
        Task<ReceiptInput> UpdateCustomerIdAsync(ReceiptInput receipt, CancellationToken token = default(CancellationToken));
        Task<int> OmitAsync(int doDelete, int loginUserId, Transaction item, CancellationToken token  =default(CancellationToken));

        Task<Receipt> UpdateReceiptSectionAsync(int DestinationSectionId, int LoginUserId, long Id, CancellationToken token  =default(CancellationToken));
        Task<Receipt> UpdateReceiptAmountAsync(decimal DestinationAmount, int LoginUserId, long Id, CancellationToken token = default(CancellationToken));
    }
}
