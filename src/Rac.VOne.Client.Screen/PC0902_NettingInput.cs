using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.NettingService;
using Rac.VOne.Client.Screen.SectionMasterService;
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
    /// <summary>相殺入力</summary>
    public partial class PC0902 : VOneScreenBase
    {
        private Currency Currency { get; set; }
        public int CurrencyId { get; set; }
        public int CustomerId { get; set; }
        public string CurrencyCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        private int UseLimitDate { get; set; }
        private int SectionId { get; set; }
        private int Precision { get; set; } = 0;
        private bool IsGridModified
        {
            get
            {
                return grdNettingInput.Rows.Any(x => (x.Cells["celCategoryCode"].Value != null && Convert.ToString(x.Cells["celCategoryCode"].Value) != "")
                                            || (x.Cells["celNote"].Value != null && Convert.ToString(x.Cells["celNote"].Value) != "")
                                            || (x.Cells["celDueAt"].Value != null && Convert.ToString(x.Cells["celDueAt"].Value) != "")
                                            || (x.Cells["celAmount"].Value != DBNull.Value && Convert.ToDecimal(x.Cells["celAmount"].Value) != 0M)
                                            || (x.Cells["celReceiptMemo"].Value != null && Convert.ToString(x.Cells["celReceiptMemo"].Value) != ""));
            }
        }

        private const int ReceiptUseInput = 1;
        private const int ReceiptCategoryType = 2;

        public PC0902()
        {
            InitializeComponent();
            grdNettingDisplay.SetupShortcutKeys();
            grdNettingInput.SetupShortcutKeys();
            grdNettingInput.GridColorType = GridColorType.Input;
            Text = "相殺入力";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("更新");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction09Caption("検索");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = Search;

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Close;
        }

        [OperationLog("更新")]
        private void Save()
        {
            try
            {
                if (!ValidateChildren()) return;

                grdNettingInput.EndEdit();

                ClearStatusMessage();
                if (!ValidateDataEntries() || !ValidateCellEntries())
                    return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                SaveNettingItems();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                ClearStatusMessage();
                if (!ValidateDeleteData())
                    return;

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                DeleteNettingItems();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("検索")]
        private void Search()
        {
            var receiptCategory = this.ShowReceiptCategorySearchDialog(useInput: true);
            if (receiptCategory != null) grdNettingInput.CurrentRow.Cells[1].Value = receiptCategory.Code;
            grdNettingInput.Focus();
            grdNettingInput.CurrentCell = grdNettingInput.Rows[grdNettingInput.CurrentRow.Index].Cells["celCategoryCode"];
            grdNettingInput.Rows[grdNettingInput.CurrentRow.Index].Cells["celCategoryCodeName"].Visible = false;
            grdNettingInput.Rows[grdNettingInput.CurrentRow.Index].Cells["celCategoryCode"].Selected = true;
            ClearStatusMessage();
        }

        [OperationLog("戻る")]
        private void Close()
        {
            if ((IsGridModified || txtSectionCode.Text != "") && !ShowConfirmDialog(MsgQstConfirmClose))
            {
                return;
            }

            BaseForm.Close();
        }

        /// <summary>Initilize 表示用グリッド</summary>
        private void InitializeDisplayGridSetting()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,   0, "Id"                , dataField: "Id"                 , cell: builder.GetNumberCell()                                                              ),
                new CellSetting(height,  40, "ChkDelete"                                           , cell: builder.GetCheckBoxCell()                 , caption: "削除"        , readOnly: false ),
                new CellSetting(height, 470, "CustomerKana"      , dataField: "CustomerKana"       , cell: builder.GetTextBoxCell()                  , caption: "振込依頼人名"                  ),
                new CellSetting(height, 115, "RecordedAt"        , dataField:  "RecordedAt"        , cell: builder.GetDateCell_yyyyMMdd()            , caption: "入金日"                        ),
                new CellSetting(height, 200, "CategoryCode"      , dataField: "CategoryCode"       , cell: builder.GetTextBoxCell()                  , caption: "入金区分"                      ),
                new CellSetting(height, 120, "Amount"            , dataField: "Amount"             , cell: builder.GetTextBoxCurrencyCell(Precision) , caption: "金額"                          ),
                new CellSetting(height,   0, "UseAdvanceReceived", dataField: "UseAdvanceReceived" ),
            });

            grdNettingDisplay.Template = builder.Build();
            grdNettingDisplay.HideSelection = true;
        }

        /// <summary>Initilize 入力用グリッド</summary>
        private void InitializeInputGridSetting()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, autoLocationSet: false);
            var height = builder.DefaultRowHeight;
            var posX = new int[]
            {
                0, 0, 200, 570, 685, 805, 835, 890
            };

            var categoryCodeCell = builder.GetTextBoxCell();
            categoryCodeCell.Style.ImeMode = ImeMode.Disable;
            categoryCodeCell.MaxLength = 2;
            categoryCodeCell.Format = "9";

            var noteCell = builder.GetTextBoxCell();
            noteCell.MaxLength = 140;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,   0, "Id"                 , location: new Point(posX[0], 0), dataField: "Id"               , cell: builder.GetTextBoxCell()                                                                                                 ),
                new CellSetting(height, 200, "CategoryCode"       , location: new Point(posX[1], 0), dataField: "CategoryCode"     , caption: "入金区分", readOnly: false, visible: true , cell: builder.GetTextBoxCell(maxLength:2,ime:ImeMode.Disable,format:"9") ),
                new CellSetting(height, 200, "CategoryCodeName"   , location: new Point(posX[1], 0), dataField: "CategoryCodeName" , caption: "入金区分", readOnly: false, visible: false, cell: builder.GetTextBoxCell()                                           ),
                new CellSetting(height, 370, "Note"               , location: new Point(posX[2], 0), dataField: "Note"             , caption: "備考"    , readOnly: false                 , cell: noteCell                                                          ),
                new CellSetting(height, 115, "DueAt"              , location: new Point(posX[3], 0), dataField: "DueAt"            , caption: "期日"    , readOnly: false                 , cell: builder.GetDateCell_yyyyMMdd(isInput: true)                       ),
                new CellSetting(height, 120, "Amount"             , location: new Point(posX[4], 0), dataField: "Amount"           , caption: "金額"    , readOnly: false                 , cell:  builder.GetNumberCellCurrencyInput(Precision, Precision, 0)      ),
                new CellSetting(height,  30, "ReceiptMemo"        , location: new Point(posX[5], 0), dataField: "ReceiptMemo"      , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter,ImeMode.Disable)                                            ),
                new CellSetting(height,  55, "Memo"               , location: new Point(posX[6], 0), dataField: "Memo"             , cell: builder.GetButtonCell()                                                                                                  ),
                new CellSetting(height,   0, "MemoData"           , location: new Point(posX[6], 0), dataField: "MemoData"         , visible: false                                                                                                                 ),
                new CellSetting(height,  55, "Clear"              , location: new Point(posX[7], 0), dataField: "Clear"            , cell: builder.GetButtonCell()                                                                                                  ),
                new CellSetting(height,   0, "UseLimitDate"       , location: new Point(posX[7], 0), dataField: "UseLimitDate"     , visible: false                                                                                                                 ),
            });

            grdNettingInput.Template = builder.Build();
            grdNettingInput.HideSelection = true;

            //for input grid
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("CategoryCode", typeof(string));
            dt.Columns.Add("Note", typeof(string));
            //dt.Columns.Add("DueAt", typeof(DateTime));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("ReceiptMemo", typeof(string));
            dt.Columns.Add("Memo", typeof(string));
            dt.Columns.Add("MemoData", typeof(string));
            dt.Columns.Add("Clear");
            dt.Columns.Add("UseLimitDate", typeof(string));

            for (int i = 0; i < 5; i++)
            {
                var dr = dt.NewRow();
                dr["Memo"] = "メモ";
                dr["Clear"] = "クリア";
                dt.Rows.Add(dr);
            }
            grdNettingInput.DataSource = dt;
        }

        /// <summary>Set Currency Info</summary>
        private async Task SetCurrencyInfo()
        {
            Currency = null;
            var currencyPreFlg = true;
            if (string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                txtCurrencyCode.Text = "JPY";
                currencyPreFlg = false;
            }

            CurrenciesResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtCurrencyCode.Text });

                if (result.ProcessResult.Result && result.Currencies.Any() && result.Currencies[0] != null)
                {
                    if (currencyPreFlg)
                    {
                        Precision = result.Currencies[0].Precision;
                    }
                    Currency = result.Currencies[0];
                    CurrencyId = result.Currencies[0].Id;
                }
                else
                {
                    CurrencyId = 0;
                }
            });
        }

        /// <summary>Set Category Items When CategoryCode Cell Leave</summary>
        /// <param name="index"> Row Index of CategoryCode Cell</param>
        private void SetCategoryItems(int index)
        {
            if (index >= 0)
            {
                string code = Convert.ToString(grdNettingInput.Rows[index]["celCategoryCode"].EditedFormattedValue);

                if (code != "")
                {
                    code = code.PadLeft(2, '0');
                    grdNettingInput.Rows[index]["celCategoryCode"].Value = code;
                }

                UseLimitDate = 0;

                CategoriesResult result = null;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CategoryMasterClient>();
                    result = await service.GetByCodeAsync(SessionKey, CompanyId, 2, new[] { code });
                });

                ProgressDialog.Start(ParentForm, Task.Run(() => task), false, SessionKey);
                if (result.ProcessResult.Result && result.Categories != null && result.Categories.Any())
                {
                    UseLimitDate = result.Categories[0].UseLimitDate;
                    grdNettingInput.Rows[index]["celCategoryCode"].Value = result.Categories[0].Code;
                    grdNettingInput.Rows[index]["celCategoryCodeName"].Value = result.Categories[0].Code + ":" + result.Categories[0].Name;
                    grdNettingInput.Rows[index]["celId"].Value = result.Categories[0].Id;
                    grdNettingInput.Rows[index]["celUseLimitDate"].Value = result.Categories[0].UseLimitDate;
                }
                else
                {
                    grdNettingInput.Rows[index]["celId"].Value = "";
                    grdNettingInput.Rows[index]["celCategoryCode"].Value = "";
                    grdNettingInput.Rows[index]["celCategoryCodeName"].Value = "";
                    grdNettingInput.Rows[index]["celUseLimitDate"].Value = "";
                }
            }
            if (UseLimitDate != 1)
            {
                grdNettingInput.Rows[index]["celDueAt"].Value = null;
            }
        }

        /// <summary>Search Netting Items</summary>
        private async Task SearchNettingItems()
        {
            NettingsResult result = null;
            var list = new List<Netting>();

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<NettingServiceClient>();
                result = await service.GetItemsAsync(SessionKey, CompanyId, CustomerId, CurrencyId);
                if (result.ProcessResult.Result)
                {
                    if (result.ProcessResult.Result)
                    {
                        InitializeDisplayGridSetting();
                        List<Netting> nettingSearchResult = result.Nettings;
                        grdNettingDisplay.DataSource = new BindingSource(nettingSearchResult, null);

                        nmbTotalOffset.Value = 0;

                        long rowCount = grdNettingDisplay.RowCount;
                        for (int i = 0; i < rowCount; i++)
                        {
                            if (nettingSearchResult[i].UseAdvanceReceived == "1")
                            {
                                grdNettingDisplay.Rows[i]["celChkDelete"].Enabled = false;
                            }
                            else
                            {
                                grdNettingDisplay.Rows[i]["celChkDelete"].Enabled = true;
                            }

                            nmbTotalOffset.Value = nmbTotalOffset.Value + Convert.ToDecimal(grdNettingDisplay.Rows[i]["celAmount"].Value);
                        }
                    }
                }
            });
        }

        /// <summary>Save Netting Items</summary>
        private void SaveNettingItems()
        {
            Func<Row, bool> isCategoryCode = (row) =>
                        row["celCategoryCode"].Value != null
                        && row["celCategoryCode"].Value is string
                        && row["celCategoryCode"].Value.ToString() != "";

            var nettingSearch = new List<Netting>();
            foreach (var row in grdNettingInput.Rows.Where(x => isCategoryCode(x)))
            {
                var netting = new Netting();

                netting.CompanyId = CompanyId;
                netting.CustomerId = CustomerId;
                netting.ReceiptCategoryId = Convert.ToInt32(row.Cells["celId"].Value);
                netting.Note = row.Cells["celNote"].Value.ToString();
                if (row.Cells["celDueAt"].Value != null)
                {
                    netting.DueAt = Convert.ToDateTime(row.Cells["celDueAt"].Value.ToString());
                }
                netting.Amount = Convert.ToDecimal(row.Cells["celAmount"].Value);
                netting.ReceiptMemo = row.Cells["celMemoData"].Value.ToString();
                netting.RecordedAt = datPaymentDate.Value.Value;
                netting.CurrencyId = CurrencyId;
                if (ApplicationControl.UseReceiptSection == 1)
                {
                    netting.SectionId = SectionId;
                }
                else
                {
                    netting.SectionId = null;
                }
                nettingSearch.Add(netting);
            }

            NettingResult resultData = null;
            var success = false;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<NettingServiceClient>();
                resultData = await service.SaveAsync(SessionKey, nettingSearch.ToArray());
                success = resultData?.ProcessResult.Result ?? false;
                if (success) await SearchNettingItems();
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (resultData.ProcessResult.Result)
            {
                DispStatusMessage(MsgInfSaveSuccess);
                grdNettingInput.DataSource = null;
                InitializeInputGridSetting();
                datPaymentDate.Focus();
                datPaymentDate.Text = DateTime.Today.ToShortDateString();
                txtSectionCode.Clear();
                lblSectionName.Clear();
                nmbTotalFee.Value = 0M;
            }
            else
            {
                ShowWarningDialog(MsgErrSaveError);
            }
        }

        /// <summary>Validate For Data Entry</summary>
        private bool ValidateDataEntries()
        {
            //入金日
            if (!datPaymentDate.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, "入金日");
                datPaymentDate.Focus();
                return false;
            }
            //入金部門
            if (ApplicationControl.UseReceiptSection == 1 && string.IsNullOrWhiteSpace(txtSectionCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "入金部門コード");
                txtSectionCode.Focus();
                return false;
            }
            return true;
        }

        /// <summary>Validate For Data Entry Of Each Cell Value</summary>
        private bool ValidateCellEntries()
        {
            Func<Row, bool> isCategoryCode = (row) =>
                           row["celCategoryCode"].Value != null
                           && row["celCategoryCode"].Value is string
                           && row["celCategoryCode"].Value.ToString() != "";
            Func<Row, bool> isAmount = (row) =>
                          row["celAmount"].Value != DBNull.Value
                          && row["celAmount"].Value is decimal
                          && Convert.ToDecimal(row["celAmount"].Value) != 0M;
            Func<Row, bool> isDueAt = (row) =>
                         row["celDueAt"].Value != null
                         && row["celDueAt"].Value is DateTime
                         && (DateTime)row["celDueAt"].Value != null;
            var rowCount = 0;
            foreach (var row in grdNettingInput.Rows.Where(x => isCategoryCode(x) || isAmount(x)))
            {
                rowCount++;
                //入金区分
                if (row.Cells["celCategoryCode"].Value.ToString() == "")
                {
                    ShowWarningDialog(MsgWngInputRequired, "入金区分");
                    grdNettingInput.Focus();
                    if (row.Cells["celCategoryCodeName"].Visible) row.Cells["celCategoryCodeName"].Visible = false;
                    grdNettingInput.CurrentCell.Selected = false;
                    grdNettingInput.CurrentCell = row.Cells["celCategoryCode"];
                    grdNettingInput.CurrentCell.Selected = true;
                    return false;
                }

                //期日
                if (row.Cells["celUseLimitDate"].Value.ToString() == "1" && !isDueAt(row))
                {
                    ShowWarningDialog(MsgWngInputRequired, "期日");
                    grdNettingInput.Focus();
                    grdNettingInput.CurrentCell.Selected = false;
                    grdNettingInput.CurrentCell = row.Cells["celDueAt"];
                    grdNettingInput.CurrentCell.Selected = true;
                    return false;
                }

                //金額
                if (row.Cells["celAmount"].Value == DBNull.Value || Convert.ToDecimal(row.Cells["celAmount"].Value) == 0M)
                {
                    ShowWarningDialog(MsgWngInputRequired, "金額");
                    grdNettingInput.Focus();
                    grdNettingInput.CurrentCell.Selected = false;
                    grdNettingInput.CurrentCell = row.Cells["celAmount"];
                    grdNettingInput.CurrentCell.Selected = true;
                    return false;
                }
            }

            if (rowCount == 0)
            {
                ShowWarningDialog(MsgWngInputGridRequired);
                grdNettingInput.Focus();
                grdNettingInput.CurrentCell.Selected = false;
                if (grdNettingInput.Rows[0].Cells["celCategoryCodeName"].Visible)
                    grdNettingInput.Rows[0].Cells["celCategoryCodeName"].Visible = false;
                grdNettingInput.CurrentCell = grdNettingInput.Rows[0].Cells["celCategoryCode"];
                grdNettingInput.CurrentCell.Selected = true;
                return false;
            }

            return true;
        }

        /// <summary>Delete Netting Items</summary>
        private void DeleteNettingItems()
        {
            var nettingId = new List<long>();

            Func<Row, bool> isExistDeleteId = (row) =>
                           row.Cells[1].Value != null
                           && Convert.ToInt32(row.Cells[1].Value) == 1;
            foreach (var row in grdNettingDisplay.Rows.Where(x => isExistDeleteId(x)))
            {
                nettingId.Add(Convert.ToInt64(row.Cells["celId"].Value.ToString()));
            }

            NettingResult resultData = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<NettingServiceClient>();
                resultData = await service.DeleteAsync(SessionKey, nettingId.ToArray());
                if (resultData.ProcessResult.Result) await SearchNettingItems();
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (resultData.ProcessResult.Result)
            {
                DispStatusMessage(MsgInfDeleteSuccess);
                datPaymentDate.Focus();
            }
            else
            {
                ShowWarningDialog(MsgErrDeleteError);
            }
        }

        private bool IsChecked(Row row) => Convert.ToBoolean(row["celChkDelete"].Value);

        /// <summary>Validate For Deleting Data</summary>
        private bool ValidateDeleteData()
        {
            if (!grdNettingDisplay.Rows.Any(x => IsChecked(x)))
            {
                ShowWarningDialog(MsgWngNoDeleteNettingData);
                return false;
            }
            return true;
        }

        private void PC0902_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }

                loadTask.Add(LoadControlColorAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask).ContinueWith(async t =>
                {
                    InitializeSetting();
                    await SetCurrencyInfo();
                    InitializeInputGridSetting();
                    SetTextBoxSetting();
                    await SearchNettingItems();
                }, TaskScheduler.FromCurrentSynchronizationContext()).Unwrap(), false, SessionKey);

                SuspendLayout();
                ResumeLayout();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>Initialize For Control </summary>
        private void InitializeSetting()
        {
            datPaymentDate.Value = DateTime.Today;
            datPaymentDate.Select();
            txtCustomerCode.Text = CustomerCode;
            lblCustomerName.Text = CustomerName;
            txtCurrencyCode.Text = CurrencyCode;

            if (ApplicationControl.UseForeignCurrency == 0)
            {
                txtCurrencyCode.Hide();
                lblCurrencyCode.Hide();
            }

            if (ApplicationControl.UseReceiptSection == 0)
            {
                lblSectionCode.Hide();
                btnSectionCode.Hide();
                txtSectionCode.Hide();
                lblSectionName.Hide();
            }

            var expression = new DataExpression(ApplicationControl);


            txtSectionCode.Format = expression.SectionCodeFormatString;
            txtSectionCode.MaxLength = expression.SectionCodeLength;
            txtSectionCode.PaddingChar = expression.SectionCodePaddingChar;
        }

        private void SetTextBoxSetting()
        {
            nmbTotalFee.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
            nmbTotalOffset.DisplayFields.AddRange(GetNumberFormat(), "", "", "-", "");
        }

        /// <summary>Display Format For NumberControl </summary>
        public string GetNumberFormat()
        {
            string displayFormatString = "0";
            var displayFieldString = "#,###,###,###,##0";
            if (Precision > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < Precision; i++)
                {
                    displayFieldString += displayFormatString;
                }
            }
            return displayFieldString;
        }

        private void grdNettingInput_CellClick(object sender, CellEventArgs e)
        {
            if (e.CellName.ToString() == "celMemo")
            {
                string memo = grdNettingInput.Rows[e.RowIndex].Cells["celMemoData"].Value.ToString();
                using (var form = ApplicationContext.Create(nameof(PH9906)))
                {
                    var screen = form.GetAll<PH9906>().First();
                    screen.Id = 0L; // register netting data and memo
                    screen.MemoType = MemoType.ReceiptMemo;
                    screen.Memo = memo;
                    screen.InitializeParentForm("入金メモ");
                    var result = ApplicationContext.ShowDialog(ParentForm, form, true);
                    if (result == DialogResult.OK)
                    {
                        grdNettingInput.Rows[e.RowIndex].Cells["celMemoData"].Value = screen.Memo;
                        grdNettingInput.Rows[e.RowIndex].Cells["celReceiptMemo"].Value
                            = string.IsNullOrEmpty(screen.Memo) ? "" : "*";
                    }
                }
            }
            else if (e.CellName.ToString() == "celClear")
            {
                if (grdNettingInput.Rows[e.RowIndex].Cells["celAmount"].Value != DBNull.Value
                    && Convert.ToDecimal(grdNettingInput.Rows[e.RowIndex].Cells["celAmount"].Value) != 0M)
                {
                    nmbTotalFee.Value = nmbTotalFee.Value - Convert.ToDecimal(grdNettingInput.Rows[e.RowIndex].Cells["celAmount"].Value);
                }
                grdNettingInput.Rows[e.RowIndex].Cells["celId"].Value = "";
                grdNettingInput.Rows[e.RowIndex].Cells["celCategoryCode"].Value = "";
                grdNettingInput.Rows[e.RowIndex].Cells["celCategoryCodeName"].Value = "";
                grdNettingInput.Rows[e.RowIndex].Cells["celNote"].Value = "";
                grdNettingInput.Rows[e.RowIndex].Cells["celDueAt"].Value = (DateTime?)null;
                grdNettingInput.Rows[e.RowIndex].Cells["celAmount"].Value = DBNull.Value;
                grdNettingInput.Rows[e.RowIndex].Cells["celReceiptMemo"].Value = "";
                grdNettingInput.Rows[e.RowIndex].Cells["celMemoData"].Value = "";
                grdNettingInput.Rows[e.RowIndex].Cells["celUseLimitDate"].Value = "";
                grdNettingInput.Rows[e.RowIndex].Cells["celDueAt"].ReadOnly = true;
            }
            else if (e.CellName.ToString() == "celDueAt")
            {
                if (grdNettingInput.Rows[e.RowIndex]["celUseLimitDate"].Value.ToString() == "0" || grdNettingInput.Rows[e.RowIndex]["celUseLimitDate"].Value.ToString() == "")
                {
                    grdNettingInput.Rows[e.RowIndex]["celDueAt"].ReadOnly = true;
                }
                else
                {
                    grdNettingInput.Rows[e.RowIndex]["celDueAt"].ReadOnly = false;
                }
            }

        }

        private void grdNettingInput_CellEnter(object sender, CellEventArgs e)
        {
            if (e.CellName.ToString() == "celDueAt")
            {
                if (grdNettingInput.Rows[e.RowIndex]["celUseLimitDate"].Value.ToString() == "0" || grdNettingInput.Rows[e.RowIndex]["celUseLimitDate"].Value.ToString() == "")
                {
                    grdNettingInput.Rows[e.RowIndex]["celDueAt"].ReadOnly = true;
                }
                else
                {
                    grdNettingInput.Rows[e.RowIndex]["celDueAt"].ReadOnly = false;
                }
            }

            if (e.CellName.ToString() == "celCategoryCode")
            {
                BaseContext.SetFunction09Enabled(this.ActiveControl == grdNettingInput);
            }
            else if (e.CellName.ToString() == "celCategoryCodeName")
            {
                grdNettingInput.CurrentCell = grdNettingInput.Rows[e.RowIndex].Cells["celCategoryCode"];
                grdNettingInput.Rows[e.RowIndex].Cells["celCategoryCodeName"].Visible = false;
                if (grdNettingInput.Rows[e.RowIndex].Cells["celCategoryCodeName"].Value.ToString() != "")
                {
                    grdNettingInput.Rows[e.RowIndex].Cells["celCategoryCode"].Value = grdNettingInput.Rows[e.RowIndex].Cells["celCategoryCode"].Value.ToString().PadLeft(2, '0');
                }
                else
                {
                    grdNettingInput.Rows[e.RowIndex].Cells["celCategoryCode"].Value = "";
                }
            }
            else
            {
                BaseContext.SetFunction09Enabled(false);
            }
        }

        private void grdNettingInput_CellValidated(object sender, CellEventArgs e)
        {
            grdNettingInput.EndEdit();
            nmbTotalFee.Value = 0;
            int rowCount = grdNettingInput.RowCount;

            for (int i = 0; i < rowCount; i++)
            {
                if (e.CellName.ToString() == "celAmount"
                    && (grdNettingInput.Rows[i]["celAmount"].Value != DBNull.Value
                    && (Convert.ToDecimal(grdNettingInput.Rows[i]["celAmount"].Value) == 0M)))
                {
                    grdNettingInput.Rows[i]["celAmount"].Value = DBNull.Value;
                }
                if (grdNettingInput.Rows[i]["celAmount"].Value != DBNull.Value)
                {
                    nmbTotalFee.Value = nmbTotalFee.Value + Convert.ToDecimal(grdNettingInput.Rows[i]["celAmount"].Value);
                }
            }

        }

        private void grdNettingDisplay_CellEnter(object sender, CellEventArgs e)
        {
            grdNettingInput.CurrentCell.Selected = false;
            BaseContext.SetFunction09Enabled(false);
            int rowCount = grdNettingDisplay.RowCount;
            for (int i = 0; i < rowCount; i++)
            {
                if (grdNettingDisplay.Rows[i]["celUseAdvanceReceived"].Value.ToString() == "1")
                {
                    this.grdNettingDisplay.Rows[i].Cells["celChkDelete"].Enabled = false;
                }
            }
        }

        private void btnSectionCode_Click(object sender, EventArgs e)
        {
            // Clear Message
            ClearStatusMessage();
            var section = this.ShowSectionSearchDialog();
            if (section != null)
            {
                txtSectionCode.Text = section.Code;
                lblSectionName.Text = section.Name;
                SectionId = section.Id;
                ClearStatusMessage();
            }
        }

        private void txtSectionCode_Validated(object sender, EventArgs e)
        {
            // Clear Message
            ClearStatusMessage();

            try
            {
                string sectionCode = txtSectionCode.Text; // 入金部門

                // if Empty
                if (string.IsNullOrEmpty(sectionCode)
                    || string.IsNullOrWhiteSpace(sectionCode))
                {
                    lblSectionName.Clear();
                    SectionId = 0;
                    return;
                }

                SectionsResult result = null;
                var list = new List<Web.Models.Section>();
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<SectionMasterClient>();
                    result = await service.GetByCodeAsync(
                     SessionKey,
                     CompanyId,
                     new string[] { sectionCode });

                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result.ProcessResult.Result && result.Sections != null)
                {
                    list = result.Sections;
                }

                Web.Models.Section sectionResult = null;
                sectionResult = list.Find(s => s.Code == sectionCode);

                if (sectionResult == null)
                {
                    // テキストボックス未入力時、メッセージ【W00320】を表示
                    string code = txtSectionCode.Text;
                    ShowWarningDialog(MsgWngMasterNotExist, "入金部門", code);
                    txtSectionCode.Clear();
                    lblSectionName.Clear();
                    SectionId = 0;
                    txtSectionCode.Focus();
                }
                else
                {
                    lblSectionName.Text = sectionResult.Name;
                    SectionId = sectionResult.Id;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grdNettingInput_CellLeave(object sender, CellEventArgs e)
        {
            if (e.CellName.ToString() == "celCategoryCode")
            {
                try
                {
                    SetCategoryItems(e.RowIndex);
                    grdNettingInput.Rows[e.RowIndex]["celCategoryCodeName"].Visible = true;
                }
                catch (Exception ex)
                {
                    Debug.Fail(ex.Message);
                    NLogHandler.WriteErrorLog(this, ex, SessionKey);
                }

            }
        }

        private void grdNettingDisplay_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            grdNettingDisplay.CommitEdit(DataErrorContexts.Commit);
        }
    }
}
