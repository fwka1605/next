using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.MatchingService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Common;
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
    /// <summary>消込仕訳出力取消</summary>
    public partial class PE0401 : VOneScreenBase
    {
        private int CustomerId { get; set; }
        private int CurrencyId { get; set; }
        private Currency Currency { get; set; }
        private int Precision { get; set; } = 0;
        private List<object> SearchField { get; set; }
        private List<MatchingJournalizingDetail> MatchingJournalizingDetailList { get; set; }
        private int PrevNameIndex { get; set; } = -1;
        private int LastNameIndex { get; set; } = 0;
        private bool FindNameFlg { get; set; } = false;
        private int NameCount { get; set; } = 0;
        private string PrevNameSearchKey { get; set; } = "";
        private int PrevCodeIndex { get; set; } = -1;
        private int LastCodeIndex { get; set; } = 0;
        private bool FindCodeFlg { get; set; } = false;
        private int CodeCount { get; set; } = 0;
        private string PrevCodeSearchKey { get; set; } = "";
        private int PrevPayerIndex { get; set; } = -1;
        private int LastPayerIndex { get; set; } = 0;
        private bool FindPayerFlg { get; set; } = false;
        private int PayerCount { get; set; } = 0;
        private string PrevPayerSearchKey { get; set; } = "";
        private string CellName(string value) => $"cel{value}";

        public PE0401()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }
        private void InitializeUserComponent()
        {
            grid.GridColorType = GridColorType.Special;
            grid.SetupShortcutKeys();
            Text = "消込仕訳出力取消";
            grid.CellFormatting += (sender, e) =>
            {
                if (e.Scope != CellScope.Row
                 || e.RowIndex < 0
                 || e.CellIndex <= 0) return;
                var model = grid.Rows[e.RowIndex].DataBoundItem as MatchingJournalizingDetail;
                if (model == null || (model.GroupIndex % 2 == 1) ) return;
                e.CellStyle.BackColor = ColorContext.GridAlternatingRowBackColor;
            };
        }

        private void InitializeHandlers()
        {
            tbcJournalizingDetail.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcJournalizingDetail.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = Exit;
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
                }
            };
        }

        private void PE0401_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                loadTask.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SetInitialSetting();
                InitializeMatchingJournalizingGrid();
                SuspendLayout();
                tbcJournalizingDetail.SelectedIndex = 0;
                ResumeLayout();
                this.ActiveControl = datFromCreateAt;
                datFromCreateAt.Focus();

                txtPayerName.Validated += (sdr, ev) =>
                {
                    txtPayerName.Text = EbDataHelper.ConvertToValidEbKana(txtPayerName.Text.Trim());
                };
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("取消");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = DoExportPrintCancel;

            BaseContext.SetFunction08Caption("全選択");
            OnF08ClickHandler = SelectAll;

            BaseContext.SetFunction09Caption("全解除");
            OnF09ClickHandler = DeselectAll;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        /// <summary>Set Currency Model,Precision and CurrencyId</summary>
        private async Task CurrencyInfo()
        {
            Currency = null;
            CurrenciesResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCurrencyCode.Text });
            });

            if (result.ProcessResult.Result && result.Currencies.Any() && result.Currencies[0] != null)
            {
                Precision = result.Currencies[0].Precision;
                Currency = result.Currencies[0];
                CurrencyId = result.Currencies[0].Id;
            }
        }

        #region 画面の Format & Warning Setting
        /// <summary>Set Initial Control</summary>
        private void SetInitialSetting()
        {
            if (ApplicationControl != null)
            {
                var expression = new DataExpression(ApplicationControl);

                txtCustomer.Format = expression.CustomerCodeFormatString;
                txtCustomer.MaxLength = expression.CustomerCodeLength;
                txtCustomer.ImeMode = expression.CustomerCodeImeMode();
                txtCustomer.PaddingChar = expression.CustomerCodePaddingChar;

                txtaddCustomerCode.Format = expression.CustomerCodeFormatString;
                txtaddCustomerCode.MaxLength = expression.CustomerCodeLength;
                txtaddCustomerCode.ImeMode = expression.CustomerCodeImeMode();
                txtaddCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;

                if (expression.UseForeignCurrency == 1)
                {
                    lblCurrencyCode.Visible = true;
                    txtCurrencyCode.Visible = true;
                    btnCurrencyCode.Visible = true;
                }
            }
        }

        #endregion

        #region グリッド作成
        private void InitializeMatchingJournalizingGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, autoLocationSet: false);
            var height = builder.DefaultRowHeight;
            var template = new Template();

            Precision = (UseForeignCurrency) ? Precision : 0;
            builder.GetNumberCellCurrency(Precision, Precision, 0);
            builder.Items.Clear();

            var posX = new int[] { };
            if (!UseForeignCurrency)
            {
                posX = new int[]
                             {
                    0, 40, 550, 935,
                    40, 80, 215, 350, 0, 430, 550, 700, 815, 935, 1050, 1165, 1285, 1400, 1550, 1550, 1550
                             };
            }
            else
            {
                posX = new int[]
                             {
                    0, 40, 625, 1010,
                    40, 80, 215, 350, 430, 505, 625, 775, 890, 1010, 1125, 1240, 1360, 1475, 1625, 1625, 1625
                             };
            }
            var posY = new int[] { 0, height, height * 2 };
            var widthCcy = UseForeignCurrency ? 75 : 0;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            #region ヘッダ
            builder.Items.AddRange(new CellSetting[]
            {
                #region ヘッダー1
                new CellSetting(height * 2,       40, "RowHeader"                                      , location: new Point(posX[ 0], posY[0])),
                new CellSetting(    height,      585, "check"                                          , location: new Point(posX[ 1], posY[0])),
                new CellSetting(    height,      385, "ReceiptInformation"                             , location: new Point(posX[ 2], posY[0]), caption: "入金情報"),
                new CellSetting(    height,      615, "BillingInformation"                             , location: new Point(posX[ 3], posY[0]), caption: "請求情報"),
                #endregion
                #region ヘッダー2
                new CellSetting(    height,       40, "CheckOn"                                        , location: new Point(posX[ 4], posY[1]), caption: "解除"    ),
                new CellSetting(    height,      135, nameof(MatchingJournalizingDetail.OutputAt)       , location: new Point(posX[ 5], posY[1]), caption: "仕訳日"  ),
                new CellSetting(    height,      135, nameof(MatchingJournalizingDetail.CreateAt)       , location: new Point(posX[ 6], posY[1]), caption: "更新日付"),
                new CellSetting(    height,       80, nameof(MatchingJournalizingDetail.JournalizingName), location: new Point(posX[ 7], posY[1]), caption: "仕訳区分"),
                new CellSetting(    height, widthCcy, "CurrencyCode"                                   , location: new Point(posX[ 8], posY[1]) , caption: "通貨コード"),
                new CellSetting(    height,      120, nameof(MatchingJournalizingDetail.Amount)         , location: new Point(posX[ 9], posY[1]) , caption: "金額"),
                new CellSetting(    height,      150, nameof(MatchingJournalizingDetail.PayerName)      , location: new Point(posX[10], posY[1]), caption: "振込依頼人名"),
                new CellSetting(    height,      115, nameof(MatchingJournalizingDetail.RecordedAt)     , location: new Point(posX[11], posY[1]), caption: "入金日"),
                new CellSetting(    height,      120, nameof(MatchingJournalizingDetail.ReceiptAmount)  , location: new Point(posX[12], posY[1]), caption: "入金額"),
                new CellSetting(    height,      115, nameof(MatchingJournalizingDetail.BilledAt)       , location: new Point(posX[13], posY[1]), caption: "請求日"),
                new CellSetting(    height,      115, nameof(MatchingJournalizingDetail.InvoiceCode)    , location: new Point(posX[14], posY[1]), caption: "請求書番号"),
                new CellSetting(    height,      120, nameof(MatchingJournalizingDetail.BillingAmount)  , location: new Point(posX[15], posY[1]), caption: "請求額"),
                new CellSetting(    height,      115, nameof(MatchingJournalizingDetail.CustomerCode)   , location: new Point(posX[16], posY[1]), caption: "得意先コード"),
                new CellSetting(    height,      150, nameof(MatchingJournalizingDetail.CustomerName)   , location: new Point(posX[17], posY[1]), caption: "得意先名"),
                new CellSetting(    height,        0, nameof(MatchingJournalizingDetail.HeaderId)       , location: new Point(posX[18], posY[1])),
                new CellSetting(    height,        0, nameof(MatchingJournalizingDetail.JournalizingType), location: new Point(posX[19], posY[1])),
                #endregion
            });
            #endregion
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();

            #region データ
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,       40, "RowHeader"                                      , cell: builder.GetRowHeaderCell()                            , location: new Point(posX[ 0], posY[0])),
                new CellSetting(height,       40, "CheckBox"                                       , readOnly: false, cell: builder.GetCheckBoxCell()            , location: new Point(posX[ 4], posY[0])),
                new CellSetting(height,      135, nameof(MatchingJournalizingDetail.OutputAt)       , dataField: nameof(MatchingJournalizingDetail.OutputAt)       , location: new Point(posX[ 5], posY[0]), cell: builder.GetDateCell_yyyyMMddHHmmss()       ),
                new CellSetting(height,      135, nameof(MatchingJournalizingDetail.CreateAt)       , dataField: nameof(MatchingJournalizingDetail.CreateAt)       , location: new Point(posX[ 6], posY[0]), cell: builder.GetDateCell_yyyyMMddHHmmss()       ),
                new CellSetting(height,       80, nameof(MatchingJournalizingDetail.JournalizingName), dataField: nameof(MatchingJournalizingDetail.JournalizingName), location: new Point(posX[ 7], posY[0]), cell: builder.GetTextBoxCell(middleCenter)       ),
                new CellSetting(height, widthCcy, nameof(MatchingJournalizingDetail.CurrencyCode)   , dataField: nameof(MatchingJournalizingDetail.CurrencyCode)   , location: new Point(posX[ 8], posY[0]), cell: builder.GetTextBoxCell(middleCenter)       ),
                new CellSetting(height,      120, nameof(MatchingJournalizingDetail.Amount)         , dataField: nameof(MatchingJournalizingDetail.Amount)         , location: new Point(posX[ 9], posY[0]), cell: builder.GetTextBoxCurrencyCell(Precision)  ),
                new CellSetting(height,      150, nameof(MatchingJournalizingDetail.PayerName)      , dataField: nameof(MatchingJournalizingDetail.PayerName)      , location: new Point(posX[10], posY[0]), cell: builder.GetTextBoxCell()                   ),
                new CellSetting(height,      115, nameof(MatchingJournalizingDetail.RecordedAt)     , dataField: nameof(MatchingJournalizingDetail.RecordedAt)     , location: new Point(posX[11], posY[0]), cell: builder.GetDateCell_yyyyMMdd()             ),
                new CellSetting(height,      120, nameof(MatchingJournalizingDetail.ReceiptAmount)  , dataField: nameof(MatchingJournalizingDetail.ReceiptAmount)  , location: new Point(posX[12], posY[0]), cell: builder.GetTextBoxCurrencyCell(Precision)  ),
                new CellSetting(height,      115, nameof(MatchingJournalizingDetail.BilledAt)       , dataField: nameof(MatchingJournalizingDetail.BilledAt)       , location: new Point(posX[13], posY[0]), cell: builder.GetDateCell_yyyyMMdd()             ),
                new CellSetting(height,      115, nameof(MatchingJournalizingDetail.InvoiceCode)    , dataField: nameof(MatchingJournalizingDetail.InvoiceCode)    , location: new Point(posX[14], posY[0]), cell: builder.GetTextBoxCell(middleCenter)       ),
                new CellSetting(height,      120, nameof(MatchingJournalizingDetail.BillingAmount)  , dataField: nameof(MatchingJournalizingDetail.BillingAmount)  , location: new Point(posX[15], posY[0]), cell: builder.GetTextBoxCurrencyCell(Precision)  ),
                new CellSetting(height,      115, nameof(MatchingJournalizingDetail.CustomerCode)   , dataField: nameof(MatchingJournalizingDetail.CustomerCode)   , location: new Point(posX[16], posY[0]), cell: builder.GetTextBoxCell(middleCenter)       ),
                new CellSetting(height,      150, nameof(MatchingJournalizingDetail.CustomerName)   , dataField: nameof(MatchingJournalizingDetail.CustomerName)   , location: new Point(posX[17], posY[0]), cell: builder.GetTextBoxCell()                   ),
                new CellSetting(height,        0, nameof(MatchingJournalizingDetail.HeaderId)       , dataField: nameof(MatchingJournalizingDetail.HeaderId)       , location: new Point(posX[18], posY[0]), cell: builder.GetTextBoxCell()                   ),
                new CellSetting(height,        0, nameof(MatchingJournalizingDetail.JournalizingType), dataField: nameof(MatchingJournalizingDetail.JournalizingType), location: new Point(posX[19], posY[0]), cell: builder.GetTextBoxCell(middleCenter)       ),
            });
            #endregion

            builder.BuildRowOnly(template);
            grid.Template = template;
        }
        #endregion

        #region 画面の F1
        [OperationLog("検索")]
        public void Search()
        {
            try
            {
                ClearStatusMessage();

                if (!ValidateSearchData()) return;
                var matchingSearch = GetSearchDataCondition();

                SearchMatchingJournalizingDetail(matchingSearch);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>Validate For Search Data</summary>
        /// <returns>Validated Result (true or false)</returns>
        private bool ValidateSearchData()
        {
            if (!datFromCreateAt.Value.HasValue)
            {
                tbcJournalizingDetail.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "更新日付");
                datFromCreateAt.Focus();
                return false;
            }

            if (!datToCreateAt.Value.HasValue)
            {
                tbcJournalizingDetail.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "更新日付");
                datToCreateAt.Focus();
                return false;
            }

            if (!datFromCreateAt.ValidateRange(datToCreateAt, () => ShowWarningDialog(MsgWngInputRangeChecked, "更新日付")))
            {
                tbcJournalizingDetail.SelectedIndex = 0;
                datFromCreateAt.Focus();
                return false;
            }

            if (UseForeignCurrency && string.IsNullOrEmpty(txtCurrencyCode.Text))
            {
                tbcJournalizingDetail.SelectedIndex = 0;
                ShowWarningDialog(MsgWngInputRequired, "通貨コード");
                txtCurrencyCode.Focus();
                return false;
            }
            return true;
        }

        private JournalizingOption GetSearchDataCondition()
        {
            var option = new JournalizingOption
            {
                CompanyId = CompanyId,
                CreateAtFrom = datFromCreateAt.Value,
            };
            if (datToCreateAt.Value.HasValue)
            {
                var createAt = datToCreateAt.Value.Value;
                option.CreateAtTo = createAt.Date.AddDays(1).AddMilliseconds(-1);
            }
            if (!string.IsNullOrWhiteSpace(txtCustomer.Text)) option.CustomerId = CustomerId;
            if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text)) option.CurrencyId = CurrencyId;
            var types = new List<int>();
            if (cbxMatching.Checked) types.Add(JournalizingType.Matching);
            if (cbxAdvanceReceivedOccured.Checked) types.Add(JournalizingType.AdvanceReceivedOccured);
            if (cbxAdvanceReceivedTransfer.Checked) types.Add(JournalizingType.AdvanceReceivedTransfer);
            if (cbxReceiptExclude.Checked) types.Add(JournalizingType.ReceiptExclude);
            option.JournalizingTypes = types;
            ReportSearchCondition();
            return option;
        }

        /// <summary> SearchMatchingJournalizingDetail Data </summary>
        private void SearchMatchingJournalizingDetail(JournalizingOption option)
        {
            List<MatchingJournalizingDetail> list = null;
            MatchingJournalizingDetailsResult result = null;
            var task = ServiceProxyFactory.DoAsync<MatchingServiceClient>(async client =>
            {
                result = await client.GetMatchingJournalizingDetailAsync(SessionKey, option);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                list = result.MatchingJournalizingDetails;
                grid.DataSource = new BindingSource(list, null);
                if (list.Any())
                {
                    InitializeMatchingJournalizingGrid();
                    tbcJournalizingDetail.SelectedTab = tbpSearchResult;
                    BaseContext.SetFunction03Enabled(true);
                    BaseContext.SetFunction08Enabled(true);
                    BaseContext.SetFunction09Enabled(true);
                }
                else
                {
                    tbcJournalizingDetail.SelectedTab = tbpSearchCondition;
                    ShowWarningDialog(MsgWngNotExistSearchData);
                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                }
            }
            else
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
            }
        }
        #endregion

        #region 画面の F2
        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            grid.Rows.Clear();
            datFromCreateAt.Clear();
            datToCreateAt.Clear();
            txtCustomer.Clear();
            lblCustomername.Clear();
            txtCurrencyCode.Clear();
            txtaddCustomerName.Clear();
            txtPayerName.Clear();
            txtaddCustomerCode.Clear();
            tbcJournalizingDetail.SelectedTab = tbpSearchCondition;
            datFromCreateAt.Select();
            cbxMatching.Checked = true;
            cbxReceiptExclude.Checked = true;
            cbxAdvanceReceivedOccured.Checked = true;
        }
        #endregion

        #region 画面の F3
        [OperationLog("取消")]
        private void DoExportPrintCancel()
        {
            try
            {
                ClearStatusMessage();
                var listId = new List<int>();
                MatchingJournalizingDetailList = new List<MatchingJournalizingDetail>();

                int rowCount = grid.Rows.Count;
                var matchingJournalizingDetail = new List<MatchingJournalizingDetail>();

                Func<Row, bool> isCheckModified = (row) =>
                    Convert.ToInt32(row[CellName("CheckBox")].Value) != 0;

                Func<Row, bool> isExistIdModified = (row) =>
                    Convert.ToInt32(row[CellName("CheckBox")].Value) != 0 && row[CellName("HeaderId")].Value != null
                    && row[CellName("HeaderId")].Value is int && ((listId != null
                    && !listId.Contains(Convert.ToInt32(row[CellName("HeaderId")].Value.ToString())) || listId == null));

                var grdMatchingList = ((IEnumerable)grid.DataSource).Cast<MatchingJournalizingDetail>().ToArray();
                foreach (var row in grid.Rows.Where(x => isCheckModified(x)))
                {
                    MatchingJournalizingDetailList.Add(grdMatchingList[row.Index]);
                    if (isExistIdModified(row))
                    {
                        var mjc = new MatchingJournalizingDetail();
                        mjc.JournalizingType = Convert.ToInt32(row[CellName(nameof(MatchingJournalizingDetail.JournalizingType))].Value);
                        mjc.Id = Convert.ToInt32(row[CellName("HeaderId")].Value.ToString());
                        mjc.UpdateBy = Login.UserId;
                        listId.Add(Convert.ToInt32(row[CellName("HeaderId")].Value.ToString()));
                        matchingJournalizingDetail.Add(mjc);
                    }
                }

                if (MatchingJournalizingDetailList.Any())
                {
                    if (!Export()) return;
                    Print();
                    Cancel(matchingJournalizingDetail.ToArray());
                }
                else
                {
                    ShowWarningDialog(MsgWngNotExistUpdateData, "仕訳取消を行うデータ");
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        /// <summary> Export Selected Data In Grid </summary>
        /// <returns>Exported Result (true or false)</returns>
        private bool Export()
        {
            ClearStatusMessage();
            string serverPath = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                serverPath = await GetServerPath();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (!Directory.Exists(serverPath))
            {
                serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            var filePath = string.Empty;
            var fileName = $"消込仕訳取消{DateTime.Today:yyyyMMdd}.csv";
            if (!ShowSaveFileDialog(serverPath, fileName, out filePath,
                confirmMessagging: () => true,
                cancellationMessaging: () => DispStatusMessage(MsgInfCancelProcess, "消込仕訳取消"))) return false;

            var definition = new MatchingJournalizingCancelFileDefinition(new DataExpression(ApplicationControl));
            if (definition.CurrencyCodeField.Ignored = (!UseForeignCurrency))
            {
                definition.CurrencyCodeField.FieldName = null;
            }

            int precision = UseForeignCurrency ? Precision : 0;
            var format = precision == 0 ? "0" : "0." + new string('0', precision);
            definition.AmountField.Format = value => value.ToString(format);
            definition.ReceiptAmountField.Format = value => value.ToString(format);
            definition.BillingAmountField.Format = value => value.ToString(format);

            var exporter = definition.CreateExporter();
            exporter.UserId = Login.UserId;
            exporter.UserCode = Login.UserCode;
            exporter.CompanyId = CompanyId;
            exporter.CompanyCode = Login.CompanyCode;

            ProgressDialog.Start(ParentForm, (cancel, progress) =>
            {
                return exporter.ExportAsync(filePath, MatchingJournalizingDetailList, cancel, progress);
            }, true, SessionKey);

            if (exporter.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                ShowWarningDialog(MsgErrExportError);
                return false;
            }

            DispStatusMessage(MsgInfFinishExport);

            return true;
        }

        /// <summary> Print Selected Data In Grid </summary>
        private void Print()
        {
            ClearStatusMessage();
            string serverPath = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                serverPath = await GetServerPath();
            });
            ProgressDialog.Start(ParentForm, Task.Run(() => task), false, SessionKey);

            MatchingJournalizingCancellationReport matchingJournalizingCancelReport = null;

            matchingJournalizingCancelReport = new MatchingJournalizingCancellationReport();
            matchingJournalizingCancelReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
            matchingJournalizingCancelReport.Name = "消込仕訳取消リスト" + DateTime.Today.ToString("yyyyMMdd");
            matchingJournalizingCancelReport.SetData(MatchingJournalizingDetailList, Precision, UseForeignCurrency);

            var matchingJournalizingReportCondition = new SearchConditionSectionReport();
            matchingJournalizingReportCondition.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName, "消込仕訳取消リスト");
            matchingJournalizingReportCondition.Name = "消込仕訳取消リスト" + DateTime.Today.ToString("yyyyMMdd");
            matchingJournalizingReportCondition.SetPageDataSetting(SearchField);
            ProgressDialog.Start(ParentForm, Task.Run(() =>
            {
                matchingJournalizingCancelReport.Run(false);
                matchingJournalizingReportCondition.SetPageNumber(matchingJournalizingCancelReport.Document.Pages.Count);
                matchingJournalizingReportCondition.Run(false);
            }), false, SessionKey);

            matchingJournalizingCancelReport.Document.Pages.AddRange((GrapeCity.ActiveReports.Document.Section.PagesCollection)matchingJournalizingReportCondition.Document.Pages.Clone());

            ShowDialogPreview(ParentForm, matchingJournalizingCancelReport, serverPath);

        }

        /// <summary>Get ServerPath</summary>
        /// <returns>Server Path</returns>
        private async Task<string> GetServerPath()
        {
            var serverPath = string.Empty;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });
            return serverPath;
        }

        /// <summary>SearchCondition For Report </summary>
        private void ReportSearchCondition()
        {
            SearchField = new List<object>();
            string checkValue = null;
            if (datFromCreateAt.Value.HasValue && datToCreateAt.Value.HasValue)
            {
                SearchField.Add(new SearchData("更新日付", datFromCreateAt.Value.Value.ToShortDateString() + " ～ " + datToCreateAt.Value.Value.ToShortDateString()));
            }

            if (!string.IsNullOrWhiteSpace(txtCustomer.Text))
            {
                SearchField.Add(new SearchData("得意先コード", txtCustomer.Text + " : " + lblCustomername.Text));
            }
            else
            {
                SearchField.Add(new SearchData("得意先コード", "(指定なし)"));
            }

            if (cbxMatching.Checked)
            {
                checkValue += "消込仕訳/";
            }

            if (cbxReceiptExclude.Checked)
            {
                checkValue += "対象外仕訳/";
            }

            if (cbxAdvanceReceivedOccured.Checked)
            {
                checkValue += "前受仕訳/";
            }

            if (checkValue != null)
            {
                checkValue = checkValue.Substring(0, checkValue.Length - 1);
                SearchField.Add(new SearchData("仕訳区分", checkValue));
            }

            if (UseForeignCurrency)
            {
                SearchField.Add(new SearchData("通貨コード", txtCurrencyCode.Text));
            }
        }

        /// <summary>Cancel Matching Journalizing </summary>
        /// <param name="matchingJounalinzingDetail"> Array Of MatchingJournalizingDetail For Update</param>
        private void Cancel(MatchingJournalizingDetail[] matchingJounalinzingDetail)
        {
            var success = false;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<MatchingServiceClient>();
                var result = await service.CancelMatchingJournalizingDetailAsync(SessionKey, matchingJounalinzingDetail);
                if (result.ProcessResult.Result)
                    success = true;
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (success)
            {
                Clear();
                DispStatusMessage(MsgInfUpdateSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrUpdateError);
            }
        }
        #endregion

        #region 画面の F8
        [OperationLog("全選択")]
        private void SelectAll()
        {
            ClearStatusMessage();
            grid.EndEdit();
            for (int i = 0; i < grid.RowCount; i++)
            {
                (grid.Rows[i].Cells[1] as CheckBoxCell).Value = 1;
            }
        }
        #endregion

        #region 画面の F9
        [OperationLog("全解除")]
        private void DeselectAll()
        {
            ClearStatusMessage();
            grid.EndEdit();
            for (int i = 0; i < grid.RowCount; i++)
            {
                (grid.Rows[i].Cells[1] as CheckBoxCell).Value = 0;
            }
        }
        #endregion

        #region 画面の F10
        [OperationLog("終了")]
        private void Exit()
        {
            ParentForm.Close();
        }

        private void ReturnToSearchCondition()
        {
            tbcJournalizingDetail.SelectedIndex = 0;
        }
        #endregion

        #region 画面の GridLoader
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtCustomer.Text     = customer.Code;
                lblCustomername.Text = customer.Name;
                CustomerId = customer.Id;
                ClearStatusMessage();
            }
        }

        private void btnCurrencyCode_Click(object sender, EventArgs e)
        {
            var currency = this.ShowCurrencySearchDialog();
            if (currency != null)
            {
                txtCurrencyCode.Text = currency.Code.ToString();
                CurrencyId = currency.Id;
                Precision = currency.Precision;
                ClearStatusMessage();
            }
        }

        private void btnAddCustomerCode_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                txtaddCustomerCode.Text = customer.Code;
                txtaddCustomerName.Text = customer.Name;
                ClearStatusMessage();
            }
        }
        #endregion

        #region 画面の CusorOut Event
        private void txtCustomer_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCustomer.Text))
                {
                    var customerList = new List<Customer>();
                    customerList = GetCustomer(txtCustomer.Text);
                    if (!customerList.Any())
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "得意先", txtCustomer.Text);
                        txtCustomer.Clear();
                        lblCustomername.Clear();
                        txtCustomer.Focus();
                    }
                    else
                    {
                        txtCustomer.Text = customerList[0].Code.ToString();
                        lblCustomername.Text = customerList[0].Name.ToString();
                        CustomerId = customerList[0].Id;
                        ClearStatusMessage();
                    }
                }
                else
                {
                    txtCustomer.Clear();
                    lblCustomername.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtaddCustomerCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtaddCustomerCode.Text))
                {
                    var customerList = new List<Customer>();
                    customerList = GetCustomer(txtaddCustomerCode.Text);
                    if (!customerList.Any())
                    {
                        txtaddCustomerCode.Text = txtaddCustomerCode.Text;
                        txtaddCustomerName.Clear();
                        ClearStatusMessage();
                    }
                    else
                    {
                        txtaddCustomerCode.Text = customerList[0].Code.ToString();
                        txtaddCustomerName.Text = customerList[0].Name.ToString();
                        ClearStatusMessage();
                    }
                }
                else
                {
                    txtaddCustomerCode.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>Get Customer Info</summary>
        /// <param name="CustomerCode"></param>
        /// <returns>List Of Customers</returns>
        private List<Customer> GetCustomer(string CustomerCode)
        {
            List<Customer> list = null;
            CustomersResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { CustomerCode });

                if (result.ProcessResult.Result)
                {
                    list = result.Customers;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return list ?? new List<Customer>();
        }
        #endregion

        #region 照合結果検索　event

        private void btnCustomerNameSearch_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            this.ButtonClicked(btnCustomerNameSearch);
            string name = txtaddCustomerName.Text.ToLower();
            FindNameFlg = false;
            NameCount = 0;

            if (PrevNameSearchKey != name)
            {
                PrevNameIndex = -1;
                LastNameIndex = 0;
                PrevNameSearchKey = name;
            }

            if (!string.IsNullOrEmpty(txtaddCustomerName.Text) && !string.IsNullOrWhiteSpace(txtaddCustomerName.Text))
            {
                foreach (Row row in grid.Rows)
                {
                    row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerName))].Selected = false;

                    if (row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerName))].Value != null)
                    {
                        string rowName = row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerName))].Value.ToString().ToLower();

                        if (rowName.Contains(name))
                        {
                            FindNameFlg = true;
                            LastNameIndex = row.Index;
                            ++NameCount;
                        }
                    }
                }

                if (FindNameFlg == false)
                {
                    ClearNameResult();

                    if (grid.RowCount > 0)
                    {
                        grid.CurrentCell.Selected = true;
                    }

                    ShowWarningDialog(MsgWngNotExistSearchData);
                }
                else
                {
                    foreach (Row row in grid.Rows)
                    {
                        if (row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerName))].Value != null)
                        {
                            string rowName = row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerName))].Value.ToString().ToLower();

                            if (rowName.Contains(name))
                            {
                                if (PrevNameIndex == -1)    // first search
                                {
                                    PrevNameIndex = row.Index;
                                    grid.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingJournalizingDetail.CustomerName)));
                                    row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerName))].Selected = true;
                                    break;
                                }
                                else if (row.Index > PrevNameIndex)
                                {
                                    PrevNameIndex = row.Index;
                                    if (PrevNameIndex == LastNameIndex)
                                    {
                                        PrevNameIndex = -1;
                                    }
                                    grid.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingJournalizingDetail.CustomerName)));
                                    row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerName))].Selected = true;
                                    break;
                                }
                                else
                                {
                                    if (NameCount == 1)
                                    {
                                        PrevNameIndex = row.Index;
                                        grid.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingJournalizingDetail.CustomerName)));
                                        row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerName))].Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ClearSearchResult()
        {
            ClearCodeResult();
            ClearNameResult();
            ClearPayerResult();
        }

        private void ClearCodeResult()
        {
            PrevCodeIndex = -1;
            LastCodeIndex = 0;
            FindCodeFlg = false;
            CodeCount = 0;
            PrevCodeSearchKey = "";
        }

        private void ClearNameResult()
        {
            PrevNameIndex = -1;
            LastNameIndex = 0;
            FindNameFlg = false;
            NameCount = 0;
            PrevNameSearchKey = "";
        }

        private void ClearPayerResult()
        {
            PrevPayerIndex = -1;
            LastPayerIndex = 0;
            FindPayerFlg = false;
            PayerCount = 0;
            PrevPayerSearchKey = "";
        }

        private void btnCustomerCodeSearch_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            this.ButtonClicked(btnCustomerCodeSearch);
            FindCodeFlg = false;
            CodeCount = 0;
            string code = txtaddCustomerCode.Text.ToLower();

            if (PrevCodeSearchKey != code)
            {
                PrevCodeIndex = -1;
                LastCodeIndex = 0;
                PrevCodeSearchKey = code;
            }

            if (!string.IsNullOrEmpty(txtaddCustomerCode.Text) && !string.IsNullOrWhiteSpace(txtaddCustomerCode.Text))
            {
                foreach (Row row in grid.Rows)
                {
                    row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerCode))].Selected = false;

                    if (row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerCode))].Value != null)
                    {
                        string rowCode = row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerCode))].Value.ToString().ToLower();

                        if (rowCode.Contains(code))
                        {
                            FindCodeFlg = true;
                            LastCodeIndex = row.Index;
                            ++CodeCount;
                        }
                    }
                }

                if (FindCodeFlg == false)
                {
                    ClearCodeResult();

                    if (grid.RowCount > 0)
                    {
                        grid.CurrentCell.Selected = true;
                    }

                    ShowWarningDialog(MsgWngNotExistSearchData);
                }
                else
                {
                    foreach (Row row in grid.Rows)
                    {
                        if (row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerCode))].Value != null)
                        {
                            string rowCode = row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerCode))].Value.ToString().ToLower();

                            if (rowCode.Contains(code))
                            {
                                if (PrevCodeIndex == -1)    // first search
                                {
                                    PrevCodeIndex = row.Index;
                                    grid.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingJournalizingDetail.CustomerCode)));
                                    row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerCode))].Selected = true;
                                    break;
                                }
                                else if (row.Index > PrevCodeIndex)
                                {
                                    PrevCodeIndex = row.Index;
                                    if (PrevCodeIndex == LastCodeIndex)
                                    {
                                        PrevCodeIndex = -1;
                                    }
                                    grid.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingJournalizingDetail.CustomerCode)));
                                    row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerCode))].Selected = true;
                                    break;
                                }
                                else
                                {
                                    if (CodeCount == 1)
                                    {
                                        PrevCodeIndex = row.Index;
                                        grid.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingJournalizingDetail.CustomerCode)));
                                        row.Cells[CellName(nameof(MatchingJournalizingDetail.CustomerCode))].Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnPayerName_Click(object sender, EventArgs e)
        {
            ClearStatusMessage();
            this.ButtonClicked(btnPayerName);
            string payername = txtPayerName.Text.ToLower();
            FindPayerFlg = false;
            PayerCount = 0;

            if (PrevPayerSearchKey != payername)
            {
                PrevPayerIndex = -1;
                LastPayerIndex = 0;
                PrevPayerSearchKey = payername;
            }

            if (!string.IsNullOrEmpty(txtPayerName.Text) && !string.IsNullOrWhiteSpace(txtPayerName.Text))
            {
                foreach (Row row in grid.Rows)
                {
                    row.Cells[CellName(nameof(MatchingJournalizingDetail.PayerName))].Selected = false;

                    if (row.Cells[CellName(nameof(MatchingJournalizingDetail.PayerName))].Value != null)
                    {
                        string rowName = row.Cells[CellName(nameof(MatchingJournalizingDetail.PayerName))].Value.ToString().ToLower();

                        if (rowName.Contains(payername))
                        {
                            FindPayerFlg = true;
                            LastPayerIndex = row.Index;
                            ++PayerCount;
                        }
                    }
                }

                if (FindPayerFlg == false)
                {
                    ClearPayerResult();

                    if (grid.RowCount > 0)
                    {
                        grid.CurrentCell.Selected = true;
                    }

                    ShowWarningDialog(MsgWngNotExistSearchData);
                }
                else
                {
                    foreach (Row row in grid.Rows)
                    {
                        if (row.Cells[CellName(nameof(MatchingJournalizingDetail.PayerName))].Value != null)
                        {
                            string rowName = row.Cells[CellName(nameof(MatchingJournalizingDetail.PayerName))].Value.ToString().ToLower();

                            if (rowName.Contains(payername))
                            {
                                if (PrevPayerIndex == -1)    // first search
                                {
                                    PrevPayerIndex = row.Index;
                                    grid.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingJournalizingDetail.PayerName)));
                                    row.Cells[CellName(nameof(MatchingJournalizingDetail.PayerName))].Selected = true;
                                    break;
                                }
                                else if (row.Index > PrevPayerIndex)
                                {
                                    PrevPayerIndex = row.Index;
                                    if (PrevPayerIndex == LastPayerIndex)
                                    {
                                        PrevPayerIndex = -1;
                                    }
                                    grid.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingJournalizingDetail.PayerName)));
                                    row.Cells[CellName(nameof(MatchingJournalizingDetail.PayerName))].Selected = true;
                                    break;
                                }
                                else
                                {
                                    if (PayerCount == 1)
                                    {
                                        PrevPayerIndex = row.Index;
                                        grid.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingJournalizingDetail.PayerName)));
                                        row.Cells[CellName(nameof(MatchingJournalizingDetail.PayerName))].Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
                {
                    ProgressDialog.Start(ParentForm, CurrencyInfo(), false, SessionKey);

                    if (Currency == null)
                    {
                        ShowWarningDialog(MsgWngMasterNotExist, "通貨", txtCurrencyCode.Text);
                        txtCurrencyCode.Focus();
                        txtCurrencyCode.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.CellName == CellName("CheckBox"))
                ChangeCellValue(e.RowIndex);
        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.CellName == CellName("CheckBox"))
                ChangeCellValue(e.RowIndex);
        }

        /// <summary>CheckBox CellValue Change</summary>
        /// <param name="cellName"> Row Index of Current Check Row</param>
        private void ChangeCellValue(int rowIndex)
        {
            var id = Convert.ToInt32(grid.Rows[rowIndex][CellName("HeaderId")].Value);
            var jounalizingType = Convert.ToInt32(grid.Rows[rowIndex][CellName(nameof(MatchingJournalizingDetail.JournalizingType))].Value);
            var checkVal = (bool)grid.Rows[rowIndex][CellName("CheckBox")].EditedFormattedValue;

            Func<Row, bool> isCheckModified = (row) =>
                Convert.ToInt32(row[CellName("HeaderId")].Value) == id && Convert.ToInt32(row[CellName(nameof(MatchingJournalizingDetail.JournalizingType))].Value) == jounalizingType;

            foreach (var row in grid.Rows.Where(x => isCheckModified(x)))
            {
                if (checkVal)
                    (grid.Rows[row.Index][CellName("CheckBox")] as CheckBoxCell).Value = 1;
                else
                    (grid.Rows[row.Index][CellName("CheckBox")] as CheckBoxCell).Value = 0;
            }
        }
        #endregion
    }
}
