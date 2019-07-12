using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BDStyles = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues;

namespace Rac.VOne.Web.Spreadsheets
{
    /// <summary>
    /// Open XML Spreadsheet 作成用のメソッド群
    /// </summary>
    public static class OpenXmlSpreadsheetExtensions
    {
        /// <summary>シートの保護に利用する パスワードのハッシュを取得</summary>
        /// <param name="raw">パスワード 平文</param>
        /// <returns>パスワードのハッシュ</returns>
        /// <remarks>
        /// password の hash 化 謎 algorithm
        /// https://social.msdn.microsoft.com/Forums/office/en-US/b166e244-d762-4bec-945a-918181ef80ef/password-is-not-working-for-protected-work-sheet
        /// </remarks>
        public static string ToHash(this string raw)
        {
            var bytes = Encoding.ASCII.GetBytes(raw);
            var hash = 0;

            int offset(int val) => ((val >> 14) & 0x01) | ((val << 1) & 0x7fff);
            if (bytes.Length > 0)
            {
                var index = bytes.Length;
                while (index-- > 0)
                {
                    hash = offset(hash);
                    hash ^= bytes[index];
                }
                // main difference from spec, also hash with charcount
                hash = offset(hash);
                hash ^= bytes.Length;
                hash ^= (0x8000 | ('N' << 8) | 'K');
            }
            return Convert.ToString(hash, 16).ToUpperInvariant();
        }

        public static Row GetRow(this SheetData data, uint rowIndex)
        {
            var row = data.Elements<Row>().FirstOrDefault(x => x.RowIndex == rowIndex);
            if (row == null)
            {
                row = new Row { RowIndex = rowIndex };
                data.Append(row);
            }
            return row;
        }

        /// <summary>
        /// <see cref="SheetData"/>から <see cref="Cell"/> を取得
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static Cell GetCell(this SheetData data, uint rowIndex, string columnName)
        {
            var row = data.GetRow(rowIndex);
            var cellReference = $"{columnName.ToUpper()}{rowIndex}";
            var cell = row.Elements<Cell>().FirstOrDefault(x => x.CellReference == cellReference);
            if (cell == null)
            {
                var refCell = row.Elements<Cell>().FirstOrDefault(x
                    => x.CellReference.Value.Length == cellReference.Length &&
                    string.Compare(x.CellReference.Value, cellReference, true) > 0);
                cell = new Cell { CellReference = cellReference };
                row.InsertBefore(cell, refCell);
                (data.Parent as Worksheet)?.Save();
            }
            return cell;
        }

        /// <summary><see cref="SheetData"/>に 文字を設定</summary>
        /// <param name="data"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <param name="text"></param>
        public static void SetText(this SheetData data, uint rowIndex, string columnName, string text, uint? styleIndex = null)
        {
            var worksheet = data.GetWorksheet();
            var bookPart = worksheet.WorksheetPart.GetWorkbookPart();
            var tablePart = bookPart.GetSharedStringTablePart();

            var cell = data.GetCell(rowIndex, columnName);
            cell.DataType = CellValues.SharedString;
            cell.CellValue = new CellValue(tablePart.GetIndex(text).ToString());
            if (styleIndex.HasValue) cell.StyleIndex = styleIndex.Value;
        }

        /// <summary><see cref="SheetData"/>に 数字を設定</summary>
        /// <param name="data"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        public static void SetNumber<T>(this SheetData data, uint rowIndex, string columnName, T value, uint? styleIndex = null) where T : struct
        {
            var cell = data.GetCell(rowIndex, columnName);
            cell.DataType = CellValues.Number;
            cell.CellValue = new CellValue(value.ToString());
            if (styleIndex.HasValue) cell.StyleIndex = styleIndex.Value;
        }

        /// <summary><see cref="SheetData"/>に 数字を設定</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <param name="styleIndex"></param>
        public static void SetNumber<T>(this SheetData data, uint rowIndex, string columnName, T? value, uint? styleIndex = null) where T : struct
        {
            if (value.HasValue)
            {
                data.SetNumber(rowIndex, columnName, value.Value, styleIndex);
            }
            else
            {
                var cell = data.GetCell(rowIndex, columnName);
                if (styleIndex.HasValue) cell.StyleIndex = styleIndex.Value;
            }
        }

