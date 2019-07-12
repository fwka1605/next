using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Rac.VOne.Web.Spreadsheets.OpenXmlSpreadsheetExtensions;
using BDStyles = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues;

namespace Rac.VOne.Web.Spreadsheets.Processors
{
    public class BillingAgingListReceiptDocumentProcessor : IProcessor
    {
        internal Company Company { private get; set; }
        internal List<BillingAgingList> Items { private get; set; }
        internal BillingAgingListSearch Option { private get; set; }
        internal bool DisplayCustomerCode { private get; set; }
        internal bool UseForeignCurrency { private get; set; }
        internal int Precision { private get; set; }
        internal string Password { private get; set; }

        private NumberingFormat[] InitializeNumeringFormats(int precision)
        {
            var code = string.Concat("#,##0.", new string('0', precision));
            return new[] {
                new NumberingFormat { NumberFormatId = 164, FormatCode = code },
            };
        }

        private Font[] InitializeFonts()
        {
            return new[] {
                /* 0 */ GetFont(), /* default font */
                /* 1 */ GetFont(size: DetailFontSize), /* normal font */
                /* 2 */ GetFont(size: TitleFontSize, underline: UnderlineValues.Single), /* title font */
                /* 3 */ GetFont(size: CaptionFontSize),
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
                /*  2 */ GetBorder(left: BDStyles.None, right: BDStyles.None                                            ), /* 列見出しA     */
                /*  3 */ GetBorder(left: BDStyles.None                                                                  ), /* 列見出しB     */
                /*  4 */ GetBorder(                                                                                     ), /* 列見出しC..J  */
                /*  5 */ GetBorder(                                                               bottom: BDStyles.Hair ), /* 列見出しEF1   */
                /*  6 */ GetBorder(                                           top: BDStyles.Hair                        ), /* 列見出しEF2   */
                /*  7 */ GetBorder(                     right: BDStyles.None                                            ), /* 列見出しK     */
                /*  8 */ GetBorder(left: BDStyles.None, right: BDStyles.None, top: BDStyles.Hair, bottom: BDStyles.Hair ), /* データA */
                /*  9 */ GetBorder(left: BDStyles.None,                       top: BDStyles.Hair, bottom: BDStyles.Hair ), /* データB */
                /* 10 */ GetBorder(                                           top: BDStyles.Hair, bottom: BDStyles.Hair ), /* データC..J */
                /* 11 */ GetBorder(                     right: BDStyles.None, top: BDStyles.Hair, bottom: BDStyles.Hair ), /* データK */
            };
        }

