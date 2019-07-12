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
    public class BillingAgingListDocumentProcessor : IProcessor
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
                /*  4 */ GetBorder(                                                                                     ), /* 列見出しC..I  */
                /*  5 */ GetBorder(                                                               bottom: BDStyles.Hair ), /* 列見出しEF1   */
                /*  6 */ GetBorder(                                           top: BDStyles.Hair                        ), /* 列見出しEF2   */
                /*  7 */ GetBorder(                     right: BDStyles.None                                            ), /* 列見出しJ     */
                /*  8 */ GetBorder(left: BDStyles.None, right: BDStyles.None, top: BDStyles.Hair, bottom: BDStyles.Hair ), /* データA */
                /*  9 */ GetBorder(left: BDStyles.None,                       top: BDStyles.Hair, bottom: BDStyles.Hair ), /* データB */
                /* 10 */ GetBorder(                                           top: BDStyles.Hair, bottom: BDStyles.Hair ), /* データC..I */
                /* 11 */ GetBorder(                     right: BDStyles.None, top: BDStyles.Hair, bottom: BDStyles.Hair ), /* データJ */
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
        const uint cfidxHCI     = 4u;
        const uint cfidxHEF1    = 5u;
        const uint cfidxHEF2    = 6u;
        const uint cfidxHJ      = 7u;

        const uint cfidxDA      = 8u;
        const uint cfidxDB      = 9u;
        const uint cfidxDCI     = 10u;
        const uint cfidxDJ      = 11u;

        const uint cfidxSA      = 12u;
        const uint cfidxSB      = 13u;
        const uint cfidxSCI     = 14u;
        const uint cfidxSJ      = 15u;
        #endregion


        private void SetTitle(SheetData data, BillingAgingListSearch option, BillingAgingList item, ref uint rowIndex)
        {
            rowIndex++;

            data.SetText(rowIndex, "A", "請求残高年齢表");
            data.SetStyle(GetCellReference(rowIndex, "A", columnNameEnd: "I"), cfidxTitle);

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
            data.SetText(rowIndex, "C", "前月請求残", cfidxHCI);
            data.SetText(rowIndex, "D", "当月売上高", cfidxHCI);
            data.SetText(rowIndex, "E", "当月消込", cfidxHCI);
            data.SetText(rowIndex, "F", "当月請求残", cfidxHCI);
            data.SetText(rowIndex, "G", option.MonthlyRemain0, cfidxHCI);
            data.SetText(rowIndex, "H", option.MonthlyRemain1, cfidxHCI);
            data.SetText(rowIndex, "I", option.MonthlyRemain2, cfidxHCI);
            data.SetText(rowIndex, "J", option.MonthlyRemain3, cfidxHJ);

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
            data.SetNumber(rowIndex, "C", item.LastMonthRemain      , cfidxDCI);
            data.SetNumber(rowIndex, "D", item.CurrentMonthSales    , cfidxDCI);
            data.SetNumber(rowIndex, "E", item.CurrentMonthMatching , cfidxDCI);
            data.SetNumber(rowIndex, "F", item.CurrentMonthRemain   , cfidxDCI);
            data.SetNumber(rowIndex, "G", item.MonthlyRemain0       , cfidxDCI);
            data.SetNumber(rowIndex, "H", item.MonthlyRemain1       , cfidxDCI);
            data.SetNumber(rowIndex, "I", item.MonthlyRemain2       , cfidxDCI);
            data.SetNumber(rowIndex, "J", item.MonthlyRemain3       , cfidxDJ);

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
            data.SetNumber(rowIndex, "C", item.LastMonthRemain          , cfidxSCI);
            data.SetNumber(rowIndex, "D", item.CurrentMonthSales        , cfidxSCI);
            data.SetNumber(rowIndex, "E", item.CurrentMonthMatching     , cfidxSCI);
            data.SetNumber(rowIndex, "F", item.CurrentMonthRemain       , cfidxSCI);
            data.SetNumber(rowIndex, "G", item.MonthlyRemain0           , cfidxSCI);
            data.SetNumber(rowIndex, "H", item.MonthlyRemain1           , cfidxSCI);
            data.SetNumber(rowIndex, "I", item.MonthlyRemain2           , cfidxSCI);
            data.SetNumber(rowIndex, "J", item.MonthlyRemain3           , cfidxSJ);

        }

        private bool RequireBreak(BillingAgingListSearch opt, BillingAgingList item, BillingAgingList itemNext, uint rowIndex, uint lastBraekRowIndex)
        {
            const uint MaxRowCount = 44u;
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
                    new Column { Min =  1, Max =  1, Width = 43.13, CustomWidth = true },
                    new Column { Min =  2, Max =  2, Width =  5.71, CustomWidth = true },
                    new Column { Min =  3, Max = 10, Width = 12.85, CustomWidth = true },
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
                    SetTitle(data, Option, item, ref rowIndex);
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