        /// <summary><see cref="SheetData"/>に 日付を設定</summary>
        /// <param name="data"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <param name="date"></param>
        public static void SetDate(this SheetData data, uint rowIndex, string columnName, DateTime date, uint? styleIndex = null)
        {
            var cell = data.GetCell(rowIndex, columnName);
            //cell.DataType = CellValues.Date;
            //cell.CellValue = new CellValue(date);
            cell.CellValue = new CellValue(date.ToOADate().ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (styleIndex.HasValue) cell.StyleIndex = styleIndex.Value;
        }

        /// <summary>特定セルに書式設定</summary>
        /// <param name="data"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <param name="styleIndex"></param>
        public static void SetStyle(this SheetData data, uint rowIndex, string columnName, uint styleIndex)
        {
            var cell = data.GetCell(rowIndex, columnName);
            cell.StyleIndex = styleIndex;
        }

        /// <summary>範囲セルに書式設定</summary>
        /// <param name="data"></param>
        /// <param name="reference"></param>
        /// <param name="styleIndex"></param>
        public static void SetStyle(this SheetData data, string reference, uint styleIndex)
        {
            foreach (var r in reference.GetCells())
                data.SetStyle(r.Item1, r.Item2, styleIndex);
        }

        /// <summary>
        /// セルアドレスの範囲指定から、 rowIndex, columnName の列挙型を取得
        /// "A1:C3" -> { { 1, "A" }, { 1, "B" }, { 1, "C" }, ..., { 3, "B" }, { 3, "C" } }
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<uint, string>> GetCells(this string reference)
        {
            if (reference.Contains('!'))
            {
                var refs = reference.Split('!');
                if (refs.Length > 2 ||
                    string.IsNullOrWhiteSpace(refs[0]) ||
                    string.IsNullOrWhiteSpace(refs[1])) throw new ArgumentException();
                reference = refs[1];
            }
            reference = reference.Replace("$", "");
            if (!reference.Contains(':'))
            {
                var rowIndex = reference.GetRowIndex();
                var columnName = reference.GetColumnName();
                yield return new Tuple<uint, string>(rowIndex, columnName);
                yield break;
            }

            {
                var refs = reference.Split(':');
                var start = refs[0];
                var end = refs[1];

                var rowStart = start.GetRowIndex();
                var rowEnd = end.GetRowIndex();
                var colStart = start.GetColumnIndex();
                var colEnd = end.GetColumnIndex();

                for (var row = rowStart; row <= rowEnd; row++)
                    for (var col = colStart; col <= colEnd; col++)
                        yield return new Tuple<uint, string>(row, GetColumnName(col));
            }
        }

        public static Worksheet GetWorksheet(this SheetData data) => data.Parent as Worksheet;
        public static WorkbookPart GetWorkbookPart(this WorksheetPart part) => part.GetParentParts().First() as WorkbookPart;

        public static SharedStringTablePart GetSharedStringTablePart(this WorkbookPart bookPart)
        {
            var tablePart = bookPart.SharedStringTablePart;
            if (tablePart == null)
            {
                tablePart = bookPart.AddNewPart<SharedStringTablePart>();
            }
            return tablePart;
        }
        public static SharedStringTable GetSharedStringTable(this SharedStringTablePart part)
        {
            if (part.SharedStringTable == null)
            {
                part.SharedStringTable = new SharedStringTable();
            }
            return part.SharedStringTable;
        }

        /// <summary><see cref="SharedStringTable"/>から、引数の<see cref="value"/>が該当するindex を取得
        /// テーブルに登録がない場合は自動的に登録</summary>
        /// <param name="part"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetIndex(this SharedStringTablePart part, string value)
        {
            var table = part.GetSharedStringTable();
            var index = 0;
            foreach (var item in table.Elements<SharedStringItem>())
            {
                if (item.InnerText == value) return index;
                index++;
            }
            table.Append(new SharedStringItem(new Text(value)));
            return index;
        }

        private static Regex ColumnPattern => new Regex("[A-Z]+");
        /// <summary>
        /// セルアドレスから、カラム名を取得 "A1" -> "A" など
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        public static string GetColumnName(this string cellName)
        {
            return ColumnPattern.Match(cellName.ToUpper()).Value;
        }

        /// <summary>
        /// カラム名から 1開始の index を取得
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        public static uint GetColumnIndex(this string cellName)
        {
            var columnName = cellName.GetColumnName();
            var columnNumber = -1;
            var multiplier = 1;
            foreach (var c in columnName.Reverse())
            {
                columnNumber += multiplier * ((int)c - 64);
                multiplier *= 26;
            }
            return (uint)columnNumber + 1;
        }

        private static Regex RowPattern => new Regex(@"\d+");

        /// <summary>
        /// セルアドレスから行番号 (rowindex 1から開始 )を取得
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        public static uint GetRowIndex(this string cellName)
        {
            var value = RowPattern.Match(cellName).Value;
            return uint.Parse(value);
        }

        /// <summary>columnIndex から A..ZAA.. などの カラム名を取得</summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        /// <remarks>現状 2桁までしか取得不可</remarks>
        public static string GetColumnName(uint columnIndex)
        {
            const int AlphabetCount = 26;
            const int AlphabetStart = 0x40;
            uint charValue(uint val) => AlphabetStart + val;
            var remainder = (columnIndex - 1) % ((AlphabetCount + 1) * AlphabetCount) + 1;
            var quotient = (remainder - 1) / (AlphabetCount);
            var remaining = (remainder - 1) % AlphabetCount + 1;
            var symbol = string.Empty;
            if (quotient > 0) symbol = Convert.ToChar(charValue(quotient)).ToString();
            symbol += Convert.ToChar(charValue(remaining)).ToString();
            return symbol;
        }
        public static FontSize DefaultFontSize => new FontSize { Val = 9 };
        public static FontSize CaptionFontSize => new FontSize { Val = 7.5 };
        public static FontSize DetailFontSize => new FontSize { Val = 6 };

        public static FontSize TitleFontSize => new FontSize { Val = 18 };
        public static FontName DefaultFontName => new FontName { Val = "ＭＳ 明朝" };
        public static Color SubTextColor => new Color { Rgb = HexBinaryValue.FromString("808080") };
        public static HexBinaryValue CaptionFillColor => HexBinaryValue.FromString("D9D9D9");

        /// <summary>フォントの取得</summary>
        /// <param name="name">フォント名 全角 半角の違いに気を付ける</param>
        /// <param name="size">フォントサイズ</param>
        /// <param name="color">フォントの前景色</param>
        /// <param name="familyNumbering">フォントファミリー
        /// 1:Roman プロポーショナルフォントかつ serif
        /// 2:Swiss プロポーショナルフォントかつ sans-serif
        /// 3:Mordern 等幅フォント
        /// 4:Script 手書き風
        /// 5:Decorative カリグラフィーなどの装飾フォント
        /// 
        /// serif 文字ストロークの端にある小さな飾り
        /// フォント名で適切なフォントを解決できない場合、上記に沿って 代替フォントを決定する
        /// </param>
        /// <param name="charSet">キャラクターセット 128: shift-jis
        /// フォント名で適切なフォントを解決できない場合、上記に沿って 代替フォントを決定する
        /// ※ 日本語の表記可能なフォントが設定されるらしい
        /// </param>
        /// <returns></returns>
        public static Font GetFont(FontName name = null,
            FontSize size = null,
            Color color = null,
            int familyNumbering = 3,
            int charSet = 128,
            UnderlineValues underline = UnderlineValues.None)
        {
            var font = new Font
            {
                FontName            = name ?? DefaultFontName,
                FontSize            = size ?? DefaultFontSize,
                FontFamilyNumbering = new FontFamilyNumbering   { Val = familyNumbering },
                FontCharSet         = new FontCharSet           { Val = charSet },
                Underline           = new Underline             { Val = underline }
            };
            if (color != null) font.Color = color;
            return font;
        }

        public static Border GetBorderAll(BDStyles style) => GetBorder(style, style, style, style);

        /// <summary>
        /// 罫線<see cref="Border"/>の取得
        /// left/right/top/bottom それぞれに 線のスタイルを指定可能
        /// <see cref="BDStyles.None"/> は なし
        /// <see cref="BDStyles.Thin"/> は 実線
        /// <see cref="BDStyles.Hair"/> は 点線（ダッシュとは異なる）
        /// <see cref="BDStyles.Double"/> は 二重線 で、主に利用するのは 前述の３つくらい
        /// <see cref="BDStyles.None"/> を利用した場合、xml に余計なタグを作成しない。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public static Border GetBorder(
            BDStyles left   = BDStyles.Thin,
            BDStyles right  = BDStyles.Thin,
            BDStyles top    = BDStyles.Thin,
            BDStyles bottom = BDStyles.Thin)
        {
            var border = new Border { DiagonalBorder = new DiagonalBorder { }, };
            border.LeftBorder   = left  == BDStyles.None ?
                new LeftBorder() :
                new LeftBorder { Style = left, Color = new Color { Auto = true } };
            border.RightBorder  = right     == BDStyles.None ?
                new RightBorder() :
                new RightBorder { Style = right, Color = new Color { Auto = true } };
            border.TopBorder    = top       == BDStyles.None ?
                new TopBorder() :
                new TopBorder { Style = top, Color = new Color { Auto = true } };
            border.BottomBorder = bottom    == BDStyles.None ?
                new BottomBorder() :
                new BottomBorder { Style = bottom, Color = new Color { Auto = true } };
            return border;
        }

