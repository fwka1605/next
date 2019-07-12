using GrapeCity.Win.Editors;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.EBFormatMasterService;
using Rac.VOne.Client.Screen.EBFileSettingMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.TaskScheduleMasterService;
using Rac.VOne.Web.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>EBファイル設定</summary>
    public partial class PH1302 : VOneScreenBase
    {
        #region 変数宣言
        public VOneScreenBase ReturnScreen { get; set; }
        public EBFileSetting Format { get; set; }
        public List<EBFormat> EBFileFormats { get; set; }
        /// <summary>タスクスケジューラ ImportType EBデータ取り込み</summary>
        private int ImportType { get { return (int)TaskScheduleImportType.EbData; } }

        #endregion

        #region PH1302 EBファイル設定
        public PH1302()
        {
            InitializeComponent();
            Text = "EBファイル設定";
            AddHandlers();
        }
        #endregion

        #region PH1302 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction10Caption("戻る");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Save;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = Delete;
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region フォームロード
        private void PH1302_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                InitializeHandlers();
                var tasks = new List<Task> {
                    LoadCompanyAsync(),
                    LoadApplicationControlAsync(),
                    LoadControlColorAsync(),
                };
                
                var loadFormatTask = LoadFormatAsync();
                tasks.Add(loadFormatTask);
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
                EBFileFormats = loadFormatTask.Result;

                cmbEBFileFormatId.Items.AddRange(EBFileFormats.Select(x => new ListItem(x.Name, x.Id)).ToArray());

                if (Format == null) return;

                LoadEBFileFormatsAsync();
                BaseContext.SetFunction03Enabled(true);
                cmbEBFileFormatId.SelectedIndex = Format.EBFormatId;
                cmbFileFieldType.SelectedItem = cmbFileFieldType.Items.OfType<ListItem>().FirstOrDefault(x => (int)x.SubItems[1].Value == Format.FileFieldType);
                txtName.Text = Format.Name;
                txtBankCode.Text = Format.BankCode;
                txtImportableValue.Text = Format.ImportableValues;
                txtInitialDirectory.Text = Format.FilePath;
                txtDisplayOrder.Text = Convert.ToString(Format.DisplayOrder);
                rdoUseValue1.Checked = Convert.ToBoolean(Format.UseValueDate);
                ClearAll();
            }
            catch (Exception ex)
            {
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private void InitializeHandlers()
        {
            Load += PH1302_Load;
            cmbEBFileFormatId.SelectedIndexChanged += (sender, e) => LoadEBFileFormatsAsync();
            cmbEBFileFormatId.SelectedIndexChanged += (sender, e) => InitialDelimiterValue();
            cmbEBFileFormatId.SelectedIndexChanged += (sender, e) => InitialFileTypeName();
            cmbFileFieldType.SelectedIndexChanged      += (sender, e) => InitialFileTypeName();
            btnPath.Click += btnPath_Click;
        }
        #endregion
        private async Task<List<EBFormat>> LoadFormatAsync()
            => await ServiceProxyFactory.DoAsync(async (EBFormatMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey);
                if (result.ProcessResult.Result)
                {
                    return result.EBFileFormats;
                }
                return new List<EBFormat>();
            });

        #region コンボボックス設定
        private void InitialDelimiterValue()
        {
            if (cmbEBFileFormatId.SelectedIndex == -1) return;
            cmbFileFieldType.Enabled = true;
            cmbFileFieldType.Items.Clear();
            var format = EBFileFormats.FirstOrDefault(x => x.Id == cmbEBFileFormatId.SelectedIndex);
            var types = (FileFieldTypes)format.FileFieldTypes;
            if (types.HasFlag(FileFieldTypes.CommaDelimited))         cmbFileFieldType.Items.Add(new ListItem("カンマ区切り",     1));
            if (types.HasFlag(FileFieldTypes.TabDelimited))           cmbFileFieldType.Items.Add(new ListItem("タブ区切り",       2));
            if (types.HasFlag(FileFieldTypes.FixedLength))            cmbFileFieldType.Items.Add(new ListItem("固定長",           3));
            if (types.HasFlag(FileFieldTypes.FixedLengthNoLineBreak)) cmbFileFieldType.Items.Add(new ListItem("固定長(改行無し)", 4));
        }
        private void InitialFileTypeName()
        {
            if (cmbEBFileFormatId.SelectedIndex != -1
                && cmbFileFieldType.SelectedIndex != -1)
                txtName.Text = cmbEBFileFormatId.Text + " " + cmbFileFieldType.Text;
        }
        #endregion

        #region 入力項目チェック
        private bool ValidateInput()
        {
            if (!ValidateChildren()) return false;
            if (!cmbEBFileFormatId.ValidateInputted(() => ShowWarningDialog(MsgWngSelectionRequired, lblEBFileFormatId.Text))) return false;
            if (!cmbFileFieldType.ValidateInputted(()  => ShowWarningDialog(MsgWngSelectionRequired, lblFIleFieldType.Text)))  return false;
            if (!txtName.ValidateInputted(()           => ShowWarningDialog(MsgWngInputRequired,     lblName.Text)))           return false;
            if (txtBankCode.Enabled && !txtBankCode.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblBankCode.Text))) return false;
            if (!string.IsNullOrEmpty(txtInitialDirectory.Text) && !System.IO.Directory.Exists(txtInitialDirectory.Text))
            {
                txtInitialDirectory.Focus();
                ShowWarningDialog(MsgWngNotExistFolder);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDisplayOrder.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblDisplayOrder.Text);
                txtDisplayOrder.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region クリア処理
        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            cmbEBFileFormatId.SelectedIndex = -1;
            cmbFileFieldType.SelectedIndex = -1;
            cmbFileFieldType.Items.Clear();
            txtName.Clear();
            txtBankCode.Clear();
            txtBankCode.Enabled = false;
            txtImportableValue.Clear();
            txtImportableValue.Enabled = false;
            txtInitialDirectory.Clear();
            txtDisplayOrder.Clear();
            rdoUseValue0.Checked = true;
            panel1.Visible = false;
            Format = null;
            BaseContext.SetFunction03Enabled(Format != null);
            ClearAll();
        }
        #endregion

        #region 全てクリア処理
        private void ClearAll() => Modified = false;
        #endregion

        #region 戻る処理
        [OperationLog("戻る")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

            BaseForm.Close();
        }
        #endregion

        #region データ取得処理
        private void LoadEBFileFormatsAsync()
        {
            if (cmbEBFileFormatId.SelectedIndex == -1) return;
            var id = (int)cmbEBFileFormatId.SelectedItem.SubItems[1].Value;
            var format = EBFileFormats.FirstOrDefault(x => x.Id == id);
            txtBankCode.Enabled = Convert.ToBoolean(format.RequireBankCode);

            var hasImportableValues = !string.IsNullOrEmpty(format.ImportableValues);
            txtImportableValue.Enabled = hasImportableValues;
            if (!hasImportableValues) txtImportableValue.Clear();

            panel1.Visible = Convert.ToBoolean(format.IsDateSelectable);
            txtImportableValue.Text = format.ImportableValues;
            txtName.Clear();
            txtBankCode.Clear();
            rdoUseValue0.Checked = true;
        }
        #endregion

        #region 登録処理
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                if (!ValidateChildren())
                    return;

                if (!ValidateInput())
                    return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                var setting = GetSettingFromControls();
                var saveTask = SaveAsync(setting);
                ProgressDialog.Start(ParentForm, saveTask, false, SessionKey);

                if(!saveTask.Result)
                {
                    DispStatusMessage(MsgErrSaveError);
                    return;
                }

                ClearAll();
                ParentForm.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                DispStatusMessage(MsgErrSaveError);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private EBFileSetting GetSettingFromControls() => new EBFileSetting {
            Id                  = Format?.Id ?? 0,
            CompanyId           = CompanyId,
            Name                = txtName.Text,
            DisplayOrder        = Convert.ToInt16(txtDisplayOrder.Text),
            IsUseable           = 1,
            EBFormatId          = cmbEBFileFormatId.SelectedIndex,
            FileFieldType       = (int)cmbFileFieldType.SelectedItem.SubItems[1].Value,
            BankCode            = txtBankCode.Text,
            UseValueDate        = Convert.ToInt16(rdoUseValue1.Checked),
            ImportableValues    = txtImportableValue.Text.Trim(','),
            FilePath            = txtInitialDirectory.Text,
            CreateBy            = Login.UserId,
            UpdateBy            = Login.UserId,
        };

        private async Task<bool> SaveAsync(EBFileSetting setting)
            => await ServiceProxyFactory.DoAsync(async (EBFileSettingMasterClient client) => {
                var result = await client.SaveAsync(SessionKey, setting);
                return result?.ProcessResult.Result ?? false;
            });

        #endregion

        #region 削除処理
        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                ClearStatusMessage();
                if (Format == null)
                    return;

                if (!ValidateChildren())
                    return;

                var checkResult = new ExistResult();
                var task = ServiceProxyFactory.DoAsync<TaskScheduleMasterClient>(async client =>
                {
                    checkResult = await client.ExistsAsync(SessionKey, CompanyId, ImportType, Format.Id);
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (checkResult.Exist)
                {
                    ShowWarningDialog(MsgWngDeleteConstraint, "タイムスケジューラー", " 取込パターン");
                    return;
                }

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var deleteTask = DeleteAsync(Format.Id);
                ProgressDialog.Start(ParentForm, deleteTask, false, SessionKey);

                if (!deleteTask.Result)
                {
                    ShowWarningDialog(MsgErrDeleteError);
                    return;
                }

                ClearAll();
                ParentForm.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrDeleteError);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 全入力項目変更イベント処理
        private void AddHandlers()
        {
            foreach (Control control in gbxEBFileFormat.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }

                if (control is Common.Controls.VOneNumberControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }

                if (control is Common.Controls.VOneComboControl)
                {
                    ((Common.Controls.VOneComboControl)control).SelectedIndexChanged +=
                        new EventHandler(OnContentChanged);
                }

                if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged +=
                        new EventHandler(OnContentChanged);
                }

                if (control is RadioButton)
                {
                    ((RadioButton)control).CheckedChanged +=
                        new EventHandler(OnContentChanged);
                }
            }
        }

        private void OnContentChanged(object sender, EventArgs e) => Modified = true;
        #endregion

        private void btnPath_Click(object sender, EventArgs e)
        {
            var path = txtInitialDirectory.Text.Trim();
            var selectedPath = string.Empty;
            var rootBrowserPath = new List<string>();
            if (!LimitAccessFolder ?
                !ShowFolderBrowserDialog(path, out selectedPath) :
                !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out rootBrowserPath, FolderBrowserType.SelectFolder)) return;

            txtInitialDirectory.Text = (!LimitAccessFolder) ? selectedPath : rootBrowserPath?.FirstOrDefault();
        }

        private async Task<bool> DeleteAsync(int settingId)
            => await ServiceProxyFactory.DoAsync(async (EBFileSettingMasterClient client) => {
                var result = await client.DeleteAsync(SessionKey, settingId);
                return result?.ProcessResult.Result ?? false;
            });
    }
}