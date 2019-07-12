using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.AccountTitleMasterService;
using Rac.VOne.Client.Screen.DepartmentMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>管理マスター</summary>
    public partial class PB1401 : VOneScreenBase
    {
        private bool DblCellClickFlg { get; set; }
        private List<GeneralSetting> GeneralSettingList { get; set; }
        private GeneralSetting CurrentGeneralSetting { get; set; }

        public PB1401()
        {
            InitializeComponent();
            grdGeneralSetting.SetupShortcutKeys();

            Text = "管理マスター";
            GeneralSettingList = new List<GeneralSetting>();
        }

        #region Initilize
        private void InitializeGeneralSettingGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                      , cell: builder.GetRowHeaderCell()                         ),
                new CellSetting(height, 200, "Code"         , dataField: nameof(GeneralSetting.Code)       , cell: builder.GetTextBoxCell()  , caption: "管理コード" ),
                new CellSetting(height, 200, "Value"        , dataField: nameof(GeneralSetting.Value)      , cell: builder.GetTextBoxCell()  , caption: "データ"     ),
                new CellSetting(height, 100, "Length"       , dataField: nameof(GeneralSetting.Length)     , cell: builder.GetNumberCell()   , caption: "有効桁数"   ),
                new CellSetting(height, 200, "Description"  , dataField: nameof(GeneralSetting.Description), cell: builder.GetTextBoxCell()  , caption: "説明"       ),
                new CellSetting(height,   0, "Id"           , dataField: nameof(GeneralSetting.Id)         , cell: builder.GetTextBoxCell()  , caption: "Id"         , visible: false )
            });

            grdGeneralSetting.Template = builder.Build();
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(false);
            OnF01ClickHandler = SaveData;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(false);
            OnF02ClickHandler = ClearData;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = PrintData;

            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction09Caption("");

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = ExitForm;
        }

        [OperationLog("登録")]
        private void SaveData()
        {
            try
            {
                if (!ValidateChildren()) return;

                if (CheckData() && ValidateInputValue())
                {
                    if (!ShowConfirmDialog(MsgQstConfirmSave))
                    {
                        DispStatusMessage(MsgInfProcessCanceled);
                        return;
                    }
                    SaveGeneralSetting();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void ClearData()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            ClearGeneralSetting();
        }

        [OperationLog("印刷")]
        private void PrintData()
        {
            try
            {
                DoPrint();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("終了")]
        private void ExitForm()
        {
            if (Modified)
            {
                if (ShowConfirmDialog(MsgQstConfirmClose))
                {
                    Modified = false;
                    BaseForm.Close();
                }
            }
            else
            {
                Modified = false;
                BaseForm.Close();
            }
        }

        #endregion

        #region WEB Service

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

        private void DoPrint()
        {
            try
            {
                string serverPath = null;
                var generalSettingReport = new GeneralSettingSectionReport();
                List<GeneralSetting> list = null;

                ProgressDialog.Start(ParentForm, Task.Run(async () =>
                {
                    serverPath = await GetServerPath();
                    list = await GetGeneralSettingListAsync();

                    if (list.Any())
                    {
                        generalSettingReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                        generalSettingReport.Name = "管理マスター" + DateTime.Now.ToString("yyyyMMdd");
                        generalSettingReport.SetData(list);
                        generalSettingReport.Run(false);
                    }
                }), false, SessionKey);

                if (list.Any())
                {
                    ShowDialogPreview(ParentForm, generalSettingReport, serverPath);
                }
                else
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private void SaveGeneralSetting()
        {
            GeneralSettingResult result = null;
            GeneralSetting GeneralSetting = GeneralSettingInfo();

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                result = await service.SaveAsync(SessionKey, GeneralSetting);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                // 更新
                Model.CopyTo(GeneralSetting, CurrentGeneralSetting, true);
                grdGeneralSetting.DataSource = new BindingSource(GeneralSettingList, null);
                ClearGeneralSetting();
                DispStatusMessage(MsgInfSaveSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrSaveError);
            }
        }

        private async Task<List<GeneralSetting>> GetGeneralSettingListAsync()
        {
            List<GeneralSetting> list = null;
            GeneralSettingsResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                result = await service.GetItemsAsync(SessionKey, CompanyId);
            });

            if (result.ProcessResult.Result)
            {
                list = new List<GeneralSetting>(result.GeneralSettings
                .Where(x =>
                {
                    if (x.Code == "手数料誤差") return !UseForeignCurrency;
                    return true;
                }));
            }

            return list ?? new List<GeneralSetting>();
        }

        #endregion

        #region その他event

        private void PB1401_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                this.ActiveControl = txtValue;
                txtValue.Select();

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

                ProgressDialog.Start(ParentForm,
                    Task.WhenAll(loadTask).ContinueWith(async t =>
                    {
                        GeneralSettingList = await GetGeneralSettingListAsync();
                    },TaskScheduler.FromCurrentSynchronizationContext()).Unwrap(), false, SessionKey);

                InitializeGeneralSettingGrid();
                grdGeneralSetting.Rows.Clear();
                grdGeneralSetting.DataSource = new BindingSource(GeneralSettingList, null);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ClearGeneralSetting()
        {
            ClearStatusMessage();
            BaseContext.SetFunction01Enabled(false);
            BaseContext.SetFunction02Enabled(false);

            lblgeneralsettingId.Text = "";
            txtValue.Clear();
            txtValue.MaxLength = 0;
            txtDescription.Clear();
            txtLength.Clear();
            lblCode.Text = "";
            txtValue.Select();
            Modified = false;
        }

        private bool CheckData()
        {
            if (CheckUnRequiredValue()) return true;

            if (!string.IsNullOrEmpty(txtValue.Text.Trim())) return true;

            ShowWarningDialog(MsgWngInputRequired, "データ");
            txtValue.Select();
            return false;
        }

        private bool CheckUnRequiredValue()
        {
            var UnValidationCode = new string[] { "仮受科目コード", "仮受部門コード" , "仮受補助コード",
                "借方消費税誤差科目コード", "借方消費税誤差部門コード" , "借方消費税誤差補助コード","振込手数料科目コード", "振込手数料部門コード" ,
                "振込手数料補助コード", "貸方消費税誤差科目コード", "貸方消費税誤差部門コード", "貸方消費税誤差補助コード", "長期前受金科目コード",
                "長期前受金部門コード", "長期前受金補助コード", "入金部門コード"};

            return (UnValidationCode.Contains(lblCode.Text));
        }

        private void grdGeneralSetting_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
               if (Modified == false || (Modified && ShowConfirmDialog(MsgQstConfirmUpdateData)))
               {
                    ClearStatusMessage();
                    BaseContext.SetFunction01Enabled(true);
                    BaseContext.SetFunction02Enabled(true);
                    DblCellClickFlg = true;
                    SetGeneralEntry(GeneralSettingList[e.RowIndex]);
                    txtValue.Select();
                }
            }
        }

        private void SetGeneralEntry(GeneralSetting general = null)
        {
            CurrentGeneralSetting = general ?? new GeneralSetting();
            lblgeneralsettingId.Text = general.Id.ToString();
            lblCode.Text = general.Code.ToString();
            txtValue.MaxLength = general.Length;
            txtValue.Text = general.Value.ToString();
            txtLength.Text = general.Length.ToString();
            txtDescription.Text = general.Description.ToString();
            Modified = false;

            txtValue.Required = !CheckUnRequiredValue();

        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            if (DblCellClickFlg)
            {
                Modified = false;
                DblCellClickFlg = false;
            }
            else
            {
                Modified = true;
            }
       }

        #endregion

        private GeneralSetting GeneralSettingInfo()
        {
            var generalsetting = new GeneralSetting();
            Model.CopyTo(CurrentGeneralSetting, generalsetting, true);

            generalsetting.Id = Convert.ToInt32(lblgeneralsettingId.Text);
            generalsetting.Value = txtValue.Text.Trim();
            generalsetting.UpdateBy = Login.UserId;
            generalsetting.CompanyId = Login.CompanyId;
            generalsetting.Description = txtDescription.Text;
            generalsetting.Code = lblCode.Text;

            return generalsetting;
        }

        #region 検証処理

        private bool ValidateLength()
        {
            if (txtValue.Text.Length <= int.Parse(txtLength.Text)) return true;

            ShowWarningDialog(MsgWngScaleViolation);
            txtValue.Focus();
            return false;
        }

        private bool ValidateRange(int start, int end)
        {
            var inputValue = int.Parse(txtValue.Text);
            if (inputValue < start || inputValue > end)
            {
                ShowWarningDialog(MsgWngInputRangeViolation, lblCode.Text, $"{start}～{end}");
                txtValue.Focus();
                return false;
            }
            return true;
        }

        private bool ValidateDigit(bool rangeFlg = false, int start = 0, int end = 0)
        {
            var digitPermission = "^[0-9]*$";

            if (Regex.IsMatch(txtValue.Text, digitPermission)) return true;

            if (rangeFlg)
            {
                ShowWarningDialog(MsgWngInputRangeViolation, lblCode.Text, $"{start}～{end}");
                txtValue.Focus();
                return false;
            }

            ShowWarningDialog(MsgWngInputRequired, "数字");
            txtValue.Focus();
            return false;
        }

        private bool ValidateDate()
        {
            DateTime Result;
            if (DateTime.TryParseExact(txtValue.Text, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out Result)) return true;

            ShowWarningDialog(MsgWngInputRequired, "有効な日付");
            txtValue.Focus();
            return false;
        }

        private bool ValidateAccessiblePath()
        {
            if (txtValue.Text.ToUpper().Contains(ApplicationControl.RootPath.ToUpper())) return true;
            ShowWarningDialog(MsgErrUnauthorizedAccessTargetFolder);
            txtValue.Focus();
            return false;
        }

        private bool ValidateServerPath()
        {
            if (Directory.Exists(txtValue.Text)) return true;
            ShowWarningDialog(MsgWngNotExistFolder);
            txtValue.Focus();
            return false;
        }

        private bool ExistAccountTitleCode()
        {
            AccountTitle accountTitleResult = null;
            AccountTitlesResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<AccountTitleMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtValue.Text });
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                accountTitleResult = result.AccountTitles.FirstOrDefault();
            }

            if (accountTitleResult != null) return true;

            ShowWarningDialog(MsgWngMasterNotExist, "科目", txtValue.Text);
            txtValue.Focus();
            return false;
        }

        private bool ExistDepartmentCode()
        {
            Department departmentResult = null;
            DepartmentsResult result = null;

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<DepartmentMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtValue.Text });
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                departmentResult = result.Departments.FirstOrDefault();
            }

            if (departmentResult != null) return true;

            ShowWarningDialog(MsgWngMasterNotExist, "請求部門", txtValue.Text);
            txtValue.Focus();
            return false;
        }


        //検証処理
        private bool ValidateInputValue()
        {
            var code = lblCode.Text;

            if (code == "サーバパス")
            {
                if (LimitAccessFolder && !ValidateAccessiblePath()) return false;
                if (!ValidateServerPath()) return false;
                if (!ValidateLength()) return false;
            }
            if (IsAccountTitleCode(code) && !string.IsNullOrEmpty(txtValue.Text))
            {
                if (!ValidateLength()) return false;
                if (!ExistAccountTitleCode()) return false;
            }
            if (IsDepartmentCode(code) && !string.IsNullOrEmpty(txtValue.Text))
            {
                if (!ValidateLength()) return false;
                if (!ExistDepartmentCode()) return false;
            } 
            if (IsSubCode(code) && !string.IsNullOrEmpty(txtValue.Text))
            {
                if (!ValidateLength()) return false;
            }
            if (code == "回収予定範囲")
            {
                if (!ValidateLength()) return false;
                if (!ValidateDigit(true, 0, 99)) return false;
                if (!ValidateRange(0, 99)) return false;
            }
            if (code == "金額計算端数処理")
            {
                if (!ValidateLength()) return false;
                if (!ValidateDigit(true, 0, 2)) return false;
                if (!ValidateRange(0, 2)) return false;
            }
            if (code == "取込時端数処理")
            {
                if (!ValidateLength()) return false;
                if (!ValidateDigit(true, 0, 3)) return false;
                if (!ValidateRange(0, 3)) return false;
            }
            if (code == "消費税計算端数処理")
            {
                if (!ValidateLength()) return false;
                if (!ValidateDigit(true, 0, 2)) return false;
                if (!ValidateRange(0, 2)) return false;
            }
            if (code == "請求入力明細件数")
            {
                if (!ValidateLength()) return false;
                if (!ValidateDigit(true, 1, 99)) return false;
                if (!ValidateRange(1, 99)) return false;
            }
            if (code == "旧消費税率" || code == "新消費税率" 
                || code == "新消費税率2" || code == "新消費税率3"
                || code == "請求データ検索開始月範囲" || code == "手数料誤差" || code == "消費税誤差")
            {
                if (!ValidateLength()) return false;
                if (!ValidateDigit()) return false;
            }
            if (code == "新税率開始年月日" || code == "新税率開始年月日2" 
                || code == "新税率開始年月日3")
            {
                if (!ValidateLength()) return false;
                if (!ValidateDate()) return false;
            }
            return true;
        }

        private bool IsAccountTitleCode(string code)
            => (code == "仮受科目コード"
             || code == "借方消費税誤差科目コード"
             || code == "振込手数料科目コード"
             || code == "貸方消費税誤差科目コード"
             || code == "長期前受金科目コード");

        private bool IsDepartmentCode(string code)
            => (code == "仮受部門コード"
             || code == "借方消費税誤差部門コード"
             || code == "振込手数料部門コード"
             || code == "貸方消費税誤差部門コード"
             || code == "長期前受金部門コード"
             || code == "入金部門コード");
        private bool IsSubCode(string code)
            => (code == "仮受補助コード"
             || code == "借方消費税誤差補助コード"
             || code == "振込手数料補助コード"
             || code == "貸方消費税誤差補助コード"
             || code == "長期前受金補助コード"
             || code == "入金区分前受コード");

        #endregion
    }
}
