using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>
    /// EBデータ取込対象外口座設定
    /// </summary>
    public partial class PD0103 : VOneScreenBase
    {
        public static Dictionary<int, string> AccountTypeDictionary = // Id -> Name
            new Dictionary<int, string>
            {
                    { 1, "普通預金" },
                    { 2, "当座預金" },
                    { 4, "貯蓄預金" },
                    { 5, "通知預金" },
            };

        #region 初期化

        public PD0103()
        {
            InitializeComponent();

            Text = "EBデータ取込対象外口座設定";
            grdSettings.SetupShortcutKeys();
 
            AddHandlers();
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = F01_RegisterSettings;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = F02_ClearScreen;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = F03_DeleteSettings;

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = F10_Exit;
        }

        private void PD0103_Load(object sender, EventArgs e)
        {
            SetScreenName();

            var loadTask = new List<Task>
            {
                LoadApplicationControlAsync(),
                LoadCompanyAsync(),
                LoadControlColorAsync(),
            };
            ProgressDialog.Start(BaseForm, Task.WhenAll(loadTask), false, SessionKey);

            InitializeComboBox();
            InitializeGrid();

            ClearScreen();
            LoadGridData();

        }

        private void InitializeComboBox()
        {
            foreach (var id in AccountTypeDictionary.Keys)
            {
                cmbAccountType.Items.Add(new ListItem(AccountTypeDictionary[id], id));
            }
        }

        private void InitializeGrid()
        {
            grdSettings.Template = GetGridTemplate();
            grdSettings.HideSelection = true;
        }

        #region GridRow

        private class GridRow
        {
            public GridRow(EBExcludeAccountSetting model)
            {
                // グリッド編集機能を持たないのでコンストラクタ内で全てのプロパティ値をセットする。モデルも保持しない。

                BankCode = model.BankCode;
                BranchCode = model.BranchCode;

                var accountType = "";
                AccountTypeDictionary.TryGetValue(model.AccountTypeId, out accountType);
                AccountType = accountType;

                var code = model.PayerCode;
                if (!string.IsNullOrWhiteSpace(code) && 10 <= code.Length)
                {
                    PayerCode1 = code.Substring(0, 3);
                    PayerCode2 = code.Substring(3, 7);
                }
            }

            /// <summary>銀行コード</summary>
            public string BankCode { get; set; }

            /// <summary>支店コード</summary>
            public string BranchCode { get; set; }

            /// <summary>預金種別</summary>
            public string AccountType { get; set; }

            /// <summary>仮想支店コード</summary>
            public string PayerCode1 { get; set; }

            /// <summary>仮想口座番号</summary>
            public string PayerCode2 { get; set; }
        }

        #endregion GridRow

        private Template GetGridTemplate()
        {
            var template = new Template();

            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);

            var rowHeight = builder.DefaultRowHeight;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(rowHeight, 120, "BankCode",    caption: "銀行コード",     dataField: "BankCode",    cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(rowHeight, 120, "BranchCode",  caption: "支店コード",     dataField: "BranchCode",  cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(rowHeight, 120, "AccountType", caption: "預金種別",       dataField: "AccountType", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleLeft)),
                new CellSetting(rowHeight, 120, "PayerCode1",  caption: "仮想支店コード", dataField: "PayerCode1",  cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(rowHeight, 120, "PayerCode2",  caption: "仮想口座番号",   dataField: "PayerCode2",  cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
            });

            return builder.Build();
        }

        #endregion 初期化

        #region 画面イベント

        [OperationLog("登録")]
        private void F01_RegisterSettings()
        {
            try
            {
                var error = ValidateForRegister();
                if (error != null)
                {
                    if (error is ValidationError<Control>)
                    {
                        var actualError = (ValidationError<Control>)error;
                        if (actualError.FocusTarget != null)
                        {
                            actualError.FocusTarget.Focus();
                        }
                    }

                    ShowWarningDialog(error.MessageId, error.MessageArgs);
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var setting = new EBExcludeAccountSetting
                {
                    CompanyId = CompanyId,
                    BankCode = txtBankCode.Text.PadLeft(4, '0'),
                    BranchCode = txtBranchCode.Text.PadLeft(3, '0'),
                    AccountTypeId = AccountTypeDictionary.Single(kv => kv.Value == cmbAccountType.Text).Key,
                    PayerCode = txtPayerCode1.Text.PadLeft(3, '0') + txtPayerCode2.Text.PadLeft(7, '0'),
                    CreateBy = Login.UserId,
                    UpdateBy = Login.UserId,
                };

                var task = SaveEBExcludeAccountSettingListAsync(SessionKey, setting);
                ProgressDialog.Start(BaseForm, task, false, SessionKey);

                if (task.Result == null)
                {
                    ShowWarningDialog(MsgErrSaveError);
                    return;
                }

                ClearScreen();
                LoadGridData();
                ShowWarningDialog(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void F02_ClearScreen()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

            ClearScreen();

          
        }

        [OperationLog("削除")]
        private void F03_DeleteSettings()
        {
            try
            {
                var error = ValidateForDeletion();
                if (error != null)
                {
                    if (error is ValidationError<Control>)
                    {
                        var actualError = (ValidationError<Control>)error;
                        if (actualError.FocusTarget != null)
                        {
                            actualError.FocusTarget.Focus();
                        }
                    }

                    ShowWarningDialog(error.MessageId, error.MessageArgs);
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    return;
                }

                var target = new EBExcludeAccountSetting
                {
                    CompanyId = CompanyId,
                    BankCode = txtBankCode.Text.PadLeft(4, '0'),
                    BranchCode = txtBranchCode.Text.PadLeft(3, '0'),
                    AccountTypeId = AccountTypeDictionary.Single(kv => kv.Value == cmbAccountType.Text).Key,
                    PayerCode = txtPayerCode1.Text.PadLeft(3, '0') + txtPayerCode2.Text.PadLeft(7, '0'),
                };

                var task = DeleteEBExcludeAccountSettingListAsync(SessionKey, target);
                ProgressDialog.Start(BaseForm, task, false, SessionKey);

                if (!task.Result.HasValue || task.Result == 0)
                {
                    ShowWarningDialog(MsgErrDeleteError);
                    return;
                }

                ClearScreen();
                LoadGridData();
                DispStatusMessage(MsgInfDeleteSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #region 終了処理
        [OperationLog("戻る")]
        private void F10_Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            BaseForm.Close();
        }
        #endregion

        /// <summary>
        /// グリッドダブルクリック<para/>
        /// 該当行の内容を入力欄に表示する。
        /// </summary>
        private void grdSettings_CellContentDoubleClick(object sender, CellEventArgs e)
        {
            ClearStatusMessage();

            if (e.RowIndex >= 0)
            {
                var row = (GridRow)grdSettings.Rows[e.RowIndex].DataBoundItem;

                if (grdSettings != null)
                {
                    txtBankCode.Text = row.BankCode;
                    txtBranchCode.Text = row.BranchCode;
                    cmbAccountType.SelectedValue = row.AccountType;
                    txtPayerCode1.Text = row.PayerCode1;
                    txtPayerCode2.Text = row.PayerCode2;

                    Modified = false;
                }
            } 
        }

        #endregion 画面イベント

        #region Functions

        /// <summary>
        /// 画面をクリアする(グリッドを除く)。
        /// </summary>
        private void ClearScreen()
        {

            ClearStatusMessage();

            txtBankCode.Clear();
            txtBranchCode.Clear();
            cmbAccountType.SelectedIndex = 0;
            txtPayerCode1.Clear();
            txtPayerCode2.Clear();

            txtBankCode.Select();
            Modified = false;
        }


        private void LoadGridData()
        {
            var task = GetEBExcludeAccountSettingListAsync(SessionKey, CompanyId);
            ProgressDialog.Start(BaseForm, task, false, SessionKey);

            var rows = task.Result;
            if (rows == null)
            {
                grdSettings.DataSource = new BindingSource(new List<GridRow>(), null);
                return;
            }

            var gridRows = rows
                .OrderBy(row => row.BankCode)
                .ThenBy(row => row.BranchCode)
                .ThenBy(row => row.AccountTypeId)
                .ThenBy(row => row.PayerCode)
                .Select(row => new GridRow(row))
                .ToList();


            grdSettings.DataSource = new BindingSource(gridRows, null);
        }

        private class ValidationError
        {
            public string MessageId { get; private set; }
            public string[] MessageArgs { get; private set; }

            public ValidationError(string messageId, params string[] messageArgs)
            {
                MessageId = messageId;
                MessageArgs = messageArgs;
            }
        }

        private class ValidationError<TFocusTarget> : ValidationError
        {
            public TFocusTarget FocusTarget { get; private set; }

            public ValidationError(TFocusTarget focusTarget, string messageId, params string[] messageArgs) : base(messageId, messageArgs)
            {
                FocusTarget = focusTarget;
            }
        }

        private ValidationError ValidateForRegister()
        {
            // 必須チェック
            if (string.IsNullOrEmpty(txtBankCode.Text))
            {
                return new ValidationError<Control>(txtBankCode, MsgWngInputRequired, "銀行コード");
            }
            if (string.IsNullOrEmpty(txtBranchCode.Text))
            {
                return new ValidationError<Control>(txtBranchCode, MsgWngInputRequired, "支店コード");
            }
            if (string.IsNullOrEmpty(txtPayerCode1.Text))
            {
                return new ValidationError<Control>(txtPayerCode1, MsgWngInputRequired, "仮想支店コード");
            }
            if (string.IsNullOrEmpty(txtPayerCode2.Text))
            {
                return new ValidationError<Control>(txtPayerCode2, MsgWngInputRequired, "仮想口座番号");
            }

            // 重複チェック(画面内)
            var existing = grdSettings.Rows
                .Select(row => (GridRow)row.DataBoundItem)
                .Any(row =>
                {
                    var c1 = (txtBankCode.Text.PadLeft(4, '0') == row.BankCode);
                    var c2 = (txtBranchCode.Text.PadLeft(3, '0') == row.BranchCode);
                    var c3 = (cmbAccountType.Text == row.AccountType);
                    var c4 = (txtPayerCode1.Text.PadLeft(3, '0') == row.PayerCode1);
                    var c5 = (txtPayerCode2.Text.PadLeft(7, '0') == row.PayerCode2);

                    return (c1 && c2 && c3 && c4 && c5);
                });
            if (existing)
            {
                return new ValidationError<Control>(txtBankCode, MsgWngAlreadyExistedData);
            }

            return null;
        }

        private ValidationError ValidateForDeletion()
        {
            // 必須チェック
            if (string.IsNullOrEmpty(txtBankCode.Text))
            {
                return new ValidationError<Control>(txtBankCode, MsgWngInputRequired, "銀行コード");
            }
            if (string.IsNullOrEmpty(txtBranchCode.Text))
            {
                return new ValidationError<Control>(txtBranchCode, MsgWngInputRequired, "支店コード");
            }
            if (string.IsNullOrEmpty(txtPayerCode1.Text))
            {
                return new ValidationError<Control>(txtPayerCode1, MsgWngInputRequired, "仮想支店コード");
            }
            if (string.IsNullOrEmpty(txtPayerCode2.Text))
            {
                return new ValidationError<Control>(txtPayerCode2, MsgWngInputRequired, "仮想口座番号");
            }

            // 存在チェック(画面内)
            var existing = grdSettings.Rows
                .Select(row => (GridRow)row.DataBoundItem)
                .Any(row =>
                {
                    var c1 = (txtBankCode.Text.PadLeft(4, '0') == row.BankCode);
                    var c2 = (txtBranchCode.Text.PadLeft(3, '0') == row.BranchCode);
                    var c3 = (cmbAccountType.Text == row.AccountType);
                    var c4 = (txtPayerCode1.Text.PadLeft(3, '0') == row.PayerCode1);
                    var c5 = (txtPayerCode2.Text.PadLeft(7, '0') == row.PayerCode2);

                    return (c1 && c2 && c3 && c4 && c5);
                });
            if (!existing)
            {
                return new ValidationError<Control>(txtBankCode, MsgWngNoDeleteData);
            }

            return null;
        }

        #endregion Functions

        #region Web Service

        private static async Task<List<EBExcludeAccountSetting>> GetEBExcludeAccountSettingListAsync(string sessionKey, int companyId)
        {
            EBExcludeAccountSettingListResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<EBExcludeAccountSettingMasterService.EBExcludeAccountSettingMasterClient>();
                result = await client.GetItemsAsync(sessionKey, companyId);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.EBExcludeAccountSettingList;

        }

        private static async Task<EBExcludeAccountSetting> SaveEBExcludeAccountSettingListAsync(string sessionKey, EBExcludeAccountSetting setting)
        {
            EBExcludeAccountSettingResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<EBExcludeAccountSettingMasterService.EBExcludeAccountSettingMasterClient>();
                result = await client.SaveAsync(sessionKey, setting);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.EBExcludeAccountSetting;
        }

        private static async Task<int?> DeleteEBExcludeAccountSettingListAsync(string sessionKey, EBExcludeAccountSetting target)
        {
            CountResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<EBExcludeAccountSettingMasterService.EBExcludeAccountSettingMasterClient>();
                result = await client.DeleteAsync(sessionKey, target);
            });

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.Count;
        }

        #endregion Web Service


        #region 全入力項目変更イベント処理
        private void AddHandlers()
        {
            foreach (Control control in grpInputForm.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }

                if (control is Common.Controls.VOneComboControl)
                {
                    ((Common.Controls.VOneComboControl)control).SelectedIndexChanged +=
                        new EventHandler(OnContentChanged);
                }
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }
        #endregion

    }
}
