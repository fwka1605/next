using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.BillingService;
using Rac.VOne.Client.Screen.CurrencyMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.NettingService;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Import;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Web.Models.FunctionType;

namespace Rac.VOne.Client.Screen
{
    /// <summary>通貨マスター</summary>
    public partial class PB2101 : VOneScreenBase
    {
        #region local variable
        private int CurrencyID { get; set; }
        private List<Currency> CurrencyList { get; set; }
        private Currency CurrentCurrency { get; set; }
        #endregion

        #region Initial
        public PB2101()
        {
            InitializeComponent();
            grdCurrencyMaster.SetupShortcutKeys();
            Text = "通貨マスター";

            CurrencyList = new List<Currency>();

            AddHandlers();
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
            BaseContext.SetFunction03Enabled(true);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction05Caption("インポート");
            OnF05ClickHandler = Import;

            BaseContext.SetFunction06Caption("エクスポート");
            OnF06ClickHandler = ExportData;

            BaseContext.SetFunction10Caption("終了");
            BaseContext.SetFunction10Enabled(true);
            OnF10ClickHandler = Exit;
        }

        private void PB2101_Load(object sender, EventArgs e)
        {
            SetScreenName();
            try
            {
                var loadTask = new List<Task>();

                if (ApplicationControl == null)
                    loadTask.Add(LoadApplicationControlAsync());

                loadTask.Add(LoadControlColorAsync());

                if (Company == null)
                    loadTask.Add(LoadCompanyAsync());

                loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));

                // グリッド表示
                Task<List<Currency>> loadListTask = LoadListAsync();
                loadTask.Add(loadListTask);

                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                CurrencyList.AddRange(loadListTask.Result);

                Initialize();
                grdCurrencyMaster.DataSource = new BindingSource(CurrencyList, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }

            Clear();
        }

