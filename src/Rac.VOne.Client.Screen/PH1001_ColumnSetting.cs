using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using GrapeCity.Win.MultiRow;
using System.Diagnostics;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>項目名称設定</summary>
    public partial class PH1001 : VOneScreenBase
    {
        //変数宣言
        private string ColumnName { get; set; }
        private string TableName { get; set; }
        private string CellName(string value) => $"cel{value}";

        public PH1001()
        {
            InitializeComponent();
            grdColumnName.SetupShortcutKeys();
            Text = "項目名称設定";
            CheckEditControl();
        }

        #region PH1001 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(false);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(false);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region フォームロード
        private void PH1001_Load(object sender, EventArgs e)
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

                Task<List<ColumnNameSetting>> ColumnNameSettingTask = GetColumnNameSettingsListAsync();
                loadTask.Add(ColumnNameSettingTask);

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                InitializeGridTemplate();
                grdColumnName.DataSource = new BindingSource(ColumnNameSettingTask.Result, null);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 項目変更イベント処理
        /// <summary>項目変更イベント処理</summary>
        private void CheckEditControl()
        {
            foreach (Control control in this.GetAll<Common.Controls.VOneTextControl>())
            {
                control.TextChanged += new EventHandler(OnContentChanged);
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }
        #endregion

        #region ColumnNameSetting取得
        private async Task<List<ColumnNameSetting>> GetColumnNameSettingsListAsync()
        {
            List<ColumnNameSetting> list = null;

            await ServiceProxyFactory.DoAsync<ColumnNameSettingMasterClient>(async client =>
            {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    list = result.ColumnNames;
                }
                Modified = false;
            });

            return list ?? new List<ColumnNameSetting>();
        }
        #endregion

        #region 画面の初期化
        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header", cell: builder.GetRowHeaderCell()),
                new CellSetting(height, 100, nameof(ColumnNameSetting.TableNameText), dataField: nameof(ColumnNameSetting.TableNameText), caption: "種別", cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter)),
                new CellSetting(height, 150, nameof(ColumnNameSetting.OriginalName) , dataField: nameof(ColumnNameSetting.OriginalName) , caption: "項目名"),
                new CellSetting(height, 150, nameof(ColumnNameSetting.Alias)        , dataField: nameof(ColumnNameSetting.Alias)        , caption: "変更後名称"),
                new CellSetting(height,   0, nameof(ColumnNameSetting.ColumnName)   , dataField: nameof(ColumnNameSetting.ColumnName), visible: false),
                new CellSetting(height,   0, nameof(ColumnNameSetting.TableName)    , dataField: nameof(ColumnNameSetting.TableName) , visible: false),
            });

            grdColumnName.Template = builder.Build();
            grdColumnName.HideSelection = true;
        }
        #endregion

        #region grdColumnName_CellDoubleClick
        private void grdColumnName_CellDoubleClick(object sender, GrapeCity.Win.MultiRow.CellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                return;

            ClearStatusMessage();

            lblTableName.Text = grdColumnName.Rows[e.RowIndex][CellName(nameof(ColumnNameSetting.TableNameText))].Value.ToString();
            ColumnName = grdColumnName.Rows[e.RowIndex][CellName(nameof(ColumnNameSetting.ColumnName))].Value.ToString();
            lblOriginalName.Text = grdColumnName.Rows[e.RowIndex][CellName(nameof(ColumnNameSetting.OriginalName))].Value.ToString();
            TableName = grdColumnName.Rows[e.RowIndex][CellName(nameof(ColumnNameSetting.TableName))].Value.ToString();

            if (grdColumnName.Rows[e.RowIndex][CellName(nameof(ColumnNameSetting.Alias))].DisplayText.Trim() != "")
                txtAlias.Text = grdColumnName.Rows[e.RowIndex][CellName(nameof(ColumnNameSetting.Alias))].Value.ToString();
            else
                txtAlias.Clear();

            Modified = false;
            txtAlias.Focus();
            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
        }
        #endregion

        #region 画面の F1
        [OperationLog("登録")]
        private void Save()
        {
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                SaveColumnNameSetting();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>登録処理</summary>
        private void SaveColumnNameSetting()
        {
            var columnNameSetting = new ColumnNameSetting();
            columnNameSetting.CompanyId = CompanyId;
            columnNameSetting.TableName = TableName;
            columnNameSetting.ColumnName = ColumnName;

            if (!string.IsNullOrEmpty(txtAlias.Text.Trim()))
                columnNameSetting.Alias = txtAlias.Text.Trim();

            columnNameSetting.UpdateBy = Login.UserId;

            bool success = false;
            List<ColumnNameSetting> list = null;

            var task = ServiceProxyFactory.DoAsync<ColumnNameSettingMasterClient>(async client =>
            {
                var result = await client.SaveAsync(SessionKey, columnNameSetting);
                success = result?.ProcessResult.Result ?? false;
                if (success)
                    list = await GetColumnNameSettingsListAsync();
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (!success)
            {
                ShowWarningDialog(MsgErrSaveError);
            }
            else
            {
                ClearControl();
                DispStatusMessage(MsgInfSaveSuccess);
            }
            grdColumnName.DataSource = new BindingSource(list, null);
            grdColumnName.Focus();
            grdColumnName.CurrentCellPosition = new CellPosition(0, CellName(nameof(ColumnNameSetting.TableNameText)));
        }
        #endregion

        #region 画面のF2
        [OperationLog("クリア")]
        private void Clear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            ClearControl();
        }

        /// <summary>画面クリア</summary>
        private void ClearControl()
        {
            ClearStatusMessage();
            BaseContext.SetFunction01Enabled(false);
            BaseContext.SetFunction02Enabled(false);
            lblTableName.Clear();
            lblOriginalName.Clear();
            txtAlias.Clear();
            grdColumnName.Focus();
            grdColumnName.CurrentCellPosition = new CellPosition(0, CellName(nameof(ColumnNameSetting.TableNameText)));
            Modified = false;
        }
        #endregion

        #region 画面の F10
        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            ParentForm.Close();
        }
        #endregion
    }
}
