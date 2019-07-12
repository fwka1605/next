using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Data.Entities
{
    public class ControlColor
    {
        public int CompanyId { get; set; }
        public int LoginUserId { get; set; }
        public int FormBackColor { get; set; }
        public int FormForeColor { get; set; }
        public int ControlEnableBackColor { get; set; }
        public int ControlDisableBackColor { get; set; }
        public int ControlForeColor { get; set; }
        public int ControlRequiredBackColor { get; set; }
        public int ControlActiveBackColor { get; set; }
        public int ButtonBackColor { get; set; }
        public int GridRowBackColor { get; set; }
        public int GridAlternatingRowBackColor { get; set; }
        public int GridLineColor { get; set; }
        public int InputGridBackColor { get; set; }
        public int InputGridAlternatingBackColor { get; set; }
        public int MatchingGridBillingBackColor { get; set; }
        public int MatchingGridReceiptBackColor { get; set; }
        public int MatchingGridBillingSelectedRowBackColor { get; set; }
        public int MatchingGridBillingSelectedCellBackColor { get; set; }
        public int MatchingGridReceiptSelectedRowBackColor { get; set; }
        public int MatchingGridReceiptSelectedCellBackColor { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }

    }
}
