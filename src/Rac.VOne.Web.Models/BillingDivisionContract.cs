using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingDivisionContract
    {
      [DataMember] public int Id { get; set; }
      [DataMember] public int CompanyId { get; set; }
      [DataMember] public int CustomerId { get; set; }
      [DataMember] public string ContractNumber { get; set; }
      [DataMember] public int FirstDateType { get; set; }
      [DataMember] public int Monthly { get; set; }
      [DataMember] public int BasisDay { get; set; }
      [DataMember] public int DivisionCount { get; set; }
      [DataMember] public int RoundingMode { get; set; }
      [DataMember] public int RemainsApportionment { get; set; }
      [DataMember] public long? BillingId { get; set; }
      [DataMember] public decimal BillingAmount { get; set; }
      [DataMember] public int Comfirm { get; set; }
      [DataMember] public DateTime? CancelDate { get; set; }
      [DataMember] public int CreateBy { get; set; }
      [DataMember] public DateTime CreateAt { get; set; }
      [DataMember] public int UpdateBy { get; set; }
      [DataMember] public DateTime UpdateAt { get; set; }
    }


    [DataContract]
    public class BillingDivisionContractsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BillingDivisionContract> BillingDivisionContracts { get; set; }
    }
}
