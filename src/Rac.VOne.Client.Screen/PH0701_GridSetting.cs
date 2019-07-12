using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GridSettingMasterService;
using Rac.VOne.Common;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>グリッド表示設定</summary>
    public partial class PH0701 : VOneScreenBase
    {
        #region initialize
        private int SelectedGridId { get; set; }
        private DataTable dt { get; set; }
        private int DragDropDestRowIndex { get; set; }
        private int PreviousSelectedGridId { get; set; }
        private string CellName(string value) => $"cel{value}";

        private void RemoveColumns(List<GridSetting> gridSettingList, List<string> removeColumns) =>
            gridSettingList.RemoveAll(x => removeColumns.Any(y => y == x.ColumnNameJp));

        public PH0701()
        {
            InitializeComponent();
            grdPreview.SetupShortcutKeys();
            grdGridColumnSetting.SetupShortcutKeys();
            Text = "グリッド表示設定";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(false);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("初期値表示");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = DisplayDefaultGridSetting;

            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exist;
        }

        private void PH0701_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();

                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());
                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());

                loadTask.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SetComboForGridType();
                InitializeGridTemplateForColumnSetting();
                ClearData();

                DragDropDestRowIndex = -1;
                SelectedGridId = 0;
                PreviousSelectedGridId = 0;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SetComboForGridType()
        {
            cmbGridType.Items.Add(new ListItem("請求データ検索", GridId.BillingSearch));
            cmbGridType.Items.Add(new ListItem("入金データ検索", GridId.ReceiptSearch));
            cmbGridType.Items.Add(new ListItem("個別消込【請求】", GridId.BillingMatchingIndividual));
            cmbGridType.Items.Add(new ListItem("個別消込【入金】", GridId.ReceiptMatchingIndividual));
            cmbGridType.Items.Add(new ListItem("請求書発行", GridId.BillingInvoicePublish));
            if (UseScheduledPayment)
                cmbGridType.Items.Add(new ListItem("入金予定入力", GridId.PaymentScheduleInput));
        }
        #endregion

        #region Function Key
        [OperationLog("登録")]
        private void Save()
        {
            grdGridColumnSetting.CommitEdit();

            if (!grdGridColumnSetting.Rows.Any(x => Convert.ToInt32(x.Cells[CellName("DisplayWidth")].Value) > 0))
            {
                ShowWarningDialog(MsgWngAtleastOneColumnWidthNeedGreaterThanZero);
                grdGridColumnSetting.Focus();
                return;
            }

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                SaveGridSetting();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            ClearStatusMessage();
            ClearData();
        }

        private void ClearData()
        {
            Modified = false;
            lblExplain.Text = " ";
            cmbGridType.SelectedItem = null;
            cmbGridType.Select();
            BaseContext.SetFunction01Enabled(false);
            BaseContext.SetFunction03Enabled(false);
            rdoLoginUserOnly.Checked = true;
            ClearPreviewGridHeader();
            grdGridColumnSetting.TabIndex = 0;
            grdGridColumnSetting.TabStop = false;
            grdGridColumnSetting.Rows.Clear();
            grdPreview.TabIndex = 0;
            grdPreview.TabStop = false;
            grdPreview.AllowAutoExtend = true;
            grdPreview.DataSource = null;
        }

        private void ClearPreviewGridHeader()
        {
            var template = new Template();
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 40, "Header", cell: builder.GetRowHeaderCell()),
            });

            builder.BuildHeaderOnly(template);
            grdPreview.Template = template;
        }

        [OperationLog("初期値表示")]
        private void DisplayDefaultGridSetting()
        {
            if (!ShowConfirmDialog(MsgQstConfirmResetDefaultGridSetting))
                return;

            ClearStatusMessage();

            try
            {
                ClearPreviewGridHeader();

                var defaultGridSetting = GetDefaultGridSetting();
                RemoveNoNeedColumn(defaultGridSetting);
                SetGridColumnSetting(defaultGridSetting);
                BindGridSettingForPreview();
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("終了")]
        private void Exist()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            BaseForm.Close();
        }
        #endregion

        #region コンボボックス　SelectedIndexChanged イベント
        private void cmbGridType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGridType.SelectedIndex == -1) return;

            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
            {
                cmbGridType.SelectedIndexChanged -= (cmbGridType_SelectedIndexChanged);
                cmbGridType.SelectedIndex = PreviousSelectedGridId;
                cmbGridType.SelectedIndexChanged += (cmbGridType_SelectedIndexChanged);
                return;
            }

            ClearStatusMessage();
            PreviousSelectedGridId = cmbGridType.SelectedIndex;
            SelectedGridId = (int)cmbGridType.SelectedItem.SubItems[1].Value;

            ShowLabelMessage();

            BindGridColumnSettingAndGridPreview();

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction03Enabled(true);
            Modified = false;
            rdoLoginUserOnly.Checked = true;
        }

        private void ShowLabelMessage()
        {
            if (SelectedGridId == 0) return;

            var commonPart = "右グリッド上の項目名をドラッグ＆ドロップすることで、表示順を入れ替えます。\r\n";

            if (SelectedGridId == GridId.BillingSearch)
            {
                lblExplain.Text = commonPart + "\r\n・未消込請求データ削除画面のグリッド表示にも適用されます。";
            }
            else if (SelectedGridId == GridId.ReceiptSearch)
            {
                lblExplain.Text = commonPart + "\r\n・未消込入金データ削除画面のグリッド表示にも適用されます。\r\n" +
                                        "・対象外、対象外区分：表示位置、表示幅 固定（入金データ検索のみ）";

            }
            else if (SelectedGridId == GridId.BillingMatchingIndividual)
            {
                lblExplain.Text = commonPart + "\r\n・消、得意先コード、得意先名：表示位置、表示幅　固定" +
                                        "\r\n・消込対象額：消込候補画面のみ表示" +
                                        "\r\n・消込額：消込済画面のみ表示";
            }
            else if (SelectedGridId == GridId.ReceiptMatchingIndividual)
            {
                lblExplain.Text = commonPart + "\r\n・消、振込依頼人名：表示位置、表示幅　固定" +
                                        "\r\n・対象外区分：消込候補画面のみ表示" +
                                        "\r\n・消込額：消込済画面のみ表示";
            }
            else if (SelectedGridId == GridId.PaymentScheduleInput)
            {
                lblExplain.Text = commonPart + "\r\n・更新：表示位置、表示幅　固定";
            }
        }

        private void BindGridColumnSettingAndGridPreview()
        {
            List<GridSetting> gridSettingList = GetGridSetting();
            RemoveNoNeedColumn(gridSettingList);
            SetGridColumnSetting(gridSettingList);//右側
            BindGridSettingForPreview();
            grdGridColumnSetting.TabStop = true;
            grdPreview.TabStop = true;
        }

        private void RemoveNoNeedColumn(List<GridSetting> gridSettingList)
        {
            switch(SelectedGridId)
            {
                case 1:
                    if (!UseForeignCurrency)
                        RemoveColumns(gridSettingList, new List<string>() { "通貨コード" });

                    if (!UseLongTermAdvanceReceived)
                        RemoveColumns(gridSettingList, new List<string>() { "契約番号", "開始" });

                    if (!UseDiscount)
                        RemoveColumns(gridSettingList, new List<string>() { "歩引額1", "歩引額2", "歩引額3", "歩引額4", "歩引額5", "歩引額合計" });

                    if (!UseAccountTransfer)
                        RemoveColumns(gridSettingList, new List<string>() { "依頼作成日", "振替結果" });

                    break;
                case 2:
                    RemoveColumns(gridSettingList, new List<string>() { "対象外", "対象外区分" });

                    if (!UseForeignCurrency)
                        RemoveColumns(gridSettingList, new List<string>() { "通貨コード" });

                    if (!UseSection)
                        RemoveColumns(gridSettingList, new List<string>() { "入金部門コード", "入金部門名" });
                    break;
                case 3:
                    RemoveColumns(gridSettingList, new List<string>() { "消", "得意先コード", "得意先名" });

                    if (!UseScheduledPayment)
                        RemoveColumns(gridSettingList, new List<string>() { "入金予定キー" });
                    break;
                case 4:
                    RemoveColumns(gridSettingList, new List<string>() { "消", "振込依頼人名" });

                    if (!UseSection)
                        RemoveColumns(gridSettingList, new List<string>() { "入金部門コード", "入金部門名" });
                    break;
                case 5:
                    RemoveColumns(gridSettingList, new List<string>() { "更新" });

                    if (!UseForeignCurrency)
                        RemoveColumns(gridSettingList, new List<string>() { "通貨コード" });

                    if (!UseDiscount)
                        RemoveColumns(gridSettingList, new List<string>() { "歩引額合計" });
                    break;
                case 6:
                    RemoveColumns(gridSettingList, new List<string>() { "選択" });
                    break;
            }
        }

        #endregion

        #region　グリッド表示
        private void InitializeGridTemplateForColumnSetting()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 305, "ItemName"    , dataField: nameof(GridSetting.ColumnNameJp), caption: "項目名"),
                new CellSetting(height,  50, "DisplayWidth", dataField: nameof(GridSetting.DisplayWidth), caption: "表示幅", readOnly: false, cell: builder.GetNumberCellFreeInput(0, 1000, 4, false)),
                new CellSetting(height,   0, "ColumnName"  , dataField: nameof(GridSetting.ColumnName))
            });
            grdGridColumnSetting.Template = builder.Build();
            grdGridColumnSetting.HideSelection = true;
        }

        /// <summary>
        /// コラム表示順、表示幅調整グリッド
        /// </summary>
        /// <param name="gridData"></param>
        private void SetGridColumnSetting(List<GridSetting> gridSettingList)
        {
            grdGridColumnSetting.RowCount = gridSettingList.Count;

            for (var i = 0; i < gridSettingList.Count; i++)
            {
                grdGridColumnSetting.Rows[i][CellName("ItemName")].Value = gridSettingList[i].ColumnNameJp;
                grdGridColumnSetting.Rows[i][CellName("DisplayWidth")].Value = gridSettingList[i].DisplayWidth;
                grdGridColumnSetting.Rows[i][CellName("ColumnName")].Value = gridSettingList[i].ColumnName;
            }
        }

        private void SetPreviewGridHeader(List<GridSetting> gridSetting, GcMultiRowTemplateBuilder builder, int height)
        {
            var alignment = MultiRowContentAlignment.MiddleLeft;
            var middleLeft = MultiRowContentAlignment.MiddleLeft;
            var middleRight = MultiRowContentAlignment.MiddleRight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;

            if (SelectedGridId == GridId.ReceiptSearch)
                RemoveColumns(gridSetting, new List<string>() { "対象外", "対象外区分" });
            else if(SelectedGridId == GridId.BillingMatchingIndividual)
                RemoveColumns(gridSetting, new List<string>() { "消", "得意先コード", "得意先名" });
            else if(SelectedGridId == GridId.ReceiptMatchingIndividual)
                RemoveColumns(gridSetting, new List<string>() { "消", "振込依頼人名" });
            else if (SelectedGridId == GridId.PaymentScheduleInput)
                RemoveColumns(gridSetting, new List<string>() { "更新" });
            else if (SelectedGridId == GridId.BillingInvoicePublish)
                RemoveColumns(gridSetting, new List<string>() { "選択" });

            foreach (var item in gridSetting)
            {
                switch (item.ColumnNameJp)
                {
                    case "請求ID":               alignment = middleRight; break; 
                    case "消込区分":             alignment = middleLeft; break;
                    case "請求日":               alignment = middleCenter; break;
                    case "得意先コード":         alignment = middleCenter; break;
                    case "売上日":               alignment = middleCenter; break;
                    case "請求締日":             alignment = middleCenter; break;
                    case "入金予定日":           alignment = middleCenter; break;
                    case "通貨コード":           alignment = middleCenter; break;
                    case "請求部門コード":       alignment = middleCenter; break;
                    case "請求金額(税込)":       alignment = middleRight; break; 
                    case "請求残":               alignment = middleRight; break;
                    case "請求書番号":           alignment = middleLeft; break;
                    case "部門コード":           alignment = middleCenter; break;
                    case "担当者コード":         alignment = middleCenter; break;
                    case "契約番号":             alignment = middleCenter; break;
                    case "開始":                 alignment = middleCenter; break;
                    case "依頼作成日":           alignment = middleCenter; break;
                    case "振替結果":             alignment = middleCenter; break;
                    case "歩引額1":              alignment = middleRight; break;
                    case "歩引額2":              alignment = middleRight; break;
                    case "歩引額3":              alignment = middleRight; break;
                    case "歩引額4":              alignment = middleRight; break;
                    case "歩引額5":              alignment = middleRight; break;
                    case "歩引額合計":           alignment = middleRight; break;
                    case "初回入金日":           alignment = middleCenter; break;
                    case "最終入金日":           alignment = middleCenter; break;
                    case "請求金額（税抜）":     alignment = middleRight; break;
                    case "消費税":               alignment = middleRight; break;
                    case "入金ID":               alignment = middleRight; break;
                    case "入金日":               alignment = middleCenter; break;
                    case "入金額":               alignment = middleRight; break;
                    case "入金残":               alignment = middleRight; break;
                    case "対象外金額":           alignment = middleRight; break;
                    case "入金期日":             alignment = middleCenter; break;
                    case "入金部門コード":       alignment = middleCenter; break;
                    case "銀行コード":           alignment = middleCenter; break;
                    case "支店コード":           alignment = middleCenter; break;
                    case "口座番号":             alignment = middleCenter; break;
                    case "仮想支店コード":       alignment = middleCenter; break;
                    case "仮想口座番号":         alignment = middleCenter; break;
                    case "入金仕訳日":           alignment = middleCenter; break;
                    case "手形番号":             alignment = middleCenter; break;
                    case "券面銀行コード":       alignment = middleCenter; break;
                    case "券面支店コード":       alignment = middleCenter; break;
                    case "振出日":               alignment = middleCenter; break;
                    case "予定日":               alignment = middleCenter; break;
                    case "請求額":               alignment = middleRight; break;
                    case "歩引額":               alignment = middleRight; break;
                    case "消込対象額":           alignment = middleRight; break;
                    case "消込額":               alignment = middleRight; break;
                    case "請求番号":             alignment = middleLeft; break;
                    case "相殺":                 alignment = middleCenter; break;
                    case "口座":                 alignment = middleCenter; break;
                    case "期日":                 alignment = middleCenter; break;
                    case "振込依頼人名（全て）": alignment = middleLeft; break;
                    case "入金予定額":           alignment = middleRight; break;
                    case "違算":                 alignment = middleRight; break;
                    case "明細件数":             alignment = middleRight; break;
                    case "送付先コード":         alignment = middleCenter; break;
                    case "発行日":               alignment = middleCenter; break;
                    case "初回発行日":           alignment = middleCenter; break;
                    default: alignment = MultiRowContentAlignment.MiddleLeft; break;
                }

                if (SelectedGridId == GridId.BillingInvoicePublish
                    && item.ColumnName == nameof(BillingInvoice.DestnationContent))
                {
                    //送付先コード
                    builder.Items.AddRange(new CellSetting[]
                        {
                            new CellSetting(height,
                            30,
                            nameof(BillingInvoice.DestnationCode),
                            caption: "",
                            cell: builder.GetTextBoxCell(middleCenter),
                            dataField: nameof(BillingInvoice.DestnationCode))
                        });
                    dt.Columns.Add(nameof(BillingInvoice.DestnationCode));

                    //送付先検索ボタン
                    builder.Items.AddRange(new CellSetting[]
                        {
                            new CellSetting(height,
                            30,
                            nameof(BillingInvoice.DestnationButton),
                            caption: "",
                            cell: builder.GetButtonCell(),
                            dataField: nameof(BillingInvoice.DestnationButton))
                        });
                    dt.Columns.Add(nameof(BillingInvoice.DestnationButton));
                }

                builder.Items.AddRange(new CellSetting[]
                {
                    new CellSetting(height, item.DisplayWidth, item.ColumnName, caption: item.ColumnNameJp, cell: builder.GetTextBoxCell(alignment), dataField: item.ColumnName)
                });
                dt.Columns.Add(item.ColumnName);
            }
        }

        private void InitializeGridTemplateForPreview(List<GridSetting> gridSetting)
        {
            grdPreview.AllowAutoExtend = false;
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);

            var height = builder.DefaultRowHeight;
            ClearPreviewGridHeader();
            dt = new DataTable();
            int freezeLeftCellIndex = -1;

            if (SelectedGridId == GridId.BillingSearch)
            {
                SetPreviewGridHeader(gridSetting, builder, height);
                dt.Rows.Add(new object[] { });
                dt.Rows.Add(new object[] { });
            }
            else if (SelectedGridId == GridId.ReceiptSearch)
            {
                builder.Items.AddRange(new CellSetting[]
                {
                    new CellSetting(height, 50, "ExcludeFlagConst"      , caption: "対象外"    , cell: builder.GetCheckBoxCell(), dataField: "ExcludeFlagConst"),
                    new CellSetting(height, 90, "ExcludeCategoryIdConst", caption: "対象外区分", cell: builder.GetTextBoxCell() , dataField: "ExcludeCategoryIdConst")
                });

                dt.Columns.Add(new DataColumn("ExcludeFlagConst", typeof(bool)));
                dt.Columns.Add(new DataColumn("ExcludeCategoryIdConst", typeof(string)));
                SetPreviewGridHeader(gridSetting, builder, height);
                dt.Rows.Add(new object[] { false, " " });
                dt.Rows.Add(new object[] { true, "01:対象外" });
                freezeLeftCellIndex = 1;
            }
            else if (SelectedGridId == GridId.BillingMatchingIndividual)
            {
                builder.Items.AddRange(new CellSetting[]
                {
                    new CellSetting(height,  40, "Cancel"           , caption: "消"          , cell: builder.GetCheckBoxCell(), dataField: "Cancel"),
                    new CellSetting(height,  90, "CustomerCodeConst", caption: "得意先コード", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter), dataField: "CustomerCodeConst"),
                    new CellSetting(height, 100, "CustomerNameConst", caption: "得意先名"    , cell: builder.GetTextBoxCell(), dataField: "CustomerNameConst")
                });

                dt.Columns.Add(new DataColumn("Cancel", typeof(bool)));
                dt.Columns.Add(new DataColumn("CustomerCodeConst", typeof(string)));
                dt.Columns.Add(new DataColumn("CustomerNameConst", typeof(string)));
                SetPreviewGridHeader(gridSetting, builder, height);
                dt.Rows.Add(new object[] { true, "0000000001", "△△△株式会社" });
                dt.Rows.Add(new object[] { false, "0000000001", "△△△株式会社" });
                freezeLeftCellIndex = 2;
            }
            else if (SelectedGridId == GridId.ReceiptMatchingIndividual)
            {
                builder.Items.AddRange(new CellSetting[]
                {
                    new CellSetting(height,  50, "Cancel", caption: "消"          , cell: builder.GetCheckBoxCell(), dataField: "Cancel"),
                    new CellSetting(height, 100, "Name"  , caption: "振込依頼人名", cell: builder.GetTextBoxCell() , dataField: "Name")
                });

                dt.Columns.Add(new DataColumn("Cancel", typeof(bool)));
                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                SetPreviewGridHeader(gridSetting, builder, height);
                dt.Rows.Add(new object[] { true, "ﾃｽﾄｶｲｼﾔ" });
                dt.Rows.Add(new object[] { false, "ﾃｽﾄｶｲｼﾔ" });
                freezeLeftCellIndex = 1;
            }
            else if (SelectedGridId == GridId.PaymentScheduleInput)
            {
                builder.Items.AddRange(new CellSetting[]
                {
                    new CellSetting(height,  40, "Update", caption: "更新"          , cell: builder.GetCheckBoxCell(), dataField: "Update"),
                });

                dt.Columns.Add(new DataColumn("Update", typeof(bool)));
                SetPreviewGridHeader(gridSetting, builder, height);
                dt.Rows.Add(new object[] { false });
                dt.Rows.Add(new object[] { true });
                freezeLeftCellIndex = 0;
            }
            else if (SelectedGridId == GridId.BillingInvoicePublish)
            {
                builder.Items.AddRange(new CellSetting[]
               {
                    new CellSetting(height,
                    50,
                    nameof(BillingInvoice.Checked),
                    caption: "選択",
                    cell: builder.GetCheckBoxCell(),
                    dataField: nameof(BillingInvoice.Checked)),
               });

                dt.Columns.Add(new DataColumn(nameof(BillingInvoice.Checked), typeof(bool)));
                SetPreviewGridHeader(gridSetting, builder, height);
                dt.Rows.Add(new object[] { });
                dt.Rows.Add(new object[] { });
            }
            grdPreview.Template = builder.Build();
            grdPreview.DataSource = dt;
            grdPreview.HideSelection = true;
            grdPreview.FreezeLeftCellIndex = freezeLeftCellIndex;
        }
        #endregion

        #region ドラッグ＆ドロップ ＆グッリド幅変更
        private void grdSetting_CellValueChanged(object sender, CellEventArgs e)
        {
            Modified = true;
        }
        private void grdSetting_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            if (grdGridColumnSetting.CurrentCellPosition.CellIndex != 1)
                return;

            ClearStatusMessage();
            BindGridSettingForPreview();
        }

        private void BindGridSettingForPreview()
        {
            var gridSettingsPreview = GetPreviewGridSetting();

            if (!gridSettingsPreview.Any(x => x.DisplayWidth > 0))
            {
                ClearPreviewGridHeader();
                grdPreview.AllowAutoExtend = true;
                grdPreview.DataSource = null;
                return;
            }

            InitializeGridTemplateForPreview(gridSettingsPreview);
            AddConstantDataAtPreviewGrid();
        }

        /// <summary>
        /// 設定プレビューのヘッダー情報取得
        /// </summary>
        /// <returns></returns>
        private List<GridSetting> GetPreviewGridSetting()
        {
            var gridSettingList = new List<GridSetting>();
            int loginUserId = Login.UserId;

            for (int i = 0; i < grdGridColumnSetting.RowCount; i++)
            {
                var gridSetting = new GridSetting();
                int result = 0;
                string displayWidth = grdGridColumnSetting.Rows[i].Cells[CellName("DisplayWidth")].DisplayText;

                gridSetting.GridId = SelectedGridId;
                gridSetting.LoginUserId = loginUserId;
                gridSetting.ColumnNameJp = grdGridColumnSetting.Rows[i].Cells[CellName("ItemName")].DisplayText;

                if (int.TryParse(displayWidth.Replace(",", ""), out result))
                {
                    gridSetting.DisplayWidth = result;
                }
                gridSetting.ColumnName = grdGridColumnSetting.Rows[i].Cells[CellName("ColumnName")].DisplayText;

                if (SelectedGridId ==  GridId.ReceiptSearch) gridSetting.DisplayOrder = i + 3;
                else if (SelectedGridId == GridId.BillingMatchingIndividual) gridSetting.DisplayOrder = i + 4;
                else if (SelectedGridId == GridId.ReceiptMatchingIndividual) gridSetting.DisplayOrder = i + 3;
                else if (SelectedGridId == GridId.PaymentScheduleInput) gridSetting.DisplayOrder = i + 2;
                else gridSetting.DisplayOrder = i + 1;

                gridSetting.CompanyId = CompanyId;
                gridSetting.CreateBy = loginUserId;
                gridSetting.UpdateBy = loginUserId;
                gridSettingList.Add(gridSetting);
            }
            
            if (SelectedGridId == GridId.ReceiptSearch)
            {
                gridSettingList.Add(new GridSetting() { CompanyId = CompanyId, LoginUserId = loginUserId, GridId = 2, ColumnNameJp = "対象外", ColumnName = "ExcludeFlag", DisplayOrder = 1, DisplayWidth = 65, CreateBy = loginUserId, UpdateBy = loginUserId });
                gridSettingList.Add(new GridSetting() { CompanyId = CompanyId, LoginUserId = loginUserId, GridId = 2, ColumnNameJp = "対象外区分", ColumnName = "ExcludeCategory", DisplayOrder = 2, DisplayWidth = 150, CreateBy = loginUserId, UpdateBy = loginUserId });
            }
            else if (SelectedGridId == GridId.BillingMatchingIndividual)
            {
                gridSettingList.Add(new GridSetting() { CompanyId = CompanyId, LoginUserId = loginUserId, GridId = 3, ColumnNameJp = "消", ColumnName = "AssignmentFlag", DisplayOrder = 1, DisplayWidth = 25, CreateBy = loginUserId, UpdateBy = loginUserId });
                gridSettingList.Add(new GridSetting() { CompanyId = CompanyId, LoginUserId = loginUserId, GridId = 3, ColumnNameJp = "得意先コード", ColumnName = "CustomerCode", DisplayOrder = 2, DisplayWidth = 74, CreateBy = loginUserId, UpdateBy = loginUserId });
                gridSettingList.Add(new GridSetting() { CompanyId = CompanyId, LoginUserId = loginUserId, GridId = 3, ColumnNameJp = "得意先名", ColumnName = "CustomerName", DisplayOrder = 3, DisplayWidth = 110, CreateBy = loginUserId, UpdateBy = loginUserId });
            }
            else if (SelectedGridId == GridId.ReceiptMatchingIndividual)
            {
                gridSettingList.Add(new GridSetting() { CompanyId = CompanyId, LoginUserId = loginUserId, GridId = 4, ColumnNameJp = "消", ColumnName = "AssignmentFlag", DisplayOrder = 1, DisplayWidth = 25, CreateBy = loginUserId, UpdateBy = loginUserId });
                gridSettingList.Add(new GridSetting() { CompanyId = CompanyId, LoginUserId = loginUserId, GridId = 4, ColumnNameJp = "振込依頼人名", ColumnName = "PayerName", DisplayOrder = 2, DisplayWidth = 118, CreateBy = loginUserId, UpdateBy = loginUserId });
            }
            else if (SelectedGridId == GridId.PaymentScheduleInput)
            {
                gridSettingList.Add(new GridSetting() { CompanyId = CompanyId, LoginUserId = loginUserId, GridId = 5, ColumnNameJp = "更新", ColumnName = "UpdateFlag", DisplayOrder = 1, DisplayWidth = 40, CreateBy = loginUserId, UpdateBy = loginUserId });
            }

            return gridSettingList;
        }

        private void AddConstantDataAtPreviewGrid()
        {
            if (SelectedGridId == GridId.BillingSearch)
            {
                grdPreview.Rows[0][CellName("Id")].Value = 1;
                grdPreview.Rows[1][CellName("Id")].Value = 2;
                grdPreview.Rows[0][CellName("AssignmentState")].Value = "未消込";
                grdPreview.Rows[1][CellName("AssignmentState")].Value = "一部消込";
                grdPreview.Rows[0][CellName("CustomerCode")].Value = "0000000001";
                grdPreview.Rows[1][CellName("CustomerCode")].Value = "0000000002";
                grdPreview.Rows[0][CellName("CustomerName")].Value = "△△△株式会社";
                grdPreview.Rows[1][CellName("CustomerName")].Value = "□□株式会社";
                grdPreview.Rows[0][CellName("BilledAt")].Value = "2016/02/04";
                grdPreview.Rows[1][CellName("BilledAt")].Value = "2016/03/05";
                grdPreview.Rows[0][CellName("SalesAt")].Value = "2016/02/04";
                grdPreview.Rows[1][CellName("SalesAt")].Value = "2016/03/05";
                grdPreview.Rows[0][CellName("ClosingAt")].Value = "2016/02/29";
                grdPreview.Rows[1][CellName("ClosingAt")].Value = "2016/03/31";
                grdPreview.Rows[0][CellName("DueAt")].Value = "2016/03/31";
                grdPreview.Rows[1][CellName("DueAt")].Value = "2016/04/30";

                if (UseForeignCurrency)
                {
                    grdPreview.Rows[0][CellName("CurrencyCode")].Value = "JPY";
                    grdPreview.Rows[1][CellName("CurrencyCode")].Value = "JPY";
                }
                grdPreview.Rows[0][CellName("BillingAmount")].Value = "5,000";
                grdPreview.Rows[1][CellName("BillingAmount")].Value = "12,000";
                grdPreview.Rows[0][CellName("RemainAmount")].Value = "5,000";
                grdPreview.Rows[1][CellName("RemainAmount")].Value = "3,000";
                grdPreview.Rows[0][CellName("InvoiceCode")].Value = "TEST00001";
                grdPreview.Rows[1][CellName("InvoiceCode")].Value = "TEST00002";
                grdPreview.Rows[0][CellName("BillingCategory")].Value = "01:売上";
                grdPreview.Rows[1][CellName("BillingCategory")].Value = "01:売上";
                grdPreview.Rows[0][CellName("CollectCategory")].Value = "01:振込";
                grdPreview.Rows[1][CellName("CollectCategory")].Value = "01:振込";
                grdPreview.Rows[0][CellName("InputType")].Value = "1:取込";
                grdPreview.Rows[1][CellName("InputType")].Value = "2:入力";
                grdPreview.Rows[0][CellName("Memo")].Value = "請求メモ1";
                grdPreview.Rows[1][CellName("Memo")].Value = "請求メモ1";
                grdPreview.Rows[0][CellName("DepartmentCode")].Value = "1";
                grdPreview.Rows[1][CellName("DepartmentCode")].Value = "2";
                grdPreview.Rows[0][CellName("DepartmentName")].Value = "東京営業部";
                grdPreview.Rows[1][CellName("DepartmentName")].Value = "関東営業第二";
                grdPreview.Rows[0][CellName("StaffCode")].Value = "1";
                grdPreview.Rows[1][CellName("StaffCode")].Value = "2";
                grdPreview.Rows[0][CellName("StaffName")].Value = "営業太郎";
                grdPreview.Rows[1][CellName("StaffName")].Value = "営業次郎";

                if (UseLongTermAdvanceReceived)
                {
                    grdPreview.Rows[0][CellName("ContractNumber")].Value = 111;
                    grdPreview.Rows[1][CellName("ContractNumber")].Value = 112;
                    grdPreview.Rows[0][CellName("Confirm")].Value = "未";
                    grdPreview.Rows[1][CellName("Confirm")].Value = "済";
                }

                if (UseAccountTransfer)
                {
                    grdPreview.Rows[0][CellName("RequestDate")].Value = "2016/03/01";
                    grdPreview.Rows[1][CellName("RequestDate")].Value = " ";
                    grdPreview.Rows[0][CellName("ResultCode")].Value = "振替済";
                    grdPreview.Rows[1][CellName("ResultCode")].Value = " ";
                }
                foreach (var i in Enumerable.Range(1, 8))
                    foreach (var rowIndex in Enumerable.Range(0, 2))
                        grdPreview.Rows[rowIndex][CellName($"Note{i}")].Value = $"備考{i}";

                if(UseDiscount)
                {
                    grdPreview.Rows[0][CellName("DiscountAmount1")].Value = "10";
                    grdPreview.Rows[1][CellName("DiscountAmount1")].Value = "20";
                    grdPreview.Rows[0][CellName("DiscountAmount2")].Value = "5";
                    grdPreview.Rows[1][CellName("DiscountAmount2")].Value = "8";
                    grdPreview.Rows[0][CellName("DiscountAmount3")].Value = "0";
                    grdPreview.Rows[1][CellName("DiscountAmount3")].Value = "0";
                    grdPreview.Rows[0][CellName("DiscountAmount4")].Value = "10";
                    grdPreview.Rows[1][CellName("DiscountAmount4")].Value = "20";
                    grdPreview.Rows[0][CellName("DiscountAmount5")].Value = "0";
                    grdPreview.Rows[1][CellName("DiscountAmount5")].Value = "0";
                    grdPreview.Rows[0][CellName("DiscountAmountSummary")].Value = "25";
                    grdPreview.Rows[1][CellName("DiscountAmountSummary")].Value = "48";
                }
                grdPreview.Rows[0][CellName("FirstRecordedAt")].Value = "2016/03/31";
                grdPreview.Rows[1][CellName("FirstRecordedAt")].Value = "2016/04/30";
                grdPreview.Rows[0][CellName("LastRecordedAt")].Value = "2016/04/30";
                grdPreview.Rows[1][CellName("LastRecordedAt")].Value = "2016/05/31";
                grdPreview.Rows[0][CellName("Price")].Value = "4,630";
                grdPreview.Rows[1][CellName("Price")].Value = "11,111";
                grdPreview.Rows[0][CellName("TaxAmount")].Value = "370";
                grdPreview.Rows[1][CellName("TaxAmount")].Value = "889";
            }
            else if (SelectedGridId == GridId.ReceiptSearch)
            {
                grdPreview.Rows[0][CellName("Id")].Value = 1;
                grdPreview.Rows[1][CellName("Id")].Value = 2;
                grdPreview.Rows[0][CellName("AssignmentState")].Value = "未消込";
                grdPreview.Rows[1][CellName("AssignmentState")].Value = "一部消込";
                grdPreview.Rows[0][CellName("RecordedAt")].Value = "2015/12/28";
                grdPreview.Rows[1][CellName("RecordedAt")].Value = "2016/01/05";
                grdPreview.Rows[0][CellName("CustomerCode")].Value = "0000000001";
                grdPreview.Rows[1][CellName("CustomerCode")].Value = "0000000002";
                grdPreview.Rows[0][CellName("CustomerName")].Value = "△△△株式会社";
                grdPreview.Rows[1][CellName("CustomerName")].Value = "□□株式会社";
                grdPreview.Rows[0][CellName("PayerName")].Value = "ｶﾌﾞｼｷｶﾞｲｼﾔ1";
                grdPreview.Rows[1][CellName("PayerName")].Value = "ｶﾌﾞｼｷｶﾞｲｼﾔ2";

                if(UseForeignCurrency)
                {
                    grdPreview.Rows[0][CellName("CurrencyCode")].Value = "JPY";
                    grdPreview.Rows[1][CellName("CurrencyCode")].Value = "JPY";
                }
                grdPreview.Rows[0][CellName("ReceiptAmount")].Value = "15000";
                grdPreview.Rows[1][CellName("ReceiptAmount")].Value = "3500";
                grdPreview.Rows[0][CellName("RemainAmount")].Value = "15000";
                grdPreview.Rows[1][CellName("RemainAmount")].Value = "2500";
                grdPreview.Rows[0][CellName("ExcludeAmount")].Value = "0";
                grdPreview.Rows[1][CellName("ExcludeAmount")].Value = "1000";
                grdPreview.Rows[0][CellName("ReceiptCategoryName")].Value = "01:振込";
                grdPreview.Rows[1][CellName("ReceiptCategoryName")].Value = "01:振込";
                grdPreview.Rows[0][CellName("InputType")].Value = "1:EB取込";
                grdPreview.Rows[1][CellName("InputType")].Value = "2:入力";
                grdPreview.Rows[0][CellName("Note1")].Value = "備考1";
                grdPreview.Rows[1][CellName("Note1")].Value = "備考1";
                grdPreview.Rows[0][CellName("Memo")].Value = "入金メモ1";
                grdPreview.Rows[1][CellName("Memo")].Value = "入金メモ2";
                grdPreview.Rows[0][CellName("DueAt")].Value = " ";
                grdPreview.Rows[1][CellName("DueAt")].Value = "2016/02/10";

                if(UseSection)
                {
                    grdPreview.Rows[0][CellName("SectionCode")].Value = 1;
                    grdPreview.Rows[1][CellName("SectionCode")].Value = 2;
                    grdPreview.Rows[0][CellName("SectionName")].Value = "入金部門1";
                    grdPreview.Rows[1][CellName("SectionName")].Value = "入金部門2";
                }
                grdPreview.Rows[0][CellName("BankCode")].Value = "1001";
                grdPreview.Rows[1][CellName("BankCode")].Value = " "; 
                grdPreview.Rows[0][CellName("BankName")].Value = "ﾃｽﾄｷﾞﾝｺｳ";
                grdPreview.Rows[1][CellName("BankName")].Value = " ";
                grdPreview.Rows[0][CellName("BranchCode")].Value = "101";
                grdPreview.Rows[1][CellName("BranchCode")].Value = " ";
                grdPreview.Rows[0][CellName("BranchName")].Value = "ﾃｽﾄｼﾃﾝ";
                grdPreview.Rows[1][CellName("BranchName")].Value = " ";
                grdPreview.Rows[0][CellName("AccountNumber")].Value = "0123456";
                grdPreview.Rows[1][CellName("AccountNumber")].Value = " ";
                grdPreview.Rows[0][CellName("SourceBankName")].Value = "ﾃｽﾄｷﾞﾝｺｳ";
                grdPreview.Rows[1][CellName("SourceBankName")].Value = " ";
                grdPreview.Rows[0][CellName("SourceBranchName")].Value = "ﾃｽﾄｼﾃﾝ";
                grdPreview.Rows[1][CellName("SourceBranchName")].Value = " ";
                grdPreview.Rows[0][CellName("VirtualBranchCode")].Value = "123";
                grdPreview.Rows[1][CellName("VirtualBranchCode")].Value = " ";
                grdPreview.Rows[0][CellName("VirtualAccountNumber")].Value = "4567890";
                grdPreview.Rows[1][CellName("VirtualAccountNumber")].Value = " ";
                grdPreview.Rows[0][CellName("OutputAt")].Value = " ";
                grdPreview.Rows[1][CellName("OutputAt")].Value = "2016/02/01 13:50:08";
                grdPreview.Rows[0][CellName("Note2")].Value = " ";
                grdPreview.Rows[1][CellName("Note2")].Value = "備考2";
                grdPreview.Rows[0][CellName("Note3")].Value = " ";
                grdPreview.Rows[1][CellName("Note3")].Value = "備考3";
                grdPreview.Rows[0][CellName("Note4")].Value = " ";
                grdPreview.Rows[1][CellName("Note4")].Value = "備考4";
                grdPreview.Rows[0][CellName("BillNumber")].Value = " ";
                grdPreview.Rows[1][CellName("BillNumber")].Value = "12345";
                grdPreview.Rows[0][CellName("BillBankCode")].Value = " ";
                grdPreview.Rows[1][CellName("BillBankCode")].Value = "1234";
                grdPreview.Rows[0][CellName("BillBranchCode")].Value = " ";
                grdPreview.Rows[1][CellName("BillBranchCode")].Value = "123";
                grdPreview.Rows[0][CellName("BillDrawAt")].Value = " ";
                grdPreview.Rows[1][CellName("BillDrawAt")].Value = "2015/12/20";
                grdPreview.Rows[0][CellName("BillDrawer")].Value = " ";
                grdPreview.Rows[1][CellName("BillDrawer")].Value = "株式会社○○";
            }
            else if (SelectedGridId == GridId.BillingMatchingIndividual)
            {
                grdPreview.Rows[0][CellName("BilledAt")].Value = "2016/06/01";
                grdPreview.Rows[1][CellName("BilledAt")].Value = "2016/06/01";
                grdPreview.Rows[0][CellName("SalesAt")].Value = "2016/06/01";
                grdPreview.Rows[1][CellName("SalesAt")].Value = "2016/06/01";
                grdPreview.Rows[0][CellName("DueAt")].Value = "2016/06/17";
                grdPreview.Rows[1][CellName("DueAt")].Value = "2016/06/17";
                grdPreview.Rows[0][CellName("BillingAmount")].Value = "2500";
                grdPreview.Rows[1][CellName("BillingAmount")].Value = "1000";
                grdPreview.Rows[0][CellName("RemainAmount")].Value = "2500";
                grdPreview.Rows[1][CellName("RemainAmount")].Value = "1000";
                grdPreview.Rows[0][CellName("DiscountAmountSummary")].Value = "120";
                grdPreview.Rows[1][CellName("DiscountAmountSummary")].Value = "230";
                grdPreview.Rows[0][CellName("TargetAmount")].Value = "2500";
                grdPreview.Rows[1][CellName("TargetAmount")].Value = "1000";
                grdPreview.Rows[0][CellName("MatchingAmount")].Value = "2500";
                grdPreview.Rows[1][CellName("MatchingAmount")].Value = "1000";
                grdPreview.Rows[0][CellName("InvoiceCode")].Value = "TEST00001";
                grdPreview.Rows[1][CellName("InvoiceCode")].Value = "TEST00002";
                grdPreview.Rows[0][CellName("BillingCategory")].Value = "01:売上";
                grdPreview.Rows[1][CellName("BillingCategory")].Value = "01:売上";
                grdPreview.Rows[0][CellName("DepartmentName")].Value = "請求部門1";
                grdPreview.Rows[1][CellName("DepartmentName")].Value = "請求部門1";
                foreach (var i in Enumerable.Range(1, 8))
                    foreach (var rowIndex in Enumerable.Range(0, 2))
                        grdPreview.Rows[rowIndex][CellName($"Note{i}")].Value = $"備考{i}";
                grdPreview.Rows[0][CellName("Memo")].Value = "請求メモ";
                grdPreview.Rows[1][CellName("Memo")].Value = "請求メモ";
                grdPreview.Rows[0][CellName("InputType")].Value = "入力";
                grdPreview.Rows[1][CellName("InputType")].Value = "入力";

                if(UseScheduledPayment)
                {
                    grdPreview.Rows[0][CellName("ScheduledPaymentKey")].Value = "キー1";
                    grdPreview.Rows[1][CellName("ScheduledPaymentKey")].Value = "キー2";
                }
            }
            else if (SelectedGridId == GridId.ReceiptMatchingIndividual)
            {
                grdPreview.Rows[0][CellName("RecordedAt")].Value = "2016/06/01";
                grdPreview.Rows[1][CellName("RecordedAt")].Value = "2016/06/01";
                grdPreview.Rows[0][CellName("ReceiptCategoryName")].Value = "振込";
                grdPreview.Rows[1][CellName("ReceiptCategoryName")].Value = "小切手";
                grdPreview.Rows[0][CellName("ReceiptAmount")].Value = "1500";
                grdPreview.Rows[1][CellName("ReceiptAmount")].Value = "150";
                grdPreview.Rows[0][CellName("RemainAmount")].Value = "1500";
                grdPreview.Rows[1][CellName("RemainAmount")].Value = "150";
                grdPreview.Rows[0][CellName("TargetAmount")].Value = "1500";
                grdPreview.Rows[1][CellName("TargetAmount")].Value = "150";
                grdPreview.Rows[0][CellName("NettingState")].Value = "";
                grdPreview.Rows[1][CellName("NettingState")].Value = "*";
                grdPreview.Rows[0][CellName("SourceBank")].Value = "ﾃｽﾄﾊﾞﾝｸ / ﾃｽﾄｼﾃﾝ";
                grdPreview.Rows[1][CellName("SourceBank")].Value = "";
                grdPreview.Rows[0][CellName("BankCode")].Value = "1001";
                grdPreview.Rows[1][CellName("BankCode")].Value = "";
                grdPreview.Rows[0][CellName("BankName")].Value = "ﾃｽﾄｷﾞﾝｺｳ";
                grdPreview.Rows[1][CellName("BankName")].Value = "";
                grdPreview.Rows[0][CellName("BranchCode")].Value = "101";
                grdPreview.Rows[1][CellName("BranchCode")].Value = "";
                grdPreview.Rows[0][CellName("BranchName")].Value = "ﾃｽﾄｼﾃﾝ";
                grdPreview.Rows[1][CellName("BranchName")].Value = "";
                grdPreview.Rows[0][CellName("AccountTypeName")].Value = "普通";
                grdPreview.Rows[1][CellName("AccountTypeName")].Value = "";
                grdPreview.Rows[0][CellName("AccountNumber")].Value = "0123456";
                grdPreview.Rows[1][CellName("AccountNumber")].Value = "";

                if(UseSection)
                {
                    grdPreview.Rows[0][CellName("SectionCode")].Value = 1;
                    grdPreview.Rows[1][CellName("SectionCode")].Value = 2;
                    grdPreview.Rows[0][CellName("SectionName")].Value = "入金部門1";
                    grdPreview.Rows[1][CellName("SectionName")].Value = "入金部門2";
                }
                grdPreview.Rows[0][CellName("Note1")].Value = " ";
                grdPreview.Rows[1][CellName("Note1")].Value = "備考1";
                grdPreview.Rows[0][CellName("Memo")].Value = "入金メモ1 ";
                grdPreview.Rows[1][CellName("Memo")].Value = "入金メモ2";
                grdPreview.Rows[0][CellName("DueAt")].Value = " ";
                grdPreview.Rows[1][CellName("DueAt")].Value = "2016/06/15";
                grdPreview.Rows[0][CellName("ExcludeCategoryName")].Value = " ";
                grdPreview.Rows[1][CellName("ExcludeCategoryName")].Value = "01:対象外";
                grdPreview.Rows[0][CellName("VirtualBranchCode")].Value = "123";
                grdPreview.Rows[1][CellName("VirtualBranchCode")].Value = " ";
                grdPreview.Rows[0][CellName("VirtualAccountNumber")].Value = "4567890";
                grdPreview.Rows[1][CellName("VirtualAccountNumber")].Value = " ";
                grdPreview.Rows[0][CellName("CustomerCode")].Value = "0000000001";
                grdPreview.Rows[1][CellName("CustomerCode")].Value = "0000000001";
                grdPreview.Rows[0][CellName("CustomerName")].Value = "△△△株式会社";
                grdPreview.Rows[1][CellName("CustomerName")].Value = "△△△株式会社";
                grdPreview.Rows[0][CellName("Note2")].Value = " ";
                grdPreview.Rows[1][CellName("Note2")].Value = "備考2";
                grdPreview.Rows[0][CellName("Note3")].Value = " ";
                grdPreview.Rows[1][CellName("Note3")].Value = "備考3";
                grdPreview.Rows[0][CellName("Note4")].Value = " ";
                grdPreview.Rows[1][CellName("Note4")].Value = "備考4";
                grdPreview.Rows[0][CellName("BillNumber")].Value = " ";
                grdPreview.Rows[1][CellName("BillNumber")].Value = "12345";
                grdPreview.Rows[0][CellName("BillBankCode")].Value = " ";
                grdPreview.Rows[1][CellName("BillBankCode")].Value = "1234";
                grdPreview.Rows[0][CellName("BillBranchCode")].Value = " ";
                grdPreview.Rows[1][CellName("BillBranchCode")].Value = "123";
                grdPreview.Rows[0][CellName("BillDrawAt")].Value = " ";
                grdPreview.Rows[1][CellName("BillDrawAt")].Value = "2015/12/20";
                grdPreview.Rows[0][CellName("BillDrawer")].Value = " ";
                grdPreview.Rows[1][CellName("BillDrawer")].Value = "株式会社○○";
                grdPreview.Rows[0][CellName("PayerNameRaw")].Value = "ｶ)ﾃｽﾄｶｲｼﾔ";
                grdPreview.Rows[1][CellName("PayerNameRaw")].Value = "ｶ)ﾃｽﾄｶｲｼﾔ";
            }
            else if (SelectedGridId == GridId.PaymentScheduleInput)
            {
                grdPreview.Rows[0][CellName("BillingId")].Value = 1;
                grdPreview.Rows[1][CellName("BillingId")].Value = 2;
                grdPreview.Rows[0][CellName("InvoiceCode")].Value = "TEST00001";
                grdPreview.Rows[1][CellName("InvoiceCode")].Value = "TEST00002";
                grdPreview.Rows[0][CellName("CustomerCode")].Value = "0000000001";
                grdPreview.Rows[1][CellName("CustomerCode")].Value = "0000000002";
                grdPreview.Rows[0][CellName("CustomerName")].Value = "△△△株式会社";
                grdPreview.Rows[1][CellName("CustomerName")].Value = "□□株式会社";
                grdPreview.Rows[0][CellName("DepartmentCode")].Value = "1";
                grdPreview.Rows[1][CellName("DepartmentCode")].Value = "2";
                grdPreview.Rows[0][CellName("DepartmentName")].Value = "東京営業部";
                grdPreview.Rows[1][CellName("DepartmentName")].Value = "関東営業第二";
                if (UseForeignCurrency)
                {
                    grdPreview.Rows[0][CellName("CurrencyCode")].Value = "JPY";
                    grdPreview.Rows[1][CellName("CurrencyCode")].Value = "JPY";
                }
                grdPreview.Rows[0][CellName("BillingAmount")].Value = "5,000";
                grdPreview.Rows[1][CellName("BillingAmount")].Value = "12,000";
                if (UseDiscount)
                {
                    grdPreview.Rows[0][CellName("DiscountAmountSummary")].Value = "10";
                    grdPreview.Rows[1][CellName("DiscountAmountSummary")].Value = "20";
                }
                grdPreview.Rows[0][CellName("RemainAmount")].Value = "5,000";
                grdPreview.Rows[1][CellName("RemainAmount")].Value = "3,000";
                grdPreview.Rows[0][CellName("PaymentAmount")].Value = "5,000";
                grdPreview.Rows[1][CellName("PaymentAmount")].Value = "3,000";
                grdPreview.Rows[0][CellName("OffsetAmount")].Value = "5,000";
                grdPreview.Rows[1][CellName("OffsetAmount")].Value = "3,000";
                grdPreview.Rows[0][CellName("BilledAt")].Value = "2016/02/04";
                grdPreview.Rows[1][CellName("BilledAt")].Value = "2016/03/05";
                grdPreview.Rows[0][CellName("BillingDueAt")].Value = "2016/03/31";
                grdPreview.Rows[1][CellName("BillingDueAt")].Value = "2016/04/30";
                grdPreview.Rows[0][CellName("BillingCategory")].Value = "01:売上";
                grdPreview.Rows[1][CellName("BillingCategory")].Value = "01:売上";
                grdPreview.Rows[0][CellName("ScheduledPaymentKey")].Value = "キー1";
                grdPreview.Rows[1][CellName("ScheduledPaymentKey")].Value = "キー2";
                grdPreview.Rows[0][CellName("SalesAt")].Value = "2016/02/04";
                grdPreview.Rows[1][CellName("SalesAt")].Value = "2016/03/05";
                grdPreview.Rows[0][CellName("ClosingAt")].Value = "2016/02/29";
                grdPreview.Rows[1][CellName("ClosingAt")].Value = "2016/03/31";
                grdPreview.Rows[0][CellName("CollectCategory")].Value = "01:振込";
                grdPreview.Rows[1][CellName("CollectCategory")].Value = "01:振込";
                grdPreview.Rows[0][CellName("Note1")].Value = "備考1";
                grdPreview.Rows[1][CellName("Note1")].Value = "備考1";
                grdPreview.Rows[0][CellName("Note2")].Value = "備考2";
                grdPreview.Rows[1][CellName("Note2")].Value = "備考2";
                grdPreview.Rows[0][CellName("Note3")].Value = "備考3";
                grdPreview.Rows[1][CellName("Note3")].Value = "備考3";
                grdPreview.Rows[0][CellName("Note4")].Value = "備考4";
                grdPreview.Rows[1][CellName("Note4")].Value = "備考4";
                grdPreview.Rows[0][CellName("StaffCode")].Value = "1";
                grdPreview.Rows[1][CellName("StaffCode")].Value = "2";
                grdPreview.Rows[0][CellName("StaffName")].Value = "営業太郎";
                grdPreview.Rows[1][CellName("StaffName")].Value = "営業次郎";
                grdPreview.Rows[0][CellName("InputType")].Value = "1:取込";
                grdPreview.Rows[1][CellName("InputType")].Value = "2:入力";
                grdPreview.Rows[0][CellName("Memo")].Value = "請求メモ1";
                grdPreview.Rows[1][CellName("Memo")].Value = "請求メモ2";
            }
            else if (SelectedGridId == GridId.BillingInvoicePublish)
            {
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.Checked))].Value = true;
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.Checked))].Value = false;
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.InvoiceTemplateId))].Value = "銀行振込";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.InvoiceTemplateId))].Value = "銀行振込";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.InvoiceCode))].Value = "TEST00001";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.InvoiceCode))].Value = "TEST00002";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.DetailsCount))].Value = "5";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.DetailsCount))].Value = "3";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.CustomerCode))].Value = "0000000001";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.CustomerCode))].Value = "0000000002";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.CustomerName))].Value = "△△△株式会社";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.CustomerName))].Value = "□□株式会社";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.AmountSum))].Value = "5,000";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.AmountSum))].Value = "12,000";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.RemainAmountSum))].Value = "5,000";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.RemainAmountSum))].Value = "3,000";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.CollectCategoryCodeAndNeme))].Value = "01:振込";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.CollectCategoryCodeAndNeme))].Value = "01:振込";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.ClosingAt))].Value = "2016/02/29";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.ClosingAt))].Value = "2016/03/31";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.BilledAt))].Value = "2016/02/04";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.BilledAt))].Value = "2016/03/05";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.DepartmentCode))].Value = "1";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.DepartmentCode))].Value = "2";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.DepartmentName))].Value = "東京営業部";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.DepartmentName))].Value = "関東営業第二";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.StaffCode))].Value = "1";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.StaffCode))].Value = "2";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.StaffName))].Value = "営業太郎";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.StaffName))].Value = "営業太郎";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.DestnationCode))].Value = "01";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.DestnationCode))].Value = "02";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.DestnationButton))].Value = "…";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.DestnationButton))].Value = "…";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.DestnationContent))].Value = "〒001-0001 東京都千代田区X-X-XX 〇〇ビル △△△株式会社御中";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.DestnationContent))].Value = "〒002-0002 東京都港区X-X-XX 〇〇ビル □□株式会社御中";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.PublishAt))].Value = "2016/03/01";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.PublishAt))].Value = "2016/04/20";
                grdPreview.Rows[0][CellName(nameof(BillingInvoice.PublishAt1st))].Value = "2016/03/01";
                grdPreview.Rows[1][CellName(nameof(BillingInvoice.PublishAt1st))].Value = "2016/04/02";
            }
        }
        #endregion

        #region ドラッグ＆ドロップ
        private void grdSetting_DragDrop(object sender, DragEventArgs e)
        {
            var grdCommon = sender as GcMultiRow;
            var clientPoint = grdCommon.PointToClient(new System.Drawing.Point(e.X, e.Y));
            var hitTestInfo = grdCommon.HitTest(clientPoint.X, clientPoint.Y);
            var sourceRow = (Row)e.Data.GetData(typeof(Row));

            if (sourceRow.GcMultiRow.Name != grdCommon.Name)
            {
                DragDropDestRowIndex = -1; return;
            }
            var cellValues = new object[sourceRow.Cells.Count];

            for (var j = 0; j < sourceRow.Cells.Count; j++)
            {
                cellValues[j] = sourceRow.Cells[j].Value;
            }

            if (hitTestInfo.SectionIndex != -1)
            {
                if (sourceRow.Index < hitTestInfo.SectionIndex)
                {
                    grdCommon.Rows.Insert(hitTestInfo.SectionIndex + 1, cellValues);
                }
                else
                {
                    grdCommon.Rows.Insert(hitTestInfo.SectionIndex, cellValues);
                }
                if (DragDropDestRowIndex == -1)
                {
                    DragDropDestRowIndex = 0;
                }
                grdCommon.Rows[DragDropDestRowIndex].Selected = true;
            }
            else
            {
                grdCommon.Rows.Add(cellValues);
                grdCommon.Rows[grdCommon.RowCount - 1].Selected = true;
            }
            sourceRow.GcMultiRow.Rows.Remove(sourceRow);

            //順序
            for (var i = 0; i < grdCommon.Rows.Count; i++)
            {
                grdCommon.Rows[i][CellName("ItemName")].Value = grdCommon.Rows[i][CellName("ItemName")].Value;
            }

            grdCommon.Invalidate();
            DragDropDestRowIndex = -1;
            BindGridSettingForPreview();
        }

        private void grdSetting_DragLeave(object sender, EventArgs e)
        {
            var grdCommon = sender as GcMultiRow;
            grdCommon.Invalidate();
            DragDropDestRowIndex = -1;
            BindGridSettingForPreview();
        }

        private void grdSetting_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            var grdCommon = sender as GcMultiRow;
            var clientPoint = grdCommon.PointToClient(new Point(e.X, e.Y));
            var hitTestInfo = grdCommon.HitTest(clientPoint.X, clientPoint.Y);
            var newRowIndex = 0;
            var sourceRow = (Row)e.Data.GetData(typeof(Row));
            if (hitTestInfo.Type == HitTestType.Row)
            {
                if (sourceRow.Index < hitTestInfo.SectionIndex)
                {
                    DragDropDestRowIndex = hitTestInfo.SectionIndex + 1;
                }
                else
                {
                    DragDropDestRowIndex = hitTestInfo.SectionIndex;
                }
            }
            else if (hitTestInfo.Type == HitTestType.None)
            {
                DragDropDestRowIndex = grdCommon.RowCount;
            }
            else if (hitTestInfo.Type == HitTestType.HorizontalScrollBar)
            {
                var topPos = grdCommon.FirstDisplayedCellPosition;
                if (topPos.RowIndex < grdCommon.RowCount)
                {
                    newRowIndex = topPos.RowIndex + 1;
                }
                else
                {
                    newRowIndex = topPos.RowIndex;
                }
                grdCommon.FirstDisplayedCellPosition = new CellPosition(newRowIndex, topPos.CellIndex);
            }
            else if (hitTestInfo.Type == HitTestType.ColumnHeader)
            {
                var topPos = grdCommon.FirstDisplayedCellPosition;

                if (topPos.RowIndex != 0)
                {
                    newRowIndex = topPos.RowIndex < 0 ? 0 : topPos.RowIndex - 1;
                    grdCommon.FirstDisplayedCellPosition = new CellPosition(newRowIndex, topPos.CellIndex);
                }
            }
            grdCommon.Invalidate();
        }

        private void grdSetting_MouseDown(object sender, MouseEventArgs e)
        {
            var grdCommon = sender as GcMultiRow;

            if (grdCommon.CurrentCellPosition.CellIndex != 0) return;

            var hitTestInfo = grdCommon.HitTest(e.X, e.Y);

            if (hitTestInfo.Type == HitTestType.Row)
            {
                grdCommon.DoDragDrop(grdCommon.Rows[hitTestInfo.SectionIndex], DragDropEffects.Move);
            }
        }

        private void grdSetting_SectionPainting(object sender, SectionPaintingEventArgs e)
        {
            var grdCommon = sender as GcMultiRow;
            using (Pen redPen = new Pen(Color.Red, 3))
            {
                if (e.Scope == CellScope.Row)
                {
                    if (e.SectionIndex == DragDropDestRowIndex)
                    {
                        e.PaintSectionBackground(e.SectionBounds);
                        e.PaintSectionBorder(e.SectionBounds);
                        e.PaintCells(e.SectionBounds);

                        if (grdCommon.CurrentCellPosition.CellIndex == 0)
                        {
                            e.Graphics.DrawLine(redPen, e.SectionBounds.Left, e.SectionBounds.Top, e.SectionBounds.Right, e.SectionBounds.Top);
                        }
                        e.Handled = true;
                    }
                    else if (DragDropDestRowIndex == grdCommon.RowCount && grdCommon.CurrentCellPosition.CellIndex == 0)
                    {
                        e.Graphics.DrawLine(redPen, e.SectionBounds.Left, e.SectionBounds.Bottom, e.SectionBounds.Right, e.SectionBounds.Bottom);
                    }
                }
            }
        }
        #endregion

        #region WebService呼び出し
        private List<GridSetting> GetDefaultGridSetting()
        {
            var resultList = new List<GridSetting>();

            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GridSettingMasterClient>();
                var result = await service.GetDefaultItemsAsync(SessionKey, CompanyId, Login.UserId, SelectedGridId);

                if (result.ProcessResult.Result)
                {
                    resultList = result.GridSettings;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return resultList;
        }

        private List<GridSetting> GetGridSetting()
        {
            var list = new List<GridSetting>();

            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GridSettingMasterClient>();
                var result = await service.GetItemsAsync(SessionKey, CompanyId, Login.UserId, SelectedGridId);

                if (result.ProcessResult.Result)
                {
                    list = result.GridSettings;
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return list;
        }

        private void SaveGridSetting()
        {
            var gridSetting = GetPreviewGridSetting();

            if (rdoAllUser.Checked)
            {
                foreach (var item in gridSetting)
                {
                    item.AllLoginUser = true;
                }
            }

            var saveResult = new GridSettingResult();

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GridSettingMasterClient>();
                saveResult = await service.SaveAsync(SessionKey, gridSetting.ToArray());
                if (!saveResult.ProcessResult.Result)
                {
                    ShowWarningDialog(MsgErrSaveError);
                    return;
                }

                ClearData();
                DispStatusMessage(MsgInfSaveSuccess);
                cmbGridType.Select();
            });
        }
        #endregion
    }
}
