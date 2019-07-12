using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common.DataHandling;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CustomerDiscount
    {
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int Sequence { get; set; }
        /// <summary>歩引き率 numeric(5, 2) 0.00 .. 100.00</summary>
        [DataMember] public decimal Rate { get; set; }
        [DataMember] public int RoundingMode { get; set; }
        [DataMember] public decimal MinValue { get; set; }
        [DataMember] public int? DepartmentId { get; set; }
        [DataMember] public int? AccountTitleId { get; set; }
        [DataMember] public string SubCode { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        // Other table fields
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public string DepartmentName { get; set; }
        [DataMember] public string AccountTitleCode { get; set; }
        [DataMember] public string AccountTitleName { get; set; }

        // インポート・エクスポート
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string CompanyCode { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public decimal? Rate1 { get; set; }
        [DataMember] public decimal? Rate2 { get; set; }
        [DataMember] public decimal? Rate3 { get; set; }
        [DataMember] public decimal? Rate4 { get; set; }
        [DataMember] public decimal? Rate5 { get; set; }
        [DataMember] public int RoundingMode1 { get; set; }
        [DataMember] public int RoundingMode2 { get; set; }
        [DataMember] public int RoundingMode3 { get; set; }
        [DataMember] public int RoundingMode4 { get; set; }
        [DataMember] public int RoundingMode5 { get; set; }
        [DataMember] public int? DepartmentId1 { get; set; }
        [DataMember] public int? DepartmentId2 { get; set; }
        [DataMember] public int? DepartmentId3 { get; set; }
        [DataMember] public int? DepartmentId4 { get; set; }
        [DataMember] public int? DepartmentId5 { get; set; }
        [DataMember] public int? AccountTitleId1 { get; set; }
        [DataMember] public int? AccountTitleId2 { get; set; }
        [DataMember] public int? AccountTitleId3 { get; set; }
        [DataMember] public int? AccountTitleId4 { get; set; }
        [DataMember] public int? AccountTitleId5 { get; set; }
        [DataMember] public string SubCode1 { get; set; }
        [DataMember] public string SubCode2 { get; set; }
        [DataMember] public string SubCode3 { get; set; }
        [DataMember] public string SubCode4 { get; set; }
        [DataMember] public string SubCode5 { get; set; }

        /// <summary>端数処理 した値を取得</summary>
        /// <param name="source">請求額</param>
        /// <param name="precision">通貨の小数点以下桁数</param>
        public decimal GetRoundingValue(decimal source, int precision)
            => ((RoundingType)RoundingMode).Calc(source * Rate / 100M, precision) ?? 0M;

        public IEnumerable<CustomerDiscount> ToUpdateItems()
        {
            if (Rate1.HasValue)
                yield return new CustomerDiscount
                {
                    CustomerId = CustomerId,
                    Sequence = 1,
                    Rate = Rate1.Value,
                    RoundingMode = RoundingMode1,
                    MinValue = MinValue,
                    DepartmentId = DepartmentId1,
                    AccountTitleId = AccountTitleId1,
                    SubCode = SubCode1,
                    CreateBy = CreateBy,
                    UpdateBy = UpdateBy,
                };
            if (Rate2.HasValue)
                yield return new CustomerDiscount
                {
                    CustomerId = CustomerId,
                    Sequence = 2,
                    Rate = Rate2.Value,
                    RoundingMode = RoundingMode2,
                    MinValue = MinValue,
                    DepartmentId = DepartmentId2,
                    AccountTitleId = AccountTitleId2,
                    SubCode = SubCode2,
                    CreateBy = CreateBy,
                    UpdateBy = UpdateBy,
                };
            if (Rate3.HasValue)
                yield return new CustomerDiscount
                {
                    CustomerId = CustomerId,
                    Sequence = 3,
                    Rate = Rate3.Value,
                    RoundingMode = RoundingMode3,
                    MinValue = MinValue,
                    DepartmentId = DepartmentId3,
                    AccountTitleId = AccountTitleId3,
                    SubCode = SubCode3,
                    CreateBy = CreateBy,
                    UpdateBy = UpdateBy,
                };
            if (Rate4.HasValue)
                yield return new CustomerDiscount
                {
                    CustomerId = CustomerId,
                    Sequence = 4,
                    Rate = Rate4.Value,
                    RoundingMode = RoundingMode4,
                    MinValue = MinValue,
                    DepartmentId = DepartmentId4,
                    AccountTitleId = AccountTitleId4,
                    SubCode = SubCode4,
                    CreateBy = CreateBy,
                    UpdateBy = UpdateBy,
                };
            if (Rate5.HasValue)
                yield return new CustomerDiscount
                {
                    CustomerId = CustomerId,
                    Sequence = 5,
                    Rate = Rate5.Value,
                    RoundingMode = RoundingMode5,
                    MinValue = MinValue,
                    DepartmentId = DepartmentId5,
                    AccountTitleId = AccountTitleId5,
                    SubCode = SubCode5,
                    CreateBy = CreateBy,
                    UpdateBy = UpdateBy,
                };
        }
    }

    [DataContract]
    public class CustomerDiscountResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public CustomerDiscount CustomerDiscount { get; set; }
    }

    [DataContract]
    public class CustomerDiscountsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<CustomerDiscount> CustomerDiscounts { get; set; }
    }
}
