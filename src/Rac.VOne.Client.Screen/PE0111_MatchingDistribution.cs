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
    public partial class PE0111 : VOneScreenBase
    {
        public PE0111()
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
                    if (button.Name == "btnF07")
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
            };
        }
        #endregion

        #region PE0111 InitializeFunctionKeys
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

            BaseContext.SetFunction04Caption("");
            BaseContext.SetFunction04Enabled(false);
            OnF04ClickHandler = null;

            BaseContext.SetFunction05Caption("");
            BaseContext.SetFunction05Enabled(false);
            OnF05ClickHandler = null;

            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction06Enabled(false);
            OnF06ClickHandler = null;

            BaseContext.SetFunction07Caption("配信");
            BaseContext.SetFunction07Enabled(true);
            OnF07ClickHandler = ProcessDistribution;

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

        #region F7 配信処理
        [OperationLog("配信")]
        private void ProcessDistribution()
        {

        }
        #endregion

        #region F10 キャンセル
        [OperationLog("キャンセル")]
        private void Exit()
        {

        }
        #endregion

        #region CommonFunction
        //public void ShowDialog(IWin32Window owner,
        //    Action<PE0111> initializer,
        //    Action<PE0111> resultSetter)
        //{
        //    var loadTask = new List<Task>();
        //    if (ApplicationControl == null)
        //    {
        //        loadTask.Add(LoadApplicationControlAsync());
        //    }
        //    if (Company == null)
        //    {
        //        loadTask.Add(LoadCompanyAsync());
        //    }
        //    ProgressDialog.Start(owner, Task.WhenAll(loadTask), false, SessionKey);

        //    InitializeParentForm("未消込入金メール配信　データ設定");
        //    initializer?.Invoke(this);

        //    if (ApplicationContext.ShowDialog(owner, ParentForm) == DialogResult.OK)
        //    {
        //        resultSetter?.Invoke(this);
        //    }
        //}
        #endregion
    }
}
