using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client.Screen.Dialogs;

namespace Rac.VOne.Client.Screen
{
    /// <summary>一括消込チェックリスト　出力内容指定</summary>
    public partial class PE0112 : VOneScreenBase
    {
        /// <summary> ダイアログの選択結果
        /// 0:全件 , 1:チェックのみ , 2:チェックなしのみ </summary>
        public int CheckType { get; set; }

        /// <summary>出力方法 0 : 印刷, 1 : エクスポート</summary>
        public int OutputType { get; set; }

        /// <summary>一括消込画面 チェック状態  0:混在 , 1:チェックのみ , 2:チェックなしのみ</summary>
        public int ParentCheckType { get; set; }

        public PE0112()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        #region 画面初期設定
        private void InitializeUserComponent()
        {
            FormWidth = 336;
            FormHeight = 214;
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (OutputType == 0 && button.Name == "btnF04"
                    ||  OutputType == 1 && button.Name == "btnF06")
                    {
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                        button.Location = new Point(1, 0);
                    }
                    else if (button.Name == "btnF10")
                    {
                        button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    }
                    else
                    {
                        button.Visible = false;
                    }
                }
            };

        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Load += (sender, e) =>
            {
                var tasks = new List<Task> {
                    LoadApplicationControlAsync(),
                    LoadCompanyAsync(),
                    LoadControlColorAsync(),
                };
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
                SetControlEnable();
            };
        }
        #endregion

        #region PE0112 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("");
            BaseContext.SetFunction01Enabled(false);
            OnF01ClickHandler = null;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);
            OnF02ClickHandler = null;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = null;

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = ProcessPrint;

            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);
            OnF05ClickHandler = null;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = ProcessExport;

            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);
            OnF07ClickHandler = null;

            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = null;

            BaseContext.SetFunction09Caption("");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = null;

            BaseContext.SetFunction10Caption("キャンセル");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region 印刷処理
        [OperationLog("印刷")]
        private void ProcessPrint()
        {
            CheckType = rdoAll.Checked ? 0 : rdoChecked.Checked ? 1 : 2;
            ParentForm.DialogResult = DialogResult.OK;
        }
        #endregion

        #region エクスポート処理
        [OperationLog("エクスポート")]
        private void ProcessExport()
        {
            CheckType = rdoAll.Checked ? 0 : rdoChecked.Checked ? 1 : 2;
            ParentForm.DialogResult = DialogResult.OK;
        }
        #endregion

        #region F10 キャンセル
        [OperationLog("キャンセル")]
        private void Exit()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region CommonFunction
        private void SetControlEnable()
        {
            BaseContext.SetFunction04Enabled(OutputType == 0);
            BaseContext.SetFunction06Enabled(OutputType == 1);

            if (ParentCheckType == 1)
            {
                rdoAll.Enabled = false;
                rdoUnChecked.Enabled = false;
            }
            else if (ParentCheckType == 2)
            {
                rdoAll.Enabled = false;
                rdoChecked.Enabled = false;
            }
            else
            {
                rdoAll.Enabled = true;
                rdoAll.Checked = true;
                rdoChecked.Enabled = true;
                rdoUnChecked.Enabled = true;
            }
        }

        #endregion
    }
}
