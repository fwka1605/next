using Rac.VOne.Client.Common;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using BatchType = Rac.VOne.Common.Constants.TaskScheduleImportType;

namespace Rac.VOne.Client.Batch
{
    public class Worker : IDisposable
    {
        private readonly LogWriter logger;
        private readonly ILogin Login;
        private readonly IWorker InnerWorker;

        private DateTime StartAt { get; set; }
        private DateTime EndAt { get; set; }

        private string AuthenticationKey { get; set; }
        private string AuthenticationCompanyCode { get; set; }

        private string CompanyCode { get; set; }
        private int? ImportType { get; set; }
        private int? ImportSubType { get; set; }

        internal bool InitializeResult { get; private set; }
        private List<string> LogMessages { get; set; }

        public Worker(string[] args)
        {
            StartAt = DateTime.Now; // UtcNow;

            logger = new LogWriter();

            try
            {
                if (!ValidateArguments(args))
                {
                    logger.Log("引数指定が正しくありません。");
                    return;
                }
                if (!InitializeApplicationSettings())
                {
                    logger.Log("設定が正しくありません。");
                    return;
                }

                Login = new Login();
                Login.SessionKey = Authenticate(AuthenticationKey, AuthenticationCompanyCode);
                if (string.IsNullOrEmpty(Login.SessionKey))
                {
                    logger.Log("認証処理に失敗しました。");
                    return;
                }
                // initialize login
                if (!InitializeCompany())
                {
                    logger.Log("会社情報初期化に失敗しました。");
                    return;
                }
                var task = InitializeTask();
                if (task == null)
                {
                    logger.Log("設定の取得に失敗しました。");
                    return;
                }
                Login.UserId = task.CreateBy;
                Login.UserName = task.CreateUserName;

                if (task.LogDestination == 0)
                    logger.FailureDirectory = task.FailedDirectory;

                InnerWorker = CreateConcreteWorker(logger, task);
                if (InnerWorker == null)
                {
                    logger.Log("初期化処理に失敗しました。");
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }
            InitializeResult = true;
        }

        public bool Work()
        {
            var batchResult = InnerWorker.Work();
            var fileMoveResult = InnerWorker.MoveTo(batchResult);
            EndAt = DateTime.Now;
            InnerWorker.RegisterLog(batchResult, StartAt, EndAt,
                batchResult ? string.Empty : logger.GetOutputPath());
            return batchResult & fileMoveResult;
        }

        #region validate arguments

        private bool ValidateArguments(string[] args)
        {
            if (args == null || args.Length != 3) return false;
            CompanyCode = args[0];
            if (!ParseIntValue(args[1], x => ImportType = x)) return false;
            if (!ParseIntValue(args[2], x => ImportSubType = x)) return false;
            return true;
        }
        private bool ParseIntValue(string value, Action<int> setter)
        {
            var result = 0;
            if (!int.TryParse(value, out result)) return false;
            setter(result);
            return true;
        }

        #endregion

        #region authentication

        private string Authenticate(string key, string code)
            => Screen.Util.Authenticate(key, code);

        #endregion

        #region initialize

        private bool InitializeApplicationSettings()
        {
            var result = false;
            try
            {
                AuthenticationKey = System.Configuration.ConfigurationManager.AppSettings["AuthenticationKey"];
                AuthenticationCompanyCode = System.Configuration.ConfigurationManager.AppSettings["CompanyCode"];
                var codeType = System.Configuration.ConfigurationManager.AppSettings["CompanyCodeType"];
                var value = 0;
                if (int.TryParse(codeType, out value))
                    DataExpression.CompanyCodeTypeGlobal = value;
                result = true;
            }
            catch { }
            return result;
        }

        private bool InitializeCompany()
        {
            var code = CompanyCode;
            try
            {
                var company = Screen.Util.GetCompany(Login, code);
                if (company == null) return false;
                Login.CompanyId = company.Id;
                Login.CompanyCode = company.Code;
                Login.CompanyName = company.Name;
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }
            return true;
        }

        private TaskSchedule InitializeTask()
        {
            try
            {
                var items = Screen.Util.GetTaskSchedule(Login);
                return items.FirstOrDefault(x => x.ImportType == ImportType && x.ImportSubType == ImportSubType);
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }
            return null;
        }

        #endregion

        #region create concrete worker

        private IWorker CreateConcreteWorker(LogWriter logger, TaskSchedule task)
        {

            ApplicationControl appControl = null;
            DataExpression expression = null;
            try
            {
                appControl = Screen.Util.GetApplicationControl(Login);
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }

            if (appControl == null)
                return null;
            else
                expression = new DataExpression(appControl);

            switch (task.ImportType)
            {
                case (int)BatchType.Customer:           return new ConcreteWorker.CustomerImporterWorker(logger, Login, task, appControl);
                case (int)BatchType.CustomerGroup:      return new ConcreteWorker.CustomerGroupImporterWorker(logger, Login, task, appControl, expression);
                case (int)BatchType.Billing:            return new ConcreteWorker.BillingImporterWorker(logger, Login, task, appControl);
                case (int)BatchType.EbData:             return new ConcreteWorker.EBDataImporterWorker(logger, Login, task);
                case (int)BatchType.Receipt:            return new ConcreteWorker.ReceiptImporterWorker(logger, Login, task, appControl);
                case (int)BatchType.PaymentSchedule:    return new ConcreteWorker.PaymentScheduleImporterWorker(logger, Login, task, appControl);
            }
            return null;
        }

        public void Dispose()
        {
            logger?.WriteLog();
        }

        #endregion

    }
}
