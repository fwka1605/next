using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.Client.Screen.Importer;

namespace Rac.VOne.Client.Batch.ConcreteWorker
{
    public class PaymentScheduleImporterWorker : WorkerBase
    {
        private PaymentScheduleImporter Importer { get; }
        public PaymentScheduleImporterWorker(LogWriter logger,
            ILogin login,
            TaskSchedule task,
            ApplicationControl applicationControl)
            : base(logger, login, task)
        {
            Importer = new PaymentScheduleImporter(login, applicationControl);
            Importer.ImporterSettingId = task.ImportSubType;
            Importer.DoReplaceAmount = task.BillingAmount == 0;
            Importer.DoTargetNotMatchedData = task.TargetBillingAssignment == 0;
            Importer.DoIgnoreSameCustomerGroup = task.UpdateSameCustomer == 0;
        }
        public override bool Work()
        {
            var result = false;
            TargetFilePath = GetLastUpdateFilePath();
            if (string.IsNullOrEmpty(TargetFilePath))
            {
                logger.Log("入金予定フリーインポーター：取込データが存在しません。");
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
