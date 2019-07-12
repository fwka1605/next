using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Client.Batch
{
    /// <summary>
    /// バッチ処理 抽象クラス
    /// </summary>
    public abstract class WorkerBase : IWorker
    {
        protected LogWriter logger { get; set; }
        protected ILogin Login { get; set; }
        protected TaskSchedule Setting { get; set; }
        protected ApplicationControl AppControl { get; set; }
        protected DataExpression DataExpression { get; set; }
        protected TaskScheduleHistory History { get; set; }

        /// <summary>
        ///  処理対象ファイルのフルパス
        /// </summary>
        protected string TargetFilePath { get; set; }

        public WorkerBase(LogWriter logger,
            ILogin login,
            TaskSchedule task,
            ApplicationControl appControl = null,
            DataExpression expression = null)
        {
            this.logger = logger;
            Login = login;
            Setting = task;
            if (appControl != null) AppControl = appControl;
            if (expression != null) DataExpression = expression;
            History = new TaskScheduleHistory
            {
                CompanyId = login.CompanyId,
                ImportType = task.ImportType,
                ImportSubType = task.ImportSubType,
                StartAt = DateTime.Now /* get DB Server timestamp */
            };
        }
        public abstract bool Work();

        protected string GetLastUpdateFilePath()
        {
            var dir = Setting?.ImportDirectory;
            if (!Directory.Exists(dir)) return null;
            var dirInfo = new DirectoryInfo(dir);
            return dirInfo.GetFiles()
                .OrderByDescending(x => x.LastAccessTime)
                .FirstOrDefault()?
                .FullName;
        }

        /// <summary>
        /// 移動先に同名ファイルが存在する場合は、一度削除してコピーを行う
        /// ※ 同名のファイルで一世代のみ残す場合を考慮
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public virtual bool MoveTo(bool success)
        {
            var result = false;
            try
            {
                if (string.IsNullOrEmpty(TargetFilePath)
                    || !File.Exists(TargetFilePath)) return result;

                var fileName = Path.GetFileName(TargetFilePath);
                var destinationDir = success ? Setting.SuccessDirectory : Setting.FailedDirectory;
                var destinationPath = Path.Combine(destinationDir, fileName);

                if (!Directory.Exists(destinationDir))
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationDir));
                if (File.Exists(destinationPath))
                    File.Delete(destinationPath);
                File.Move(TargetFilePath, destinationPath);
                result = true;
            }
            catch (Exception ex)
            {
                logger.Log($"ファイル移動に失敗しました。{Environment.NewLine}{ex.Message}");
            }
            return result;
        }

        public virtual bool RegisterLog(bool success, DateTime startAt, DateTime endAt, string errorLogPath)
        {
            var result = false;
            if (string.IsNullOrEmpty(errorLogPath))
                errorLogPath = string.Empty;
            else if (errorLogPath.Length > 255)
                errorLogPath = errorLogPath.Substring(0, 255);

            try
            {
                var webResult = Screen.Util.SaveTaskScheduleHistory(Login, new TaskScheduleHistory
                {
                    CompanyId = Login.CompanyId,
                    ImportType = Setting.ImportType,
                    ImportSubType = Setting.ImportSubType,
                    StartAt= startAt,
                    EndAt = endAt,
                    Result = success ? 0 : 1,
                    Errors = errorLogPath,
                });
            }
            catch (Exception ex)
            {
                logger.Log($"タイムスケジューラ実行ログの登録に失敗しました。{Environment.NewLine}{ex.Message}");
            }
            return result;
        }
    }
}
