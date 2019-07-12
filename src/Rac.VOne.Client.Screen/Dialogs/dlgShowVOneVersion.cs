using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class dlgShowVOneVersion : Form
    {
        //CopyRights
        //private string strCopyRights = "Copyright (c) 2003-2010 David Hall";
        public dlgShowVOneVersion()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var version = asm.GetName().Version;
            this.VOneVer2.Text = $"Version {version}";

            //Cloud版使用時、アイコン等差し替え
            var isCloudEdition = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsCloudEdition"]);
            var appName = isCloudEdition ? CloudApplicationName : StandardApplicationName;
            Text = $"{appName}のバージョン情報";
            VOneVer1.Text = appName;
            picturelogo.Image = (isCloudEdition) ? Properties.Resources.logo_cloud : Properties.Resources.logo_002;
        }
      
        private void OKButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
