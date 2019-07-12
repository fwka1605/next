using Rac.VOne.Web.Api.Legacy.Extensions;
using Rac.VOne.Web.Common;
using Rac.VOne.Web.Common.Importers;
using Rac.VOne.Web.Common.Reports;
using Rac.VOne.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rac.VOne.Web.Api.Legacy.Controllers
{
    /// <summary>
    ///  
    /// </summary>
    public class BillingController : ApiControllerAuthorized
    {
        private readonly IBillingProcessor billingProcessor;
        private readonly IBillingSaveProcessor billingSaveProcessor;
        private readonly IBillingSaveForInputProcessor billingSaveForInputProcessor;
        private readonly IBillingJournalizingProcessor billingJournalizingProcessor;
        private readonly IBillingSearchProcessor billingSearchProcessor;
        private readonly IBillingMemoProcessor billingMemoProcessor;
        private readonly IBillingDueAtModifyProcessor billingDueAtModifyProcessor;
        private readonly IBillingDivisionContractProcessor billingDivisionContractProcessor;
        private readonly IBillingDiscountProcessor billingDiscountProcessor;
        private readonly IBillingScheduledPaymentProcessor billingScheduledPaymentProcessor;
        private readonly IBillingAccountTransferProcessor billingAccountTransferProcessor;
        private readonly IImporterSettingDetailProcessor importerSettingDetailProcessor;
        private readonly ICustomerProcessor customerProcessor;


        private readonly IBillingFileImportProcessor billingFileImportProcessor;
        private readonly IPaymentScheduleFileImportProcessor paymentScheduleFileImportProcessor;

        private readonly IBillingAccountTransferFileImportProcessor billingAccountTransferFileImportProcessor;

        /// <summary>constructor</summary>
        public BillingController(
            IBillingProcessor billingProcessor,
            IBillingSaveProcessor billingSaveProcessor,
            IBillingSaveForInputProcessor billingSaveForInputProcessor,
            IBillingJournalizingProcessor billingJournalizingProcessor,
            IBillingSearchProcessor billingSearchProcessor,
            IBillingMemoProcessor billingMemoProcessor,
            IBillingDueAtModifyProcessor billingDueAtModifyProcessor,
            IBillingDivisionContractProcessor billingDivisionContractProcessor,
            IBillingDiscountProcessor billingDiscountProcessor,
            IBillingScheduledPaymentProcessor billingScheduledPaymentProcessor,
            IBillingAccountTransferProcessor billingAccountTransferProcessor,
            IImporterSettingDetailProcessor importerSettingDetailProcessor,
            ICustomerProcessor customerProcessor,

            IBillingFileImportProcessor billingFileImportProcessor,
            IPaymentScheduleFileImportProcessor paymentScheduleFileImportProcessor,

            IBillingAccountTransferFileImportProcessor billingAccountTransferFileImportProcessor
            )
        {
            this.billingProcessor = billingProcessor;
            this.billingSaveProcessor = billingSaveProcessor;
            this.billingSaveForInputProcessor = billingSaveForInputProcessor;
            this.billingJournalizingProcessor = billingJournalizingProcessor;
            this.billingSearchProcessor = billingSearchProcessor;
            this.billingMemoProcessor = billingMemoProcessor;
            this.billingDueAtModifyProcessor = billingDueAtModifyProcessor;
            this.billingDivisionContractProcessor = billingDivisionContractProcessor;
            this.billingDiscountProcessor = billingDiscountProcessor;
            this.billingScheduledPaymentProcessor = billingScheduledPaymentProcessor;
            this.billingAccountTransferProcessor = billingAccountTransferProcessor;
            this.importerSettingDetailProcessor = importerSettingDetailProcessor;
            this.customerProcessor = customerProcessor;


            this.billingFileImportProcessor = billingFileImportProcessor;
            this.paymentScheduleFileImportProcessor = paymentScheduleFileImportProcessor;

            this.billingAccountTransferFileImportProcessor = billingAccountTransferFileImportProcessor;
        }

        #region create (crud の順)

        /// <summary>
        /// 登録 配列
        /// </summary>
        /// <param name="billings"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Billing>> Save(IEnumerable<Billing> billings, CancellationToken token)
            => (await billingSaveForInputProcessor.SaveAsync(billings, token)).ToArray();



        /// <summary>
        /// 歩引き登録
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> SaveDiscount(BillingDiscount discount, CancellationToken token)
            => await billingDiscountProcessor.SaveAsync(discount, token);

        /// <summary>
        /// メモ登録
        /// </summary>
        /// <param name="memo"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> SaveMemo(BillingMemo memo, CancellationToken token)
            => await billingMemoProcessor.SaveMemoAsync(memo.BillingId, memo.Memo, token);

        #endregion

        #region read

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="ids">請求ID 配列</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Billing>> Get([FromBody] long[] ids, CancellationToken token)
            => (await billingProcessor.GetByIdsAsync(ids, token)).ToArray();


        /// <summary>
        /// 取得 歩引き配列
        /// </summary>
        /// <param name="billingId">請求ID</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<BillingDiscount>> GetDiscount([FromBody] long billingId, CancellationToken token)
            => (await billingDiscountProcessor.GetAsync(billingId, token)).ToArray();

        /// <summary>
        /// 取得 配列
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Billing>> GetItems(BillingSearch option, CancellationToken token)
            => (await billingSearchProcessor.GetAsync(option, token)).ToArray();


        /// <summary>
        /// メモ取得
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetMemo([FromBody] long id, CancellationToken token)
            => await billingMemoProcessor.GetMemoAsync(id, token);

        #region exist check

        /// <summary>
        /// 科目ID が利用されているか確認
        /// </summary>
        /// <param name="accountTitleId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistAccountTitle([FromBody] int accountTitleId, CancellationToken token)
            => await billingProcessor.ExistDebitAccountTitleAsync(accountTitleId, token) ||
               await billingProcessor.ExistCreditAccountTitleAsync(accountTitleId, token);

        /// <summary>
        /// 区分ID が登録されているか確認
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistCollectCategory([FromBody] int categoryId, CancellationToken token)
            => await billingProcessor.ExistCategoryAsync(categoryId, token);

        /// <summary>
        /// 得意先ID が登録されているか確認
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistCustomer([FromBody] int customerId, CancellationToken token)
            => await billingProcessor.ExistCustomerAsync(customerId, token);

        /// <summary>
        /// 請求区分IDが登録されているか確認
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistBillingCategory([FromBody] int categoryId, CancellationToken token)
            => await billingProcessor.ExistBillingCategoryAsync(categoryId, token);

        /// <summary>
        /// 担当者ID が登録されているか確認
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistStaff([FromBody] int staffId, CancellationToken token)
            => await billingProcessor.ExistStaffAsync(staffId, token);

        /// <summary>
        /// 通貨ID が登録されているか確認
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistCurrency([FromBody] int currencyId, CancellationToken token)
            => await billingProcessor.ExistCurrencyAsync(currencyId, token);

        /// <summary>
        /// 請求部門ID が登録されているか確認
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistDepartment([FromBody] int departmentId, CancellationToken token)
            => await billingProcessor.ExistDepartmentAsync(departmentId, token);

        /// <summary>
        /// 仕向先ID が登録されているか確認
        /// </summary>
        /// <param name="destinationId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistDestination([FromBody] int destinationId, CancellationToken token)
            => await billingProcessor.ExistDestinationAsync(destinationId, token);

        #endregion

        #endregion

        #region update

        /// <summary>
        /// 論理削除/戻し処理
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CountResult> Omit(OmitSource source, CancellationToken token)
            => await billingProcessor.OmitAsync(source, token);

        /// <summary>
        /// 入金予定入力実施
        /// </summary>
        /// <param name="billings"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Billing>> InputScheduledPayment(IEnumerable<Billing> billings, CancellationToken token)
            => (await billingScheduledPaymentProcessor.SaveAsync(billings, token)).ToArray();

        #endregion

        #region delete

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="source">請求ID 配列, 会社ID, 更新者ID LoginUserId を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(BillingDeleteSource source, CancellationToken token)
            => await billingProcessor.DeleteAsync(source, token);


        /// <summary>
        /// 歩引き削除
        /// </summary>
        /// <param name="billingId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> DeleteDiscount([FromBody] long billingId, CancellationToken token)
            => await billingDiscountProcessor.DeleteAsync(billingId, token);

        /// <summary>
        /// メモ削除
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> DeleteMemo([FromBody] long Id, CancellationToken token)
            => await billingMemoProcessor.DeleteAsync(Id);

        #endregion

        #region dueat modify

        /// <summary>
        /// 入金予定日変更用 オブジェクト取得 配列
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<BillingDueAtModify>> GetDueAtModifyItems(BillingSearch option, CancellationToken token)
            => (await billingDueAtModifyProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 更新日時 更新
        /// </summary>
        /// <param name="billings"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Billing>> UpdateDueAt(IEnumerable<BillingDueAtModify> billings, CancellationToken token)
            => (await billingDueAtModifyProcessor.UpdateAsync(billings, token)).ToArray();

        #endregion

        #region import

        /// <summary>請求フリーインポーター 読込・検証処理</summary>
        /// <param name="source">CompanyId, LoginUserId, ImporterSettingId, EncodingCodePage, Data を指定</param>
        /// <param name="token"></param>
        /// <returns>ImportData.Id が登録時に必要なので、記録するように</returns>
        [HttpPost]
        public async Task<ImportDataResult> Read(TransactionImportSource source, CancellationToken token)
            => await billingFileImportProcessor.ReadAsync(source, token);

        /// <summary>請求フリーインポーター 新規得意先取得</summary>
        /// <param name="source">
        /// <see cref="TransactionImportSource.CompanyId"/>,
        /// <see cref="TransactionImportSource.ImporterSettingId"/>,
        /// <see cref="TransactionImportSource.ImportDataId"/>,
        /// <see cref="TransactionImportSource.LoginUserId"/> を指定
        /// </param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Customer>> GetImportNewCustomer(TransactionImportSource source, CancellationToken token)
            => (await billingFileImportProcessor.GetCustomers(source, token)).ToArray();

        /// <summary>請求フリーインポーター 登録処理</summary>
        /// <param name="source">CompanyId, LoginUserId, ImporterSettingId, ImportDataId を指定</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportDataResult> Import(TransactionImportSource source, CancellationToken token)
            => await billingFileImportProcessor.ImportAsync(source, token);

        /// <summary>入金予定フリーインポーター 読込・検証処理</summary>
        /// <param name="source">CompanyId, LoginUserId, ImporterSettingId, EncodingCodePage, Data を指定</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportDataResult> ReadScheduledPayment(TransactionImportSource source, CancellationToken token)
            => await paymentScheduleFileImportProcessor.ReadAsync(source, token);

        /// <summary>入金予定フリーインポーター 登録処理</summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportDataResult> ImportPaymentSchedule(TransactionImportSource source, CancellationToken token)
            => await paymentScheduleFileImportProcessor.ImportAsync(source, token);

        #endregion


        #region journalizing

        /// <summary>
        /// 仕訳履歴（サマリー）取得 オプションで、未取得分のサマリーも取得可
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<JournalizingSummary>> GetBillingJournalizingSummary(BillingJournalizingOption option, CancellationToken token)
            => (await billingJournalizingProcessor.GetSummaryAsync(option, token)).ToArray();

        /// <summary>
        /// 仕訳抽出
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<BillingJournalizing>> ExtractBillingJournalizing(BillingJournalizingOption option, CancellationToken token)
            => (await billingJournalizingProcessor.ExtractAsync(option, token)).ToArray();

        /// <summary>
        /// 仕訳出力更新
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Billing>> UpdateOutputAt(BillingJournalizingOption option, CancellationToken token)
            => (await billingJournalizingProcessor.UpdateAsync(option, token)).ToArray();

        /// <summary>
        /// 仕訳出力取消
        /// </summary>
        /// <param name="option">検索条件</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Billing>> CancelBillingJournalizing(BillingJournalizingOption option, CancellationToken token)
            => (await billingJournalizingProcessor.CancelAsync(option, token)).ToArray();

        #endregion


    }
}
