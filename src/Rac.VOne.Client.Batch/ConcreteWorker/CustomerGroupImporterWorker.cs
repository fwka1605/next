using Rac.VOne.Common.DataHandling;
using Rac.VOne.Import;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.Client.Batch.ConcreteWorker
{
    public class CustomerGroupImporterWorker : WorkerBase
    {
        private IImporter Importer { get; set; }
        public CustomerGroupImporterWorker(
            LogWriter logger,
            ILogin login,
            TaskSchedule task,
            ApplicationControl appControl,
            DataExpression expression)
            : base(logger, login, task, appControl, expression)
        {
            var difinition = new CustomerGroupFileDefinition(expression);
            difinition.GetCustomerDictionary = val
                => Screen.Util.ConvertToDictionary(Screen.Util.GetCustomerList(Login, val), x => x.Code);
            difinition.GetDbCsutomerGroups = ()
                => Task.Run(async () => await Screen.Util.GetCustomerGroupListAsync(Login)).Result;
            var importer = difinition.CreateImporter(x => new { x.ParentCustomerId, x.ChildCustomerId });
            importer.UserId = Login.UserId;
            importer.UserCode = Login.UserCode;
            importer.CompanyId = Login.CompanyId;
            importer.CompanyCode = Login.CompanyCode;
            importer.LoadAsync = async () => await Screen.Util.GetCustomerGroupListAsync(Login);
            importer.RegisterAsync = async unitOfWork => await Screen.Util.ImportCustomerGroupAsync(Login, unitOfWork);
            importer.ErrorLogPath = logger.GetOutputPath();
            Importer = importer;
        }

        public override bool Work()
        {
            var result = false;
            TargetFilePath = GetLastUpdateFilePath();
            if (string.IsNullOrEmpty(TargetFilePath)
                || !System.IO.File.Exists(TargetFilePath))
            {
                logger.Log("債権代表者マスター：取込データが存在しません。");
                return result;
            }

            ImportResult res = null;
            try
            {
                res = Importer.ImportAsync(TargetFilePath,
                    (ImportMethod)Setting.ImportSubType,
                    cancel: null, progress: null).Result;
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
