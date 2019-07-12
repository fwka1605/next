using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class LogData : IByCompany
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string ClientName { get; set; }
        [DataMember] public DateTime LoggedAt { get; set; }
        [DataMember] public string LoginUserCode { get; set; }
        [DataMember] public string LoginUserName { get; set; }
        [DataMember] public int? MenuId { get; set; }
        [DataMember] public string MenuName { get; set; }
        [DataMember] public string OperationName { get; set; }
        [DataMember] public int LogCount { get; set; }
        [DataMember] public DateTime FirstLoggedAt { get; set; }
    }

    [DataContract]
    public class LogDataResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public List<LogData> LogData { get; set; }
    }

    [DataContract]
    public class LogDatasResult : IProcessResult
    {
        [DataMember]
        public ProcessResult ProcessResult { get; set; }
        [DataMember]
        public LogData[] Datas { get; set; }
    }
}
