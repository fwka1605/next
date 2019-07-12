using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using static Rac.VOne.Web.Spreadsheets.OpenXmlSpreadsheetExtensions;
using BDStyles = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues;

namespace Rac.VOne.Web.Spreadsheets.Processors
{
    public class ArrearagesListSummaryDocumentProcessor : IProcessor
    {

        internal Company Company { private get; set; }
        internal List<ArrearagesList> Items { private get; set; }
        internal int Precision { private get; set; }
        internal string Password { private get; set; }

        private NumberingFormat[] InitializeNumeringFormats(int precision)
        {
            if (precision == 0) return null;
            var code = string.Concat("#,##0.", new string('0', precision));
            return new[] {
                new NumberingFormat { NumberFormatId = 164, FormatCode = code }
            };
        }

        private Font[] InitializeFonts()
        {
            return new[] {
                /* 0 */ GetFont(), /* default font */
                /* 1 */ GetFont(), /* normal font */
                /* 2 */ GetFont(size: TitleFontSize, underline: UnderlineValues.Single), /* title font */
            };
        }

        private Fill[] InitializeFills()
        {
            return new[] {
                /* 0 */ new Fill { PatternFill         = new PatternFill { PatternType = PatternValues.None     }, },
                /* 1 */ new Fill { PatternFill         = new PatternFill { PatternType = PatternValues.Gray125  }, },
                /* 2 */ new Fill { PatternFill         = new PatternFill {
                            PatternType         = PatternValues.Solid,
                            ForegroundColor     = new ForegroundColor { Rgb = CaptionFillColor },
                            BackgroundColor     = new BackgroundColor { Indexed = 64 },
                        },
                    }
            };
        }

        private Border[] InitializeBorders()
        {
            return new[] {
                /*  0 */ GetBorderAll(BDStyles.None),
                /*  1 */ GetBorder(),
                /*  2 */ GetBorder(left: BDStyles.None,     bottom: BDStyles.None), /* 列見出しA1    */
                /*  3 */ GetBorder(left: BDStyles.None,     top:    BDStyles.None), /* 列見出しA2    */
                /*  4 */ GetBorder(                         bottom: BDStyles.None), /* 列見出しB..C1 */
                /*  5 */ GetBorder(                         top:    BDStyles.None), /* 列見出しB..C2 */
                /*  6 */ GetBorder(right: BDStyles.None,    bottom: BDStyles.None), /* 列見出しD1    */
                /*  7 */ GetBorder(right: BDStyles.None,    top:    BDStyles.None), /* 列見出しD2    */
                /*  8 */ GetBorder(left: BDStyles.None,     top: BDStyles.Hair, bottom: BDStyles.None), /* データA1 */
                /*  9 */ GetBorder(left: BDStyles.None,     top: BDStyles.None, bottom: BDStyles.Hair), /* データA2 */
                /* 10 */ GetBorder(                         top: BDStyles.Hair, bottom: BDStyles.None), /* データB..C1 */
                /* 11 */ GetBorder(                         top: BDStyles.None, bottom: BDStyles.Hair), /* データB..C2 */
                /* 12 */ GetBorder(right: BDStyles.None,    top: BDStyles.Hair, bottom: BDStyles.None), /* データD1 */
                /* 13 */ GetBorder(right: BDStyles.None,    top: BDStyles.None, bottom: BDStyles.Hair), /* データD2 */
                /* 14 */ GetBorder(left: BDStyles.None, right: BDStyles.None,   bottom: BDStyles.None), /* 合計行D1    */
                /* 15 */ GetBorder(left: BDStyles.None, right: BDStyles.None,   top:    BDStyles.None), /* 合計行D2    */
            };
        }

        #region cellformat