        #region cellformat
        private CellFormat[] InitializeCellFormats(int decNumFmt)
        {
            return new[] {
                /*  0 */ GetCellFormat(),
                /*  1 */ GetCellFormat(fontId: 2, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  2 */ GetCellFormat(fontId: 3, fillId: 2, borderId: 2, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  3 */ GetCellFormat(fontId: 3, fillId: 2, borderId: 3, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  4 */ GetCellFormat(fontId: 3, fillId: 2, borderId: 4, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  5 */ GetCellFormat(fontId: 3, fillId: 2, borderId: 5, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  6 */ GetCellFormat(fontId: 3, fillId: 2, borderId: 6, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  7 */ GetCellFormat(fontId: 3, fillId: 2, borderId: 7, horizontal: HorizontalAlignmentValues.CenterContinuous),
                /*  8 */ GetCellFormat(fontId: 1, borderId:  8),
                /*  9 */ GetCellFormat(fontId: 1, borderId:  9, horizontal: HorizontalAlignmentValues.Right),
                /* 10 */ GetCellFormat(fontId: 1, borderId: 10, numFmtId: decNumFmt, horizontal: HorizontalAlignmentValues.Right),
                /* 11 */ GetCellFormat(fontId: 1, borderId: 11, numFmtId: decNumFmt, horizontal: HorizontalAlignmentValues.Right),
                /* 12 */ GetCellFormat(fontId: 3, fillId: 2, borderId: 2),
                /* 13 */ GetCellFormat(fontId: 3, fillId: 2, borderId: 3),
                /* 14 */ GetCellFormat(fontId: 1, fillId: 2, borderId: 4, numFmtId: decNumFmt, horizontal: HorizontalAlignmentValues.Right),
                /* 15 */ GetCellFormat(fontId: 1, fillId: 2, borderId: 7, numFmtId: decNumFmt, horizontal: HorizontalAlignmentValues.Right),
            };
        }
        const uint cfidxTitle   = 1u;
        const uint cfidxHA      = 2u;
        const uint cfidxHB      = 3u;
        const uint cfidxHCJ     = 4u;
        const uint cfidxHEF1    = 5u;
        const uint cfidxHEF2    = 6u;
        const uint cfidxHK      = 7u;

        const uint cfidxDA      = 8u;
        const uint cfidxDB      = 9u;
        const uint cfidxDCJ     = 10u;
        const uint cfidxDK      = 11u;
        const uint cfidxSA      = 12u;
        const uint cfidxSB      = 13u;
        const uint cfidxSCJ     = 14u;
        const uint cfidxSK      = 15u;
        #endregion


        private void SetTitle(SheetData data, BillingAgingListSearch option, BillingAgingList item, ref uint rowIndex, List<uint> indecis)
        {
            rowIndex++;

            data.SetText(rowIndex, "A", "請求残高年齢表");
            data.SetStyle(GetCellReference(rowIndex, "A", columnNameEnd: "K"), cfidxTitle);

            if (option.RequireDepartmentSubtotal)
            {
                rowIndex++;
                data.SetText(rowIndex, "A", $"請求部門コード：{item.DepartmentCode} {item.DepartmentName}");
            }

            if (option.RequireStaffSubtotal)
            {
                rowIndex++;
                data.SetText(rowIndex, "A", $"担当者コード　：{item.StaffCode} {item.StaffName}");
            }

            rowIndex++;
            rowIndex++;

            data.SetText(rowIndex, "A", "得意先", cfidxHA);
            data.SetStyle(rowIndex, "B", cfidxHB);
            data.SetText(rowIndex, "C", "前月請求残", cfidxHCJ);
            data.SetText(rowIndex, "D", "当月売上高", cfidxHCJ);
            data.SetText(rowIndex, "E", "当月入金", cfidxHEF1);
            data.SetStyle(rowIndex, "F", cfidxHEF1);
            data.SetText(rowIndex, "G", "当月請求残", cfidxHCJ);
            data.SetText(rowIndex, "H", option.MonthlyRemain0, cfidxHCJ);
            data.SetText(rowIndex, "I", option.MonthlyRemain1, cfidxHCJ);
            data.SetText(rowIndex, "J", option.MonthlyRemain2, cfidxHCJ);
            data.SetText(rowIndex, "K", option.MonthlyRemain3, cfidxHK);

            indecis.Add(rowIndex);

            rowIndex++;
            data.SetStyle(rowIndex, "A", cfidxHA);
            data.SetStyle(rowIndex, "B", cfidxHB);
            data.SetStyle(GetCellReference(rowIndex, "C", columnNameEnd: "D"), cfidxHCJ);
            data.SetText(rowIndex, "E", "当月入金", cfidxHEF2);
            data.SetText(rowIndex, "F", "当月消込", cfidxHEF2);
            data.SetStyle(GetCellReference(rowIndex, "G", columnNameEnd: "J"), cfidxHCJ);
            data.SetStyle(rowIndex, "K", cfidxHK);
        }

        private void SetDetail(SheetData data, BillingAgingListSearch option, BillingAgingList item, ref uint rowIndex)
        {
            rowIndex++;
            var isChildCustomer = item.ParentCustomerFlag != 1 && item.ParentCustomerId.HasValue && option.ConsiderCustomerGroup;
            var isParentCustomer = item.ParentCustomerFlag == 1 && option.ConsiderCustomerGroup;
            var customerInfo = string.Concat(
                isChildCustomer ? "　" : "",
                DisplayCustomerCode ? $"{item.CustomerCode} " : "",
                item.CustomerName);

            data.SetText(rowIndex, "A", customerInfo, cfidxDA);

            var caption = isParentCustomer ? "小計" : "";
            data.SetText(rowIndex, "B", caption, cfidxDB);
            data.SetNumber(rowIndex, "C", item.LastMonthRemain      , cfidxDCJ);
            data.SetNumber(rowIndex, "D", item.CurrentMonthSales    , cfidxDCJ);
            data.SetNumber(rowIndex, "E", item.CurrentMonthReceipt  , cfidxDCJ);
            data.SetNumber(rowIndex, "F", item.CurrentMonthMatching , cfidxDCJ);
            data.SetNumber(rowIndex, "G", item.CurrentMonthRemain   , cfidxDCJ);
            data.SetNumber(rowIndex, "H", item.MonthlyRemain0       , cfidxDCJ);
            data.SetNumber(rowIndex, "I", item.MonthlyRemain1       , cfidxDCJ);
            data.SetNumber(rowIndex, "J", item.MonthlyRemain2       , cfidxDCJ);
            data.SetNumber(rowIndex, "K", item.MonthlyRemain3       , cfidxDK);

        }

        private void SetSubtotal(SheetData data, BillingAgingListSearch opt, BillingAgingList item, ref uint rowIndex)
        {
            rowIndex++;
            // 合計行
            var caption
                = item.RecordType == 1 ? $"{item.StaffCode} {item.StaffName} 計"
                : item.RecordType == 2 ? $"{item.DepartmentCode} {item.DepartmentName} 計"
                : item.RecordType == 3 ? (UseForeignCurrency ? "通貨計" : "総合計")
                : string.Empty;
            data.SetText(rowIndex, "A", caption, cfidxSA);
            data.SetStyle(rowIndex, "B", cfidxSB);
            data.SetNumber(rowIndex, "C", item.LastMonthRemain          , cfidxSCJ);
            data.SetNumber(rowIndex, "D", item.CurrentMonthSales        , cfidxSCJ);
            data.SetNumber(rowIndex, "E", item.CurrentMonthReceipt      , cfidxSCJ);
            data.SetNumber(rowIndex, "F", item.CurrentMonthMatching     , cfidxSCJ);
            data.SetNumber(rowIndex, "G", item.CurrentMonthRemain       , cfidxSCJ);
            data.SetNumber(rowIndex, "H", item.MonthlyRemain0           , cfidxSCJ);
            data.SetNumber(rowIndex, "I", item.MonthlyRemain1           , cfidxSCJ);
            data.SetNumber(rowIndex, "J", item.MonthlyRemain2           , cfidxSCJ);
            data.SetNumber(rowIndex, "K", item.MonthlyRemain3           , cfidxSK);

        }

        private bool RequireBreak(BillingAgingListSearch opt, BillingAgingList item, BillingAgingList itemNext, uint rowIndex, uint lastBraekRowIndex)
        {
            const uint MaxRowCount = 46u;
            return itemNext != null
                && (
                    opt.RequireStaffSubtotal && item.RecordType == 1 && itemNext.RecordType == 0
                ||  opt.RequireDepartmentSubtotal && item.RecordType == 2 && itemNext.RecordType == 0
                ||  (rowIndex - lastBraekRowIndex) >= MaxRowCount
                );
        }

        public void Process(SpreadsheetDocument document)
        {
            Option.InitializeYearMonthConditions();

            var workbookPart    = document.AddWorkbookPart();
            var worksheetPart   = workbookPart.AddNewPart<WorksheetPart>();
            var workbook        = (workbookPart.Workbook = new Workbook());
            var worksheet       = (worksheetPart.Worksheet = new Worksheet());
            var bookstylesPart  = workbookPart.AddNewPart<WorkbookStylesPart>();

            var title = "請求残高年齢表";

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
                    new Column { Min =  1, Max =  1, Width = 30.28, CustomWidth = true },
                    new Column { Min =  2, Max =  2, Width =  5.71, CustomWidth = true },
                    new Column { Min =  3, Max = 11, Width = 12.85, CustomWidth = true },
                });
            worksheet.Append(columns);

            var data = new SheetData();
            worksheet.Append(data);

            if (!string.IsNullOrEmpty(Password))
            {
                worksheet.Append(new SheetProtection {
                    Password    = Password.ToHash(),
                    Sheet       = true,
                    Scenarios   = true,
                });
            }

            var rowIndex = 0u;
            var rowspanIndices = new List<uint>();
            var rowbreakIndices = new List<uint>();
            var lastBreakRowIndex = 0u;

            BillingAgingList itemNext = null;
            foreach (var item in Items)
            {
                var index = Items.IndexOf(item);
                var indexNext = index + 1;
                itemNext = indexNext < Items.Count ? Items[indexNext] : null;

                var requireBreak = RequireBreak(Option, item, itemNext, rowIndex, lastBreakRowIndex);
                if (lastBreakRowIndex == rowIndex)
                {
                    SetTitle(data, Option, item, ref rowIndex, rowspanIndices);
                }

                if (item.RecordType == 0)
                {
                    SetDetail(data, Option, item, ref rowIndex);
                }
                else
                {
                    SetSubtotal(data, Option, item, ref rowIndex);
                }
                if (requireBreak)
                {
                    rowbreakIndices.Add(rowIndex);
                    lastBreakRowIndex = rowIndex;
                }
            }

            if (rowspanIndices.Any())
            {
                var mergeCells = new MergeCells();
                mergeCells.Append(Concat(
                    rowspanIndices.Select(x => GetMergeCell(x, "A", x + 1, "B")),
                    (new[] { "C", "D", "G", "H", "I", "J", "K" }).SelectMany(c => GetNextRowMergeCells(c, rowspanIndices))
                    ));
                worksheet.Append(mergeCells);
            }

            worksheet.Append(GetPageSetup());

            var companyInfo = $"{Company.Code}：{Company.Name}";
            var outputInfo = $"出力日時：{DateTime.Today:yyyy年MM月dd日}";
            worksheet.Append(GetHeaderFooter(leftHeader: companyInfo, rightHeader: outputInfo));

            if (rowbreakIndices.Any())
            {
                worksheet.Append(GetRowBreaks(rowbreakIndices));
            }

            workbook.Save();
        }
    }
}
