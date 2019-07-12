using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BankAccountSearch
    {
        /// <summary>
        /// bool でどうこう とか 微妙すぎ 汎用検索用の string proeprty 設ければいいでしょうが ノーセンス君
        /// </summary>
        [DataMember] public bool UseCommonSearch { get; set; }
        [DataMember] public int? CompanyId { get; set; }
        [DataMember] public string[] BankCodes { get; set; }
        [DataMember] public string[] BranchCodes { get; set; }
        [DataMember] public int AccountTypeId { get; set; }
        [DataMember] public string AccountNumber { get; set; }
        [DataMember] public string BankName { get; set; }
        [DataMember] public string BranchName { get; set; }
        [DataMember] public int? ReceiptCategoryId { get; set; }
        [DataMember] public int? SectionId { get; set; }
        [DataMember] public int? ImportSkipping { get; set; }

        [DataMember] public string SearchWord { get; set; }
        [DataMember] public int[] Ids { get; set; }
    }
}
