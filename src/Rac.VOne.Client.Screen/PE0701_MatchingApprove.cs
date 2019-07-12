using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.MatchingService;
using Rac.VOne.Export;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using Section = Rac.VOne.Web.Models.Section;

namespace Rac.VOne.Client.Screen
{
    /// <summary>消込データ承認</summary>
    public partial class PE0701 : VOneScreenBase
    {
        private int Precision { get; set; }
        private bool FindNameFlg { get; set; }
        private int NameCount { get; set; }
        private string PrevNameSearchKey { get; set; } = string.Empty;
        private int PrevPayerIndex { get; set; } = -1;
        private int LastPayerIndex { get; set; }
        private bool FindPayerFlg { get; set; }
        private int PayerCount { get; set; }
        private string PrevPayerSearchKey { get; set; } = string.Empty;
        private string PrevCodeSearchKey { get; set; } = string.Empty;
        private int PrevNameIndex { get; set; } = -1;
        private int LastNameIndex { get; set; }
        private int PrevCodeIndex { get; set; } = -1;
        private int LastCodeIndex { get; set; }
        private bool FindCodeFlg { get; set; }
        private int CodeCount { get; set; }
        private int FormatNumber { get; set; }
        private bool AllSection { get; set; }
        private List<int> SectionIds { get; set; }
        private List<int> DepartmentIds { get; set; }
        private Color MatchingGridBillingBackColor { get; set; }
        private Color GridLineColor { get; set; }
        private List<Section> SectionsWithLoginUser { get; set; }
        private List<Section> Sections { get; set; }
        private List<Department> DepartmentsWithLoginUser { get; set; }
        private List<Department> Departments { get; set; }

        private string CellName(string value) => $"cel{value}";
        private bool IsChecked(Row row) => Convert.ToBoolean(row[CellName("CheckBox")].Value);
        private bool IsGridModified { get { return grdApprovalResult.Rows.Any(x => IsChecked(x)); } }

        #region 画面初期化

        public PE0701()
        {
            InitializeComponent();
            grdApprovalResult.SetupShortcutKeys();
            Text = "消込データ承認";
            InitializeHandlers();
        }


