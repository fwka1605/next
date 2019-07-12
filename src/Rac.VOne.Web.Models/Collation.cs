using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Collation
    {
        [DataMember] public bool Checked { get; set; }
        [DataMember] public int CompnayId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int ParentCustomerId { get; set; }
        [DataMember] public int PaymentAgencyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public string PaymentAgencyCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string PaymentAgencyName { get; set; }
        [DataMember] public int BillingCount { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public string PayerName { get; set; }
        [DataMember] public int ReceiptCount { get; set; }
        [DataMember] public decimal ReceiptAmount { get; set; }
        [DataMember] public int AdvanceReceivedCount { get; set; }
        [DataMember] public decimal BankTransferFee { get; set; }
        [DataMember] public decimal TaxDifference { get; set; }

        /// <summary>
        /// 手数料負担区分 0 : 相手先, 1 : 自社負担(マスター/誤差範囲考慮)
        /// 得意先マスター<see cref="Customer"/>, 決済代行会社 <see cref="PaymentAgency"/> いずれかの値
        /// </summary>
        [DataMember] public int ShareTransferFee { get; set; }
        /// <summary>手数料誤差利用</summary>
        [DataMember] public int UseFeeTolerance { get; set; }
        /// <summary>手数料自動学習 0 : しない, 1 : する  登録済の手数料を確認した場合 <see cref="VerifyCheckable"/>で、0 に変更</summary>
        [DataMember] public int UseFeeLearning { get; set; }
        /// <summary>カナ自動学習 0 : しない, 1 : する </summary>
        [DataMember] public int UseKanaLearning { get; set; }
        /// <summary>得意先マスター 一括消込対象外（個別消込優先） 1 消込可の場合でも、checkしない 手動でのcheckは可</summary>
        [DataMember] public int PrioritizeMatchingIndividually { get; set; }
        /// <summary>入金区分 一括消込から除外 0 : 一括消込可, 1 : 一括消込不可(個別消込強制)</summary>
        [DataMember] public int ForceMatchingIndividually { get; set; }
        [DataMember] public string DispCustomerCode { get; set; }
        [DataMember] public string DispCustomerName { get; set; }
        /// <summary>
        ///  学習履歴登録用 得意先/決済代表会社 カナ 異なるものは 学習履歴に登録
        /// </summary>
        [DataMember] public string DispCustomerKana { get; set; }
        [DataMember] public int IsParent { get; set; }
        [DataMember] public int DisplayOrder { get; set; }
        [DataMember] public bool UpdateFlag { get; set; }

        [DataMember] public decimal? DispBillingAmount { get; set; }
        [DataMember] public decimal? DispReceiptAmount { get; set; }
        [DataMember] public int? DispBillingCount { get; set; }
        [DataMember] public int? DispReceiptCount { get; set; }

        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public decimal CurrencyTolerance { get; set; }
        [DataMember] public string CustomerCode { get; set; }

        [DataMember] public int BillingOrder { get; set; }
        [DataMember] public int ReceiptOrder { get; set; }

        [DataMember] public int DupeCheck { get; set; }

        /// <summary> 一括消込 請求情報表示順を優先する </summary>
        [DataMember] public int BillingPriority { get; set; }
        /// <summary> 一括消込 入金情報表示順を優先する </summary>
        [DataMember] public int ReceiptPriority { get; set; }
        /// <summary> 一括消込 請求情報表示順 </summary>
        [DataMember] public int BillingDisplayOrder { get; set; }
        /// <summary> 一括消込 入金情報表示順 </summary>
        [DataMember] public int ReceiptDisplayOrder { get; set; }

        public string DispShareTransferFee
        {
            get
            {
                return !DispBillingCount.HasValue ? string.Empty
               : (ShareTransferFee == 0) ? "相"
               : (ShareTransferFee == 1) ? "自" : string.Empty;
            }
        }
        public string DispAdvanceReceivedCount
        {
            get
            {
                return !DispReceiptCount.HasValue ? string.Empty
               : (AdvanceReceivedCount == 2) ? "○"
               : (AdvanceReceivedCount == 1) ? "△" : string.Empty;
            }
        }
        /// <summary>差額  請求残 - 入金残</summary>
        public decimal Different { get { return BillingAmount - ReceiptAmount; } }

        /// <summary>｢差額｣画面表示値。照合不能時はnull。</summary>
        public decimal? DispDifferent
        {
            get
            {
                return (DispBillingCount == null || DispReceiptCount == null)
              ? (decimal?)null : Different;
            }
        }

        public string ReportDispShareTransferFee
        {
            get
            {
                return !DispBillingCount.HasValue ? string.Empty
               : (ShareTransferFee == 0) ? "0：相手先"
               : (ShareTransferFee == 1) ? "1：自社" : string.Empty;
            }
        }

        /// <summary>
        /// 一括消込可 不可 判定処理
        /// </summary>
        /// <param name="setting">照合設定</param>
        /// <param name="feeTolerance">手数料 誤差範囲</param>
        /// <param name="taxTolerance">消費税 誤差範囲</param>
        /// <param name="useForeignCurrency">外貨対応フラグ</param>
        /// <param name="checkFeeRegistered">得意先/決済代行会社 の任意の手数料登録確認のデリゲート</param>
        /// <param name="checkFeeExist">得意先/決済代行会社 の 差額が手数料として登録されているか確認するデリゲート</param>
        /// <remarks>
        /// 
        /// </remarks>
        public bool VerifyCheckable(
            CollationSetting setting,
            decimal feeTolerance,
            decimal taxTolerance,
            bool useForeignCurrency,
            Func<Collation, bool> checkFeeRegistered,
            Func<Collation, bool> checkFeeExist)
        {
            // 照合不能 or 一括消込不可設定
            if (!DispDifferent.HasValue || ForceMatchingIndividually == 1)
            {
                Checked = false;
                BankTransferFee = 0;
                TaxDifference = 0;

                return false;
            }

            var checkable = false; // 照合結果一覧で一括消込可能か否か(チェックボックスのクリック可否)
            var docheck = false;   // 初期状態でチェックしておくか否か
            var bankFee = 0M;
            var taxDifference = 0M;

            if (IsEqual)
            {
                checkable = true;
                if (ShareTransferFee == 0 || (ShareTransferFee == 1 && setting.ForceShareTransferFee == 0))
                {
                    docheck = true;
                }
            }
            else if (IsTaxToleranceEnabled(useForeignCurrency, taxTolerance))
            {
                checkable = true;
                docheck = setting.PrioritizeMatchingIndividuallyTaxTolerance == 0;
                taxDifference = -Different; // 入金側 誤差+ は 符合逆転
            }
            else if (IsFeeRegistered(checkFeeRegistered(this)))
            {
                if (checkFeeExist(this)) // 同じ金額で登録済
                {
                    checkable = true;
                    docheck = true;
                    bankFee = Different;
                    UseFeeLearning = 0;
                }
            }
            else if (IsFeeToleranceEnabled(feeTolerance))
            {
                checkable = true;
                docheck = true;
                bankFee = Different;
            }

            // 複数入金時に一括消込対象から外す設定で、複数入金
            if (docheck && setting.PrioritizeMatchingIndividuallyMultipleReceipts == 1 && 1 < ReceiptCount)
            {
                docheck = false;
            }

            //一括消込対象外（得意先マスター）
            if (docheck && PrioritizeMatchingIndividually == 1)
            {
                docheck = false;
            }

            Checked = docheck;
            BankTransferFee = bankFee;
            TaxDifference = taxDifference;

            return checkable;
        }

        /// <summary>差額が0</summary>
        private bool IsEqual { get { return DispDifferent == 0M; } }

        /// <summary>消費税誤差 有効
        /// 外貨 非利用 かつ JPY 消費税誤差設定が 0以外 で 誤差が 絶対値以内で有効</summary>
        /// <param name="useForeignCurrency"></param>
        /// <param name="taxTolerance"></param>
        /// <returns></returns>
        private bool IsTaxToleranceEnabled(bool useForeignCurrency, decimal taxTolerance)
            => !useForeignCurrency
                && CurrencyCode == Common.Constants.DefaultCurrencyCode
                && taxTolerance != 0
                && Math.Abs(Different) <= taxTolerance;

        /// <summary>手数料登録済  <see cref="ShareTransferFee"/>が 1 : 自社負担 かつ 手数料登録済</summary>
        /// <param name="registered">対象 得意先/決済代行会社でなんらかの登録がある</param>
        /// <returns></returns>
        private bool IsFeeRegistered(bool registered)
            => ShareTransferFee == 1 && registered;

        /// <summary>手数料誤差 有効
        /// 手数料誤差 利用 かつ 誤差金額が 0 より大きく 設定されている 手数料誤差金額以内</summary>
        /// <param name="feeTolerance">消費税誤差 範囲</param>
        /// <returns></returns>
        private bool IsFeeToleranceEnabled(decimal feeTolerance)
            => UseFeeTolerance == 1 && 0M < Different && Different <= feeTolerance;
    }

    [DataContract]
    public class CollationsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Collation> Collation { get; set; }
    }
}
