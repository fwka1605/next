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
using Rac.VOne.Client.Common.Settings;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class dlgPublishInvoices : Dialog
    {
        #region メンバー＆コンストラクター
        public PublishResult Result;
        public enum PublishResult
        {
            Cancel = 0, Print, CSV
        }
        public dlgPublishInvoices()
        {
            InitializeComponent();
        }
        #endregion

        #region 初期化処理
        private void dlgPublishInvoices_Load(object sender, EventArgs e)
        {
            rdoPrintPossible.Checked = Settings.RestoreControlValue<dlgPublishInvoices, bool>(Login, rdoPrintPossible.Name) ?? false;
            if (!rdoPrintPossible.Checked) rdoPrintImpossible.Checked = true;
            btnF03.Select();
        }
        #endregion

        #region ファンクションキー押下処理
        [OperationLog("戻る")]
        private void btnF10_Click(object sender, EventArgs e)
        {
            this.FunctionCalled(10, btnF10);
            Result = PublishResult.Cancel;
            Exit();
            Close();
        }
        [OperationLog("発行")]
        private void btnF03_Click(object sender, EventArgs e)
        {
            this.FunctionCalled(10, btnF03);
            if (rdoPrintPossible.Checked)
            {
                Result = PublishResult.Print;
            }
            else
            {
                Result = PublishResult.CSV;
            }
            Exit();
            Close();
        }

        private void Exit()
        {
            Settings.SaveControlValue<dlgPublishInvoices>(Login, rdoPrintPossible.Name, rdoPrintPossible.Checked);

        }
        #endregion
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F10: btnF10.PerformClick(); return true;
                case Keys.F3: btnF03.PerformClick(); return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
