using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;
using Rac.VOne.Client.Screen.CategoryMasterService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Screen
{
    /// <summary>請求書設定・回収区分選択</summary> ///
    public partial class PC1502 : VOneScreenBase
    {
        private List<Category> CollectCategoryAll { get; set; }
        public List<int> InitialIds { private get; set; }
        public List<int> SelectedIds { get; private set; }
        public string SelectedState { get; private set; }

        /// /// <summary>呼出し時 「すべて」 となっていたか否か</summary> ///
        public bool AllSelected { get; set; }

        public PC1502() : base()
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

            this.Load += (s, e) =>
            {
                var tasks = new List<Task>();
                if (ApplicationControl == null)
                    tasks.Add(LoadApplicationControlAsync());

                if (Company == null)
                    tasks.Add(LoadCompanyAsync());

                tasks.Add(LoadControlColorAsync());
                var categorySearch = new CategorySearch();
                categorySearch.CompanyId = CompanyId;
                categorySearch.CategoryType = CategoryType.Collect;
                //Task<List<Category>> categoryTask = GetCollectCategoryListAsync(categorySearch);
                Task<List<Category>> categoryTask = GetCollectCategoryListAsync();
                tasks.Add(categoryTask);

                ProgressDialog.Start(ParentForm, Task.WhenAll(tasks), false, SessionKey);
                CollectCategoryAll = categoryTask.Result;

                ParentForm.Shown += (sender, ea) =>
                {
                    grdSearch.Template = CreateGridTemplate();
                    grdSearch.DataSource = new BindingSource(CollectCategoryAll, null);
                    if (!AllSelected && InitialIds.Any())
                        SetCheckState(check: 1, filter: x => InitialIds.Contains(x.Id));
                    else
                        SetCheckState(check: 0);

                    InitialIds = GetCheckedIds();
                    SortGridData();
                };
            };
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.KeyPreview = false;

            txtSearchKey.KeyDown += (sender, e) => control_KeyDown(sender, e);
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
            Func<string, bool> contains = value
                => !string.IsNullOrEmpty(value)
                && value.IndexOf(code, StringComparison.OrdinalIgnoreCase) >= 0;
            var ids = GetCheckedIds();
            var filteredSource = CollectCategoryAll.FindAll(x =>
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

            grdSearch.DataSource = new BindingSource(CollectCategoryAll, null);
            if (InitialIds.Any())
                SetCheckState(check: 1, filter: x => InitialIds.Contains(x.Id));
            SortGridData();
        }

        [OperationLog("確定")]
        private void Confirm()
        {
            SelectedIds = GetCheckedIds();
            //if (!SelectedIds.Any()) SelectedIds = CollectCategoryAll.Select(x => x.Id).ToList();
            var count = SelectedIds.Count;
            SelectedState
                = (count == CollectCategoryAll.Count) ? "すべて"
                : (count == 1) ? ConvertRowToCategory(grdSearch.Rows.First(x => IsChecked(x))).Name
                : (count == 0 ) ? string.Empty : "対象外有";
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
                new CellSetting(height, 40 , "Header"      , cell:builder.GetRowHeaderCell()),
                new CellSetting(height, 50 , "Check"       , cell:builder.GetCheckBoxCell()           , caption: "選択", readOnly: false),
                new CellSetting(height, 115, "Code"        , cell:builder.GetTextBoxCell(MiddleCenter), caption: "回収区分コード", dataField: nameof(Category.Code), sortable:true),
                new CellSetting(height, 385, "Name"        , cell:builder.GetTextBoxCell()            , caption: "回収区分名"    , dataField: nameof(Category.Name), sortable:true),
                new CellSetting(height, 0  , "CategoryId", dataField: nameof(Category.Id)   )
            });
            return builder.Build();
        }

        private GrapeCity.Win.MultiRow.MultiRowContentAlignment MiddleCenter
        { get { return GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter; } }


        #region grid methods

        private List<int> GetCheckedIds()
            => grdSearch.Rows.Where(x => IsChecked(x))
            .Select(x => ConvertRowToCategory(x).Id).ToList();

        private Category ConvertRowToCategory(GrapeCity.Win.MultiRow.Row row)
            => row.DataBoundItem as Category;
        private void SetCheckState(int check, Predicate<Category> filter = null)
        {
            grdSearch.EndEdit();
            var items = filter != null
                ? grdSearch.Rows.Where(row => filter(ConvertRowToCategory(row)))
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

        #region webservice
        private async Task<List<Category>> GetCollectCategoryListAsync()
            => await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) =>
            {
                List<Category> list = null;
                var result = await client.GetInvoiceCollectCategoriesAsync(SessionKey, CompanyId, CategoryType.Collect);
                if (result.ProcessResult.Result)
                    list = result.Categories;
                return list ?? new List<Category>();
            });
        #endregion


    }
}
