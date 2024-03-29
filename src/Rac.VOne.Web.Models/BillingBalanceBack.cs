﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingBalanceBack : IByCompany
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int CurrencyId { get; set; }
        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int StaffId { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public DateTime CarryOverAt { get; set; }
        [DataMember] public decimal BalanceCarriedOver { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
    }
    [DataContract]
    public class BillingBalanceBackResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public BillingBalance BillingBalance { get; set; }
    }

    [DataContract]
    public class BillingBalancesBackResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<BillingBalanceBack> BillingBalancesBack { get; set; }
    }
}