        private void InitializeHandlers()
        {
            tbcMatchingApproval.SelectedIndexChanged += (sender, e) =>
            {
                if (tbcMatchingApproval.SelectedIndex == 0)
                {
                    BaseContext.SetFunction10Caption("終了");
                    OnF10ClickHandler = ExitForm;
                }
                else
                {
                    BaseContext.SetFunction10Caption("戻る");
                    OnF10ClickHandler = ReturnToSearchCondition;
                }
            };
        }
        private void PE0701_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                var tasks = new List<Task>();
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadDepartmentAsync());
                tasks.Add(LoadDepartmentsByLoginUserIdAsync());
                tasks.Add(LoadSectionAsync());
                tasks.Add(LoadSectionWithLoginAsync());
                tasks.Add(LoadControlColorAsync());

                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);

                SetApplicationSetting();
                InitialApprovalGridSetting();
                SuspendLayout();
                tbcMatchingApproval.SelectedIndex = 0;
                ResumeLayout();
                InitializeDepartmentSelection();
                InitializeSectionSelection();
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
            OnF01ClickHandler = ApprovalSearchData;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("承認");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = ExportData;

            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = SelectAll;

            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = DeselectAll;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = ExitForm;
        }

        private void InitializeDepartmentSelection()
        {
            if (!UseSection || DepartmentsWithLoginUser.Count == 0)
            {
                lblDepartmentName.Text = "すべて";
                DepartmentIds = Departments.Select(x => x.Id).ToList();
            }
            else
            {
                if (Departments.Count == DepartmentsWithLoginUser.Count)
                {
                    lblDepartmentName.Text = "すべて";
                }
                else if (DepartmentsWithLoginUser.Count == 1)
                {
                    lblDepartmentName.Text = DepartmentsWithLoginUser.First().Name;
                }
                else
                {
                    lblDepartmentName.Text = "請求部門絞込有";
                }
                DepartmentIds = DepartmentsWithLoginUser.Select(x => x.Id).ToList();
            }
        }

        private void InitializeSectionSelection()
        {
            if (SectionsWithLoginUser.Count == 0)
            {
                lblSectionName.Text = "すべて";
                SectionIds = Sections.Select(item => item.Id).ToList();
            }
            else
            {
                if (Sections.Count == SectionsWithLoginUser.Count)
                {
                    lblSectionName.Text = "すべて";
                }
                else if (SectionsWithLoginUser.Count == 1)
                {
                    lblSectionName.Text = SectionsWithLoginUser.First().Name;
                }
                else
                {
                    lblSectionName.Text = "入金部門絞込有";
                }
                SectionIds = SectionsWithLoginUser.Select(x => x.Id).ToList();
            }
        }


        /// <summary>Set Initial Control</summary>
        private void SetApplicationSetting()
        {
            var expression = new DataExpression(ApplicationControl);
            txtCustomerCode.Format = expression.CustomerCodeFormatString;
            txtCustomerCode.ImeMode = expression.CustomerCodeImeMode();
            txtCustomerCode.MaxLength = expression.CustomerCodeLength;
            txtCustomerCode.PaddingChar = expression.CustomerCodePaddingChar;

            pnlSection.Visible = UseSection;

            if (!UseForeignCurrency)
            {
                lblLength.Hide();
                nmbLength.Hide();
                btnIncrease.Hide();
                btnDecrease.Hide();
            }

            if (ColorContext != null)
            {
                MatchingGridBillingBackColor = ColorContext.MatchingGridBillingBackColor;
                GridLineColor = ColorContext.GridLineColor;
            }
            nmbLength.Value = Settings.RestoreControlValue<PE0701, decimal>(Login, nmbLength.Name) ?? 2M;
            FormatNumber = Convert.ToInt32(nmbLength.Value);
        }
        #endregion

        private void btnSectionName_Click(object sender, EventArgs e)
        {
            var allSelected = Sections.Count == SectionIds.Count;

            using (var form = ApplicationContext.Create(nameof(PE0106)))
            {
                var screen = form.GetAll<PE0106>().First();
                screen.AllSection = allSelected;
                screen.InitialIds = SectionIds;

                screen.InitializeParentForm("入金部門選択");

                var dialogResult = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (dialogResult == DialogResult.OK)
                {
                    ClearStatusMessage();
                    lblSectionName.Text = screen.SelectedState;
                    SectionIds = screen.SelectedIds;
                }
            }
        }

        private void btnInitializeSectionSelection_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnInitializeSectionSelection);
            InitializeSectionSelection();
        }

        private void btnDepartmentCode_Click(object sender, EventArgs e)
        {
            var allSelected = Departments.Count == DepartmentIds.Count;

            using (var form = ApplicationContext.Create(nameof(PE0114)))
            {
                var screen = form.GetAll<PE0114>().First();
                screen.AllSelected = allSelected;
                screen.InitialIds = DepartmentIds;
                screen.InitializeParentForm("請求部門絞込");

                var res = ApplicationContext.ShowDialog(ParentForm, form, true);

                if (res == DialogResult.OK)
                {
                    ClearStatusMessage();
                    lblDepartmentName.Text = screen.SelectedState;
                    DepartmentIds = screen.SelectedIds;
                }
            }
        }

        private void btnInitializeDepartmentSelection_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnInitializeDepartmentSelection);
            InitializeDepartmentSelection();
        }


        #region グリッド作成
        private void InitialApprovalGridSetting()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);

            if (UseForeignCurrency)
            {
                nmbLength.Value = FormatNumber;
                Precision = FormatNumber;

                if (FormatNumber == 5)
                {
                    btnIncrease.Enabled = false;
                }
                else if (FormatNumber == 0)
                {
                    btnDecrease.Enabled = false;
                }
            }

            var height = builder.DefaultRowHeight;
            var template = new Template();
            var checkBoxCaption = (cbxApprovalData.Checked) ? "解除" : "承認";
            var widthCcy = UseForeignCurrency ? 40 : 0;
            var widthCustomer = 160 - widthCcy;
            Precision = (UseForeignCurrency) ? Precision : 0;

            #region header row1

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  90, "SpaceStart"),
                new CellSetting(height, 425, "BillingInfo", caption: "請求情報"),
                new CellSetting(height, 250, "ReceiptInfo", caption: "入金情報"),
                new CellSetting(height, 175, "SpaceEnd")
            });
            builder.BuildHeaderOnly(template);
            builder.Items.Clear();
            #endregion

            #region header row2
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,            30, "Header"),
                new CellSetting(height,            30, "CheckBox"                             , caption: checkBoxCaption),
                new CellSetting(height,            30, "Individual"                           , caption: "個別"),
                new CellSetting(height,      widthCcy, nameof(MatchingHeader.CurrencyCode)    , caption: "通貨" ),
                new CellSetting(height,           115, nameof(MatchingHeader.DispCustomerCode)    , caption: "得意先コード"),
                new CellSetting(height, widthCustomer, nameof(MatchingHeader.DispCustomerName)    , caption: "得意先名（代表者）"),
                new CellSetting(height,            30, nameof(MatchingHeader.BillingCount)    , caption: "件数"),
                new CellSetting(height,           120, nameof(MatchingHeader.BillingAmount)   , caption: "金額"),
                new CellSetting(height,           100, nameof(MatchingHeader.PayerName)       , caption: "振込依頼人名"),
                new CellSetting(height,            30, nameof(MatchingHeader.ReceiptCount)    , caption: "件数"),
                new CellSetting(height,           120, nameof(MatchingHeader.ReceiptAmount)   , caption: "金額"),
                new CellSetting(height,            50, nameof(MatchingHeader.ShareTransferFee), caption: "手数科"),
                new CellSetting(height,            90, nameof(MatchingHeader.DispDifferent)   , caption: "差額"),
                new CellSetting(height,            35, nameof(MatchingHeader.DispMemo)        , caption: "メモ"),
            });
            builder.BuildHeaderOnly(template);
            #endregion

            #region rowItems

            var thickBorder = builder.GetBorder(right: LineStyle.Medium);
            var backColor = MatchingGridBillingBackColor;

            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var middleRight = MultiRowContentAlignment.MiddleRight;

            builder.Items.Clear();
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,            30, "Header"                               , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,            30, "CheckBox"                             , readOnly: false, dataField: "Checked", cell: builder.GetCheckBoxCell(isBoolType: true)),
                new CellSetting(height,            30, "Individual"                           , cell: builder.GetButtonCell(), value: "…"),
                new CellSetting(height,      widthCcy, nameof(MatchingHeader.CurrencyCode)    , dataField: nameof(MatchingHeader.CurrencyCode)          , cell: builder.GetTextBoxCell(middleCenter)     , backColor: backColor),
                new CellSetting(height,           115, nameof(MatchingHeader.DispCustomerCode), dataField: nameof(MatchingHeader.DispCustomerCode)      , cell: builder.GetTextBoxCell(middleCenter)     , backColor: backColor),
                new CellSetting(height, widthCustomer, nameof(MatchingHeader.DispCustomerName), dataField: nameof(MatchingHeader.DispCustomerName)      , cell: builder.GetTextBoxCell()                 , backColor: backColor),
                new CellSetting(height,            30, nameof(MatchingHeader.BillingCount)    , dataField: nameof(MatchingHeader.DispBillingCount)      , cell: builder.GetNumberCell(middleRight)       , backColor: backColor),
                new CellSetting(height,           120, nameof(MatchingHeader.BillingAmount)   , dataField: nameof(MatchingHeader.DispBillingAmount)     , cell: builder.GetTextBoxCurrencyCell(Precision), backColor: backColor, border: thickBorder),
                new CellSetting(height,           100, nameof(MatchingHeader.PayerName)       , dataField: nameof(MatchingHeader.PayerName)             , cell: builder.GetTextBoxCell()),
                new CellSetting(height,            30, nameof(MatchingHeader.ReceiptCount)    , dataField: nameof(MatchingHeader.DispReceiptCount)      , cell: builder.GetNumberCell(middleRight)),
                new CellSetting(height,           120, nameof(MatchingHeader.ReceiptAmount)   , dataField: nameof(MatchingHeader.DispReceiptAmount)     , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height,            50, nameof(MatchingHeader.ShareTransferFee), dataField: nameof(MatchingHeader.DispShareTransferFee)  , cell: builder.GetTextBoxCell(middleCenter)),
                new CellSetting(height,            90, nameof(MatchingHeader.DispDifferent)   , dataField: nameof(MatchingHeader.DispDifferent)         , cell: builder.GetTextBoxCurrencyCell(Precision)),
                new CellSetting(height,            35, nameof(MatchingHeader.DispMemo)        , dataField: nameof(MatchingHeader.DispMemo)              , cell: builder.GetTextBoxCell(middleCenter)),
                new CellSetting(height,             0, nameof(MatchingHeader.Id)              , dataField: nameof(MatchingHeader.Id))
            });

            builder.BuildRowOnly(template);
            grdApprovalResult.Template = template;
            #endregion

            grdApprovalResult.DefaultCellStyle.BackColor = Color.Empty;
            grdApprovalResult.AlternatingRowsDefaultCellStyle.BackColor = Color.Empty;
        }
        #endregion

        #region Function Key Event
        [OperationLog("検索")]
        private void ApprovalSearchData()
        {
            ClearStatusMessage();
            try
            {
                NLogHandler.WriteDebug(this, "消込承認処理 検索開始");
                ProgressDialog.Start(ParentForm, SearchMatchedData(true), false, SessionKey);
                NLogHandler.WriteDebug(this, "消込承認処理 検索終了");
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private CollationSearch GetSearchCondition() => new CollationSearch {
            CompanyId       = CompanyId,
            Approved        = cbxApprovalData.Checked, 
            CreateAtFrom    = datCreateAtFrom.Value,
            CreateAtTo      = datCreateAtTo.Value,
        };

        /// <summary> 検索処理</summary>
        private async Task SearchMatchedData(bool isShowMsgDialog = false)
        {

            var clientKey = await Util.CreateClientKey(Login, nameof(PE0701));
            var useDepartmentFilter = Departments.Count != DepartmentIds.Count;
            var useSectionFilter = Sections.Count != SectionIds.Count;

            var option = GetSearchCondition();
            option.ClientKey = clientKey;
            option.UseDepartmentWork = useDepartmentFilter;
            option.UseSectionWork = useSectionFilter;

            if (useDepartmentFilter)
                await Util.SaveWorkDepartmentTargetAsync(Login, clientKey, DepartmentIds.ToArray());

            if (useSectionFilter)
                await Util.SaveWorkSectionTargetAsync(Login, clientKey, SectionIds.ToArray());

            var result = await ServiceProxyFactory.DoAsync(async (MatchingServiceClient client)
                => await client.SearchMatchedDataAsync(SessionKey, option, string.Empty));

            if (result.ProcessResult.Result)
            {
                var matchingHeader = result.MatchingHeaders;
                grdApprovalResult.DataSource = new BindingSource(matchingHeader, null);

                if (matchingHeader.Any())
                {
                    ClearStatusMessage();
                    tbcMatchingApproval.SelectedTab = tbpSearchResult;
                    BaseContext.SetFunction03Enabled(true);
                    BaseContext.SetFunction06Enabled(true);
                    BaseContext.SetFunction08Enabled(true);
                    BaseContext.SetFunction09Enabled(true);
                    cbxApprovalData.Enabled = false;

                    if (cbxApprovalData.Checked)
                    {
                        OnF03ClickHandler = ApprovalCancel;
                    }
                    else
                    {
                        OnF03ClickHandler = Approve;
                    }
                }
                else
                {
                    if (isShowMsgDialog)
                        ShowWarningDialog(MsgWngNotExistSearchData);

                    BaseContext.SetFunction03Enabled(false);
                    BaseContext.SetFunction06Enabled(false);
                    BaseContext.SetFunction08Enabled(false);
                    BaseContext.SetFunction09Enabled(false);
                }
            }
            else
            {
                if (isShowMsgDialog)
                    ShowWarningDialog(MsgWngNotExistSearchData);
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

            ClearStatusMessage();
            txtCustomerCode.Clear();
            txtCustomerName.Clear();
            txtPayerName.Clear();

            cbxApprovalData.Checked = false;
            cbxApprovalData.Enabled = true;
            datCreateAtFrom.Clear();
            datCreateAtTo.Clear();

            grdApprovalResult.DataSource = null;
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            ClearSearchResult();
            tbcMatchingApproval.SelectedTab = tbpSearchCondition;
        }

        [OperationLog("承認解除")]
        private void ApprovalCancel()
        {
            try
            {
                ClearStatusMessage();
                grdApprovalResult.EndEdit();

                if (!ValidateApproveData()) return;

                if (!ShowConfirmDialog(MsgQstConfirmCancel))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                var items = grdApprovalResult.Rows
                    .Select(x => x.DataBoundItem as MatchingHeader)
                    .Where(x => x.Checked).ToArray();
                foreach (var item in items)
                    item.UpdateBy = Login.UserId;

                CancelApprovalMatching(items);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("承認")]
        private void Approve()
        {
            try
            {
                ClearStatusMessage();
                grdApprovalResult.EndEdit();

                if (!ValidateApproveData()) return;

                if (!ShowConfirmDialog(MsgQstConfirmApprove))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var items = grdApprovalResult.Rows
                    .Select(x => x.DataBoundItem as MatchingHeader)
                    .Where(x => x.Checked).ToArray();
                foreach (var item in items)
                    item.UpdateBy = Login.UserId;

                ApprovalMatching(items);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>承認解除データチェック</summary>
        /// <returns>bool</returns>
        private bool ValidateApproveData()
        {
            if (grdApprovalResult.Rows.Any(x => IsChecked(x))) return true;
            if (cbxApprovalData.Checked)
            {
                ShowWarningDialog(MsgWngSelectionRequired, "承認解除を行う明細");
                return false;
            }
            else
            {
                ShowWarningDialog(MsgWngSelectionRequired, "承認を行う明細");
                return false;
            }
        }

        /// <summary>承認解除処理</summary>
        /// <param name="matchingHeaderId"></param>
        private void CancelApprovalMatching(MatchingHeader[] headers)
        {
            var success = true;
            var updateValid = true;
            var task = ServiceProxyFactory.DoAsync(async (MatchingServiceClient client) =>
            {
                var result = await client.CancelApprovalAsync(SessionKey, headers);

                success = result?.ProcessResult.Result ?? false;
                updateValid = !(result.ProcessResult.ErrorCode == Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated);
                if (success) await SearchMatchedData();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (!updateValid)
            {
                ShowWarningDialog(MsgWngAlreadyUpdated);
                return;
            }
            if (!success)
            {
                ShowWarningDialog(MsgErrCancelApprovalError);
                return;
            }
            DispStatusMessage(MsgInfCancelApprovalSuccess);
        }

        /// <summary>承認処理</summary>
        private void ApprovalMatching(MatchingHeader[] headers)
        {
            var success = true;
            var updateValid = true;
            var task = ServiceProxyFactory.DoAsync(async (MatchingServiceClient client) =>
            {
                var result = await client.ApproveAsync(SessionKey, headers);

                success = result?.ProcessResult.Result ?? false;
                updateValid = !(result.ProcessResult.ErrorCode == Rac.VOne.Common.ErrorCode.OtherUserAlreadyUpdated);
                if (success) await SearchMatchedData();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (!updateValid)
            {
                ShowWarningDialog(MsgWngAlreadyUpdated);
                return;
            }

            if (!success)
            {
                ShowWarningDialog(MsgErrApprovalError);
                return;
            }
            DispStatusMessage(MsgInfApprovalSuccess);
        }

        [OperationLog("エクスポート")]
        private void ExportData()
        {
            ClearStatusMessage();
            try
            {
                var checkType = GetCheckedType();
                using (var form = ApplicationContext.Create(nameof(PE0112)))
                {
                    var screen = form.GetAll<PE0112>().First();
                    screen.OutputType = 1;
                    screen.ParentCheckType = checkType;
                    screen.InitializeParentForm($"消込データ{(cbxApprovalData.Checked ? "承認済" : "未承認")}チェックリスト　出力内容指定");
                    var result = ApplicationContext.ShowDialog(Parent, form, true);
                    if (result != DialogResult.OK)
                    {
                        DispStatusMessage(MsgInfCancelProcess, "エクスポート");
                        return;
                    }
                    checkType = screen.CheckType;
                }
                var items = grdApprovalResult.Rows.Select(x => x.DataBoundItem as MatchingHeader)
                    .Where(x => checkType == 0
                        || checkType == 1 && x.Checked
                        || checkType == 2 && !x.Checked).ToList();
                if (!items.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }
                var task = Util.GetGeneralSettingServerPathAsync(Login);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                var dir = task.Result;

                var filePath = string.Empty;
                var fileName = $"消込データ{(cbxApprovalData.Checked ? "承認済" : "未承認")}チェックリスト{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(dir, fileName, out filePath)) return;

                var definition = new MatchingHeaderFileDefinition(new DataExpression(ApplicationControl));
                if (!UseForeignCurrency)
                {
                    definition.CurrencyCodeField.Ignored = true;
                    definition.CurrencyCodeField.FieldName = null;
                }
                var format = Precision == 0 ? "0" : "0." + new string('0', Precision);
                definition.BillingAmountField.Format = value => value.ToString(format);
                definition.ReceiptAmountField.Format = value => value.ToString(format);
                definition.DifferentField.Format = value => value.ToString(format);
                if (!cbxApprovalData.Checked)
                    definition.CheckedField.FieldName = "承認";

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                    exporter.ExportAsync(filePath, items, cancel, progress),
                    true, SessionKey);

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
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }
        private int GetCheckedType()
        {
            var allChecked = true;
            var allUnchecked = true;
            foreach (var item in grdApprovalResult.Rows.Select(x => x.DataBoundItem as MatchingHeader))
            {
                allChecked &= item.Checked;
                allUnchecked &= !item.Checked;
                if (!allChecked && !allUnchecked) break;
            }
            return allChecked ? 1
                 : allUnchecked ? 2 : 0;
        }

        [OperationLog("全選択")]
        private void SelectAll()
        {
            ClearStatusMessage();
            grdApprovalResult.EndEdit();
            for (var i = 0; i < grdApprovalResult.RowCount; i++)
            {
                (grdApprovalResult.Rows[i].Cells[1] as CheckBoxCell).Value = true;
            }
        }

        [OperationLog("全解除")]
        private void DeselectAll()
        {
            ClearStatusMessage();
            grdApprovalResult.EndEdit();
            for (var i = 0; i < grdApprovalResult.RowCount; i++)
            {
                (grdApprovalResult.Rows[i].Cells[1] as CheckBoxCell).Value = false;
            }
        }

        [OperationLog("終了")]
        private void ExitForm()
        {
            try
            {
                if (IsGridModified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

                Settings.SaveControlValue<PE0701>(Login, nmbLength.Name, nmbLength.Value);
                ParentForm.Close();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        private void ReturnToSearchCondition()
        {
            tbcMatchingApproval.SelectedIndex = 0;
        }

        #region 表示 桁数 変更
        private void btnDecrease_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnDecrease);
            var currentVal = nmbLength.Value;
            nmbLength.Value = (currentVal <= 0) ? 0M : --currentVal;

            if (currentVal == 5)
            {
                btnIncrease.Enabled = false;
                btnDecrease.Enabled = true;
            }
            else if (currentVal == 0)
            {
                btnIncrease.Enabled = true;
                btnDecrease.Enabled = false;
            }
            else
            {
                btnIncrease.Enabled = true;
                btnDecrease.Enabled = true;
            }

            SetFieldValue();
        }

        /// <summary>Set Precision Number Field</summary>
        private void SetFieldValue()
        {
            if (nmbLength.Value.HasValue)
                FormatNumber = Convert.ToInt32(nmbLength.Value);

            var newBindingSource = new BindingSource();
            newBindingSource.DataSource = grdApprovalResult.DataSource;

            InitialApprovalGridSetting();
            grdApprovalResult.DataSource = newBindingSource;
        }

        private void btnIncrease_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnIncrease);
            var currentVal = nmbLength.Value;
            nmbLength.Value = (currentVal == 5) ? 5M : ++currentVal;

            if (currentVal == 5)
            {
                btnIncrease.Enabled = false;
                btnDecrease.Enabled = true;
            }
            else if (currentVal == 0)
            {
                btnIncrease.Enabled = true;
                btnDecrease.Enabled = false;
            }
            else
            {
                btnIncrease.Enabled = true;
                btnDecrease.Enabled = true;
            }

            SetFieldValue();
        }

        private void nmbLength_Leave(object sender, EventArgs e)
        {
            if (nmbLength.Value.HasValue)
            {
                var currentVal = Convert.ToInt32(nmbLength.Value);

                if (currentVal == 5)
                {
                    btnIncrease.Enabled = false;
                    btnDecrease.Enabled = true;
                }
                else if (currentVal == 0)
                {
                    btnIncrease.Enabled = true;
                    btnDecrease.Enabled = false;
                }
                else
                {
                    btnIncrease.Enabled = true;
                    btnDecrease.Enabled = true;
                }
            }

            SetFieldValue();
        }
        #endregion

        #region event handlers

        private void btnCustomerNameSearch_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnCustomerNameSearch);
            ClearStatusMessage();
            var name = txtCustomerName.Text.ToLower();
            FindNameFlg = false;
            NameCount = 0;

            if (PrevNameSearchKey != name)
            {
                PrevNameIndex = -1;
                LastNameIndex = 0;
                PrevNameSearchKey = name;
            }

            if (string.IsNullOrEmpty(txtCustomerName.Text)
                || string.IsNullOrWhiteSpace(txtCustomerName.Text)) return;

            foreach (Row row in grdApprovalResult.Rows)
            {
                row.Cells[CellName(nameof(MatchingHeader.DispCustomerName))].Selected = false;

                if (row.Cells[CellName(nameof(MatchingHeader.DispCustomerName))].Value != null)
                {
                    var rowName = row.Cells[CellName(nameof(MatchingHeader.DispCustomerName))].Value.ToString().ToLower();

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

                if (grdApprovalResult.RowCount > 0)
                {
                    grdApprovalResult.CurrentCell.Selected = true;
                }

                ShowWarningDialog(MsgWngNotExistSearchData);
            }
            else
            {
                foreach (Row row in grdApprovalResult.Rows)
                {
                    if (row.Cells[CellName(nameof(MatchingHeader.DispCustomerName))].Value != null)
                    {
                        var rowName = row.Cells[CellName(nameof(MatchingHeader.DispCustomerName))].Value.ToString().ToLower();

                        if (rowName.Contains(name))
                        {
                            if (PrevNameIndex == -1)
                            {
                                PrevNameIndex = row.Index;
                                grdApprovalResult.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingHeader.DispCustomerName)));
                                row.Cells[CellName(nameof(MatchingHeader.DispCustomerName))].Selected = true;
                                break;
                            }
                            else if (row.Index > PrevNameIndex)
                            {
                                PrevNameIndex = row.Index;
                                if (PrevNameIndex == LastNameIndex)
                                {
                                    PrevNameIndex = -1;
                                }
                                grdApprovalResult.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingHeader.DispCustomerName)));
                                row.Cells[CellName(nameof(MatchingHeader.DispCustomerName))].Selected = true;
                                break;
                            }
                            else
                            {
                                if (NameCount == 1)
                                {
                                    PrevNameIndex = row.Index;
                                    grdApprovalResult.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingHeader.DispCustomerName)));
                                    row.Cells[CellName(nameof(MatchingHeader.DispCustomerName))].Selected = true;
                                    break;
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
            PrevCodeSearchKey = string.Empty;
        }

        private void ClearNameResult()
        {
            PrevNameIndex = -1;
            LastNameIndex = 0;
            FindNameFlg = false;
            NameCount = 0;
            PrevNameSearchKey = string.Empty;
        }

        private void ClearPayerResult()
        {
            PrevPayerIndex = -1;
            LastPayerIndex = 0;
            FindPayerFlg = false;
            PayerCount = 0;
            PrevPayerSearchKey = string.Empty;
        }

        private void txtCustomerCode_Validated(object sender, EventArgs e)
        {
            ClearStatusMessage();
            var customerResult = new Customer();
            if (string.IsNullOrEmpty(txtCustomerCode.Text)) return;

            try
            {
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CustomerMasterClient>();
                    CustomersResult result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCustomerCode.Text });

                    customerResult = result.Customers.FirstOrDefault();

                    if (customerResult != null)
                    {
                        txtCustomerCode.Text = customerResult.Code;
                        txtCustomerName.Text = customerResult.Name;
                    }
                    else
                    {
                        txtCustomerName.Clear();
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void btnCustomerCode_Click(object sender, EventArgs e)
        {
            var customer = this.ShowCustomerMinSearchDialog();
            if (customer != null)
            {
                ClearStatusMessage();
                txtCustomerCode.Text = customer.Code;
                txtCustomerName.Text = customer.Name;
            }
        }

        private void btnCustomerCodeSearch_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnCustomerCodeSearch);
            ClearStatusMessage();
            FindCodeFlg = false;
            CodeCount = 0;

            var code = txtCustomerCode.Text.ToLower();

            if (PrevCodeSearchKey != code)
            {
                PrevCodeIndex = -1;
                LastCodeIndex = 0;
                PrevCodeSearchKey = code;
            }

            if (string.IsNullOrEmpty(txtCustomerCode.Text)
                || string.IsNullOrWhiteSpace(txtCustomerCode.Text)) return;

            foreach (Row row in grdApprovalResult.Rows)
            {
                row.Cells[CellName(nameof(MatchingHeader.DispCustomerCode))].Selected = false;

                if (row.Cells[CellName(nameof(MatchingHeader.DispCustomerCode))].Value != null)
                {
                    var rowCode = row.Cells[CellName(nameof(MatchingHeader.DispCustomerCode))].Value.ToString().ToLower();

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

                if (grdApprovalResult.RowCount > 0)
                {
                    grdApprovalResult.CurrentCell.Selected = true;
                }

                ShowWarningDialog(MsgWngNotExistSearchData);
            }
            else
            {
                foreach (Row row in grdApprovalResult.Rows)
                {
                    if (row.Cells[CellName(nameof(MatchingHeader.DispCustomerCode))].Value != null)
                    {
                        var rowCode = row.Cells[CellName(nameof(MatchingHeader.DispCustomerCode))].Value.ToString().ToLower();

                        if (rowCode.Contains(code))
                        {
                            if (PrevCodeIndex == -1)
                            {
                                PrevCodeIndex = row.Index;
                                grdApprovalResult.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingHeader.DispCustomerCode)));
                                row.Cells[CellName(nameof(MatchingHeader.DispCustomerCode))].Selected = true;
                                break;
                            }
                            else if (row.Index > PrevCodeIndex)
                            {
                                PrevCodeIndex = row.Index;
                                if (PrevCodeIndex == LastCodeIndex)
                                {
                                    PrevCodeIndex = -1;
                                }
                                grdApprovalResult.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingHeader.DispCustomerCode)));
                                row.Cells[CellName(nameof(MatchingHeader.DispCustomerCode))].Selected = true;
                                break;
                            }
                            else
                            {
                                if (CodeCount == 1)
                                {
                                    PrevCodeIndex = row.Index;
                                    grdApprovalResult.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingHeader.DispCustomerCode)));
                                    row.Cells[CellName(nameof(MatchingHeader.DispCustomerCode))].Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnPayerName_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnPayerName);
            ClearStatusMessage();
            var payerName = txtPayerName.Text.ToLower();
            FindPayerFlg = false;
            PayerCount = 0;

            if (PrevPayerSearchKey != payerName)
            {
                PrevPayerIndex = -1;
                LastPayerIndex = 0;
                PrevPayerSearchKey = payerName;
            }

            if (string.IsNullOrEmpty(txtPayerName.Text) || string.IsNullOrWhiteSpace(txtPayerName.Text)) return;

            foreach (Row row in grdApprovalResult.Rows)
            {
                row.Cells[CellName(nameof(MatchingHeader.PayerName))].Selected = false;

                if (row.Cells[CellName(nameof(MatchingHeader.PayerName))].Value != null)
                {
                    var rowName = row.Cells[CellName(nameof(MatchingHeader.PayerName))].Value.ToString().ToLower();

                    if (rowName.Contains(payerName))
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

                if (grdApprovalResult.RowCount > 0)
                {
                    grdApprovalResult.CurrentCell.Selected = true;
                }

                ShowWarningDialog(MsgWngNotExistSearchData);
            }
            else
            {
                foreach (Row row in grdApprovalResult.Rows)
                {
                    if (row.Cells[CellName(nameof(MatchingHeader.PayerName))].Value != null)
                    {
                        var rowName = row.Cells[CellName(nameof(MatchingHeader.PayerName))].Value.ToString().ToLower();

                        if (rowName.Contains(payerName))
                        {
                            if (PrevPayerIndex == -1)
                            {
                                PrevPayerIndex = row.Index;
                                grdApprovalResult.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingHeader.PayerName)));
                                row.Cells[CellName(nameof(MatchingHeader.PayerName))].Selected = true;
                                break;
                            }
                            else if (row.Index > PrevPayerIndex)
                            {
                                PrevPayerIndex = row.Index;
                                if (PrevPayerIndex == LastPayerIndex)
                                {
                                    PrevPayerIndex = -1;
                                }
                                grdApprovalResult.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingHeader.PayerName)));
                                row.Cells[CellName(nameof(MatchingHeader.PayerName))].Selected = true;
                                break;
                            }
                            else
                            {
                                if (PayerCount == 1)
                                {
                                    PrevPayerIndex = row.Index;
                                    grdApprovalResult.CurrentCellPosition = new CellPosition(row.Index, CellName(nameof(MatchingHeader.PayerName)));
                                    row.Cells[CellName(nameof(MatchingHeader.PayerName))].Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void cbxApprovalData_CheckedChanged(object sender, EventArgs e)
        {
            InitialApprovalGridSetting();

            var caption = (cbxApprovalData.Checked) ? "承認解除" : "承認";
            BaseContext.SetFunction03Caption(caption);
        }

        private void grdApprovalResult_CellClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.CellName == CellName("Individual"))
            {
                var form = ApplicationContext.Create(nameof(PE0102));
                var screen = form.GetAll<PE0102>().FirstOrDefault();
                screen.CollationSearch = GetSearchCondition();

                var header = (MatchingHeader)grdApprovalResult.Rows[e.RowIndex].DataBoundItem;
                screen.MatchingHeader = header;
                screen.CollationGrid = grdApprovalResult;
                screen.SelectRowIndex = e.RowIndex;
                screen.IsMatched = true;

                form.StartPosition = FormStartPosition.CenterParent;
                ApplicationContext.ShowDialog(ParentForm, form);
            }
        }

        #endregion

        #region call web services

        private async Task LoadSectionAsync()
            => Sections = await Util.GetSectionByCodesAsync(Login, codes: null);

        private async Task LoadSectionWithLoginAsync()
            => SectionsWithLoginUser = await Util.GetSectionByLoginUserIdAsync(Login);

        private async Task LoadDepartmentAsync()
            => Departments = await Util.GetDepartmentByCodesAsync(Login, codes: null);

        private async Task LoadDepartmentsByLoginUserIdAsync()
            => DepartmentsWithLoginUser = await Util.GetDepartmentByLoginUserIdAsync(Login);

        #endregion
    }
}
