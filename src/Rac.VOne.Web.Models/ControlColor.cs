using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Drawing;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class ControlColor
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public int LoginUserId { get; set; }
        [DataMember] public Color FormBackColor { get; set; }
        [DataMember] public Color FormForeColor { get; set; }
        [DataMember] public Color ControlEnableBackColor { get; set; }
        [DataMember] public Color ControlDisableBackColor { get; set; }
        [DataMember] public Color ControlForeColor { get; set; }
        [DataMember] public Color ControlRequiredBackColor { get; set; }
        [DataMember] public Color ControlActiveBackColor { get; set; }
        [DataMember] public Color ButtonBackColor { get; set; }
        [DataMember] public Color GridRowBackColor { get; set; }
        [DataMember] public Color GridAlternatingRowBackColor { get; set; }
        [DataMember] public Color GridLineColor { get; set; }
        [DataMember] public Color InputGridBackColor { get; set; }
        [DataMember] public Color InputGridAlternatingBackColor { get; set; }
        [DataMember] public Color MatchingGridBillingBackColor { get; set; }
        [DataMember] public Color MatchingGridReceiptBackColor { get; set; }
        [DataMember] public Color MatchingGridBillingSelectedRowBackColor { get; set; }
        [DataMember] public Color MatchingGridBillingSelectedCellBackColor { get; set; }
        [DataMember] public Color MatchingGridReceiptSelectedRowBackColor { get; set; }
        [DataMember] public Color MatchingGridReceiptSelectedCellBackColor { get; set; }
        [DataMember] public int CreateBy { get; set; }
        [DataMember] public DateTime CreateAt { get; set; }
        [DataMember] public int UpdateBy { get; set; }
        [DataMember] public DateTime UpdateAt { get; set; }
    }

    [DataContract]
    public class ControlColorResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public ControlColor[] Color { get; set; }
    }

    [DataContract]
    public class ControlColorsResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
        [DataMember] public List<ControlColor> Colors { get; set; }
    }
}
