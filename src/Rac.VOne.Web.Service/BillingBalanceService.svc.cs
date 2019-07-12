using Rac.VOne.Common.Logging;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Models;
using Rac.VOne.Web.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NLog;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "BillingBalanceService" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで BillingBalanceService.svc または BillingBalanceService.svc.cs を選択し、デバッグを開始してください。
    public class BillingBalanceService : IBillingBalanceService
    {
        private readonly IAuthorizationProcessor authorizationProcess;
        private readonly IBillingBalanceProcessor billingBalanceProcessor;
        private readonly IBillingBalanceBackProcessor billingBalanceBackProcessor;
        private readonly IDbFunctionProcessor dbFunctionProcessor;
        private readonly ILogger logger;

        public BillingBalanceService(IAuthorizationProcessor authorization,
            IBillingBalanceProcessor billingBalanceProcessor,
            IBillingBalanceBackProcessor billingBalanceBackProcessor,
            IDbFunctionProcessor dbFunctionProcessor,
            ILogManager logManager)
        {
            authorizationProcess = authorization;
            this.billingBalanceProcessor = billingBalanceProcessor;
            this.billingBalanceBackProcessor = billingBalanceBackProcessor;
            this.dbFunctionProcessor = dbFunctionProcessor;
            logger = logManager.GetLogger(typeof(BillingBalanceService));
        }

        public async Task<BillingBalanceResult> GetLastCarryOverAtAsync(string SessionKey, int CompanyId)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var result = await billingBalanceProcessor.GetLastCarryOverAtAsync(CompanyId, token);
                return new BillingBalanceResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    LastCarryOverAt = result,
                };
            }, logger);
        }

        public async Task<BillingBalancesResult> SaveAsync(string SessionKey, int CompanyId, DateTime CarryOverAt, int LoginUserId)
        {
            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                var billingBalanceList = new List<BillingBalance>();

                var lastCarryOverAt = await billingBalanceProcessor.GetLastCarryOverAtAsync(CompanyId, token);

                //保存中の繰越データを削除
                await billingBalanceBackProcessor.DeleteAsync(CompanyId, token);

                //前回繰越データの待避
                await billingBalanceBackProcessor.SaveAsync(CompanyId, token);

                //繰越対象得意先を取得
                var billingBalances = (await billingBalanceProcessor.GetBillingBalancesAsync(CompanyId,
                    lastCarryOverAt, CarryOverAt, token)).ToList();

                var CreateAt = await dbFunctionProcessor.GetDbDateTimeAsync(token);

                //繰越残高を取得
                foreach (BillingBalance billingBalance in billingBalances)
                {
                    //前回繰越残高を取得
                    var lastBillingBalanceResult = (await billingBalanceProcessor.GetLastBillingBalanceAsync(CompanyId,
                        billingBalance.CurrencyId, billingBalance.CustomerId, billingBalance.StaffId,
                        billingBalance.DepartmentId, token)).ToList();
                    var lastBillingBalance = (lastBillingBalanceResult.Count > 0) ? lastBillingBalanceResult.FirstOrDefault()
                        .BalanceCarriedOver : 0M;

                    //対象期間内の請求金額取得
                    var billingAmount = await billingBalanceProcessor.GetBillingAmountAsync(CompanyId,
                        billingBalance.CurrencyId, billingBalance.CustomerId, billingBalance.StaffId,
                        billingBalance.DepartmentId, CarryOverAt, lastCarryOverAt, token);

                    //対象期間内の消込額取得
                    var receiptAmount = await billingBalanceProcessor.GetReceiptAmountAsync(CompanyId,
                        billingBalance.CurrencyId, billingBalance.CustomerId, billingBalance.StaffId,
                        billingBalance.DepartmentId, CarryOverAt, lastCarryOverAt, token);

                    billingBalance.BalanceCarriedOver = lastBillingBalance + billingAmount - receiptAmount;
                    billingBalance.CarryOverAt = CarryOverAt;
                    billingBalance.CreateAt = CreateAt;
                    billingBalance.CreateBy = LoginUserId;
                }

                //前回繰越残高を削除
                await billingBalanceProcessor.DeleteAsync(CompanyId, token);

                //今回繰越残高登録
                foreach (BillingBalance billingBalance in billingBalances)
                {
                    if (billingBalance.BalanceCarriedOver != 0)
                    {
                        var billingBalanceResult = await billingBalanceProcessor.SaveAsync(billingBalance, token);
                        billingBalanceList.Add(billingBalanceResult);
                    }
                }
                return new BillingBalancesResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    BillingBalances = billingBalanceList
                };
            }, logger);
        }

        public async Task<CountResult> CancelAsync(string SessionKey, int CompanyId)
        {
            var deleteResult = 0;

            return await authorizationProcess.DoAuthorizeAsync(SessionKey, async token =>
            {
                await billingBalanceProcessor.DeleteAsync(CompanyId, token);

                await billingBalanceProcessor.RestoreBillingBalanceAsync(CompanyId, token);

                deleteResult = await billingBalanceBackProcessor.DeleteAsync(CompanyId, token);

                return new CountResult
                {
                    ProcessResult = new ProcessResult { Result = true },
                    Count = deleteResult,
                };
            }, logger);
        }
    }
}
