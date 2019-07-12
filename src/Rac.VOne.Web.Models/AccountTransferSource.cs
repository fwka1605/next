using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    /// <summary>口座振替 結果反映用</summary>
    [DataContract] public class AccountTransferSource
    {
        // 請求書情報

        /// <summary>照合処理で見つかった請求データ(同一得意先)。照合不能時はnull。</summary>
        [DataMember] public IEnumerable<Billing> Billings { get; set; }

        // 口座振替情報(ファイル記載情報)

        /// <summary>振替結果コード</summary>
        [DataMember] public int TransferResultCode { get; set; }

        /// <summary>引落金額(振替不能時は振替依頼時の金額)</summary>
        [DataMember] public decimal TransferAmount { get; set; }

        [DataMember] public string TransferBankName { get; set; }
        [DataMember] public string TransferBranchName { get; set; }
        [DataMember] public string TransferCustomerCode { get; set; }
        [DataMember] public string TransferAccountName { get; set; }

        /// <summary>振替不可時に、口座振替依頼データ作成を 再度可能とする初期化処理を無視する</summary>
        [DataMember] public bool IgnoreInitialization { get; set; }
        /// <summary>郵貯 の 再振替日</summary>
        [DataMember] public DateTime NewDueAt { get; set; }

        public AccountTransferSource() { }

        /// <summary>constructor</summary>
        /// <param name="billings">照合処理で見つかった請求データ(同一得意先)。照合不能時はnull</param>
        /// <param name="transferResultCode">振替結果コード</param>
        /// <param name="transferAmount">引落金額(振替不能時は振替依頼時の金額)</param>
        public AccountTransferSource(IEnumerable<Billing> billings, int transferResultCode, decimal transferAmount)
        {
            Billings            = billings;
            TransferResultCode  = transferResultCode;
            TransferAmount      = transferAmount;
        }

        public IEnumerable<string> GetInvalidLogs()
        {
            if (!(Billings?.Any() ?? false)) yield return $"{GetRecordInfo()}: 請求データを参照できません";
            if (TransferResultCode != 0) yield return $"{GetRecordInfo()}: 振替不能 （{TransferResultCode}）";
        }

        private string GetRecordInfo() => $"{TransferBankName}/{TransferBranchName}/{TransferCustomerCode}/{TransferAccountName}";

    }
}
