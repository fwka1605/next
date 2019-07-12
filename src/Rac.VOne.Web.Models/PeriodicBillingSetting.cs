using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class PeriodicBillingSetting : IByCompany, ITransactional
    {
        /// <summary>定期請求設定ID</summary>
        [DataMember] public long Id { get; set; }
        /// <summary>会社ID</summary>
        [DataMember] public int CompanyId { get; set; }
        /// <summary>パターン名</summary>
        [DataMember] public string Name { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        /// <summary>得意先ID</summary>
        [DataMember] public int CustomerId { get; set; }
        /// <summary>送付先ID</summary>
        [DataMember] public int? DestinationId { get; set; }
        /// <summary>請求部門ID</summary>
        [DataMember] public int DepartmentId { get; set; }
        /// <summary>営業担当者ID</summary>
        [DataMember] public int StaffId { get; set; }
        /// <summary>回収区分ID</summary>
        [DataMember] public int CollectCategoryId { get; set; }
        /// <summary>請求サイクル</summary>
        [DataMember] public int BilledCycle { get; set; }
        /// <summary>請求日</summary>
        [DataMember] public int BilledDay { get; set; }
        /// <summary>開始年月</summary>
        [DataMember] public DateTime StartMonth { get; set; }
        /// <summary>終了年月</summary>
        [DataMember] public DateTime? EndMonth { get; set; }
        /// <summary>請求書番号</summary>
        [DataMember] public string InvoiceCode { get; set; }
        /// <summary>備考1 明細に入力した値か、サイクルに基づく 年月分を設定するか</summary>
        [DataMember] public int SetBillingNote1 { get; set; }
        /// <summary>備考2 明細に入力した値か、サイクルに基づく 年月分を設定するか</summary>
        [DataMember] public int SetBillingNote2 { get; set; }
        /// <summary>登録者ID</summary>
        [DataMember] public int CreateBy { get; set; }
        /// <summary>登録日時</summary>
        [DataMember] public DateTime CreateAt { get; set; }
        /// <summary>更新者ID</summary>
        [DataMember] public int UpdateBy { get; set; }
        /// <summary>更新日時</summary>
        [DataMember] public DateTime UpdateAt { get; set; }

        #region other table
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string DestinationCode { get; set; }
        [DataMember] public string Addressee { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public string StaffName { get; set; }
        [DataMember] public DateTime LastUpdateAt { get; set; }
        [DataMember] public string LastUpdatedBy { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public DateTime BaseDate { get; set; }
        /// <summary>定期請求作成済テーブル 処理年月のMAX値</summary>
        [DataMember] public DateTime? LastCreateYearMonth { get; set; }

        /// <summary></summary>
        [DataMember] public List<PeriodicBillingSettingDetail> Details { get; set; } = new List<PeriodicBillingSettingDetail>();

        /// <summary>送付先 宛名 DepartmentName or Addressee + Honorific クエリで対応</summary>
        [DataMember] public string DestinationName { get; set; }

        /// <summary>エクスポートで必要</summary>
        public decimal BillingAmount => Details?.Sum(x => x.BillingAmount) ?? 0M;

        public bool IsDetailsInputted => Details?.Any() ?? false;

        public string BilledCycleStatus // 余計なお世話な気がするが...
            => BilledCycle == 1 ? "毎月" : $"{BilledCycle}ヶ月";

        /// <summary>画面 パターン選択時に利用</summary>
        public bool Selected { get; set; }

        public void InitializeForGetBilling(DateTime baseDate)
        {
            BaseDate = baseDate;
            BilledAt = new DateTime(BaseDate.Year, BaseDate.Month,
                       (28 <= BilledDay ? DateTime.DaysInMonth(BaseDate.Year, BaseDate.Month) : BilledDay));
        }

        /// <summary><see cref="PeriodicBillingSetting"/>to<see cref="Billing"/>
        /// 自動生成 請求書番号がある場合は invoiceCode に値を設定する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Billing> GetBillings(Customer customer,
            IEnumerable<HolidayCalendar> holidays)
            => Details.Select(x => {
                var closingAt   = customer.GetClosingAt(BilledAt).Value;
                var dueAt       = customer.GetDueAt(closingAt, holidays).Value;
                return new Billing {
                    #region header
                    CompanyId           = CompanyId,
                    CurrencyId          = CurrencyId,
                    CustomerId          = CustomerId,
                    DestinationId       = DestinationId,
                    DepartmentId        = DepartmentId,
                    StaffId             = StaffId,
                    CollectCategoryId   = CollectCategoryId,
                    InvoiceCode         = InvoiceCode ?? string.Empty,
                    InputType           = (int)Common.Constants.BillingInputType.PeriodicBilling,
                    BilledAt            = BilledAt,
                    SalesAt             = BilledAt,
                    ClosingAt           = closingAt,
                    DueAt               = dueAt,
                    #endregion
                    #region detail
                    BillingCategoryId   = x.BillingCategoryId,
                    TaxClassId          = x.TaxClassId,
                    DebitAccountTitleId = x.DebitAccountTitleId,
                    BillingAmount       = x.BillingAmount,
                    RemainAmount        = x.BillingAmount,
                    TaxAmount           = x.TaxAmount,
                    Quantity            = x.Quantity,
                    UnitPrice           = x.UnitPrice,
                    UnitSymbol          = x.UnitSymbol,
                    Note1               = SetBillingNote1 == 0 ? x.Note1 : GetBillingTermInformationg(BilledAt),
                    Note2               = SetBillingNote2 == 0 ? x.Note2 : GetBillingTermInformationg(BilledAt),
                    Note3               = x.Note3,
                    Note4               = x.Note4,
                    Note5               = x.Note5,
                    Note6               = x.Note6,
                    Note7               = x.Note7,
                    Note8               = x.Note8,
                    Memo                = x.Memo,
                    CreateBy            = UpdateBy,
                    UpdateBy            = UpdateBy,
                    #endregion
                };
            }).ToList();

        private string GetBillingTermInformationg(DateTime ym)
         => BilledCycle == 1
         ? $"{ym:yyyy年MM月}分"
         : $"{ym:yyyy年MM月} ～ {ym.AddMonths(BilledCycle - 1):yyyy年MM月}分";
        #endregion
    }

    [DataContract] public class PeriodicBillingSettingResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public PeriodicBillingSetting PeriodicBillingSetting { get; set; }
    }

    [DataContract]
    public class PeriodicBillingSettingsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<PeriodicBillingSetting> PeriodicBillingSettings { get; set; }
    }
}
