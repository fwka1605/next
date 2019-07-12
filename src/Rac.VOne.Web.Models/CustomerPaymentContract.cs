using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CustomerPaymentContract
    {
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public decimal ThresholdValue { get; set; }
        [DataMember] public int LessThanCollectCategoryId { get; set; }
        [DataMember] public int GreaterThanCollectCategoryId1 { get; set; }
        [DataMember] public decimal? GreaterThanRate1 { get; set; }
        [DataMember] public int? GreaterThanRoundingMode1 { get; set; }
        [DataMember] public int? GreaterThanSightOfBill1 { get; set; }
        [DataMember] public int? GreaterThanCollectCategoryId2 { get; set; }
        [DataMember] public decimal? GreaterThanRate2 { get; set; }
        [DataMember] public int? GreaterThanRoundingMode2 { get; set; }
        [DataMember] public int? GreaterThanSightOfBill2 { get; set; }
        [DataMember] public int? GreaterThanCollectCategoryId3 { get; set; }
        [DataMember] public decimal? GreaterThanRate3 { get; set; }
        [DataMember] public int? GreaterThanRoundingMode3 { get; set; }
        [DataMember] public int? GreaterThanSightOfBill3 { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }

        //other table fields
        [DataMember] public string GreaterThanCode1 { get; set; }
        [DataMember] public string GreaterThanName1 { get; set; }
        [DataMember] public string GreaterThanCode2 { get; set; }
        [DataMember] public string GreaterThanName2 { get; set; }
        [DataMember] public string GreaterThanCode3 { get; set; }
        [DataMember] public string GreaterThanName3 { get; set; }
        [DataMember] public string LessThanCode { get; set; }
        [DataMember] public string LessThanName { get; set; }
        [DataMember] public string CollectCategoryCode{ get; set; }
        [DataMember] public string CollectCategoryName { get; set; }

     }

    [DataContract]
    public class CustomerPaymentContractsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<CustomerPaymentContract> Payments { get; set; }
    }

    [DataContract]
    public class CustomerPaymentContractResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public CustomerPaymentContract Payment { get; set; }
    }
}
