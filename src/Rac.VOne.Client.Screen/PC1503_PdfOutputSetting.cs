using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Rac.VOne.Client.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.PdfOutputSettingMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using System.IO;

using static Rac.VOne.Message.Constants;


namespace Rac.VOne.Client.Screen
{
    /// <summary>
    /// PDFファイル出力設定
    /// </summary>
    public partial class PC1503 : VOneScreenBase
    {
        #region 初期化
        public VOneScreenBase ReturnScreen { get; set; }
        public PC1503()
        {
            InitializeComponent();
            Text = "PDFファイル出力設定";
            InitializeHandlers();
        }
        private void InitializeHandlers()
        {
            nmbMaxSize.Leave += nmbMaxSize_Leave;
            txtFileName.TextChanged += txtFileName_TextChanged;
            cbxUseCompression.CheckedChanged += cbxUseCompression_CheckedChanged;
            rdoByReport.CheckedChanged += rdoByReport_CheckedChanged;
        }

        private void PC1503_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeFunctionKeys();
                AddHandlers();
                SetScreenName();

                var tasks = new List<Task>();
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());
                tasks.Add(LoadControlColorAsync());
                if (Company == null)
                    tasks.Add(LoadCompanyAsync());

                var taskGetSetting = GetPdfOutputSetting();
                tasks.Add(taskGetSetting);

                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks.ToArray()), false, SessionKey);

                SetOdfOutputSetting(taskGetSetting.Result);
                rdoAllInOne.Focus();
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

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("再読込");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Reloading;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }
        private void SetOdfOutputSetting(PdfOutputSetting setting)
        {
            if (setting.IsAllInOne)
            {
                SetAllInOne();
                Modified = false;
                rdoAllInOne.Select();
                return;
            }

            rdoByReport.Checked = true;
            txtFileName.Text = setting.FileName;
            cbxUseCompression.Checked = setting.UseZip;
            nmbMaxSize.Value = setting.MaximumSize;
            Modified = false;
            rdoByReport.Select();
        }

        private void SetAllInOne()
        {
            rdoAllInOne.Checked = true;
            gbFileNameSetting.Enabled = false;
            gbCompression.Enabled = false;
            txtFileName.Clear();
            cbxUseCompression.Checked = false;
            nmbMaxSize.Clear();
        }
        private void AddHandlers()
        {
            rdoAllInOne.CheckedChanged += new EventHandler(OnContentChanged);
            rdoByReport.CheckedChanged += new EventHandler(OnContentChanged);
            txtFileName.TextChanged += new EventHandler(OnContentChanged);
            cbxUseCompression.CheckedChanged += new EventHandler(OnContentChanged);
            nmbMaxSize.TextChanged += new EventHandler(OnContentChanged);
        }
        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified)
            {
                Modified = true;
            }
        }
        #endregion

        #region ファンクションキーイベント
        #region F01/登録
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                ClearStatusMessage();
                if (!ValidateForSave()) return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var task = SavePdfOutputSetting(GetInputedValues());
                var result = ProgressDialog.Start(ParentForm, task, false, SessionKey);

                SetOdfOutputSetting(task.Result);
                DispStatusMessage(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSaveError);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        private PdfOutputSetting GetInputedValues()
        {
            var setting = new PdfOutputSetting {
                CompanyId = Login.CompanyId,
                CreateBy = Login.UserId,
                UpdateBy = Login.UserId,
            };

            if (ReturnScreen is PC1501)
            {
                setting.ReportType = (int)PdfOutputSettingReportType.Invoice;
            }
            else if (ReturnScreen is PI0401)
            {
                setting.ReportType = (int)PdfOutputSettingReportType.Reminder;
            }

            if (rdoAllInOne.Checked)
            {
                setting.OutputUnit = (int)PdfOutputSettingOutputUnit.AllInOne;
                return setting;
            }
            setting.OutputUnit = (int)PdfOutputSettingOutputUnit.ByReport;
            setting.FileName = txtFileName.Text.Trim();
            setting.UseCompression = cbxUseCompression.Checked ? 1 : 0;
            setting.MaximumSize = nmbMaxSize.Value;
            return setting;
        }
        private bool ValidateForSave()
        {
            if (rdoAllInOne.Checked) return true;
            if (string.IsNullOrEmpty(txtFileName.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblFileName.Text);
                txtFileName.Focus();
                return false;
            }
            if (nmbMaxSize.Value.HasValue && nmbMaxSize.Value.Value == 0)
            {
                ShowWarningDialog(MsgWngInputRequired, lblMaxSize.Text);
                nmbMaxSize.Focus();
                return false;
            }

            return true;
        }
        #endregion

        #region F02/再読込
        [OperationLog("再読込")]
        private void Reloading()
        {
            try
            {
                ClearStatusMessage();
                if (Modified && !ShowConfirmDialog(MsgQstConfirmReloadData))
                    return;
                var task = GetPdfOutputSetting();
                var result = ProgressDialog.Start(ParentForm, task, false, SessionKey);
                SetOdfOutputSetting(task.Result);
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "再読込");
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region F10/終了
        [OperationLog("終了")]
        public void Exit()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            BaseForm.Close();
        }
        #endregion
        #endregion

        #region WebService
        private async Task<PdfOutputSetting> GetPdfOutputSetting() =>
        await ServiceProxyFactory.DoAsync(async (PdfOutputSettingMasterClient client) =>
            {
                int reportType = 0;
                if (ReturnScreen is PC1501)
                {
                    reportType = (int)PdfOutputSettingReportType.Invoice;
                }
                else if (ReturnScreen is PI0401)
                {
                    reportType = (int)PdfOutputSettingReportType.Reminder;
                }
                var result = await client.GetAsync(
                    SessionKey,
                    Login.CompanyId,
                    reportType,
                    Login.UserId);

                if (result == null || result.ProcessResult == null || !result.ProcessResult.Result)
                    return null;
                return result.PdfOutputSetting;
            });

        private async Task<PdfOutputSetting> SavePdfOutputSetting(PdfOutputSetting setting) =>
             await ServiceProxyFactory.DoAsync(async (PdfOutputSettingMasterClient client) =>
             {
                 var result = await client.SaveAsync(SessionKey, setting);
                 if (result == null || result.ProcessResult == null || !result.ProcessResult.Result)
                     return null;
                 return result.PdfOutputSetting;
             });
        #endregion

        #region イベントハンドラー
        private void rdoByReport_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoByReport.Checked)
            {
                gbFileNameSetting.Enabled = true;
                gbCompression.Enabled = true;
                if (cbxUseCompression.Checked)
                {
                    nmbMaxSize.Enabled = true;
                }
                else
                {
                    nmbMaxSize.Enabled = false;
                    nmbMaxSize.Clear();
                }
            }
            else {
                SetAllInOne();
            }
        }

        private void cbxUseCompression_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxUseCompression.Checked)
            {
                nmbMaxSize.Enabled = true;
            }
            else
            {
                nmbMaxSize.Enabled = false;
                nmbMaxSize.Clear();
            }
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            txtFileName.Text = string.Concat(txtFileName.Text.Where(c => !invalidChars.Contains(c)));
        }
        private void nmbMaxSize_Leave(object sender, EventArgs e)
        {
            if (nmbMaxSize.Value.HasValue
               && nmbMaxSize.Value.Value == 0M)
            {
                nmbMaxSize.Clear();
            }
        }
        #endregion
    }
}