        private CellFormat[] InitializeCellFormats(int decNumFmt)
        {
            return new[] {
                /*  0 */ GetCellFormat(),
                /*  1 */ GetCellFormat(fontId: 2, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  2 */ GetCellFormat(fillId: 2, borderId: 2, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  3 */ GetCellFormat(fillId: 2, borderId: 3, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  4 */ GetCellFormat(fillId: 2, borderId: 4, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  5 */ GetCellFormat(fillId: 2, borderId: 5, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  6 */ GetCellFormat(fillId: 2, borderId: 6, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  7 */ GetCellFormat(fillId: 2, borderId: 7, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  8 */ GetCellFormat(borderId:  8),
                /*  9 */ GetCellFormat(borderId:  9),
                /* 10 */ GetCellFormat(borderId: 10, numFmtId: decNumFmt, horizontal: HorizontalAlignmentValues.Right),
                /* 11 */ GetCellFormat(borderId: 11),
                /* 12 */ GetCellFormat(borderId: 10),
                /* 13 */ GetCellFormat(borderId: 12),
                /* 14 */ GetCellFormat(borderId: 13),
                /* 15 */ GetCellFormat(fillId: 2, borderId:  2),
                /* 16 */ GetCellFormat(fillId: 2, borderId:  3),
                /* 17 */ GetCellFormat(fillId: 2, borderId:  4, numFmtId: decNumFmt, horizontal: HorizontalAlignmentValues.Right),
                /* 18 */ GetCellFormat(fillId: 2, borderId:  5),
                /* 19 */ GetCellFormat(fillId: 2, borderId:  6),
                /* 20 */ GetCellFormat(fillId: 2, borderId:  7),
                /* 21 */ GetCellFormat(fillId: 2, borderId: 14),
                /* 22 */ GetCellFormat(fillId: 2, borderId: 15),
            };
        }

        const uint cfidxTitle   = 1u;
        const uint cfidxHA1     = 2u;
        const uint cfidxHA2     = 3u;
        const uint cfidxHBC1    = 4u;
        const uint cfidxHBC2    = 5u;
        const uint cfidxHD1     = 6u;
        const uint cfidxHD2     = 7u;

        const uint cfidxDA1     = 8u;
        const uint cfidxDA2     = 9u;
        const uint cfidxDB1     = 10u;
        const uint cfidxDBC2    = 11u;
        const uint cfidxDC1     = 12u;
        const uint cfidxDD1     = 13u;
        const uint cfidxDD2     = 14u;

        const uint cfidxSA1     = 15u;
        const uint cfidxSA2     = 16u;
        const uint cfidxSB1     = 17u;
        const uint cfidxSB2     = 18u;
        const uint cfidxSC1     = 19u;
        const uint cfidxSC2     = 20u;
        const uint cfidxSD1     = 21u;
        const uint cfidxSD2     = 22u;

        #endregion

        public void Process(SpreadsheetDocument document)
        {
            var workbookPart    = document.AddWorkbookPart();
            var worksheetPart   = workbookPart.AddNewPart<WorksheetPart>();
            var workbook        = (workbookPart.Workbook = new Workbook());
            var worksheet       = (worksheetPart.Worksheet = new Worksheet());
            var bookstylesPart  = workbookPart.AddNewPart<WorkbookStylesPart>();
            var title = "滞留明細一覧";

            var sheets = workbook.AppendChild(new Sheets());
            var sheet = new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = title,
            };
            sheets.Append(sheet);

            var decNumFmt = Precision == 0 ? 38 : 164;

            var stylesheet = bookstylesPart.InitizalizeStylesheet(
                initializeNumberingFormats: () => InitializeNumeringFormats(Precision),
                initializeFonts:            () => InitializeFonts(),
                initializeFills:            () => InitializeFills(),
                initializeBorders:          () => InitializeBorders(),
                initializeCellFormats:      () => InitializeCellFormats(decNumFmt)
            );

            var columns = new Columns();
            columns.Append(new[] {
                    new Column { Min = 1, Max = 1, Width = 41.14, CustomWidth = true },
                    new Column { Min = 2, Max = 2, Width = 30   , CustomWidth = true },
                    new Column { Min = 3, Max = 3, Width = 42.14, CustomWidth = true },
                    new Column { Min = 4, Max = 4, Width = 43.85, CustomWidth = true },
                });
            worksheet.Append(columns);

            var data = new SheetData();
            worksheet.Append(data);


            if (!string.IsNullOrEmpty(Password))
                worksheet.Append(new SheetProtection {
                    Password    = Password.ToHash(),
                    Sheet       = true,
                    Scenarios   = true,
                });

            var rowIndex = 1u;
            var bIndices = new List<uint>();

            data.SetText(rowIndex, "A", title);
            data.SetStyle("A1:D1", cfidxTitle);

            rowIndex++;
            rowIndex++;

            data.SetText(rowIndex, "A", "得意先コード"   , cfidxHA1);
            data.SetText(rowIndex, "B", "回収予定額"     , cfidxHBC1);
            data.SetText(rowIndex, "C", "請求部門コード" , cfidxHBC1);
            data.SetText(rowIndex, "D", "担当者コード"   , cfidxHD1);
            bIndices.Add(rowIndex);

            rowIndex++;
            data.SetText(rowIndex, "A", "得意先名"       , cfidxHA2);
            data.SetStyle(rowIndex, "B", cfidxHBC2);
            data.SetText(rowIndex, "C", "請求部門名"     , cfidxHBC2);
            data.SetText(rowIndex, "D", "担当者名"       , cfidxHD2);

            var endIndex = rowIndex;

            var totalAmount = 0M;
            foreach (var item in Items)
            {
                rowIndex++;

                data.SetText(rowIndex, "A", item.CustomerCode, cfidxDA1);
                data.SetNumber(rowIndex, "B", item.RemainAmount, cfidxDB1);
                data.SetText(rowIndex, "C", item.DepartmentCode, cfidxDC1);
                data.SetText(rowIndex, "D", item.StaffCode, cfidxDD1);
                bIndices.Add(rowIndex);
                totalAmount += item.RemainAmount;

                rowIndex++;

                data.SetText(rowIndex, "A", item.CustomerName, cfidxDA2);
                data.SetStyle(rowIndex, "B", cfidxDBC2);
                data.SetText(rowIndex, "C", item.DepartmentName, cfidxDBC2);
                data.SetText(rowIndex, "D", item.StaffName, cfidxDD2);
            }

            rowIndex++;
            // 合計行
            data.SetText(rowIndex, "A", "　総合計", cfidxSA1);
            data.SetNumber(rowIndex, "B", totalAmount, cfidxSB1);
            data.SetStyle(rowIndex, "C", cfidxSC1);
            data.SetStyle(rowIndex, "D", cfidxSD1);
            bIndices.Add(rowIndex);
            var totalCaptionIndex = rowIndex;

            rowIndex++;
            data.SetStyle(rowIndex, "A", cfidxSA2);
            data.SetStyle(rowIndex, "B", cfidxSB2);
            data.SetStyle(rowIndex, "C", cfidxSC2);
            data.SetStyle(rowIndex, "D", cfidxSD2);


            var mergeCells = new MergeCells();
            mergeCells.Append(Concat(
                GetNextRowMergeCells("A", new[] { totalCaptionIndex }),
                GetNextRowMergeCells("B", bIndices)
                ));
            worksheet.Append(mergeCells);

            worksheet.Append(GetPageSetup());

            var companyInfo = $"{Company.Code}：{Company.Name}";
            var outputInfo = $"出力日時：{DateTime.Today:yyyy年MM月dd日}";
            worksheet.Append(GetHeaderFooter(leftHeader: companyInfo, rightHeader: outputInfo));

            var definedNames = (workbook.DefinedNames = new DefinedNames());
            definedNames.Append(sheet.GetPrintTitlesDefine(1u, endIndex));

            workbook.Save();
        }
    }
}
