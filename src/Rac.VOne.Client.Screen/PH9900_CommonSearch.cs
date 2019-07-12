using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.Dialogs;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>コード検索画面</summary>
    public partial class PH9900 : VOneScreenBase
    {
        private dynamic GridLoader { get; set; }
        private bool ShowBankBranch { get { return GridLoader is BankBranchGridLoader; } }

        //private string FormTitle { get; set; }
        //public int FormWidth { get; set; }
        //public int FormHeight { get; set; }

        private bool DoPreset { get { return Company == null || Company?.PresetCodeSearchDialog == 1; } }

        public PH9900() : base()
        {
            InitializeComponent();
            grdSearch.SetupShortcutKeys();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            grdSearch.CellDoubleClick += (sender, e) =>
            {
                if (e.Scope != GrapeCity.Win.MultiRow.CellScope.Row) return;
                this.FunctionCalled(2, SelectItem);
            };
            FormWidth = 700;
            FormHeight = 600;
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF01"
                        || button.Name == "btnF02")
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

            ParentForm.Shown += (sender, e) =>
            {
                pnlSearchBankBranch.Visible = ShowBankBranch;
                if (ShowBankBranch)
                {
                    lblSearchKey.Text = "銀行検索";
                    lblBranchCode.Text = "支店検索";
                }
                txtSearchKey.Select();

                grdSearch.ShortcutKeyManager.Unregister(Keys.Enter);
                grdSearch.ShortcutKeyManager.Unregister(Keys.Shift | Keys.Enter);
                grdSearch.ShortcutKeyManager.Register(new SelectAction(), Keys.Enter);
            };

            ParentForm.KeyPreview = false;

            txtSearchKey.KeyDown += (sender, e) => control_KeyDown(sender, e);
            txtBranchCode.KeyDown += (sender, e) => control_KeyDown(sender, e);
        }

        public class SelectAction : GrapeCity.Win.MultiRow.IAction
        {
            public bool CanExecute(GrapeCity.Win.MultiRow.GcMultiRow target) => true;
            public string DisplayName => ToString();
            public void Execute(GrapeCity.Win.MultiRow.GcMultiRow target)
            {
                if (target?.CurrentCell == null
                    || target?.CurrentCell.RowIndex < 0) return;
                var parentForm = target.TopLevelControl as Form;
                if (parentForm == null) return;
                parentForm.DialogResult = DialogResult.OK;
            }
        }

        private void control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;
            if (ShowBankBranch && txtSearchKey.Equals(sender))
            {
                SelectNextControl(ActiveControl, !e.Shift, true, true, true);
                return;
            }

            if (txtSearchKey.Equals(sender)
                || txtBranchCode.Equals(sender))
            {
                Search();
            }
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption("選択");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = SelectItem;

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

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        [OperationLog("検索")]
        private void Search()
        {
            ClearStatusMessage();
            var code = txtSearchKey.Text.ToLower();
            var branchCode = txtBranchCode.Text.ToLower();

            try
            {
                var task = ShowBankBranch
                ? GridLoader.SearchByKey(code, branchCode)
                : GridLoader.SearchByKey(code);

                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                grdSearch.DataSource = new BindingSource(task.Result, null);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }

            if (grdSearch.RowCount == 0)
            {
                ShowWarningDialog(MsgWngNotExistSearchData);
                txtSearchKey.Select();
            }
            else
            {
                grdSearch.Select();
            }
        }

        [OperationLog("選択")]
        private void SelectItem()
        {
            ClearStatusMessage();
            if (grdSearch?.CurrentCell == null
                || grdSearch?.CurrentCell.RowIndex < 0) return;
            ParentForm.DialogResult = DialogResult.OK;
        }

        [OperationLog("戻る")]
        private void Exit()
        {
            ClearStatusMessage();
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        public TModel ShowDialog<TModel>(IWin32Window owner, IGridLoader<TModel> loader)
            where TModel : class
        {
            GridLoader = loader as IGridLoader<TModel>;

            var loadTask = new List<Task>();
            if (ApplicationControl == null)
            {
                loadTask.Add(LoadApplicationControlAsync());
            }
            if (Company == null)
            {
                loadTask.Add(LoadCompanyAsync());
            }
            loadTask.Add(LoadControlColorAsync());

            IEnumerable<TModel> items = null;
            var task = Task.Run(async () =>
            {
                await Task.WhenAll(loadTask);
                if (!DoPreset) return;
                items = await loader.SearchInfo();
            });
            ProgressDialog.Start(owner, task, false, SessionKey);

            ParentForm.Shown += (sender, e) =>
            {
                grdSearch.Template = loader.CreateGridTemplate();
                if (DoPreset)
                {
                    grdSearch.DataSource = new BindingSource(items, null);
                }
            };

            if (ApplicationContext.ShowDialog(owner, ParentForm, true) == DialogResult.OK)
            {
                var model = grdSearch.CurrentRow.DataBoundItem as TModel;
                if (model != null) return model;
            }
            return default(TModel);
        }

    }
}
