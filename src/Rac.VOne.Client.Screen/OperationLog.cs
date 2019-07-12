using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.LogDataService;
using Rac.VOne.Message;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rac.VOne.Client.Screen
{
    internal interface ILoggable
    {
        ApplicationControl ApplicationControl { get; }
        string Caption { get; }
        ILogin Login { get; }
    }

    public static class OperationLog
    {
        /// <summary>機能の実行を記録する。</summary>
        /// <param name="target">対象の機能(Screen/Dialog)</param>
        /// <param name="funcKeyNo">Fキー番号</param>
        /// <param name="action">機能の本処理</param>
        internal static void FunctionCalled(this ILoggable target, int funcKeyNo, Action action)
        {
            if (action == null) return;

            bool logging = (target.ApplicationControl?.UseOperationLogging ?? 0) == 1;
            ILogin login = target.Login;
            string viewCaption = target.Caption;

            MeasurementHelper.ProcessStart();

            if (logging)
            {
                var attribute = action.Method
                    .GetCustomAttributes(false)
                    .OfType<OperationLogAttribute>()
                    .FirstOrDefault();

                if (attribute != null)
                {
                    LogAsync(login, null, viewCaption, $"F{funcKeyNo}:{attribute.FunctionName}");
                }
            }
            action();

            if (!string.IsNullOrEmpty(viewCaption) && funcKeyNo != 10)
            {
                MeasurementHelper.ProcessEnd($"{viewCaption} F{funcKeyNo}");
            }
        }

        /// <summary>機能の実行を記録する。</summary>
        /// <param name="target">対象の機能(Screen/Dialog)</param>
        /// <param name="funcKeyNo">Fキー番号</param>
        /// <param name="button">ボタン</param>
        internal static void FunctionCalled(this ILoggable target, int funcKeyNo, Button button)
        {
            bool logging = (target.ApplicationControl?.UseOperationLogging ?? 0) == 1;
            ILogin login = target.Login;
            string viewCaption = target.Caption;

            MeasurementHelper.ProcessStart();

            if (logging && button != null)
            {
                LogAsync(login, null, viewCaption, button.Text);
            }

            if (!string.IsNullOrEmpty(viewCaption) && funcKeyNo != 10)
            {
                MeasurementHelper.ProcessEnd($"{viewCaption} F{funcKeyNo}");
            }
        }

        /// <summary>画面表示を記録する。</summary>
        /// <param name="target">画面(Screen/Dialog)</param>
        internal static void ViewOpened(this ILoggable target)
        {
            if (!(target is Control)) return;
            bool logging = (target.ApplicationControl?.UseOperationLogging ?? 0) == 1;

            if (logging)
            {
                LogAsync(target.Login, null, target.Caption, "開始");
            }
        }

        /// <summary>ボタンのクリック操作を記録する。</summary>
        /// <param name="target">対象の機能(Screen/Dialog)</param>
        /// <param name="button">ボタン</param>
        /// <param name="alternativeText">代替テキスト</param>
        internal static void ButtonClicked(this ILoggable target, Button button, string alternativeText = null)
        {
            bool logging = (target.ApplicationControl?.UseOperationLogging ?? 0) == 1;

            if (logging)
            {
                LogAsync(target.Login, null, target.Caption, alternativeText ?? button.Text);
            }
        }

        /// <summary>確認メッセージの選択を記録する。</summary>
        /// <param name="target">対象の機能(Screen/Dialog)</param>
        /// <param name="selected">ユーザーの選択</param>
        /// <returns>ユーザーの選択</returns>
        internal static DialogResult Confirmed(this ILoggable target,
                DialogResult selected)
        {
            bool logging = (target.ApplicationControl?.UseOperationLogging ?? 0) == 1;

            if (logging)
            {
                LogAsync(target.Login, null, target.Caption,
                        GetDialogResultCaption(selected));
            }
            return selected;
        }

        internal static DialogResult FileSelected(this ILoggable target,
                CommonDialog dialog, DialogResult selected)
        {
            bool logging = (target.ApplicationControl?.UseOperationLogging ?? 0) == 1;

            if (logging)
            {
                string okCaption = null;
                if (dialog is SaveFileDialog) okCaption = "保存";
                else if (dialog is OpenFileDialog) okCaption = "開く";

                LogAsync(target.Login, null, target.Caption,
                        GetDialogResultCaption(selected, okCaption));
            }
            return selected;
        }

        internal static void LogOperated(this ILoggable target, string operationName)
        {
            bool logging = (target.ApplicationControl?.UseOperationLogging ?? 0) == 1;

            if (logging)
            {
                LogAsync(target.Login, null, target.Caption, operationName);
            }
        }

        private static string GetDialogResultCaption(DialogResult selected, string okCaption = null)
        {
            if (okCaption == null) okCaption = "OK";

            switch (selected)
            {
                case DialogResult.Abort: return "中止";
                case DialogResult.Cancel: return "キャンセル";
                case DialogResult.Ignore: return"無視";
                case DialogResult.No: return "いいえ";
                case DialogResult.OK: return okCaption;
                case DialogResult.Retry: return "再試行";
                case DialogResult.Yes: return "はい";
                default: return string.Empty;
            }
        }

        private static void LogAsync(ILogin login, int? menuId, string menuName, string operationName)
        {
            var logData = new LogData
            {
                CompanyId = login.CompanyId,
                ClientName = Util.GetRemoteHostName(),
                LoggedAt = DateTime.Now,
                LoginUserCode = login.UserCode ?? "",
                LoginUserName = login.UserName ?? "",
                MenuId = menuId,
                MenuName = menuName,
                OperationName = operationName,
            };

            Task.Run(async () =>
            {
                await ServiceProxyFactory.DoAsync(async (LogDataServiceClient client)
                    => await client.LogAsync(login.SessionKey, logData));
            });
        }
    }
}
