using Rac.VOne.Web.Models;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Forms
{
    static class Program
    {
        private static string ApplicationName { get; set; } = "Victory ONE G4";
        private readonly static Mutex mutex = new Mutex(initiallyOwned: false, name: ApplicationName);

        /// <summary>
        /// [App.Config 設定値]
        /// 認証キー。
        /// </summary>
        private static string AuthenticationKey { get; set; }

        /// <summary>
        /// [App.Config 設定値]
        /// 認証に用いる会社コード。
        /// ログイン時の会社コードとは異なるので先頭にAuthenticationを付けている。
        /// </summary>
        private static string AuthenticationCompanyCode { get; set; }

        /// <summary>ES版かどうか</summary>
        private static bool IsCloudEdition { get; set; }

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!LoadAppConfigSettings())
            {
                MessageBox.Show("LoadAppConfigSettings failed."); // UNDONE: 仮コード
                return;
            }

            var singleInstance = ValidateMutext(MainProcess);
            if (!singleInstance)
            {
                MessageBox.Show("すでにシステムが起動しています。",
                    IsCloudEdition ? CloudApplicationName : StandardApplicationName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private static bool ValidateMutext(Action main = null)
        {
            var hasHandle = false;
            try {
                hasHandle = mutex.WaitOne(0, false);
                if (!hasHandle) return hasHandle;
                main?.Invoke();
            }
            catch (Exception)
            {
                hasHandle = true;
            }
            finally
            {
                if (hasHandle)
                    mutex.ReleaseMutex();
                mutex.Close();
            }
            return hasHandle;
        }

        private static void MainProcess()
        {
            Screen.PA0102.ShowSplashScreen();

            Thread.GetDomain().UnhandledException += (sender, e) => {
                Debug.Fail(e.ExceptionObject.ToString());
            };

            var context = Initialize();
            if (context == null) return;
            var mainForm = new Screen.PA0101(context);
            Application.Run(mainForm);
        }

        private static IApplication Initialize()
        {
            var context = new FormLoader();

            var sessionKey = Screen.Util.Authenticate(AuthenticationKey, AuthenticationCompanyCode);
            if (sessionKey == null)
            {
                MessageBox.Show("Authentication faild."); // UNDONE: 仮コード
                return null;
            }

            context.Login.SessionKey = sessionKey;
            return context;
        }

        /// <summary>
        /// 初期化に必要な情報を App.config から読み込む。
        /// <para>AuthenticationKey</para>
        /// <para>CompanyCode</para>
        /// <para>CompanyCodeType → DataExpression.CompanyCodeTypeGlobal に格納</para>
        /// </summary>
        private static bool LoadAppConfigSettings()
        {
            try
            {
                AuthenticationKey = ConfigurationManager.AppSettings["AuthenticationKey"];
                AuthenticationCompanyCode = ConfigurationManager.AppSettings["CompanyCode"];
                IsCloudEdition = Convert.ToBoolean(ConfigurationManager.AppSettings["IsCloudEdition"]);
                DataExpression.CompanyCodeTypeGlobal = int.Parse(ConfigurationManager.AppSettings["CompanyCodeType"]);
            }
            catch
            {
                // TODO: エラーハンドリング
                return false;
            }

            return true;
        }
    }
}
