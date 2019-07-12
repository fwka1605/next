using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CollationSetting : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        /// <summary>得意先コードセットを必須にする</summary>
        [DataMember] public int RequiredCustomer { get; set; }
        /// <summary>得意先コードを自動でセットする</summary>
        [DataMember] public int AutoAssignCustomer { get; set; }
        /// <summary>学習履歴機能を利用する</summary>
        [DataMember] public int LearnKanaHistory { get; set; }
        /// <summary>入金データ振分画面を使用する</summary>
        [DataMember] public int UseApportionMenu { get; set; }
        /// <summary>自動更新を許可する</summary>
        [DataMember] public int ReloadCollationData { get; set; }
        /// <summary>前受振替を使用する</summary>
        [DataMember] public int UseAdvanceReceived { get; set; }
        /// <summary>前受伝票日付設定方法
        /// 0 : 未入力, 1 : システム日付, 2 : 請求日, 3 : 売上日, 4 : 請求締日, 5 : 入金予定日, 6 : 入金日
        /// </summary>
        [DataMember] public int AdvanceReceivedRecordedDateType { get; set; }
        /// <summary>一括消込チェックONのデータを消込実行させておく</summary>
        [DataMember] public int AutoMatching { get; set; }
        /// <summary>一括消込チェックONのソートをあらかじめ実行しておく</summary>
        [DataMember] public int AutoSortMatchingEnabledData { get; set; }
        /// <summary>入金日・入金予定日の絞込開始日を指定する</summary>
        [DataMember] public int UseFromToNarrowing { get; set; }
        /// <summary>消込済データ表示時、消込日時にシステム日時を設定</summary>
        [DataMember] public int SetSystemDateToCreateAtFilter { get; set; }
        /// <summary>入金が複数件の場合は一括消込対象から外す</summary>
        [DataMember] public int PrioritizeMatchingIndividuallyMultipleReceipts { get; set; }
        /// <summary>金額一致していても手数料自社負担の場合には、一括消込対象から外す</summary>
        [DataMember] public int ForceShareTransferFee { get; set; }
        /// <summary>入金データ修正時に得意先を付与した場合、学習履歴に登録する</summary>
        [DataMember] public int LearnSpecifiedCustomerKana { get; set; }
        /// <summary>個別消込時の消込順序</summary>
        [DataMember] public int MatchingSilentSortedData { get; set; }
        /// <summary>消込時の請求情報・入金情報表示設定</summary>
        [DataMember] public int BillingReceiptDisplayOrder { get; set; }

        /// <summary>入金データ取込 取込時スペースを除去</summary>
        [DataMember] public int RemoveSpaceFromPayerName { get; set; }
        /// <summary>差額が消費税誤差の範囲内でも一括消込対象外から外す（消費税誤差時に、個別消込優先）</summary>
        [DataMember] public int PrioritizeMatchingIndividuallyTaxTolerance { get; set; }
        /// <summary>仕訳出力内容設定 0:標準, 1:汎用</summary>
        [DataMember] public int JournalizingPattern { get; set; }
        /// <summary>請求書単位で消費税計算を行う BillingInputId 単位で消費税計算</summary>
        [DataMember] public int CalculateTaxByInputId { get; set; }
        /// <summary> 一括消込入金情報ソート順コラム </summary>
        [DataMember] public int SortOrderColumn { get; set; }
        /// <summary> 一括消込入金情報ソート順 0:昇順・1:降順 </summary>
        [DataMember] public int SortOrder { get; set; }

        /// <summary> 一括消込　請求情報・入金情報表示順 </summary>
        public SortOrderColumnType SortOrderDirection =>
            SortOrderColumn == (int)SequencialCollationSortColumn.PayerName         && SortOrder == (int)SortOrderType.Ascending  ? SortOrderColumnType.PayerNameAsc :
            SortOrderColumn == (int)SequencialCollationSortColumn.PayerName         && SortOrder == (int)SortOrderType.Descending ? SortOrderColumnType.PayerNameDesc :
            SortOrderColumn == (int)SequencialCollationSortColumn.ReceiptRecordedAt && SortOrder == (int)SortOrderType.Ascending  ? SortOrderColumnType.MinRecordedAt :
            SortOrderColumn == (int)SequencialCollationSortColumn.ReceiptRecordedAt && SortOrder == (int)SortOrderType.Descending ? SortOrderColumnType.MaxRecordedAt :
            SortOrderColumn == (int)SequencialCollationSortColumn.ReceiptId         && SortOrder == (int)SortOrderType.Ascending  ? SortOrderColumnType.MinReceiptId :
            SortOrderColumn == (int)SequencialCollationSortColumn.ReceiptId         && SortOrder == (int)SortOrderType.Descending ? SortOrderColumnType.MaxReceiptId :
            SortOrderColumnType.BillingDisplayOrder;


        [DataMember] public CollationOrder[] CollationOrders         { get; set;}
        [DataMember] public MatchingOrder[]  BillingMatchingOrders    { get; set;}
        [DataMember] public MatchingOrder[]  ReceiptMatchingOrders    { get; set;}

    }

    [DataContract]
    public class CollationSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public CollationSetting CollationSetting { get; set; }
    }
}
