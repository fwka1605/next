using Rac.VOne.Client.Common;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class dlgChangePassword : Dialog
    {
        /// <summary>
        /// 旧パスワード
        /// </summary>
        private string OldPassword;

        #region 初期化

        private dlgChangePassword()
        {
            InitializeComponent();
        }

        public dlgChangePassword(string oldPassword) : this()
        {
            OldPassword = oldPassword;
        }

        private void dlgChangePassword_Load(object sender, EventArgs e)
        {
            statusStrip.Visible = true;
            txtNewPassword.Select();
        }

        #endregion 初期化

        #region Function Keys

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F1: ChangePassword(); return true;
                case Keys.F2: ClearPasswords(); return true;
                case Keys.F10: CloseDialog(); return true;
            }

            return false;
        }

        #endregion Function Keys

        #region 画面操作関連イベントハンドラ

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                ChangePassword();
            }
            catch (Exception ex)
            {
                ShowWarningDialog(MsgErrSaveError);
                txtNewPassword.Select();

                NLogHandler.WriteErrorLog(this, ex, ApplicationContext.Login.SessionKey);
            }
        }

        private void btnClearPasswords_Click(object sender, EventArgs e)
        {
            ClearPasswords();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CloseDialog();
        }

        #endregion 画面操作関連イベントハンドラ

        #region functions

        /// <summary>
        /// 新パスワード
        /// </summary>
        private string NewPassword
        {
            get { return txtNewPassword.Text; }
            set { txtNewPassword.Text = value; }
        }

        /// <summary>
        /// 新パスワード(確認入力)
        /// </summary>
        private string Confirmation
        {
            get { return txtConfirmation.Text; }
            set { txtConfirmation.Text = value; }
        }

        /// <summary>
        /// パスワードを変更する。
        /// </summary>
        private void ChangePassword()
        {
            if (!CheckInputPasswords())
            {
                return;
            }
            if (!CheckPasswordPolicy())
            {
                return;
            }

            var login = ApplicationContext.Login;

            var result = ChangePassword(login.SessionKey, login.CompanyId, login.UserId, OldPassword, NewPassword);
            if (result == null || result != PasswordChangeResult.Success)
            {
                switch (result)
                {
                    case null:
                    case PasswordChangeResult.Failed:
                        ShowWarningDialog(MsgErrSaveError);
                        break;

                    case PasswordChangeResult.ProhibitionSamePassword:
                        ShowWarningDialog(MsgWngProhibitionSamePassword);
                        break;

                    default:
                        throw new NotImplementedException($"PasswordChangeResult = {result.ToString()}");
                }

                txtNewPassword.Select();
                return;
            }

            OldPassword = NewPassword; // ダイアログを閉じずに再変更するケースに対応
            ClearPasswords(false);

            ShowWarningDialog(MsgInfSaveSuccess);
        }

        /// <summary>
        /// Nullチェック／必須チェック／一致チェック。
        /// エラーがあれば表示し入力フォーカスを制御する。
        /// </summary>
        /// <returns>合否</returns>
        private bool CheckInputPasswords()
        {
            if (string.IsNullOrEmpty(NewPassword))
            {
                ShowWarningDialog(MsgWngInputRequired, "パスワード");
                txtNewPassword.Select();
                return false;
            }
            if (string.IsNullOrEmpty(Confirmation))
            {
                ShowWarningDialog(MsgWngInputRequired, "パスワード(確認)");
                txtConfirmation.Select();
                return false;
            }
            if (NewPassword != Confirmation)
            {
                ShowWarningDialog(MsgWngInvalidPassword);
                txtConfirmation.Select();
                return false;
            }

            return true;
        }

        /// <summary>
        /// パスワードポリシーによるパスワード妥当性チェック。
        /// エラーがあれば表示し入力フォーカスを制御する。
        /// </summary>
        /// <returns>合否</returns>
        private bool CheckPasswordPolicy()
        {
            var policy = GetPasswordPolicy(ApplicationContext.Login.SessionKey, ApplicationContext.Login.CompanyId);
            if (policy == null)
            {
                ShowWarningDialog(MsgErrSaveError);
                txtNewPassword.Select();
                return false;
            }

            var validationResult = policy.Validate(NewPassword);
            if (validationResult != PasswordValidateResult.Valid)
            {
                switch (validationResult)
                {
                    case PasswordValidateResult.ProhibitionAlphabetChar:
                        ShowWarningDialog(MsgWngProhibitionAlphabetChar);
                        break;
                    case PasswordValidateResult.ProhibitionNumberChar:
                        ShowWarningDialog(MsgWngProhibitionNumberChar);
                        break;
                    case PasswordValidateResult.ProhibitionSymbolChar:
                        ShowWarningDialog(MsgWngProhibitionSymbolChar);
                        break;
                    case PasswordValidateResult.ProhibitionNotAllowedSymbolChar:
                        ShowWarningDialog(MsgWngProhibitionNotAllowedSymbolChar);
                        break;
                    case PasswordValidateResult.ShortageAlphabetCharCount:
                        ShowWarningDialog(MsgWngShortageAlphabetCharCount, policy.MinAlphabetUseCount.ToString());
                        break;
                    case PasswordValidateResult.ShortageNumberCharCount:
                        ShowWarningDialog(MsgWngShortageNumberCharCount, policy.MinNumberUseCount.ToString());
                        break;
                    case PasswordValidateResult.ShortageSymbolCharCount:
                        ShowWarningDialog(MsgWngShortageSymbolCharCount, policy.MinSymbolUseCount.ToString());
                        break;
                    case PasswordValidateResult.ShortagePasswordLength:
                        ShowWarningDialog(MsgWngShortagePasswordLength, policy.MinLength.ToString());
                        break;
                    case PasswordValidateResult.ExceedPasswordLength:
                        ShowWarningDialog(MsgWngExceedPasswordLength, policy.MaxLength.ToString());
                        break;
                    case PasswordValidateResult.ExceedSameRepeatedChar:
                        ShowWarningDialog(MsgWngExceedSameRepeatedChar, policy.MinSameCharacterRepeat.ToString());
                        break;
                    default:
                        throw new NotImplementedException($"PasswordValidateResult = {validationResult.ToString()}");
                }

                txtNewPassword.Select();
                return false;
            }

            return true;
        }

        /// <summary>
        /// パスワード入力欄をクリアし、入力フォーカスを移動する。
        /// 併せてステータスバー領域のメッセージもクリアする。
        /// </summary>
        private void ClearPasswords(bool clearStatusMessage = true)
        {
            txtNewPassword.Clear();
            txtConfirmation.Clear();

            if (clearStatusMessage)
            {
                ClearStatusMessage();
            }

            txtNewPassword.Select();
        }

        /// <summary>
        /// ダイアログを閉じる。
        /// </summary>
        private void CloseDialog()
        {
            Close();
        }

        #endregion functions

        #region Web Service

        /// <summary>
        /// パスワードポリシー取得処理(PasswordPolicyMaster.svc:Get)を呼び出して結果を取得する。
        /// </summary>
        private static PasswordPolicy GetPasswordPolicy(string sessionKey, int companyId)
        {
            PasswordPolicyResult result = null;

            ServiceProxyFactory.Do<PasswordPolicyMasterService.PasswordPolicyMasterClient>(client
                => result = client.Get(sessionKey, companyId));

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.PasswordPolicy;
        }

        /// <summary>
        /// パスワード変更処理(LoginUserPasswordMaster.svc:Change)を呼び出して結果を取得する。
        /// </summary>
        /// <param name="newPassword">null/empty時はArgumentExceptionをスロー。要事前チェック。</param>
        private static PasswordChangeResult? ChangePassword(string sessionKey, int companyId, int userId, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                new ArgumentException("null/empty password can't be allowed.", nameof(newPassword));
            }

            LoginPasswordChangeResult result = null;

            ServiceProxyFactory.Do<LoginUserPasswordMasterService.LoginUserPasswordMasterClient>(client
                => result = client.Change(sessionKey, companyId, userId, oldPassword, newPassword));

            if (result == null || result.ProcessResult.Result == false)
            {
                return null;
            }

            return result.Result;
        }

        #endregion Web Service
    }
}
