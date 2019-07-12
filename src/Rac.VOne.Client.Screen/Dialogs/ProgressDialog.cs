using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Client.Common;

namespace Rac.VOne.Client.Screen.Dialogs
{
    public partial class ProgressDialog : Form
    {
        #region 従来
        private readonly string StatusMessageTimer = "処理中{0}";

        public ProgressDialog()
        {
            InitializeComponent();
        }

        private int Count { get; set; }
        private void InitializeTimer()
        {
            Count = 0;
            timer.Start();
        }

        private void FinalizeTimer()
        {
            timer.Stop();
        }

        private void ProgressDialog_Shown(object sender, EventArgs e)
        {
            InitializeTimer();
        }

        private void timer_tick(object sender, System.EventArgs e)
        {
            //prgBar.Value = ((prgBar.Value + 1) % 101);
            Count = (Count >= 100) ? 0 : Count + 1;
            SetStatus(string.Format(StatusMessageTimer, new string('.', Count % 10)));
        }

        private void SetStatus(string value)
        {
            lblStatus.Text = value;
        }
        #endregion

        public static DialogResult Start(IWin32Window parent, Task task, bool cancellable, string sessionKey)
        {
            if (task == null || task.IsCompleted) return DialogResult.OK;

            using (var dialog = new ProgressDialog())
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
                    Debug.Fail(ex.InnerException.Message);
                    foreach (Exception inner in ex.InnerExceptions)
                    {
                        //Debug.Fail(ex.ToString());
                        NLogHandler.WriteErrorLog(new ProgressDialog(), inner, sessionKey);
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
            using (var dialog = new ProgressDialog())
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
                    NLogHandler.WriteErrorLog(new ProgressDialog(), ex, sessionKey);
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
            using (var dialog = new ProgressDialog())
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
                    NLogHandler.WriteErrorLog(new ProgressDialog(), ex, sessionKey);
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
