using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client.Common;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class dlgDriveSelect : Dialog
    {
        public string Drive { get; private set; }

        public string InitialDrive { get; set; }

        public dlgDriveSelect()
        {
            InitializeComponent();
            statusStrip.Visible = false;
        }

        private void dlgDriveSelect_Load(object sender, EventArgs e)
        {
            txtDrive.Text = InitialDrive;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidInput()) return;

            Drive = txtDrive.Text;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Drive = string.Empty;
            DialogResult = DialogResult.Cancel;
        }

        private bool ValidInput()
        {
            if (string.IsNullOrEmpty(txtDrive.Text))
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, lblDrive.Text);
                txtDrive.Focus();
                return false;
            }

            try
            {
                if (SystemInformation.TerminalServerSession)
                {
                    Directory.GetDirectories(DefaultClient + txtDrive.Text);
                }
                else
                {
                    var isAvailable = DriveInfo.GetDrives().Any(x => x.DriveType == DriveType.Fixed && x.Name == txtDrive.Text + @":\");

                    if (!isAvailable)
                    {
                        ShowWarningDialog(MsgWngInvalidDriveLetter, txtDrive.Text);
                        txtDrive.Select();
                        return false;
                    }
                }
            }
            catch(IOException ex)
            {
                if (ex.Message.Contains("ネットワーク名が見つかりません。") && SystemInformation.TerminalServerSession)
                    ShowWarningDialog(MsgWngSetLocalDriveForClientAccess);
                else if (ex.Message.Contains("デバイスの準備ができていません。"))
                    ShowWarningDialog(MsgWngInvalidDriveLetter, txtDrive.Text);
                else
                    ShowWarningDialog(MsgErrSomethingError, "ドライブ取得");

                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }
            catch(Exception ex)
            {
                ShowWarningDialog(MsgErrSomethingError, "ドライブ取得");
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                return false;
            }

            return true;
        }
    }
}
