using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Rac.VOne.Client.Reports.Settings.PF0401;
using static Rac.VOne.Web.Spreadsheets.OpenXmlSpreadsheetExtensions;
using BDStyles = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues;

namespace Rac.VOne.Web.Spreadsheets.Processors
{
    public class ArrearagesListDocumentProcessor : IProcessor
    {
        internal Company Company { private get; set; }
        internal List<ArrearagesList> Items { private get; set; }
        internal ArrearagesListSearch Option { private get; set; }
        internal int Precision { private get; set; }
        internal string Note1 { private get; set; } = "備考1";
        internal string Password { private get; set; }

        private bool RequireDepartmentSubtotal(ArrearagesListSearch opt)
            => opt?.ReportSettings.Any(x => x.DisplayOrder == DepartmentTotal && x.ItemKey == "1") ?? false;

        private bool RequireDepartmentSubtotal(ArrearagesListSearch opt, ArrearagesList item, ArrearagesList itemBuf)
            => RequireDepartmentSubtotal(opt)
            && itemBuf != null
            && (item == null
                || item.DepartmentCode != itemBuf.DepartmentCode);

        private bool RequireStaffSubtotal(ArrearagesListSearch opt)
            => opt?.ReportSettings.Any(x => x.DisplayOrder == StaffTotal && x.ItemKey == "1") ?? false;

        private bool RequireStaffSubtotal(ArrearagesListSearch opt, ArrearagesList item, ArrearagesList itemBuf)
            => RequireStaffSubtotal(opt)
            && itemBuf != null
            && (item == null
                || RequireDepartmentSubtotal(opt, item, itemBuf)
                || item.StaffCode != itemBuf.StaffCode);

        private bool RequireCustomerSubtotal(ArrearagesListSearch opt)
            => opt?.ReportSettings.Any(x => x.DisplayOrder == CustomerTotal && x.ItemKey == "1") ?? false;

        private bool RequireCustomerSubtotal(ArrearagesListSearch opt, ArrearagesList item, ArrearagesList itemBuf)
            => RequireCustomerSubtotal(opt)
            && itemBuf != null
            && (item == null
                || RequireDepartmentSubtotal(opt, item, itemBuf)
                || RequireStaffSubtotal(opt, item, itemBuf)
                || item.CustomerCode != itemBuf.CustomerCode);

        private bool RequireDueAtSubtotal(ArrearagesListSearch opt)
            => opt?.ReportSettings.Any(x => x.DisplayOrder == DueAtTotal && x.ItemKey == "1") ?? false;

        private bool RequireDueAtSubtotal(ArrearagesListSearch opt, ArrearagesList item, ArrearagesList itemBuf)
            => RequireDueAtSubtotal(opt)
            && itemBuf != null
            && (item == null
                || RequireDepartmentSubtotal(opt, item, itemBuf)
                || RequireStaffSubtotal(opt, item, itemBuf)
                || RequireCustomerSubtotal(opt, item, itemBuf)
                || item.DueAt != itemBuf.DueAt);

