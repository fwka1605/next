using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Screen.ColumnNameSettingMasterService;
using Rac.VOne.Client.Screen.Dialogs;
using Rac.VOne.Client.Screen.ExportFieldSettingMasterService;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Rac.VOne.Message.Constants;

namespace Rac.VOne.Client.Screen
{
    /// <summary>出力項目設定</summary>
    public partial class PH9904 : VOneScreenBase
    {
        /// <summary>
        /// 出力ファイル種別
        ///  1 : 消込済み入金データ
        ///  2 : 請求書発行データ
        /// </summary>
        internal int ExportFileType { get; set; }
        private int DragDropDestRowIndex { get; set; } = -1;
        private List<ColumnNameSetting> BillingColumnName { get; set; } = new List<ColumnNameSetting>();
        private List<ColumnNameSetting> ReceiptColumnName { get; set; } = new List<ColumnNameSetting>();

        #region initialize

        public PH9904()
        {
            InitializeComponent();
            InitializeUserComponent();
            InitializeHandlers();
        }

        private void InitializeUserComponent()
        {
            grid.SetupShortcutKeys();
            grid.MultiSelect = true;
            FormWidth = 400;
            FormHeight = 610;
            FunctionKeysSetter = buttons =>
            {
                foreach (var button in buttons)
                {
                    if (button.Name == "btnF01"
                    || button.Name == "btnF02")
                        button.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    else if (button.Name == "btnF10")
                        button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    else
                        button.Visible = false;
                }
            };
        }

        private void InitializeHandlers()
        {
            grid.MouseDown += grid_MouseDown;
            grid.DragLeave += grid_DragLeave;
            grid.DragDrop += grid_DragDrop;
            grid.DragOver += grid_DragOver;
            grid.SectionPainting += grid_SectionPainting;
            grid.CurrentCellDirtyStateChanged += grid_CurrentCellDirtyStateChanged;
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            if (ParentForm == null) return;
            ParentForm.Load += PH9904_Load;
        }

        protected override void InitializeFunctionKeys()
        {
            base.InitializeFunctionKeys();

            BaseContext.SetFunction01Caption("登録");
            BaseContext.SetFunction02Caption("再表示");
            BaseContext.SetFunction03Caption("");
            BaseContext.SetFunction10Caption("戻る");

            BaseContext.SetFunction01Enabled(true);
            BaseContext.SetFunction02Enabled(true);
            BaseContext.SetFunction03Enabled(false);
            BaseContext.SetFunction10Enabled(true);

            OnF01ClickHandler = Register;
            OnF02ClickHandler = Reload;
            OnF10ClickHandler = Exit;
        }

