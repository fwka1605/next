using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class BillingAccountTransferSearch
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int PaymentAgencyId { get; set; }
        [DataMember] public DateTime TransferDate { get; set; }
    }
}