        /// <summary>CellFormat の取得</summary>
        /// <param name="numFmtId">円貨の金額の場合 38 それ以外は、NumberingFormat を追加し、追加したNumberFormatId</param>
        /// <param name="fmtId"></param>
        /// <param name="fontId">FontId</param>
        /// <param name="fillId">FillId</param>
        /// <param name="borderId">BorderId</param>
        /// <param name="horizontal"><see cref="HorizontalAlignmentValues"/> [配置]-[横位置]</param>
        /// <returns></returns>
        public static CellFormat GetCellFormat(
            int numFmtId    = 0,
            int fmtId       = 0,
            int fontId      = 0,
            int fillId      = 0,
            int borderId    = 0,
            HorizontalAlignmentValues horizontal = HorizontalAlignmentValues.General)
        {
            var format = new CellFormat
            {
                NumberFormatId  = (uint)numFmtId,
                FormatId        = (uint)fmtId,
                FontId          = (uint)fontId,
                FillId          = (uint)fillId,
                BorderId        = (uint)borderId,
            };
            if (numFmtId    > 0u) format.ApplyNumberFormat = true;
            if (fillId      > 0u) format.ApplyFill = true;
            if (borderId    > 0u) format.ApplyBorder = true;

            if (horizontal == HorizontalAlignmentValues.General)
            {
                format.AppendChild(new Alignment { Vertical = VerticalAlignmentValues.Center, });
            }
            else
            {
                format.AppendChild(new Alignment
                {
                    Vertical = VerticalAlignmentValues.Center,
                    Horizontal = horizontal,
                });
                format.ApplyAlignment = true;
            }
            return format;
        }

