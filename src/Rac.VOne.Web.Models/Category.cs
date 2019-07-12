using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class Category : IMasterData, IIdentical, IByCompany
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CategoryType { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int? AccountTitleId { get; set; }
        [DataMember] public string SubCode { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public int? TaxClassId { get; set; }
        [DataMember] public int UseLimitDate { get; set; }
        [DataMember] public int UseLongTermAdvanceReceived { get; set; }
        [DataMember] public int UseCashOnDueDates { get; set; }
        [DataMember] public int UseAccountTransfer { get; set; }
        [DataMember] public int? PaymentAgencyId { get; set; }
        [DataMember] public int UseDiscount { get; set; }
        [DataMember] public int UseAdvanceReceived { get; set; }
        [DataMember] public int ForceMatchingIndividually { get; set; }
        [DataMember] public int UseInput { get; set; }
        [DataMember] public int MatchingOrder { get; set; }
        [DataMember] public string StringValue1 { get; set; }
        [DataMember] public string StringValue2 { get; set; }
        [DataMember] public string StringValue3 { get; set; }
        [DataMember] public string StringValue4 { get; set; }
        [DataMember] public string StringValue5 { get; set; }
        [DataMember] public int IntValue1 { get; set; }
        [DataMember] public int IntValue2 { get; set; }
        [DataMember] public int IntValue3 { get; set; }
        [DataMember] public int IntValue4 { get; set; }
        [DataMember] public int IntValue5 { get; set; }
        [DataMember] public decimal NumericValue1 { get; set; }
        [DataMember] public decimal NumericValue2 { get; set; }
        [DataMember] public decimal NumericValue3 { get; set; }
        [DataMember] public decimal NumericValue4 { get; set; }
        [DataMember] public decimal NumericValue5 { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
        [DataMember] public string ExternalCode { get; set; }

        [DataMember] public int CategoryCompanyId { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int ExcludeInvoicePublish { get; set; }

        /// <summary>長期前受管理</summary>
        public string UseLongTermAdvanceReceivedText
        { get { return UseLongTermAdvanceReceived == 0 ? "行わない" : "行う"; } }

        /// <summary>期日現金管理</summary>
        public string UseLimitDateText
        { get { return UseLimitDate == 0 ? "行わない" : "行う"; } }
        /// <summary>コード：名称 <see cref="Code"/>：<see cref="Name"/></summary>
        public string CodeAndName { get { return $"{Code}：{Name}"; } }

        // AccountTitleマスターより
        [DataMember] public string AccountTitleCode { get; set; }
        [DataMember] public string AccountTitleName { get; set; }

        // TaxClassマスターより
        [DataMember] public string TaxClassName { get; set; }

        // PaymentAgencyマスターより
        [DataMember]public string PaymentAgencyCode { get; set; }
        [DataMember]public string PaymentAgencyName { get; set; }
    }

    [DataContract]
    public class CategoriesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<Category> Categories { get; set; }
    }

    [DataContract]
    public class CategoryResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public Category Category { get; set; }
    }
}
