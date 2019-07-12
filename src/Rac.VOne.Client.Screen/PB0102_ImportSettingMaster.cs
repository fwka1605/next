using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ImportSettingMasterService;
using Rac.VOne.Import;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>マスターインポート設定</summary>
    public partial class PB0102 : VOneScreenBase
    {

        public PB0102()
        {
            InitializeComponent();
            grdImportSetting.SetupShortcutKeys();

            Text = "マスターインポート設定";
        }

        #region 画面の初期化
        private void InitializeImportlSettingGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;

            var cellImportMode = ImportModeComboBoxCell(builder.GetComboBoxCell());
            var cellErrorLog = ErrorLogDestinationComboCell(builder.GetComboBoxCell());
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 230, "ImportFileName"     , dataField: nameof(ImportSetting.ImportFileName)     , cell: builder.GetTextBoxCell()   , caption: "マスター名"                          ),
                new CellSetting(height, 130, "ImportMode"         , dataField: nameof(ImportSetting.ImportMode)         , cell: cellImportMode             , caption: "取込設定"          , readOnly: false ),
                new CellSetting(height,  80, "ExportErrorLog"     , dataField: nameof(ImportSetting.ExportErrorLog)     , cell: builder.GetCheckBoxCell()  , caption: "エラーログ"        , readOnly: false ),
                new CellSetting(height, 230, "ErrorLogDestination", dataField: nameof(ImportSetting.ErrorLogDestination), cell: cellErrorLog               , caption: "エラーログ出力場所", readOnly: false ),
                new CellSetting(height,  80, "Confirmation"       , dataField: nameof(ImportSetting.Confirm)            , cell: builder.GetCheckBoxCell()  , caption: "取込時確認"        , readOnly: false ),
                new CellSetting(height,   0, "ImportFileType"     , dataField: nameof(ImportSetting.ImportFileType)     , cell: builder.GetTextBoxCell()   , caption: "ImportFileType"    , visible: false  ),
            });
            grdImportSetting.Template = builder.Build();
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = CallConfirmDialog;

            BaseContext.SetFunction02Caption("再表示");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Reload;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region FunctionKey Events
        [OperationLog("登録")]
        private void CallConfirmDialog()
        {
            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {

                Task<bool> task = Save();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                if (task.Result)
                {
                    ClearStatusMessage();

                    DispStatusMessage(MsgInfSaveSuccess);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<bool> Save()
        {
            var a = grdImportSetting.DataSource as BindingSource;
            var list = a.DataSource as List<ImportSetting>;
            int count = grdImportSetting.Rows.Count;
            var importSettingTemp = new ImportSetting[count];
            ImportSettingResult result = null;

            for (int i = 0; i < count; i++)
            {
                var importsetting = new ImportSetting();
                importsetting.CompanyId = CompanyId;

                importsetting.ImportFileName = list[i].ImportFileName;
                importsetting.ImportMode = list[i].ImportMode;
                importsetting.ExportErrorLog = list[i].ExportErrorLog;
                importsetting.ErrorLogDestination = list[i].ErrorLogDestination;
                importsetting.Confirm = list[i].Confirm;
                importsetting.ImportFileType = list[i].ImportFileType;
                importsetting.CreateAt = list[i].CreateAt;
                list[i].CreateBy = Login.UserId;
                importsetting.CreateBy = list[i].CreateBy;
                list[i].UpdateBy = Login.UserId;
                importsetting.UpdateBy = list[i].UpdateBy;
                importsetting.UpdateAt = list[i].UpdateAt;
                importSettingTemp[i] = importsetting;
            }

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ImportSettingMasterClient>();
                result = await service.SaveAsync(SessionKey, importSettingTemp);
                if (result.ProcessResult.Result)
                {
                    await LoadImportingSettingGrid();
                }
            });
            return result.ProcessResult.Result;
        }

        [OperationLog("再表示")]
        private void Reload()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                ProgressDialog.Start(ParentForm, LoadImportingSettingGrid(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("戻る")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
            {
                return;
            }
            Modified = false;
            ParentForm.Close();
        }
        #endregion

        #region その他Function
        private void PB0102_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                InitializeImportlSettingGrid();

                var loadTask = new List<Task>();
                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                loadTask.Add(LoadControlColorAsync());

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                loadTask.Add(LoadImportingSettingGrid());

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task LoadImportingSettingGrid()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ImportSettingMasterClient>();
                ImportSettingResults result = await service.GetItemsAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                {
                    var setting = result.ImportSettings;
                    if (setting != null)
                        setting = FilterOptions(setting);

                    if (LimitAccessFolder) setting.ForEach(x => x.ErrorLogDestination = 1);

                    grdImportSetting.DataSource = new BindingSource(setting, null);
                    ComboBoxDisable(setting);//for combobox disable
                    Modified = false;
                }
            });
        }

        private List<ImportSetting> FilterOptions(List<ImportSetting> setting)
            => setting
            .Where(x => UseForeignCurrency  || !(x.ImportFileType == ImportFileType.Currency))
            .Where(x => UseSection          || !(x.ImportFileType == ImportFileType.Section
                                            ||   x.ImportFileType == ImportFileType.SectionWithDepartment
                                            ||   x.ImportFileType == ImportFileType.SectionWithLoginUser))
            .Where(x => UseScheduledPayment || !(x.ImportFileType == ImportFileType.BillingDivisionContract))
            .ToList();

        private void ComboBoxDisable(List<ImportSetting> importSettings)
        {
            int i = 0;
            foreach (var item in importSettings)
            {
                if (item.ExportErrorLog == 0)
                {
                    grdImportSetting.Rows[i].Cells[3].Enabled = false;
                    var comboBox = grdImportSetting.Rows[i].Cells[3] as ComboBoxCell;
                    var errorlog = GetErrorLogSource(isblank: true);
                    comboBox.DataSource = new BindingSource(errorlog, null);
                }
                else
                {
                    grdImportSetting.Rows[i].Cells[3].Enabled = !LimitAccessFolder;
                }
                i++;
            }
        }


        private Cell ImportModeComboBoxCell(ComboBoxCell importmodecomboBoxCell)
        {
            var importMode = new Dictionary<string, string>
            {
                { "0", "上書" },
                { "1", "追加" },
                { "2", "更新" },
            };
            importmodecomboBoxCell.DataSource = new BindingSource(importMode, null);
            importmodecomboBoxCell.DisplayMember = "Value";
            importmodecomboBoxCell.ValueMember = "Key";

            return importmodecomboBoxCell;
        }

        private Dictionary<string, string> GetErrorLogSource(bool isblank = false)
            => new Dictionary<string, string>
            {
                {"0", isblank ? "" : "ユーザーフォルダー" },
                {"1", isblank ? "" : "取込ファイルと同一フォルダー" },
            };

        private Cell ErrorLogDestinationComboCell(ComboBoxCell errorlogdestinationcomboBoxCell)
        {
            errorlogdestinationcomboBoxCell.DataSource = new BindingSource(GetErrorLogSource(), null);
            errorlogdestinationcomboBoxCell.DisplayMember = "Value";
            errorlogdestinationcomboBoxCell.ValueMember = "Key";

            return errorlogdestinationcomboBoxCell;

        }

        private void grid_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            grdImportSetting.EndEdit();
            Modified = true;
        }

        private void grid_CellValueChanged(object sender, CellEventArgs e)
        {
            if (e.Scope != CellScope.Row
                || e.CellIndex != 2) return;

            var checkBox = grdImportSetting.CurrentCell as CheckBoxCell;
            var comboBox = grdImportSetting.Rows[e.RowIndex][e.CellIndex + 1] as ComboBoxCell;
            var check = Convert.ToBoolean(checkBox.EditedFormattedValue);
            comboBox.Enabled = (LimitAccessFolder) ? false : check;
            var temp_errorlog = GetErrorLogSource(!check);
            comboBox.DataSource = new BindingSource(temp_errorlog, null);
        }

        #endregion
    }
}