        /// <summary>
        /// <see cref="Stylesheet"/>の初期化処理
        /// </summary>
        /// <param name="part"><see cref="WorkbookStylesPart"/></param>
        /// <param name="initializeNumberingFormats"><see cref="NumberingFormats"/>の定義
        /// カスタム定義は <see cref="NumberingFormat.NumberFormatId"/> を 164 以降で定義する必要あり
        /// 日付、外貨の金額部分などのカスタム書式を定義する場合に利用
        /// 円貨は、built-in で定義されている 38 を利用すれば良い
        /// 未指定 の場合は 何も登録しない
        /// </param>
        /// <param name="initializeFonts"><see cref="Fonts"/>の定義
        /// 未指定の場合は、デフォルトフォントを登録
        /// </param>
        /// <param name="initializeFills"><see cref="Fills"/>の定義
        /// 必ず、None, Gray125 の定義が必要
        /// それ以降に 任意の <see cref="Fill"/>を登録する
        /// 未指定の場合は、 None, Gray125 を登録
        /// </param>
        /// <param name="initializeBorders"><see cref="Borders"/>の定義
        /// 必ず最初に 空の <see cref="Border"/>の指定が必要 <see cref="GetBorderAll(BDStyles)"/>を利用すると楽
        /// left/right/top/bottom の組み合わせで登録する必要があるため、組み合わせが膨大になる可能性がある
        /// </param>
        /// <param name="initializeCellFormats"><see cref="CellFormats"/>の定義
        /// 必ず最初に 空の <see cref="CellFormat"/>の指定が必要 <see cref="GetCellFormat(int, int, int, int, int, HorizontalAlignmentValues)"/>を利用すると楽
        /// font, fill, border, numberingFormatId の組み合わせが異なるごとに定義が必要
        /// index で指定する必要があるため、該当するセルがどのインデックスとなるかを、把握する必要がある</param>
        /// <returns></returns>
        public static Stylesheet InitizalizeStylesheet(this WorkbookStylesPart part,
            Func<NumberingFormat[]> initializeNumberingFormats  = null,
            Func<Font[]>            initializeFonts             = null,
            Func<Fill[]>            initializeFills             = null,
            Func<Border[]>          initializeBorders           = null,
            Func<CellFormat[]>      initializeCellFormats       = null
            )
        {
            var stylesheet = part.Stylesheet;
            if (stylesheet == null)
            {
                stylesheet = (part.Stylesheet = new Stylesheet());
            }

            //var minFormatId = 164u; // Excel "built-in" number-format-id was less than 164;
            if (initializeNumberingFormats != null)
            {
                var numberingFormats = new NumberingFormats();
                numberingFormats.Append(initializeNumberingFormats());
                stylesheet.Append(numberingFormats);
            }


            var fonts = new Fonts { KnownFonts = true };
            fonts.Append(
                initializeFonts?.Invoke() ??
                new[] {
                GetFont(), /* 0: default font */
            });
            fonts.Count = (uint)fonts.ChildElements.Count;
            stylesheet.Append(fonts);

            var fills = new Fills { };
            fills.Append(
                initializeFills?.Invoke() ??
                new[] {
                /* 0 */ new Fill { PatternFill         = new PatternFill { PatternType = PatternValues.None     }, },
                /* 1 */ new Fill { PatternFill         = new PatternFill { PatternType = PatternValues.Gray125  }, },
            });
            fills.Count = (uint)fills.ChildElements.Count;
            stylesheet.Append(fills);


            var borders = new Borders { };
            borders.Append(
                initializeBorders?.Invoke() ??
                new[] {
                /* 0 */ GetBorderAll(BDStyles.None),
            });
            borders.Count = (uint)borders.ChildElements.Count;
            stylesheet.Append(borders);

            var cellStyleFormats = new CellStyleFormats { };
            cellStyleFormats.Append(new[] {
                GetCellFormat(),
                GetCellFormat(22),
            });
            cellStyleFormats.Count = (uint)cellStyleFormats.ChildElements.Count;
            stylesheet.Append(cellStyleFormats);

            var cellFormats = new CellFormats { };
            cellFormats.Append(
                initializeCellFormats?.Invoke() ??
                new[] {
                /* 0 */ GetCellFormat(),
            });
            cellFormats.Count = (uint)cellFormats.ChildElements.Count;
            stylesheet.Append(cellFormats);

            return stylesheet;
        }

