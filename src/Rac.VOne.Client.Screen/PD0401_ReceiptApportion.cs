using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.CategoryMasterService;
using Rac.VOne.Client.Screen.CollationSettingMasterService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.CustomerMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Client.Screen.SectionMasterService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金データ振分</summary>
    public partial class PD0401 : VOneScreenBase
    {
        public Func<IEnumerable<ITransactionData>, bool> SavePostProcessor { get; set; }
        public Func<IEnumerable<ITransactionData>, bool> ExcludePostProcessor { get; set; }

        /// <summary>得意先入力必須</summary>
        private bool RequiredCustomer { get { return CollationSetting?.RequiredCustomer == 1; } }
        /// <summary>入金部門入力必須</summary>
        private bool RequiredSection { get { return ApplicationControl?.UseReceiptSection == 1; } }
        /// <summary>得意先・入金部門 いずれか入力必須</summary>
        private bool RequiredAny { get { return RequiredCustomer || RequiredSection; } }
        /// <summary>得意先自動設定</summary>
        private bool DoAssignCustomer { get { return CollationSetting?.AutoAssignCustomer == 1; } }
        private CollationSetting CollationSetting { get; set; }
        private List<Category> CategoryList { get; set; }
        private List<Web.Models.Section> SectionList { get; set; }
        private List<ReceiptHeader> ReceiptHeaderList { get; set; }
        private List<ReceiptApportion> ReceiptApportionList { get; set; }
        private List<ReceiptApportion> UpdateReceiptDetailData { get; set; } = new List<ReceiptApportion>();
        private int SelectedRowIndex { get; set; } = -1;
        private long SelectedReceiptHeaderId { get; set; } = 0L;
        private CellEventArgs CellEventArgs { get; set; } = null;
        private bool ValueChange { get; set; } = false;
        private int PrecisionLength { get; set; } = 0;

        public PD0401()
        {
            InitializeComponent();
            grdImportHistory.SetupShortcutKeys();
            grdReceiptDetails.SetupShortcutKeys();
            Text = "入金データ振分";
        }

        private void ValidateMasterData()
        {
            cbxSectionAssign.Visible = false;
            if (ApplicationControl?.UseReceiptSection == 1)
            {
                cbxSectionAssign.Visible = true;
                GetSection();
                if (!(SectionList?.Any() ?? false))
                {
                    ShowWarningDialog(MsgWngNotSettingMaster, "入金部門マスター");
                }
            }

            if (!(CategoryList?.Any() ?? false))
            {
                ShowWarningDialog(MsgWngNotSettingMaster, "対象外区分マスター");
            }
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = SaveApportion;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = ClearFormData;

            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction03Enabled(false);

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = PrintApportion;

            BaseContext.SetFunction05Caption("一括削除");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = DeleteAll;

            BaseContext.SetFunction06Caption("");
            BaseContext.SetFunction06Enabled(false);
            BaseContext.SetFunction07Caption("");
            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Caption("");
            BaseContext.SetFunction08Enabled(false);

            BaseContext.SetFunction09Caption("一括振分");
            BaseContext.SetFunction09Enabled(true);
            OnF09ClickHandler = ApportionAll;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = CloseForm;
        }

        private void PD0401_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                BaseForm.Shown += (sender2, e2) => ValidateMasterData();

                var loadTask = new List<Task>();
                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }
                if (CollationSetting == null)
                {
                    loadTask.Add(GetCollationSettingAsync());
                }
                if (CategoryList == null)
                {
                    loadTask.Add(GetCategoryAsync());
                }
                if (ReceiptHeaderList == null)
                {
                    loadTask.Add(GetReceiptHeaderAsync());
                }
                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }
                loadTask.Add(LoadControlColorAsync());

                if (Authorities == null)
                {
                    loadTask.Add(LoadFunctionAuthorities(FunctionType.ModifyReceipt));
                }

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                InitializeGrid();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        #region Initialize Grid Setting
        private void InitializeGrid()
        {
            InitializeGridTemplate();
            BaseContext.SetFunction01Enabled(false);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);

            if (ReceiptHeaderList == null || !ReceiptHeaderList.Any())
            {
                BaseContext.SetFunction09Enabled(false);
            }
            else
            {
                grdImportHistory.DataSource = new BindingSource(ReceiptHeaderList, null);
            }
        }

        private void InitializeGridTemplate()
        {
            #region 取込履歴グリット
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;

            var widthCcy = UseForeignCurrency ? 90 : 0;
            var widthSecCd = UseSection ? 95 : 0;
            var widthSecBt = UseSection ? 35 : 0;
            var widthSecNm = UseSection ? 130 : 0;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, Width =       40, Name = "Header"             , cell: builder.GetRowHeaderCell() ),
                new CellSetting(height, Width =       60, Name = "btnSelect"                                                              , caption: "選択", cell: builder.GetButtonCell(), value: "…"),
                new CellSetting(height, Width =      120, Name = "BankName"           , dataField: nameof(ReceiptHeader.BankName)         , caption: "銀行"  ),
                new CellSetting(height, Width =      120, Name = "BranchName"         , dataField: nameof(ReceiptHeader.BranchName)       , caption: "支店"  ),
                new CellSetting(height, Width =       80, Name = "BankAccountTypeName", dataField: nameof(ReceiptHeader.AccountTypeName)  , caption: "預金種目", cell: builder.GetTextBoxCell(middleCenter)),
                new CellSetting(height, Width =      100, Name = "AccountNumber"      , dataField: nameof(ReceiptHeader.AccountNumber)    , caption: "口座番号", cell: builder.GetTextBoxCell(middleCenter)),
                new CellSetting(height, Width =      170, Name = "Createdate"         , dataField: nameof(ReceiptHeader.CreateAt)         , caption: "取込日時", cell: builder.GetDateCell_yyyyMMddHHmmss()),
                new CellSetting(height, Width =      120, Name = "Category"                                                               , caption: "対象外区分", cell: CategoryComboBoxCell(builder, true), readOnly: false ),
                new CellSetting(height, Width = widthCcy, Name = "Currency"           , dataField: nameof(ReceiptHeader.CurrencyCode)     , caption: "通貨コード", cell: builder.GetTextBoxCell(middleCenter)),
                new CellSetting(height, Width =        0, Name = "ExistApportioned"   , dataField: nameof(ReceiptHeader.ExistApportioned) ),
                new CellSetting(height, Width =        0, Name = "IsAllApportioned"   , dataField: nameof(ReceiptHeader.IsAllApportioned) ),
                new CellSetting(height, Width =        0, Name = "CurrencyId"         , dataField: nameof(ReceiptHeader.CurrencyId) ),
                new CellSetting(height, Width =        0, Name = "ReceiptHeaderId"    , dataField: nameof(ReceiptHeader.Id) )
            });
            grdImportHistory.Template = builder.Build();
            #endregion

            #region 入金明細グリット
            var builder1 = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var template = new Template();

            #region header
            builder1.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, Width =         40, Name = "Header"),
                new CellSetting(height, Width =         60, Name = "ReceiptExcludeFlag"         , caption: "対象外", sortable: true),
                new CellSetting(height, Width =        120, Name = "ExcludeCategoryName"        , caption: "対象外区分", sortable: true),
                new CellSetting(height, Width =        120, Name = "ExcludeAmount"              , caption: "対象外金額", sortable: true),
                new CellSetting(height, Width = widthSecNm, Name = "SectionCode"                , caption: "入金部門コード", sortable: true),
                new CellSetting(height, Width = widthSecNm, Name = "SectionName"                , caption: "入金部門名", sortable: true),
                new CellSetting(height, Width =        130, Name = "CustomerCode"               , caption: "得意先コード", sortable: true),
                new CellSetting(height, Width =        130, Name = "CustomerName"               , caption: "得意先名", sortable: true ),
                new CellSetting(height, Width =        130, Name = "PayerName"                  , caption: "振込依頼人名", sortable: true),
                new CellSetting(height, Width =   widthCcy, Name = "CurencyCode"                , caption: "通貨コード", sortable: true),
                new CellSetting(height, Width =        120, Name = "ReceiptAmount"              , caption: "入金額", sortable: true),
                new CellSetting(height, Width =        115, Name = "RecordTime"                 , caption: "入金日", sortable: true),
                new CellSetting(height, Width =        115, Name = "WorkDay"                    , caption: "作成日", sortable: true   ),
                new CellSetting(height, Width =        120, Name = "SourceBankName"             , caption: "仕向銀行", sortable: true),
                new CellSetting(height, Width =        120, Name = "SourceBranchName"           , caption: "仕向支店", sortable: true ),
                new CellSetting(height, Width =        120, Name = "PayerCode1"                 , caption: "仮想支店コード", sortable: true),
                new CellSetting(height, Width =        120, Name = "PayerCode2"                 , caption: "仮想口座番号", sortable: true),
                new CellSetting(height, Width = widthSecNm, Name = "ReferenceCustomerCode"      , caption: "参照得意先コード", sortable: true),
                new CellSetting(height, Width = widthSecNm, Name = "ReferenceCustomerName"      , caption: "参照得意先名", sortable: true)
            });
            builder1.BuildHeaderOnly(template);
            builder1.Items.Clear();
            #endregion

            var numberCell = builder1.GetNumberCellCurrencyInput(PrecisionLength, PrecisionLength, 0);
            numberCell.MaxValue = 99999999999.99999M;
            numberCell.MinValue = 0M;

            var expression = new DataExpression(ApplicationControl);
            Func<GcTextBoxCell> getCodeCell = () =>
            {
                var cell = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter);
                cell.AcceptsCrLf = GrapeCity.Win.Editors.CrLfMode.Filter;
                cell.AcceptsTabChar = GrapeCity.Win.Editors.TabCharMode.Filter;
                cell.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
                return cell;
            };
            var sectionCodeCell = getCodeCell();
            sectionCodeCell.Format = expression.SectionCodeFormatString;
            sectionCodeCell.MaxLength = expression.SectionCodeLength;
            sectionCodeCell.Style.ImeMode = ImeMode.Disable;

            var customerCodeCell = getCodeCell();
            customerCodeCell.Format = expression.CustomerCodeFormatString;
            customerCodeCell.MaxLength = expression.CustomerCodeLength;
            customerCodeCell.Style.ImeMode = ApplicationControl.CustomerCodeType == 2 ? ImeMode.KatakanaHalf : ImeMode.Disable;

            #region row
            builder1.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height, Width =         40, Name = "Header"                , cell: builder1.GetRowHeaderCell() ),
                new CellSetting(height, Width =         60, Name = "ReceiptExcludeFlag"    , dataField: nameof(ReceiptApportion.ExcludeFlag)              , cell: builder1.GetCheckBoxCell()             , readOnly: false),
                new CellSetting(height, Width =        120, Name = "ExcludeCategoryName"   , dataField: nameof(ReceiptApportion.ExcludeCategoryId)        , cell: CategoryComboBoxCell(builder, false)   , readOnly: false),
                new CellSetting(height, Width =        120, Name = "ExcludeAmount"         , dataField: nameof(ReceiptApportion.ExcludeAmount)            , cell: numberCell                             , readOnly: false),
                new CellSetting(height, Width = widthSecCd, Name = "SectionCode"           , dataField: nameof(ReceiptApportion.SectionCode)              , cell: sectionCodeCell                        , readOnly: false),
                new CellSetting(height, Width = widthSecBt, Name = "SectionBtn"                                                                           , cell: builder1.GetButtonCell(), value: "…"),
                new CellSetting(height, Width = widthSecNm, Name = "SectionName"           , dataField: nameof(ReceiptApportion.SectionName)              ),
                new CellSetting(height, Width =         95, Name = "CustomerCode"          , dataField: nameof(ReceiptApportion.CustomerCode)             , cell: customerCodeCell, readOnly: false),
                new CellSetting(height, Width =         35, Name = "CustomerBtn"                                                                          , cell:  builder1.GetButtonCell(), value: "…"),
                new CellSetting(height, Width =        130, Name = "CustomerName"          , dataField: nameof(ReceiptApportion.CustomerName)),
                new CellSetting(height, Width =        130, Name = "PayerName"             , dataField: nameof(ReceiptApportion.PayerName)),
                new CellSetting(height, Width =          0, Name = "PayerNameRaw"          , dataField: nameof(ReceiptApportion.PayerNameRaw)),
                new CellSetting(height, Width =   widthCcy, Name = "CurencyCode"           , dataField: nameof(ReceiptApportion.CurrencyCode)             , cell: builder1.GetTextBoxCell(middleCenter)         ),
                new CellSetting(height, Width =        120, Name = "ReceiptAmount"         , dataField: nameof(ReceiptApportion.ReceiptAmount)            , cell: builder1.GetNumberCellCurrency(PrecisionLength, PrecisionLength, 0)    ),
                new CellSetting(height, Width =        115, Name = "RecordTime"            , dataField: nameof(ReceiptApportion.RecordedAt)               , cell: builder1.GetDateCell_yyyyMMdd()   ),
                new CellSetting(height, Width =        115, Name = "WorkDay"               , dataField: nameof(ReceiptApportion.Workday)                  , cell: builder1.GetDateCell_yyyyMMdd()   ),
                new CellSetting(height, Width =        120, Name = "SourceBankName"        , dataField: nameof(ReceiptApportion.SourceBankName)                      ),
                new CellSetting(height, Width =        120, Name = "SourceBranchName"      , dataField: nameof(ReceiptApportion.SourceBranchName)                    ),
                new CellSetting(height, Width =        120, Name = "PayerCode1"            , dataField: nameof(ReceiptApportion.ExcludeVirtualBranchCode) , cell: builder1.GetTextBoxCell(middleCenter)         ),
                new CellSetting(height, Width =        120, Name = "PayerCode2"            , dataField: nameof(ReceiptApportion.ExcludeAccountNumber)     , cell: builder1.GetTextBoxCell(middleCenter)         ),
                new CellSetting(height, Width = widthSecNm, Name = "ReferenceCustomerCode" , dataField: nameof(ReceiptApportion.RefCustomerCode)          , cell: builder1.GetTextBoxCell(middleCenter)         ),
                new CellSetting(height, Width = widthSecNm, Name = "ReferenceCustomerName" , dataField: nameof(ReceiptApportion.RefCustomerName)                     ),
                new CellSetting(height, Width =          0, Name = "Apportioned"           , dataField: nameof(ReceiptApportion.Apportioned) ),
                new CellSetting(height, Width =          0, Name = "Id"                    , dataField: nameof(ReceiptApportion.Id) ),
                new CellSetting(height, Width =          0, Name = "CustomerId"            , dataField: nameof(ReceiptApportion.CustomerId) ),
                new CellSetting(height, Width =          0, Name = "SectionId"             , dataField: nameof(ReceiptApportion.SectionId) ),
                new CellSetting(height, Width =          0, Name = "UpdateAt"              , dataField: nameof(ReceiptApportion.UpdateAt) ),
                new CellSetting(height, Width =          0, Name = "CurrencyId"            , dataField: nameof(ReceiptApportion.CurrencyId ))
            });
            builder1.BuildRowOnly(template);
            #endregion
            grdReceiptDetails.Template = template;
            #endregion
        }

        private Cell CategoryComboBoxCell(GcMultiRowTemplateBuilder builder, bool isHistory)
        {
            var comboBoxCell = builder.GetComboBoxCell();
            var dataTable = new DataTable("LIST");
            dataTable.Columns.Add("ExcludeCategoryId", typeof(int));
            dataTable.Columns.Add("Category", typeof(string));
            if (CategoryList != null)
            {
                if (isHistory) dataTable.Rows.Add(0, "");
                foreach (Category value in CategoryList)
                {
                    dataTable.Rows.Add(value.Id, value.Code + ":" + value.Name);
                    dataTable.AcceptChanges();
                }
            }
            comboBoxCell.DataSource = dataTable;
            comboBoxCell.DisplayMember = "Category";
            comboBoxCell.ValueMember = "ExcludeCategoryId";
            return comboBoxCell;
        }
        #endregion

        #region Webサービス呼び出し
        private async Task GetCollationSettingAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CollationSettingMasterClient>();
                CollationSettingResult result = await service.GetAsync(
                SessionKey, CompanyId);

                if (result.ProcessResult.Result)
                {
                    CollationSetting = result.CollationSetting;
                }
            });
        }

        private async Task GetCategoryAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CategoryMasterClient>();
                var categorySearch = new CategorySearch();
                categorySearch.CompanyId = CompanyId;
                categorySearch.CategoryType = 4;

                CategoriesResult result = await service.GetItemsAsync(
                    SessionKey, categorySearch);

                if (result.ProcessResult.Result)
                {
                    CategoryList = result.Categories;
                }
            });
        }

        private async Task GetReceiptHeaderAsync()
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReceiptServiceClient>();
                ReceiptHeadersResult result = await service.GetHeaderItemsAsync(
                SessionKey, CompanyId);
                service.Close();
                if (result.ProcessResult.Result)
                {
                    ReceiptHeaderList = null;
                    ReceiptHeaderList = new List<ReceiptHeader>();
                    ReceiptHeaderList = result.ReceiptHeaders;
                }
            });
        }

        private void GetSection()
        {
            SectionsResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, Code: null);
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (result.ProcessResult.Result)
            {
                SectionList = result.Sections;
            }
        }

        private async Task<Customer> GetCustomerName(string code)
        {
            Customer customerName = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CustomerMasterClient>();
                CustomersResult result = await service.GetByCodeAsync(SessionKey,
                    CompanyId, new string[] { code });

                if (result.ProcessResult.Result && result.Customers.Any(x => x != null))
                {
                    customerName = result.Customers[0];
                }
            });
            return customerName;
        }

        private async Task<Web.Models.Section> GetSectionName(string code)
        {
            Web.Models.Section section = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                SectionsResult result = await service.GetByCodeAsync(SessionKey,
                    CompanyId, new[] { code });

                if (result.ProcessResult.Result && result.Sections.Any(x => x != null))
                {
                    section = result.Sections[0];
                }
            });
            return section;
        }

        private async Task<int> GetCurrencyPrecision(int id)
        {
            int length = 0;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                CurrenciesResult result = await service.GetAsync(SessionKey, new[] { id });
                service.Close();
                if (result.Currencies.Any())
                {
                    length = result.Currencies[0].Precision;
                }
            });
            return length;
        }

        private void GetReceiptData(long[] id)
        {
            ReceiptApportionsResult result = null;
            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReceiptServiceClient>();
                result = await service.GetApportionItemsAsync(SessionKey, id);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (result.ProcessResult.Result)
            {
                ReceiptApportionList = result.ReceiptApportion;
            }
        }

        private async Task SaveApportionAsync(IEnumerable<ReceiptApportion> detailData)
        {
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<ReceiptServiceClient>();
                ReceiptApportionsResult result = await service.ApportionAsync(
                    SessionKey, detailData.ToArray());

                ReceiptApportionList = null;
                if (result.ProcessResult.Result)
                {
                    if (result.ReceiptApportion != null && result.ReceiptApportion.Any())
                    {
                        ReceiptApportionList = result.ReceiptApportion;
                    }
                }
            });
        }

        #endregion

        #region Function Key Event
        [OperationLog("登録")]
        private void SaveApportion()
        {
            try
            {
                ClearStatusMessage();
                var items = ValidateInputValues();

                if (!items.Any()) return;

                var updateItems = items.ToList();

                var loadTask = new List<Task>();
                loadTask.Add(SaveApportionAsync(updateItems).ContinueWith(t =>
                {
                    if (ReceiptApportionList != null && ReceiptApportionList.Any())
                    {
                        return GetReceiptHeaderAsync();
                    }
                    return Task.CompletedTask;
                }).Unwrap());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                var success = ReceiptApportionList != null;

                var syncResult = true;
                if (success)
                {
                    syncResult = SynchronizeData(ReceiptApportionList);
                }
                success &= syncResult;

                if (!syncResult)
                {
                    ShowWarningDialog(MsgErrPostProcessFailure);
                    return;
                }

                if (!success)
                {
                    ShowWarningDialog(MsgErrSaveError);
                    return;
                }

                if (success && ReceiptApportionList.Any())
                {
                    grdImportHistory.DataSource = null;
                    grdImportHistory.Refresh();
                    grdImportHistory.DataSource = ReceiptHeaderList;
                    grdImportHistory.Refresh();

                    ReceiptHeader updateData = ReceiptHeaderList.SingleOrDefault(x => x.Id == SelectedReceiptHeaderId);
                    if (updateData != null)
                    {
                        grdImportHistory.Rows[CellEventArgs.RowIndex].Cells["celbtnSelect"].Selected = true;
                        grdImportHistory_CellContentButtonClick(this, CellEventArgs);
                    }
                    else
                    {
                        grdImportHistory.ClearSelection();
                        ReceiptApportionList = null;
                        grdReceiptDetails.DataSource = ReceiptApportionList;
                        grdReceiptDetails.Refresh();
                    }

                    DispStatusMessage(MsgInfSaveSuccess);
                    ValueChange = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>データ同期処理 同期のFunc デリゲート未実装時は true を返す</summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private bool SynchronizeData(IEnumerable<ReceiptApportion> entities)
        {
            if (SavePostProcessor == null || ExcludePostProcessor == null) return true;

            var syncResult = true;
            var excludes = ReceiptApportionList
                .SelectMany(x => x.ReceiptExcludes.Select(y => y as ITransactionData));
            if (excludes?.Any(x => x != null) ?? false)
            {
                syncResult = ExcludePostProcessor.Invoke(excludes);
            }

            if (syncResult)
            {
                syncResult = SavePostProcessor.Invoke(ReceiptApportionList.Select(x => x as ITransactionData));
            }
            return syncResult;
        }

        /// <summary>振分前の検証処理</summary>
        /// <returns>振分処理を行う <see cref="ReceiptApportion"/></returns>
        /// <remarks>
        /// </remarks>
        private IEnumerable<ReceiptApportion> ValidateInputValues()
        {

            grdReceiptDetails.EndEdit();

            IEnumerable<ReceiptApportion> result = Enumerable.Empty<ReceiptApportion>();

            Func<Row, bool> isApportioned = (row)
                => Convert.ToInt32(row["celApportioned"].Value) == 1;

            Func<Row, bool> isExclude = (row)
                => Convert.ToBoolean(row["celReceiptExcludeFlag"].Value);

            Func<Row, bool> isExcludeCategoryEmpty = (row)
                => string.IsNullOrEmpty(Convert.ToString(row["celExcludeCategoryName"].Value));

            Func<Row, bool> isCustomerEmpty = (row)
                => string.IsNullOrEmpty(Convert.ToString(row["celCustomerCode"].Value));

            Func<Row, bool> isSectionEmpty = (row)
                => string.IsNullOrEmpty(Convert.ToString(row["celSectionCode"].Value));

            Func<Row, bool> isRowEmpty = (row)
                => !isExclude(row)
                && isCustomerEmpty(row)
                && (!RequiredSection || isSectionEmpty(row));

            var targets = grdReceiptDetails.Rows.Where(x
                => !isApportioned(x)
                    && (!RequiredAny || !isRowEmpty(x)));

            Action<Row, string, string> invalidHandler = (row, id, arg1) =>
            {
                ShowWarningDialog(id, arg1);
                grdReceiptDetails.ClearSelection();
                grdReceiptDetails.Rows[row.Index].Selected = true;
            };

            foreach (var row in targets)
            {
                if (isExclude(row))
                {
                    if (isExcludeCategoryEmpty(row))
                    {
                        invalidHandler(row, MsgWngSelectionRequired, "対象外区分");
                        return result;
                    }
                    if (Convert.ToDecimal(row["celExcludeAmount"].Value) == 0M)
                    {
                        invalidHandler(row, MsgWngInputRequired, "対象外金額");
                        return result;
                    }
                    if (Convert.ToDecimal(row["celReceiptAmount"].Value) < Convert.ToDecimal(row["celExcludeAmount"].Value))
                    {
                        invalidHandler(row, MsgWngInputRequired, "入金額以下の「対象外金額」");
                        return result;
                    }
                }
                if (RequiredSection && isSectionEmpty(row))
                {
                    invalidHandler(row, MsgWngInputRequired, "入金部門コード");
                    return result;
                }
                if (RequiredCustomer && isCustomerEmpty(row))
                {
                    invalidHandler(row, MsgWngInputRequired, "得意先コード");
                    return result;
                }
            }

            if (!targets.Any())
            {
                if (RequiredSection)
                {
                    var row = grdReceiptDetails.Rows.FirstOrDefault(x => !isApportioned(x) && isSectionEmpty(x));
                    if (row != null)
                    {
                        invalidHandler(row, MsgWngInputRequired, "入金部門コード");
                        return result;
                    }
                }
                if (RequiredCustomer)
                {
                    var row = grdReceiptDetails.Rows.FirstOrDefault(x => !isApportioned(x) && isCustomerEmpty(x));
                    if (row != null)
                    {
                        invalidHandler(row, MsgWngInputRequired, "得意先コード");
                        return result;
                    }
                }
            }

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return result;
            }
            result = targets.Select(x => ConvertModel(x));
            return result;
        }

        private ReceiptApportion ConvertModel(Row row, bool doDelete = false)
        {
            var receiptApportion = new ReceiptApportion();
            receiptApportion.ReceiptHeaderId = SelectedReceiptHeaderId;
            receiptApportion.Id = Convert.ToInt64(row.Cells["celId"].Value);
            receiptApportion.CompanyId = CompanyId;
            receiptApportion.CurrencyId = Convert.ToInt32(row.Cells["celCurrencyId"].Value);
            receiptApportion.UpdateAt = Convert.ToDateTime(row.Cells["celUpdateAt"].Value);
            receiptApportion.UpdateBy = Login.UserId;
            receiptApportion.Apportioned = 1;
            receiptApportion.PayerName = Convert.ToString(row.Cells["celPayerName"].Value);
            receiptApportion.SourceBankName = Convert.ToString(row.Cells["celSourceBankName"].Value);
            receiptApportion.SourceBranchName = Convert.ToString(row.Cells["celSourceBranchName"].Value);

            if (row.Cells["celCustomerId"].Value != null)
            {
                receiptApportion.CustomerId = Convert.ToInt32(row.Cells["celCustomerId"].Value);
                receiptApportion.CustomerCode = Convert.ToString(row.Cells["celCustomerCode"].Value);
            }
            if (row.Cells["celSectionId"].Value != null)
            {
                receiptApportion.SectionId = Convert.ToInt32(row.Cells["celSectionId"].Value);
            }
            receiptApportion.ExcludeFlag = Convert.ToInt32(row.Cells["celReceiptExcludeFlag"].Value);
            if (row.Cells["celExcludeCategoryName"].Value != null)
            {
                receiptApportion.ExcludeCategoryId = Convert.ToInt32(row.Cells["celExcludeCategoryName"].Value);
            }
            receiptApportion.ExcludeAmount = Convert.ToDecimal(row.Cells["celExcludeAmount"].Value);
            receiptApportion.UpdateBy = Login.UserId;
            receiptApportion.UpdateAt = Convert.ToDateTime(row.Cells["celUpdateAt"].Value);
            if (!doDelete)
            {
                receiptApportion.DoDelete = 0;
                receiptApportion.LearnIgnoreKana = (cbxLearnIgnoreKana.Checked) ? 1 : 0;
                receiptApportion.PayerNameRaw = row.Cells["celPayerNameRaw"].Value.ToString();
                receiptApportion.LearnKanaHistory = CollationSetting.LearnKanaHistory;
            }
            else
            {
                receiptApportion.DoDelete = 1;
                receiptApportion.LearnIgnoreKana = 0;
                receiptApportion.LearnKanaHistory = 0;
            }
            return receiptApportion;
        }

        private void GetDetailModel(List<int> rowNormal, string funName)
        {
            foreach (int rowIndex in rowNormal)
            {
                var row = grdReceiptDetails.Rows[rowIndex];
                var receiptData = new ReceiptApportion();
                receiptData.ReceiptHeaderId = SelectedReceiptHeaderId;
                receiptData.Id = Convert.ToInt64(row.Cells["celId"].Value);
                receiptData.CompanyId = CompanyId;
                receiptData.CurrencyId = Convert.ToInt32(row.Cells["celCurrencyId"].Value);
                receiptData.UpdateAt = Convert.ToDateTime(row.Cells["celUpdateAt"].Value);
                receiptData.UpdateBy = Login.UserId;
                receiptData.Apportioned = 1;

                if (row.Cells["celCustomerId"].Value != null)
                {
                    receiptData.CustomerId = Convert.ToInt32(row.Cells["celCustomerId"].Value);
                }
                if (row.Cells["celSectionId"].Value != null)
                {
                    receiptData.SectionId = Convert.ToInt32(row.Cells["celSectionId"].Value);
                }
                receiptData.ExcludeFlag = Convert.ToInt32(row.Cells["celReceiptExcludeFlag"].Value);
                if (row.Cells["celExcludeCategoryName"].Value != null)
                {
                    receiptData.ExcludeCategoryId = Convert.ToInt32(row.Cells["celExcludeCategoryName"].Value);
                }
                receiptData.ExcludeAmount = Convert.ToDecimal(row.Cells["celExcludeAmount"].Value);
                receiptData.UpdateBy = Login.UserId;
                receiptData.UpdateAt = Convert.ToDateTime(row.Cells["celUpdateAt"].Value);

                receiptData.DoDelete = 1;
                receiptData.LearnIgnoreKana = 0;

                if (funName == "振分")
                {
                    receiptData.DoDelete = 0;
                    receiptData.LearnIgnoreKana = (cbxLearnIgnoreKana.Checked) ? 1 : 0;
                    receiptData.PayerNameRaw = row.Cells["celPayerName"].Value.ToString();
                }

                UpdateReceiptDetailData.Add(receiptData);
            }
        }

        [OperationLog("クリア")]
        private void ClearFormData()
        {
            if (ValueChange && !ShowConfirmDialog(MsgQstConfirmClear)) return;
            ClearInputValues();
        }

        private void ClearInputValues()
        {
            grdImportHistory.ClearSelection();
            if (CellEventArgs != null && CellEventArgs.RowIndex != -1
                && grdImportHistory.RowCount > CellEventArgs.RowIndex)
            {
                grdImportHistory.RemoveSelection(CellEventArgs.RowIndex);
            }

            foreach (Row row in grdImportHistory.Rows)
            {
                grdImportHistory.Rows[row.Index].Cells["celCategory"].Value = null;
            }

            ReceiptApportionList = null;
            grdReceiptDetails.DataSource = ReceiptApportionList;

            BaseContext.SetFunction01Enabled(false);
            BaseContext.SetFunction02Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction05Enabled(false);
            BaseContext.SetFunction09Enabled(true);
            ClearStatusMessage();
            grdImportHistory.Select();
            ValueChange = false;
        }

        [OperationLog("印刷")]
        private void PrintApportion()
        {
            try
            {
                var receiptReport = new ReceiptApportionSectionReport();
                receiptReport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                var receiptHeader = new ReceiptHeader();
                receiptHeader = ReceiptHeaderList.Find(x => x.Id == SelectedReceiptHeaderId);
                string category = grdImportHistory.Rows[SelectedRowIndex].Cells["celCategory"].DisplayText.ToString();
                receiptReport.SetHeaderSetting(receiptHeader, category);
                receiptReport.Name = "入金データ振分リスト" + DateTime.Now.ToString("yyyyMMdd");

                var receiptReportList = new List<ReceiptApportion>();
                if (grdReceiptDetails.RowCount > 0)
                {
                    foreach (Row apportion in grdReceiptDetails.Rows)
                    {
                        var receipt = new ReceiptApportion();
                        receipt.ExcludeFlag = int.Parse(apportion.Cells["celReceiptExcludeFlag"].Value.ToString());
                        if (apportion.Cells["celExcludeCategoryName"].Value != null)
                            receipt.CustomerName = apportion.Cells["celExcludeCategoryName"].DisplayText.ToString();

                        if (apportion.Cells["celExcludeAmount"].Value != null)
                        {
                            receipt.ExcludeAmount = decimal.Parse(apportion.Cells["celExcludeAmount"].Value.ToString());
                            receipt.ExcludeAmount = Math.Round((decimal)receipt.ExcludeAmount, PrecisionLength);
                        }

                        receipt.ReceiptAmount = decimal.Parse(apportion.Cells["celReceiptAmount"].Value.ToString());
                        receipt.ReceiptAmount = Math.Round(receipt.ReceiptAmount, PrecisionLength);
                        receipt.PayerName = apportion.Cells["celPayerName"].Value.ToString();
                        if (ApplicationControl.UseReceiptSection == 1 && apportion.Cells["celSectionCode"].Value != null && apportion.Cells["celSectionCode"].Value.ToString() != "")
                            receipt.SectionName = apportion.Cells["celSectionCode"].Value.ToString() + ":" + apportion.Cells["celSectionName"].Value.ToString();
                        receipt.SourceBankName = apportion.Cells["celSourceBankName"].Value.ToString();
                        receipt.SourceBranchName = apportion.Cells["celSourceBranchName"].Value.ToString();
                        receipt.Workday = Convert.ToDateTime(apportion.Cells["celWorkDay"].Value.ToString());
                        receipt.RecordedAt = Convert.ToDateTime(apportion.Cells["celRecordTime"].Value.ToString());
                        receiptReportList.Add(receipt);
                    }
                    receiptReport.SetPageDataSetting(receiptReportList, ApplicationControl.UseReceiptSection, PrecisionLength);

                    ProgressDialog.Start(ParentForm, Task.Run(() =>
                    {
                        receiptReport.Run(false);

                    }), false, SessionKey);

                    var task = Util.GetGeneralSettingServerPathAsync(Login);
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);
                    var serverPath = task.Result;

                    if (!Directory.Exists(serverPath))
                    {
                        serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }

                    ShowDialogPreview(ParentForm, receiptReport, serverPath);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        [OperationLog("一括削除")]
        private void DeleteAll()
        {
            try
            {
                ClearStatusMessage();

                grdImportHistory.EndEdit();

                if (!ValidateDelete()) return;

                var deleteRow = new List<int>();
                foreach (Row detailsrow in grdReceiptDetails.Rows)
                {
                    deleteRow.Add(detailsrow.Index);
                }

                GetDetailModel(deleteRow, "一括削除");
                if (UpdateReceiptDetailData != null && UpdateReceiptDetailData.Any())
                {
                    var loadTask = new List<Task>();
                    loadTask.Add(SaveApportionAsync(UpdateReceiptDetailData).ContinueWith(t =>
                    {
                        if (ReceiptApportionList != null && ReceiptApportionList.Any())
                        {
                            return GetReceiptHeaderAsync();
                        }
                        return Task.CompletedTask;
                    }));
                    ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                    //一括削除が失敗
                    if (ReceiptApportionList == null)
                    {
                        ShowWarningDialog(MsgErrDeleteError);
                        return;
                    }

                    //一括削除成功
                    grdImportHistory.DataSource = null;
                    grdImportHistory.Refresh();
                    grdImportHistory.DataSource = ReceiptHeaderList;
                    grdImportHistory.Refresh();
                    grdReceiptDetails.DataSource = null;
                    grdReceiptDetails.Refresh();

                    grdImportHistory.ClearSelection();
                    BaseContext.SetFunction01Enabled(false);
                    BaseContext.SetFunction02Enabled(false);
                    BaseContext.SetFunction04Enabled(false);
                    BaseContext.SetFunction05Enabled(false);
                    grdImportHistory.Select();

                    if (ReceiptHeaderList == null || !ReceiptHeaderList.Any())
                    {
                        BaseContext.SetFunction09Enabled(false);
                    }
                    else
                    {
                        BaseContext.SetFunction09Enabled(true);
                    }

                    DispStatusMessage(MsgInfDeleteSuccess);
                    ValueChange = false;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool ValidateDelete()
        {
            grdReceiptDetails.EndEdit();
            if (ApplicationControl?.UseReceiptSection == 1)
            {
                var row = grdReceiptDetails.Rows.Where(x => string.IsNullOrEmpty(Convert.ToString(x["celSectionCode"].Value))).FirstOrDefault();

                if (row != null)
                {
                    row.Selected = true;
                    ShowWarningDialog(MsgWngInputRequired, "入金部門コード");
                    return false;
                }
            }

            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return false;
            }

            return true;
        }

        [OperationLog("一括振分")]
        private void ApportionAll()
        {
            try
            {
                grdImportHistory.EndEdit();

                if (!ShowConfirmDialog(MsgQstConfirmStartXXX, "一括振分"))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var success = true;
                ProgressDialog.Start(ParentForm, async (cancel, progress) =>
                {
                    var updateItems = await PrepareAllApportionItems();
                    if (!updateItems.Any())
                    {
                        success = false;
                        return;
                    }

                    await SaveApportionAsync(updateItems);

                    success = ReceiptApportionList?.Any() ?? false;

                    await GetReceiptHeaderAsync();

                }, false, SessionKey);

                if (!success)
                {
                    ShowWarningDialog(MsgErrSomethingError, "一括振分");
                    return;
                }

                var syncResult = true;
                if (success)
                {
                    syncResult = SynchronizeData(ReceiptApportionList);
                }
                success &= syncResult;

                if (!syncResult)
                {
                    ShowWarningDialog(MsgErrPostProcessFailure);
                    return;
                }

                ClearInputValues();
                grdImportHistory.DataSource = null;
                grdImportHistory.Refresh();
                grdImportHistory.DataSource = ReceiptHeaderList;
                grdImportHistory.Refresh();

                var deletable = ReceiptHeaderList?.Any() ?? false;
                BaseContext.SetFunction09Enabled(deletable);

                DispStatusMessage(MsgInfProcessFinish);
                ValueChange = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("終了")]
        private void CloseForm()
        {
            if (ValueChange && !ShowConfirmDialog(MsgQstConfirmClose))
            {
                return;
            }
            BaseForm.Close();
        }
        #endregion

        #region グリットイベント処理
        private void grdImportHistory_DataBindingComplete(object sender, MultiRowBindingCompleteEventArgs e)
        {
            foreach (Row row in grdImportHistory.Rows)
            {
                int existAppor = Convert.ToInt32(row.Cells["celExistApportioned"].Value);
                if (existAppor == 1)
                {
                    grdImportHistory.Rows[row.Index].Cells["celbtnSelect"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdImportHistory.Rows[row.Index].Cells["celBankName"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdImportHistory.Rows[row.Index].Cells["celBranchName"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdImportHistory.Rows[row.Index].Cells["celBankAccountTypeName"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdImportHistory.Rows[row.Index].Cells["celAccountNumber"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdImportHistory.Rows[row.Index].Cells["celCreatedate"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdImportHistory.Rows[row.Index].Cells["celCategory"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdImportHistory.Rows[row.Index].Cells["celCategory"].Style.DisabledBackColor = Color.LightGoldenrodYellow;
                    grdImportHistory.Rows[row.Index].Cells["celCurrency"].Style.BackColor = Color.LightGoldenrodYellow;
                }

                int allApportion = Convert.ToInt32(row.Cells["celIsAllApportioned"].Value);
                if (allApportion == 1)
                {
                    grdImportHistory.Rows[row.Index].Cells["celCategory"].Enabled = false;
                }
            }
        }

        private void grdImportHistory_CellContentButtonClick(object sender, CellEventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (e.CellName.ToString() == "celbtnSelect")
                {
                    CellEventArgs = e;
                    SelectedRowIndex = e.RowIndex;
                    SelectedReceiptHeaderId = Convert.ToInt64(grdImportHistory.Rows[e.RowIndex].Cells["celReceiptHeaderId"].Value);

                    var receiptHeaderId = new long[] { SelectedReceiptHeaderId };
                    GetReceiptData(receiptHeaderId);

                    if (ReceiptApportionList != null && ReceiptApportionList.Any())
                    {
                        if (cbxSectionAssign.Checked)
                        {
                            SettingForSectionNotShown();
                        }
                        else
                        {
                            SettingForSectionShown();
                        }
                    }
                    BaseContext.SetFunction09Enabled(false);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void SettingForSectionNotShown()
        {
            var notSectionList = new List<ReceiptApportion>();
            notSectionList = ReceiptApportionList.Where(c => c.SectionCode == null)
                                                         .Select(x => x)
                                                         .ToList();
            grdReceiptDetails.DataSource = notSectionList;
            grdReceiptDetails.Refresh();
            SettingForButtonEnable(notSectionList);
        }

        private void SettingForSectionShown()
        {
            Parent.Cursor = Cursors.WaitCursor;
            grdReceiptDetails.DataSource = ReceiptApportionList;
            grdReceiptDetails.Refresh();
            SettingForButtonEnable(ReceiptApportionList);
            Parent.Cursor = Cursors.Default;
        }

        private void SettingForButtonEnable(List<ReceiptApportion> ReceiptList)
        {
            int currencyId = Convert.ToInt32(grdImportHistory.Rows[SelectedRowIndex].Cells["celCurrencyId"].Value);
            Task<int> loadTask = GetCurrencyPrecision(currencyId);
            ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
            PrecisionLength = loadTask.Result;

            if (ReceiptList == null || !ReceiptList.Any())
            {
                BaseContext.SetFunction01Enabled(false);
                BaseContext.SetFunction04Enabled(false);
                BaseContext.SetFunction05Enabled(false);

                BaseContext.SetFunction02Enabled(true);
                ShowWarningDialog(MsgWngNotExistSearchData);
            }
            else
            {
                ReceiptGridFormatting();

                var allApportion = ReceiptList.Select(x => x.Apportioned).ToArray();
                if (!allApportion.Contains(0))
                {
                    BaseContext.SetFunction01Enabled(false);
                    BaseContext.SetFunction05Enabled(false);

                    BaseContext.SetFunction02Enabled(true);
                    BaseContext.SetFunction04Enabled(true);
                }
                else
                {
                    BaseContext.SetFunction01Enabled(true);
                    BaseContext.SetFunction02Enabled(true);
                    BaseContext.SetFunction04Enabled(true);
                    BaseContext.SetFunction05Enabled(false);
                }

                if (allApportion.Count() == ReceiptList.Where(x => x.Apportioned == 0).Count())
                {
                    BaseContext.SetFunction05Enabled(Authorities[FunctionType.ModifyReceipt]);
                }

                ClearStatusMessage();
            }
        }

        private void ReceiptGridFormatting()
        {
            var fieldString = "###,###,###,###";

            if (PrecisionLength > 0)
            {
                fieldString += ".";
                for (int i = 0; i < PrecisionLength; i++)
                {
                    fieldString += "#";
                }
            }

            var displayFieldString = "###,###,###,##0";

            if (PrecisionLength > 0)
            {
                displayFieldString += ".";
                for (int i = 0; i < PrecisionLength; i++)
                {
                    displayFieldString += "0";
                }
            }

            int selectCategory = 0;
            if (grdImportHistory.Rows[SelectedRowIndex].Cells["celCategory"].Value != null)
            {
                selectCategory = (int)grdImportHistory.Rows[SelectedRowIndex].Cells["celCategory"].Value;
            }

            int countApportioned = 0;
            foreach (Row rowReceipt in grdReceiptDetails.Rows)
            {
                if (selectCategory != 0 && Convert.ToInt32(grdReceiptDetails.Rows[rowReceipt.Index].Cells["celApportioned"].Value) != 1)
                {
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celReceiptExcludeFlag"].Value = 1;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celExcludeCategoryName"].Value = selectCategory;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celExcludeAmount"].Value = grdReceiptDetails.Rows[rowReceipt.Index].Cells["celReceiptAmount"].Value;
                }

                foreach (Cell rowcell in rowReceipt.Cells)
                {
                    if (rowcell.Name == "celReceiptExcludeFlag")
                    {
                        grdReceiptDetails.Rows[rowReceipt.Index].Cells["celExcludeCategoryName"].Enabled = Convert.ToInt32(rowcell.Value.ToString()) == 1;
                        grdReceiptDetails.Rows[rowReceipt.Index].Cells["celExcludeAmount"].Enabled = Convert.ToInt32(rowcell.Value.ToString()) == 1;
                    }
                    if (rowcell.Name == "celExcludeAmount" || rowcell.Name == "celReceiptAmount")
                    {
                        GcNumberCell gcNumberCell = (GcNumberCell)rowcell;
                        gcNumberCell.Fields.SetFields(fieldString, "", "", "-", "");
                        gcNumberCell.DisplayFields.Clear();
                        gcNumberCell.DisplayFields.AddRange(displayFieldString, "", "", "-", "");
                        rowcell.Value = gcNumberCell.Value;
                    }
                }

                int apportioned = Convert.ToInt32(rowReceipt.Cells["celApportioned"].Value);
                if (apportioned == 1)
                {
                    countApportioned = 1;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celReceiptExcludeFlag"].Enabled = false;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celExcludeCategoryName"].Enabled = false;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celExcludeAmount"].Enabled = false;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celSectionCode"].Enabled = false;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celSectionBtn"].Enabled = false;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celCustomerCode"].Enabled = false;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celCustomerBtn"].Enabled = false;

                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celReceiptExcludeFlag"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celReceiptExcludeFlag"].Style.DisabledBackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celExcludeCategoryName"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celExcludeCategoryName"].Style.DisabledBackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celExcludeAmount"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celExcludeAmount"].Style.DisabledBackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celSectionCode"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celSectionCode"].Style.DisabledBackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celSectionBtn"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celSectionBtn"].Style.DisabledBackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celSectionName"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celCustomerCode"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celCustomerCode"].Style.DisabledBackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celCustomerBtn"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celCustomerBtn"].Style.DisabledBackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celCustomerName"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celPayerName"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celCurencyCode"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celReceiptAmount"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celRecordTime"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celWorkDay"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celSourceBankName"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celSourceBranchName"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celPayerCode1"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celPayerCode2"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celReferenceCustomerCode"].Style.BackColor = Color.LightGoldenrodYellow;
                    grdReceiptDetails.Rows[rowReceipt.Index].Cells["celReferenceCustomerName"].Style.BackColor = Color.LightGoldenrodYellow;
                }
            }
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction04Enabled(true);

            if (countApportioned == 0)
            {
                BaseContext.SetFunction01Enabled(false);
                BaseContext.SetFunction05Enabled(false);
            }
            else
            {
                BaseContext.SetFunction01Enabled(true);
            }
        }

        private void grdReceiptDetails_CellBtnClick(object sender, CellEventArgs e)
        {
            if (e.CellName.ToString() == "celSectionBtn")
            {
                var section = this.ShowSectionSearchDialog();
                if (section != null)
                {
                    grdReceiptDetails.Rows[e.RowIndex].Cells["celSectionCode"].Value = section.Code;
                    grdReceiptDetails.Rows[e.RowIndex].Cells["celSectionName"].Value = section.Name;
                    grdReceiptDetails.Rows[e.RowIndex].Cells["celSectionId"].Value = section.Id;
                    ClearStatusMessage();
                }
                return;
            }

            if (e.CellName.ToString() == "celCustomerBtn")
            {
                var customer = this.ShowCustomerMinSearchDialog();
                if (customer != null)
                {
                    grdReceiptDetails.Rows[e.RowIndex].Cells["celCustomerCode"].Value = customer.Code;
                    grdReceiptDetails.Rows[e.RowIndex].Cells["celCustomerName"].Value = customer.Name;
                    grdReceiptDetails.Rows[e.RowIndex].Cells["celCustomerId"].Value = customer.Id;
                    ClearStatusMessage();
                }
            }
        }

        private void grdReceiptDetails_CellValidating(object sender, CellValidatingEventArgs e)
        {
            try
            {
                string code = null;
                if (e.CellName.ToString() == "celCustomerCode" &&
                        grdReceiptDetails.Rows[e.RowIndex].Cells["celCustomerCode"].EditedFormattedValue != null)
                {
                    code = grdReceiptDetails.Rows[e.RowIndex].Cells["celCustomerCode"].EditedFormattedValue.ToString();
                    if (code != "")
                    {
                        if (ApplicationControl?.CustomerCodeType == 0)
                        {
                            code = code.PadLeft(ApplicationControl.CustomerCodeLength, '0');
                        }
                        Task<Customer> loadTask = GetCustomerName(code);
                        ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                        Customer result = loadTask.Result;
                        if (result != null)
                        {
                            ClearStatusMessage();
                            grdReceiptDetails.EndEdit();
                            grdReceiptDetails.SetValue(e.RowIndex, "celCustomerCode", result.Code);
                            grdReceiptDetails.SetValue(e.RowIndex, "celCustomerName", result.Name);
                            grdReceiptDetails.SetValue(e.RowIndex, "celCustomerId", result.Id);
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "得意先", code);
                            grdReceiptDetails.SetValue(e.RowIndex, "celCustomerCode", "");
                            grdReceiptDetails.SetValue(e.RowIndex, "celCustomerName", "");
                            grdReceiptDetails.SetValue(e.RowIndex, "celCustomerId", null);
                        }
                    }
                }
                else if (e.CellName.ToString() == "celCustomerCode" &&
                        grdReceiptDetails.Rows[e.RowIndex].Cells["celCustomerCode"].EditedFormattedValue == null)
                {
                    grdReceiptDetails.SetValue(e.RowIndex, "celCustomerName", "");
                    grdReceiptDetails.SetValue(e.RowIndex, "celCustomerId", null);
                    ClearStatusMessage();
                }

                if (e.CellName.ToString() == "celSectionCode" &&
                      grdReceiptDetails.Rows[e.RowIndex].Cells["celSectionCode"].EditedFormattedValue != null)
                {
                    code = grdReceiptDetails.Rows[e.RowIndex].Cells["celSectionCode"].EditedFormattedValue.ToString();
                    if (code != "")
                    {
                        if (ApplicationControl?.SectionCodeType == 0)
                        {
                            code = code.PadLeft(ApplicationControl.SectionCodeLength, '0');
                        }
                        Task<Web.Models.Section> loadTask = GetSectionName(code);
                        ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                        Web.Models.Section result = loadTask.Result;
                        if (result != null)
                        {
                            ClearStatusMessage();
                            grdReceiptDetails.EndEdit();
                            grdReceiptDetails.SetValue(e.RowIndex, "celSectionCode", result.Code);
                            grdReceiptDetails.SetValue(e.RowIndex, "celSectionName", result.Name);
                            grdReceiptDetails.SetValue(e.RowIndex, "celSectionId", result.Id);
                        }
                        else
                        {
                            ShowWarningDialog(MsgWngMasterNotExist, "入金部門", code);
                            grdReceiptDetails.SetValue(e.RowIndex, "celSectionCode", "");
                            grdReceiptDetails.SetValue(e.RowIndex, "celSectionName", "");
                            grdReceiptDetails.SetValue(e.RowIndex, "celSectionId", null);
                        }
                    }
                }
                else if (e.CellName.ToString() == "celSectionCode" &&
                      grdReceiptDetails.Rows[e.RowIndex].Cells["celSectionCode"].EditedFormattedValue == null)
                {
                    grdReceiptDetails.SetValue(e.RowIndex, "celSectionName", "");
                    grdReceiptDetails.SetValue(e.RowIndex, "celSectionId", null);
                    ClearStatusMessage();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grdReceiptDetails_CellEndEdit(object sender, CellEndEditEventArgs e)
        {
            if (e.CellName.ToString() == "celSectionCode" &&
                Convert.ToString(grdReceiptDetails.Rows[e.RowIndex].Cells["celSectionName"].Value) == "")
            {
                grdReceiptDetails.Rows[e.RowIndex].Cells["celSectionCode"].Value = "";
            }

            if (e.CellName.ToString() == "celCustomerCode" &&
                Convert.ToString(grdReceiptDetails.Rows[e.RowIndex].Cells["celCustomerName"].Value) == "")
            {
                grdReceiptDetails.Rows[e.RowIndex].Cells["celCustomerCode"].Value = "";
            }
        }

        private void grdReceiptDetails_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.CellName.ToString() == "celReceiptExcludeFlag")
            {
                if (grdReceiptDetails.Rows[e.RowIndex].Cells["celReceiptExcludeFlag"].Enabled)
                {
                    if (Convert.ToBoolean(grdReceiptDetails.Rows[e.RowIndex].Cells["celReceiptExcludeFlag"].EditedFormattedValue))
                    {
                        grdReceiptDetails.Rows[e.RowIndex].Cells["celExcludeCategoryName"].Value = grdImportHistory.Rows[SelectedRowIndex].Cells["celCategory"].Value;
                        grdReceiptDetails.Rows[e.RowIndex].Cells["celExcludeCategoryName"].Enabled = true;
                        grdReceiptDetails.Rows[e.RowIndex].Cells["celExcludeAmount"].Enabled = true;
                        grdReceiptDetails.Rows[e.RowIndex].Cells["celExcludeAmount"].Value = grdReceiptDetails.Rows[e.RowIndex].Cells["celReceiptAmount"].Value;
                    }
                    else
                    {
                        grdReceiptDetails.Rows[e.RowIndex].Cells["celExcludeCategoryName"].Value = null;
                        grdReceiptDetails.Rows[e.RowIndex].Cells["celExcludeAmount"].Value = 0M;
                        grdReceiptDetails.Rows[e.RowIndex].Cells["celExcludeCategoryName"].Enabled = false;
                        grdReceiptDetails.Rows[e.RowIndex].Cells["celExcludeAmount"].Enabled = false;
                    }
                }
            }
        }

        private void grdReceiptDetails_CellDoubleClick(object sender, CellEventArgs e)
        {
            grdReceiptDetails_CellContentClick(sender, e);
        }
        #endregion

        private void cbxSectionAssign_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ReceiptApportionList != null && ReceiptApportionList.Any())
                {
                    if (cbxSectionAssign.Checked)
                    {
                        SettingForSectionNotShown();
                    }
                    else
                    {
                        SettingForSectionShown();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void grdImportHistory_CellValueChanged(object sender, CellEventArgs e)
        {
            ValueChange = true;
        }

        private void grdReceiptDetails_CellValueChanged(object sender, CellEventArgs e)
        {
            ValueChange = true;
            grdReceiptDetails_CellContentClick(sender, e);
        }

        /// <summary>一括振替用 ReceiptHeader で指定してある対象外区分を設定できるようにする</summary>
        /// <param name="messaging">必須項目 自動設定不可の場合のメッセージ表示処理</param>
        /// <returns>振替対象の <see cref="ReceiptApportion"/> </returns>
        /// <remarks>
        /// 得意先必須 入金部門利用時に自動設定できない場合は 要素0 の 値を返す
        /// </remarks>
        private async Task<IEnumerable<ReceiptApportion>> PrepareAllApportionItems()
        {
            IEnumerable<ReceiptApportion> result = Enumerable.Empty<ReceiptApportion>();

            var headers = grdImportHistory.Rows.Select(x =>
            {
                var header = x.DataBoundItem as ReceiptHeader;
                var excludeCategoryId = (int?)x["celCategory"].Value;
                return new
                {
                    ReceiptHeaderId = header.Id,
                    IsAllApportioned = header.IsAllApportioned,
                    ExcludeCategoryId = excludeCategoryId,
                };
            }).Where(x => x.IsAllApportioned == 0);

            var headerIds = headers.Select(x => x.ReceiptHeaderId);

            List<ReceiptApportion> receipts = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<ReceiptServiceClient>();
                var getResult = await client.GetApportionItemsAsync(SessionKey, headerIds.ToArray());
                if (getResult.ProcessResult.Result)
                    receipts = getResult.ReceiptApportion.Where(x => x.Apportioned == 0).ToList();
            });

            foreach (var receipt in receipts)
            {
                receipt.Apportioned = 1;
                receipt.UpdateBy = Login.UserId;

                var header = headers.First(x => x.ReceiptHeaderId == receipt.ReceiptHeaderId);
                if (header.ExcludeCategoryId.HasValue)
                {
                    receipt.ExcludeFlag = 1;
                    receipt.ExcludeAmount = receipt.ReceiptAmount;
                    receipt.ExcludeCategoryId = header.ExcludeCategoryId;
                }

                var customerId = receipt.CustomerId ?? receipt.RefCustomerId;

                if (RequiredCustomer && !receipt.CustomerId.HasValue)
                {
                    ShowWarningDialog(MsgWngNotApportionedCustomer);
                    return result;
                }

                if (RequiredSection
                    && !receipt.SectionId.HasValue)
                {
                    if (customerId.HasValue)
                    {
                        var section = await GetSectionAsync(customerId.Value);
                        receipt.SectionId = section?.Id;
                    }
                    if (!receipt.SectionId.HasValue)
                    {
                        ShowWarningDialog(MsgWngNotApportionedSection);
                        return result;
                    }
                }
            }
            result = receipts;
            if (!receipts.Any())
            {
                ShowWarningDialog(MsgWngNoData, "対象となる");
                return result;
            }
            return result;
        }

        private async Task<Web.Models.Section> GetSectionAsync(int customerId)
        {
            Web.Models.Section section = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var client = factory.Create<SectionMasterClient>();
                var result = await client.GetByCustomerIdAsync(SessionKey, customerId);
                if (result?.ProcessResult.Result ?? false)
                {
                    section = result.Section.FirstOrDefault();
                }
            });
            return section;
        }
    }
}