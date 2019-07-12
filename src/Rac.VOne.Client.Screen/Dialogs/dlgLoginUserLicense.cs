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
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common.MultiRow;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class dlgLoginUserLicense : Dialog
    {
        public List<LoginUserLicense> RegisteredLoginUserLicense;
        public string ProductKey;
        public int CurrentCompanyId;
        public bool IsUpdated = false;

        #region 初期化

        public dlgLoginUserLicense()
        {
            InitializeComponent();
        }

        private void dlgLoginUserLicense_Load(object sender, EventArgs e)
        {
            InitializeScreen();
        }

        private void InitializeScreen()
        {
            grid.SetupShortcutKeys();
            InitializeGridTemplate();
            InitializeGrid();

            statusStrip.Visible = true;
            InitializeSelected();
        }


        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var txtCell = builder.GetTextBoxCell();
            txtCell.MaxLength = 64;

            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "ColumnOrder", cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, 250, "LicenseKey", caption: "ライセンスキー", cell:txtCell, readOnly:false),
            });

            grid.Template = builder.Build();
            grid.AllowUserToAddRows = true;
            grid.EditMode = EditMode.EditOnKeystrokeOrShortcutKey;
            grid.ScrollBars = ScrollBars.Vertical;
            grid.AllowUserToResize = false;
            grid.AllowClipboard = true;
            grid.AllowDrop = true;
            grid.ShortcutKeyManager.Register(EditingActions.Paste, Keys.Control | Keys.V);

        }

        private void InitializeGrid()
        {

            for (var i = 0; i < RegisteredLoginUserLicense.Count; i++)
            {
                grid.Rows.Add();

                grid.SetValue(i, "celLicenseKey", RegisteredLoginUserLicense[i].LicenseKey);
                grid.Rows[i]["celLicenseKey"].Enabled = false;
            }
        }

        private void InitializeSelected()
        {
            grid.Select();
            grid.CurrentCellPosition = new CellPosition(grid.RowCount - 1, "celLicenseKey");
            grid.FirstDisplayedCellPosition = new CellPosition(grid.RowCount - 1, "celLicenseKey");
        }


        #endregion 初期化

        #region 画面操作関連イベントハンドラ

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnClose);
            CloseDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.ButtonClicked(btnAdd);
            var newLicenseList = new List<string>();

            for (var i = 0; i < grid.RowCount - 1; i++)
            {
                if (grid.Rows[i]["celLicenseKey"].Enabled == false) continue;
                
                var currentLicenseKey = Convert.ToString(grid.GetValue(i, "celLicenseKey"));

                if (!CheckLicenseKey(currentLicenseKey, i)) return;
                newLicenseList.Add(currentLicenseKey);
            }
            if (newLicenseList.Count() < 1)
            {
                ShowWarningDialog(MsgWngNoLicenseKey, "ライセンスキー");
                InitializeSelected();
                return;
            }

            if (!ShowConfirmDialog(MsgQstConfirmSave))
                return;

            var isUpdated = RegisterLicenseKeys(newLicenseList.ToArray());
            if (isUpdated)
            {
                if (!IsUpdated) IsUpdated = true;
                for (var i = 0; i < grid.RowCount - 1; i++)
                {
                    if (i == grid.RowCount - 1) break;
                    grid.Rows[i]["celLicenseKey"].Enabled = false;
                }
            }
        }

        #endregion 画面操作関連イベントハンドラ

        #region functions

        /// <summary>
        /// ダイアログを閉じる。
        /// </summary>
        private void CloseDialog()
        {
            Close();
        }

        private bool CheckLicenseKey(string licenseKey, int rowIndex)
        {
            if (string.IsNullOrEmpty(licenseKey) ||
                !VOne.Common.Security.License.CheckDecryptCode(licenseKey, this.ProductKey))
            {
                ShowWarningDialog(MsgWngUnjustLicenseKey);
                return false;
            }

            if (!CheckRepetition(licenseKey, rowIndex))
            {
                ShowWarningDialog(MsgWngLicenseKeyIsRepeated);
                return false;
            }
            return true;

        }

        private bool CheckRepetition(string licenseKey, int rowIndex)
        {
            for (var i = 0; i < grid.RowCount - 1; i++)
            {
                if (i == rowIndex) continue;

                if (Convert.ToString( grid.GetValue(i, "celLicenseKey")) == licenseKey)
                {
                    return false;
                }
            }
            return true;
        }

        private bool RegisterLicenseKeys(string[] LicenseKeys)
        {
            var result = false;

            var task = Task.Run(async () =>
            {
                result = await SaveUserLicensesAsync(LicenseKeys);
            });
            ProgressDialog.Start(this, task, false, SessionKey);

            if (result)
            {
                ShowWarningDialog(MsgInfSaveSuccess);
                return true;
            }
            else
            {
                ShowWarningDialog(MsgErrSaveError);
                return false;
            }
        }

        #endregion functions

        #region Web Service

        /// <summary>
        /// ユーザーライセンスを登録する
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="licenseKey"></param>
        /// <returns></returns>
        private async Task<bool> SaveUserLicensesAsync(string[] LicenseKeys)
        {
            var result = await ServiceProxyFactory.DoAsync(async (LoginUserLicenseMasterService.LoginUserLicenseMasterClient client)
                => await client.SaveAsync(SessionKey, CurrentCompanyId, LicenseKeys));

            if (result == null || result.ProcessResult.Result == false)
                return false;

            return result.ProcessResult.Result;
        }

        #endregion Web Service
    }
}
