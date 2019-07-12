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
    /// 入金
    /// </summary>
    public class ReceiptController : ApiControllerAuthorized
    {
        private readonly IReceiptHeaderProcessor receiptHeaderProcessor;
        private readonly IReceiptProcessor receiptProcessor;
        private readonly IReceiptApportionProcessor receiptApportionProcessor;
        private readonly IReceiptSearchProcessor receiptSearchProcessor;
        private readonly IReceiptExcludeProcessor receiptExcludeProcessor;
        private readonly IReceiptMemoProcessor receiptMemoProcessor;
        private readonly IReceiptJournalizingProcessor receiptJournalizingProcessor;
        private readonly IReceiptSectionTransferProcessor receiptSectionTransferProcessor;
        private readonly IAdvanceReceivedProcessor advanceReceivedProcessor;
        private readonly IAdvanceReceivedSplitProcessor advanceReceivedSplitProcessor;

        private readonly IReceiptFileImportProcessor receiptFileImportProcessor;

        /// <summary>constructor</summary>
        public ReceiptController(
            IReceiptProcessor receiptProcessor,
            IReceiptApportionProcessor receiptApportionProcessor,
            IReceiptSearchProcessor receiptSearchProcessor,
            IReceiptHeaderProcessor receiptHeaderProcessor,
            IReceiptExcludeProcessor receiptExcludeProcessor,
            IReceiptMemoProcessor receiptMemoProcessor,
            IReceiptJournalizingProcessor receiptJournalizingProcessor,
            IReceiptSectionTransferProcessor receiptSectionTransferProcessor,
            IAdvanceReceivedProcessor advanceReceivedProcessor,
            IAdvanceReceivedSplitProcessor advanceReceivedSplitProcessor,
            IReceiptFileImportProcessor receiptFileImportProcessor
            )
        {
            this.receiptHeaderProcessor = receiptHeaderProcessor;
            this.receiptProcessor = receiptProcessor;
            this.receiptApportionProcessor = receiptApportionProcessor;
            this.receiptSearchProcessor = receiptSearchProcessor;
            this.receiptExcludeProcessor = receiptExcludeProcessor;
            this.receiptMemoProcessor = receiptMemoProcessor;
            this.receiptJournalizingProcessor = receiptJournalizingProcessor;
            this.receiptSectionTransferProcessor = receiptSectionTransferProcessor;
            this.advanceReceivedProcessor = advanceReceivedProcessor;
            this.advanceReceivedSplitProcessor = advanceReceivedSplitProcessor;
            this.receiptFileImportProcessor = receiptFileImportProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items">入金データ 登録用オブジェクト</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReceiptInputsResult> Save(ReceiptSaveItem items, CancellationToken token)
            => await receiptProcessor.SaveAsync(items, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete([FromBody] long id, CancellationToken token)
            => await receiptProcessor.DeleteAsync(id, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Receipt>> Get([FromBody] long[] ids, CancellationToken token)
            => (await receiptProcessor.GetByIdsAsync(ids, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<Receipt>> GetItems(ReceiptSearch option, CancellationToken token)
            => (await receiptSearchProcessor.GetAsync(option, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalReceiptId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdvanceReceiptsResult> GetAdvanceReceipts([FromBody] long originalReceiptId, CancellationToken token)
        {
            var originalReceipt = await receiptProcessor.GetReceiptAsync(originalReceiptId);
            var advanceReceipts = (await advanceReceivedProcessor.GetAdvanceReceiptsAsync(originalReceiptId)).ToList();

            return new AdvanceReceiptsResult
            {
                ProcessResult = new ProcessResult { Result = true },
                OriginalReceipt = originalReceipt,
                AdvanceReceipts = advanceReceipts,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excludes"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task< ReceiptExcludeResult > SaveExcludeAmount(IEnumerable<ReceiptExclude> excludes, CancellationToken token)
            => await receiptExcludeProcessor.SaveAsync(excludes, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReceiptMemo> SaveMemo(ReceiptMemo memo)
        {
            return await receiptMemoProcessor.SaveAsync(memo.ReceiptId, memo.Memo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> DeleteMemo(long receiptId)
        {
            await receiptMemoProcessor.DeleteAsync(receiptId);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiptId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetMemo([FromBody] long receiptId, CancellationToken token)
            => (await receiptMemoProcessor.GetAsync(receiptId, token))?.Memo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveds"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdvanceReceivedResult> SaveAdvanceReceived(IEnumerable<AdvanceReceived> receiveds, CancellationToken token)
            => await advanceReceivedProcessor.SaveAsync(receiveds, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveds"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdvanceReceivedResult> CancelAdvanceReceived(IEnumerable<AdvanceReceived> receiveds, CancellationToken token)
            => await advanceReceivedProcessor.CancelAsync(receiveds, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReceiptHeader>> GetHeaderItems([FromBody] int companyId, CancellationToken token)
            => (await receiptHeaderProcessor.GetItemsAsync(companyId, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerIds"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReceiptApportion>> GetApportionItems([FromBody] IEnumerable<long> headerIds, CancellationToken token)
            => (await receiptApportionProcessor.GetAsync(headerIds, token)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apportions"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReceiptApportionsResult> Apportion(IEnumerable<ReceiptApportion> apportions, CancellationToken token)
            => await receiptApportionProcessor.ApportionAsync(apportions, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurrencyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistCurrency([FromBody] int CurrencyId, CancellationToken token)
            => await receiptProcessor.ExistCurrencyAsync(CurrencyId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistReceiptCategory([FromBody] int CategoryId, CancellationToken token)
            => await receiptProcessor.ExistReceiptCategoryAsync(CategoryId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistCustomer([FromBody] int CustomerId, CancellationToken token)
            => await receiptProcessor.ExistCustomerAsync(CustomerId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SectionId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistSection([FromBody] int SectionId, CancellationToken token)
            => await receiptProcessor.ExistSectionAsync(SectionId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistCompany([FromBody] int CompanyId, CancellationToken token)
            => await receiptProcessor.ExistCompanyAsync(CompanyId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistExcludeCategory([FromBody] int CategoryId, CancellationToken token)
            => await receiptExcludeProcessor.ExistExcludeCategoryAsync(CategoryId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReceiptId"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistOriginalReceipt([FromBody] long ReceiptId, CancellationToken token)
            => await receiptProcessor.ExistOriginalReceiptAsync(ReceiptId, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistNonApportionedReceipt(ClosingSearch option, CancellationToken token)
            => await receiptProcessor.ExistNonApportionedAsync(option.CompanyId, option.ClosingAtFrom, option.ClosingAtTo, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistNonOutputedReceipt(ClosingSearch option, CancellationToken token)
            => await receiptProcessor.ExistNonOutputedAsync(option.CompanyId, option.ClosingAtFrom, option.ClosingAtTo, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> ExistNonAssignmentReceipt(ClosingSearch option, CancellationToken token)
            => await receiptProcessor.ExistNonAssignmentAsync(option.CompanyId, option.ClosingAtFrom, option.ClosingAtTo, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CountResult> Omit(OmitSource source, CancellationToken token)
            => await receiptProcessor.OmitAsync(source, token);


        /// <summary>
        /// 入金振替処理
        /// </summary>
        /// <param name="transfers"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ReceiptSectionTransfersResult> SaveReceiptSectionTransfer(IEnumerable<ReceiptSectionTransfer> transfers, CancellationToken token)
            => await receiptSectionTransferProcessor.SaveAsync(transfers, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReceiptSectionTransfer>> GetReceiptSectionTransferForPrint([FromBody] int companyId)
            => (await receiptSectionTransferProcessor.GetReceiptSectionTransferForPrintAsync(companyId)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<ReceiptSectionTransfer>> UpdateReceiptSectionTransferPrintFlag([FromBody] int companyId)
            => (await receiptSectionTransferProcessor.UpdateReceiptSectionTransferPrintFlagAsync(companyId)).ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<int>> ReceiptImportDuplicationCheck(ReceiptImportDuplicationSearch option, CancellationToken token)
            => (await receiptProcessor.ReceiptImportDuplicationCheckAsync(option.CompanyId, option.Items, option.Details, token)).ToArray();

        /// <summary>
        /// 前受振替
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> AdvanceReceivedDataSplit(AdvanceReceivedSplitSource source, CancellationToken token)
            => await advanceReceivedSplitProcessor.SplitAsync(source, token);

        /// <summary>
        /// 前受取消
        /// </summary>
        /// <param name="source"></param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> CancelAdvanceReceivedDataSplit(AdvanceReceivedSplitSource source, CancellationToken token)
            => await advanceReceivedSplitProcessor.CancelAsync(source, token);



        #region journalizing

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable< JournalizingSummary >> GetReceiptJournalizingSummaryAsync(JournalizingOption option)
        {
            return (await receiptJournalizingProcessor.GetSummaryAsync(option)).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable< ReceiptJournalizing >> ExtractReceiptJournalizingAsync(JournalizingOption option)
        {
            return (await receiptJournalizingProcessor.ExtractAsync(option)).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable< GeneralJournalizing >> ExtractReceiptGeneralJournalizingAsync(JournalizingOption option)
        {
            return (await receiptJournalizingProcessor.ExtractGeneralAsync(option)).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> UpdateOutputAtAsync(JournalizingOption option)
            => await receiptJournalizingProcessor.UpdateOutputAtAsync(option);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> CancelReceiptJournalizingAsync(JournalizingOption option)
            => await receiptJournalizingProcessor.CancelJournalizingAsync(option);

        #endregion


        /// <summary>インポート対象 ファイルの読み込み・検証・一時データの保存</summary>
        /// <param name="source">CompanyId, LoginUserId, ImporterSettingId, EncodingCodePage, Data を設定</param>
        /// <param name="token">自動バインド</param>
        /// <returns>戻り値の <see cref="ImportData.Id"/>が登録処理に必須となる</returns>
        [HttpPost]
        public async Task<ImportDataResult> Read(TransactionImportSource source, CancellationToken token)
            => await receiptFileImportProcessor.ReadAsync(source, token);


        /// <summary>インポート処理 （登録処理）</summary>
        /// <param name="source"> CompanyId, LoginUserId, ImportDataId <see cref="ImportData.Id"/> を指定</param>
        /// <param name="token">自動バインド</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportDataResult> Import(TransactionImportSource source, CancellationToken token)
            => await receiptFileImportProcessor.ImportAsync(source, token);
    }
}