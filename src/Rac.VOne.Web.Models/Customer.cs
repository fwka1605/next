using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Customer : IMasterData, IMaster, ISynchronization
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Kana { get; set; }
        [DataMember] public string PostalCode { get; set; } = string.Empty;
        [DataMember] public string Address1 { get; set; } = string.Empty;
        [DataMember] public string Address2 { get; set; } = string.Empty;
        [DataMember] public string Tel { get; set; } = string.Empty;
        [DataMember] public string Fax { get; set; } = string.Empty;
        [DataMember] public string CustomerStaffName { get; set; } = string.Empty;
        [DataMember] public string ExclusiveBankCode { get; set; } = string.Empty;
        [DataMember] public string ExclusiveBankName { get; set; } = string.Empty;
        [DataMember] public string ExclusiveBranchCode { get; set; } = string.Empty;
        [DataMember] public string ExclusiveBranchName { get; set; } = string.Empty;
        [DataMember] public string ExclusiveAccountNumber { get; set; } = string.Empty;
        [DataMember] public int? ExclusiveAccountTypeId { get; set; }
        [DataMember] public int ShareTransferFee { get; set; }
        [DataMember] public decimal CreditLimit { get; set; }
        [DataMember] public int ClosingDay { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public int StaffId { get; set; }
        [DataMember] public int IsParent { get; set; }
        [DataMember] public string Note { get; set; } = string.Empty;
        [DataMember] public int? SightOfBill { get; set; }
        [DataMember] public string DensaiCode { get; set; } = string.Empty;
        [DataMember] public string CreditCode { get; set; } = string.Empty;
        [DataMember] public string CreditRank { get; set; } = string.Empty;
        [DataMember] public string TransferBankCode { get; set; } = string.Empty;
        [DataMember] public string TransferBankName { get; set; } = string.Empty;
        [DataMember] public string TransferBranchCode { get; set; } = string.Empty;
        [DataMember] public string TransferBranchName { get; set; } = string.Empty;
        [DataMember] public string TransferAccountNumber { get; set; } = string.Empty;
        [DataMember] public int? TransferAccountTypeId { get; set; }
        [DataMember] public int ReceiveAccountId1 { get; set; }
        [DataMember] public int ReceiveAccountId2 { get; set; }
        [DataMember] public int ReceiveAccountId3 { get; set; }
        [DataMember] public int UseFeeLearning { get; set; }
        [DataMember] public int UseKanaLearning { get; set; }
        /// <summary>休業日設定
        /// 0 : 考慮しない
        /// 1 : 休業日の前
        /// 2 : 休業日の後
        /// </summary>
        [DataMember] public int HolidayFlag { get; set; }
        [DataMember] public int UseFeeTolerance { get; set; }
        [DataMember] public string StringValue1 { get; set; } = string.Empty;
        [DataMember] public string StringValue2 { get; set; } = string.Empty;
        [DataMember] public string StringValue3 { get; set; } = string.Empty;
        [DataMember] public string StringValue4 { get; set; } = string.Empty;
        [DataMember] public string StringValue5 { get; set; } = string.Empty;
        [DataMember] public int? IntValue1 { get; set; }
        [DataMember] public int? IntValue2 { get; set; }
        [DataMember] public int? IntValue3 { get; set; }
        [DataMember] public int? IntValue4 { get; set; }
        [DataMember] public int? IntValue5 { get; set; }
        [DataMember] public decimal? NumericValue1 { get; set; }
        [DataMember] public decimal? NumericValue2 { get; set; }
        [DataMember] public decimal? NumericValue3 { get; set; }
        [DataMember] public decimal? NumericValue4 { get; set; }
        [DataMember] public decimal? NumericValue5 { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string TransferCustomerCode { get; set; } = string.Empty;
        [DataMember] public string TransferNewCode { get; set; } = string.Empty;
        [DataMember] public string TransferAccountName { get; set; } = string.Empty;
        [DataMember] public int CollectOffsetMonth { get; set; }
        [DataMember] public int CollectOffsetDay { get; set; }
        [DataMember] public int ParentCustomerId { get; set; }
        /// <summary>一括消込対象外（個別消込優先）</summary>
        [DataMember] public int PrioritizeMatchingIndividually { get; set; }
        [DataMember] public string CollationKey { get; set; } = string.Empty;
        [DataMember] public int ExcludeInvoicePublish { get; set; }
        [DataMember] public int ExcludeReminderPublish { get; set; }
        [DataMember] public string DestinationDepartmentName { get; set; } = string.Empty;
        [DataMember] public string Honorific { get; set; } = string.Empty;
        public string ClosingDayText
        {
            get
            {
                switch (ClosingDay)
                {
                    case 0: return $"都度";
                    case 99: return $"末日";
                    default: return $"{ClosingDay}日";
                }
            }
        }
        public string CollectOffsetText
        {
            get
            {
                if (CollectOffsetMonth == 0)
                {
                    if (CollectOffsetDay == 99)
                    {
                        return $"末日";
                    }
                    else
                    {
                        return (ClosingDay == 0 ) ? $"{CollectOffsetDay}日以内" : $"{CollectOffsetDay}日";
                    }
                }
                else if (CollectOffsetMonth == 1)
                {
                    if (CollectOffsetDay == 99)
                    {
                        return $"翌月の末日";
                    }
                    else
                    {
                        return $"翌月の{CollectOffsetDay}日";
                    }
                }
                else
                {
                    if (CollectOffsetDay == 99)
                    {
                        return $"{CollectOffsetMonth}ヶ月後の末日";
                    }
                    else
                    {
                        return $"{CollectOffsetMonth}ヶ月後の{CollectOffsetDay}日";
                    }
                }
            }
        }
        public string IsParentText
        {
            get
            {
                switch (IsParent)
                {
                    case 0: return "";
                    case 1: return "*";
                    default: return string.Empty;
                }
            }
        }
        public string ShareTransferFeeText
        {
            get
            {
                switch (ShareTransferFee)
                {
                    case 0: return "相手先";
                    case 1: return "自社";
                    default: return string.Empty;
                }
            }
        }

        public string GreaterThanRateOneText
        {
            get { return (Convert.ToDecimal(GreaterThanRate1) == 0M) ? string.Empty : $"{GreaterThanRate1}%"; }
        }
        public string GreaterThanRateTwoText
        {
            get { return (Convert.ToDecimal(GreaterThanRate2) == 0M) ? string.Empty : $"{GreaterThanRate2}%"; }
        }
        public string GreaterThanRateThreeText
        {
            get { return (Convert.ToDecimal(GreaterThanRate3) == 0M) ? string.Empty : $"{GreaterThanRate3}%"; }
        }

        public string ExclusiveAccountTypeText
        {
            get
            {
                switch (ExclusiveAccountTypeId)
                {
                    case 1: return "普通";
                    case 2: return "当座";
                    case 4: return "貯蓄";
                    case 5: return "通知";
                    case 8: return "外貨";
                    default: return string.Empty;
                }
            }
        }

        public string TransferAccountTypeText
        {
            get
            {
                switch (TransferAccountTypeId)
                {
                    case 1: return "普通";
                    case 2: return "当座";
                    case 4: return "貯蓄";
                    case 5: return "通知";
                    case 8: return "外貨";
                    default: return string.Empty;
                }
            }
        }

        public string HolidayFlagText
        {
            get
            {
                switch (HolidayFlag)
                {
                    case 0: return "考慮しない";
                    case 1: return "休業日の前";
                    case 2: return "休業日の後";
                    default: return string.Empty;
                }
            }
        }

        public string GreaterThanRoundingModeOneText
        {
            get
            {
                if (CollectCategoryCode != "00") return string.Empty;
                switch (GreaterThanRoundingMode1)
                {
                    case 0: return "端数";
                    case 1: return "一";
                    case 2: return "十";
                    case 3: return "百";
                    case 4: return "千";
                    case 5: return "万";
                    case 6: return "十万";
                    default: return string.Empty;
                }
            }
        }

        public string GreaterThanRoundingModeTwoText
        {
            get
            {
                if (CollectCategoryCode != "00") return string.Empty;
                switch (GreaterThanRoundingMode2)
                {
                    case 0: return "端数";
                    case 1: return "一";
                    case 2: return "十";
                    case 3: return "百";
                    case 4: return "千";
                    case 5: return "万";
                    case 6: return "十万";
                    default: return string.Empty;
                }
            }
        }

        public string GreaterThanRoundingModeThreeText
        {
            get
            {
                if (CollectCategoryCode != "00") return string.Empty;
                switch (GreaterThanRoundingMode3)
                {
                    case 0: return "端数";
                    case 1: return "一";
                    case 2: return "十";
                    case 3: return "百";
                    case 4: return "千";
                    case 5: return "万";
                    case 6: return "十万";
                    default: return string.Empty;
                }
            }
        }

        //Other table fields
        [DataMember] public string StaffName { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public decimal ThresholdValue { get; set; }
        [DataMember] public string CollectCategoryName { get; set; }
        [DataMember] public string LessThanCollectCategoryCode{ get; set; }
        [DataMember] public int LessThanCollectCategoryId { get; set; }
        [DataMember] public int GreaterThanCollectCategoryId1 { get; set; }
        [DataMember] public string GreaterThanCollectCategoryCode1 { get; set; }
        [DataMember] public int? GreaterThanCollectCategoryId2 { get; set; }
        [DataMember] public string GreaterThanCollectCategoryCode2 { get; set; }
        [DataMember] public int? GreaterThanCollectCategoryId3{get; set;}
        [DataMember] public string GreaterThanCollectCategoryCode3 { get; set; }
        [DataMember] public decimal? GreaterThanRate1 { get; set; }
        [DataMember] public decimal? GreaterThanRate2 { get; set; }
        [DataMember] public decimal? GreaterThanRate3{ get; set; }
        [DataMember] public int? GreaterThanRoundingMode1{ get; set; }
        [DataMember] public int? GreaterThanRoundingMode2{ get; set; }
        [DataMember] public int? GreaterThanRoundingMode3{ get; set; }
        [DataMember] public int? GreaterThanSightOfBill1{ get; set; }
        [DataMember] public int? GreaterThanSightOfBill2{ get; set; }
        [DataMember] public int? GreaterThanSightOfBill3{ get; set; }
        [DataMember] public string ParentCode { get; set; }
        [DataMember] public string ParentName { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public string LessThanCollectCategoryName { get; set; }
        [DataMember] public string GreaterThanCollectCategoryName1 { get; set; }
        [DataMember] public string GreaterThanCollectCategoryName2 { get; set; }
        [DataMember] public string GreaterThanCollectCategoryName3 { get; set; }
        [DataMember] public string CompanyBankInfo1 { get; set; }
        [DataMember] public string CompanyBankInfo2 { get; set; }
        [DataMember] public string CompanyBankInfo3 { get; set; }
        [DataMember] public string LastUpdateUser { get; set; }

        /// <summary>CSV インポート時に利用</summary>
        public int LineNo { get; set; }

        /// <summary>請求日と得意先マスタ設定値から請求締日を求める。</summary>
        /// <param name="customer"></param>
        /// <param name="billingDate">請求日</param>
        /// <returns></returns>
        public DateTime? GetClosingAt(DateTime billingDate)
        {
            var result = billingDate.Date;
            var closingDay = ClosingDay;
            try
            {
                if (closingDay == 0) // 都度請求時は請求日と同日
                {
                    return billingDate;
                }

                if (closingDay < billingDate.Day)
                    result = result.AddMonths(1);
                if (closingDay < 28)
                    return new DateTime(result.Year, result.Month, closingDay);
                else
                    return new DateTime(result.Year, result.Month, 1).AddMonths(1).AddDays(-1);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>請求締日と得意先マスタ、カレンダーマスタ設定値から予定日を求める。</summary>
        /// <param name="customer"></param>
        /// <param name="closingDate">請求締日</param>
        /// <param name="holidays"></param>
        /// <returns></returns>
        public DateTime? GetDueAt(DateTime closingDate, IEnumerable<HolidayCalendar> holidays)
        {
            var offsetMonth = CollectOffsetMonth;
            var offsetDay   = CollectOffsetDay;
            var closingDay  = ClosingDay;
            if (offsetMonth == 0)
                offsetDay = Math.Max(offsetDay, closingDay);

            try
            {
                DateTime dueAt;
                if (ClosingDay == 0) // 都度請求時は支払期日(日)の日数を足し込む
                {
                    dueAt = closingDate.Date.AddDays(offsetDay);
                }
                else
                {
                    dueAt = closingDate.Date.AddMonths(offsetMonth);
                    if (offsetDay < 28)
                        dueAt = new DateTime(dueAt.Year, dueAt.Month, offsetDay);
                    else
                        dueAt = new DateTime(dueAt.Year, dueAt.Month, 1).AddMonths(1).AddDays(-1);
                }

                return GetBusinessDay(dueAt, (CustomerHolidayFlag)HolidayFlag, holidays);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// Customer.HolidayFlag
        /// </summary>
        private enum CustomerHolidayFlag
        {
            考慮しない = 0,
            休業日の前 = 1,
            休業日の後 = 2,
        }

        /// <summary>
        /// 休業日設定とカレンダーマスタ設定値を元に、日付を営業日に補正する。
        /// </summary>
        /// <param name="date"></param>
        /// <param name="holidayFlag">休業日設定(Customer.HolidayFlag)</param>
        /// <param name="holidays">カレンダーマスタ設定値</param>
        /// <returns></returns>
        private DateTime GetBusinessDay(DateTime date,
            CustomerHolidayFlag holidayFlag, IEnumerable<HolidayCalendar> holidays)
        {
            if (holidayFlag == CustomerHolidayFlag.考慮しない) // 補正なし
            {
                return date;
            }

            var vector = (holidayFlag == CustomerHolidayFlag.休業日の前) ? -1 : 1;

            while (true)
            {
                var sat = (date.DayOfWeek == DayOfWeek.Saturday);
                var sun = (date.DayOfWeek == DayOfWeek.Sunday);
                // 現在の日付が土／日／休業日でなければ営業日(ループ終了)
                if (!(sat || sun || holidays.Any(h => h.Holiday.Date == date.Date)))
                {
                    break;
                }

                date = date.AddDays(vector);
            };

            return date;
        }

        public CustomerPaymentContract GetContract()
            => new CustomerPaymentContract {
                CustomerId                      = Id,
                ThresholdValue                  = ThresholdValue,
                LessThanCollectCategoryId       = LessThanCollectCategoryId,
                GreaterThanCollectCategoryId1   = GreaterThanCollectCategoryId1,
                GreaterThanRate1                = GreaterThanRate1,
                GreaterThanRoundingMode1        = GreaterThanRoundingMode1,
                GreaterThanSightOfBill1         = GreaterThanSightOfBill1 ?? 0,
                GreaterThanCollectCategoryId2   = GreaterThanCollectCategoryId2,
                GreaterThanRate2                = GreaterThanRate2,
                GreaterThanRoundingMode2        = GreaterThanRoundingMode2,
                GreaterThanSightOfBill2         = GreaterThanSightOfBill2,
                GreaterThanCollectCategoryId3   = GreaterThanCollectCategoryId3,
                GreaterThanRate3                = GreaterThanRate3,
                GreaterThanRoundingMode3        = GreaterThanRoundingMode3,
                GreaterThanSightOfBill3         = GreaterThanSightOfBill3,
                CreateBy                        = CreateBy,
                UpdateBy                        = UpdateBy,
            };


    }

    [DataContract]
    public class CustomersResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Customer> Customers { get; set; }
    }

    [DataContract]
    public class CustomerResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Customer Customer { get; set; }
    }
}
