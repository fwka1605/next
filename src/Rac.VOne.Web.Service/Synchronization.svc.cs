using Rac.VOne.Common.Extensions;
using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NLog;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "Synchronization" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで Synchronization.svc または Synchronization.svc.cs を選択し、デバッグを開始してください。
    public class Synchronization : ISynchronization
    {
        private readonly ILogger logger;
        private readonly ISynchronizationProcessor synchronizationProcessor;
        private readonly IAuthorizationProcessor authorizationProcessor;

        public Synchronization(IAuthorizationProcessor authorizationProcessor,
            ISynchronizationProcessor synchronizationProcessor,
            ILogManager logManager)
        {
            this.authorizationProcessor = authorizationProcessor;
            this.synchronizationProcessor = synchronizationProcessor;
            logger = logManager.GetLogger(typeof(Synchronization));
        }

        public async Task<DataSet> CheckCustomerAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckCustomerAsync, logger);
        }

        public async Task<DataSet> CheckCompanyAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckCompanyAsync, logger);
        }

        public async Task<DataSet> CheckDepartmentAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckDepartmentAsync, logger);
        }

        public async Task<DataSet> CheckStaffAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckStaffAsync, logger);
        }

        public async Task<DataSet> CheckLoginUserAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckLoginUserAsync, logger);
        }

        public async Task<DataSet> CheckAccountTitleAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckAccountTitleAsync, logger);
        }

        public async Task<DataSet> CheckBankAccountAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckBankAccountAsync, logger);
        }

        public async Task<DataSet> CheckBillingAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckBillingAsync, logger);
        }

        public async Task<DataSet> CheckReceiptAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckReceiptAsync, logger);
        }

        public async Task<DataSet> CheckMatchingHeaderAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckMatchingHeaderAsync, logger);
        }

        public async Task<DataSet> CheckMatchingAsync(DataSet arg)
        {
            return await CheckAsync(arg, synchronizationProcessor.CheckMatchingAsync, logger);
        }

        private async Task<DataSet> CheckAsync<TData>(DataSet arg, Func<DateTime, CancellationToken, Task<IEnumerable<TData>>> check,
                ILogger logger = null,
                [CallerMemberName]string caller = "")
        {
            if (arg == null
                || arg.Tables == null
                || arg.Tables.Count == 0
                || arg.Tables[0].Rows.Count == 0)
            {
                throw new ArgumentException("arg");
            }

            SynchronizationResult<TData> result = null;
            var ds = new DataSet();
            try
            {
                var sessionKey = string.Empty;
                DateTime updateAt;

                var row = arg
                    ?.Tables.Cast<DataTable>().FirstOrDefault()
                    ?.AsEnumerable().FirstOrDefault();
                sessionKey = row.Field<string>("SessionKey");
                updateAt = row.Field<DateTime>("UpdateAt");

                result = await authorizationProcessor.DoAuthorizeAsync(sessionKey, async token =>
                {
                    TData[] data = (await check(updateAt, token)).ToArray();
                    var list = (data == null) ? null : new List<TData>(data);
                    return new SynchronizationResult<TData>
                    {
                        Synchronizations = list,
                        ProcessResult = new ProcessResult { Result = true },
                    };
                }, logger, null, caller);
            }
            catch (Exception ex)
            {
                result = new SynchronizationResult<TData> {
                    ProcessResult = new ProcessResult {
                        ErrorCode = Rac.VOne.Common.ErrorCode.ExceptionOccured,
                        ErrorMessage = ex.Message
                    }
                };
                //ログ出力
            }

            ds.Tables.Add((new List<ProcessResult> { result.ProcessResult }).ToDataTable());
            //ds.Tables.Add(result.Synchronizations).ToDataTable());
            if(result.Synchronizations != null)
            {
             ds.Tables.Add(result.Synchronizations.ToDataTable("Synchronization"));
            }

            return ds;
        }
    }
}
