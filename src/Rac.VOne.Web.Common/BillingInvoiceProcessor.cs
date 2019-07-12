using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;

namespace Rac.VOne.Web.Common
{
    public class BillingInvoiceProcessor : IBillingInvoiceProcessor
    {
        private readonly IBillingInputProcessor billingInputProcessor;
        private readonly IBillingProcessor billingProcessor;
        private readonly IInvoiceNumberSettingProcessor invoiceNumberSettingProcessor;
        private readonly IInvoiceNumberHistoryProcessor invoiceNumberHistoryProcessor;
        private readonly IInvoiceCommonSettingProcessor invoiceCommonSettingProcessor;

        private readonly IBillingInvoiceQueryProcessor billingInvoiceQueryProcessor;

        private readonly ITransactionScopeBuilder transactionScopeBuilder;

        private InvoiceNumberSetting invoiceNumberSetting { get; set; }
        private long MaxNumber { get; set; }

        public BillingInvoiceProcessor(
            IBillingInvoiceQueryProcessor billingInvoiceQueryProcessor,
            IBillingInputProcessor billingInputProcessor,
            IBillingProcessor billingProcessor,
            IInvoiceNumberSettingProcessor invoiceNumberSettingProcessor,
            IInvoiceNumberHistoryProcessor invoiceNumberHistoryProcessor,
            IInvoiceCommonSettingProcessor invoiceCommonSettingProcessor,
            ITransactionScopeBuilder transactionScopeBuilder
            )
        {
            this.billingInvoiceQueryProcessor = billingInvoiceQueryProcessor;
            this.billingInputProcessor = billingInputProcessor;
            this.billingProcessor = billingProcessor;
            this.invoiceNumberSettingProcessor = invoiceNumberSettingProcessor;
            this.invoiceNumberHistoryProcessor = invoiceNumberHistoryProcessor;
            this.invoiceCommonSettingProcessor = invoiceCommonSettingProcessor;
            this.transactionScopeBuilder = transactionScopeBuilder;
        }

        public async Task<IEnumerable<BillingInvoice>> GetAsync(BillingInvoiceSearch searchOption,
           CancellationToken token = default(CancellationToken))
        {
            var setting = await invoiceCommonSettingProcessor.GetAsync(searchOption.CompanyId, token);
            return await billingInvoiceQueryProcessor.GetAsync(searchOption, setting, token);
        }

        public async Task<BillingInputResult> PublishInvoicesAsync(BillingInvoiceForPublish[] invoices,
            int loginUserId,
            CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new BillingInputResult {
                    BillingInputIds = new List<long>(),
                    ProcessResult   = new ProcessResult(),
                };

                var first = invoices.First();

                // refactor private variable to local variable
                invoiceNumberSetting = await invoiceNumberSettingProcessor.GetAsync(first.CompanyId, token);
                var useNumbering = (invoiceNumberSetting?.UseNumbering == 1);
                if (useNumbering)
                {
                    MaxNumber = Convert.ToInt64("999999999999999".Substring(0, invoiceNumberSetting.Length));
                }

                //同時実行制御
                //最終更新日時をチェック
                var tempIds = invoices.Select(x => x.TemporaryBillingInputId).ToArray();
                var maxUpdateAt = await billingInvoiceQueryProcessor.GetMaxUpdateAtAsync(first.ClientKey, tempIds, token);
                var maxUpdateAtSource = invoices.Max(x => x.UpdateAt);
                if (!maxUpdateAt.HasValue ||  maxUpdateAtSource != maxUpdateAt)
                {
                    result.BillingInputIds = null;
                    result.ProcessResult.ErrorCode = ErrorCode.OtherUserAlreadyUpdated;
                    return result;
                }

                foreach (BillingInvoiceForPublish invoice in invoices)
                {
                    var doUpdateInvoiceCode = false;
                    InvoiceNumberHistory history = null;
                    if (useNumbering
                        && string.IsNullOrEmpty(invoice.InvoiceCode))
                    {
                        doUpdateInvoiceCode = true;
                        //自動採番の請求書番号を取得
                        var condition = GetKeysForNumberingHistory(invoice);

                        history = await GetHistoryAsync(condition, token);

                        invoice.InvoiceCode = GetNewInvoiceCode(invoice, condition, history);
                    }

                    if (invoice.BillingInputId == null || invoice.BillingInputId == 0)
                    {
                        //InputId新規生成
                        invoice.BillingInputId = billingInputProcessor.SaveAsync().Id;
                        //請求データ側のInputIdも更新
                        await billingInvoiceQueryProcessor.UpdateBillingForPublishNewInputId(invoice, doUpdateInvoiceCode, token);
                    }
                    else
                        await billingProcessor.UpdateBillingForPublishAsync(invoice, doUpdateInvoiceCode, token);

                    //発行済みフラグの更新
                    var billingInputSource = new BillingInputSource {
                        Id                      = invoice.BillingInputId.Value,
                        IsFirstPublish          = true,
                        UseInvoiceCodeNumbering = doUpdateInvoiceCode ? 1 : 0,
                        InvoiceTemplateId       = invoice.InvoiceTemplateId
                    };
                    var resultBillingInput = billingInputProcessor.UpdateForPublishAsync(billingInputSource);

                    if (invoice.BillingInputId.HasValue && resultBillingInput != null)
                        result.BillingInputIds.Add(invoice.BillingInputId.Value);

                    //自動採番履歴を更新
                    if (history != null)
                    {
                        history.UpdateBy = loginUserId;
                        await invoiceNumberHistoryProcessor.SaveAsync(history, token);
                    }
                }

                if (result.BillingInputIds.Count != invoices.Count())
                    return null;

                scope.Complete();

                result.ProcessResult.Result = true;
                return result;
            }

        }

