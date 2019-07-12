using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;
using Rac.VOne.EbData;
using Rac.VOne.Client.Common;
using Rac.VOne.Client.Screen.EBFileSettingMasterService;

namespace Rac.VOne.Client.Batch.ConcreteWorker
{
    public class EBDataImporterWorker : WorkerBase
    {
        private EbDataImporter Importer { get; }
        public EBDataImporterWorker(
            LogWriter logger,
            ILogin login,
            TaskSchedule task)
            : base(logger, login, task)
        {
            Importer = new EbDataImporter();
            Importer.Login = Login;
            Importer.Year = DateTime.Today.Year; /* get database timestamp */
        }

        public override bool Work()
        {
            var result = false;
            TargetFilePath = GetLastUpdateFilePath();
            if (string.IsNullOrEmpty(TargetFilePath)
                || !System.IO.File.Exists(TargetFilePath))
            {
                logger.Log("EBデータ取込：取込対象となるファイルがありません。");
                return result;
            }

            List<FileInformation> importResult = null;
            try
            {
                var setting = Screen.Util.GetEBFileSetting(Login, Setting.ImportSubType);
                if (setting == null)
                {
                    logger.Log("EBファイル設定 の取得に失敗しました。");
                    return result;
                }

                importResult = Importer.ReadAndSaveFiles(new List<FileInformation> {
                    new FileInformation (0, TargetFilePath, setting)
                });
            }
            catch (Exception ex)
            {
                logger.Log($"取込処理に失敗しました。{Environment.NewLine}{ex.Message}");
            }
            // eb data 取込 の メッセージング
            // フォーマットエラーなど
            result = importResult?.All(x => x.Result == EbData.ImportResult.Success) ?? false;
            return result;
        }

    }
}
