using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class InputControl
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public int InputGridTypeId { get; set; }
        [DataMember] public string ColumnName { get; set; }
        [DataMember] public string ColumnNameJp { get; set; }
        [DataMember] public int ColumnOrder { get; set; }
        [DataMember] public int TabStop { get; set; }
        [DataMember] public int TabIndex { get; set; }

        public bool IsTabStop
            => System.Convert.ToBoolean(TabStop);
    }


    [DataContract]
    public class InputControlsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<InputControl> InputControls { get; set; }
    }
}