        private NumberingFormat[] InitializeNumeringFormats(int precision)
        {
            var code = string.Concat("#,##0.", new string('0', precision));
            return new[] {
                new NumberingFormat { NumberFormatId = 164, FormatCode = code },
                new NumberingFormat { NumberFormatId = 165, FormatCode = "yyyy/mm/dd"},
                new NumberingFormat { NumberFormatId = 166, FormatCode = "#\"日\""}
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
                /*  2 */ GetBorder(left: BDStyles.None,     bottom: BDStyles.None), /* 列見出しA1    */
                /*  3 */ GetBorder(left: BDStyles.None,     top:    BDStyles.None), /* 列見出しA2    */
                /*  4 */ GetBorder(                         bottom: BDStyles.None), /* 列見出しB..I1 */
                /*  5 */ GetBorder(                         top:    BDStyles.None), /* 列見出しB..I2 */
                /*  6 */ GetBorder(right: BDStyles.None,    bottom: BDStyles.None), /* 列見出しJ1    */
                /*  7 */ GetBorder(right: BDStyles.None,    top:    BDStyles.None), /* 列見出しJ2    */
                /*  8 */ GetBorder(left: BDStyles.None,     top: BDStyles.Hair, bottom: BDStyles.None), /* データA1 */
                /*  9 */ GetBorder(left: BDStyles.None,     top: BDStyles.None, bottom: BDStyles.Hair), /* データA2 */
                /* 10 */ GetBorder(                         top: BDStyles.Hair, bottom: BDStyles.None), /* データB..I1 */
                /* 11 */ GetBorder(                         top: BDStyles.None, bottom: BDStyles.Hair), /* データB..I2 */
                /* 12 */ GetBorder(right: BDStyles.None,    top: BDStyles.Hair, bottom: BDStyles.None), /* データJ1 */
                /* 13 */ GetBorder(right: BDStyles.None,    top: BDStyles.None, bottom: BDStyles.Hair), /* データJ2 */
                /* 14 */ GetBorder(left: BDStyles.None, right: BDStyles.None,   bottom: BDStyles.None), /* 合計行1    */
                /* 15 */ GetBorder(left: BDStyles.None, right: BDStyles.None,   top:    BDStyles.None), /* 合計行2    */
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
                /*  9 */ GetCellFormat(fontId: 1, borderId:  9),
                /* 10 */ GetCellFormat(fontId: 1, borderId: 10),
                /* 11 */ GetCellFormat(fontId: 1, borderId: 11),
                /* 12 */ GetCellFormat(fontId: 1, borderId: 12),
                /* 13 */ GetCellFormat(fontId: 1, borderId: 13),
                /* 14 */ GetCellFormat(fontId: 1, borderId: 10, numFmtId: 165, horizontal: HorizontalAlignmentValues.Center),
                /* 15 */ GetCellFormat(fontId: 1, borderId: 11, numFmtId: 165, horizontal: HorizontalAlignmentValues.Center),
                /* 16 */ GetCellFormat(fontId: 1, borderId: 11, numFmtId: 166, horizontal: HorizontalAlignmentValues.Right),
                /* 17 */ GetCellFormat(fontId: 1, borderId: 11, numFmtId: decNumFmt, horizontal: HorizontalAlignmentValues.Right),

                /* 18 */ GetCellFormat(fontId: 3, fillId: 2, borderId:  2),
                /* 19 */ GetCellFormat(fontId: 3, fillId: 2, borderId:  3),
                /* 20 */ GetCellFormat(fontId: 1, fillId: 2, borderId:  4, numFmtId: decNumFmt, horizontal: HorizontalAlignmentValues.Right),
                /* 21 */ GetCellFormat(fontId: 3, fillId: 2, borderId:  5),
                /* 22 */ GetCellFormat(fontId: 3, fillId: 2, borderId:  6),
                /* 23 */ GetCellFormat(fontId: 3, fillId: 2, borderId:  7),
                /* 24 */ GetCellFormat(fontId: 3, fillId: 2, borderId: 14),
                /* 25 */ GetCellFormat(fontId: 3, fillId: 2, borderId: 15),
            };
        }
        const uint cfidxTitle  = 1u;
        const uint cfidxHA1    = 2u;
        const uint cfidxHA2    = 3u;
        const uint cfidxHBI1   = 4u;
        const uint cfidxHBI2   = 5u;
        const uint cfidxHJ1    = 6u;
        const uint cfidxHJ2    = 7u;

        const uint cfidxDA1    = 8u;
        const uint cfidxDA2    = 9u;
        const uint cfidxDBI1   = 10u;
        const uint cfidxDBI2   = 11u;
        const uint cfidxDJ1    = 12u;
        const uint cfidxDJ2    = 13u;
        const uint cfidxDDate1 = 14u;
        const uint cfidxDDate2 = 15u;
        const uint cfidxDDay2  = 16u;
        const uint cfidxDVal1  = 17u;

        const uint cfidxSF1    = 18u;
        const uint cfidxSF2    = 19u;
        const uint cfidxSG1    = 20u;
        const uint cfidxSG2    = 21u;
        const uint cfidxSH1    = 22u;
        const uint cfidxSH2    = 23u;
        const uint cfidxS1     = 24u;
        const uint cfidxS2     = 25u;
        #endregion