        /// <summary>
        /// ページレイアウト取得
        /// </summary>
        /// <param name="paperSize">A4: 9</param>
        /// <returns></returns>
        public static PageSetup GetPageSetup(uint paperSize = 9u) => new PageSetup
        {
            Orientation = OrientationValues.Landscape,
            PaperSize = paperSize,
        };

        /// <summary>
        /// ヘッダーフッターの取得
        /// </summary>
        /// <param name="leftHeader"></param>
        /// <param name="centerHeader"></param>
        /// <param name="rightHeader"></param>
        /// <returns></returns>
        public static HeaderFooter GetHeaderFooter(
            string leftHeader   = null,
            string centerHeader = null,
            string rightHeader  = null)
        {
            var headerText = string.Concat(
                string.IsNullOrEmpty(leftHeader)    ? string.Empty : $"&L{EscapeAmpersand(leftHeader)}",
                string.IsNullOrEmpty(centerHeader)  ? string.Empty : $"&C{EscapeAmpersand(centerHeader)}",
                string.IsNullOrEmpty(rightHeader)   ? string.Empty : $"&R{EscapeAmpersand(rightHeader)}"
                );
            var headerFooter = new HeaderFooter
            {
                OddHeader = new OddHeader { Text = headerText },
                OddFooter = new OddFooter { Text = "&C&P / &N ページ" },
                DifferentOddEven = false,
            };
            return headerFooter;
        }

