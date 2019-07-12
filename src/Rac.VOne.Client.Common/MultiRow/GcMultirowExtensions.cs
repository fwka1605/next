using GrapeCity.Win.MultiRow;
using Rac.VOne.Client;
using Rac.VOne.Client.Common.Controls;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Common.MultiRow
{
    /// <summary>
    ///  グリッド背景色設定
    /// </summary>
    public enum GridColorType
    {
        /// <summary>データ検索などの一般的なもの 交互色設定</summary>
        General,
        /// <summary>データ入力などの特殊なもの</summary>
        Input,
        /// <summary>個別に背景色を設定するパターン 交互色/入力用の色設定を行わない</summary>
        Special
    }

    public static class GcMultirowExtensions
    {
        public static int GetWidth(GrapeCity.Win.MultiRow.Row row)
        {
            return (row != null && row.Cells.Any() ? row.Cells.Select(c => c.Width).Sum() : 0);
        }

        public static void SetRowColor(this VOneGridControl grid, IColors colors, GridColorType type = GridColorType.General)
        {
            switch (type)
            {
                case GridColorType.General:
                    grid.RowsDefaultCellStyle.BackColor = colors.GridRowBackColor;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = colors.GridAlternatingRowBackColor;
                    break;

                case GridColorType.Input:
                    grid.RowsDefaultCellStyle.BackColor = colors.InputGridBackColor;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = colors.InputGridAlternatingBackColor;
                    break;
                default:
                    break;
            }
        }

        public static Border GridLine(this IColors colors)
        {
            return new Border
            {
                All = new GrapeCity.Win.MultiRow.Line
                {
                    Color = colors.GridLineColor,
                    Style = LineStyle.Thin,
                },
            };
        }
    }
}