        private void SetColumnCaptin(SheetData data, ref uint rowIndex, List<uint> indices)
        {
            rowIndex++;

            data.SetText(rowIndex, "A", "ID", cfidxHA1);
            data.SetText(rowIndex, "B", "得意先コード", cfidxHBI1);
            data.SetText(rowIndex, "C", "得意先電話番号", cfidxHBI1);
            data.SetText(rowIndex, "D", "請求日", cfidxHBI1);
            data.SetText(rowIndex, "E", "請求締日", cfidxHBI1);
            data.SetText(rowIndex, "F", "当初予定日", cfidxHBI1);
            data.SetText(rowIndex, "G", "回収予定額", cfidxHBI1);
            data.SetText(rowIndex, "H", "請求書番号", cfidxHBI1);
            data.SetText(rowIndex, "I", "請求部門コード", cfidxHBI1);
            data.SetText(rowIndex, "J", "担当者コード", cfidxHJ1);
            indices.Add(rowIndex);

            rowIndex++;
            data.SetStyle(rowIndex, "A", cfidxHA2);
            data.SetText(rowIndex, "B", "得意先名", cfidxHBI2);
            data.SetText(rowIndex, "C", "得意先備考", cfidxHBI2);
            data.SetText(rowIndex, "D", "売上日", cfidxHBI2);
            data.SetText(rowIndex, "E", "入金予定日", cfidxHBI2);
            data.SetText(rowIndex, "F", "滞留日数", cfidxHBI2);
            data.SetStyle(rowIndex, "G", cfidxHBI2);
            data.SetText(rowIndex, "H", Note1, cfidxHBI2);
            data.SetText(rowIndex, "I", "請求部門名", cfidxHBI2);
            data.SetText(rowIndex, "J", "担当者名", cfidxHJ2);

        }

        private void SetDetail(SheetData data, ArrearagesList item, ref uint rowIndex, List<uint> indices)
        {
            rowIndex++;

            data.SetNumber(rowIndex, "A", item.Id, cfidxDA1);
            data.SetText(rowIndex, "B", item.CustomerCode, cfidxDBI1);
            data.SetText(rowIndex, "C", item.Tel, cfidxDBI1);
            data.SetDate(rowIndex, "D", item.BilledAt, cfidxDDate1);
            data.SetDate(rowIndex, "E", item.ClosingAt, cfidxDDate1);
            data.SetDate(rowIndex, "F", item.OriginalDueAt, cfidxDDate1);
            data.SetNumber(rowIndex, "G", item.RemainAmount, cfidxDVal1);
            data.SetText(rowIndex, "H", item.InvoiceCode, cfidxDBI1);
            data.SetText(rowIndex, "I", item.DepartmentCode, cfidxDBI1);
            data.SetText(rowIndex, "J", item.StaffCode, cfidxDJ1);
            indices.Add(rowIndex);

            rowIndex++;

            data.SetStyle(rowIndex, "A", cfidxDA2);
            data.SetText(rowIndex, "B", item.CustomerName, cfidxDBI2);
            data.SetText(rowIndex, "C", item.CustomerNote, cfidxDBI2);
            data.SetDate(rowIndex, "D", item.SalesAt, cfidxDDate2);
            data.SetDate(rowIndex, "E", item.DueAt, cfidxDDate2);
            data.SetNumber(rowIndex, "F", item.ArrearagesDayCount, cfidxDDay2);
            data.SetStyle(rowIndex, "G", cfidxDBI2);
            data.SetText(rowIndex, "H", item.Note1, cfidxDBI2);
            data.SetText(rowIndex, "I", item.DepartmentName, cfidxDBI2);
            data.SetText(rowIndex, "J", item.StaffName, cfidxDJ2);

        }

        private void SetSubtotal(SheetData data, string caption, decimal amount, ref uint rowIndex, List<uint> indices, List<uint> subtotalIndices)
        {
            rowIndex++;
            // 合計行
            data.SetText(rowIndex, "A", caption);
            data.SetNumber(rowIndex, "G", amount, cfidxSG1);
            data.SetStyle(GetCellReference(rowIndex, "A", columnNameEnd: "E"), cfidxS1);
            data.SetStyle(rowIndex, "F", cfidxSF1);
            data.SetStyle(rowIndex, "H", cfidxSH1);
            data.SetStyle(GetCellReference(rowIndex, "I", columnNameEnd: "J"), cfidxS1);
            indices.Add(rowIndex);
            subtotalIndices.Add(rowIndex);

            rowIndex++;
            data.SetStyle(GetCellReference(rowIndex, "A", columnNameEnd: "E"), cfidxS2);
            data.SetStyle(rowIndex, "F", cfidxSF2);
            data.SetStyle(rowIndex, "G", cfidxSG2);
            data.SetStyle(rowIndex, "H", cfidxSH2);
            data.SetStyle(GetCellReference(rowIndex, "I", columnNameEnd: "J"), cfidxS2);

        }

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
                    new Column { Min =  1, Max =  1, Width = 10.28, CustomWidth = true },
                    new Column { Min =  2, Max =  2, Width = 26.57, CustomWidth = true },
                    new Column { Min =  3, Max =  3, Width = 17.57, CustomWidth = true },
                    new Column { Min =  4, Max =  6, Width = 10   , CustomWidth = true },
                    new Column { Min =  7, Max =  7, Width = 14.42, CustomWidth = true },
                    new Column { Min =  8, Max =  8, Width = 19.28, CustomWidth = true },
                    new Column { Min =  9, Max =  9, Width = 25   , CustomWidth = true },
                    new Column { Min = 10, Max = 10, Width = 17.57, CustomWidth = true },
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
            var indices = new List<uint>();
            var subtotalIndices = new List<uint>();
            var rowbreakIndices = new List<uint>();

