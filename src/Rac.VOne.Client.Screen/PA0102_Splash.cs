using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Screen
{
    /// <summary>
    /// スプラッシュ画面
    /// </summary>
    public partial class PA0102 : Form
    {
        public PA0102()
        {
            InitializeComponent();

            var isCloudEdition = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsCloudEdition"]);
            BackgroundImage = (isCloudEdition) ? Properties.Resources.cloud_splash : Properties.Resources.splash;
        }

        private static PA0102 splashScreen;

        /// <summary>
        /// スプラッシュ画面を表示する。
        /// </summary>
        public static void ShowSplashScreen()
        {
            if (splashScreen == null)
            {
                Application.Idle += new EventHandler(Application_Idle);

                splashScreen = new PA0102();
                splashScreen.Show();
            }
        }

        /// <summary>
        /// アプリケーションがアイドル状態になったらスプラッシュ画面を閉じる。
        /// </summary>
        private static void Application_Idle(object sender, EventArgs e)
        {
            if (splashScreen?.IsDisposed == false)
            {
                splashScreen.Close();
            }

            splashScreen = null;
            Application.Idle -= new EventHandler(Application_Idle);
        }
    }
}