        /// <summary>
        /// ヘッダーフッターでは、& が特殊文字なので、エスケープ処理が必要
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string EscapeAmpersand(string value) => value.Replace("&", "&&");

        /// <summary>
        /// ページレイアウト 印刷タイトル行の設定
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static DefinedName GetPrintTitlesDefine(this Sheet sheet,
            uint startIndex, uint endIndex) => new DefinedName
            {
                Name = "_xlnm.Print_Titles",
                LocalSheetId = sheet.SheetId - 1,
                Text = $"{sheet.Name}!${startIndex}:${endIndex}",
            };

        /// <summary>
        /// 改ページ の取得
        /// </summary>
        /// <param name="indecis"></param>
        /// <returns></returns>
        public static RowBreaks GetRowBreaks(IEnumerable<uint> indecis)
        {
            var rowBreaks = new RowBreaks();
            rowBreaks.Append(indecis.Select(x => new Break
            {
                Id = x,
                Max = 16383,
                ManualPageBreak = true,
            }).ToArray());
            var count = (uint)indecis.Count();
            rowBreaks.Count = count;
            rowBreaks.ManualBreakCount = count;
            return rowBreaks;
        }

        /// <summary>
        /// 範囲指定のセル参照取得 rowIndexEnd, columnNameEnd のいずれかは指定必須
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <param name="rowIndexEnd"></param>
        /// <param name="columnNameEnd"></param>
        /// <returns></returns>
        public static string GetCellReference(uint rowIndex, string columnName, uint? rowIndexEnd = null, string columnNameEnd = null)
            => $"{columnName}{rowIndex}:{(columnNameEnd ?? columnName)}{(rowIndexEnd ?? rowIndex)}";

        /// <summary>
        /// <see cref="MergeCell"/>の取得 rowIndexEnd, columnNameEnd のいずれかは指定必須
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <param name="rowIndexEnd"></param>
        /// <param name="columnNameEnd"></param>
        /// <returns></returns>
        public static MergeCell GetMergeCell(uint rowIndex, string columnName, uint? rowIndexEnd = null, string columnNameEnd = null)
            => new MergeCell { Reference = GetCellReference(rowIndex, columnName, rowIndexEnd, columnNameEnd) };

        /// <summary>columnName と 次の行とマージする rowIndex の配列から、<see cref="MergeCell"/>の配列を取得</summary>
        /// <param name="columnName"></param>
        /// <param name="rowIndices"></param>
        /// <returns></returns>
        public static MergeCell[] GetNextRowMergeCells(string columnName, IEnumerable<uint> rowIndices)
            => rowIndices.Select(x => GetMergeCell(x, columnName, x + 1)).ToArray();

        /// <summary>
        /// 複数の <see cref="MergeCell"/>の配列を 単一の配列へ集約
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public static MergeCell[] Concat(params IEnumerable<MergeCell>[] cells) => cells.SelectMany(x => x).ToArray();
    }
}
