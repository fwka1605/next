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
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{

    /// <summary>
    /// 前受金消込処理年月日の設定
    /// </summary>
    public partial class PE0110 : VOneScreenBase
    {
        public int AdvanceReceiveSetting { get; set; }
        public DateTime? AdvanceDat { get; set; }
        public DateTime AdvanceReceiveRecordDate { get; set; }

        public PE0110()
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
                    if (button.Name == "btnF01")
                    {
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
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
                    LoadControlColorAsync()
                };
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
                SetMatchingDate();
            };
        }
        #endregion

        #region PE0110 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("消込");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = ProcessMatching;

            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction02Enabled(false);
            OnF02ClickHandler = null;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = null;

            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = null;

            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);
            OnF05ClickHandler = null;

            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = null;

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

        #region F01 消込処理
        [OperationLog("消込")]
        private void ProcessMatching()
        {
            if (!datMatchingDate.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, lblRecordedAt.Text);
                datMatchingDate.Focus();
                return;
            }
            AdvanceReceiveRecordDate = datMatchingDate.Value.Value;
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
        private void SetMatchingDate()
        {
            if (AdvanceReceiveSetting == 0) // 未入力
            {
                datMatchingDate.Clear();
            }
            else if (AdvanceReceiveSetting == 1)
            {
                datMatchingDate.Value = DateTime.Today;
            }
            else if (AdvanceDat.HasValue)
            {
                datMatchingDate.Value = AdvanceDat.Value;
            }
        }
        #endregion
    }
}
