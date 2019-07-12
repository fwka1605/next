using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Common
{
    public class CustomerLedgerProcessor :
        ICustomerLedgerProcessor
    {
        private readonly ICustomerLedgerQueryProcessor ledgerQueryProcessor;
        public CustomerLedgerProcessor(ICustomerLedgerQueryProcessor ledgerQueryProcessor)
        {
            this.ledgerQueryProcessor = ledgerQueryProcessor;
        }
        public async Task<IEnumerable<CustomerLedger>> GetAsync(CustomerLedgerSearch searchOption, CancellationToken token, IProgressNotifier notifier)
        {
            var ledgers = (await ledgerQueryProcessor.GetAsync(searchOption, token, notifier)).ToArray();
            var balances = (await ledgerQueryProcessor.GetBalancesAsync(searchOption, token, notifier))
                .ToDictionary(x => x.Key, x => x.Value);

            SortedList<int, long[]> keys = null;
            if (searchOption.DisplayMatchingSymbol)
            {
                var items = await ledgerQueryProcessor.GetKeysAsync(searchOption, token, notifier);
                keys = new SortedList<int, long[]>(
                    items.GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToArray()));
            }
            var dicKeys = new Dictionary<long, int>();
            List<long> balanceKeys = null;
            List<long> currentKeys = null;
            var monthlyKeys = new List<long>();
            var carryOverKeys = new List<long>();
            var symbol = string.Empty;
            var carryOverIndex = 0;

            var balance = 0M;
            decimal? slipTotal = null;
            var closingDay = searchOption.YearMonthTo.Day;
            if (closingDay > 27) closingDay = 99;
            DateTime yearMonth;
            var nextYM = new DateTime(0);
            var monthSub = GetSubtotalList();
            var parentSub = GetSubtotalList();

            var result = new List<CustomerLedger>();

            var parentCustomerId = 0;
            for (var i = 0; i < ledgers.Length; i++)
            {
                var ledger = ledgers[i];
                var nextLedger = (i + 1 < ledgers.Length) ? ledgers[i + 1] : null;
                var newParent = ledger.ParentCustomerId != parentCustomerId;

                ParseYearMonth(ledger, closingDay, out yearMonth);
                ledger.YearMonth = yearMonth;
                if (newParent)
                {
                    if (searchOption.DisplayMatchingSymbol)
                    {
                        balanceKeys = keys.ContainsKey(ledger.ParentCustomerId)
                            ? keys[ledger.ParentCustomerId].ToList() : new List<long>();
                        monthlyKeys.Clear();
                        carryOverKeys.Clear();
                    }
                    balance = balances.ContainsKey(ledger.ParentCustomerId)
                        ? balances[ledger.ParentCustomerId] : 0M;
                    var carryOver = new CustomerLedger();
                    carryOver.ParentCustomerId = ledger.ParentCustomerId;
                    carryOver.ParentCustomerCode = ledger.ParentCustomerCode;
                    carryOver.ParentCustomerName = ledger.ParentCustomerName;
                    carryOver.CurrencyCode = ledger.CurrencyCode;
                    carryOver.RecordType = CustomerLedger.RecordTypeAlias.CarryOverRecord;
                    carryOver.YearMonth = yearMonth;
                    carryOver.RemainAmount = balance;
                    result.Add(carryOver);
                    if (searchOption.DisplayMatchingSymbol)
                        carryOverIndex = result.IndexOf(carryOver);
                }

                parentCustomerId = ledger.ParentCustomerId;

                if (searchOption.DisplaySlipTotal
                    && ledger.DataType == 1
                    && ledger.BillingAmount.HasValue)
                    slipTotal = (slipTotal ?? 0M) + ledger.BillingAmount.Value;
                var setSlipTotal = searchOption.DisplaySlipTotal && ledger.DataType == 1
                    && (nextLedger == null
                        || ledger.DataType != nextLedger.DataType
                        || ledger.BillingInputId != nextLedger.BillingInputId
                        || ledger.RecordedAt != nextLedger.RecordedAt);

                if (searchOption.DisplayMatchingSymbol && ledger.DataType != 2)
                {
                    currentKeys = ledger.GetKeys();
                    ConvertKeysToSymbols(dicKeys, currentKeys, ref symbol);
                }
                if (ledger.DataType == 1 && ledger.BillingAmount.HasValue) balance += ledger.BillingAmount.Value;
                if (ledger.DataType == 2 && ledger.ReceiptAmount.HasValue && searchOption.UseReceipt) balance -= ledger.ReceiptAmount.Value;
                if (ledger.DataType == 3 && ledger.MatchingAmount.HasValue && !searchOption.UseReceipt) balance -= ledger.MatchingAmount.Value;

                if (setSlipTotal)
                {
                    ledger.SlipTotal = slipTotal;
                    slipTotal = null;
                }
                AddSubtotal(monthSub, ledger);
                AddSubtotal(parentSub, ledger);

                if (ledger.DataType == 1 && (!searchOption.DisplaySlipTotal || setSlipTotal)
                 || ledger.DataType == 2 && searchOption.UseReceipt
                 || ledger.DataType == 3 && !searchOption.UseReceipt)
                {
                    ledger.RemainAmount = balance;
                }

                if (searchOption.DisplayMatchingSymbol)
                {
                    if (ledger.DataType == 1)
                    {
                        ledger.MatchingSymbolBilling = symbol;
                        foreach (var key in currentKeys)
                            if (!balanceKeys.Contains(key)
                                && !monthlyKeys.Contains(key))
                                monthlyKeys.Add(key);
                    }
                    if (ledger.DataType == 3)
                    {
                        ledger.MatchingSymbolReceipt = symbol;
                        foreach (var key in currentKeys)
                            if (balanceKeys.Contains(key)
                                && !carryOverKeys.Contains(key))
                                carryOverKeys.Add(key);
                    }
                }
                ledger.RecordType = CustomerLedger.RecordTypeAlias.DataRecord;
                ledger.Truncate(searchOption.UnitPrice);
                result.Add(ledger);

                var sameYM = IsSameYearMonth(yearMonth, nextLedger, closingDay, out nextYM);
                var sameParent = nextLedger?.ParentCustomerId == ledger.ParentCustomerId;


                if (!sameYM)
                {
                    var sub = new CustomerLedger();
                    sub.ParentCustomerId = ledger.ParentCustomerId;
                    sub.ParentCustomerCode = ledger.ParentCustomerCode;
                    sub.ParentCustomerName = ledger.ParentCustomerName;
                    sub.CurrencyCode = ledger.CurrencyCode;
                    sub.YearMonth = yearMonth;
                    sub.RecordType = CustomerLedger.RecordTypeAlias.MonthlySubtotalRecord;
                    sub.RemainAmount = balance;
                    //sub.Caption = $"{yearMont:MM}月合計";
                    SetSubtotal(monthSub, sub);
                    ClearSubtotal(monthSub);
                    sub.Truncate(searchOption.UnitPrice);
                    result.Add(sub);
                }

                if (!sameYM && sameParent
                    && searchOption.IsPrint && searchOption.RequireMonthlyBreak)
                {
                    var carryOver = new CustomerLedger();
                    carryOver.ParentCustomerId = ledger.ParentCustomerId;
                    carryOver.ParentCustomerCode = ledger.ParentCustomerCode;
                    carryOver.ParentCustomerName = ledger.ParentCustomerName;
                    carryOver.CurrencyCode = ledger.CurrencyCode;
                    carryOver.YearMonth = nextYM;
                    carryOver.RecordType = CustomerLedger.RecordTypeAlias.CarryOverRecord;
                    carryOver.RemainAmount = balance;
                    //carryOver.Caption = "繰越";
                    carryOver.Truncate(searchOption.UnitPrice);
                    result.Add(carryOver);
                    if (searchOption.DisplayMatchingSymbol)
                    {
                        ConvertKeysToSymbols(dicKeys, carryOverKeys, ref symbol);
                        result[carryOverIndex].MatchingSymbolBilling = symbol;

                        foreach (var key in monthlyKeys)
                            if (!balanceKeys.Contains(key)) balanceKeys.Add(key);
                        monthlyKeys.Clear();
                        carryOverKeys.Clear();
                        carryOverIndex = result.IndexOf(carryOver);
                    }
                }

                if (!sameParent)
                {
                    var sub = new CustomerLedger();
                    sub.ParentCustomerId = ledger.ParentCustomerId;
                    sub.ParentCustomerCode = ledger.ParentCustomerCode;
                    sub.ParentCustomerName = ledger.ParentCustomerName;
                    sub.CurrencyCode = ledger.CurrencyCode;
                    sub.YearMonth = yearMonth;
                    sub.RecordType = CustomerLedger.RecordTypeAlias.TotalRecord;
                    sub.RemainAmount = balance;
                    //sub.Caption = "総合計";
                    SetSubtotal(parentSub, sub);
                    ClearSubtotal(parentSub);
                    sub.Truncate(searchOption.UnitPrice);
                    result.Add(sub);
                    if (searchOption.DisplayMatchingSymbol)
                    {
                        ConvertKeysToSymbols(dicKeys, carryOverKeys, ref symbol);
                        result[carryOverIndex].MatchingSymbolBilling = symbol;
                    }
                }
            }
            notifier?.UpdateState();
            return result;
        }

        private void ParseYearMonth(CustomerLedger ledger, int closingDay, out DateTime yearMonth)
        {
            var ymd = ledger.RecordedAt.Value;
            if (closingDay <= 27 && closingDay < ymd.Day
                && !(ymd.Year == 9999 && ymd.Month == 12))
                ymd = ymd.AddMonths(1);
            yearMonth = new DateTime(ymd.Year, ymd.Month,
                closingDay != 99 ? closingDay : DateTime.DaysInMonth(ymd.Year, ymd.Month));
        }
        private bool IsSameYearMonth(DateTime yearMonth, CustomerLedger next, int closingDay, out DateTime nextYM)
        {
            nextYM = new DateTime(0);
            if (next == null) return false;
            ParseYearMonth(next, closingDay, out nextYM);
            return yearMonth.Equals(nextYM);
        }


        #region 合計金額
        private enum SubtotalField
        {
            BillingAmount = 0,
            BillingSlipAmount,
            ReceiptAmount,
            MatchingAmount,
        }

        private List<decimal> GetSubtotalList()
        {
            var count = Enum.GetValues(typeof(SubtotalField)).Length;
            return Enumerable.Repeat(0M, count).ToList();
        }
        private void ClearSubtotal(List<decimal> list)
        {
            for (var i = 0; i < list.Count; i++)
                list[i] = 0M;
        }

        private void SetSubtotal(List<decimal> subtotal, CustomerLedger ledger)
        {
            ledger.BillingAmount  = subtotal[(int)SubtotalField.BillingAmount];
            ledger.SlipTotal      = subtotal[(int)SubtotalField.BillingSlipAmount];
            ledger.ReceiptAmount  = subtotal[(int)SubtotalField.ReceiptAmount];
            ledger.MatchingAmount = subtotal[(int)SubtotalField.MatchingAmount];
        }

        private void AddSubtotal(List<decimal> subtotal, CustomerLedger ledger)
        {
            subtotal[(int)SubtotalField.BillingAmount]      += ledger.BillingAmount  ?? 0M;
            subtotal[(int)SubtotalField.BillingSlipAmount]  += ledger.SlipTotal      ?? 0M;
            subtotal[(int)SubtotalField.ReceiptAmount]      += ledger.ReceiptAmount  ?? 0M;
            subtotal[(int)SubtotalField.MatchingAmount]     += ledger.MatchingAmount ?? 0M;
        }


        #endregion

        #region 消込記号

        private void ConvertKeysToSymbols(Dictionary<long, int> dic, List<long> keys, ref string symbol)
        {
            var symbols = new List<string>();
            for (var i = 0; i < keys.Count; i++)
            {
                if (i == 5)
                {
                    symbols.Add("…");
                    break;
                }
                else
                {
                    if (!dic.ContainsKey(keys[i]))
                    {
                        dic.Add(keys[i], dic.Count + 1);
                    }
                    symbols.Add(ConvertKeyToSymbol(dic[keys[i]]));
                }
            }
            symbol = string.Join(",", symbols.ToArray());
        }

        /// <summary>任意の整数から アルファベット2桁の記号を返す処理</summary>
        /// <param name="number">1以上の整数</param>
        /// <returns>
        ///  A : 1             or 1 + (26 + 1) * 26 ...
        /// ZZ : (26 + 1) * 26 ...
        /// </returns>
        /// <remarks>英字最大二文字のため、 27 * 26 の剰余で循環</remarks>
        private string ConvertKeyToSymbol(int number)
        {
            const int AlphabetCount = 26;
            const int AlphabetStart = 0x40;
            var remainder = (number - 1) % ((AlphabetCount + 1) * AlphabetCount) + 1;
            var quotient = (remainder - 1) / (AlphabetCount);
            var rem = (remainder - 1) % AlphabetCount + 1;
            var symbol = string.Empty;
            if (quotient > 0) symbol = Convert.ToChar(AlphabetStart + quotient).ToString();
            symbol += Convert.ToChar(AlphabetStart + rem).ToString();
            return symbol;
        }

        #endregion
    }
}
