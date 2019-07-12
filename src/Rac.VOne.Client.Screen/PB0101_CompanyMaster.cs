using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.CompanyMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Screen
{
    /// <summary>会社マスター</summary>
    public partial class PB0101 : VOneScreenBase
    {
        private List<CompanyLogo> CompanyLogos { get; set; }
        private List<CompanyLogo> SaveLogos { get; set; }
        private List<CompanyLogo> DeleteLogos { get; set; }
        private bool Changed { get; set; } = false;

        public PB0101()
        {
            InitializeComponent();
            Text = "会社マスター";
            InitializeHandlers();
        }

        #region 画面の初期化
        private void InitializeHandlers()
        {
            btnLogoSelect.Click += (sender, e) => SetPicture(sender, e);
            btnSquareSelect.Click += (sender, e) => SetPicture(sender, e);
            btnRoundSelect.Click += (sender, e) => SetPicture(sender, e);

            btnLogoClear.Click += (sender, e) => ClearPicture(sender, e);
            btnSquareClear.Click += (sender, e) => ClearPicture(sender, e);
            btnRoundClear.Click += (sender, e) => ClearPicture(sender, e);
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = UpdateCompany;

            BaseContext.SetFunction02Caption("再表示");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ReloadCompany;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction07Caption("取込設定");
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = ImportSettingForCompany;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = CloseCompany;
        }

        private void PB0101_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                SettingAccountType();
                CompanyInitialize();
                SettingFormData();
                txtAccountNumber1.PaddingChar = '0';
                txtAccountNumber2.PaddingChar = '0';
                txtAccountNumber3.PaddingChar = '0';
                SaveLogos = new List<CompanyLogo>();
                DeleteLogos = new List<CompanyLogo>();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SettingAccountType()
        {
            cmbAccountType1.Items.Add("");
            cmbAccountType1.Items.Add("普通預金");
            cmbAccountType1.Items.Add("当座預金");
            cmbAccountType1.Items.Add("貯蓄預金");
            cmbAccountType1.Items.Add("通知預金");

            cmbAccountType2.Items.Add("");
            cmbAccountType2.Items.Add("普通預金");
            cmbAccountType2.Items.Add("当座預金");
            cmbAccountType2.Items.Add("貯蓄預金");
            cmbAccountType2.Items.Add("通知預金");

            cmbAccountType3.Items.Add("");
            cmbAccountType3.Items.Add("普通預金");
            cmbAccountType3.Items.Add("当座預金");
            cmbAccountType3.Items.Add("貯蓄預金");
            cmbAccountType3.Items.Add("通知預金");
        }

        private void CompanyInitialize()
        {
            var loadTask = new List<Task>();
            loadTask.Add(LoadControlColorAsync());
            if (ApplicationControl == null)
                loadTask.Add(LoadApplicationControlAsync());
            if (Company == null)
                loadTask.Add(LoadCompanyAsync());
            loadTask.Add(LoadCompanyLogosAsync());

            ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
            txtCompanyName.Focus();
        }

        private void SetPicture(object sender, EventArgs e)
        {
            var pictureBox = new PictureBox();
            var sizeMode = new PictureBoxSizeMode();

            if (btnLogoSelect.Equals(sender))
            {
                this.ButtonClicked(btnLogoSelect);
                pictureBox = picLogo;
                sizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (btnSquareSelect.Equals(sender))
            {
                this.ButtonClicked(btnSquareSelect);
                pictureBox = picSquareSeal;
                sizeMode = PictureBoxSizeMode.Zoom;
            }
            else if (btnRoundSelect.Equals(sender))
            {
                this.ButtonClicked(btnRoundSelect);
                pictureBox = picRoundSeal;
                sizeMode = PictureBoxSizeMode.Zoom;
            }

            try
            {
                var serverPath = "";
                ProgressDialog.Start(ParentForm, Task.Run(() =>
                {
                    serverPath = Util.GetGeneralSettingServerPathAsync(Login).Result;
                    serverPath = Util.GetDirectoryName(serverPath);
                }), false, SessionKey);

                var fileNames = new List<string>();
                if (!LimitAccessFolder ?
                    !ShowOpenFileDialog(serverPath, out fileNames, filter: "画像ファイル) | *.BMP; *.JPG; *.GIF; *.PNG") :
                    !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out fileNames, FolderBrowserType.SelectFile)) return;

                pictureBox.Load(fileNames?.FirstOrDefault());
                pictureBox.SizeMode = sizeMode;
                Changed = true;
            }
            catch (Exception)
            {
                pictureBox.Image = null;
                pictureBox.ImageLocation = null;
            }

        }

        private void ClearPicture(object sender, EventArgs e)
        {
            var pictureBox = new PictureBox();
            if (btnLogoClear.Equals(sender))
            {
                this.ButtonClicked(btnLogoClear);
                pictureBox = picLogo;
            }
            else if (btnSquareClear.Equals(sender))
            {
                this.ButtonClicked(btnSquareClear);
                pictureBox = picSquareSeal;
            }
            else if (btnRoundClear.Equals(sender))
            {
                this.ButtonClicked(btnRoundClear);
                pictureBox = picRoundSeal;
            }

            pictureBox.Image = null;
            pictureBox.ImageLocation = null;
            Changed = true;
        }

        #endregion

        #region Function Key Event
        [OperationLog("登録")]
        private void UpdateCompany()
        {
            ClearStatusMessage();
            if (!ValidateInputData()) return;

            ZeroLeftPaddingWithoutValidated();

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            try
            {
                var result = false;
                PrepareUpdateData();
                Task task = SaveCompanyInfoAsync()
                    .ContinueWith(async t =>
                    {
                        result = t.Result.ProcessResult.Result;
                        if (result)
                        {
                            PrepareLogos();
                            if (SaveLogos.Any())
                                result = await SaveCompanyLogosAsync();
                            if (result && DeleteLogos.Any())
                                result = await DeleteCompanyLogos();
                            if (result)
                                await LoadCompanyLogosAsync();
                        }
                    },TaskScheduler.FromCurrentSynchronizationContext())
                    .Unwrap();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result)
                {
                    SettingFormData();
                    DispStatusMessage(MsgInfSaveSuccess);
                }
                else
                    ShowWarningDialog(MsgErrSaveError);

            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSaveError);
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("再表示")]
        private void ReloadCompany()
        {
            try
            { 
                ClearStatusMessage();
                if (Changed && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

                var loadTask = new List<Task>();
                loadTask.Add(LoadCompanyAsync());
                loadTask.Add(LoadCompanyLogosAsync());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                SettingFormData();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("取込設定")]
        private void ImportSettingForCompany()
        {
            using (Form form = ApplicationContext.Create(nameof(PB0102)))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.Tag = this.ParentForm;
                DialogResult result = ApplicationContext.ShowDialog(ParentForm, form);
            }
        }

        [OperationLog("終了")]
        private void CloseCompany()
        {
            if (Changed && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            BaseForm.Close();
        }
        #endregion

        #region Webサービス呼び出し

        private async Task LoadCompanyLogosAsync()
            => await ServiceProxyFactory.DoAsync(async (CompanyMasterClient client) =>
            {
                var results = await client.GetLogosAsync(SessionKey, CompanyId);
                CompanyLogos = results.CompanyLogos;
            });

        private async Task<CompanyResult> SaveCompanyInfoAsync()
            => await ServiceProxyFactory.DoAsync(async (CompanyMasterClient client) =>
            {
                var result = await client.SaveAsync(SessionKey, Company);
                if (result.ProcessResult.Result)
                    Company = result.Company;
                return result;
            });

        private async Task<bool> SaveCompanyLogosAsync()
            => await ServiceProxyFactory.DoAsync(async (CompanyMasterClient client) =>
            {
                var result = await client.SaveLogosAsync(SessionKey, SaveLogos.ToArray());
                if (result.ProcessResult.Result)
                    CompanyLogos = result.CompanyLogos;
                return result.ProcessResult.Result;
            });

        private async Task<bool> DeleteCompanyLogos()
            => await ServiceProxyFactory.DoAsync(async (CompanyMasterClient client) =>
            {
                var result = await client.DeleteLogosAsync(SessionKey, DeleteLogos.ToArray());
                return result.Count > 0;
            });

        #endregion

        #region Form Sub Function
        private void SettingFormData()
        {
            txtCompanyCode.Text = Company.Code;
            txtProductKey.Text = Company.ProductKey;
            txtCompanyName.Text = Company.Name;
            txtCompanyNameKana.Text = Company.Kana;
            mskPostalCode.Text = Company.PostalCode;
            txtAddress1.Text = Company.Address1;
            txtAddress2.Text = Company.Address2;
            txtTel.Text = Company.Tel;
            txtFax.Text = Company.Fax;
            txtBankAccountName.Text = Company.BankAccountName;
            txtBankAccountNameKana.Text = Company.BankAccountKana;
            txtBankName1.Text = Company.BankName1;
            txtBankName2.Text = Company.BankName2;
            txtBankName3.Text = Company.BankName3;
            txtBranchName1.Text = Company.BranchName1;
            txtBranchName2.Text = Company.BranchName2;
            txtBranchName3.Text = Company.BranchName3;
            cmbAccountType1.Text = Company.AccountType1 ?? string.Empty;
            cmbAccountType2.Text = Company.AccountType2 ?? string.Empty;
            cmbAccountType3.Text = Company.AccountType3 ?? string.Empty;
            txtAccountNumber1.Text = Company.AccountNumber1;
            txtAccountNumber2.Text = Company.AccountNumber2;
            txtAccountNumber3.Text = Company.AccountNumber3;
            txtClosingDay.Text = Company.ClosingDay.ToString("00");
            cbxPresetCodeSearchDialog.Checked = (Company.PresetCodeSearchDialog == 1);
            cbxShowConfirmDialog.Checked = (Company.ShowConfirmDialog == 1);
            cbxShowWarningDialog.Checked = (Company.ShowWarningDialog == 1);
            cbxTransferAggregate.Checked = (Company.TransferAggregate == 1);
            cbxAutoCloseProgressDialog.Checked = (Company.AutoCloseProgressDialog == 1);
            picLogo.Image = null;
            picLogo.ImageLocation = null;

            if (UseClosing)
            {
                var information = UtilClosing.GetClosingInformation(Login.SessionKey, Login.CompanyId);
                if (information.Closing != null)
                    txtClosingDay.Enabled = false;
            }

            foreach (var companyLogo in CompanyLogos)
            {
                if (companyLogo.Logo.Length <= 0)
                    continue;

                switch (companyLogo.LogoType)
                {
                    case (int)CompanyLogoType.Logo:
                        picLogo.Image = ChangeByteToImage(companyLogo.Logo);
                        picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                        picLogo.Refresh();
                        picLogo.ImageLocation = null;
                        break;
                    case (int)CompanyLogoType.SquareSeal:
                        picSquareSeal.Image = ChangeByteToImage(companyLogo.Logo);
                        picSquareSeal.SizeMode = PictureBoxSizeMode.Zoom;
                        picSquareSeal.Refresh();
                        picSquareSeal.ImageLocation = null;
                        break;
                    case (int)CompanyLogoType.RoundSeal:
                        picRoundSeal.Image = ChangeByteToImage(companyLogo.Logo);
                        picRoundSeal.SizeMode = PictureBoxSizeMode.Zoom;
                        picRoundSeal.Refresh();
                        picRoundSeal.ImageLocation = null;
                        break;
                }
            }

            txtCompanyName.Focus();
            Changed = false;
        }

        private void InputControls_OnChange(object sender, EventArgs e)
        {
            Changed = true;
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(0, txtAccountNumber1.TextLength, txtAccountNumber1.MaxLength))
            {
                txtAccountNumber1.Text = ZeroLeftPadding(txtAccountNumber1);
            }
            if (IsNeedValidate(0, txtAccountNumber2.TextLength, txtAccountNumber2.MaxLength))
            {
                txtAccountNumber2.Text = ZeroLeftPadding(txtAccountNumber2);
            }
            if (IsNeedValidate(0, txtAccountNumber3.TextLength, txtAccountNumber3.MaxLength))
            {
                txtAccountNumber3.Text = ZeroLeftPadding(txtAccountNumber3);
            }
            if (!IsValidClosingDay(txtClosingDay.Text, txtClosingDay.MaxLength))
            {
                txtClosingDay_Validated(null, null);
            }
        }

        private bool ValidateInputData()
        {
            ClearStatusMessage();
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblCompanyName.Text);
                txtCompanyName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCompanyNameKana.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblCompanyNameKana.Text);
                txtCompanyNameKana.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtClosingDay.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblClosingDay.Text);
                txtClosingDay.Focus();
                return false;
            }

            if (mskPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length > 0 &&
                mskPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length != 7)
            {
                ShowWarningDialog(MsgWngInputNeedxxDigits, lblPostalCode.Text, "7桁");
                mskPostalCode.Focus();
                return false;
            }

            return true;
        }

        private void PrepareUpdateData()
        {
            Company.Id = CompanyId;
            Company.Code = txtCompanyCode.Text;
            Company.ProductKey = txtProductKey.Text;
            Company.Name = txtCompanyName.Text.Trim();
            Company.Kana = txtCompanyNameKana.Text.Trim();
            Company.PostalCode = mskPostalCode.Text.Replace("_", string.Empty).Replace("-", string.Empty).Length == 0 ?
                      string.Empty : mskPostalCode.Text;
            Company.Address1 = txtAddress1.Text.Trim();
            Company.Address2 = txtAddress2.Text.Trim();
            Company.Tel = txtTel.Text;
            Company.Fax = txtFax.Text;
            Company.BankAccountName = txtBankAccountName.Text.Trim();
            Company.BankAccountKana = txtBankAccountNameKana.Text.Trim();
            Company.BankName1 = txtBankName1.Text.Trim();
            Company.BranchName1 = txtBranchName1.Text.Trim();
            Company.AccountType1 = cmbAccountType1.Text;
            Company.AccountNumber1 = txtAccountNumber1.Text;
            Company.BankName2 = txtBankName2.Text.Trim();
            Company.BranchName2 = txtBranchName2.Text.Trim();
            Company.AccountType2 = cmbAccountType2.Text ?? string.Empty;
            Company.AccountNumber2 = txtAccountNumber2.Text ?? string.Empty;
            Company.BankName3 = txtBankName3.Text.Trim();
            Company.BranchName3 = txtBranchName3.Text.Trim();
            Company.AccountType3 = cmbAccountType3.Text ?? string.Empty;
            Company.AccountNumber3 = txtAccountNumber3.Text ?? string.Empty;
            var day = 0;
            if (int.TryParse(txtClosingDay.Text, out day))
                Company.ClosingDay = day;
            Company.ShowWarningDialog = cbxShowWarningDialog.Checked ? 1 : 0;
            Company.ShowConfirmDialog = cbxShowConfirmDialog.Checked ? 1 : 0;
            Company.PresetCodeSearchDialog = cbxPresetCodeSearchDialog.Checked ? 1 : 0;
            Company.TransferAggregate = cbxTransferAggregate.Checked ? 1 : 0;
            Company.AutoCloseProgressDialog = cbxAutoCloseProgressDialog.Checked ? 1 : 0;
            Company.UpdateBy = Login.UserId;
            Company.UpdateAt = Company.UpdateAt;
        }

        private void PrepareLogos()
        {
            if (SaveLogos.Any()) SaveLogos.Clear();
            if (DeleteLogos.Any()) DeleteLogos.Clear();

            foreach (int logoType in Enum.GetValues(typeof(CompanyLogoType)))
            {
                var pictureBox = new PictureBox();
                if (logoType == (int)CompanyLogoType.Logo)
                    pictureBox = picLogo;
                else if (logoType == (int)CompanyLogoType.SquareSeal)
                    pictureBox = picSquareSeal;
                else if (logoType == (int)CompanyLogoType.RoundSeal)
                    pictureBox = picRoundSeal;

                if (CompanyLogos != null &&
                    CompanyLogos.Any(x => x.LogoType == logoType) &&
                    pictureBox.Image == null)
                {
                    //削除対象
                    var deleteLogo = CompanyLogos.FirstOrDefault(x => x.LogoType == logoType);
                    DeleteLogos.Add(deleteLogo);
                    continue;
                }

                var hasLogo = true;
                var companyLogo = new CompanyLogo();
                companyLogo.Logo = new byte[0];
                var filePath = pictureBox.ImageLocation;

                if (!string.IsNullOrEmpty(filePath))
                {
                    companyLogo.Logo = ChangeImageToBinary(filePath);
                    companyLogo.UpdateAt = DateTime.Now;
                }
                else if (pictureBox.Image != null)
                {
                    var currentLogo = CompanyLogos.Where(x => x.LogoType == logoType).FirstOrDefault();
                    companyLogo.Logo = currentLogo.Logo;
                    companyLogo.UpdateAt = currentLogo.UpdateAt;
                }
                else
                    hasLogo = false;

                if (hasLogo)
                {
                    companyLogo.CompanyId = Company.Id;
                    companyLogo.CreateBy = Login.UserId;
                    companyLogo.UpdateBy = Login.UserId;
                    companyLogo.LogoType = logoType;
                    SaveLogos.Add(companyLogo);
                }
            }
        }

        private static byte[] ChangeImageToBinary(string imagePath)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                var imgBinary = new byte[stream.Length];
                stream.Read(imgBinary, 0, (int)stream.Length);
                return imgBinary;

            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }
        }

        private static Bitmap ChangeByteToImage(byte[] bimage)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                stream.Write(bimage, 0, bimage.Length);
                var bmPicture = new Bitmap(stream, false);
                return bmPicture;
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }
        }

        private void txtClosingDay_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClosingDay.Text)) return;
            var value = txtClosingDay.Text;
            var day = 0;
            if (int.TryParse(value, out day))
            {
                if (day > 27) day = 99;
                txtClosingDay.Text = day.ToString("00");
            }
        }

        private void txt_keyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                e.Handled = true;
            }
        }
        #endregion
    }
}
