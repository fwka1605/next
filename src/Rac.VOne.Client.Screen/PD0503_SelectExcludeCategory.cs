using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Web.Models;
using GrapeCity.Win.Editors;

namespace Rac.VOne.Client.Screen
{
    /// <summary>対象外区分選択</summary>
    public partial class PD0503 : VOneScreenBase
    {
        public PD0503()
        {
            InitializeComponent();
            InitializeUserComponent();
        }

        public int ExcludeCategoryId { get; set; }

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
            ParentForm.Load += PD0503_Load;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("選択");
            BaseContext.SetFunction02Caption("");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction10Caption("キャンセル");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = btnF01_Click;
            OnF02ClickHandler = null;
            OnF03ClickHandler = null;
            OnF10ClickHandler = btnF10_Click;
        }

        private void PD0503_Load(object sender, EventArgs e)
        {
            ProgressDialog.Start(ParentForm, InitializeLoadDataAsyn(), false, SessionKey);
        }

        private async Task InitializeLoadDataAsyn()
            => await Task.WhenAll(
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadCategoryFlagComboAsync());

        [OperationLog("選択")]
        private void btnF01_Click()
        {
            this.ExcludeCategoryId = (int)cmbCategoryFlag.SelectedItem.SubItems[1].Value;
            ParentForm.DialogResult = DialogResult.OK;
        }

        [OperationLog("キャンセル")]
        private void btnF10_Click()
        {
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        #region webサービス呼び出し
        private async Task<List<Category>> LoadCategoryFlagComboAsync()
         => await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) =>
            {
                CategorySearch categorySearch = new CategorySearch();
                categorySearch.CompanyId = CompanyId;
                categorySearch.CategoryType = 4;

                var result = await client.GetItemsAsync(SessionKey, categorySearch);

                if (result != null
                    || result.ProcessResult.Result)
                {
                    var CategoryList = result.Categories;
                    for (int i = 0; i < CategoryList.Count; i++)
                    {
                        cmbCategoryFlag.Items.Add(new ListItem(CategoryList[i].CodeAndName, CategoryList[i].Id));
                    }

                    cmbCategoryFlag.SelectedIndex = 0;

                    return result.Categories;
                }
                else
                {
                    return null;
                }
            });
        #endregion
    }
}
