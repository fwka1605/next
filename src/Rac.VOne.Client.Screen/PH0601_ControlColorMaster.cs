using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.ControlColorMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    public partial class PH0601:VOneScreenBase
    {
        #region initialize
        private ControlColor ControlColorData { get; set; }
        private bool IsChanged { get { return (ControlColorData.ButtonBackColor.Name != "0" && IsColorChanged()); } }
        private bool IsRestoreDefault { get; set; } = false;

        public PH0601()
        {
            InitializeComponent();
            grdSearch.SetupShortcutKeys();
            grdInput.SetupShortcutKeys();
            grdMatchingSequential.SetupShortcutKeys();
            grdMatchingIndividualBilling.SetupShortcutKeys();
            grdMatchingIndividualReceipt.SetupShortcutKeys();

            Text = "色マスター";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = SaveControlColor;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Enabled(false);

            BaseContext.SetFunction05Caption("初期値表示");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = DefaultColorSetting;

            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exist;
        }

        private bool IsColorChanged()
        {
            bool result = false;

            result = btnGridRow.BackColor.ToArgb() != ControlColorData.GridRowBackColor.ToArgb()
                  || btnGridAlternatingRow.BackColor.ToArgb() != ControlColorData.GridAlternatingRowBackColor.ToArgb()
                  || btnInputGrid.BackColor.ToArgb() != ControlColorData.InputGridBackColor.ToArgb()
                  || btnInputGridAlternating.BackColor.ToArgb() != ControlColorData.InputGridAlternatingBackColor.ToArgb()
                  || btnMatchingGridBilling.BackColor.ToArgb() != ControlColorData.MatchingGridBillingBackColor.ToArgb()
                  || btnMatchingGridReceipt.BackColor.ToArgb() != ControlColorData.MatchingGridReceiptBackColor.ToArgb()
                  || btnMatchingGridBillingSelectedRow.BackColor.ToArgb() != ControlColorData.MatchingGridBillingSelectedRowBackColor.ToArgb()
                  || btnMatchingGridBillingSelectedCell.BackColor.ToArgb() != ControlColorData.MatchingGridBillingSelectedCellBackColor.ToArgb()
                  || btnMatchingGridReceiptSelectedRow.BackColor.ToArgb() != ControlColorData.MatchingGridReceiptSelectedRowBackColor.ToArgb()
                  || btnMatchingGridReceiptSelectedCell.BackColor.ToArgb() != ControlColorData.MatchingGridReceiptSelectedCellBackColor.ToArgb();

            return result;
        }

        private void PH0601_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                InitializeGridForSearch();
                InitializeGridForInput();
                InitializeGridSequencialMatching();
                InitializeGridIndividualBilling();
                InitializeGridIndividualReceipt();

                var loadTask = new List<Task>();

                if (Company == null)
                {
                    Task loadCompany = LoadCompanyAsync();
                    loadTask.Add(loadCompany);
                }

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SetControlColor();
                DisableSelectable();
                grdMatchingSequential.Rows[3][3].Style.ForeColor = Color.Red;
                btnGridRow.Focus();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitializeGridForSearch()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  30, "Header"      , caption: ""                                       , cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, 130, "CustomerCode", caption: "得意先コード", dataField: "CustomerCode", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 130, "CustomerName", caption: "得意先名"    , dataField: "CustomerName", cell: builder.GetTextBoxCell()),
                new CellSetting(height, 100, "Note"        , caption: "備考"        , dataField: "Note"        , cell: builder.GetTextBoxCell()),
            });

            var table = new DataTable();
            table.Columns.Add(new DataColumn("Header", typeof(int)));
            table.Columns.Add(new DataColumn("CustomerCode", typeof(string)));
            table.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            table.Columns.Add(new DataColumn("Note", typeof(string)));
            table.Rows.Add(new object[] { 1, "TEST0001", "テスト1", "備考1" });
            table.Rows.Add(new object[] { 2, "TEST0002", "テスト2", "備考2" });
            table.Rows.Add(new object[] { 3, "TEST0003", "テスト3", "備考3" });
            table.Rows.Add(new object[] { 4, "TEST0004", "テスト4", "備考4" });
            grdSearch.Template = builder.Build();
            grdSearch.DataSource = table;
            grdSearch.HideSelection = true;
        }

        private void InitializeGridForInput()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,   0, "Id"                                    , dataField: "Id" ),
                new CellSetting(height, 130, "Category"         , caption: "請求区分", dataField: "Category"         , cell: builder.GetTextBoxCell(), readOnly: false),
                new CellSetting(height, 130, "Tax"              , caption: "税区"    , dataField: "Tax"              , cell: builder.GetTextBoxCell(), readOnly: false),
                new CellSetting(height, 130, "DebitAccountTitle", caption: "債権科目", dataField: "DebitAccountTitle", cell: builder.GetTextBoxCell(), readOnly: false),
            });

            var table = new DataTable();
            table.Columns.Add(new DataColumn("Id", typeof(int)));
            table.Columns.Add(new DataColumn("Category", typeof(string)));
            table.Columns.Add(new DataColumn("Tax", typeof(string)));
            table.Columns.Add(new DataColumn("DebitAccountTitle", typeof(string)));
            table.Rows.Add(new object[] { 1 });
            table.Rows.Add(new object[] { 2 });
            table.Rows.Add(new object[] { 3 });
            table.Rows.Add(new object[] { 4 });
            grdInput.Template = builder.Build();
            grdInput.DataSource = table;
            grdInput.HideSelection = true;
        }

        private void InitializeGridSequencialMatching()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  50, "Sequencial"  , caption: "一括"        , dataField: "Sequencial"  , cell: builder.GetCheckBoxCell()),
                new CellSetting(height, 100, "CustomerCode", caption: "得意先コード", dataField: "CustomerCode", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 100, "CustomerName", caption: "得意先名"    , dataField: "CustomerName", cell: builder.GetTextBoxCell() ),
                new CellSetting(height, 100, "Balance"     , caption: "差額"        , dataField: "Balance"     , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight)),
            });

            var table = new DataTable();
            table.Columns.Add(new DataColumn("Sequencial", typeof(bool)));
            table.Columns.Add(new DataColumn("CustomerCode", typeof(string)));
            table.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            table.Columns.Add(new DataColumn("Balance", typeof(string)));
            table.Rows.Add(new object[] { true, "TEST0001", "テスト1", "0" });
            table.Rows.Add(new object[] { false, "TEST0002", "テスト2", "3,000" });
            table.Rows.Add(new object[] { true, "TEST0003", "テスト3", "200" });
            table.Rows.Add(new object[] { false, "TEST0004", "テスト4", "-15,000" });
            grdMatchingSequential.Template = builder.Build();
            grdMatchingSequential.DataSource = table;
            grdMatchingSequential.HideSelection = true;
        }

        private void InitializeGridIndividualBilling()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  50, "IndividualClearing", caption: "消"          , dataField: "IndividualClearing", cell: builder.GetCheckBoxCell()),
                new CellSetting(height, 100, "CustomerCode"      , caption: "得意先コード", dataField: "CustomerCode"      , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 100, "CustomerName"      , caption: "得意先名"    , dataField: "CustomerName"      , cell: builder.GetTextBoxCell()),
                new CellSetting(height, 100, "BillingAmount"     , caption: "請求金額"    , dataField: "BillingAmount"     , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight)),
            });

            var table = new DataTable();
            table.Columns.Add(new DataColumn("IndividualClearing", typeof(bool)));
            table.Columns.Add(new DataColumn("CustomerCode", typeof(string)));
            table.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            table.Columns.Add(new DataColumn("BillingAmount", typeof(string)));
            table.Rows.Add(new object[] { true, "TEST0001", "テスト1", "1,000" });
            table.Rows.Add(new object[] { true, "TEST0001", "テスト1", "2,000" });
            table.Rows.Add(new object[] { true, "TEST0001", "テスト1", "3,000" });
            grdMatchingIndividualBilling.Template = builder.Build();
            grdMatchingIndividualBilling.DataSource = table;
            grdMatchingIndividualBilling.HideSelection = false;
        }

        private void InitializeGridIndividualReceipt()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  50, "IndividualClearing", caption: "消"          , dataField: "IndividualClearing", cell: builder.GetCheckBoxCell()),
                new CellSetting(height, 100, "PayerName"         , caption: "振込依頼人名", dataField: "PayerName"),
                new CellSetting(height, 100, "ReceiptAt"         , caption: "入金日"      , dataField: "ReceiptAt"         , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 130, "ReceiptCategory"   , caption: "区分"        , dataField: "ReceiptCategory"),
            });

            var table = new DataTable();
            table.Columns.Add(new DataColumn("IndividualClearing", typeof(bool)));
            table.Columns.Add(new DataColumn("PayerName", typeof(string)));
            table.Columns.Add(new DataColumn("ReceiptAt", typeof(string)));
            table.Columns.Add(new DataColumn("ReceiptCategory", typeof(string)));
            table.Rows.Add(new object[] { true, "ﾃｽﾄ1", "2016/07/27", "振込" });
            table.Rows.Add(new object[] { true, "ﾃｽﾄ1", "2016/07/30", "手形" });
            table.Rows.Add(new object[] { true, "ﾃｽﾄ1", "2016/08/02", "前受" });
            grdMatchingIndividualReceipt.Template = builder.Build();
            grdMatchingIndividualReceipt.DataSource = table;
            grdMatchingIndividualReceipt.HideSelection = false;
        }
        #endregion

        #region Function Key Event
        [OperationLog("登録")]
        private void SaveControlColor()
        {
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                ControlColorData = SetControlColorForSave();
                ControlColorResult result = null;
                var task = ServiceProxyFactory.DoAsync<ControlColorMasterClient>(async client
                    => result = await client.SaveAsync(SessionKey, ControlColorData));
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                var success = result?.ProcessResult.Result ?? false;

                if (success)
                {
                    ColorSetting.FromDb = new ColorSetting(ControlColorData);
                    DispStatusMessage(MsgInfSaveSuccess);
                    btnGridRow.Focus();
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private ControlColor SetControlColorForSave()
        {
            var controlColor = new ControlColor();
            controlColor.CompanyId = CompanyId;
            controlColor.LoginUserId = Login.UserId;
            controlColor.FormBackColor = ColorContext.FormBackColor;
            controlColor.FormForeColor = ColorContext.FormForeColor;
            controlColor.ControlEnableBackColor = ColorContext.ControlEnableBackColor;
            controlColor.ControlDisableBackColor = ColorContext.ControlDisableBackColor;
            controlColor.ControlForeColor = ColorContext.ControlForeColor;
            controlColor.ControlRequiredBackColor = ColorContext.ControlRequiredBackColor;
            controlColor.ControlActiveBackColor = ColorContext.ControlActiveBackColor;
            controlColor.ButtonBackColor = ColorContext.ButtonBackColor;
            controlColor.GridRowBackColor = btnGridRow.BackColor;
            controlColor.GridAlternatingRowBackColor = btnGridAlternatingRow.BackColor;
            controlColor.GridLineColor = ColorContext.GridLineColor;
            controlColor.InputGridBackColor = btnInputGrid.BackColor;
            controlColor.InputGridAlternatingBackColor = btnInputGridAlternating.BackColor;
            controlColor.MatchingGridBillingBackColor = btnMatchingGridBilling.BackColor;
            controlColor.MatchingGridReceiptBackColor = btnMatchingGridReceipt.BackColor;
            controlColor.MatchingGridBillingSelectedRowBackColor = btnMatchingGridBillingSelectedRow.BackColor;
            controlColor.MatchingGridBillingSelectedCellBackColor = btnMatchingGridBillingSelectedCell.BackColor;
            controlColor.MatchingGridReceiptSelectedRowBackColor = btnMatchingGridReceiptSelectedRow.BackColor;
            controlColor.MatchingGridReceiptSelectedCellBackColor = btnMatchingGridReceiptSelectedCell.BackColor;
            controlColor.CreateBy = Login.UserId;
            controlColor.UpdateBy = Login.UserId;

            return controlColor;
        }

        [OperationLog("初期値表示")]
        private void DefaultColorSetting()
        {
            if (!IsRestoreDefault && IsChanged && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                return;

            try
            {
                ClearStatusMessage();
                SetDefaultButtonColor();
                SetColorGridForSearch();
                SetColorGridForInput();
                SetColorGridSequencialMatching();
                grdMatchingIndividualBilling_CellClick(null, null);
                SetColorGridIndividualBilling();
                grdMatchingIndividualReceipt_CellClick(null, null);
                SetColorGridIndividualReceipt();
                IsRestoreDefault = true;
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
            if (IsChanged && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            ClearStatusMessage();
            BaseForm.Close();
        }
        #endregion

        #region 共有メソッド

        private void SetDefaultButtonColor()
        {
            btnGridRow.BackColor = Color.White;
            btnGridAlternatingRow.BackColor = Color.MintCream;
            btnInputGrid.BackColor = Color.White;
            btnInputGridAlternating.BackColor = Color.MintCream;
            btnMatchingGridBilling.BackColor = Color.LightCyan;
            btnMatchingGridReceipt.BackColor = Color.NavajoWhite;
            btnMatchingGridBillingSelectedRow.BackColor = Color.Azure;
            btnMatchingGridBillingSelectedCell.BackColor = Color.Cyan;
            btnMatchingGridReceiptSelectedRow.BackColor = Color.Honeydew;
            btnMatchingGridReceiptSelectedCell.BackColor = Color.PaleGreen;
        }

        private void SetControlColor()
        {
            GetControlColor();

            if(ControlColorData.ButtonBackColor.Name == "0")
            {
                SetDefaultButtonColor();
            }
            else
            {
                btnGridRow.BackColor = ControlColorData.GridRowBackColor;
                btnGridAlternatingRow.BackColor = ControlColorData.GridAlternatingRowBackColor;
                btnInputGrid.BackColor = ControlColorData.InputGridBackColor;
                btnInputGridAlternating.BackColor = ControlColorData.InputGridAlternatingBackColor;
                btnMatchingGridBilling.BackColor = ControlColorData.MatchingGridBillingBackColor;
                btnMatchingGridReceipt.BackColor = ControlColorData.MatchingGridReceiptBackColor;
                btnMatchingGridBillingSelectedRow.BackColor = ControlColorData.MatchingGridBillingSelectedRowBackColor;
                btnMatchingGridBillingSelectedCell.BackColor = ControlColorData.MatchingGridBillingSelectedCellBackColor;
                btnMatchingGridReceiptSelectedRow.BackColor = ControlColorData.MatchingGridReceiptSelectedRowBackColor;
                btnMatchingGridReceiptSelectedCell.BackColor = ControlColorData.MatchingGridReceiptSelectedCellBackColor;
            }

            SetColorGridForSearch();
            SetColorGridForInput();
            SetColorGridSequencialMatching();
            grdMatchingIndividualBilling_CellClick(null, null);
            SetColorGridIndividualBilling();
            grdMatchingIndividualReceipt_CellClick(null, null);
            SetColorGridIndividualReceipt();
        }

        private void SetColorGridForSearch()
        {
            grdSearch.DefaultCellStyle.BackColor = btnGridRow.BackColor;
            grdSearch.AlternatingRowsDefaultCellStyle.BackColor = btnGridAlternatingRow.BackColor;
        }

        private void SetColorGridForInput()
        {
            grdInput.DefaultCellStyle.BackColor = btnInputGrid.BackColor;
            grdInput.AlternatingRowsDefaultCellStyle.BackColor = btnInputGridAlternating.BackColor;
        }

        private void SetColorGridSequencialMatching()
        {
            for (int i = 0; i < grdMatchingSequential.RowCount; i++)
            {
                grdMatchingSequential.Rows[i].Cells[1].Style.BackColor = btnMatchingGridBilling.BackColor;
                grdMatchingSequential.Rows[i].Cells[2].Style.BackColor = btnMatchingGridBilling.BackColor;
            }

            grdMatchingSequential.Rows[0][3].Style.BackColor = btnMatchingGridReceipt.BackColor;
            grdMatchingSequential.Rows[0][3].Style.SelectionBackColor = btnMatchingGridReceipt.BackColor;
            grdMatchingSequential.Rows[2][3].Style.BackColor = btnMatchingGridReceipt.BackColor;
            grdMatchingSequential.Rows[2][3].Style.SelectionBackColor = btnMatchingGridReceipt.BackColor;
        }

        private void SetColorGridIndividualBilling()
        {
            grdMatchingIndividualBilling.DefaultCellStyle.SelectionBackColor = btnMatchingGridBillingSelectedCell.BackColor;
            grdMatchingIndividualBilling.DefaultCellStyle.SelectionForeColor = Color.Black;
            grdMatchingIndividualBilling.CurrentCellBorderLine = new Line(LineStyle.None, Color.Black);
            grdMatchingIndividualBilling.CurrentRowBorderLine = new Line(LineStyle.None, Color.Black);
        }

        private void SetColorGridIndividualReceipt()
        {
            grdMatchingIndividualReceipt.DefaultCellStyle.SelectionBackColor = btnMatchingGridReceiptSelectedCell.BackColor;
            grdMatchingIndividualReceipt.DefaultCellStyle.SelectionForeColor = Color.Black;
            grdMatchingIndividualReceipt.CurrentCellBorderLine = new Line(LineStyle.None, Color.Black);
            grdMatchingIndividualReceipt.CurrentRowBorderLine = new Line(LineStyle.None, Color.Black);
        }

        private void GetControlColor()
        {
            ControlColorData = new ControlColor();

            Task task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ControlColorMasterClient>();
                var result = await service.GetAsync(SessionKey, CompanyId, Login.UserId);
                if (result.ProcessResult.Result && result.Color != null)
                {
                    ControlColorData = result.Color[0];
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
        }

        private void DisableSelectable()
        {
            grdSearch.Rows[3].Selectable = false;
            grdSearch.Rows[2].Selectable = false;
            grdSearch.Rows[1].Selectable = false;
            grdSearch.Rows[0].Selectable = false;
            grdMatchingSequential.Rows[3].Selectable = false;
            grdMatchingSequential.Rows[2].Selectable = false;
            grdMatchingSequential.Rows[1].Selectable = false;
            grdMatchingSequential.Rows[0].Selectable = false;
        }

        #endregion

        #region ボタンイベント

        private void btnGridRow_Click(object sender, EventArgs e)
        {
            btnGridRow.BackColor = setBtnColor(btnGridRow.BackColor);
            SetColorGridForSearch();
        }

        private void btnGridAlternatingRow_Click(object sender, EventArgs e)
        {
            btnGridAlternatingRow.BackColor = setBtnColor(btnGridAlternatingRow.BackColor);
            SetColorGridForSearch();
        }

        private void btnInputGrid_Click(object sender, EventArgs e)
        {
            btnInputGrid.BackColor = setBtnColor(btnInputGrid.BackColor);
            SetColorGridForInput();
        }

        private void btnInputGridAlternating_Click(object sender, EventArgs e)
        {
            btnInputGridAlternating.BackColor = setBtnColor(btnInputGridAlternating.BackColor);
            SetColorGridForInput();
        }

        private void btnMatchingGridBilling_Click(object sender, EventArgs e)
        {
            btnMatchingGridBilling.BackColor = setBtnColor(btnMatchingGridBilling.BackColor);
            SetColorGridSequencialMatching();
        }

        private void btnMatchingGridReceipt_Click(object sender, EventArgs e)
        {
            btnMatchingGridReceipt.BackColor = setBtnColor(btnMatchingGridReceipt.BackColor);
            SetColorGridSequencialMatching();
        }

        private void btnMatchingGridBillingSelectedRow_Click(object sender, EventArgs e)
        {
            btnMatchingGridBillingSelectedRow.BackColor = setBtnColor(btnMatchingGridBillingSelectedRow.BackColor);
            grdMatchingIndividualBilling_CellClick(sender, null);
        }

        private void btnMatchingGridBillingSelectedCell_Click(object sender, EventArgs e)
        {
            btnMatchingGridBillingSelectedCell.BackColor = setBtnColor(btnMatchingGridBillingSelectedCell.BackColor);
            SetColorGridIndividualBilling();
            grdMatchingIndividualBilling_CellClick(sender, null);
        }

        private void btnMatchingGridReceiptSelectedRow_Click(object sender, EventArgs e)
        {
            btnMatchingGridReceiptSelectedRow.BackColor = setBtnColor(btnMatchingGridReceiptSelectedRow.BackColor);
            grdMatchingIndividualReceipt_CellClick(sender, null);
        }

        private void btnMatchingGridReceiptSelectedCell_Click(object sender, EventArgs e)
        {
            btnMatchingGridReceiptSelectedCell.BackColor = setBtnColor(btnMatchingGridReceiptSelectedCell.BackColor);
            grdMatchingIndividualReceipt_CellClick(sender, null);
            SetColorGridIndividualReceipt();
        }

        private Color setBtnColor(Color currentColor)
        {
            var colorDialog = new ColorDialog();

            colorDialog.Color = currentColor;
            //カスタム カラーを定義可能にする (初期値 True)
            colorDialog.AllowFullOpen = true;
            //カスタム カラーを表示した状態にする (初期値 False)
            colorDialog.FullOpen = true;
            //使用可能なすべての色を基本セットに表示する (初期値 False)
            colorDialog.AnyColor = true;
            //純色のみ表示する (初期値 False)
            colorDialog.SolidColorOnly = true;
            //[ヘルプ] ボタンを表示する
            colorDialog.ShowHelp = true;

            return colorDialog.ShowDialog() == DialogResult.OK ? colorDialog.Color : currentColor;
        }

        #endregion

        #region グッリドインベンド

        private void grdMatchingIndividualBilling_CellClick(object sender, CellEventArgs e)
        {
            for (var i = 0; i < grdMatchingIndividualBilling.RowCount; i++)
            {
                if (grdMatchingIndividualBilling.CurrentCellPosition.RowIndex != i)
                {
                    grdMatchingIndividualBilling.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    grdMatchingIndividualBilling.Rows[i].DefaultCellStyle.BackColor = btnMatchingGridBillingSelectedRow.BackColor;
                }
            }
        }

        private void grdMatchingIndividualReceipt_CellClick(object sender, CellEventArgs e)
        {
            for (var i = 0; i < grdMatchingIndividualReceipt.RowCount; i++)
            {
                if (grdMatchingIndividualReceipt.CurrentCellPosition.RowIndex != i)
                {
                    grdMatchingIndividualReceipt.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    grdMatchingIndividualReceipt.Rows[i].DefaultCellStyle.BackColor = btnMatchingGridReceiptSelectedRow.BackColor;
                }
            }
        }

        #endregion
    }
}
