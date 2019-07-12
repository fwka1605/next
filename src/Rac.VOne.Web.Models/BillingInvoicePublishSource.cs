using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract] public class BillingInvoicePublishSource
    {
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public string ConnectionId { get; set; }
        [DataMember] public BillingInvoiceForPublish[] Items { get; set; }
    }
}