        private void PH9904_Load(object sender, EventArgs e)
        {
            try
            {
                var name = string.Empty;
                switch (ExportFileType)
                {
                    case 1: name = "消込済み入金データ"; break;
                    case 2: name = "請求書発行データ"; break;
                }
                lblExportFileName.Text = name;
                InitializeDateFormatCombobox();
                InitializeGridTemplate();
                ProgressDialog.Start(ParentForm, InitializeLoadDataAsync(), false, SessionKey);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        private void InitializeDateFormatCombobox()
        {
            cmbDateFormat.Items.Clear();
            cmbDateFormat.Items.Add(new ListItem("0：yyyy/MM/dd", 0));
            cmbDateFormat.Items.Add(new ListItem("1：yy/MM/dd", 1));
            cmbDateFormat.Items.Add(new ListItem("2：yyyyMMdd", 2));
            cmbDateFormat.Items.Add(new ListItem("3：yyMMdd", 3));
        }

        private void InitializeGridTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext);
            var height = builder.DefaultRowHeight;
            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "ColumnOrder"   , caption: "順序", cell: builder.GetRowHeaderCell() ),
                new CellSetting(height,  40, "AllowExport"   , caption: "出力", readOnly: false, cell: builder.GetCheckBoxCell()),
                new CellSetting(height, 250, "Caption"       , caption: "項目名"),
                new CellSetting(height,   0, "ColumnName"    , caption: "ColumnName"),
                new CellSetting(height,   0, "ExportFileType", caption: "ExportFileType"),
                new CellSetting(height,   0, "DataType"      , caption: "DataType"),
            });

            grid.Template = builder.Build();
        }

        private async Task InitializeLoadDataAsync()
        {
            await Task.WhenAll(
                LoadCompanyAsync(),
                LoadApplicationControlAsync(),
                LoadControlColorAsync(),
                LoadColumnNameSettingAsync(),
                InitializeExportFieldSettingsAsync());
        }

        private async Task InitializeExportFieldSettingsAsync()
        {
            SetExportFieldSettings(await GetExportFieldSettingAsync());
        }

        #endregion

        #region ファンクションキー

        [OperationLog("登録")]
        private void Register()
        {
            ClearStatusMessage();

            if (!ValidateInputValues()) return;

            if (!ShowConfirmDialog(MsgQstConfirmSave))
            {
                DispStatusMessage(MsgInfProcessCanceled);
                return;
            }
            try
            {
                var result = SaveExportFieldSetting();
                if (!result)
                {
                    ShowWarningDialog(MsgErrSaveError);
                    return;
                }
                DispStatusMessage(MsgInfSaveSuccess);
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("再表示")]
        private void Reload()
        {
            ClearStatusMessage();

            if (Modified && !ShowConfirmDialog(MsgQstConfirmUpdateData)) return;

            try
            {
                ProgressDialog.Start(ParentForm, InitializeExportFieldSettingsAsync(), false, SessionKey);
                Modified = false;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                NLogHandler.WriteErrorLog(this, ex, SessionKey);
            }
        }

        [OperationLog("戻る")]
        private void Exit()
        {
            if (Modified && !ShowConfirmDialog(MsgQstConfirmClose)) return;
            ParentForm.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region WebService
        private async Task<List<ExportFieldSetting>> GetExportFieldSettingAsync()
        {
            var result = await ServiceProxyFactory.DoAsync(async (ExportFieldSettingMasterClient client)
                => await client.GetItemsByExportFileTypeAsync(SessionKey, CompanyId, ExportFileType));
            if (result.ProcessResult.Result)
                return result.ExportFieldSettings;
            return new List<ExportFieldSetting>();
        }

        private async Task LoadColumnNameSettingAsync()
        {
            var result = await ServiceProxyFactory.DoAsync(async (ColumnNameSettingMasterClient client)
                => await client.GetItemsAsync(SessionKey, CompanyId));
            if (result.ProcessResult.Result && result.ColumnNames != null)
            {
                BillingColumnName = result.ColumnNames.Where(x => x.TableName == "Billing").ToList();
                ReceiptColumnName = result.ColumnNames.Where(x => x.TableName == "Receipt").ToList();
            }
        }

        #endregion

        #region グリッド表示

        private void SetExportFieldSettings(List<ExportFieldSetting> settings)
        {
            var foreignCurrencyFilter = new[] { "CurrencyCode" };
            if (!UseForeignCurrency)
                settings.RemoveAll(x => foreignCurrencyFilter.Any(a => a == x.ColumnName));

            var sectionFilter = new[] { "SectionCode", "SectionName" };
            if (!UseSection)
                settings.RemoveAll(x => sectionFilter.Any(a => a == x.ColumnName));

            var fields = settings.Where(x => x.IsStandardField).ToList();
            grid.RowCount = fields.Count;
            for (var i = 0; i < fields.Count; i++)
            {
                var row = grid.Rows[i];
                var caption = GetColumnNameAlias(fields[i].ColumnName);
                row["celAllowExport"]   .Value = fields[i].AllowExport;
                row["celCaption"]       .Value = string.IsNullOrEmpty(caption) ? fields[i].Caption : caption;
                row["celColumnName"]    .Value = fields[i].ColumnName;
                row["celExportFileType"].Value = fields[i].ExportFileType;
                row["celDataType"]      .Value = fields[i].DataType;
            }
            cmbDateFormat.SelectedIndex = fields.FirstOrDefault(x => x.DataType == 1)?.DataFormat ?? -1;

            cbxRequireHeader.Checked = settings.FirstOrDefault(x => x.ColumnName == "RequireHeader")?.AllowExport == 1;
        }

        private string GetColumnNameAlias(string columnName)
        {
            if (columnName == "BillingNote1") return BillingColumnName.Find(x => x.ColumnName == "Note1")?.Alias;
            if (columnName == "BillingNote2") return BillingColumnName.Find(x => x.ColumnName == "Note2")?.Alias;
            if (columnName == "BillingNote3") return BillingColumnName.Find(x => x.ColumnName == "Note3")?.Alias;
            if (columnName == "BillingNote4") return BillingColumnName.Find(x => x.ColumnName == "Note4")?.Alias;
            if (columnName == "ReceiptNote1") return ReceiptColumnName.Find(x => x.ColumnName == "Note1")?.Alias;
            if (columnName == "ReceiptNote2") return ReceiptColumnName.Find(x => x.ColumnName == "Note2")?.Alias;
            if (columnName == "ReceiptNote3") return ReceiptColumnName.Find(x => x.ColumnName == "Note3")?.Alias;
            if (columnName == "ReceiptNote4") return ReceiptColumnName.Find(x => x.ColumnName == "Note4")?.Alias;
            return string.Empty;
        }

        #endregion

        #region grid evetn handlers ドラッグ＆ドロップ

        private void grid_MouseDown(object sender, MouseEventArgs e)
        {
            if (grid.CurrentCellPosition.CellIndex != 2) return;

            var hitTestInfo = grid.HitTest(e.X, e.Y);

            if (hitTestInfo.Type == HitTestType.Row)
            {
                grid.DoDragDrop(grid.Rows[hitTestInfo.SectionIndex], DragDropEffects.Move);
            }
        }

        private void grid_DragLeave(object sender, EventArgs e)
        {
            grid.Invalidate();
            DragDropDestRowIndex = -1;
        }

        private void grid_DragDrop(object sender, DragEventArgs e)
        {
            var clientPoint = grid.PointToClient(new System.Drawing.Point(e.X, e.Y));
            var hitTestInfo = grid.HitTest(clientPoint.X, clientPoint.Y);
            var sourceRow = (Row)e.Data.GetData(typeof(Row));

            if (sourceRow.GcMultiRow.Name != grid.Name)
            {
                DragDropDestRowIndex = -1; return;
            }
            var cellValues = new object[sourceRow.Cells.Count];

            for (var j = 0; j < sourceRow.Cells.Count; j++)
            {
                cellValues[j] = sourceRow.Cells[j].Value;
            }

            if (hitTestInfo.SectionIndex != -1)
            {
                if (sourceRow.Index < hitTestInfo.SectionIndex)
                {
                    grid.Rows.Insert(hitTestInfo.SectionIndex + 1, cellValues);
                }
                else
                {
                    grid.Rows.Insert(hitTestInfo.SectionIndex, cellValues);
                }
                if (DragDropDestRowIndex == -1)
                {
                    DragDropDestRowIndex = 0;
                }
                grid.Rows[DragDropDestRowIndex].Selected = true;
                grid.CurrentCellPosition = new CellPosition(DragDropDestRowIndex, "celCaption");
            }
            else
            {
                grid.Rows.Add(cellValues);
                grid.Rows[grid.RowCount - 1].Selected = true;
                grid.CurrentCellPosition = new CellPosition(grid.RowCount - 1, "celCaption");

            }
            sourceRow.GcMultiRow.Rows.Remove(sourceRow);

            for (var i = 0; i < grid.Rows.Count; i++)
            {
                grid.Rows[i]["celColumnOrder"].Value = i + 1;
            }

            grid.Invalidate();

            DragDropDestRowIndex = -1;
        }

        private void grid_DragOver(object sender, DragEventArgs e)
        {
            Modified = true;
            e.Effect = DragDropEffects.Move;
            var clientPoint = grid.PointToClient(new Point(e.X, e.Y));
            var hitTestInfo = grid.HitTest(clientPoint.X, clientPoint.Y);
            var newRowIndex = 0;
            var sourceRow = (Row)e.Data.GetData(typeof(Row));
            if (hitTestInfo.Type == HitTestType.Row)
            {
                if (sourceRow.Index < hitTestInfo.SectionIndex)
                {
                    DragDropDestRowIndex = hitTestInfo.SectionIndex + 1;
                }
                else
                {
                    DragDropDestRowIndex = hitTestInfo.SectionIndex;
                }
            }
            else if (hitTestInfo.Type == HitTestType.None)
            {
                DragDropDestRowIndex = grid.RowCount;
            }
            else if (hitTestInfo.Type == HitTestType.HorizontalScrollBar)
            {
                var topPos = grid.FirstDisplayedCellPosition;
                if (topPos.RowIndex < grid.RowCount)
                {
                    newRowIndex = topPos.RowIndex + 1;
                }
                else
                {
                    newRowIndex = topPos.RowIndex;
                }
                grid.FirstDisplayedCellPosition = new CellPosition(newRowIndex, topPos.CellIndex);
            }
            else if (hitTestInfo.Type == HitTestType.ColumnHeader)
            {
                var topPos = grid.FirstDisplayedCellPosition;

                if (topPos.RowIndex != 0)
                {
                    newRowIndex = topPos.RowIndex < 0 ? 0 : topPos.RowIndex - 1;
                    grid.FirstDisplayedCellPosition = new CellPosition(newRowIndex, topPos.CellIndex);
                }
            }
            grid.Invalidate();
        }

        /// <summary>Drag & Drop 時の強調表示用</summary>
        private void grid_SectionPainting(object sender, SectionPaintingEventArgs e)
        {
            if (e.Scope != CellScope.Row) return;

            using (var redPen = new Pen(Color.Red, 3))
            {
                if (e.SectionIndex == DragDropDestRowIndex)
                {
                    e.PaintSectionBackground(e.SectionBounds);
                    e.PaintSectionBorder(e.SectionBounds);
                    e.PaintCells(e.SectionBounds);

                    if (grid.CurrentCellPosition.CellIndex == 2)
                    {
                        e.Graphics.DrawLine(redPen, e.SectionBounds.Left, e.SectionBounds.Top,
                            e.SectionBounds.Right, e.SectionBounds.Top);
                    }
                    e.Handled = true;
                }
                else if (DragDropDestRowIndex == grid.RowCount && grid.CurrentCellPosition.CellIndex == 2)
                {
                    e.Graphics.DrawLine(redPen, e.SectionBounds.Left, e.SectionBounds.Bottom,
                            e.SectionBounds.Right, e.SectionBounds.Bottom);
                }
            }
        }

        private void grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        #endregion

        #region save

        private bool ValidateInputValues()
        {
            if (!grid.Rows.Any(x => Convert.ToBoolean(x["celAllowExport"].EditedFormattedValue)))
            {
                ShowWarningDialog(MsgWngSelectAnyOutputItem);
                return false;
            }
            return true;
        }

        private bool SaveExportFieldSetting()
        {
            var settings = GetExportFieldSettingsFromScreen();
            var task = ServiceProxyFactory.DoAsync(async (ExportFieldSettingMasterClient client)
                => await client.SaveAsync(SessionKey, settings.ToArray()));
            ProgressDialog.Start(ParentForm, task, false, SessionKey);
            if (task.Exception != null)
            {
                NLogHandler.WriteErrorLog(this, task.Exception, SessionKey);
                return false;
            }
            if (!task.Result.ProcessResult.Result)
            {
                return false;
            }
            return true;
        }

        private List<ExportFieldSetting> GetExportFieldSettingsFromScreen()
        {
            var settings = new List<ExportFieldSetting>();
            foreach (var row in grid.Rows)
            {
                var setting = new ExportFieldSetting {
                    CompanyId       = CompanyId,
                    ColumnName      = row.Cells["celColumnName"].Value.ToString(),
                    Caption         = row.Cells["celCaption"].Value.ToString(),
                    ColumnOrder     = row.Index + 1,
                    AllowExport     = int.Parse(row.Cells["celAllowExport"].Value.ToString()),
                    ExportFileType  = int.Parse(row.Cells["celExportFileType"].Value.ToString()),
                    UpdateBy        = Login.UserId,
                };
                if (int.Parse(row.Cells["celDataType"].Value.ToString()) == 1)
                    setting.DataFormat = Convert.ToInt32(cmbDateFormat.SelectedItem.SubItems[1].Value);

                settings.Add(setting);
            }
            settings.Add(new ExportFieldSetting
            {
                CompanyId       = CompanyId,
                ColumnName      = "RequireHeader",
                Caption         = cbxRequireHeader.Text,
                ColumnOrder     = 0,
                AllowExport     = cbxRequireHeader.Checked ? 1 : 0,
                ExportFileType  = ExportFileType,
                UpdateBy        = Login.UserId,
            });
            return settings;
        }

        #endregion

    }
}
