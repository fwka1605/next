using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary> 請求残高年齢表（明細）</summary>
    public partial class PF0102 : VOneScreenBase
    {
        private string CellName(string value) => $"cel{value}";
        private ColumnNameSetting BillingNote1ColumnNameSetting { get; set; }
        public List<BillingAgingListDetail> BillingAgingDetailList { get; set; }
        public List<object> ReportCondition { get; set; }
        public BillingAgingListSearch SearchCondition { get; set; }
        public int Precision { get; set; }

        #region 画面の初期化
        public PF0102()
        {
            InitializeComponent();
            grdBillingAgingDetail.SetupShortcutKeys();
            Text = "請求残高年齢表（明細）";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("");
            BaseContext.SetFunction01Enabled(false);

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = PrintBillingAgingDetail;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ExportBillingAgingDetail;

            BaseContext.SetFunction10Caption("戻る");
            OnF10ClickHandler = ExitBillingAgingDetail;
        }

        private void PF0102_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var tasks = new List<Task>();
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadControlColorAsync());
                tasks.Add(LoadColumnNameSetting());

                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
                SetSearchData();
                InitializeGridTemplate();
                grdBillingAgingDetail.DataSource = new BindingSource(BillingAgingDetailList, null);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetSearchData()
        {
            lblDepartmentCodeFrom.Text = SearchCondition.DepartmentCodeFrom;
            lblDepartmentCodeTo.Text = SearchCondition.DepartmentCodeTo;
            lblStaffCodeFrom.Text = SearchCondition.StaffCodeFrom;
            lblStaffCodeTo.Text = SearchCondition.StaffCodeTo;
            lblDepartmentNameFrom.Text = SearchCondition.DepartmentNameFrom;
            lblDepartmentNameTo.Text = SearchCondition.DepartmentNameTo;
            lblStaffNameFrom.Text = SearchCondition.StaffNameFrom;
            lblStaffNameTo.Text = SearchCondition.StaffNameTo;
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);

            var height = builder.DefaultRowHeight;
            var currencyWidth = UseForeignCurrency ? 80 : 0;
            var customerWidth = UseForeignCurrency ? 200 : 280;
            var posX = new int[] { 0, 40, 155, 355, 435, 535, 635, 735, 885, 1035, 1135, 1235, 1335 };
            var note = BillingNote1ColumnNameSetting.DisplayColumnName;
            
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,            40, "Header", cell: builder.GetRowHeaderCell()),
                new CellSetting(height,           115, nameof(BillingAgingListDetail.CustomerCode) , dataField: nameof(BillingAgingListDetail.CustomerCode) , caption: "得意先コード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, customerWidth, nameof(BillingAgingListDetail.CustomerName) , dataField: nameof(BillingAgingListDetail.CustomerName) , caption: "得意先名"    , cell: builder.GetTextBoxCell()),
                new CellSetting(height, currencyWidth, nameof(BillingAgingListDetail.CurrencyCode) , dataField: nameof(BillingAgingListDetail.CurrencyCode) , caption: "通貨コード"  , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height,           100, nameof(BillingAgingListDetail.BilledAt)     , dataField: nameof(BillingAgingListDetail.BilledAt)     , caption: "請求日"      , cell: builder.GetDateCell_yyyyMMdd()),
                new CellSetting(height,           100, nameof(BillingAgingListDetail.DueAt)        , dataField: nameof(BillingAgingListDetail.DueAt)        , caption: "入金予定日"  , cell: builder.GetDateCell_yyyyMMdd()),
                new CellSetting(height,           100, nameof(BillingAgingListDetail.SalesAt)      , dataField: nameof(BillingAgingListDetail.SalesAt)      , caption: "売上日"      , cell: builder.GetDateCell_yyyyMMdd()),
                new CellSetting(height,           150, nameof(BillingAgingListDetail.BillingAmount), dataField: nameof(BillingAgingListDetail.BillingAmount), caption: "請求金額"    , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height,           150, nameof(BillingAgingListDetail.RemainAmount) , dataField: nameof(BillingAgingListDetail.RemainAmount) , caption: "請求残"      , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height,           100, nameof(BillingAgingListDetail.StaffCode)    , dataField: nameof(BillingAgingListDetail.StaffCode)    , caption: "担当者コード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height,           100, nameof(BillingAgingListDetail.StaffName)    , dataField: nameof(BillingAgingListDetail.StaffName)    , caption: "担当者名"    , cell: builder.GetTextBoxCell()),
                new CellSetting(height,           100, nameof(BillingAgingListDetail.InvoiceCode)  , dataField: nameof(BillingAgingListDetail.InvoiceCode)  , caption: "請求書番号"  , cell: builder.GetTextBoxCell()),
                new CellSetting(height,           250, nameof(BillingAgingListDetail.Note)         , dataField: nameof(BillingAgingListDetail.Note)         , caption: note          , cell: builder.GetTextBoxCell()),
            });

            var billing = 0M;
            var remain = 0M;
            foreach (var detail in BillingAgingDetailList)
            {
                billing += detail.BillingAmount;
                remain += detail.RemainAmount;
            }

            var template = builder.Build();
            var decimalFormat = ((Precision == 0) ? "#,##0" : "#,##0" + "." + new string('0', Precision));
            var footer = new ColumnFooterSection();
            footer.Height = height;
            footer.Cells.AddRange(new Cell[]
            {
                new HeaderCell() { Size = new Size(40,  height), Location = new Point(posX[0], 0)},
                new HeaderCell() { Size = new Size(695, height), Location = new Point(posX[1], 0), Value = "合計"},
                new HeaderCell() { Size = new Size(150, height), Location = new Point(posX[7], 0), Name ="TotalBillingAmount", Value = billing.ToString(decimalFormat), Style = new CellStyle() { TextAlign = MultiRowContentAlignment.MiddleRight }},
                new HeaderCell() { Size = new Size(150, height), Location = new Point(posX[8], 0), Name ="TotalRemainAmount",  Value = remain.ToString(decimalFormat) , Style = new CellStyle() { TextAlign = MultiRowContentAlignment.MiddleRight }},
                new HeaderCell() { Size = new Size(550, height), Location = new Point(posX[9], 0)},
            });

            template.ColumnFooters.Add(footer);

            grdBillingAgingDetail.Template = template;
            grdBillingAgingDetail.HideSelection = true;

            if (UseForeignCurrency)
            {
                grdBillingAgingDetail.FreezeLeftCellName = CellName(nameof(BillingAgingListDetail.CurrencyCode));
            }
            else
            {
                grdBillingAgingDetail.FreezeLeftCellName = CellName(nameof(BillingAgingListDetail.CustomerName));
            }
        }

        private async Task LoadColumnNameSetting()
        {
            await ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterClient client) =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    BillingNote1ColumnNameSetting = result.ColumnNames.Where(x => x.TableName == "Billing" && x.ColumnName == "Note1").First();
            });
        }

        #endregion

        #region Function Key Event


        [OperationLog("印刷")]
        private void PrintBillingAgingDetail()
        {
            try
            {
                var source = grdBillingAgingDetail.DataSource as BindingSource;
                var list = source?.DataSource as List<BillingAgingListDetail> ?? new List<BillingAgingListDetail>();

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                    return;
                }
                BillingAgingListDetailSectionReport report = null;
                var serverPath = string.Empty;
                var task = Task.Run(async () =>
                {
                    serverPath = await GetServerPathAsync();
                    if (!Directory.Exists(serverPath))
                    {
                        serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }

                    report = new BillingAgingListDetailSectionReport();
                    var title = $"請求残高年齢表（明細）{ DateTime.Today:yyyyMMdd}";
                    report.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                    report.Name = title;
                    report.SetData(list, Precision, BillingNote1ColumnNameSetting);

                    var searchReport = new SearchConditionSectionReport();
                    searchReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "請求残高年齢表（明細）");
                    searchReport.Name = title;
                    searchReport.SetPageDataSetting(ReportCondition);

                    report.Run(false);
                    searchReport.SetPageNumber(report.Document.Pages.Count);
                    searchReport.Run(false);

                    report.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)searchReport.Document.Pages.Clone());
                });

                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                ShowDialogPreview(ParentForm, report, serverPath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("エクスポート")]
        private void ExportBillingAgingDetail()
        {
            ClearStatusMessage();
            try
            {
                var list = ((IEnumerable)grdBillingAgingDetail.DataSource).Cast<BillingAgingListDetail>().ToList();

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                var task = GetServerPathAsync();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var serverPath = task.Result;

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"請求明細一覧表(明細){SearchCondition.CustomerCodeFrom}_{SearchCondition.YearMonth:yyyyMM}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var decimalFormat = '0' + ((Precision == 0) ? string.Empty : "." + new string('0', Precision));
                var definition = new BillingAgingListDetailFileDefinition(new DataExpression(ApplicationControl), BillingNote1ColumnNameSetting);

                if (definition.CurrencyCodeField.Ignored = (!UseForeignCurrency))
                {
                    definition.CurrencyCodeField.FieldName = null;
                }
                definition.BillingAmountField.Format = value => value.ToString(decimalFormat);
                definition.RemainAmountField.Format  = value => value.ToString(decimalFormat);
                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, list, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return;
                }

                DispStatusMessage(MsgInfFinishExport);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        [OperationLog("戻る")]
        private void ExitBillingAgingDetail()
        {
            ParentForm.Close();
        }
        #endregion

        #region web service
        private async Task<string> GetServerPathAsync()
        {
            return await Util.GetGeneralSettingServerPathAsync(Login);
        }
        #endregion

    }
}
