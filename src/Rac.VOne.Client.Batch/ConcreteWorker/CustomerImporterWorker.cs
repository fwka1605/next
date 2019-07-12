using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen;
using Rac.VOne.Client.Screen.Importer;

namespace Rac.VOne.Client.Batch.ConcreteWorker
{
    public class CustomerImporterWorker : WorkerBase
    {
        private readonly CustomerImporter Importer;
        public CustomerImporterWorker(
            LogWriter logger,
            ILogin login,
            TaskSchedule task,
            ApplicationControl appControl)
            : base(logger, login, task, appControl)
        {
            Importer = new CustomerImporter(Login, appControl);
        }
        public override bool Work()
        {
            var result = false;
            Importer.PatternNo = Setting.ImportSubType.ToString().PadLeft(2, '0');
            Importer.InitializeAsync().Wait();
            if (!Importer.IsImporterSettingRegistered)
            {
                logger.Log("取込設定が行われていません。");
                return result;
            }
            TargetFilePath = GetLastUpdateFilePath();
            if (string.IsNullOrEmpty(TargetFilePath))
            {
                logger.Log("得意先マスター：取込対象となるファイルがありません。");
                return result;
            }
            ImportResult res = null;
            try
            {
                res = Importer.ImportAsync(TargetFilePath, Setting.ImportMode, logger.GetOutputPath()).Result;
                result = true;
            }
            catch (Exception ex)
            {
                logger.Log($"取込処理に失敗しました。{Environment.NewLine}{ex.Message}");
            }
            if (!result) return result;

            result = res.ValidItemCount > 0 && res.InvalidItemCount == 0;

            return result;
        }
    }
}
