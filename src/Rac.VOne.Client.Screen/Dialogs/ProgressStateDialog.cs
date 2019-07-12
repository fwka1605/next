using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client.Common;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class ProgressStateDialog : Form
    {
        protected IColors ColorContext
        {
            get { return ColorSetting.Current; }
        }

        #region 従来
        private readonly string statusMessageTimer = "処理中{0}";
        //private readonly string statusMessageProgress = "処理中 ： {0}";

        public ProgressStateDialog()
        {
            InitializeComponent();
            this.Load += (s, e) =>
            {
                if (ColorContext != null)
                {
                    this.BackColor = ColorContext.FormBackColor;
                    this.ForeColor = ColorContext.FormForeColor;
                    this.InitializeColor(ColorContext);
                }
                this.initializeFont(System.Configuration.ConfigurationManager.AppSettings["FontFamilyName"]);
            };
        }

        private int count = 0;
        private void InitializeTimer()
        {
            count = 0;
            timer.Start();
        }

        private void FinalizeTimer()
        {
            timer.Stop();
        }

        private void ProgressStateDialog_Shown(object sender, EventArgs e)
        {
            InitializeTimer();
        }

        private void timer_tick(object sender, System.EventArgs e)
        {
            //prgBar.Value = ((prgBar.Value + 1) % 101);
            count = (count >= 100) ? 0 : count + 1;
            SetStatus(string.Format(statusMessageTimer, new string('.', count % 10)));
        }

        private void SetStatus(string value)
        {
            lblStatus.Text = value;
        }
        #endregion

        public static DialogResult Start(IWin32Window parent, Task task, bool cancellable, string sessionKey)
        {
            if (task == null || task.IsCompleted) return DialogResult.OK;

            using (var dialog = new ProgressStateDialog())
            using (var cancel = new System.Threading.CancellationTokenSource())
            {
                dialog.btnCancel.Visible = cancellable;
                dialog.btnCancel.Enabled = cancellable;

                dialog.btnCancel.Click += (sender, e) =>
                {
                    if (MessageBox.Show("処理をキャンセルしますか？",
                            "確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1) == DialogResult.No) return;

                    cancel.Cancel();
                    dialog.btnCancel.Enabled = false;
                    dialog.DialogResult = DialogResult.Cancel;
                };
                dialog.Shown += (sender, e) =>
                {
                    task.ContinueWith(t =>
                    {
                        bool cancelled = cancel.IsCancellationRequested;
                        dialog.DialogResult = cancelled ? DialogResult.Cancel : DialogResult.OK;
                    });
                };

                dialog.ShowDialog(parent);
                try
                {
                    task.Wait(cancel.Token);
                }
                catch (AggregateException ex)
                {
                    //Debug.Fail(ex.StackTrace);
                    foreach (Exception inner in ex.InnerExceptions)
                    {
                        //Debug.Fail(ex.ToString());
                        NLogHandler.WriteErrorLog(new ProgressStateDialog(), inner, sessionKey);
                    }
                    return DialogResult.Abort;
                }
                catch (OperationCanceledException)
                {
                }
                return dialog.DialogResult;
            }
        }

        public static DialogResult Start(IWin32Window parent,
                Func<System.Threading.CancellationToken, Task> transaction,
                bool cancellable, string sessionKey)
        {
            using (var dialog = new ProgressStateDialog())
            using (var cancel = new System.Threading.CancellationTokenSource())
            {
                dialog.btnCancel.Click += (sender, e) =>
                {
                    if (MessageBox.Show("処理をキャンセルしますか？",
                            "確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1) == DialogResult.No) return;

                    cancel.Cancel();
                    dialog.btnCancel.Enabled = false;
                    dialog.DialogResult = DialogResult.Cancel;
                };

                Task task = transaction(cancel.Token);
                if (task == null || task.IsCompleted) return DialogResult.OK;

                dialog.Shown += (sender, e) =>
                {
                    dialog.btnCancel.Enabled = cancellable;
                    task.ContinueWith(t =>
                    {
                        bool cancelled = cancel.IsCancellationRequested;
                        dialog.DialogResult = cancelled ? DialogResult.Cancel : DialogResult.OK;
                    });
                };
                dialog.ShowDialog(parent);
                try
                {
                    task.Wait(cancel.Token);
                }
                catch (AggregateException ex)
                {
                    NLogHandler.WriteErrorLog(new ProgressStateDialog(), ex, sessionKey);
                    return DialogResult.Abort;
                }
                catch (OperationCanceledException)
                {
                }
                return dialog.DialogResult;
            }
        }

        public static DialogResult Start(IWin32Window parent,
                Func<System.Threading.CancellationToken, IProgress<int>, Task> transaction,
                bool cancellable, string sessionKey)
        {
            using (var dialog = new ProgressStateDialog())
            using (var cancel = new System.Threading.CancellationTokenSource())
            {
                dialog.btnCancel.Click += (sender, e) =>
                {
                    if (MessageBox.Show("処理をキャンセルしますか？",
                            "確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1) == DialogResult.No) return;

                    cancel.Cancel();
                    dialog.btnCancel.Enabled = false;
                    dialog.DialogResult = DialogResult.Cancel;
                };

                // プログレスバーの更新は、非同期処理に任せる
                IProgress<int> progress = new Progress<int>(percent => dialog.prgBar.Value = percent);
                progress.Report(0);
                Task task = transaction(cancel.Token, progress);
                if (task == null || task.IsCompleted) return DialogResult.OK;

                dialog.Shown += (sender, e) =>
                {
                    dialog.btnCancel.Enabled = cancellable;
                    task.ContinueWith(t =>
                    {
                        bool cancelled = cancel.IsCancellationRequested;
                        dialog.DialogResult = cancelled ? DialogResult.Cancel : DialogResult.OK;
                    });
                };
                dialog.ShowDialog(parent);
                try
                {
                    task.Wait(cancel.Token);
                }
                catch (AggregateException ex)
                {
                    NLogHandler.WriteErrorLog(new ProgressStateDialog(), ex, sessionKey);
                    return DialogResult.Abort;
                }
                catch (OperationCanceledException)
                {
                }
                return dialog.DialogResult;
            }
        }
    }
}