            data.SetText(rowIndex, "A", title);
            data.SetStyle(GetCellReference(rowIndex, "A", columnNameEnd:"J"), cfidxTitle);

            rowIndex++;

            SetColumnCaptin(data, ref rowIndex, indices);

            var endIndex = rowIndex;

            var dueAtAmount = 0M;
            var customerAmount = 0M;
            var staffAmount = 0M;
            var departmentAmount = 0M;
            var totalAmount = 0M;
            ArrearagesList itemBuf = null;
            foreach (var item in Items)
            {
                var requireDueAtSubtotal        = RequireDueAtSubtotal      (Option, item, itemBuf);
                var requireCustomerSubtotal     = RequireCustomerSubtotal   (Option, item, itemBuf);
                var requireStaffSubtotal        = RequireStaffSubtotal      (Option, item, itemBuf);
                var requireDepertmentSubtotal   = RequireDepartmentSubtotal (Option, item, itemBuf);

                if (requireDueAtSubtotal)
                {
                    SetSubtotal(data, "予定日計", dueAtAmount, ref rowIndex, indices, subtotalIndices);
                    dueAtAmount = 0M;
                }

                if (requireCustomerSubtotal)
                {
                    SetSubtotal(data, "得意先計", customerAmount, ref rowIndex, indices, subtotalIndices);
                    customerAmount = 0M;
                }

                if (requireStaffSubtotal)
                {
                    SetSubtotal(data, "担当者計", staffAmount, ref rowIndex, indices, subtotalIndices);
                    staffAmount = 0M;
                }

                if (requireDepertmentSubtotal)
                {
                    SetSubtotal(data, "請求部門計", departmentAmount, ref rowIndex, indices, subtotalIndices);
                    departmentAmount = 0M;
                }

                if (requireCustomerSubtotal || requireCustomerSubtotal || requireStaffSubtotal || requireDepertmentSubtotal)
                    rowbreakIndices.Add(rowIndex);

                SetDetail(data, item, ref rowIndex, indices);

                dueAtAmount         += item.RemainAmount;
                customerAmount      += item.RemainAmount;
                staffAmount         += item.RemainAmount;
                departmentAmount    += item.RemainAmount;
                totalAmount         += item.RemainAmount;

                itemBuf = item;
            }

            if (RequireDueAtSubtotal(Option, null, itemBuf))
                SetSubtotal(data, "予定日計", dueAtAmount, ref rowIndex, indices, subtotalIndices);

            if (RequireCustomerSubtotal(Option, null, itemBuf))
                SetSubtotal(data, "得意先計", customerAmount, ref rowIndex, indices, subtotalIndices);

            if (RequireStaffSubtotal(Option, null, itemBuf))
                SetSubtotal(data, "担当者計", staffAmount, ref rowIndex, indices, subtotalIndices);

            if (RequireDepartmentSubtotal(Option, null, itemBuf))
                SetSubtotal(data, "請求部門計", departmentAmount, ref rowIndex, indices, subtotalIndices);

            SetSubtotal(data, "　総合計", totalAmount, ref rowIndex, indices, subtotalIndices);

            var mergeCells = new MergeCells();
            mergeCells.Append(Concat(
                GetNextRowMergeCells("A", indices.Except(subtotalIndices)),
                GetNextRowMergeCells("G", indices),
                subtotalIndices.Select(x => GetMergeCell(x, "A", x + 1, "F"))
                ));
            worksheet.Append(mergeCells);

            worksheet.Append(GetPageSetup());

            var companyInfo = $"{Company.Code}：{Company.Name}";
            var outputInfo = $"出力日時：{DateTime.Today:yyyy年MM月dd日}";
            worksheet.Append(GetHeaderFooter(leftHeader: companyInfo, rightHeader: outputInfo));

            if (rowbreakIndices.Any())
            {
                worksheet.Append(GetRowBreaks(rowbreakIndices));
            }

            var definedNames = (workbook.DefinedNames = new DefinedNames());
            definedNames.Append(sheet.GetPrintTitlesDefine(1u, endIndex));


            workbook.Save();
        }
    }
}
