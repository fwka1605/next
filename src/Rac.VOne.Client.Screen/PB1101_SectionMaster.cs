using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Reports;
using Rac.VOne.Client.Screen.BankAccountMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.LoginUserMasterService;
using Rac.VOne.Client.Screen.NettingService;
using Rac.VOne.Client.Screen.ReceiptService;
using Rac.VOne.Client.Screen.SectionMasterService;
using Rac.VOne.Client.Screen.SectionWithDepartmentMasterService;
using Rac.VOne.Client.Screen.SectionWithLoginUserMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Import;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
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
using static Rac.VOne.Web.Models.FunctionType;
using Section = Rac.VOne.Web.Models.Section;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金部門マスター</summary>
    public partial class PB1101 : VOneScreenBase
    {
        #region 画面のinitalize
        private int SectionId { get; set; }
        List<Section> SectionList { get; set; }
        private string CellName(string value) => $"cel{value}";

        public PB1101()
        {
            InitializeComponent();
            grdSectionMaster.SetupShortcutKeys();
            Text = "入金部門マスター";
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();
            BaseContext.SetFunction01Caption("登録");
            OnF01ClickHandler = Save;

            BaseContext.SetFunction02Caption("クリア");
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction04Caption("印刷");
            BaseContext.SetFunction04Enabled(true);
            OnF04ClickHandler = Print;

            BaseContext.SetFunction05Caption("インポート");
            BaseContext.SetFunction05Enabled(true);
            OnF05ClickHandler = Import;

            BaseContext.SetFunction06Caption("エクスポート");
            BaseContext.SetFunction06Enabled(true);
            OnF06ClickHandler = Export;

            BaseContext.SetFunction07Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }

        private void PB1101_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();
                AddHandlers();
                var loadTask = new List<Task>();
                SectionList = new List<Section>();

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }

                loadTask.Add(LoadControlColorAsync());
                loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }

                Task loadListTask = LoadGridAsync()
                        .ContinueWith(task => SectionList.AddRange(task.Result));
                loadTask.Add(loadListTask);
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                InitializeGridLoad();
                grdSectionMaster.DataSource = new BindingSource(SectionList, null);
                SetFormat();
                Clears();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void AddHandlers()
        {
            foreach(Control control in gbxSectionInput.Controls)
            {
                if(control is Common.Controls.VOneTextControl)
                {
                    control.TextChanged += new EventHandler(OnContentChanged);
                }
            }
        }

        private void OnContentChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void InitializeGridLoad()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var middleCenter = MultiRowContentAlignment.MiddleCenter;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting( height,  40, "Header"                                                                 , cell: builder.GetRowHeaderCell() ),
                new CellSetting( height, 100, nameof(Section.Code)          , dataField: nameof(Section.Code)          , cell: builder.GetTextBoxCell(middleCenter) , caption: "入金部門コード"),
                new CellSetting( height, 320, nameof(Section.Name)          , dataField: nameof(Section.Name)          , cell: builder.GetTextBoxCell()             , caption: "入金部門名"    ),
                new CellSetting( height,  90, nameof(Section.PayerCodeLeft) , dataField: nameof(Section.PayerCodeLeft) , cell: builder.GetTextBoxCell(middleCenter) , caption: "仮想支店コード"),
                new CellSetting( height, 100, nameof(Section.PayerCodeRight), dataField: nameof(Section.PayerCodeRight), cell: builder.GetTextBoxCell(middleCenter) , caption: "仮想口座番号"  ),
                new CellSetting( height,   0, nameof(Section.Id)            , dataField: nameof(Section.Id)            , visible: true ),
                new CellSetting( height,   0, nameof(Section.Note)          , dataField: nameof(Section.Note)          , visible: true )
            });
            grdSectionMaster.Template = builder.Build();
            grdSectionMaster.SetRowColor(ColorContext);
            grdSectionMaster.HideSelection = true;
        }

        private void SetFormat()
        {
            var expression = new DataExpression(ApplicationControl);
            txtSectionCode.Format = expression.SectionCodeFormatString;
            txtSectionCode.MaxLength = expression.SectionCodeLength;
            txtSectionCode.PaddingChar = expression.SectionCodePaddingChar;
            txtLeftPayerCode.PaddingChar = '0';
            txtRightPayerCode.PaddingChar = '0';
        }

        #endregion

        #region FunctionKey
        [OperationLog("登録")]
        private void Save()
        {
            try
            {
                if (!ValidateChildren()) return;
                ZeroLeftPaddingWithoutValidated();

                if (!CheckRequired()) return;
                if (!CheckPayerCode()) return;

                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                Section sectionInsert = SectionDataInfo();
                SectionResults saveResult = null;
                List<Section> newList = null;
                var success = false;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    var service = factory.Create<SectionMasterClient>();
                    saveResult = saveResult = await service.SaveAsync(SessionKey, sectionInsert);
                    success = saveResult?.ProcessResult.Result ?? false;
                    if (success) newList = await LoadGridAsync();
                });
                ProgressDialog.Start(ParentForm, task, false, SessionKey);

                if (!success)
                {
                    ShowWarningDialog(MsgErrSaveError);
                    return;
                }

                SectionList = newList;
                grdSectionMaster.DataSource = new BindingSource(SectionList, null);
                Clears();
                DispStatusMessage(MsgInfSaveSuccess);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private bool CheckPayerCode()
        {
            var flag = true;

            if (!string.IsNullOrWhiteSpace(txtLeftPayerCode.Text) &&
                   !string.IsNullOrWhiteSpace(txtRightPayerCode.Text))
            {
                var tempSection = new List<Section>();
                Task<List<Section>> loadListTask = LoadGridAsync();
                ProgressDialog.Start(ParentForm, loadListTask, false, SessionKey);
                tempSection.AddRange(loadListTask.Result);
                var payerCode = txtLeftPayerCode.Text + txtRightPayerCode.Text;
                var hasSectionId = tempSection.Exists(x => x.Id == SectionId);

                if (hasSectionId)
                {
                    var index = tempSection.FindIndex(x => x.Id == SectionId);
                    tempSection.RemoveAt(index);
                }

                var hasPayerCode = tempSection.Exists(x => x.PayerCode == payerCode);
                var rowIndex = SectionList.FindIndex(x => x.PayerCode == payerCode);

                if (hasPayerCode)
                {
                    ShowWarningDialog(MsgWngPayerCodeDuplicate);

                    for (var j = 0; j < grdSectionMaster.RowCount; j++)
                    {
                        grdSectionMaster.Rows[j].Cells[CellName("Header")].Style.BackColor = Color.WhiteSmoke;
                        grdSectionMaster.Rows[j].Selectable = false;
                    }

                    grdSectionMaster.Rows[rowIndex].Cells[CellName("Header")].Style.BackColor = Color.Blue;
                    grdSectionMaster.Rows[rowIndex].Cells[CellName(nameof(Section.Code))].Selectable = true;

                    for (var i = 0; i < grdSectionMaster.RowCount; i++)
                    {
                        grdSectionMaster.Rows[i].Selectable = true;
                    }

                    flag = false;
                }
            }
            return flag;
        }

        private Section SectionDataInfo()
        {
            var userInsert = new Section();
            userInsert.CompanyId = CompanyId;
            userInsert.Id = SectionId;
            userInsert.Code = txtSectionCode.Text;
            userInsert.Name = txtSectionName.Text.Trim();
            userInsert.Note = txtNote.Text.Trim();
            userInsert.PayerCodeLeft = txtLeftPayerCode.Text;
            userInsert.PayerCodeRight = txtRightPayerCode.Text;
            userInsert.UpdateBy = Login.UserId;
            userInsert.CreateBy = Login.UserId;
            return userInsert;
        }

        private void ZeroLeftPaddingWithoutValidated()
        {
            if (IsNeedValidate(ApplicationControl.SectionCodeType, txtSectionCode.TextLength, ApplicationControl.SectionCodeLength))
            {
                txtSectionCode.Text = ZeroLeftPadding(txtSectionCode);
                txtSectionCode_Validated(null, null);
            }
            if (IsNeedValidate(0, txtLeftPayerCode.TextLength, txtLeftPayerCode.MaxLength))
            {
                txtLeftPayerCode.Text = ZeroLeftPadding(txtLeftPayerCode);
            }
            if (IsNeedValidate(0, txtRightPayerCode.TextLength, txtRightPayerCode.MaxLength))
            {
                txtRightPayerCode.Text = ZeroLeftPadding(txtRightPayerCode);
            }
        }

        private bool CheckRequired()
        {
            if (string.IsNullOrWhiteSpace(txtSectionCode.Text))
            {
                ShowWarningDialog(MsgWngInputRequired, "入金部門コード");
                txtSectionCode.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtSectionName.Text.Trim()))
            {
                ShowWarningDialog(MsgWngInputRequired, "入金部門名");
                txtSectionName.Focus();
                return false;
            }

            if ((string.IsNullOrEmpty(txtLeftPayerCode.Text) && !string.IsNullOrEmpty(txtRightPayerCode.Text))
                || (!string.IsNullOrEmpty(txtLeftPayerCode.Text) && string.IsNullOrEmpty(txtRightPayerCode.Text)))
            {
                ShowWarningDialog(MsgWngPayerCodeIncomplete);
                txtLeftPayerCode.Focus();
                return false;
            }
            ClearStatusMessage();
            return true;
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            if (Modified &&
                !ShowConfirmDialog(MsgQstConfirmClear))
                return;

            Clears();
            ClearStatusMessage();
            Modified = false;
        }

        private void Clears()
        {
            txtSectionCode.Clear();
            txtSectionCode.Enabled = true;
            txtSectionCode.Select();
            txtSectionName.Clear();
            txtNote.Clear();
            txtLeftPayerCode.Clear();
            txtRightPayerCode.Clear();
            BaseContext.SetFunction03Enabled(false);
            Modified = false;
            BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
            BaseContext.SetFunction06Enabled(Authorities[MasterExport]);
            SectionId = 0;
        }

        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                var validateTask = ValidateDeleteSectionId();

                ProgressDialog.Start(ParentForm, validateTask, false, SessionKey);
                if (!validateTask.Result) return;

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var deleteTask = DeleteSectionInfo();
                ProgressDialog.Start(ParentForm, deleteTask, false, SessionKey);

                var success = deleteTask.Result;

                if (success)
                {
                    DispStatusMessage(MsgInfDeleteSuccess);
                    Clears();
                }
                else
                    ShowWarningDialog(MsgErrDeleteError);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task<bool> DeleteSectionInfo()
        {
            var sectionResult = new CountResult();
            var newList = new List<Section>();
            var success = false;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                sectionResult = await service.DeleteAsync(SessionKey, SectionId);
                success = (sectionResult?.ProcessResult.Result ?? false)
                                && sectionResult?.Count > 0;
                if (success) newList = await LoadGridAsync();
            });

            if (success)
            {
                SectionList = newList;
                grdSectionMaster.DataSource = new BindingSource(SectionList, null);
            }

            return success;
        }

        private async Task<bool> ValidateDeleteSectionId()
        {
            {
                var sectionValid = false;
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<BankAccountMasterClient>();
                    var result = await client.ExistSectionAsync(SessionKey, SectionId);
                    sectionValid = !result.Exist;
                });
                if (!sectionValid)
                {
                    ShowWarningDialog(MsgWngDeleteConstraint, "銀行口座マスター", "入金部門コード");
                    return false;
                }
            }
            {
                var sectionWithDepartment = false;
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<SectionWithDepartmentMasterClient>();
                    var result = await client.ExistSectionAsync(SessionKey, SectionId);
                    sectionWithDepartment = !result.Exist;
                });
                if (!sectionWithDepartment)
                {
                    ShowWarningDialog(MsgWngDeleteConstraint, "入金・請求部門対応マスター", "入金部門コード");
                    return false;
                }
            }
            {
                var sectionWithLoginuser = false;
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<SectionWithLoginUserMasterClient>();
                    var result = await client.ExistSectionAsync(SessionKey, SectionId);
                    sectionWithLoginuser = !result.Exist;
                });
                if (!sectionWithLoginuser)
                {
                    ShowWarningDialog(MsgWngDeleteConstraint, "入金部門・担当者対応マスター", "入金部門コード");
                    return false;
                }
            }
            {
                var receiptValid = false;
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<ReceiptServiceClient>();
                    var result = await client.ExistSectionAsync(SessionKey, SectionId);
                    receiptValid = !result.Exist;
                });
                if (!receiptValid)
                {
                    ShowWarningDialog(MsgWngDeleteConstraint, "入金データ", "入金部門コード");
                    return false;
                }
            }
            {
                var nettingValid = false;
                await ServiceProxyFactory.LifeTime(async factory =>
                {
                    var client = factory.Create<NettingServiceClient>();
                    var result = await client.ExistSectionAsync(SessionKey, SectionId);
                    nettingValid = !result.Exist;
                });
                if (!nettingValid)
                {
                    ShowWarningDialog(MsgWngDeleteConstraint, "相殺データ", "入金部門コード");
                    return false;
                }
            }
            return true;
        }

        [OperationLog("インポート")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {

                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.Section);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new SectionFileDefinition(new DataExpression(ApplicationControl));
                definition.SectionCodeField.ValidateAdditional = (val, param) =>
                {
                    var reports = new List<WorkingReport>();
                    if (((ImportMethod)param) != ImportMethod.Replace)
                        return reports;

                    if (val.Any(a => a.Value.Code == null)) return reports;
                    if (val.Select(x => x.Value.Code).Distinct().Count() != val.Values.Count)
                        return reports;

                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var section = factory.Create<SectionMasterClient>();
                        MasterDatasResult resBankAccount = section.GetImportItemsForBankAccount(
                                SessionKey, CompanyId, val.Values.Select(x => x.Code).ToArray());
                        foreach (MasterData ca in resBankAccount.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                        {
                            reports.Add(new WorkingReport
                            {
                                LineNo = null,
                                FieldNo = definition.SectionCodeField.FieldIndex,
                                FieldName = definition.SectionCodeField.FieldName,
                                Message = $"銀行口座マスターに存在する{ca.Code}：{ca.Name}が存在しないため、インポートできません。",
                            });
                        }
                        MasterDatasResult resSectionWithDept = section.GetImportItemsForSectionWithDepartment(
                            SessionKey, CompanyId, val.Values.Select(x => x.Code).ToArray());
                        foreach (MasterData item in resSectionWithDept.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                        {
                            reports.Add(new WorkingReport
                            {
                                LineNo = null,
                                FieldNo = definition.SectionCodeField.FieldIndex,
                                FieldName = definition.SectionCodeField.FieldName,
                                Message = $"入金・請求部門対応マスターに存在する{item.Code}：{item.Name}が存在しないため、インポートできません。",
                            });
                        }
                        MasterDatasResult resSectionWithLoginUser = section.GetImportItemsForSectionWithLoginUser(
                            SessionKey, CompanyId, val.Values.Select(l => l.Code).ToArray());
                        foreach (MasterData item in resSectionWithLoginUser.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                        {
                            reports.Add(new WorkingReport
                            {
                                LineNo = null,
                                FieldNo = definition.SectionCodeField.FieldIndex,
                                FieldName = definition.SectionCodeField.FieldName,
                                Message = $"入金部門・担当者対応マスターに存在する{item.Code}：{item.Name}が存在しないため、インポートできません。",
                            });
                        }
                        MasterDatasResult resReceipt = section.GetImportItemsForReceipt(
                            SessionKey, CompanyId, val.Values.Select(l => l.Code).ToArray());
                        foreach (MasterData item in resReceipt.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                        {
                            reports.Add(new WorkingReport
                            {
                                LineNo = null,
                                FieldNo = definition.SectionCodeField.FieldIndex,
                                FieldName = definition.SectionCodeField.FieldName,
                                Message = $"入金データに存在する{item.Code}：{item.Name}が存在しないため、インポートできません。",
                            });
                        }
                        MasterDatasResult resNetting = section.GetImportItemsForNetting(
                            SessionKey, CompanyId, val.Values.Select(l => l.Code).ToArray());
                        foreach (MasterData item in resNetting.MasterDatas.Where(p => !val.Any(a => a.Value.Code == p.Code)))
                        {
                            reports.Add(new WorkingReport
                            {
                                LineNo = null,
                                FieldNo = definition.SectionCodeField.FieldIndex,
                                FieldName = definition.SectionCodeField.FieldName,
                                Message = $"相殺データに存在する{item.Code}：{item.Name}が存在しないため、インポートできません。",
                            });
                        }
                    });
                    return reports;
                };
                definition.PayerCodeLeftField.ValidateAdditional = (val, param) =>
                {
                    var reports = new List<WorkingReport>();
                    var uniqueKeys = new Dictionary<string, int>();
                    var duplicatedLines = new List<int>();
                    SectionsResult res = null;
                    ServiceProxyFactory.LifeTime(factory =>
                    {
                        var section = factory.Create<SectionMasterClient>();
                        res = section.GetImportItemsForSection(
                            SessionKey, CompanyId, val.Values.Select(l => l.PayerCodeLeft + l.PayerCodeRight).ToArray());
                    });
                    foreach (var pair in val)
                    {
                        var branchCodeIsEmpty = false;
                        var accountNumberIsEmpty = false;

                        if (pair.Value.PayerCodeLeft == "")
                        {
                            branchCodeIsEmpty = true;
                        }
                        if (pair.Value.PayerCodeRight == "")
                        {
                            accountNumberIsEmpty = true;
                        }

                        if ((branchCodeIsEmpty && !accountNumberIsEmpty)
                            || (!branchCodeIsEmpty && accountNumberIsEmpty))
                        {
                            StringFieldDefinition<Section> field = null;
                            var value = string.Empty;
                            if (branchCodeIsEmpty)
                            {
                                field = definition.PayerCodeLeftField;
                                value = pair.Value.PayerCodeLeft;
                            }
                            else
                            {
                                field = definition.PayerCodeRightField;
                                value = pair.Value.PayerCodeRight;
                            }
                            reports.Add(new WorkingReport
                            {
                                LineNo = pair.Key,
                                FieldNo = field.FieldIndex,
                                FieldName = field.FieldName,
                                Message = "仮想支店コード・仮想口座番号のどちらかが未入力のため、インポートできません。",
                            });
                        }
                        if (res.Sections.Count != 0)
                        {
                            if (((ImportMethod)param) == ImportMethod.InsertOnly)
                            {
                                reports.Add(new WorkingReport
                                {
                                    LineNo = pair.Key,
                                    FieldNo = definition.PayerCodeLeftField.FieldIndex,
                                    FieldName = "仮想支店コード、仮想口座番号",
                                    Message = "既に登録されている仮想支店コード、仮想口座番号のため、インポートできません。",
                                });
                            }
                            else
                            {
                                if (res.Sections.Any(p => p.Code != pair.Value.Code
                                && p.PayerCode == pair.Value.PayerCodeLeft + pair.Value.PayerCodeRight
                                && p.PayerCode != ""))
                                {
                                    reports.Add(new WorkingReport
                                    {
                                        LineNo = pair.Key,
                                        FieldNo = definition.PayerCodeLeftField.FieldIndex,
                                        FieldName = "仮想支店コード、仮想口座番号",
                                        Message = "既に登録されている仮想支店コード、仮想口座番号のため、インポートできません。",
                                    });
                                }
                            }
                        }
                        string key = pair.Value.PayerCodeLeft + pair.Value.PayerCodeRight;
                        if (key.Length == 10)
                        {
                            if (string.IsNullOrEmpty(key)) continue;
                            if (uniqueKeys.ContainsKey(key))
                            {
                                var duplicated = uniqueKeys[key];
                                if (uniqueKeys.ContainsKey(key))
                                {
                                    duplicatedLines.Add(duplicated);
                                }
                                duplicatedLines.Add(pair.Key);
                            }
                            else
                            {
                                uniqueKeys.Add(key, pair.Key);
                            }
                        }
                    }
                    duplicatedLines.ForEach(lineNo =>
                    {
                        reports.Add(new WorkingReport() // キー重複
                            {
                            LineNo = lineNo,
                            FieldNo = definition.PayerCodeLeftField.FieldIndex,
                            FieldName = "仮想支店コード、仮想口座番号",
                            Message = "仮想支店コード、仮想口座番号が重複しているため、インポートできません。",
                            Value = val[lineNo].PayerCodeLeft + val[lineNo].PayerCodeRight,
                        });
                    });
                    return reports;
                };
                var importer = definition.CreateImporter(m => new { m.Code });
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await LoadGridAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);

                var importResult = DoImport(importer, importSetting);
                if (!importResult) return;
                Clears();
                SectionList.Clear();
                Task<List<Section>> loadTask = LoadGridAsync();
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                SectionList.AddRange(loadTask.Result);
                grdSectionMaster.DataSource = new BindingSource(SectionList, null);

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<Section> imported)
        {
            ImportResultSection result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                result = await service.ImportAsync(SessionKey,
                    imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });

            return result;
        }

        [OperationLog("エクスポート")]
        private void Export()
        {
            try
            {
                ClearStatusMessage();
                List<Section> list = null;
                var serverPath = string.Empty;
                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    serverPath = await GetServerPath();
                });
                Task<List<Section>> loadTask = LoadGridAsync();
                ProgressDialog.Start(ParentForm, Task.WhenAll(task, loadTask), false, SessionKey);
                list = loadTask.Result;

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"入金部門マスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new SectionFileDefinition(new DataExpression(ApplicationControl));
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

            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                DispStatusMessage(MsgErrExportError);
            }
        }

        [OperationLog("印刷")]
        private void Print()
        {
            try
            {
                ClearStatusMessage();
                var loginUser = new List<LoginUser>();
                string serverPath = null;
                var salereport = new SectionMasterReport();
                var loginResult = new UsersResult();
                int sectionListCount = SectionList.Count;

                var task = ServiceProxyFactory.LifeTime(async factory =>
                {
                    serverPath = await GetServerPath();
                    var client = factory.Create<LoginUserMasterClient>();
                    loginResult = await client.GetItemsAsync(SessionKey, CompanyId, new LoginUserSearch());

                    if (sectionListCount > 0 && loginResult.ProcessResult.Result)
                    {
                        loginUser = loginResult.Users;
                        for (var i = 0; i < sectionListCount; i++)
                        {
                            SectionList[i].UpdateDate = SectionList[i].UpdateAt.ToString("yyyy/MM/dd");
                            for (var j = 0; j < loginUser.Count; j++)
                            {
                                if (SectionList[i].UpdateBy == loginUser[j].Id)
                                {
                                    SectionList[i].LoginUserName = loginUser[j].Name;
                                }
                            }
                        }
                        salereport.SetBasicPageSetting(Login.CompanyCode, Login.CompanyName);
                        salereport.Name = "入金部門マスター" + DateTime.Today.ToString("yyyyMMdd");
                        salereport.SetData(SectionList);
                        salereport.Run(true);
                    }
                });
                ProgressDialog.Start(ParentForm, Task.Run(() => task), false, SessionKey);

                if (sectionListCount > 0 && loginResult.ProcessResult.Result)
                {
                    ShowDialogPreview(ParentForm, salereport, serverPath);
                }
                else
                {
                    ShowWarningDialog(MsgWngPrintDataNotExist);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrCreateReportError);
            }
        }

        private async Task<string> GetServerPath()
        {
            var serverPath = string.Empty;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<GeneralSettingMasterClient>();
                GeneralSettingResult result = await service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");

                if (result.ProcessResult.Result)
                {
                    serverPath = result.GeneralSetting?.Value;
                }
            });
            return serverPath;
        }

        [OperationLog("終了")]
        private void Exit()
        {
            ClearStatusMessage();
            if (Modified
                && !ShowConfirmDialog(MsgQstConfirmClose))
                return;

            ParentForm.Close();
        }
        #endregion

        #region 画面のWeb Service
        public async Task<List<Section>> LoadGridAsync()
        {
            var result = new SectionsResult();
            List<Section> list = null;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                result = await service.GetByCodeAsync(SessionKey, CompanyId, Code: null);

                if (result.ProcessResult.Result)
                {
                    list = result.Sections;
                }
            });

            for (var i = 0; i < list.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(list[i].PayerCode))
                {
                    list[i].PayerCodeLeft = list[i].PayerCode.Substring(0, 3);
                    list[i].PayerCodeRight = list[i].PayerCode.Substring(3);
                }
            }

            return list ?? new List<Section>();
        }
        #endregion

        #region 画面の Validate
        private void txtSectionCode_Validated(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                if (!string.IsNullOrWhiteSpace(txtSectionCode.Text))
                    GetItemsByCode();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void GetItemsByCode()
        {
            var section = new Section();

            var task = ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<SectionMasterClient>();
                var result = await service.GetByCodeAsync(SessionKey, CompanyId, new string[] { txtSectionCode.Text });
                if (result.ProcessResult.Result && result.Sections.Any())
                {
                    section = result.Sections[0];
                }
            });
            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            if (section?.Code == null)
            {
                DispStatusMessage(MsgInfSaveNewData, "入金部門");
                return;
            }

            txtSectionCode.Text = section.Code;
            txtSectionName.Text = section.Name;
            txtNote.Text = section.Note;
            SectionId = section.Id;

            if (!string.IsNullOrWhiteSpace(section.PayerCode))
            {
                txtLeftPayerCode.Text = section.PayerCode.Substring(0, 3);
                txtRightPayerCode.Text = section.PayerCode.Substring(3, 7);
            }

            txtSectionCode.Enabled = false;
            BaseContext.SetFunction03Enabled(true);
            Modified = false;
        }

        private void GridCellDoubleData(int rowIndex)
        {
            Modified = false;
            txtSectionCode.Text = grdSectionMaster.Rows[rowIndex].Cells[CellName(nameof(Section.Code))].DisplayText;
            txtSectionName.Text = grdSectionMaster.Rows[rowIndex].Cells[CellName(nameof(Section.Name))].DisplayText;
            txtLeftPayerCode.Text = grdSectionMaster.Rows[rowIndex].Cells[CellName(nameof(Section.PayerCodeLeft))].DisplayText;
            txtRightPayerCode.Text = grdSectionMaster.Rows[rowIndex].Cells[CellName(nameof(Section.PayerCodeRight))].DisplayText;
            txtNote.Text = grdSectionMaster.Rows[rowIndex].Cells[CellName(nameof(Section.Note))].DisplayText;
            txtSectionCode.Enabled = false;
            SectionId = int.Parse(grdSectionMaster.Rows[rowIndex].Cells[CellName(nameof(Section.Id))].DisplayText);
        }

        private void grdSectionMaster_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData))
                    return;

                ClearStatusMessage();
                GridCellDoubleData(e.RowIndex);
                BaseContext.SetFunction03Enabled(true);
                txtSectionName.Select();
                Modified = false;
            }
        }

        private void grdSectionMaster_CellClick(object sender, CellEventArgs e)
        {
            for (var i = 0; i < grdSectionMaster.RowCount; i++)
            {
                grdSectionMaster.Rows[i].Cells[CellName("Header")].Style.BackColor = Color.WhiteSmoke;
                grdSectionMaster.Rows[i].Selectable = true;
            }
        }
        #endregion
    }
}
