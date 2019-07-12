using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.EBFileSettingMasterService;
using Rac.VOne.Client.Screen.ImportFileLogService;
using Rac.VOne.EbData;
using Rac.VOne.Message;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>入金EBデータ取込</summary>
    public partial class PD0101 : VOneScreenBase
    {

        private EbDataImporter Importer { get; set; }

        private bool EnableDoubleClick = true;

        /// <summary>EBファイル設定</summary>
        private List<EBFileSetting> EBFileSettings { get; set; }

        private string GetMesasge(string id, params string[] args)
        {
            return XmlMessenger.GetMessageInfo(id, args)?.Text;
        }

        private List<FileInformation> TargetFiles { get; set; }

        #region initialize

        public PD0101()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            grdImportItems.SetupShortcutKeys();
            grdHistory.SetupShortcutKeys();
            Text = "入金EBデータ取込";
        }

        private void InitializeHandlers()
        {
            Load += PD0101_Load;
            cmbDefaultEBFileSetting.SelectedIndexChanged += (sender, e) =>
            {
                Settings.SaveControlValue<PD0101>(ApplicationContext.Login,
                    cmbDefaultEBFileSetting.Name,
                    cmbDefaultEBFileSetting.SelectedIndex);
            };

            btnShowOpenFilesDialog.Click += btnShowOpenFilesDialog_Click;

            grdImportItems.CellDoubleClick                 += grdImportItems_CellDoubleClick;
            grdImportItems.CellEditedFormattedValueChanged += grdImportItems_CellEditedFormattedValueChanged;

            grdHistory.CellValueChanged                += grdHistory_CellValueChanged;
            grdHistory.CellEditedFormattedValueChanged += grdHistory_CellEditedFormattedValueChanged;

            grdHistory.DataBindingComplete += (sender, e) => {
                foreach (var row in grdHistory.Rows)
                {
                    var log = row.DataBoundItem as ImportFileLog;
                    if (log == null) continue;
                    row[CellName(nameof(ImportFileLog.DoDelete))].Enabled = log.Apportioned == 0;
                }
            };
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Shown += (sender, e) => {
                if (GetUseableEBFileSettings().Any()) return;
                ShowWarningDialog(MsgWngNotSettingMaster, "EBファイル設定");
                btnShowOpenFilesDialog.Enabled = false;
            };
        }

        private void PD0101_Load(object sender, EventArgs e)
        {
            SetScreenName();
            try
            {
                ProgressDialog.Start(ParentForm, InitializelLoadDataAsync(), false, SessionKey);

                InitializeHistoryGridTemplate();
                ClearControl();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private async Task InitializelLoadDataAsync()
        {
            var tasks = new List<Task> {
                LoadApplicationControlAsync(),
                LoadCompanyAsync(),
                LoadControlColorAsync(),
                LoadHistoryAsync(),
            };
            await Task.WhenAll(tasks);

            foreach (var task in tasks.Where(x => x.Exception != null))
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);

            await InitializeEBFileSettingSourceAsync();
        }

        private async Task InitializeEBFileSettingSourceAsync()
        {
            await LoadEBFileSettingsAsync();
            cmbDefaultEBFileSetting.Items.Clear();
            InitializeDefaultEbFileSettingtComboBox();
            InitializeImportItemsGridTemplate();
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("読込");
            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction03Caption("登録");
            BaseContext.SetFunction04Caption("削除");
            BaseContext.SetFunction07Caption("取込設定");
            BaseContext.SetFunction08Caption("対象外設定");
            BaseContext.SetFunction10Caption("終了");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction04Enabled(false);
            BaseContext.SetFunction07Enabled(true);
            BaseContext.SetFunction08Enabled(true);

            OnF01ClickHandler = Read;
            OnF02ClickHandler = Clear;
            OnF03ClickHandler = Import;
            OnF04ClickHandler = Delete;
            OnF07ClickHandler = ShowFileSetting;
            OnF08ClickHandler = ShowEBExcludeAccountSetting;
            OnF10ClickHandler = Close;
        }

        /// <summary>initialize default eb file setting combobox</summary>
        private void InitializeDefaultEbFileSettingtComboBox()
        {
            cmbDefaultEBFileSetting.Items.AddRange(GetUseableEBFileSettings()
                .Select(x => new GrapeCity.Win.Editors.ListItem(x.Name, x.Id)).ToArray());
        }

        private List<EBFileSetting> GetUseableEBFileSettings()
            => EBFileSettings
                .Where(x => x.IsUseable == 1)
                .OrderBy(x => x.DisplayOrder).ToList();

        /// <summary>initialize import items grid template.</summary>
        private void InitializeImportItemsGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var combo = GetMasterBoundComboBox(builder.GetComboBoxCell());
            builder.Items.AddRange(new []
            {
                new CellSetting(height,  40, "Header"                                                                                                     , cell: builder.GetRowHeaderCell()                 ),
                new CellSetting(height,  40, nameof(ImportItem.DoImport)        , dataField: nameof(ImportItem.DoImport)       , caption: "取込"          , cell: builder.GetCheckBoxCell(isBoolType:true) , readOnly: false),
                new CellSetting(height, 290, nameof(ImportItem.FilePath)        , dataField: nameof(ImportItem.FilePath)       , caption: "ファイルパス"  , cell: builder.GetTextBoxCell()                   ),
                new CellSetting(height, 240, nameof(ImportItem.EBFileSettingId) , dataField: nameof(ImportItem.EBFileSettingId), caption: "EBファイル設定", cell: combo                     , readOnly: false),
                new CellSetting(height, 100, nameof(ImportItem.ReadCount)       , dataField: nameof(ImportItem.ReadCount)      , caption: "読込件数"      , cell: builder.GetNumberCell()                    ),
                new CellSetting(height, 100, nameof(ImportItem.TargetCount)     , dataField: nameof(ImportItem.TargetCount)    , caption: "取込対象件数"  , cell: builder.GetNumberCell()                    ),
                new CellSetting(height, 120, nameof(ImportItem.Status)          , dataField: nameof(ImportItem.Status)         , caption: "取込状況"      , cell: builder.GetTextBoxCell()                   ),
            });

            grdImportItems.Template = builder.Build();
            grdImportItems.HideSelection = true;
        }

        /// <summary>ComboBoxCell For CapturePattern</summary>
        /// <param name="cell">ComboBoxCell</param>
        /// <returns>Binded ComboBoxCell</returns>
        private ComboBoxCell GetMasterBoundComboBox(ComboBoxCell cell)
        {
            var fileSettings = GetUseableEBFileSettings().ToDictionary(x => x.Id, x => x.Name);
            if (!fileSettings.Any()) return cell;
            cell.DataSource = new BindingSource(fileSettings, null);
            cell.DisplayMember = "Value";
            cell.ValueMember = "Key";
            return cell;
        }

        /// <summary>Setting For DisplayGrid </summary>
        private void InitializeHistoryGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            var celFileSize = builder.GetTextBoxCell(MultiRowContentAlignment.MiddleRight);
            celFileSize.Style.Format = "#,0 B";
            builder.Items.AddRange(new[] {
                new CellSetting(height,  40, "Header"      , cell: builder.GetRowHeaderCell()),
                new CellSetting(height,  40, nameof(ImportFileLog.DoDelete)    , dataField: nameof(ImportFileLog.DoDelete)    , caption: "削除"          , cell: builder.GetCheckBoxCell(isBoolType: true), readOnly: false),
                new CellSetting(height, 300, nameof(ImportFileLog.FileName)    , dataField: nameof(ImportFileLog.FileName)    , caption: "ファイル名"                                                      ),
                new CellSetting(height, 180, nameof(ImportFileLog.FileSize)    , dataField: nameof(ImportFileLog.FileSize)    , caption: "ファイルサイズ", cell: celFileSize ),
                new CellSetting(height, 135, nameof(ImportFileLog.CreateAt)    , dataField: nameof(ImportFileLog.CreateAt)    , caption: "取込日時"      , cell: builder.GetDateCell_yyyyMMddHHmmss()),
                new CellSetting(height, 120, nameof(ImportFileLog.ImportCount) , dataField: nameof(ImportFileLog.ImportCount) , caption: "件数"          , cell: builder.GetNumberCell()),
                new CellSetting(height, 120, nameof(ImportFileLog.ImportAmount), dataField: nameof(ImportFileLog.ImportAmount), caption: "金額"          , cell: builder.GetNumberCell()),
            });

            grdHistory.Template = builder.Build();
            grdHistory.HideSelection = true;
        }

        #endregion

        #region function keys

        [OperationLog("読込")]
        private void Read()
        {
            try
            {
                grdImportItems.EndEdit();
                ClearStatusMessage();

                if (!CheckValidate())
                    return;

                if (!ShowConfirmDialog(MsgQstConfirmReading))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                ReadFiles();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("クリア")]
        private void Clear()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClear)) return;

            ClearStatusMessage();
            ClearControl();
        }

        [OperationLog("登録")]
        private void Import()
        {
            try
            {
                ClearStatusMessage();
                if (!ShowConfirmDialog(MsgQstConfirmSave))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                ImportFiles();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                ClearStatusMessage();
                if (!ShowConfirmDialog(MsgQstConfirmDeleteXXX, "取込履歴データ")) return;

                DeleteImportFileLog();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("取込設定")]
        private void ShowFileSetting()
        {
            ClearStatusMessage();
            using (var form = ApplicationContext.Create(nameof(PH1302)))
            {
                var screen = form.GetAll<PH1302>().First();
                screen.ReturnScreen = this;

                var result = ApplicationContext.ShowDialog(ParentForm, form);
                //if (result != DialogResult.OK) return;
            }
            ProgressDialog.Start(ParentForm, InitializeEBFileSettingSourceAsync(), false, SessionKey);
        }

        [OperationLog("対象外設定")]
        private void ShowEBExcludeAccountSetting()
        {
            try
            {
                ClearStatusMessage();
                var form = ApplicationContext.Create(nameof(PD0103));
                var result = ApplicationContext.ShowDialog(ParentForm, form);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("終了")]
        private void Close()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;

            Importer?.Dispose();
            BaseForm.Close();
        }

        #endregion

        #region event handlers

        private void btnShowOpenFilesDialog_Click(object sender, EventArgs e)
        {
            var id = (int?)cmbDefaultEBFileSetting.SelectedItem?.SubItems[1].Value ?? -1;
            if (id == -1) return;
            try
            {

                this.ButtonClicked(btnShowOpenFilesDialog);

                var setting = GetUseableEBFileSettings().First(x => x.Id == id);

                var path = setting.FilePath; // GetInitilaDirectory();
                var fileNames = new List<string>();
                if (!LimitAccessFolder ?
                    !ShowOpenFileDialog(path, out fileNames, multiSelect: true, index: 1) :
                    !ShowRootFolderBrowserDialog(ApplicationControl.RootPath, out fileNames, FolderBrowserType.SelectMultiFile)) return;

                var source = GetBilindingSource();
                foreach (var file in fileNames)
                {
                    if (source.Any(x => x.FilePath == file)) continue;
                    source.Add(new ImportItem(id, file));
                }
                grdImportItems.DataSource = new BindingSource(source, null);

                foreach (var row in grdImportItems.Rows)
                {
                    var item = row.DataBoundItem as ImportItem;
                    if (item == null || !item.Executed) continue;
                    row[CellName(nameof(ImportItem.DoImport))].Enabled = false;
                    row[CellName(nameof(ImportItem.EBFileSettingId))].Enabled = false;
                }

                var ids = source.Where(x => x.DoImport).Select(x => x.EBFileSettingId).Distinct().ToArray();
                datYear.Enabled = EBFileSettings.Any(x => x.RequireYear == 1 && ids.Contains(x.Id));

                Modified = true;
                ClearStatusMessage();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private List<ImportItem> GetBilindingSource()
            => (grdImportItems.DataSource as BindingSource)?.DataSource as List<ImportItem> ?? new List<ImportItem>();

        private void grdImportItems_CellDoubleClick(object sender, CellEventArgs e)
        {
            if (!EnableDoubleClick || e.RowIndex < 0) return;
            if (e.CellName == CellName(nameof(ImportItem.Status)))
            {
                var item = grdImportItems.Rows[e.RowIndex].DataBoundItem as ImportItem;
                if (string.IsNullOrEmpty(item?.Status)) return;
                MessageBox.Show(item.Status);
            }
            if (e.CellName == CellName(nameof(ImportItem.FilePath)))
            {
                var path = (grdImportItems.Rows[e.RowIndex].DataBoundItem as ImportItem).FilePath;
                System.Diagnostics.Process.Start("notepad.exe", path);
            }
        }

        private void grdImportItems_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (!(e.CellName == CellName(nameof(ImportItem.DoImport))
                || e.CellName == CellName(nameof(ImportItem.EBFileSettingId)))) return;

            grdImportItems.CommitEdit();

            var settingIds = grdImportItems.Rows
                .Select(x => x.DataBoundItem as ImportItem)
                .Where(x => x.DoImport)
                .Select(x => x.EBFileSettingId).Distinct().ToArray();

            var requireYear = EBFileSettings.Any(x => x.IsUseable == 1 && x.RequireYear == 1 && settingIds.Contains(x.Id));
            datYear.Enabled = requireYear;
        }

        private void grdHistory_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            SetDeleteFunctionKeyEnabled();
        }

        private void grdHistory_CellValueChanged(object sender, CellEventArgs e)
        {
            SetDeleteFunctionKeyEnabled();
        }

        #endregion

        /// <summary>Clear Control In Form</summary>
        private void ClearControl()
        {
            grdImportItems.DataSource = null;
            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction07Enabled(true);

            btnShowOpenFilesDialog.Enabled = true;
            datYear.Enabled = false;
            datYear.Text = DateTime.Today.Year.ToString();
            EnableDoubleClick = true;
            cmbDefaultEBFileSetting.Enabled = true;

            if (cmbDefaultEBFileSetting.Items.Count > 0)
            {
                var sindex = Settings.RestoreControlValue<PD0101>(ApplicationContext.Login, cmbDefaultEBFileSetting.Name);
                var index = 0;
                if (!(int.TryParse(sindex, out index)
                    && 0 <= index && index < cmbDefaultEBFileSetting.Items.Count))
                {
                    index = 0;
                }
                cmbDefaultEBFileSetting.SelectedIndex = index;
            }
            this.ActiveControl = cmbDefaultEBFileSetting;
            cmbDefaultEBFileSetting.Focus();

            Modified = false;
        }

        /// <summary>Search CaptureHistory And Bind In DisplayGrid </summary>
        private async Task LoadHistoryAsync()
        {
            var logs = await GetImportFileLogsAsync();
            if (!logs.Any())
            {
                grdHistory.DataSource = null;
                return;
            }

            grdHistory.DataSource = new BindingSource(logs, null);
        }

        /// <summary>Delete ImportFileLog Data</summary>
        private void DeleteImportFileLog()
        {
            var importFileLogIds = grdHistory.Rows
                .Select(x => x.DataBoundItem as ImportFileLog)
                .Where(x => x.DoDelete)
                .Select(x => x.Id).ToArray();

            ImportFileLogsResult result = null;
            var task = ServiceProxyFactory.DoAsync(async (ImportFileLogServiceClient client) =>
            {
                result = await client.DeleteItemsAsync(SessionKey, importFileLogIds.ToArray());
                if (result.ProcessResult.Result)
                {
                    DispStatusMessage(MsgInfDeleteSuccess);
                    await LoadHistoryAsync();
                    BaseContext.SetFunction04Enabled(false);
                }
                else
                {
                    ShowWarningDialog(MsgErrDeleteError);
                }
            });

            ProgressDialog.Start(ParentForm, Task.WhenAll(task), false, SessionKey);
        }


        /// <summary>Validate Data For Reading CSV File </summary>
        /// <returns>Validated Result (true or false)</returns>
        private bool CheckValidate()
        {
            if (!grdImportItems.Rows.Any(x => (x.DataBoundItem as ImportItem)?.DoImport ?? false))
            {
                ShowWarningDialog(MsgWngSelectionRequired, "取込ファイル");
                return false;
            }
            if (datYear.Enabled && !datYear.ValidateInputted(() => ShowWarningDialog(MsgWngInputRequired, lblYear.Text))) return false;

            return true;

        }

        /// <summary>Read CSV File </summary>
        private void ReadFiles()
        {
            TargetFiles?.Clear();

            if (Importer == null) Importer = new EbDataImporter();
            Importer.Login = ApplicationContext.Login;
            if (datYear.Value.HasValue) Importer.Year = datYear.Value.Value.Year;

            var files = new List<FileInformation>();

            var items = grdImportItems.Rows.Select((row, index) => new { item = row.DataBoundItem as ImportItem, index })
                .Where(x => x.item.DoImport)
                .Select(x => {
                    var setting = GetUseableEBFileSettings().First(y => y.Id == x.item.EBFileSettingId);
                    return new FileInformation(x.index, x.item.FilePath, setting);
                });
            files.AddRange(items);

            var task = Task.Run(() =>
            {
                TargetFiles = Importer.ReadFiles(files);
            });

            ProgressDialog.Start(ParentForm, task, false, SessionKey);

            var anyFileImportable = TargetFiles.Any(x => x.Result == EbData.ImportResult.Success);

            foreach (var file in TargetFiles)
            {
                var messageId = string.Empty;
                var row = grdImportItems.Rows[file.Index];
                var item = row.DataBoundItem as ImportItem;
                item.Executed = true;
                if (file.Result == EbData.ImportResult.Success)
                {
                    item.ReadCount      = file.ImportFileLog.ReadCount;
                    item.TargetCount    = file.ImportFileLog.ImportCount;
                    messageId = MsgInfFinishFileReadingProcess;
                }
                else
                {
                    if (anyFileImportable) item.DoImport = false;

                    if (file.Result == EbData.ImportResult.FileNotFound             ) messageId = MsgWngOpenFileNotFound;
                    if (file.Result == EbData.ImportResult.FileReadError            ) messageId = MsgErrReadingError;
                    if (file.Result == EbData.ImportResult.FileFormatError          ) messageId = MsgErrReadingError;
                    if (file.Result == EbData.ImportResult.BankAccountMasterError   ) messageId = MsgWngNotExistBankAccountMaster;
                    if (file.Result == EbData.ImportResult.ImportDataNotFound       ) messageId = MsgWngNoReceiptData;
                    if (file.Result == EbData.ImportResult.BankAccountFormatError   ) messageId = MsgErrBankAccountFormatError;
                    if (file.Result == EbData.ImportResult.PayerCodeFormatError     ) messageId = MsgErrPayerCodeFormatError;
                }
                if (!string.IsNullOrEmpty(messageId))
                {
                    var message = GetMesasge(messageId);

                    if (file.Result == EbData.ImportResult.BankAccountMasterError)
                    {
                        message += Environment.NewLine + file.BankInformation;
                    }
                    item.Status = message;
                }
            }
            if (anyFileImportable)
            {
                foreach (var row in grdImportItems.Rows)
                {
                    row[CellName(nameof(ImportItem.DoImport))].Enabled = false;
                    row[CellName(nameof(ImportItem.EBFileSettingId))].Enabled = false;
                }
                cmbDefaultEBFileSetting.Enabled = false;
                btnShowOpenFilesDialog.Enabled = false;
                BaseContext.SetFunction01Enabled(false);
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction07Enabled(false);
            }
            grdImportItems.Focus();
        }
        private string CellName(string name) => $"cel{name}";

        /// <summary>import files </summary>
        private void ImportFiles()
        {
            var target = TargetFiles
                .Where(x => (grdImportItems.Rows[x.Index].DataBoundItem as ImportItem).DoImport)
                .ToList();
            if (!target.Any()) return;

            ProgressDialog.Start(ParentForm, async (cancel, progress) =>
            {
                var saveResults = await Task.Run(() => Importer.SaveFiles(target));
                foreach (var saveResult in saveResults)
                {
                    var messageId = string.Empty;
                    var row = grdImportItems.Rows[saveResult.Index];
                    var item = row.DataBoundItem as ImportItem;
                    var args = new List<string>();
                    if (saveResult.Result == EbData.ImportResult.Success)
                    {
                        row[CellName(nameof(ImportItem.DoImport))].Enabled = false;
                        item.DoImport = false;

                        BaseContext.SetFunction03Enabled(false);
                        EnableDoubleClick = false;
                        var moveResult = MoveFile(saveResult.Path);
                        if (moveResult)
                        {
                            messageId = MsgInfEBFileImportFinish;
                        }
                        else
                        {
                            messageId = MsgErrErrorFileHandling;
                            args.Add("");
                            args.Add("移動");
                        }
                    }
                    else
                    {
                        messageId = MsgErrEBFileImportError;
                    }
                    item.Status = GetMesasge(messageId, args.ToArray());
                }
                if (saveResults.All(x => x.Result == EbData.ImportResult.Success))
                {
                    await LoadHistoryAsync();
                    Modified = false;
                }
            }, false, SessionKey);
            grdImportItems.Focus();
        }

        /// <summary>Move File</summary>
        /// <returns>Moving Result(true or false)</returns>
        private bool MoveFile(string path)
        {
            var result = false;

            const string MoveFolderName = "取込済ファイル";
            var currenctDirectory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);
            var newDirectory = Path.Combine(currenctDirectory, MoveFolderName);

            try
            {
                if (!Directory.Exists(newDirectory))
                {
                    Directory.CreateDirectory(newDirectory);
                }
                var list = new List<string>();
                list.Add(fileName);
                list.Add($"_{DateTime.Now:yyyyMMddHHmmss}");

                if (!string.IsNullOrEmpty(extension))
                {
                    list.Add(extension);
                }

                var newFileName = string.Concat(list);
                var newPath = Path.Combine(newDirectory, newFileName);

                File.Move(path, newPath);
                result = true;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }
            return result;
        }


        /// <summary>
        /// Change Function Key Enabled Or Disabled from grid delete checkbox cells. 
        /// </summary>
        private void SetDeleteFunctionKeyEnabled()
        {
            grdHistory.EndEdit();
            var deletable = grdHistory.Rows.Any(x => (x.DataBoundItem as ImportFileLog).DoDelete);
            BaseContext.SetFunction04Enabled(deletable);
        }

        #region call web services

        private async Task<List<ImportFileLog>> GetImportFileLogsAsync()
            => await ServiceProxyFactory.DoAsync(async (ImportFileLogServiceClient client) => {
                var result = await client.GetHistoryAsync(SessionKey, CompanyId);
                if (result.ProcessResult.Result)
                    return result.ImportFileLogs;
                return new List<ImportFileLog>();
            });

        private async Task LoadEBFileSettingsAsync()
            => await ServiceProxyFactory.DoAsync(async (EBFileSettingMasterClient client) => {
                var result = await client.GetItemsAsync(SessionKey, CompanyId);
                EBFileSettings = (result.ProcessResult.Result)
                    ? result.EBFileSettings
                    : new List<EBFileSetting>();
            });

        #endregion

        /// <summary>EBデータ取込用 内部クラス</summary>
        public class ImportItem
        {
            /// <summary>取込有無</summary>
            public bool DoImport { get; set; } = true;
            /// <summary>ファイルパス</summary>
            public string FilePath { get; set; }
            /// <summary>EBファイル設定ID</summary>
            public int EBFileSettingId { get; set; }
            /// <summary>読込件数</summary>
            public int? ReadCount { get; set; }
            /// <summary>取込対象件数</summary>
            public int? TargetCount { get; set; }
            /// <summary>取込状況</summary>
            public string Status { get; set; }
            /// <summary>取込完了</summary>
            public bool Executed { get; set; }

            public ImportItem() { }
            public ImportItem(int id, string path) {
                EBFileSettingId = id;
                FilePath        = path;
            }
        }

    }

}
