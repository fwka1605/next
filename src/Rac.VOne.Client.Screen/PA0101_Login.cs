using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.Extensions;
using Rac.VOne.Message;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>ログイン画面</summary>
    public partial class PA0101 : Form
    {
        /// <summary>
        /// アプリケーション共有データ
        /// </summary>
        private readonly IApplication ApplicationContext;

        #region 初期化

        private PA0101()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        public PA0101(IApplication applicationContext) : this()
        {
            Debug.Assert(applicationContext != null);
            Debug.Assert(applicationContext.Login != null);

            ApplicationContext = applicationContext;
        }

        private bool IsCloudEdition { get; set; }

        private void InitializeUserComponent()
        {
            IsCloudEdition = Convert.ToBoolean(ConfigurationManager.AppSettings["IsCloudEdition"]);
            Icon = (IsCloudEdition) ? Properties.Resources.cloud_icon : Properties.Resources.app_icon;
            picLogo.Image = (IsCloudEdition) ? Properties.Resources.cloud_logo : Properties.Resources.logo1;
            if (IsCloudEdition)
            {
                Bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            }
            Text = IsCloudEdition ? CloudApplicationName : StandardApplicationName;
            btnMinimizeWindow.Visible = !IsCloudEdition;

            pnlMain.MouseDown += PA0101_MouseDown;
            pnlMain.MouseClick += (sender, e) =>
            {
                if (txtCompanyName.Bounds.Contains(e.Location)) txtCompanyCode.Focus();
                if (txtUserName.Bounds.Contains(e.Location)) txtUserCode.Focus();
            };
        }

        private void PA0101_Load(object sender, EventArgs e)
        {
            ShowApplicationVersion();

            txtCompanyCode.Format = DataExpression.CompanyCodeTypeGlobal == 0 ? "9" : "9A";

            ApplyLastLoginInfo();
            if (InputCompany != null && InputUser != null)
            {
                txtPassword.Select();
            }
            else
            {
                txtCompanyCode.Select();
            }
        }

        /// <summary>
        /// バージョン番号を画面表示する。
        /// </summary>
        private void ShowApplicationVersion()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;

            lblAssemblyVersion.Text = $"Ver {version}";
        }

        /// <summary>
        /// 前回ログイン情報がある場合は読み込んで画面に適用する。
        /// </summary>
        private void ApplyLastLoginInfo()
        {
            var login = new DummyLogin();

            var lastCompanyCode = Settings.RestoreControlValue<PA0101>(login, txtCompanyCode.Name);
            if (!string.IsNullOrEmpty(lastCompanyCode))
            {
                txtCompanyCode.Text = lastCompanyCode;
                ValidateCompanyCode();
            }

            var lastUserCode = Settings.RestoreControlValue<PA0101>(login, txtUserCode.Name);
            if (!string.IsNullOrEmpty(lastUserCode))
            {
                txtUserCode.Text = lastUserCode;
                ValidateUserCode();
            }
        }

        /// <summary>
        /// Settingsクラスを使用するための固定値がセットされたILoginダミー実装。
        /// </summary>
        /// <remarks>
        /// ログイン前なので固定値を使用する。
        /// </remarks>
        private class DummyLogin : ILogin
        {
            int ILogin.CompanyId { get; set; } = 1357924680;
            string ILogin.CompanyCode { get; set; } = "LoginForm";
            string ILogin.CompanyName { get; set; } = "dummy company for login form";
            int ILogin.UserId { get; set; } = 1357924680;
            string ILogin.UserCode { get; set; } = "LoginForm";
            string ILogin.UserName { get; set; } = "dummy user for login form";
            string ILogin.SessionKey { get; set; }
        }

        #endregion 初期化

        #region Function Keys

        /// <summary>
        /// ファンクションキー押下イベントからの処理分岐
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F1:  OnFunctionKey01Click(); return true;
                case Keys.F7:  OnFunctionKey07Click(); return true;
                case Keys.F10: OnFunctionKey10Click(); return true;
            }
            return false;
        }
        private void OnFunctionKey01Click() => Login();
        private void OnFunctionKey07Click() => ShowChangePassword();
        private void OnFunctionKey10Click() => ExitApplication();

        #endregion Function Keys

        #region 画面操作関連イベントハンドラ

        private void btnMinimizeWindow_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 「ログイン」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        /// <summary>
        /// 「パスワード変更」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ShowChangePassword();
        }

        /// <summary>
        /// 「終了」ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExitApplication_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        /// <summary>
        /// 会社コード検索ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchCompany_Click(object sender, EventArgs e)
        {
            ShowCompanySearchDialog();
        }

        /// <summary>
        /// 担当者コード検索ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchUser_Click(object sender, EventArgs e)
        {
            ShowLoginUserSearchDialog();
        }

        /// <summary>
        /// 会社コード入力値検証
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCompanyCode_Validated(object sender, EventArgs e)
        {
            ValidateCompanyCode();

            // ユーザーコードが入力されている場合はそちらも再取得を試みる
            if (InputCompany != null && !string.IsNullOrEmpty(txtUserCode.Text))
            {
                ValidateUserCode();
            }
        }

        /// <summary>
        /// 担当者コード入力値検証
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserCode_Validated(object sender, EventArgs e)
        {
            ValidateUserCode();
        }

        #region フォーム直上のどの位置をクリックしてもフォームをドラッグ＆ドロップできるようにする

        // ロゴ画像を含むコントロール上では効かない

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();

        private void DisguiseFormClickToTitleBarClick()
            => SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);

        private void PA0101_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (txtUserName.Bounds.Contains(e.Location)) return;
            if (txtCompanyName.Bounds.Contains(e.Location)) return;

            ReleaseCapture();
            DisguiseFormClickToTitleBarClick();
        }

        private void btnSearchUser_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).BackgroundImage = Properties.Resources.icon_search_on;
        }

        private void btnSearchUser_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackgroundImage = Properties.Resources.icon_search;
        }

        private void btnSearchUser_MouseDown(object sender, MouseEventArgs e)
        {
            ((Button)sender).BackgroundImage = Properties.Resources.icon_search_on;
        }

        #endregion フォーム直上のどの位置をクリックしてもフォームをドラッグ＆ドロップできるようにする

        #endregion 画面操作関連イベントハンドラ

        private Company inputCompany;
        /// <summary>
        /// 画面に現在入力されている会社
        /// 無効時はnull。
        /// </summary>
        private Company InputCompany
        {
            get
            {
                return inputCompany;
            }
            set
            {
                if (value == null)
                {
                    ApplicationContext.Login.CompanyId = 0;
                    ApplicationContext.Login.CompanyCode = null;
                    ApplicationContext.Login.CompanyName = null;
                    txtCompanyName.Text = "";
                    InputUser = null; // 会社コードが無効となった場合はユーザーも無効化
                }
                else
                {
                    ApplicationContext.Login.CompanyId = value.Id;
                    ApplicationContext.Login.CompanyCode = value.Code;
                    ApplicationContext.Login.CompanyName = value.Name;
                    txtCompanyCode.Text = value.Code;
                    txtCompanyName.Text = value.Name;
                }

                inputCompany = value;
            }
        }

        private LoginUser inputUser;
        /// <summary>
        /// 画面に現在入力されているログインユーザー。
        /// 無効時はnull。
        /// </summary>
        private LoginUser InputUser
        {
            get
            {
                return inputUser;
            }
            set
            {
                if (value == null)
                {
                    ApplicationContext.Login.UserId = 0;
                    ApplicationContext.Login.UserCode = null;
                    ApplicationContext.Login.UserName = null;
                    txtUserName.Text = "";
                }
                else
                {
                    ApplicationContext.Login.UserId = value.Id;
                    ApplicationContext.Login.UserCode = value.Code;
                    ApplicationContext.Login.UserName = value.Name;
                    txtUserCode.Text = value.Code;
                    txtUserName.Text = value.Name;
                }

                inputUser = value;
            }
        }

        /// <summary>
        /// 会社コード検索画面を表示する。
        /// </summary>
        private void ShowCompanySearchDialog()
        {
            var title = "会社コード検索";
            var company = ShowSearchDialog(title, new Dialogs.CompanyGridLoader(ApplicationContext), 800, 600);

            if (company != null)
            {
                InputCompany = company;

                // ユーザーコードが入力されている場合はそちらも再取得を試みる
                if (!string.IsNullOrEmpty(txtUserCode.Text))
                    txtUserCode_Validated(txtUserCode, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 担当者コード検索画面を表示する。
        /// </summary>
        private void ShowLoginUserSearchDialog()
        {
            if (InputCompany == null)
            {
                return;
            }

            var title = "ログインユーザー検索";
            var user = ShowSearchDialog(title, new Dialogs.LoginUserGridLoader(ApplicationContext, true), 800, 600);

            if (user != null)
            {
                InputUser = user;
                ColorSetting.ClearLoadedColors();
            }
        }

        /// <summary>
        /// ログイン処理を実行する。
        /// ログインに成功した場合は ApplicationContext.Login の各項目をセットする。
        /// </summary>
        private void Login()
        {
            if (!Authenticate()) return;

            var login = new DummyLogin();
            Settings.SaveControlValue<PA0101>(login, txtCompanyCode.Name, txtCompanyCode.Text);
            Settings.SaveControlValue<PA0101>(login, txtUserCode.Name, txtUserCode.Text);

            ShowMainMenu();
        }

        /// <summary>
        /// ログイン認証を実行する。
        /// </summary>
        /// <returns>認証成否</returns>
        private bool Authenticate(bool ignoreExpired = false)
        {
            var validate = new Func<bool, Control, string, string, bool>((isValid, control, messageId, controlTitle) =>
            {
                if (isValid) return true;

                ShowWarningDialog(messageId, controlTitle);
                control.Focus();

                return false;
            });

            if (!validate(!string.IsNullOrEmpty(txtCompanyCode.Text), txtCompanyCode, MsgWngInputRequired, "会社コード"))
            {
                return false;
            }

            if (!validate(InputCompany != null, txtCompanyCode, MsgWngInvalidInputValue, "会社コード"))
            {
                return false;
            }

            if (!validate(!string.IsNullOrEmpty(txtUserCode.Text), txtUserCode, MsgWngInputRequired, "担当者コード"))
            {
                return false;
            }

            if (!validate(InputUser != null, txtUserCode, MsgWngInvalidInputValue, "担当者コード"))
            {
                return false;
            }

            if (!validate(!string.IsNullOrEmpty(txtPassword.Text), txtPassword, MsgWngInputRequired, "パスワード"))
            {
                return false;
            }

            if (!ValidateLoginUserLicense()) return false;

            var loginResult = Login(ApplicationContext.Login.SessionKey, InputCompany.Id, InputUser.Id, txtPassword.Text);
            if (!loginResult.HasValue || loginResult == LoginResult.DBError)
            {
                ShowWarningDialog(MsgErrSomethingError, "ログイン処理");
                return false;
            }
            if (loginResult == LoginResult.Failed)
            {
                ShowWarningDialog(MsgWngInvalidInputValue, "パスワード");
                txtPassword.Focus();
                return false;
            }
            if (!ignoreExpired && loginResult == LoginResult.Expired)
            {
                ShowWarningDialog(MsgWngExpiredPaswordAndChangeNewOne);
                return false;
            }
            
            if (loginResult == LoginResult.Success ||
                ignoreExpired && loginResult == LoginResult.Expired) return true;
            throw new NotImplementedException();
        }

        /// <summary>
        /// ログイン画面を隠してメインメニュー画面を表示する。
        /// </summary>
        /// <remarks>
        /// ログイン画面はアプリケーションのメインウィンドウなのでCloseしてはいけない。
        /// </remarks>
        private void ShowMainMenu()
        {
            PA0201 menu = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                menu = new PA0201(ApplicationContext)
                {
                    StartPosition = FormStartPosition.CenterParent
                };

                Hide();
                ApplicationContext.ShowDialog(this, menu, true);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                menu?.Dispose();

                if (!this.IsDisposed) // メニュー画面でApplication.Exitするとエラーになるため回避
                {
                    Logout();
                    Show();
                }
            }
        }

        /// <summary>
        /// ログアウト処理(UI制御を含む)
        /// </summary>
        private void Logout()
        {
            txtPassword.Text = "";
            txtPassword.Select();
        }

        /// <summary>
        /// パスワード変更画面を表示する。
        /// </summary>
        private void ShowChangePassword()
        {
            if (!Authenticate(true))
            {
                return;
            }

            // VOneScreenBase.CreateDialog相当の処理。現時点で保持していないデータは渡さない。
            var dialog = new Dialogs.dlgChangePassword(txtPassword.Text);
            dialog.CompanyInfo = null;
            dialog.ApplicationContext = ApplicationContext;
            dialog.ApplicationControl = null;
            dialog.XmlMessenger = messenger;

            ApplicationContext.ShowDialog(this, dialog);
            txtPassword.Select();
        }

        /// <summary>
        /// アプリケーションを終了する。
        /// </summary>
        private void ExitApplication()
        {
            if (!ShowConfirmDialog(MsgQstConfirmApplicationExit)) return;
            Application.Exit();
        }

        /// <summary>
        /// 会社コード入力値を検証し、InputCompanyプロパティ値を制御する。
        /// </summary>
        private void ValidateCompanyCode()
        {
            ColorSetting.ClearLoadedColors();
            if (string.IsNullOrEmpty(txtCompanyCode.Text))
            {
                InputCompany = null;
                txtUserCode.Text = "";
                return;
            }

            txtCompanyCode.Text = LeftPaddingZero(txtCompanyCode.Text);

            var result = GetCompanyByCode(ApplicationContext.Login.SessionKey, txtCompanyCode.Text);
            if (result == null || !result.ProcessResult.Result)
            {
                InputCompany = null;
                return;
            }
            var company = result.Company;
            InputCompany = company;
        }

        /// <summary>
        /// 担当者コード入力値を検証し、InputCompanyプロパティ値を制御する。
        /// </summary>
        private void ValidateUserCode()
        {
            ColorSetting.ClearLoadedColors();
            if (InputCompany == null || string.IsNullOrEmpty(txtUserCode.Text)) // 会社コードが無効 or ユーザコードが空欄
            {
                InputUser = null;
                return;
            }

            var result = GetLoginUserByCode(ApplicationContext.Login.SessionKey, InputCompany.Id, txtUserCode.Text);
            if (result == null
                || !result.ProcessResult.Result
                || result.Users == null
                || result.Users.Count() < 1)
            {
                InputUser = null;
                return;
            }
            var users = GetLoginUser_UseClient(inputCompany.Id);
            if (users == null || users.Count() < 1)
            {
                InputUser = null;
                return;
            }
            InputUser = users.Where(x => x.Code == result.Users.FirstOrDefault().Code).FirstOrDefault();
        }

        private bool ValidateLoginUserLicense()
        {
            var loginUserLicenses = Util.GetLoginUserLicenses(ApplicationContext.Login, InputCompany.Id);
            if (loginUserLicenses == null)
            {
                ShowWarningDialog(MsgErrSomethingError, "ログイン処理");
                return false;
            }

            if (!CheckDecryptCodeAll(loginUserLicenses))
            {
                return false;
            }

            return true;
        }

        private bool CheckDecryptCodeAll(List<LoginUserLicense> loginUserLicenses)
        {
            foreach (var loginUserLicense in loginUserLicenses) {
                if (!VOne.Common.Security.License.CheckDecryptCode(loginUserLicense.LicenseKey,
                    InputCompany.ProductKey))
                {
                    ShowWarningDialog(MsgWngLicenseIsUnjust);
                    return false;
                }

                var containCount = 0;
                foreach (var loginUserLicense_ in loginUserLicenses)
                {
                    if (loginUserLicense.LicenseKey == loginUserLicense_.LicenseKey)
                    {
                        containCount++;
                    }
                }
                if (containCount > 1)
                {
                    ShowWarningDialog(MsgWngLicenseIsUnjust);
                    return false;
                }
            }

            var uses = GetLoginUser_UseClient(InputCompany.Id);
            if (uses == null)
            {
                ShowWarningDialog(MsgErrSomethingError, "ログイン処理");
                return false;
            }

            if (loginUserLicenses.Count < uses.Count())
            {
                ShowWarningDialog(MsgWngLicenseIsUnjust);
                return false;
            }
            return true;
        }

        #region Web Service

        /// <summary>
        /// ログイン処理(LoginUserPasswordMaster.svc:Login)を呼び出して結果を取得する。
        /// </summary>
        private LoginResult? Login(string sessionKey, int companyId, int loginUserId, string password)
        {
            LoginProcessResult result = null;
            ServiceProxyFactory.Do<LoginUserPasswordMasterService.LoginUserPasswordMasterClient>(client =>
            {
                try
                {
                    result = client.Login(sessionKey, companyId, loginUserId, password);
                }
                catch (Exception ex)
                {
                    NLogHandler.WriteErrorLog(this, ex, sessionKey);
                }
            });
            return result?.Result;
        }

        /// <summary>
        /// 会社情報取得処理(CompanyMaster.svc:GetByCode)を呼び出して結果を取得する。
        /// </summary>
        private CompanyResult GetCompanyByCode(string sessionKey, string companyCode)
        {
            return  ServiceProxyFactory.Do((CompanyMasterService.CompanyMasterClient client) =>
            {
                try
                {
                    return client.GetByCode(sessionKey, companyCode);
                }
                catch (Exception ex)
                {
                    NLogHandler.WriteErrorLog(this, ex, sessionKey);
                }
                return null;
            });
        }

        /// <summary>
        /// 担当者情報取得処理(LoginUserMaster.svc:GetByCode)を呼び出して結果を取得する。
        /// </summary>
        private UsersResult GetLoginUserByCode(string sessionKey, int companyId, string userCode)
        {
            return ServiceProxyFactory.Do((LoginUserMasterService.LoginUserMasterClient client) =>
            {
                try
                {
                    return client.GetByCode(sessionKey, companyId, new[] { userCode });
                }
                catch (Exception ex)
                {
                    NLogHandler.WriteErrorLog(this, ex, sessionKey);
                }
                return null;
            });
        }
        private List<LoginUser> GetLoginUser_UseClient(int companyId)
     => ServiceProxyFactory.Do((LoginUserMasterService.LoginUserMasterClient client) =>
     {
         var result = client.GetItems(ApplicationContext.Login.SessionKey,
             companyId,
             new LoginUserSearch { UseClient = 1 });
         if (result == null || result.ProcessResult.Result == false) return null;
         return result.Users;
     });

        #endregion Web Service

        #region Helper

        private XmlMessenger messenger = new XmlMessenger();
        private DialogResult ShowDialog(string messageId, params string[] args)
            => messenger.GetMessageInfo(messageId, args).ShowMessageBox(this);

        /// <summary>
        /// メッセージボックスを表示する。
        /// </summary>
        /// <remarks>
        /// VOneScreenBaseから流用。
        /// </remarks>
        private void ShowWarningDialog(string messageId, params string[] args)
            => ShowDialog(messageId, args);

        private bool ShowConfirmDialog(string messageId, params string[] args)
            => ShowDialog(messageId, args) == DialogResult.Yes;

        /// <summary>
        /// ダイアログ画面を表示する。
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="formName"></param>
        /// <param name="gridLoader"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        /// <remarks>
        /// VOneScreenBaseから流用。
        /// </remarks>
        private TModel ShowSearchDialog<TModel>(string formName, IGridLoader<TModel> gridLoader, int width = 700, int height = 600)
            where TModel : class
        {
            using (var form = ApplicationContext.Create(nameof(PH9900)))
            {
                var screen = form.GetAll<PH9900>().FirstOrDefault();

                Debug.Assert(screen != null);
                screen.InitializeParentForm(formName, width, height);
                var result = screen.ShowDialog(ParentForm, gridLoader);
                if (result != null) return result;
            }
            return default(TModel);
        }

        /// <summary>会社コード のコードタイプが数字の場合、4桁 0 で LeftPaddingする</summary>
        private string LeftPaddingZero(string code)
        {
            if (DataExpression.CompanyCodeTypeGlobal != 0) return code;
            return code.PadLeft(4, '0');
        }

        #endregion Helper
    }
}
