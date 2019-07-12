using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client.Screen.Dialogs;

namespace Rac.VOne.Client.Screen
{
    /// <summary>債権代表者登録</summary>
    public partial class PE0113 : VOneScreenBase
    {
        internal string ParentCustomerId { get; set; }
        public PE0113()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            FormWidth = 340;
            FormHeight = 200;
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF01")
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    else if (button.Name == "btnF10")
                        button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    else
                        button.Visible = false;
                }
            };
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Load += PE0113_Load;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("選択");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = btnF01_Click;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);
            OnF02ClickHandler = null;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = null;

            BaseContext.SetFunction10Caption("キャンセル");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = btnF10_Click;
        }

        internal Action<Rac.VOne.Client.Common.Controls.VOneComboControl> ComboBoxInitializer { private get; set; }

        private void PE0113_Load(object sender, EventArgs e)
        {
            ComboBoxInitializer?.Invoke(cmbCus);
            ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
        }

        private async Task InitializeLoadDataAsync()
        {
            await Task.WhenAll(
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync());
        }

        [OperationLog("選択")]
        private void btnF01_Click()
        {
            ParentCustomerId = cmbCus.SelectedItem.SubItems[1].Value.ToString();
            ParentForm.DialogResult = DialogResult.OK;
        }

        [OperationLog("キャンセル")]
        private void btnF10_Click()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }

    }
}
