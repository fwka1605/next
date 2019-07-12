using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrapeCity.Win.MultiRow;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Common.MultiRow;
using Rac.VOne.Client.Common.Settings;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class ProgressDialogState : Dialog
    {
        public bool AutoClose { get; set; }
        public TaskProgressManager Manager { get; set; }
        public System.Threading.CancellationTokenSource CancellationToken { get; set; } = new System.Threading.CancellationTokenSource();

        private Stopwatch stopwatch = new Stopwatch();
        private Timer timer = new Timer();
        public readonly Progress<bool> Progress;
        private string ElapsedTime { get { return $"{stopwatch.Elapsed:hh\\:mm\\:ss}"; } }

        public System.Action OnCancel { get; set; }

        public ProgressDialogState()
        {
            InitializeComponent();
            Progress = new Progress<bool>(TaskProgressStatusChanged);
            grid.HideSelection = true;
        }

        private void TaskProgressStatusChanged(bool taskCompleted)
        {
            OnProgressChanged(taskCompleted);
        }

        public void Initialize(bool cancellable)
        {
            this.Load += (s, e) =>
            {
                if (ColorContext != null)
                {
                    this.BackColor = ColorContext.FormBackColor;
                    this.ForeColor = ColorContext.FormForeColor;
                    this.InitializeColor(ColorContext);
                }

                var isCloudEdition = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsCloudEdition"]);
                Icon = (isCloudEdition) ? Properties.Resources.cloud_icon : Properties.Resources.app_icon;

                pnlDetail.Visible = RestoreControlValue(pnlDetail.Name);
                if (!pnlDetail.Visible)
                {
                    Size = new Size(Width, Height - (pnlDetail.Height));
                    btnDispDetail.Text = "詳細表示";
                }
                pnlSummary.BackColor = Color.White;
                lblSummary.BackColor = Color.White;
            };

            this.FormClosed += (s, e) =>
            {
                SaveControlValue(pnlDetail.Name, pnlDetail.Visible);
            };

            btnDispDetail.Click += (s, e) =>
            {
                pnlDetail.Visible = !pnlDetail.Visible;
                Size = new Size(Width, Height + pnlDetail.Height * (pnlDetail.Visible ? 1 : -1));
                btnDispDetail.Text = pnlDetail.Visible ? "詳細非表示" : "詳細表示";
            };

            timer.Interval = 1000;
            timer.Tick += (sender, e) =>
            {
                lblElapsedTime.Text = ElapsedTime;
            };

            stopwatch.Start();
            timer.Start();
            btnExecute.Text = "キャンセル";
            btnExecute.Enabled = cancellable;
            lblStartDateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            btnExecute.Click += (sender, e) =>
            {
                if (Manager.Status != TaskProgressState.InProcess)
                {
                    DialogResult
                        = Manager.Canceled ? DialogResult.Cancel
                        : Manager.Completed ? DialogResult.OK
                        :                     DialogResult.Abort;
                }
                else
                {
                    if (MessageBox.Show("処理をキャンセルしますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No) return;
                    if (Manager.Status == TaskProgressState.InProcess)
                    {
                        Manager.Canceled = true;
                        try
                        {
                            OnCancel?.Invoke();
                        }
                        catch (Exception ex)
                        {
                            Debug.Print(ex.Message);
                        }
                        CancellationToken.Cancel();
                        if (AutoClose)
                            DialogResult = DialogResult.Cancel;
                    }
                }
            };

            CreateTemplate();
            InitializeGrid();
        }

        private void SaveControlValue(string controlName, object value)
        {
            var info = new DialogInfo()
            {
                CompanyCode = Login.CompanyCode,
                UserCode = Login.UserCode,
                Key = $"{this.Name}.{controlName}",
                Value = value.ToString(),
            };
            Settings.Singleton.SaveInternal(info);
        }

        private bool RestoreControlValue(string controlName)
        {
            DialogInfo info = Settings.Singleton.ReadInternal<DialogInfo>(
                Login, d => d.Key == $"{this.Name}.{controlName}");
            var result = false;
            if (bool.TryParse(info?.Value, out result))
            {
                return result;
            }
            else
            {
                return false;
            }
        }

        private void CreateTemplate()
        {
            var builder = ApplicationContext.CreateGcMultirowTemplateBuilder(ColorContext, allowHorizontalResize: false);
            var height = builder.DefaultRowHeight;

            var imgCell = new ImageCell();
            imgCell.SupportsAnimation = true;
            imgCell.Selectable = false;

            builder.Items.AddRange(new CellSetting[]
            {
                new CellSetting(height,  40, "Icon"     , cell: imgCell ),
                new CellSetting(height, 200, "Text"     ),
                new CellSetting(height, 200, "TimeStamp")
            });
            var template = builder.Build();
            foreach(var header in template.ColumnHeaders)
            {
                header.Visible = false;
            }

            grid.Template = template;
            grid.ScrollBars = ScrollBars.Vertical;
            grid.SplitMode = SplitMode.None;
            grid.TabStop = false;
        }

        private enum eCellIndex
        {
            Icon = 0,
            Text,
            TimeStamp,
        }

        private void InitializeGrid()
        {
            grid.RowCount = Manager.TaskProgressList.Count;
            for(var i = 0; i <= grid.Rows.Count - 1; i++)
            {
                grid.SetValue(i, (int)eCellIndex.Text, Manager.TaskProgressList[i].Name);
                grid.SetValue(i, (int)eCellIndex.TimeStamp, "終了時刻　----/--/--　--:--:--");
            }
        }

        private void OnProgressChanged(bool taskCompleted)
        {
            prgBar.Value = Manager.OverallProgress;
            lblProgressPercentage.Text = Manager.OverallProgress.ToString() + "%";
            lblTaskProgress.Text = (Manager.CurrentTask.Weight == 1) ? "" : $"{Manager.CurrentTask.Complete:#,##0}/{Manager.CurrentTask.Weight:#,##0} 件";

            for(var i = 0; i <= Manager.TaskIndex; i++)
            {
                var task = Manager.TaskProgressList[i];
                switch (task.Status)
                {
                    case TaskProgressState.Completed:
                        grid.SetValue(i, (int)eCellIndex.Icon, Properties.Resources.check_16);
                        grid.SetValue(i, (int)eCellIndex.Text, task.Name + "　完了");
                        grid.SetValue(i, (int)eCellIndex.TimeStamp, $"終了時刻　{task.EndTime?.ToString("yyyy/MM/dd　HH:mm:ss")}");
                        break;
                    case TaskProgressState.InProcess:
                        grid.SetValue(i, (int)eCellIndex.Icon, Properties.Resources.loader_arrows);
                        grid.SetValue(i, (int)eCellIndex.Text, task.Name + "　処理中");
                        break;
                    case TaskProgressState.Error:
                        if (Manager.Canceled)
                            grid.SetValue(i, (int)eCellIndex.Icon, null);
                        else
                            grid.SetValue(i, (int)eCellIndex.Icon, Properties.Resources.error_16);
                        grid.SetValue(i, (int)eCellIndex.Text, task.Name);
                        break;
                    case TaskProgressState.Cancel:
                        grid.SetValue(i, (int)eCellIndex.Icon, null);
                        grid.SetValue(i, (int)eCellIndex.Text, task.Name);
                        break;
                }
            }

            if (Manager.CurrentTask.Status != TaskProgressState.InProcess)
            {
                stopwatch.Stop();
                timer.Stop();
                if (Manager.Canceled)
                {
                    lblSummary.Text = "処理をキャンセルしました。";
                    pbxIcon.Image = Properties.Resources.nodata_32;
                }
                else if (Manager.Completed)
                {
                    if (Manager.NoData)
                    {
                        pbxIcon.Image = Properties.Resources.nodata_32;
                        lblSummary.Text = "該当データは見つかりません。";
                    }
                    else
                    {
                        pbxIcon.Image = Properties.Resources.check_32;
                        lblSummary.Text = "全ての処理が完了しました。";
                    }
                }
                else if (Manager.CurrentTask.Status == TaskProgressState.Error)
                {
                    pbxIcon.Image = Properties.Resources.error_32;
                    lblSummary.Text = "処理中にエラーが発生しました。";
                }

                btnExecute.Text = "OK";
                btnExecute.Enabled = true;
                btnExecute.Select();
                lblElapsedTime.Text = ElapsedTime;

                if (Manager.Completed && AutoClose)
                {
                    Task.Delay(TimeSpan.FromMilliseconds(300)).Wait();
                    DialogResult = DialogResult.OK;
                    return;
                }
            }
            else
            {
                pbxIcon.Image = Properties.Resources.loader_indicator;
                lblSummary.Text = Manager.CurrentTask?.Name + " 処理中";
            }
        }
    }
}
