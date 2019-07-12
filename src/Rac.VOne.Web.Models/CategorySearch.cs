using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class CategorySearch
    {
        [DataMember] public bool UseCommonSearch { get; set; }
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public int? CategoryType { get; set; }
        [DataMember] public string[] Codes { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int? UseAccountTransfer { get; set; }
        [DataMember] public int? UseLimitDate { get; set; }
        [DataMember] public int? UseInput { get; set; }

        [DataMember] public int[] Ids { get; set; }

        // other options

        public Func<Category, bool> SearchPredicate { get; set; }
    }
}