        private void Initialize()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var rowHeight = builder.DefaultRowHeight;
            Func<Cell> getNumberCell = () => UseForeignCurrency
                ? builder.GetNumberCellCurrency(5, 5, 1, "#")
                : builder.GetNumberCell();

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(rowHeight, 50 , "Header"      , cell: builder.GetRowHeaderCell() ),
                new CellSetting(rowHeight, 60 , "Code"        , caption: "通貨コード"    , dataField: nameof(Currency.Code)        , cell: builder.GetTextBoxCell(MultiRowContentAlignment.MiddleCenter) ),
                new CellSetting(rowHeight, 150, "Name"        , caption: "名称"          , dataField: nameof(Currency.Name)        , cell: builder.GetTextBoxCell() ),
                new CellSetting(rowHeight, 40 , "Symbol"      , caption: "単位"          , dataField: nameof(Currency.Symbol)      , cell: builder.GetTextBoxCell() ),
                new CellSetting(rowHeight, 100, "Precision"   , caption: "小数点以下桁数", dataField: nameof(Currency.Precision)   , cell: builder.GetNumberCell()  ),
                new CellSetting(rowHeight, 50 , "DisplayOrder", caption: "表示順"        , dataField: nameof(Currency.DisplayOrder), cell: builder.GetNumberCell()  ),
                new CellSetting(rowHeight, 200, "Note"        , caption: "備考"          , dataField: nameof(Currency.Note)        , cell: builder.GetTextBoxCell() ),
                new CellSetting(rowHeight, 100, "Tolerance"   , caption: "手数料誤差"    , dataField: nameof(Currency.Tolerance)   , cell: getNumberCell() ),
                new CellSetting(rowHeight, 0  , "CurrencyId"  , visible: false, dataField: nameof(Currency.Id) ),
            });

            grdCurrencyMaster.Template = builder.Build();
            grdCurrencyMaster.HideSelection = true;
        }
        #endregion

        #region Function Key Event
        [OperationLog("登録")]
        private void Save()
        {
            CurrencyResult result = null;
            List<Currency> currencyList = null;

            try
            {
                // 入力項目チェック
                if (!ValidateInput())
                    return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                ClearStatusMessage();

                Currency currency = CurrencyInfo();

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<CurrencyMasterClient>();

                    result = await service.SaveAsync(SessionKey, currency);
                    if (result.ProcessResult.Result)
                    {
                        currencyList = await LoadListAsync();
                    }
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (result.ProcessResult.Result)
                {
                    if (result.Currency != null)
                    {
                        Clear();
                        DispStatusMessage(MsgInfSaveSuccess);
                    }
                    else
                    {
                        ShowWarningDialog(MsgErrSaveError);
                    }

                    CurrencyList.Clear();
                    CurrencyList.AddRange(currencyList);
                }

                if (currencyList != null)
                    grdCurrencyMaster.DataSource = new BindingSource(CurrencyList, null);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void ConfirmToClear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            Clear();
            Modified = false;
        }

        [OperationLog("終了")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            BaseForm.Close();
        }

        [OperationLog("インポート")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {
                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.Currency);

                // 取込設定取得
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new CurrencyFileDefinition(new DataExpression(ApplicationControl));

                //その他
                definition.CurrencyCodeField.ValidateAdditional = (val, param) =>
                {
                    var reports = new List<WorkingReport>();
                    if (((ImportMethod)param) != ImportMethod.Replace)
                        return reports;

                    MasterDatasResult billResult = null;
                    MasterDatasResult receiptResult = null;
                    MasterDatasResult nettingResult = null;

                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var currencyMaster = factory.Create<CurrencyMasterClient>();
                        var codes = val.Values.Where(x => !string.IsNullOrEmpty(x.Code)).Select(x => x.Code).Distinct().ToArray();

                        billResult = currencyMaster.GetImportItemsBilling(SessionKey, CompanyId, codes);
                        receiptResult = currencyMaster.GetImportItemsReceipt(SessionKey, CompanyId, codes);
                        nettingResult = currencyMaster.GetImportItemsNetting(SessionKey, CompanyId, codes);
                    });

                    foreach (MasterData ca in billResult.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.CurrencyCodeField.FieldIndex,
                            FieldName = definition.CurrencyCodeField.FieldName,
                            Message = $"請求データに存在する{ca.Code}が存在しないため、インポートできません。",
                        });
                    }

                    foreach (var ca in receiptResult.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.CurrencyCodeField.FieldIndex,
                            FieldName = definition.CurrencyCodeField.FieldName,
                            Message = $"入金データに存在する{ca.Code}が存在しないため、インポートできません。",
                        });
                    }

                    foreach (var ca in nettingResult.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                    {
                        reports.Add(new WorkingReport
                        {
                            LineNo = null,
                            FieldNo = definition.CurrencyCodeField.FieldIndex,
                            FieldName = definition.CurrencyCodeField.FieldName,
                            Message = $"入金予定相殺データに存在する{ca.Code}が存在しないため、インポートできません。",
                        });
                    }

                    return reports;
                };

                definition.CurrencyToleranceField.ValidateAdditional = (val, param) =>
                {
                    var reports = new List<WorkingReport>();
                    foreach (var pair in val)
                    {
                        if (pair.Value.Precision < 0) continue;
                        var significant = (pair.Value.Tolerance * Amount.Pow10(pair.Value.Precision));

                        if (Math.Floor(significant) != significant)
                        {
                            reports.Add(new WorkingReport
                            {
                                LineNo = pair.Key,
                                FieldNo = definition.CurrencyToleranceField.FieldIndex,
                                FieldName = definition.CurrencyToleranceField.FieldName,
                                Message = $"手数料誤差金額は小数点以下{pair.Value.Precision}桁までとなるよう入力してください。",
                            });
                        }
                    }
                    return reports;
                };

                var importer = definition.CreateImporter(m => m.Code);
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = Login.CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await LoadListAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);

                var importResult = DoImport(importer, importSetting, Clear);
                if (!importResult) return;
                CurrencyList.Clear();
                var loadListTask = LoadListAsync();
                ProgressDialog.Start(ParentForm, loadListTask, false, SessionKey);
                CurrencyList.AddRange(loadListTask.Result);
                grdCurrencyMaster.DataSource = new BindingSource(CurrencyList, null);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<Currency> imported)
        {
            ImportResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                result = await service.ImportAsync(SessionKey, imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }

        [OperationLog("削除")]
        private void Delete()
        {
            ClearStatusMessage();
            try
            {
                DeleteCurrency();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void DeleteCurrency()
        {
            var currencyCode = txtCurrencyCode.Text;
            ExistResult resultBill = null;
            ExistResult resultRec = null;
            ExistResult resultNet = null;

            //MessageInfo msg = null;
            System.Action messaging = null;
            var checkTask = ServiceProxyFactory.LifeTime(async factory =>
            {
                var serviceBill = factory.Create<BillingServiceClient>();
                var serviceRec = factory.Create<ReceiptServiceClient>();
                var serviceNetting = factory.Create<NettingServiceClient>();

                resultBill = await serviceBill.ExistCurrencyAsync(SessionKey, CurrencyID);
                if (resultBill.Exist)
                {
                    messaging = () => ShowWarningDialog(MsgWngDeleteConstraint, "請求テーブル", "通貨コード");
                    return;
                }
                resultRec = await serviceRec.ExistCurrencyAsync(SessionKey, CurrencyID);
                if (resultRec.Exist)
                {
                    messaging = () => ShowWarningDialog(MsgWngDeleteConstraint, "入金テーブル", "通貨コード");
                    return;
                }
                resultNet = await serviceNetting.ExistCurrencyAsync(SessionKey, CurrencyID);
                if (resultNet.Exist)
                {
                    messaging = () => ShowWarningDialog(MsgWngDeleteConstraint, "入金予定相殺テーブル", "通貨コード");
                    return;
                }
            });
            ProgressDialog.Start(ParentForm, checkTask, false, SessionKey);

            if (messaging != null)
            {
                messaging.Invoke();
                return;
            }

            // Show Confirm
            if (!ShowConfirmDialog(MsgQstConfirmDelete))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }

            CountResult deleteResult = null;
            CurrentCurrency = CurrencyList.Find(b => (b.Code == currencyCode));

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();

                deleteResult = await service.DeleteAsync(SessionKey, CurrentCurrency.Id);
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (deleteResult.Count > 0)
            {
                CurrencyList.Remove(CurrentCurrency);
                grdCurrencyMaster.DataSource = new BindingSource(CurrencyList, null);
                Clear();
                DispStatusMessage(MsgInfDeleteSuccess);
            }
            else
                ShowWarningDialog(MsgErrDeleteError);
        }

        [OperationLog("エクスポート")]
        private void ExportData()
        {
            ClearStatusMessage();

            List<Currency> list = null;
            string serverPath = null;

            try
            {
                var loadTask = LoadListAsync();
                var pathTask = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<GeneralSettingMasterClient>();
                    var result = await service.GetByCodeAsync(
                            SessionKey, CompanyId, "サーバパス");

                    if (result.ProcessResult.Result)
                    {
                        serverPath = result.GeneralSetting?.Value;
                    }

                    if (!Directory.Exists(serverPath))
                    {
                        serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                });
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask, pathTask), false, SessionKey);

                list = loadTask.Result;

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                var filePath = string.Empty;
                var fileName = $"通貨マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new CurrencyFileDefinition(new DataExpression(ApplicationControl));
                definition.CurrencyToleranceField.Format = value => value.ToString("G29");

                var exporter = definition.CreateExporter();
                exporter.UserId = Login.UserId;
                exporter.UserCode = Login.UserCode;
                exporter.CompanyId = CompanyId;
                exporter.CompanyCode = Login.CompanyCode;

                ProgressDialog.Start(ParentForm, (cancel, progress) =>
                {
                    return exporter.ExportAsync(filePath, list, cancel, progress);
                }, true, SessionKey);

                if (exporter.Exception != null)
                {
                    NLogHandler.WriteErrorLog(this, exporter.Exception, SessionKey);
                    ShowWarningDialog(MsgErrExportError);
                    return;
                }

                DispStatusMessage(MsgInfFinishExport);
                Settings.SavePath<Receipt>(Login, filePath);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }
        #endregion

        #region 通貨コード処理
        private async Task<List<Currency>> LoadListAsync()
        {
            List<Currency> list = null;
            CurrenciesResult result = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<CurrencyMasterClient>();
                result = await service.GetItemsAsync(SessionKey, CompanyId, new CurrencySearch());

                if (result.ProcessResult.Result)
                {
                    list = result.Currencies.OrderBy(b => b.DisplayOrder)
                        .ThenBy(b => b.Code).ToList();
                }
            });
            return list ?? new List<Currency>();
        }

        private void txtCurrencyCode_Validated(object sender, EventArgs e)
        {
            try
            {
                CurrenciesResult result = null;

                if (txtCurrencyCode.Text != string.Empty)
                {
                    var task = ServiceProxyFactory.LifeTime(async factory =>
                    {
                        var service = factory.Create<CurrencyMasterClient>();

                        result = await service.GetByCodeAsync(
                                SessionKey, CompanyId,
                                new string[] { txtCurrencyCode.Text });
                    });
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (result.ProcessResult.Result && result.Currencies.Any())
                    {
                        SetCurrencyData(result.Currencies[0]);
                        Modified = false;
                    }
                    else
                    {
                        SetCurrencyData(null);
                    }
                }
                else
                {
                    ClearStatusMessage();
                    txtCurrencyCode.Clear();
                    CurrencyID = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private Currency CurrencyInfo()
        {
            var currency = new Currency();
            Model.CopyTo(CurrentCurrency, currency, true);

            currency.Code = txtCurrencyCode.Text;
            currency.CompanyId = CompanyId;
            currency.Name = txtName.Text.Trim();
            currency.Symbol = txtSymbol.Text;
            currency.Precision = int.Parse(nmbPrecision.Value.ToString());
            currency.DisplayOrder = int.Parse(nmbDisplayOrder.Value.ToString());
            currency.Note = txtNote.Text.Trim();
            currency.Tolerance = Convert.ToDecimal(nmbTolerance.Value);
            currency.CreateBy = Login.UserId;
            currency.UpdateBy = Login.UserId;

            return currency;
        }
        #endregion

        #region SubFunction呼び出し
        private void SetCurrencyData(Currency selectedCurrency)
        {
            CurrentCurrency = selectedCurrency ?? new Currency();

            if (selectedCurrency == null)
            {
                txtCurrencyCode.Enabled = true;

                DispStatusMessage(MsgInfNewData, "通貨コード");
            }
            else
            {
                txtCurrencyCode.Text = selectedCurrency.Code;
                txtName.Text = selectedCurrency.Name;
                txtSymbol.Text = selectedCurrency.Symbol;
                nmbPrecision.Text = selectedCurrency.Precision.ToString();
                nmbDisplayOrder.Text = selectedCurrency.DisplayOrder.ToString();
                txtNote.Text = selectedCurrency.Note;
                nmbTolerance.Value = selectedCurrency.Tolerance;

                txtCurrencyCode.Enabled = false;

                if (selectedCurrency.Code != "JPY")
                {
                    BaseContext.SetFunction03Enabled(true);

                    txtName.Enabled = true;
                    txtName.Focus();
                    txtSymbol.Enabled = true;
                    nmbPrecision.Enabled = true;
                    nmbDisplayOrder.Enabled = true;
                    txtNote.Enabled = true;
                }
                else
                {
                    BaseContext.SetFunction03Enabled(false);

                    txtName.Enabled = false;
                    txtSymbol.Enabled = false;
                    nmbPrecision.Enabled = false;
                    nmbDisplayOrder.Enabled = false;
                    txtNote.Enabled = false;
                    nmbTolerance.Focus();
                }

                CurrencyID = selectedCurrency.Id;
                ClearStatusMessage();
            }
        }

        private bool ValidateInput()
        {
            var success = true;
            var fail = false;

            if (string.IsNullOrWhiteSpace(txtCurrencyCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblCurrencyCode.Text);
                txtCurrencyCode.Focus();
                return fail;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblName.Text);
                txtName.Focus();
                return fail;
            }

            if (string.IsNullOrWhiteSpace(txtSymbol.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, lblSymbol.Text);
                txtSymbol.Focus();
                return fail;
            }

            if (!nmbPrecision.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, lblPrecision.Text);
                nmbPrecision.Focus();
                return fail;
            }

            if (!nmbDisplayOrder.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, lblDisplayOrder.Text);
                nmbDisplayOrder.Focus();
                return fail;
            }

            if (!nmbTolerance.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, lblTolerance.Text);
                nmbTolerance.Focus();
                return fail;
            }
            string Tolerance = nmbTolerance.Text;

            if (!PrecisionCheck(Tolerance, int.Parse(nmbPrecision.Text)))
            {
                ShowWarningDialog(MsgWngInvalidFeeToleranceScale, nmbPrecision.Text);
                nmbTolerance.Focus();
                return fail;
             } 
            return success;
        }

        private bool PrecisionCheck(string Tolerance, int Parse)
        {
            if (Tolerance.Contains('.'))
            {
                string t1 = Tolerance.Split('.')[1];

                if (t1.Length > Parse)
                {

                    for (int i = Parse; i < t1.Length; i++)
                    {
                        if (t1[i] != '0')
                        {
                            return false;
                        }
                    }
                }
            }return true;
        }

        public void Clear()
        {
            ClearStatusMessage();

            txtCurrencyCode.Clear();
            txtName.Clear();
            txtSymbol.Clear();
            nmbPrecision.Clear();
            nmbDisplayOrder.Clear();
            txtNote.Clear();
            nmbTolerance.Clear();

            txtCurrencyCode.Enabled = true;
            txtName.Enabled = true;
            txtSymbol.Enabled = true;
            nmbPrecision.Enabled = true;
            nmbDisplayOrder.Enabled = true;
            txtNote.Enabled = true;
            nmbTolerance.Enabled = true;

            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
            this.ActiveControl = txtCurrencyCode;

            // 変更フラグ
            Modified = false;
        }
        #endregion

        #region Function for other event
        private void AddHandlers()
        {
            foreach (Control control in gbxCurrencyInput.Controls)
            {
                if (control is Common.Controls.VOneTextControl)
                    control.TextChanged += new EventHandler(OnContentChanged);

                if (control is Common.Controls.VOneNumberControl)
                    control.TextChanged += new EventHandler(OnContentChanged);
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }
        #endregion

        #region グリッドのクリックイベント
        private void grdCurrencyMaster_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                grdCurrencyMaster.CurrentCell.Selected = true;
                CurrencyID = (grdCurrencyMaster.CurrentRow.DataBoundItem as Currency)?.Id ?? 0;

                if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                    return;

                SetCurrencyData(CurrencyList[e.RowIndex]);
                Modified = false;
            }
        }
        #endregion
    }
}
