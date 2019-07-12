using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client
{
    public interface IColors
    {
        Color FormBackColor { get; set; }
        Color FormForeColor { get; set; }
        Color ControlEnableBackColor { get; set; }
        Color ControlDisableBackColor { get; set; }
        Color ControlForeColor { get; set; }
        Color ControlRequiredBackColor { get; set; }
        Color ControlActiveBackColor { get; set; }
        Color ButtonBackColor { get; set; }
        Color GridRowBackColor { get; set; }
        Color GridAlternatingRowBackColor { get; set; }
        Color GridLineColor { get; set; }
        Color InputGridBackColor { get; set; }
        Color InputGridAlternatingBackColor { get; set; }
        Color MatchingGridBillingBackColor { get; set; }
        Color MatchingGridReceiptBackColor { get; set; }
        Color MatchingGridBillingSelectedRowBackColor { get; set; }
        Color MatchingGridBillingSelectedCellBackColor { get; set; }
        Color MatchingGridReceiptSelectedRowBackColor { get; set; }
        Color MatchingGridReceiptSelectedCellBackColor { get; set; }

        Color CollationDupedReceiptCellBackColor { get; set; }
    }
}