        public async Task<IEnumerable<BillingInvoiceDetailForPrint>> GetDetailsForPrintAsync(BillingInvoiceDetailSearch option, CancellationToken token = default(CancellationToken))
        {
            var invoiceCommonSetting = await invoiceCommonSettingProcessor.GetAsync(option.CompanyId, token);
            return await billingInvoiceQueryProcessor.GetDetailsForPrintAsync(option, invoiceCommonSetting, token);
        }

        public async Task<int> DeleteWorkTableAsync(byte[] clientKey, CancellationToken token = default(CancellationToken))
             => await billingInvoiceQueryProcessor.DeleteWorkTableAsync(clientKey, token);

        public async Task<CountResult> CancelPublishAsync(long[] BillingInputIds, CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new CountResult() { ProcessResult = new ProcessResult(), Count = 0 };

                await billingProcessor.UpdateForResetInvoiceCodeAsync(BillingInputIds, token);
                await billingProcessor.UpdateForResetInputIdAsync(BillingInputIds, token);
                var deleteCount = await billingInputProcessor.DeleteForCancelPublishAsync(BillingInputIds, token);
                var resetCount = await billingInputProcessor.UpdateForCancelPublishAsync(BillingInputIds, token);

                result.Count = (deleteCount + resetCount);
                result.ProcessResult.Result = (result.Count == BillingInputIds.Count());
                if (result.ProcessResult.Result) scope.Complete();

                return result;
            }
        }


        #region 採番取得処理

        #region private Enum
        /// <summary>連番リセット通し番号のリセット単位</summary>
        private enum ResetType
        {
            /// <summary>年次 年の切り替わりは<see cref="InvoiceNumberSetting.ResetMonth"/></summary>
            Year = 0,
            /// <summary>月次</summary>
            Month,
            /// <summary>最大値に到達したらリセット</summary>
            Max
        }
        /// <summary>使用日付 請求書番号 採番書式設定</summary>
        private enum FormatType
        {
            OnlyNumber = 0,
            NumberAndDate,
            NumberAndFixedString
        }
        private enum DateFormat
        { yyyy = 0, yyyyMM }
        private enum FixedStringType
        { isFixed = 0, byTemplate }

        /// <summary>書式の配置 ?? 採番書式設定が 数値のみ 以外の場合に、付加文字列の位置指定</summary>
        private enum DisplayFormat
        {
            /// <summary>指定書式 + 連番</summary>
            Prefix = 0,
            /// <summary>連番 + 指定書式</summary>
            Suffix
        }

        private enum DateType
        { BilledAt = 0, ClosingAt}

        #endregion

        #region 請求書番号取得

        private async Task<InvoiceNumberHistory> GetHistoryAsync(InvoiceNumberHistory condition, CancellationToken token)
        {
            var history = await invoiceNumberHistoryProcessor.GetAsync(condition, token);

            if (history == null)
            {
                history = condition;
                history.LastNumber = 0;
            }
            return history;
        }

        private string GetNewInvoiceCode(BillingInvoiceForPublish billing, InvoiceNumberHistory condition, InvoiceNumberHistory history)
        {
            var invoiceCode = "";
            //最大まで到達したらリセット
            if (history.LastNumber >= MaxNumber)
            {
                history.LastNumber = 1;
            }
            else
            {
                history.LastNumber++;
            }

            //前ゼロ処理
            if (invoiceNumberSetting.ZeroPadding == 0)
            {
                invoiceCode += history.LastNumber.ToString();
            }
            else
            {
                invoiceCode += history.LastNumber.ToString().PadLeft(invoiceNumberSetting.Length,'0');
            }

            //連番のみ
            if (invoiceNumberSetting.FormatType == (int)FormatType.OnlyNumber)
            {
                return invoiceCode;
            }

            var formatString = string.Empty;
            //日付を使用
            if (invoiceNumberSetting.FormatType == (int)FormatType.NumberAndDate)
            {
                var targetDate = invoiceNumberSetting.DateType == (int)DateType.BilledAt
                ? billing.BilledAt
                : billing.ClosingAt;
                if (invoiceNumberSetting.DateFormat == (int)DateFormat.yyyy)
                {
                    formatString = condition.NumberingYear;
                }
                else
                {
                    formatString = condition.NumberingYear + condition.NumberingMonth;
                }
            }
            //固定文字を使用
            else
            {
                //完全固定
                if (invoiceNumberSetting.FixedStringType == (int)FixedStringType.isFixed)
                {
                    formatString = invoiceNumberSetting.FixedString;
                }
                //文面パターンごとに固定
                else
                {
                    formatString = billing.InvoiceTemplateFixedString;
                }
            }

            invoiceCode = GetDisplayFormat(invoiceCode, formatString);
            return invoiceCode;
        }
        private string GetDisplayFormat(string invoiceCode, string format)
        {
            if (invoiceNumberSetting.DisplayFormat == (int)DisplayFormat.Prefix)
            {
                return (format + invoiceNumberSetting.Delimiter + invoiceCode);
            }
            else
            {
                return (invoiceCode + invoiceNumberSetting.Delimiter + format);
            }
        }

        /// <summary>??? </summary>
        /// <param name="billing"></param>
        /// <returns></returns>
        private InvoiceNumberHistory GetKeysForNumberingHistory(BillingInvoiceForPublish billing)
        {

            var history = new InvoiceNumberHistory { CompanyId = billing.CompanyId };

            if (invoiceNumberSetting.ResetType == (int)ResetType.Max)
            {
                if (invoiceNumberSetting.FormatType == (int)FormatType.NumberAndFixedString
                    && invoiceNumberSetting.FixedStringType == (int)FixedStringType.isFixed)
                {
                    history.FixedString = invoiceNumberSetting.FixedString;
                    return history;

                }
                else if (invoiceNumberSetting.FormatType == (int)FormatType.NumberAndFixedString
                  && invoiceNumberSetting.FixedStringType != (int)FixedStringType.isFixed)
                {
                    history.FixedString = billing.InvoiceTemplateFixedString;
                    return history;
                }
                else if (invoiceNumberSetting.FormatType == (int)FormatType.OnlyNumber)
                {
                    return history;
                }
            }

            var targetDate = invoiceNumberSetting.DateType == (int)DateType.BilledAt
            ? billing.BilledAt
            : billing.ClosingAt;
            var year = targetDate.Year;
            var month = targetDate.Month;

            if (invoiceNumberSetting.ResetType == (int)(ResetType.Year))
            {
                //var targetYear = targetDate.Year;
                //var targetMonth = targetDate.Month;

                var x = (invoiceNumberSetting.ResetMonth.Value - 1);
                if (month <= x)
                {
                    history.NumberingYear = (year - 1).ToString();
                }
                else
                {
                    history.NumberingYear = targetDate.ToString("yyyy");
                }
                return history;
            }
            else
            {
                history.NumberingYear = year.ToString("0000");
                history.NumberingMonth = month.ToString("00");
                return history;
            }

        }
        #endregion

        #endregion

        public async Task<IEnumerable<BillingInvoiceDetailForExport>> GetDetailsForExportAsync(
            IEnumerable<long> BillingInputIds,
            int CompanyId,
            CancellationToken token = default(CancellationToken))
        {
            var setting = await invoiceCommonSettingProcessor.GetAsync(CompanyId, token);
            return await billingInvoiceQueryProcessor.GetDetailsForExportAsync(BillingInputIds, setting, token);
        }

        public async Task<CountResult> UpdatePublishAtAsync(long[] BillingInputIds,
            CancellationToken token = default(CancellationToken))
        {
            using (var scope = transactionScopeBuilder.Create())
            {
                var result = new CountResult() { ProcessResult = new ProcessResult(), Count = 0 };

                await Task.Run(() => {

                    foreach (var id in BillingInputIds)
                    {
                        var billingInputSource = new BillingInputSource()
                        {
                            Id = id,
                            IsFirstPublish = false
                        };
                        var billingInput = billingInputProcessor.UpdateForPublishAsync(billingInputSource);
                        if (billingInput != null)
                        {
                            result.Count++;
                        }
                    }
                    result.ProcessResult.Result = (result.Count == BillingInputIds.Count());
                });

                if (result.ProcessResult.Result) scope.Complete();

                return result;

            }
        }


    }
}
