using Rac.VOne.Client;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common
{
    public class ColorSetting : IColors
    {
        public static IColors Default { get; } = new ColorSetting()
        {
            FormBackColor = Color.White,
            FormForeColor = SystemColors.WindowText,

            ControlEnableBackColor = SystemColors.Window,
            ControlDisableBackColor = SystemColors.Control,
            ControlForeColor = SystemColors.WindowText,
            ControlRequiredBackColor = Color.LightCyan,
            ControlActiveBackColor = Color.Khaki,
            ButtonBackColor = SystemColors.Control,

            GridRowBackColor = Color.White,
            GridAlternatingRowBackColor = Color.MintCream,
            GridLineColor = SystemColors.WindowText,

            InputGridBackColor = SystemColors.Window,
            InputGridAlternatingBackColor = Color.MintCream,

            MatchingGridBillingBackColor = Color.LightCyan,
            MatchingGridReceiptBackColor = Color.NavajoWhite,

            MatchingGridBillingSelectedRowBackColor = Color.Azure,
            MatchingGridBillingSelectedCellBackColor = Color.Cyan,
            MatchingGridReceiptSelectedRowBackColor = Color.Honeydew,
            MatchingGridReceiptSelectedCellBackColor = Color.PaleGreen,

            CollationDupedReceiptCellBackColor = Color.Salmon,
        };

        public static IColors FromDb { private get; set; }
        public static IColors FromUser { private get; set; }
        public static IColors Current
        {
            get { return FromUser ?? FromDb ?? Default; }
        }
        public static void ClearLoadedColors()
        {
            FromUser    = null;
            FromDb      = null;
        }

        public ColorSetting()
        {
        }

        public ColorSetting(ControlColor controlColor)
        {
            if (controlColor != null)
            {
                FormBackColor = controlColor.FormBackColor;
                FormForeColor = controlColor.FormForeColor;
                ControlEnableBackColor = controlColor.ControlEnableBackColor;
                ControlDisableBackColor = controlColor.ControlDisableBackColor;
                ControlForeColor = controlColor.ControlForeColor;
                ControlRequiredBackColor = controlColor.ControlRequiredBackColor;
                ControlActiveBackColor = controlColor.ControlActiveBackColor;
                ButtonBackColor = controlColor.ButtonBackColor;
                GridRowBackColor = controlColor.GridRowBackColor;
                GridAlternatingRowBackColor = controlColor.GridAlternatingRowBackColor;
                GridLineColor = controlColor.GridLineColor;
                InputGridBackColor = controlColor.InputGridBackColor;
                InputGridAlternatingBackColor = controlColor.InputGridAlternatingBackColor;
                MatchingGridBillingBackColor = controlColor.MatchingGridBillingBackColor;
                MatchingGridReceiptBackColor = controlColor.MatchingGridReceiptBackColor;
                MatchingGridBillingSelectedRowBackColor = controlColor.MatchingGridBillingSelectedRowBackColor;
                MatchingGridBillingSelectedCellBackColor = controlColor.MatchingGridBillingSelectedCellBackColor;
                MatchingGridReceiptSelectedRowBackColor = controlColor.MatchingGridReceiptSelectedRowBackColor;
                MatchingGridReceiptSelectedCellBackColor = controlColor.MatchingGridReceiptSelectedCellBackColor;
            }
        }

        public Color FormBackColor { get; set; }
        public Color FormForeColor { get; set; }
        public Color ControlEnableBackColor { get; set; }
        public Color ControlDisableBackColor { get; set; }
        public Color ControlForeColor { get; set; }
        public Color ControlRequiredBackColor { get; set; }
        public Color ControlActiveBackColor { get; set; }
        public Color ButtonBackColor { get; set; }
        public Color GridRowBackColor { get; set; }
        public Color GridAlternatingRowBackColor { get; set; }
        public Color GridLineColor { get; set; }
        public Color InputGridBackColor { get; set; }
        public Color InputGridAlternatingBackColor { get; set; }
        public Color MatchingGridBillingBackColor { get; set; }
        public Color MatchingGridReceiptBackColor { get; set; }
        public Color MatchingGridBillingSelectedRowBackColor { get; set; }
        public Color MatchingGridBillingSelectedCellBackColor { get; set; }
        public Color MatchingGridReceiptSelectedRowBackColor { get; set; }
        public Color MatchingGridReceiptSelectedCellBackColor { get; set; }

        public Color CollationDupedReceiptCellBackColor { get; set; } = Color.Salmon;
    }
}
