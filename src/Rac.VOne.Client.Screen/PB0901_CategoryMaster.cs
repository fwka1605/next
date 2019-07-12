using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.AccountTitleMasterService;
using Rac.VOne.Client.Screen.BankAccountMasterService;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.CustomerPaymentContractMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.IgnoreKanaMasterService;
using Rac.VOne.Client.Screen.NettingService;
using Rac.VOne.Client.Screen.PaymentAgencyMasterService;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Client.Screen.TaxClassMasterService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>区分マスター</summary>
    public partial class PB0901 : VOneScreenBase
    {
        //変数宣言
        public Func<IEnumerable<IMasterData>, bool> SavePostProcessor { get; set; }
        public Func<IEnumerable<IMasterData>, bool> DeletePostProcessor { get; set; }
        private string TaxClass { get; set; }
        private string HiddenId { get; set; }
        private string AccountTitleId { get; set; }
        private string PaymentAgencyId { get; set; }

        private bool IsBillingCategory { get { return cmbCategoryType.SelectedIndex == 0; } }
        private bool IsReceiptCategory { get { return cmbCategoryType.SelectedIndex == 1; } }
        private bool IsCollectCategory { get { return cmbCategoryType.SelectedIndex == 2; } }
        private bool IsExcludeCategory { get { return cmbCategoryType.SelectedIndex == 3; } }

        public PB0901()
        {
            InitializeComponent();
            grdCategory.SetupShortcutKeys();
            lblPaymentAgencyName.Visible = false;
            Text = "区分マスター";
            CheckEditControl();
        }

        /// <summary>消費税属性コンボボックス設定</summary>
        private async Task TaxClassComboData()
        {
            List<TaxClass> TaxClassList = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<TaxClassMasterClient>();
                TaxClassResult result = await service.GetItemsAsync(SessionKey);

                if (result.ProcessResult.Result)
                {
                    TaxClassList = result.TaxClass;
                    for (int i = 0; i < TaxClassList.Count; i++)
                    {
                        cmbTaxClassName.Items.Add(new ListItem(TaxClassList[i].Id.ToString() + " : " + TaxClassList[i].Name.ToString(), TaxClassList[i].Id));
                        cmbTaxClassName.Items[i].Tag = TaxClassList[i];
                    }
                }
            });
        }

        /// <summary>区分識別コンボボックス(表示・非表示制御)</summary>
        public void CheckShowHideCombo()
        {
            if (ApplicationControl != null)
            {
                var useLongTermAdvancedReceived = ApplicationControl.UseLongTermAdvanceReceived;
                var useDiscount = ApplicationControl.UseDiscount;
                var useCashOnDueDate = ApplicationControl.UseCashOnDueDates;
                var useAccountTransfer = ApplicationControl.UseAccountTransfer;

                if (IsBillingCategory && useLongTermAdvancedReceived == 1)
                {
                    cbxUseLongTermAdvanceReceived.Enabled = true;
                    cbxUseLongTermAdvanceReceived.Visible = true;
                }
                else if (IsBillingCategory && useLongTermAdvancedReceived == 0)
                {
                    cbxUseLongTermAdvanceReceived.Enabled = false;
                    cbxUseLongTermAdvanceReceived.Visible = false;
                }

                if (IsBillingCategory && useDiscount == 1)
                {
                    cbxUseDiscount.Enabled = true;
                    cbxUseDiscount.Visible = true;
                }
                else if (IsBillingCategory && useDiscount == 0)
                {
                    cbxUseDiscount.Enabled = false;
                    cbxUseDiscount.Visible = false;
                }

                if (IsReceiptCategory && useCashOnDueDate == 1)
                {
                    cbxUseDiscount.Enabled = false;
                    cbxUseDiscount.Visible = true;
                }
                else if (IsReceiptCategory && useCashOnDueDate == 0)
                {
                    cbxUseDiscount.Enabled = false;
                    cbxUseDiscount.Visible = false;
                }

                if (IsCollectCategory && useAccountTransfer == 1)
                {
                    cbxUseDiscount.Enabled = true;
                    cbxUseDiscount.Visible = true;
                    lblPaymentAgency.Visible = true;
                    lblPaymentAgency.Enabled = true;
                    txtPaymentAgencyCode.Visible = true;
                    txtPaymentAgencyCode.Enabled = true;
                    btnPaymentAgency.Visible = true;
                    btnPaymentAgency.Enabled = true;
                    lblPaymentAgencyName.Visible = true;

                }
                else if (IsCollectCategory && useAccountTransfer == 0)
                {
                    cbxUseDiscount.Enabled = false;
                    cbxUseDiscount.Visible = false;
                    lblPaymentAgency.Visible = false;
                    lblPaymentAgency.Enabled = false;
                    txtPaymentAgencyCode.Visible = false;
                    txtPaymentAgencyCode.Enabled = false;
                    btnPaymentAgency.Visible = false;
                    btnPaymentAgency.Enabled = false;
                    lblPaymentAgencyName.Visible = false;
                }
            }
        }

        #region 画面のComboboxをえらぶ
        private void SelectCombo()
        {
            txtCategoryCode.Clear();
            txtCategoryName.Clear();
            txtAccountTitleCode.Clear();
            lblAccountTitle.Clear();

            if (IsBillingCategory)
            {
                ChangeSeikyuuCategory();
                CheckShowHideCombo();
                return;
            }

            if (IsReceiptCategory)
            {
                ChangeNyukinCategory();
                CheckShowHideCombo();
                return;
            }

            if (IsCollectCategory)
            {
                ChangeKaishuuCategory();
                btnPaymentAgency.Enabled = false;
                txtPaymentAgencyCode.Enabled = false;
                CheckShowHideCombo();
                return;
            }

            if (IsExcludeCategory)
            {
                ChangeTaishougaiCategory();
                return;
            }
        }

        /// <summary>1 : 請求(表示・非表示制御)</summary>
        private void ChangeSeikyuuCategory()
        {
            ChangeCommonKomoku();
            cbxUseLongTermAdvanceReceived.Text = "長前管理を行う";
            cbxUseDiscount.Text = "歩引計算を行う";
            cbxForeceMatchingIndividually.Visible = false;
            cbxForeceMatchingIndividually.Enabled = false;
        }

        /// <summary>2 : 入金(表示・非表示制御)</summary>
        private void ChangeNyukinCategory()
        {
            ChangeCommonKomoku();
            cbxUseLongTermAdvanceReceived.Text = "期日入力を行う";
            cbxUseDiscount.Text = "期日入金管理を行う";
            cbxForeceMatchingIndividually.Visible = true;
            cbxForeceMatchingIndividually.Enabled = true;
        }

        /// <summary>Common(表示・非表示制御)</summary>
        private void ChangeCommonKomoku()
        {
            lblTaxClassName.Visible = true;
            lblTaxClassName.Enabled = true;
            cmbTaxClassName.Visible = true;
            cmbTaxClassName.Enabled = true;
            lblMatchingOrder.Visible = true;
            lblMatchingOrder.Enabled = true;
            txtMatchingOrder.Visible = true;
            txtMatchingOrder.Enabled = true;
            cbxUseLongTermAdvanceReceived.Visible = true;
            cbxUseLongTermAdvanceReceived.Enabled = true;
            cbxUseInput.Visible = true;
            cbxUseInput.Enabled = true;
            lblPaymentAgency.Visible = false;
            lblPaymentAgency.Enabled = false;
            txtPaymentAgencyCode.Visible = false;
            txtPaymentAgencyCode.Enabled = false;
            btnPaymentAgency.Visible = false;
            btnPaymentAgency.Enabled = false;
            lblPaymentAgencyName.Visible = false;
        }

        /// <summary>3 : 回収(表示・非表示制御)</summary>
        private void ChangeKaishuuCategory()
        {
            cbxUseLongTermAdvanceReceived.Visible = true;
            cbxUseLongTermAdvanceReceived.Enabled = true;
            cbxUseDiscount.Visible = true;
            cbxUseDiscount.Enabled = true;
            cbxUseLongTermAdvanceReceived.Text = "期日入力を行う";
            cbxUseDiscount.Text = "口座振替を行う";
            lblPaymentAgency.Visible = true;
            lblPaymentAgency.Enabled = true;
            txtPaymentAgencyCode.Visible = true;
            txtPaymentAgencyCode.Enabled = true;
            btnPaymentAgency.Visible = true;
            btnPaymentAgency.Enabled = true;
            lblPaymentAgencyName.Visible = true;
            cbxUseInput.Visible = true;
            cbxUseInput.Enabled = true;
            cbxForeceMatchingIndividually.Visible = false;
            cbxForeceMatchingIndividually.Enabled = false;
            lblTaxClassName.Visible = false;
            lblTaxClassName.Enabled = false;
            cmbTaxClassName.Visible = false;
            cmbTaxClassName.Enabled = false;
            cmbTaxClassName.SelectedIndex = -1;
            lblMatchingOrder.Visible = false;
            lblMatchingOrder.Enabled = false;
            txtMatchingOrder.Visible = false;
            txtMatchingOrder.Enabled = false;
        }

        /// <summary>4 : 対象外(表示・非表示制御)</summary>
        private void ChangeTaishougaiCategory()
        {
            lblTaxClassName.Visible = true;
            lblTaxClassName.Enabled = true;
            cmbTaxClassName.Visible = true;
            cmbTaxClassName.Enabled = true;
            cbxForeceMatchingIndividually.Visible = false;
            cbxForeceMatchingIndividually.Enabled = false;
            cbxUseInput.Visible = false;
            cbxUseInput.Enabled = false;
            lblMatchingOrder.Visible = false;
            lblMatchingOrder.Enabled = false;
            txtMatchingOrder.Visible = false;
            txtMatchingOrder.Enabled = false;
            cbxUseLongTermAdvanceReceived.Visible = false;
            cbxUseLongTermAdvanceReceived.Enabled = false;
            cbxUseDiscount.Visible = false;
            cbxUseDiscount.Enabled = false;
            lblPaymentAgency.Visible = false;
            lblPaymentAgency.Enabled = false;
            txtPaymentAgencyCode.Visible = false;
            txtPaymentAgencyCode.Enabled = false;
            btnPaymentAgency.Visible = false;
            btnPaymentAgency.Enabled = false;
            lblPaymentAgencyName.Visible = false;
        }

        /// <summary>区分識別コンボボックス設定</summary>
        private void AddCategoryItems()
        {
            cmbCategoryType.Items.Add(new ListItem("1 :請求", 1));
            cmbCategoryType.Items.Add(new ListItem("2 :入金", 2));
            cmbCategoryType.Items.Add(new ListItem("3 :回収", 3));
            cmbCategoryType.Items.Add(new ListItem("4 :対象外", 4));
        }

        private void cmbCategoryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectCombo();
                InitializeCategoryGrid();
                Task<List<Category>> task = GetCategoryList();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                grdCategory.DataSource = new BindingSource(task.Result, null);
                Clear();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 画面の初期化
        public void InitializeCategoryGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            var middleRight = MultiRowContentAlignment.MiddleRight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                                                   , cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, 115, "Code"                         , dataField: nameof(Category.Code)                          , cell: builder.GetTextBoxCell(middleCenter) , caption: "区分コード"               ),
                new CellSetting(height, 130, "Name"                         , dataField: nameof(Category.Name)                          , cell: builder.GetTextBoxCell()             , caption: "区分名"                   ),
                new CellSetting(height, 115, "AccountTitleCode"             , dataField: nameof(Category.AccountTitleCode)              , cell: builder.GetTextBoxCell(middleCenter) , caption: "科目コード"               ),
                new CellSetting(height, 100, "AccountTitleName"             , dataField: nameof(Category.AccountTitleName)              , cell: builder.GetTextBoxCell()             , caption: "科目名"                   ),
                new CellSetting(height, 115, "SubCode"                      , dataField: nameof(Category.SubCode)                       , cell: builder.GetTextBoxCell(middleCenter) , caption: "補助コード"               ),
                new CellSetting(height,   0, "UseLimitDate"                 , dataField: nameof(Category.UseLimitDateText)              , cell: builder.GetTextBoxCell()             , caption: "期日入力"                 ),
                new CellSetting(height,   0, "UseLongTermAdvanceReceived"   , dataField: nameof(Category.UseLongTermAdvanceReceivedText), cell: builder.GetTextBoxCell()             , caption: "長前管理"                 ),
                new CellSetting(height,   0, "TaxClassName"                 , dataField: nameof(Category.TaxClassName)                  , cell: builder.GetTextBoxCell()             , caption: "消費税属性"               ),
                new CellSetting(height,   0, "MatchingOrder"                , dataField: nameof(Category.MatchingOrder)                 , cell: builder.GetTextBoxCell(middleRight)  , caption: "消込順序"                 ),
                new CellSetting(height, 100, "Note"                         , dataField: nameof(Category.Note)                          , cell: builder.GetTextBoxCell()             , caption: "備考"                     ),
                new CellSetting(height,   0, "Id"                           , dataField: nameof(Category.Id)                            , cell: builder.GetTextBoxCell()             , caption: "Id"                       ),
                new CellSetting(height,   0, "ForceMatchingIndividually"    , dataField: nameof(Category.ForceMatchingIndividually)     , cell: builder.GetTextBoxCell()             , caption: "ForceMatchingIndividually"),
                new CellSetting(height,   0, "UseInput"                     , dataField: nameof(Category.UseInput)                      , cell: builder.GetTextBoxCell()             , caption: "UseInput"                 ),
                new CellSetting(height,   0, "UseDiscount"                  , dataField: nameof(Category.UseDiscount)                   , cell: builder.GetTextBoxCell()             , caption: "UseDiscount"              ),
                new CellSetting(height,   0, "UseCashOnDueDates"            , dataField: nameof(Category.UseCashOnDueDates)             , cell: builder.GetTextBoxCell()             , caption: "UseCashOnDueDates"        ),
                new CellSetting(height,   0, "AccountTitleId"               , dataField: nameof(Category.AccountTitleId)                , cell: builder.GetTextBoxCell()             , caption: "AccountTitleId"           ),
                new CellSetting(height,   0, "PaymentAgencyId"              , dataField: nameof(Category.PaymentAgencyId)               , cell: builder.GetTextBoxCell()             , caption: "PaymentAgencyId"          ),
                new CellSetting(height,   0, "PaymentAgencyName"            , dataField: nameof(Category.PaymentAgencyName)             , cell: builder.GetTextBoxCell()             , caption: "PaymentAgencyName"        ),
                new CellSetting(height,   0, "UpdateAt"                     , dataField: nameof(Category.UpdateAt)                      , cell: builder.GetTextBoxCell()             , caption: "UpdateAt"                 ),
                new CellSetting(height,   0, "UseAccountTransfer"           , dataField: nameof(Category.UseAccountTransfer)            , cell: builder.GetTextBoxCell()             , caption: "UseAccountTransfer"       ),
                new CellSetting(height,   0, "PaymentAgencyCode"            , dataField: nameof(Category.PaymentAgencyCode)             , cell: builder.GetTextBoxCell()             , caption: "PaymentAgencyCode"        ),
                new CellSetting(height,   0, "TaxClassId"                   , dataField: nameof(Category.TaxClassId)                    , cell: builder.GetTextBoxCell()             , caption: "TaxClassId"               ),
                new CellSetting(height,   0, "ExternalCode"                 , dataField: nameof(Category.ExternalCode)                  , cell: builder.GetTextBoxCell()             , caption: "外部コード"               )
            });

            if (IsBillingCategory)
            {
                SetBillingWidth(builder);
            }
            else if (IsReceiptCategory)
            {
                SetReceiptWidth(builder);
            }
            else if (IsCollectCategory)
            {
                SetCollectWidth(builder);
            }
            else if (IsExcludeCategory)
            {
                SetExcludeWidth(builder);
            }

            grdCategory.Template = builder.Build();
            grdCategory.HideSelection = true;
        }

        /// <summary>請求設定</summary>
        private void SetBillingWidth(GcMultiRowTemplateBuilder builder)
        {
            if (ApplicationControl?.UseLongTermAdvanceReceived == 1)
            {
                builder.Items[7].Width = 80; //長前管理
            }
            else
            {
                builder.Items[7].Width = 0; //長前管理
            }
            builder.Items[8].Width = 80; //消費税属性
            builder.Items[9].Width = 80; //消込順序
            builder.Items[10].Width = 0; //備考
        }

        /// <summary>入金設定</summary>
        private void SetReceiptWidth(GcMultiRowTemplateBuilder builder)
        {
            builder.Items[6].Width = 80;//期日入力
            builder.Items[8].Width = 80;//消費税属性
            builder.Items[9].Width = 80;//消込順序
            builder.Items[10].Width = 0; //備考
        }

        /// <summary>回収設定</summary>
        private void SetCollectWidth(GcMultiRowTemplateBuilder builder)
        {
            builder.Items[6].Width = 80;//期日入力
            builder.Items[10].Width = 0;//備考
        }

        /// <summary>対象外設定</summary>
        private void SetExcludeWidth(GcMultiRowTemplateBuilder builder)
        {
            builder.Items[8].Width = 80;//消費税属性
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ConfirmToClear;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }
        #endregion

        #region 画面の show data of gridview
        /// <summary>区分データ取得処理</summary>
        /// <returns>Category</returns>
        private async Task<List<Category>> GetCategoryList()
        {
            int categoryType = cmbCategoryType.SelectedIndex + 1;
            List<Category> list = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                CategoriesResult result = await service.GetByCodeAsync(SessionKey, CompanyId, categoryType, null);

                if (result.ProcessResult.Result)
                {
                    list = result.Categories;
                }
            });
            return list ?? new List<Category>();
        }
        #endregion

        #region 画面の gridview celldoubleclick
        private void grdCategory_CellDoubleClick(object sender, GrapeCity.Win.MultiRow.CellEventArgs e)
        {
            try
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

                ClearStatusMessage();

                if (e.RowIndex >= 0)
                {
                    txtCategoryCode.Enabled = false;
                    txtCategoryCode.Text = grdCategory.Rows[e.RowIndex].Cells[1].DisplayText;
                    txtCategoryName.Text = grdCategory.Rows[e.RowIndex].Cells[2].DisplayText;
                    txtAccountTitleCode.Text = grdCategory.Rows[e.RowIndex].Cells[3].DisplayText;
                    lblAccountTitle.Text = grdCategory.Rows[e.RowIndex].Cells[4].DisplayText;
                    txtCategorySubCode.Text = grdCategory.Rows[e.RowIndex].Cells[5].DisplayText;

                    if (grdCategory.Rows[e.RowIndex].Cells[22].Value != null)
                    {
                        cmbTaxClassName.Text = grdCategory.Rows[e.RowIndex].Cells[22].Value + " : " + grdCategory.Rows[e.RowIndex].Cells[8].Value;
                    }
                    else
                        cmbTaxClassName.SelectedIndex = -1;

                    txtMatchingOrder.Text = grdCategory.Rows[e.RowIndex].Cells[9].DisplayText;
                    txtExternalCode.Text = grdCategory.Rows[e.RowIndex].Cells[23].DisplayText;
                    txtExternalCode.Enabled = (!(IsReceiptCategory && string.CompareOrdinal(txtCategoryCode.Text, "98") >= 0));
                    txtNote.Text = grdCategory.Rows[e.RowIndex].Cells[10].DisplayText;
                    HiddenId = grdCategory.Rows[e.RowIndex].Cells[11].DisplayText;
                    AccountTitleId = grdCategory.Rows[e.RowIndex].Cells[16].DisplayText;

                    if (grdCategory.Rows[e.RowIndex].Cells[21].DisplayText == "")
                    {
                        txtPaymentAgencyCode.Clear();
                    }
                    else
                    {
                        txtPaymentAgencyCode.Text = grdCategory.Rows[e.RowIndex].Cells[21].DisplayText;
                    }

                    lblPaymentAgencyName.Text = grdCategory.Rows[e.RowIndex].Cells[18].DisplayText;
                    PaymentAgencyId = grdCategory.Rows[e.RowIndex].Cells[17].DisplayText;
                    string useLimit = grdCategory.Rows[e.RowIndex].Cells[6].DisplayText;
                    string useLong = grdCategory.Rows[e.RowIndex].Cells[7].DisplayText;

                    if ((useLimit == "行う" && IsReceiptCategory) || (useLong == "行う" && IsBillingCategory) || (useLimit == "行う" && IsCollectCategory))
                    {
                        cbxUseLongTermAdvanceReceived.Checked = true;
                    }
                    else
                    {
                        cbxUseLongTermAdvanceReceived.Checked = false;
                    }

                    if (int.Parse(grdCategory.Rows[e.RowIndex].Cells[13].DisplayText) == 1)
                    {
                        cbxUseInput.Checked = true;
                    }
                    else
                    {
                        cbxUseInput.Checked = false;
                    }

                    if (int.Parse(grdCategory.Rows[e.RowIndex].Cells[12].DisplayText) == 1)
                    {
                        cbxForeceMatchingIndividually.Checked = true;
                    }
                    else
                    {
                        cbxForeceMatchingIndividually.Checked = false;
                    }

                    if (int.Parse(grdCategory.Rows[e.RowIndex].Cells[20].DisplayText) == 1 && IsCollectCategory)
                    {
                        cbxUseDiscount.Checked = true;
                        btnPaymentAgency.Enabled = true;
                        txtPaymentAgencyCode.Enabled = true;
                    }
                    else if (int.Parse(grdCategory.Rows[e.RowIndex].Cells[15].DisplayText) == 1 && IsReceiptCategory)
                    {
                        cbxUseDiscount.Checked = true;
                    }
                    else if (int.Parse(grdCategory.Rows[e.RowIndex].Cells[14].DisplayText) == 1 && IsBillingCategory)
                    {
                        cbxUseDiscount.Checked = true;
                    }
                    else
                    {
                        cbxUseDiscount.Checked = false;
                        btnPaymentAgency.Enabled = false;
                        txtPaymentAgencyCode.Enabled = false;
                    }

                    if (IsBillingCategory)
                    {
                        ProgressDialog.Start(ParentForm, ExistBillingCategory(), false, SessionKey);
                    }

                    txtCategoryName.Focus();
                    Modified = false;
                    DeleteConstraint();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        #region 画面の F1
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                if (!ValidateChildren()) return;

                if (!ValidateSaveData()) return;

                ZeroLeftPaddingWithoutValidated();

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }
                SaveCategory();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(0, txtCategoryCode.TextLength, txtCategoryCode.MaxLength))
            {
                txtCategoryCode.Text = ZeroLeftPadding(txtCategoryCode);
                txtCategoryCode_Validated(null, null);
            }
            if (IsNeedValidate(ApplicationControl.AccountTitleCodeType, txtAccountTitleCode.TextLength, ApplicationControl.AccountTitleCodeLength))
            {
                txtAccountTitleCode.Text = ZeroLeftPadding(txtAccountTitleCode);
                txtAccountTitleCode_Validated(null, null);
            }
            if (IsNeedValidate(0, txtPaymentAgencyCode.TextLength, txtPaymentAgencyCode.MaxLength))
            {
                txtPaymentAgencyCode.Text = ZeroLeftPadding(txtPaymentAgencyCode);
                txtPaymentAgencyCode_Validated(null, null);
            }
        }

        private bool IsDuplicateExternalCode()
        {
            Task<List<Category>> task = GetCategoryList();
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            return task.Result.Any(x => x.Code != txtCategoryCode.Text && 
                                   x.ExternalCode == txtExternalCode.Text.Trim());
        }

        /// <summary>入力項目チェック</summary>
        /// <returns>bool</returns>
        private bool ValidateSaveData()
        {
            ClearStatusMessage();
            if (cmbCategoryType.SelectedIndex == -1)
            {
                ShowWarningDialog(MsgWngSelectionRequired, lblCategoryType.Text);
                cmbCategoryType.Select();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCategoryCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblCategoryCode.Text);
                txtCategoryCode.Select();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblCategoryName.Text);
                txtCategoryName.Select();
                return false;
            }

            if ((cmbTaxClassName.SelectedIndex == -1 && IsBillingCategory)
                || (cmbTaxClassName.SelectedIndex == -1 && IsReceiptCategory)
                || (cmbTaxClassName.SelectedIndex == -1 && IsExcludeCategory))
            {
                ShowWarningDialog(MsgWngSelectionRequired, lblTaxClassName.Text);
                cmbTaxClassName.Select();
                return false;
            }

            if ((string.IsNullOrWhiteSpace(txtMatchingOrder.Text) && IsBillingCategory)
                || (string.IsNullOrWhiteSpace(txtMatchingOrder.Text) && IsReceiptCategory))
            {
                ShowWarningDialog(MsgWngInputRequired, lblMatchingOrder.Text);
                txtMatchingOrder.Select();
                return false;
            }

            if (cbxUseDiscount.Checked && string.IsNullOrWhiteSpace(txtPaymentAgencyCode.Text) && IsCollectCategory)
            {
                ShowWarningDialog(MsgWngInputRequired, lblPaymentAgency.Text);
                txtPaymentAgencyCode.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtExternalCode.Text) && IsDuplicateExternalCode())
            {
                ShowWarningDialog(MsgWngDuplicateExternalCode);
                txtExternalCode.Select();
                return false;
            }

            return true;
        }

        /// <summary>登録処理</summary>
        private void SaveCategory()
        {
            var category = GetCategoryFromInputValues();
            var success = false;
            List<Category> newList = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                CategoryResult saveResult = null;
                saveResult = await service.SaveAsync(SessionKey, category);

                success = saveResult?.ProcessResult.Result ?? false;
                var syncResult = true;

                if (SavePostProcessor != null && success)
                {
                    syncResult = SavePostProcessor.Invoke(new IMasterData[] { saveResult.Category as IMasterData });
                }
                success &= syncResult;

                if (success)
                {
                    newList = await GetCategoryList();
                }
            });
            ProgressDialog.Start(ParentForm, task, false, base.SessionKey);

            if (success)
            {
                grdCategory.DataSource = new BindingSource(newList, null);
                Clear();
                btnPaymentAgency.Enabled = false;
                txtPaymentAgencyCode.Enabled = false;
                cmbCategoryType.Select();
                cbxUseInput.Checked = true;
                DispStatusMessage(MsgInfSaveSuccess);
            }
            else
            {
                ShowWarningDialog(MsgErrSaveError);
            }
        }

        /// <summary>登録項目</summary>
        private Category GetCategoryFromInputValues()
        {
            var category = new Category();

            if (HiddenId == "" || HiddenId == null)
            {
                HiddenId = 0.ToString();
            }
            category.Id = int.Parse(HiddenId);
            category.CompanyId = CompanyId;
            category.CategoryType = cmbCategoryType.SelectedIndex + 1;
            category.Code = txtCategoryCode.Text;
            category.Name = txtCategoryName.Text.Trim();
            var accountTitleId = 0;
            category.AccountTitleId = int.TryParse(AccountTitleId, out accountTitleId) ? (int?)accountTitleId : null;

            category.SubCode = txtCategorySubCode.Text;
            category.ExternalCode = txtExternalCode.Text;
            category.Note = txtNote.Text.Trim();
            var taxClassId = 0;
            category.TaxClassId = int.TryParse(TaxClass, out taxClassId) ? (int?)taxClassId : null;

            var paymentAgencyId = 0;
            category.PaymentAgencyId = int.TryParse(PaymentAgencyId, out paymentAgencyId) ? (int?)paymentAgencyId : null;
            category.AccountTitleCode = txtAccountTitleCode.Text;

            category.UseLongTermAdvanceReceived = cbxUseLongTermAdvanceReceived.Checked && IsBillingCategory ? 1 : 0;
            category.UseLimitDate = (cbxUseLongTermAdvanceReceived.Checked && (IsReceiptCategory || IsCollectCategory)) ? 1 : 0;

            category.UseDiscount = cbxUseDiscount.Checked && IsBillingCategory ? 1 : 0;
            category.UseCashOnDueDates = cbxUseDiscount.Checked && IsReceiptCategory ? 1 : 0;
            category.UseAccountTransfer = cbxUseDiscount.Checked && IsCollectCategory ? 1 : 0;

            category.UseInput = cbxUseInput.Checked && (IsBillingCategory || IsReceiptCategory || IsCollectCategory) ? 1 : 0;
            category.ForceMatchingIndividually = cbxForeceMatchingIndividually.Checked ? 1 : 0;
            category.UseAdvanceReceived = (IsReceiptCategory && category.Code == "99") ? 1 : 0;

            var matchingOrder = 0;
            int.TryParse(txtMatchingOrder.Text, out matchingOrder);
            category.MatchingOrder = matchingOrder;

            category.UpdateBy = Login.UserId;
            category.CreateBy = Login.UserId;
            return category;
        }
        #endregion

        #region 画面の F2
        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            ClearStatusMessage();
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            Clear();
        }

        /// <summary>画面クリア</summary>
        private void Clear()
        {
            ClearStatusMessage();
            BaseContext.SetFunction03Enabled(false);
            txtCategoryCode.Enabled = true;
            txtCategoryCode.Clear();
            txtCategoryName.Clear();
            txtAccountTitleCode.Clear();
            lblAccountTitle.Clear();
            txtCategorySubCode.Clear();
            txtExternalCode.Clear();
            txtExternalCode.Enabled = true;
            txtNote.Clear();
            txtMatchingOrder.Clear();
            txtPaymentAgencyCode.Clear();
            lblPaymentAgencyName.Clear();
            AccountTitleId = null;
            PaymentAgencyId = null;
            HiddenId = null;
            txtCategoryCode.Focus();
            cbxUseLongTermAdvanceReceived.Enabled = true;
            cmbTaxClassName.SelectedItem = null;
            cbxUseLongTermAdvanceReceived.Checked = false;
            cbxUseDiscount.Checked = false;
            cbxForeceMatchingIndividually.Checked = false;
            btnPaymentAgency.Enabled = false;
            txtPaymentAgencyCode.Enabled = false;
            cbxUseInput.Checked = true;
            if (IsReceiptCategory)
            {
                cbxUseDiscount.Enabled = false;
            }
            (cmbCategoryType.SelectedIndex == -1 ? (Control)cmbCategoryType : txtCategoryCode).Select();
            Modified = false;
        }
        #endregion

        #region 画面の F3
        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                if (!ValidateChildren()) return;
                //既存チェック
                Task<bool> isValid = ValidateDeleteCategoryId();
                ProgressDialog.Start(ParentForm, isValid, false, SessionKey);
                if (!isValid.Result) return;
                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                DeleteCategory();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>削除データ既存チェック</summary>
        /// <returns>Bool</returns>
        private async Task<bool> ValidateDeleteCategoryId()
        {
            var categoryId = int.Parse(HiddenId);

            Action<string[]> errorHandler = (args) =>
            {
                var id = MsgWngDeleteConstraint;
                ShowWarningDialog(id, args);
            };

            var bankAccountMasterValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<BankAccountMasterClient>();
                var result = await client.ExistCategoryAsync(SessionKey, categoryId);
                bankAccountMasterValid = !result.Exist;
                if (!bankAccountMasterValid)
                {
                    errorHandler.Invoke(new string[] { "銀行口座マスター", "区分コード" });
                }
            });
            if (!bankAccountMasterValid) return false;

            var customermasterValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CustomerMasterClient>();
                var result = await client.ExistCategoryAsync(SessionKey, categoryId);
                customermasterValid = !result.Exist;
                if (!customermasterValid)
                {
                    errorHandler.Invoke(new string[] { "得意先マスター", "区分コード" });
                }
            });
            if (!customermasterValid) return false;

            var customerPaymentMasterValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<CustomerPaymentContractMasterClient>();
                var result = await client.ExistAsync(categoryId, SessionKey);
                customerPaymentMasterValid = !result.Exist;
                if (!customerPaymentMasterValid)
                {
                    errorHandler.Invoke(new string[] { "得意先マスター約定", "区分コード" });
                }
            });
            if (!customerPaymentMasterValid) return false;

            var ignoreKanaMasterValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<IgnoreKanaMasterClient>();
                var result = await client.ExistCategoryAsync(SessionKey, categoryId);
                ignoreKanaMasterValid = !result.Exist;
                if (!ignoreKanaMasterValid)
                {
                    errorHandler.Invoke(new string[] { "除外カナマスター", "区分コード" });
                }
            });
            if (!ignoreKanaMasterValid) return false;

            if (IsBillingCategory)
            {
                var billingDataValid = false;
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<BillingServiceClient>();
                    var result = await client.ExistBillingCategoryAsync(SessionKey, categoryId);
                    billingDataValid = !result.Exist;
                    if (!billingDataValid)
                    {
                        errorHandler.Invoke(new string[] { "請求データ", "区分コード" });
                    }
                });
                if (!billingDataValid) return false;
            }
            if (IsCollectCategory)
            {
                var billingDataValid = false;
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<BillingServiceClient>();
                    var result = await client.ExistCollectCategoryAsync(SessionKey, categoryId);
                    billingDataValid = !result.Exist;
                    if (!billingDataValid)
                    {
                        errorHandler.Invoke(new string[] { "請求データ", "区分コード" });
                    }
                });
                if (!billingDataValid) return false;
            }

            if (IsReceiptCategory)
            {
                var receiptDataValid = false;
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<ReceiptServiceClient>();
                    var result = await client.ExistReceiptCategoryAsync(SessionKey, categoryId);
                    receiptDataValid = !result.Exist;
                    if (!receiptDataValid)
                    {
                        errorHandler.Invoke(new string[] { "入金データ", "区分コード" });
                    }
                });
                if (!receiptDataValid) return false;
            }

            if (IsExcludeCategory)
            {
                var receiptDataValid = false;
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<ReceiptServiceClient>();
                    var result = await client.ExistExcludeCategoryAsync(SessionKey, categoryId);
                    receiptDataValid = !result.Exist;
                    if (!receiptDataValid)
                    {
                        errorHandler.Invoke(new string[] { "入金データ", "区分コード" });
                    }
                });
                if (!receiptDataValid) return false;
            }

            var nettingDataValid = false;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<NettingServiceClient>();
                var result = await client.ExistReceiptCategoryAsync(SessionKey, categoryId);
                nettingDataValid = !result.Exist;
                if (!nettingDataValid)
                {
                    errorHandler.Invoke(new string[] { "相殺データ", "区分コード" });
                }
            });
            if (!nettingDataValid) return false;

            return true;
        }

        /// <summary>削除処理</summary>
        private void DeleteCategory()
        {
            var categoryId = int.Parse(HiddenId);
            bool success = false;

            CountResult deleteResult = null;
            CategoriesResult categoryResult = null;
            List<Category> newList = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                if (DeletePostProcessor != null)
                {
                    categoryResult = await service.GetAsync(SessionKey,
                        new int[] { categoryId });
                }
                deleteResult = await service.DeleteAsync(SessionKey, categoryId);
                success = (deleteResult.ProcessResult.Result)
                    && deleteResult.Count > 0;

                var syncResult = true;
                if (DeletePostProcessor != null && success)
                {
                    syncResult = DeletePostProcessor.Invoke(categoryResult
                        .Categories.AsEnumerable().Select(x => x as IMasterData));
                }
                success &= syncResult;

                if (success)
                {
                    newList = await GetCategoryList();
                }
            });
            ProgressDialog.Start(ParentForm, task, false, base.SessionKey);

            if (success)
            {
                grdCategory.DataSource = new BindingSource(newList, null);
                Clear();
                DispStatusMessage(MsgInfDeleteSuccess);
                btnPaymentAgency.Enabled = false;
                txtPaymentAgencyCode.Enabled = false;
            }
            else
            {
                ShowWarningDialog(MsgErrDeleteError);
            }
        }

        /// <summary>削除コンストレイント</summary>
        public void DeleteConstraint()
        {
            if ((IsReceiptCategory && txtCategoryCode.Text == "01")
                 || (IsReceiptCategory && txtCategoryCode.Text == "98")
                 || (IsReceiptCategory && txtCategoryCode.Text == "99")
                 || (IsCollectCategory && txtCategoryCode.Text == "00"))
            {
                BaseContext.SetFunction03Enabled(false);
            }
            else
            {
                BaseContext.SetFunction03Enabled(true);
            }
        }
        #endregion

        #region 画面の F10
        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.Close();
        }
        #endregion

        /// <summary>設定処理</summary>
        private async Task ExistBillingCategory()
        {
            int id = int.Parse(HiddenId);
            int deleteCount = 0;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<BillingServiceClient>();
                ExistResult billingResult = await service.ExistBillingCategoryAsync(SessionKey, id);
                deleteCount = billingResult.Exist ? 1 : 0;
                if (deleteCount > 0)
                {
                    cbxUseLongTermAdvanceReceived.Enabled = false;
                }
                else
                {
                    cbxUseLongTermAdvanceReceived.Enabled = true;
                }
            });
        }

        private void btnAccountTitleCode_Click(object sender, EventArgs e)
        {
            var accountTitleResult = this.ShowAccountTitleSearchDialog();
            if (accountTitleResult != null)
            {
                SetAccountTitle(accountTitleResult.Code, accountTitleResult.Name, accountTitleResult.Id);
            }
        }

        private void SetAccountTitle(string code, string name, int id)
        {
            txtAccountTitleCode.Text = code;
            lblAccountTitle.Text = name;
            AccountTitleId = id.ToString();
            ClearStatusMessage();
        }

        private void btnPaymentAgencyId_Click(object sender, EventArgs e)
        {
            var paymentAgency = this.ShowPaymentAgencySearchDialog();
            if (paymentAgency != null)
            {
                SetPaymentAgency(paymentAgency.Code, paymentAgency.Name, paymentAgency.Id);
            }
        }

        private void SetPaymentAgency(string code, string name, int id)
        {
            txtPaymentAgencyCode.Text = code;
            lblPaymentAgencyName.Text = name;
            PaymentAgencyId = id.ToString();
            ClearStatusMessage();
        }

        private async Task RetrieveCategoryData()
        {
            int categoryType = cmbCategoryType.SelectedIndex + 1;
            var result =  await ServiceProxyFactory.DoAsync(async (CategoryMasterClient client) => 
                await client.GetByCodeAsync(SessionKey, CompanyId,
                    categoryType, new[] { txtCategoryCode.Text }));
            var category = result.Categories?.FirstOrDefault();
            if (category == null)
            {
                DispStatusMessage(MsgInfSaveNewData, "区分");
                BaseContext.SetFunction03Enabled(false);
                return;
            }

            txtCategoryCode.Enabled = false;
            AccountTitleId = category.AccountTitleId.ToString();
            PaymentAgencyId = category.PaymentAgencyId.ToString();
            txtCategoryName.Text = category.Name;
            txtAccountTitleCode.Text = category.AccountTitleCode;
            lblAccountTitle.Text = category.AccountTitleName;
            txtCategorySubCode.Text = category.SubCode;
            cmbTaxClassName.SelectedValue = category.TaxClassId + " : " + category.TaxClassName;
            txtMatchingOrder.Text = category.MatchingOrder.ToString();
            txtExternalCode.Text = category.ExternalCode;
            txtExternalCode.Enabled = (!(IsReceiptCategory && string.CompareOrdinal(category.Code, "98") >= 0));
            txtNote.Text = category.Note;
            txtPaymentAgencyCode.Text = category.PaymentAgencyCode;
            lblPaymentAgencyName.Text = category.PaymentAgencyName;
            HiddenId = category.Id.ToString();

            if (category.ForceMatchingIndividually == 1)
            {
                cbxForeceMatchingIndividually.Checked = true;
            }
            else
            {
                cbxForeceMatchingIndividually.Checked = false;
            }

            if (category.UseInput == 1)
            {
                cbxUseInput.Checked = true;
            }
            else
            {
                cbxUseInput.Checked = false;
            }

            if ((category.UseLimitDate == 1 && IsReceiptCategory)
                || (category.UseLimitDate == 1 && IsCollectCategory)
                || (category.UseLongTermAdvanceReceived == 1 && IsBillingCategory))
            {
                cbxUseLongTermAdvanceReceived.Checked = true;
            }
            else
            {
                cbxUseLongTermAdvanceReceived.Checked = false;
            }

            if (category.UseCashOnDueDates == 1
                || category.UseAccountTransfer == 1
                || category.UseDiscount == 1)
            {
                cbxUseDiscount.Checked = true;
                btnPaymentAgency.Enabled = true;
                txtPaymentAgencyCode.Enabled = true;
            }
            else
            {
                cbxUseDiscount.Checked = false;
                btnPaymentAgency.Enabled = false;
                txtPaymentAgencyCode.Enabled = false;
            }

            DeleteConstraint();
            if (IsBillingCategory)
            {
                await ExistBillingCategory();
            }

            Modified = false;
            ClearStatusMessage();

        }

        private void SetFormat()
        {
            if (ApplicationControl == null) return;
            var expression = new DataExpression(ApplicationControl);
            txtAccountTitleCode.MaxLength = expression.AccountTitleCodeLength;
            txtAccountTitleCode.Format = expression.AccountTitleCodeFormatString;
            txtPaymentAgencyCode.PaddingChar = '0';
            txtCategoryCode.PaddingChar = '0';
            if (ApplicationControl.AccountTitleCodeType == 0)
            {
                txtAccountTitleCode.PaddingChar = '0';
            }
        }

        private void cbxUseDiscount_Click(object sender, EventArgs e)
        {
            if (cbxUseDiscount.Checked)
            {
                btnPaymentAgency.Enabled = true;
                txtPaymentAgencyCode.Enabled = true;
            }
            else
            {
                txtPaymentAgencyCode.Clear();
                lblPaymentAgencyName.Clear();
                PaymentAgencyId = null;
                btnPaymentAgency.Enabled = false;
                txtPaymentAgencyCode.Enabled = false;
            }
        }

        private void cbxUseLongTermAdvanceReceived_CheckedChanged(object sender, EventArgs e)
        {
            if (IsReceiptCategory && cbxUseLongTermAdvanceReceived.Checked)
            {
                cbxUseDiscount.Enabled = true;
            }
            else if (IsReceiptCategory && cbxUseLongTermAdvanceReceived.Checked == false)
            {
                cbxUseDiscount.Enabled = false;
                cbxUseDiscount.Checked = false;
            }
        }

        private void cmbTaxClassName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTaxClassName.SelectedIndex != -1)
            {
                TaxClass selected = cmbTaxClassName.Items[cmbTaxClassName.SelectedIndex].Tag as TaxClass;
                TaxClass = selected.Id.ToString();
            }
        }

        private void PB0901_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeCategoryGrid();
                SetScreenName();
                cmbCategoryType.Select();

                var loadTask = new List<Task>();

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }

                loadTask.Add(LoadControlColorAsync());
                loadTask.Add(TaxClassComboData());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                //区分識別 の　コンボボックス
                AddCategoryItems();

                SelectCombo();
                SetFormat();
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #region 入力項目変更イベント処理
        /// <summary>入力項目変更イベント処理</summary>
        private void CheckEditControl()
        {
            foreach (Control control in gbxCategoryInput.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }

                if (control is Common.Controls.VOneComboControl)
                {
                    ((Common.Controls.VOneComboControl)control).SelectedIndexChanged +=
                        new EventHandler(OnContentChanged);
                }

                if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged +=
                        new EventHandler(OnContentChanged);
                }
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            if (!Modified)
            {
                Modified = true;
            }
        }
        #endregion

        private void txtAccountTitleCode_Validated(object sender, EventArgs e)
        {
            try
            {
                AccountTitleId = null;
                if (!string.IsNullOrWhiteSpace(txtAccountTitleCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<AccountTitleMasterClient>();
                        AccountTitlesResult result = await service.GetByCodeAsync(SessionKey, CompanyId,
                            new string[] { txtAccountTitleCode.Text });

                        if (result.ProcessResult.Result)
                        {
                            var accountTitleResult = new AccountTitle();
                            accountTitleResult = result.AccountTitles.FirstOrDefault();
                            if (accountTitleResult == null)
                            {
                                ShowWarningDialog(MsgWngMasterNotExist, "科目", txtAccountTitleCode.Text);
                                txtAccountTitleCode.Clear();
                                lblAccountTitle.Clear();
                            }
                            else
                            {
                                ClearStatusMessage();
                                accountTitleResult = result.AccountTitles.FirstOrDefault();
                                txtAccountTitleCode.Text = accountTitleResult.Code;
                                lblAccountTitle.Text = accountTitleResult.Name;
                                AccountTitleId = accountTitleResult.Id.ToString();
                            }
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    if (string.IsNullOrWhiteSpace(txtAccountTitleCode.Text))
                    {
                        txtAccountTitleCode.Focus();
                    }
                }
                else
                {
                    lblAccountTitle.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtPaymentAgencyCode_Validated(object sender, EventArgs e)
        {
            try
            {
                PaymentAgencyId = null;
                if (!string.IsNullOrWhiteSpace(txtPaymentAgencyCode.Text))
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<PaymentAgencyMasterClient>();
                        PaymentAgenciesResult result = await service.GetByCodeAsync(SessionKey, CompanyId,
                            new string[] { txtPaymentAgencyCode.Text });

                        if (result.ProcessResult.Result)
                        {
                            var paymentAgencyResult = new PaymentAgency();
                            paymentAgencyResult = result.PaymentAgencies.FirstOrDefault();
                            if (paymentAgencyResult == null)
                            {
                                ShowWarningDialog(MsgWngMasterNotExist, "決済代行会社", txtPaymentAgencyCode.Text);
                                txtPaymentAgencyCode.Clear();
                                lblPaymentAgencyName.Clear();
                            }
                            else
                            {
                                ClearStatusMessage();
                                paymentAgencyResult = result.PaymentAgencies.FirstOrDefault();
                                lblPaymentAgencyName.Text = paymentAgencyResult.Name;
                                PaymentAgencyId = paymentAgencyResult.Id.ToString();
                            }
                        }
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (string.IsNullOrWhiteSpace(txtPaymentAgencyCode.Text))
                    {
                        txtPaymentAgencyCode.Focus();
                    }
                }
                else
                {
                    txtPaymentAgencyCode.Clear();
                    lblPaymentAgencyName.Clear();
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void txtCategoryCode_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCategoryCode.Text))
                {
                    if (cmbCategoryType.SelectedIndex == -1)
                    {
                        ShowWarningDialog(MsgWngSelectionRequired, "区分識別");
                        txtCategoryCode.Clear();
                        cmbCategoryType.Focus();
                    }
                    else if (!string.IsNullOrWhiteSpace(txtCategoryCode.Text))
                    {
                        ProgressDialog.Start(ParentForm, RetrieveCategoryData(), false, SessionKey);
                    }
                    else
                    {
                        txtCategoryCode.Clear();
                        txtCategoryName.Clear();
                    }
                }
                else
                {
                    Modified = false;
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
    }
}
