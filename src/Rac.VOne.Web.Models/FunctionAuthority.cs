using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class FunctionAuthority
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int AuthorityLevel { get; set; }
        [DataMember] public FunctionType FunctionType { get; set; }
        [DataMember] public bool Available { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class FunctionAuthorityResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public FunctionAuthority FunctionAuthority { get; set; }
    }

    [DataContract]
    public class FunctionAuthoritiesResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<FunctionAuthority> FunctionAuthorities { get; set; }
    }

    public enum FunctionType
    {
        /// <summary>0:マスターインポート</summary>
        MasterImport = 0,
        /// <summary>1:マスターエクスポート</summary>
        MasterExport,
        /// <summary>2:請求データ修正・削除</summary>
        ModifyBilling,
        /// <summary>3:請求データ復活</summary>
        RecoverBilling,
        /// <summary>4:入金データ修正・削除</summary>
        ModifyReceipt,
        /// <summary>5:入金データ復活</summary>
        RecoverReceipt,
        /// <summary>6:消込解除</summary>
        CancelMatching,
    }

    public class FunctionAuthorities
    {
        private Dictionary<FunctionType, bool> Available { get; } = new Dictionary<FunctionType, bool>();
        public bool this[FunctionType value]
        {
            get { return Available[value]; }
        }

        public FunctionAuthorities(IEnumerable<FunctionAuthority> authorities)
        {
            if (authorities != null)
            {
                foreach (FunctionAuthority auth in authorities)
                {
                    Available.Add(auth.FunctionType, auth.Available);
                }
            }

            foreach (FunctionType type in Enum.GetValues(typeof(FunctionType)))
            {
                if (!Available.ContainsKey(type))
                {
                    Available.Add(type, false);
                }
            }
        }
    }
}
