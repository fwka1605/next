﻿using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Screen
{
    /// <summary>一括消込・入金部門絞込</summary>
    public partial class PE0106 : VOneScreenBase
    {
        private List<Section> SectionsAll { get; set; }
        private List<Section> SectionsWithLoginUser { get; set; }
        public List<int> InitialIds { private get; set; }
        public List<int> SelectedIds { get; private set; }
        public string SelectedState { get; private set; }

        /// <summary>
        ///  呼出し時 「すべて」 となっていたか否か
        /// </summary>
        public bool AllSection { get; set; }


        public PE0106() : base()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        private void InitializeUserComponent()
        {
            grdSearch.SetupShortcutKeys();
            FormWidth = 700;
            FormHeight = 600;
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF01"
                        || button.Name == "btnF02"
                        || button.Name == "btnF03")
                    {
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    }
                    else if (button.Name == "btnF10"
                        || button.Name == "btnF08"
                        || button.Name == "btnF09")
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
            ParentForm.KeyPreview = false;

            txtSearchKey.KeyDown += (sender, e) => control_KeyDown(sender, e);

            ParentForm.Load += (sender, e) => {
                var tasks = new List<Task> {
                    LoadApplicationControlAsync(),
                    LoadCompanyAsync(),
                    LoadControlColorAsync(),
                    Task.Run(async () => SectionsAll = await GetSectionsAsync()),
                    Task.Run(async () => SectionsWithLoginUser = await GetSectionsWithLoginUserAsync())
                };
                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
            };
            ParentForm.Shown += (sender, ea) =>
            {
                grdSearch.Template = CreateGridTemplate();
                grdSearch.DataSource = new BindingSource(SectionsAll, null);
                if (AllSection)
                {
                    SetCheckState(check: 1);
                }
                else
                {
                    var ids = InitialIds.Any()
                        ? InitialIds
                        : new List<int>(SectionsWithLoginUser.Select(x => x.Id));
                    SetCheckState(check: 1, filter: x => ids.Contains(x.Id));
                }
                InitialIds = GetCheckedIds();
                SortGridData();
                grdSearch.CurrentCellDirtyStateChanged += (_, __) => grdSearch.CommitEdit(GrapeCity.Win.MultiRow.DataErrorContexts.Commit);
            };
        }

        private void control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;

            if (txtSearchKey.Equals(sender))
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

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("確定");
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = Confirm;

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

            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction08Enabled(true);
            OnF08ClickHandler = DoCheck;

            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction09Enabled(true);
            OnF09ClickHandler = DoUncheck;

            BaseContext.SetFunction10Caption("戻る");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        [OperationLog("検索")]
        private void Search()
        {
            var code = txtSearchKey.Text.ToLower();
            //var branchCode = txtBranchCode.Text.ToLower();
            Func<string, bool> contains = value
                => !string.IsNullOrEmpty(value)
                && value.IndexOf(code, StringComparison.OrdinalIgnoreCase) >= 0;
            var ids = GetCheckedIds();
            var filteredSource = SectionsAll.FindAll(x =>
                contains(x.Code) || contains(x.Name) || ids.Contains(x.Id));
            grdSearch.DataSource = new BindingSource(filteredSource, null);
            SetCheckState(check: 1, filter: x => ids.Contains(x.Id));
            SortGridData();
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            txtSearchKey.Clear();
            txtSearchKey.Focus();

            grdSearch.DataSource = new BindingSource(SectionsAll, null);
            if (InitialIds.Any())
                SetCheckState(check: 1, filter: x => InitialIds.Contains(x.Id));
            SortGridData();
        }

        [OperationLog("確定")]
        private void Confirm()
        {
            grdSearch.EndEdit();
            SelectedIds = GetCheckedIds();
            if (!SelectedIds.Any()) SelectedIds = SectionsAll.Select(x => x.Id).ToList();
            var count = SelectedIds.Count;
            SelectedState
                = (count == SectionsAll.Count) ? "すべて"
                : (count == 1) ? (grdSearch.Rows.First(x => IsChecked(x)).DataBoundItem as Section).Name
                : "入金部門絞込有";
            ParentForm.DialogResult = DialogResult.OK;
        }

        [OperationLog("全選択")]
        private void DoCheck() => SetCheckState(check: 1);

        [OperationLog("全解除")]
        private void DoUncheck() => SetCheckState(check: 0);
        

        [OperationLog("戻る")]
        private void Exit()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        private GrapeCity.Win.MultiRow.Template CreateGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, sortable: true);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, 40 , "Header"   , cell:builder.GetRowHeaderCell()),
                new CellSetting(height, 50 , "Check"    , cell:builder.GetCheckBoxCell()           , caption: "選択", readOnly: false),
                new CellSetting(height, 115, "Code"     , cell:builder.GetTextBoxCell(MiddleCenter), caption: "入金部門コード", dataField: nameof(Section.Code), sortable:true),
                new CellSetting(height, 385, "Name"     , cell:builder.GetTextBoxCell()            , caption: "入金部門名"    , dataField: nameof(Section.Name), sortable:true),
                new CellSetting(height, 0  , "SectionId", dataField: nameof(Section.Id)   )
            });
            return builder.Build();
        }

        private GrapeCity.Win.MultiRow.MultiRowContentAlignment MiddleCenter
        { get { return GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter; } }


        #region grid methods

        private List<int> GetCheckedIds()
            => grdSearch.Rows.Where(x => IsChecked(x))
            .Select(x => ConvertRowToSection(x).Id).ToList();

        private Section ConvertRowToSection(GrapeCity.Win.MultiRow.Row row)
            => row.DataBoundItem as Section;
        private void SetCheckState(int check, Predicate<Section> filter = null)
        {
            grdSearch.EndEdit();
            var items = filter != null
                ? grdSearch.Rows.Where(row => filter(ConvertRowToSection(row)))
                : grdSearch.Rows;
            foreach (var row in items)
                SetRowCheckState(row, check);
        }
        private void SetRowCheckState(GrapeCity.Win.MultiRow.Row row, int check)
            => row.Cells["celCheck"].Value = check;

        private bool IsChecked(GrapeCity.Win.MultiRow.Row row)
            => Convert.ToBoolean(row["celCheck"].Value);

        private void SortGridData()
        {
            var items = new GrapeCity.Win.MultiRow.SortItem[]
                {
                    new GrapeCity.Win.MultiRow.SortItem("celCheck", SortOrder.Descending),
                    new GrapeCity.Win.MultiRow.SortItem("celCode", SortOrder.Ascending),
                };
            grdSearch.Sort(items);
        }
        #endregion

        #region web service call

        private async Task<List<Section>> GetSectionsAsyncCommon(Func<SectionMasterService.SectionMasterClient, Task<SectionsResult>> getter)
        {
            List<Section> results = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<SectionMasterService.SectionMasterClient>();
                var webRes = await getter(client);
                if (webRes.ProcessResult.Result)
                    results = webRes.Sections;
            });
            return results ?? new List<Section>();
        }

        private async Task<List<Section>> GetSectionsAsync()
            => await GetSectionsAsyncCommon(async client
                => await client.GetByCodeAsync(SessionKey, CompanyId, Code: null));

        private async Task<List<Section>> GetSectionsWithLoginUserAsync()
            => await GetSectionsAsyncCommon(async client
                => await client.GetByLoginUserIdAsync(SessionKey, Login.UserId ));


        #endregion
    }
}