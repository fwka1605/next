using Rac.VOne.Common;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Web.Common
{
    public class CollectionScheduleProcessor :
        ICollectionScheduleProcessor
    {
        private readonly ICollectionScheduleQueryProcessor collectionScheduleQueryProcessor;
        private readonly ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor;
        private readonly ICustomerPaymentContractQueryProcessor customerPaymentContractQueryProcessor;

        public CollectionScheduleProcessor(
            ICollectionScheduleQueryProcessor collectionScheduleQueryProcessor,
            ICategoryByCodeQueryProcessor categoryByCodeQueryProcessor,
            ICustomerPaymentContractQueryProcessor customerPaymentContractQueryProcessor
            )
        {
            this.collectionScheduleQueryProcessor = collectionScheduleQueryProcessor;
            this.categoryByCodeQueryProcessor = categoryByCodeQueryProcessor;
            this.customerPaymentContractQueryProcessor = customerPaymentContractQueryProcessor;
        }

        private const int RecordTypeData = 0;
        private const int RecordTypeStaffSubtotal = 1;
        private const int RecordTypeDepartmentSubtotal = 2;
        private const int RecordTypeCollectCategoryTotal = 3;
        private const int RecordTypeGrandTotal = 4;

        /// <summary>回収予定表 データ抽出/整形</summary>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        /// <remarks>
        /// 得意先 回収区分の情報をどのように取り扱うか ビジネスロジックはここに寄せる
        /// </remarks>
        public async Task<IEnumerable<CollectionSchedule>> GetAsync(CollectionScheduleSearch searchOption, CancellationToken token, IProgressNotifier notifier)
        {
            var initializeResult = searchOption.InitializeYearMonth();

            var schedules = (await collectionScheduleQueryProcessor.GetAsync(searchOption, token ,notifier))
                .Where(x => x.HasAnyValue).ToList();

            if (!(schedules?.Any() ?? false))
            {
                notifier?.UpdateState();
                return Enumerable.Empty<CollectionSchedule>();
            }

            const int collectCategoryType = 3;
            const string ContractCode = "00";
            var categories = (await categoryByCodeQueryProcessor.GetAsync(new CategorySearch { CompanyId = searchOption.CompanyId, CategoryType = collectCategoryType }, token)).ToList();
            var contractId = categories.FirstOrDefault(x => x.Code == ContractCode)?.Id ?? 0;
            var customerIds = schedules.Where(x => x.CustomerCollectCategoryId == contractId)
                .Select(x => x.CustomerId).Distinct().ToArray();
            var contracts = await customerPaymentContractQueryProcessor.GetAsync(customerIds, token);


            var result = new List<CollectionSchedule>();
            var groupCount = 0;

            var subtotal = new Dictionary<string, Dictionary<int, decimal>>();
            var subDpt = new Dictionary<string, Dictionary<int, decimal>>();
            var subStf = new Dictionary<string, Dictionary<int, decimal>>();
            var deptBuf = string.Empty;
            var stafBuf = string.Empty;
            var requireDepartmentSubtotal = searchOption.IsPrint && searchOption.NewPagePerDepartment;
            var requireStaffSubtotal = searchOption.IsPrint && searchOption.NewPagePerStaff;

            foreach (var group in schedules.GroupBy(x => x.CustomerId))
            {
                var detail = group.First();
                var category = categories.FirstOrDefault(x => x.Id != contractId && x.Id == detail.CustomerCollectCategoryId);
                var contract = contracts.FirstOrDefault(x => x.CustomerId == detail.CustomerId);
                var customerInfo = GetCustomerInfo(detail, category, contract);

                if (requireStaffSubtotal
                    && !string.IsNullOrEmpty(stafBuf) && stafBuf != detail.StaffCode)
                    result.AddRange(GetSubtotalRecords(subStf, categories, deptBuf, stafBuf, 1, "担当者計", ref groupCount));

                if (requireDepartmentSubtotal
                    && !string.IsNullOrEmpty(deptBuf) && deptBuf != detail.DepartmentCode)
                    result.AddRange(GetSubtotalRecords(subDpt, categories, deptBuf, stafBuf, 2, "部門計", ref groupCount));

                var list = new List<CollectionSchedule>();

                CollectionSchedule contractPayment = null;
                foreach (var item in group)
                {
                    if (item.CollectCategoryCode == ContractCode)
                    {
                        contractPayment = item;
                        continue;
                    }
                    list.Add(item);
                }

                if (contractPayment != null && contract != null)
                {
                    foreach (var divided in Divide(contractPayment, contract))
                    {
                        var index = list.FindIndex(x => x.CollectCategoryCode == divided.CollectCategoryCode);
                        if (index < 0)
                        {
                            list.Add(divided);
                            continue;
                        }
                        list[index].UncollectedAmountLast += divided.UncollectedAmountLast;
                        list[index].UncollectedAmount0 += divided.UncollectedAmount0;
                        list[index].UncollectedAmount1 += divided.UncollectedAmount1;
                        list[index].UncollectedAmount2 += divided.UncollectedAmount2;
                        list[index].UncollectedAmount3 += divided.UncollectedAmount3;
                    }
                    list.Sort((x, y) => string.Compare(x.CollectCategoryCode, y.CollectCategoryCode));
                }

                for (var i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                    {
                        list[i].RowId = ++groupCount;
                    }
                    else
                    {
                        list[i].DepartmentName = string.Empty;
                        list[i].StaffName = string.Empty;
                        list[i].ClosingDay = null;
                    }

                    var code = list[i].CollectCategoryCode;
                    if (!subtotal.ContainsKey(code)) subtotal.Add(code, Pivot(null));
                    if (requireDepartmentSubtotal && !subDpt.ContainsKey(code)) subDpt.Add(code, Pivot(null));
                    if (requireStaffSubtotal && !subStf.ContainsKey(code)) subStf.Add(code, Pivot(null));

                    if (!list[i].HasAnyValue) continue;

                    AddValue(subtotal[code], list[i]);
                    if (requireDepartmentSubtotal) AddValue(subDpt[code], list[i]);
                    if (requireStaffSubtotal) AddValue(subStf[code], list[i]);
                }

                for (var i = 0; i < customerInfo.Count; i++)
                {
                    if (list.Count <= i)
                    {
                        var item = new CollectionSchedule();
                        item.CustomerId = detail.CustomerId;
                        item.StaffCode = detail.StaffCode;
                        item.DepartmentCode = detail.DepartmentCode;
                        list.Add(item);
                    }
                    list[i].CustomerInfo = customerInfo[i];

                }
                result.AddRange(list);

                deptBuf = detail.DepartmentCode;
                stafBuf = detail.StaffCode;
            }

            if (requireStaffSubtotal)
                result.AddRange(GetSubtotalRecords(subStf, categories, deptBuf, stafBuf, 1, "担当者計", ref groupCount));

            if (requireDepartmentSubtotal)
                result.AddRange(GetSubtotalRecords(subDpt, categories, deptBuf, stafBuf, 2, "部門計", ref groupCount));

            // grand total
            {
                // category total
                result.AddRange(GetSubtotalRecords(new Dictionary<string, Dictionary<int, decimal>>(subtotal), categories, "", "", 3, "合計", ref groupCount));

                var item = GetNewItem(string.Empty, string.Empty);
                item.CustomerInfo = "総合計";
                item.RowId = ++groupCount;
                item.RecordType = 4;
                foreach (var key in subtotal.Keys)
                    foreach (var field in subtotal[key].Keys)
                        SetValue(item, field, subtotal[key][field]);

                result.Add(item);
            }

            if (searchOption.UnitPrice != 1M)
            {
                foreach (var item in result)
                    item.Truncate(searchOption.UnitPrice);
            }
            notifier?.UpdateState();
            return result;
        }

        /// <summary>
        /// 得意先情報 および 得意先マスター設定の 回収区分情報取得
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="category"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        private List<string> GetCustomerInfo(CollectionSchedule detail,
            Category category,
            CustomerPaymentContract contract)
        {
            var list = new List<string>();
            list.Add($"{detail.CustomerCode} {detail.CustomerName}");
            if (category != null)
            {
                list.Add(detail.CustomerSightOfBill.HasValue ?
                    $"　{category.Name} {detail.CustomerSightOfBill}日" :
                    $"　{category.Name}");
                return list;
            }
            if (contract == null) return list;
            list.Add($"　{contract.ThresholdValue:#,##0}円未満 {contract.LessThanName}");

            {
                var item = new List<string>();
                item.Add("　以上");
                item.Add($"{contract.GreaterThanRate1}％");
                item.Add(contract.GreaterThanName1);
                if (contract.GreaterThanSightOfBill1 != 0)
                    item.Add($"{contract.GreaterThanSightOfBill1}日");
                if (contract.GreaterThanRoundingMode1 == 0)
                    item.Add("端数");
                list.Add(string.Join(" ", item.ToArray()));
            }

            if (contract.GreaterThanCollectCategoryId2.HasValue)
            {
                var item = new List<string>();
                item.Add("　以上");
                item.Add($"{contract.GreaterThanRate2}％");
                item.Add(contract.GreaterThanName2);
                if ((contract.GreaterThanSightOfBill2 ?? 0) != 0)
                    item.Add($"{contract.GreaterThanSightOfBill2}日");
                if (contract.GreaterThanRoundingMode2 == 0)
                    item.Add("端数");
                list.Add(string.Join(" ", item.ToArray()));
            }
            if (contract.GreaterThanCollectCategoryId3.HasValue)
            {
                var item = new List<string>();
                item.Add("　以上");
                item.Add($"{contract.GreaterThanRate3}％");
                item.Add(contract.GreaterThanName3);
                if ((contract.GreaterThanSightOfBill3 ?? 0) != 0)
                    item.Add($"{contract.GreaterThanSightOfBill3}日");
                if (contract.GreaterThanRoundingMode3 == 0)
                    item.Add("端数");
                list.Add(string.Join(" ", item.ToArray()));
            }
            return list;
        }

        /// <summary>
        /// 明細一行を約定条件によって分割する処理
        /// </summary>
        /// <param name="detail">分割対象の 約定の行</param>
        /// <param name="contract">約定条件</param>
        /// <returns>
        /// 列ごとの約定金額判定を実施するため、dic に 横縦変換して保持する
        /// </returns>
        private List<CollectionSchedule> Divide(CollectionSchedule detail,
            CustomerPaymentContract contract)
        {
            var list = new List<CollectionSchedule>();
            var dic = Pivot(detail);
            if (dic.Values.Any(x => x != 0M && x < contract.ThresholdValue))
            {
                list.Add(GetNewItem(contract.LessThanCode, contract.LessThanName, detail));
            }
            if (dic.Values.Any(x => x >= contract.ThresholdValue))
            {
                if (!list.Any(x => x.CollectCategoryCode == contract.GreaterThanCode1))
                    list.Add(GetNewItem(contract.GreaterThanCode1, contract.GreaterThanName1, detail));
                if ((contract.GreaterThanCollectCategoryId2 ?? 0) != 0
                    && !list.Any(x => x.CollectCategoryCode == contract.GreaterThanCode2))
                    list.Add(GetNewItem(contract.GreaterThanCode2, contract.GreaterThanName2, detail));
                if ((contract.GreaterThanCollectCategoryId3 ?? 0) != 0
                    && !list.Any(x => x.CollectCategoryCode == contract.GreaterThanCode3))
                    list.Add(GetNewItem(contract.GreaterThanCode3, contract.GreaterThanName3, detail));
            }

            foreach (var key in dic.Keys)
            {
                if (dic[key] == 0M) continue;
                if (dic[key] < contract.ThresholdValue)
                {
                    var index = list.FindIndex(x => x.CollectCategoryCode == contract.LessThanCode);
                    SetValue(list[index], key, dic[key]);
                    continue;
                }

                var remainingIndex = -1;
                var remaining = dic[key];

                {
                    var index = list.FindIndex(x => x.CollectCategoryCode == contract.GreaterThanCode1);
                    var mode = contract.GreaterThanRoundingMode1.Value;
                    if (mode == 0) remainingIndex = index;
                    var scale = GetScale(mode);
                    var value = decimal.Truncate(dic[key] * contract.GreaterThanRate1.Value / 100M / scale) * scale;
                    SetValue(list[index], key, value);
                    remaining -= value;
                }

                if ((contract.GreaterThanCollectCategoryId2 ?? 0) != 0)
                {
                    var index = list.FindIndex(x => x.CollectCategoryCode == contract.GreaterThanCode2);
                    var mode = contract.GreaterThanRoundingMode2.Value;
                    if (mode == 0) remainingIndex = index;
                    var scale = GetScale(mode);
                    var value = decimal.Truncate(dic[key] * contract.GreaterThanRate2.Value / 100M / scale) * scale;
                    SetValue(list[index], key, value);
                    remaining -= value;
                }

                if ((contract.GreaterThanCollectCategoryId3 ?? 0) != 0)
                {
                    var index = list.FindIndex(x => x.CollectCategoryCode == contract.GreaterThanCode3);
                    var mode = contract.GreaterThanRoundingMode3.Value;
                    if (mode == 0) remainingIndex = index;
                    var scale = GetScale(mode);
                    var value = decimal.Truncate(dic[key] * contract.GreaterThanRate3.Value / 100M / scale) * scale;
                    SetValue(list[index], key, value);
                    remaining -= value;
                }
                if (remaining != 0M && remainingIndex != -1)
                {
                    SetValue(list[remainingIndex], key, remaining);
                }
            }
            return list;
        }

        private IEnumerable<CollectionSchedule> GetSubtotalRecords(
            Dictionary<string, Dictionary<int, decimal>> subtotal,
            List<Category> categories,
            string departmentCode,
            string staffCode,
            int recordType,
            string subtotalCaption,
            ref int groupCount)
        {
            var result = new List<CollectionSchedule>();
            var first = subtotal.Keys.OrderBy(x => x).FirstOrDefault();
            foreach (var key in subtotal.Keys.OrderBy(x => x))
            {
                var category = categories.Find(x => x.Code == key);
                var item = GetNewItem(key, category.Name);
                item.DepartmentCode = departmentCode;
                item.StaffCode = staffCode;
                item.RecordType = recordType;
                foreach (var field in subtotal[key].Keys)
                    SetValue(item, field, subtotal[key][field]);
                if (key == first)
                {
                    item.CustomerInfo = subtotalCaption;
                    item.RowId = ++groupCount;
                }
                result.Add(item);
            }
            subtotal.Clear();
            return result;
        }

        /// <summary>
        /// 横縦変換処理 列をintのkeyで変換
        /// </summary>
        /// <param name="detail"></param>
        /// <returns>
        /// 0 : -1 カ月迄, 1 : 当月, 2 : 翌月, 3 : +2カ月, 4 : +3カ月以降
        /// reflection を利用して property setter を保持する案も考慮したが、
        /// 大量にreflection が発生するため、dictionary にて対応
        /// key を const で代替しても良いが、下記3つのメソッド内部のみの使用のため、別途 const は設けていない
        /// </returns>
        private Dictionary<int, decimal> Pivot(CollectionSchedule detail)
        {
            var dic = new Dictionary<int, decimal>(5);
            dic.Add(0, detail?.UncollectedAmountLast ?? 0M);
            dic.Add(1, detail?.UncollectedAmount0 ?? 0M);
            dic.Add(2, detail?.UncollectedAmount1 ?? 0M);
            dic.Add(3, detail?.UncollectedAmount2 ?? 0M);
            dic.Add(4, detail?.UncollectedAmount3 ?? 0M);
            return dic;
        }

        private void SetValue(CollectionSchedule detail, int key, decimal value)
        {
            switch (key)
            {
                case 0: detail.UncollectedAmountLast += value; break;
                case 1: detail.UncollectedAmount0    += value; break;
                case 2: detail.UncollectedAmount1    += value; break;
                case 3: detail.UncollectedAmount2    += value; break;
                case 4: detail.UncollectedAmount3    += value; break;
            }
        }

        private void AddValue(Dictionary<int, decimal> dic, CollectionSchedule detail)
        {
            if (dic == null) return;
            dic[0] += detail?.UncollectedAmountLast ?? 0M;
            dic[1] += detail?.UncollectedAmount0 ?? 0M;
            dic[2] += detail?.UncollectedAmount1 ?? 0M;
            dic[3] += detail?.UncollectedAmount2 ?? 0M;
            dic[4] += detail?.UncollectedAmount3 ?? 0M;
        }


        private CollectionSchedule GetNewItem(
            string collectCode, string collectName,
            CollectionSchedule detail = null)
        {
            var item = new CollectionSchedule();
            item.CustomerId                 = detail?.CustomerId ?? 0;
            item.CustomerCode               = detail?.CustomerCode;
            item.CustomerName               = detail?.CustomerName;
            item.CustomerCollectCategoryId  = detail?.CustomerCollectCategoryId ?? 0;
            item.CustomerSightOfBill        = detail?.CustomerSightOfBill;
            item.ClosingDay                 = detail?.ClosingDay;
            item.DepartmentCode             = detail?.DepartmentCode;
            item.DepartmentName             = detail?.DepartmentName;
            item.StaffCode                  = detail?.StaffCode;
            item.StaffName                  = detail?.StaffName;
            item.CollectCategoryCode        = collectCode;
            item.CollectCategoryName        = collectName;
            item.UncollectedAmountLast      = 0M;
            item.UncollectedAmount0         = 0M;
            item.UncollectedAmount1         = 0M;
            item.UncollectedAmount2         = 0M;
            item.UncollectedAmount3         = 0M;
            return item;
        }

        /// <summary>
        /// 約定条件 端数処理 桁数用金額取得
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private decimal GetScale(int mode)
        {
            switch (mode)
            {
                case 2 : return 10M;
                case 3 : return 100M;
                case 4 : return 1000M;
                case 5 : return 10000M;
                case 6 : return 100000M;
                default: return 1M;
            }
        }
    }
}
