using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.Importer;

namespace Rac.VOne.Client.Batch.ConcreteWorker
{
    public class ReceiptImporterWorker : WorkerBase
    {
        private ReceiptImporter Importer { get; }
        public ReceiptImporterWorker(LogWriter logger,
            ILogin login,
            TaskSchedule task,
            ApplicationControl applicationControl)
            : base(logger, login, task)
        {
            Importer = new ReceiptImporter(login, applicationControl);
            Importer.ImporterSettingId = task.ImportSubType;
        }

        public override bool Work()
        {
            var result = false;
            TargetFilePath = GetLastUpdateFilePath();
            if (string.IsNullOrEmpty(TargetFilePath))
            {
                logger.Log("入金フリーインポーター：取込データが存在しません。");
                return result;
            }
            Importer.FilePath = TargetFilePath;
            Importer.ReadCsvAsync().Wait();
            var importable = Importer.ReadCount > 0
                && Importer.ValidCount > 0
                && Importer.InvalidCount == 0;
            if (!importable)
            {
                Importer.WriteErrorLog(logger.GetOutputPath());
                return result;
            }

            var saveResult = Importer.ImportAsync().Result;
            var moveResult = MoveTo(saveResult);
            result = saveResult & moveResult;
            return result;
        }
    }
}
