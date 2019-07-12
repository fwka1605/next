using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.GeneralSettingMasterService;
using Rac.VOne.Client.Screen.HolidayCalendarMasterService;
using Rac.VOne.Common.DataHandling;
using Rac.VOne.Export;
using Rac.VOne.Import;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;
using static Rac.VOne.Web.Models.FunctionType;

namespace Rac.VOne.Client.Screen
{
    /// <summary>カレンダーマスター</summary>
    public partial class PB1601 : VOneScreenBase
    {
        private string CellName(string value) => $"cel{value}";

        public PB1601()
        {
            InitializeComponent();
            grdHolidayCalendar.SetupShortcutKeys();
            Text = "カレンダーマスター";
        }

        #region 画面の初期化
        public void InitializeCalendarGrid()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Header"                                                                    , cell: builder.GetRowHeaderCell()                                       ),
                new CellSetting(height,  40, "Chk"                                                                       , cell: builder.GetCheckBoxCell()     , caption: "削除", readOnly: false ),
                new CellSetting(height, 115, nameof(HolidayCalendar.Holiday), dataField: nameof(HolidayCalendar.Holiday) , cell: builder.GetDateCell_yyyyMMdd(), caption: "休業日"                )
            });

            grdHolidayCalendar.Template = builder.Build();
            grdHolidayCalendar.HideSelection = true;
        }
        #endregion

        #region PB1601 InitializeFunctionKeys
        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("検索");
            BaseContext.SetFunction01Enabled(true);
            OnF01ClickHandler = Search;

            BaseContext.SetFunction02Caption("クリア");
            BaseContext.SetFunction02Enabled(true);
            OnF02ClickHandler = Clear;

            BaseContext.SetFunction03Caption("削除");
            BaseContext.SetFunction03Enabled(false);
            OnF03ClickHandler = Delete;

            BaseContext.SetFunction05Caption("インポート");
            OnF05ClickHandler = Import;

            BaseContext.SetFunction06Caption("エクスポート");
            OnF06ClickHandler = Export;

            BaseContext.SetFunction08Caption("全選択");
            BaseContext.SetFunction08Enabled(false);
            OnF08ClickHandler = SelectAll;

            BaseContext.SetFunction09Caption("全解除");
            BaseContext.SetFunction09Enabled(false);
            OnF09ClickHandler = DeselectAll;

            BaseContext.SetFunction10Caption("終了");
            OnF10ClickHandler = Exit;
        }
        #endregion

        /// <summary>Initial DateFormat</summary>
        private void InitialDateFormat()
        {
            var today = DateTime.Today;
            datFromHoliday.Value = new DateTime(today.Year, 01, 01);
            datToHoliday.Value = new DateTime(today.Year, 12, 31);
        }

        #region フォームロード
        private void PB1601_Load(object sender, EventArgs e)
        {
            try
            {
                SetScreenName();

                var loadTask = new List<Task>();
                loadTask.Add(LoadControlColorAsync());

                if (Company == null)
                {
                    loadTask.Add(LoadCompanyAsync());
                }

                if (ApplicationControl == null)
                {
                    loadTask.Add(LoadApplicationControlAsync());
                }

                loadTask.Add(LoadFunctionAuthorities(MasterImport, MasterExport));
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);
                InitializeCalendarGrid();
                Clear();

                BaseContext.SetFunction05Enabled(Authorities[MasterImport]);
                BaseContext.SetFunction06Enabled(Authorities[MasterExport]);

                this.ActiveControl = datFromHoliday;
                datFromHoliday.Focus();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }
        #endregion

        /// <summary>カレンダーデータ取得処理 </summary>
        /// <param name="option">HolidayCalendarSearch Model</param>
        /// <returns>HolidayCalendar</returns>
        private async Task<List<HolidayCalendar>> GetHolidayCalendarAsync(HolidayCalendarSearch option = null)
        {
            return await ServiceProxyFactory.DoAsync(async (HolidayCalendarMasterClient client) =>
            {
                if (option == null) option = new HolidayCalendarSearch { CompanyId = CompanyId };
                var result = await client.GetItemsAsync(SessionKey, option);
                if (result.ProcessResult.Result)
                    return result.HolidayCalendars;
                return new List<HolidayCalendar>();
            });
        }

        #region 画面の Save
        private void btnAddDate_Click(object sender, EventArgs e)
        {
            try
            {
                ClearStatusMessage();
                this.ButtonClicked(btnAddDate);

                if (!ValidateEntry())
                {
                    datAddDate.Select();
                    return;
                }

                bool succeeded = false;
                ProgressDialog.Start(ParentForm, async cancel =>
                {
                    succeeded = await SaveCalendar();

                    if (succeeded)
                    {
                        await SearchCalendarGrid();
                    }
                }, false, SessionKey);

                if (succeeded)
                {
                    datAddDate.Select();
                    DispStatusMessage(MsgInfSaveSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrSaveError);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>入力項目チェック</summary>
        /// <returns>bool</returns>
        private bool ValidateEntry()
        {
            if (!datAddDate.Value.HasValue)
            {
                ShowWarningDialog(MsgWngInputRequired, "追加する日付");
                return false;
            }

            if (datFromHoliday.Value.HasValue && datToHoliday.Value.HasValue)
            {
                if (!datFromHoliday.ValidateRange(datAddDate, () => ShowWarningDialog(MsgWngNotExistUpdateData,
                        datFromHoliday.Value.Value.ToShortDateString() + '～' +
                        datToHoliday.Value.Value.ToShortDateString() + "の日付"))) return false;
                if (!datAddDate.ValidateRange(datToHoliday, () => ShowWarningDialog(MsgWngNotExistUpdateData,
                        datFromHoliday.Value.Value.ToShortDateString() + '～' +
                        datToHoliday.Value.Value.ToShortDateString() + "の日付"))) return false;
            }

            if (!datFromHoliday.ValidateRange(datAddDate, () => ShowWarningDialog(MsgWngNotExistUpdateData,
                datFromHoliday.Value.Value.ToShortDateString() + '～' + "の日付"))) return false;

            if (!datAddDate.ValidateRange(datToHoliday, () => ShowWarningDialog(MsgWngNotExistUpdateData,
                '～' + datToHoliday.Value.Value.ToShortDateString() + "の日付"))) return false;

            var day = datAddDate.Value.Value.DayOfWeek;
            var addDate = datAddDate.Value.Value.ToShortDateString();

            Func<Row, bool> isExistHoliday = (row) =>(datAddDate.Value.HasValue
                && addDate.Equals((row.DataBoundItem as HolidayCalendar).Holiday.ToShortDateString()));

            if (grdHolidayCalendar.Rows.Any(x => isExistHoliday(x)))
            {
                ShowWarningDialog(MsgWngAlreadyRegistData, datAddDate.Value.Value.ToShortDateString());
                return false;
            }

            if ((day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday))
            {
                ShowWarningDialog(MsgWngNotExistUpdateData, "土曜日、日曜日以外");
                return false;
            }

            return true;
        }

        /// <summary>追加処理</summary>
        private async Task<bool> SaveCalendar()
        {
            var holidayCalendar = new HolidayCalendar();
            holidayCalendar.CompanyId = CompanyId;
            holidayCalendar.Holiday = datAddDate.Value.Value;
            var succeeded = false;

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<HolidayCalendarMasterClient>();
                HolidayCalendarResult result = await service.SaveAsync(SessionKey, holidayCalendar);

                succeeded = result.ProcessResult.Result && result.HolidayCalendar != null;
            });

            return succeeded;
        }
        #endregion

        #region 画面のF1
        [OperationLog("検索")]
        private void Search()
        {
            try
            {
                ClearStatusMessage();

                if (!ValidateSearch()) return;
                var loadTask = new List<Task>();
                loadTask.Add(SearchCalendarGrid());
                ProgressDialog.Start(ParentForm, Task.WhenAll(loadTask), false, SessionKey);

                datFromHoliday.Enabled = false;
                datToHoliday.Enabled = false;
                datAddDate.Enabled = true;
                btnAddDate.Enabled = true;
                datAddDate.Select();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>検索項目チェック</summary>
        /// <returns>bool</returns>
        private bool ValidateSearch()
        {
            if (datFromHoliday.Value.HasValue && datToHoliday.Value.HasValue)
            {
                if (!datFromHoliday.ValidateRange(datToHoliday, () => ShowWarningDialog(MsgWngInputRangeChecked, "対象期間")))
                {
                    datFromHoliday.Select();
                    return false;
                }
            }

            return true;
        }

        /// <summary>検索処理</summary>
        private async Task SearchCalendarGrid()
        {
            grdHolidayCalendar.Rows.Clear();

            var option = new HolidayCalendarSearch {
                CompanyId       = CompanyId,
                FromHoliday     = datFromHoliday.Value,
                ToHoliday       = datToHoliday.Value,
            };

            var list = await GetHolidayCalendarAsync(option);
            if (!list.Any())
            {
                BaseContext.SetFunction03Enabled(false);
                BaseContext.SetFunction08Enabled(false);
                BaseContext.SetFunction09Enabled(false);
            }
            else
            {
                grdHolidayCalendar.DataSource = new BindingSource(list, null);
                BaseContext.SetFunction03Enabled(true);
                BaseContext.SetFunction08Enabled(true);
                BaseContext.SetFunction09Enabled(true);
            }
        }
        #endregion

        #region 画面のF2
        [OperationLog("クリア")]
        private void Clear()
        {
            ClearStatusMessage();
            InitialDateFormat();
            datFromHoliday.Enabled = true;
            datToHoliday.Enabled = true;
            datAddDate.Enabled = false;
            btnAddDate.Enabled = false;
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction08Enabled(false);
            BaseContext.SetFunction09Enabled(false);
            datFromHoliday.Select();
            datAddDate.Clear();
            grdHolidayCalendar.Rows.Clear();
        }
        #endregion

        #region 画面の F3
        [OperationLog("削除")]
        private void Delete()
        {
            try
            {
                grdHolidayCalendar.EndEdit();
                ClearStatusMessage();

                if (!ValidateDeleteData()) return;

                if (!ShowConfirmDialog(MsgQstConfirmDelete))
                {
                    DispStatusMessage(MsgInfProcessCanceled);
                    return;
                }

                var task = DeleteDate();
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                bool succeeded = task.Result;
                if (succeeded)
                {
                    DispStatusMessage(MsgInfDeleteSuccess);
                }
                else
                {
                    ShowWarningDialog(MsgErrDeleteError);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        /// <summary>グリッドの「削除」セルのON / OFFチェック </summary>
        /// <returns>bool</returns>
        private bool IsChecked(Row row) => Convert.ToBoolean(row[CellName("Chk")].Value);

        /// <summary>削除データチェック</summary>
        /// <returns>bool</returns>
        private bool ValidateDeleteData()
        {
            if (!grdHolidayCalendar.Rows.Any(x => IsChecked(x)))
            {
                ShowWarningDialog(MsgWngSelectionRequired, "削除する休業日");
                return false;
            }

            ClearStatusMessage();
            return true;
        }

        /// <summary>削除処理</summary>
        private async Task<bool> DeleteDate()
        {
            bool succeeded = false;
            var checkHoliday = grdHolidayCalendar.Rows
                .Where(x => IsChecked(x))
                .Select(x => x.DataBoundItem as HolidayCalendar).Select(x => x.Holiday).ToArray();
            var deleteHoliday = checkHoliday.Cast<DateTime>().ToArray();

            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<HolidayCalendarMasterClient>();
                CountResult deleteResult = await service.DeleteAsync(SessionKey, CompanyId, deleteHoliday.ToArray());
                if (deleteResult.ProcessResult.Result && deleteResult.Count > 0)
                {
                    succeeded = true;
                    await SearchCalendarGrid();
                }
            });

            return succeeded;
        }
        #endregion

        #region 画面のF5
        [OperationLog("インポート")]
        private void Import()
        {
            ClearStatusMessage();
            try
            {
                ImportSetting importSetting = null;
                var task = Util.GetMasterImportSettingAsync(Login, ImportFileType.HolidayCalendar);
                ProgressDialog.Start(ParentForm, task, false, SessionKey);
                importSetting = task.Result;

                var definition = new HolidayCalendarFileDefinition(new DataExpression(ApplicationControl));

                var importer = definition.CreateImporter(m => m.Holiday);
                importer.UserId = Login.UserId;
                importer.UserCode = Login.UserCode;
                importer.CompanyId = CompanyId;
                importer.CompanyCode = Login.CompanyCode;
                importer.LoadAsync = async () => await GetHolidayCalendarAsync();
                importer.RegisterAsync = async unitOfWork => await RegisterForImportAsync(unitOfWork);

                DoImport(importer, importSetting, Clear);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
                ShowWarningDialog(MsgErrImportErrorWithoutLog);
            }
        }

        /// <summary>インポートデータ登録</summary>
        /// <param name="imported">CSVから生成したModel</param>
        /// <returns>登録結果</returns>
        private async Task<ImportResult> RegisterForImportAsync(UnitOfWork<HolidayCalendar> imported)
        {
            ImportResult result = null;
            await ServiceProxyFactory.LifeTime(async factory =>
            {
                var service = factory.Create<HolidayCalendarMasterClient>();

                result = await service.ImportAsync(SessionKey, imported.New.ToArray(), imported.Dirty.ToArray(), imported.Removed.ToArray());
            });
            return result;
        }
        #endregion

        #region 画面のF6
        [OperationLog("エクスポート")]
        private void Export()
        {
            ClearStatusMessage();

            try
            {
                var loadTask = GetHolidayCalendarAsync();
                ProgressDialog.Start(ParentForm, loadTask, false, SessionKey);
                var list = loadTask.Result;

                if (!list.Any())
                {
                    ShowWarningDialog(MsgWngNoExportData);
                    return;
                }

                string serverPath = null;
                ServiceProxyFactory.LifeTime(factory =>
                {
                    var service = factory.Create<GeneralSettingMasterClient>();
                    var task = service.GetByCodeAsync(SessionKey, CompanyId, "サーバパス");
                    ProgressDialog.Start(ParentForm, task, false, SessionKey);

                    if (task.Result.ProcessResult.Result)
                    {
                        serverPath = task.Result.GeneralSetting?.Value;
                    }
                });

                if (!Directory.Exists(serverPath))
                {
                    serverPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                var filePath = string.Empty;
                var fileName = $"カレンダーマスター{DateTime.Today:yyyyMMdd}.csv";
                if (!ShowSaveExportFileDialog(serverPath, fileName, out filePath)) return;

                var definition = new HolidayCalendarFileDefinition(new DataExpression(ApplicationControl));
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
        #endregion

        #region 画面の F8
        [OperationLog("全選択")]
        private void SelectAll()
        {
            ClearStatusMessage();
            grdHolidayCalendar.EndEdit();
            for (var i = 0; i < grdHolidayCalendar.RowCount; i++)
            {
                (grdHolidayCalendar.Rows[i].Cells[CellName("Chk")] as CheckBoxCell).Value = 1;
            }
        }
        #endregion

        #region 画面の F9
        [OperationLog("全解除")]
        private void DeselectAll()
        {
            ClearStatusMessage();
            grdHolidayCalendar.EndEdit();
            for (var i = 0; i < grdHolidayCalendar.RowCount; i++)
            {
                (grdHolidayCalendar.Rows[i].Cells[CellName("Chk")] as CheckBoxCell).Value = 0;
            }
        }
        #endregion

        #region 画面のF10
        [OperationLog("終了")]
        private void Exit()
        {
            ParentForm.Close();
        }
        #endregion
    }
}
