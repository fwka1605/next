using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rac.VOne.Data.QueryProcessors;
using Rac.VOne.Web.Models;
using Rac.VOne.Common;

namespace Rac.VOne.Web.Common
{
    public class BillingAgingListProcessor : IBillingAgingListProcessor
    {
        private readonly IBillingAgingListQueryProcessor billingAgingListQueryProcessor;

        public BillingAgingListProcessor(
            IBillingAgingListQueryProcessor billingAgingListQueryProcessor)
        {
            this.billingAgingListQueryProcessor = billingAgingListQueryProcessor;
        }

        /// <summary>請求残高年齢表データ取得 オプションによって合計行計算も実施</summary>
        /// <param name="SearchOption"></param>
        /// <returns></returns>
        /// <remarks>
        ///  合計行の集計
        ///  TODO: next → 合計を端数処理、端数処理した値を合計 変えられるようにする
        /// </remarks>
        public async Task<IEnumerable<BillingAgingList>> GetAsync(BillingAgingListSearch searchOption,
            IProgressNotifier notifier = null,
            CancellationToken token = default(CancellationToken))
        {
            const int staffSubtotalType = 1;
            const int departmentSubtotalType = 2;
            const int grandTotalType = 3;

            var opt = searchOption;
            opt.InitializeYearMonthConditions();

            var details = (await billingAgingListQueryProcessor.GetAsync(opt, notifier, token)).ToArray();

            var useReceipt = opt.BillingRemainType == 1;
            var unit = opt.UnitValue;

            var list = new List<BillingAgingList>();
            var pcusIndexBuf = (int?)null;

            var stafSub = GetSubtotalList();
            var deptSub = GetSubtotalList();
            var pcusSub = GetSubtotalList();
            var grndSub = GetSubtotalList();

            BillingAgingList detailBuf = null;
            foreach (var detail in details)
            {
                detail.CurrentMonthSales = detail.BillingAmount0;
                detail.CurrentMonthReceipt = detail.ReceiptAmount0;
                detail.CurrentMonthMatching = detail.MatchingAmount0;

                detail.LastMonthRemain = detail.Balance + detail.BillingAmountK
                    - (useReceipt ? detail.ReceiptAmountK : detail.MatchingAmountK);

                detail.CurrentMonthRemain = detail.LastMonthRemain + detail.CurrentMonthSales
                    - (useReceipt ? (detail.CurrentMonthReceipt ?? 0M) : detail.CurrentMonthMatching);

                detail.MonthlyRemain0 = detail.BillingAmount0 - detail.BillingMatchingAmount0 + (useReceipt ? detail.MatchingAmount0 - detail.ReceiptAmount0 : 0M);
                detail.MonthlyRemain1 = detail.BillingAmount1 - detail.BillingMatchingAmount1 + (useReceipt ? detail.MatchingAmount1 - detail.ReceiptAmount1 : 0M);
                detail.MonthlyRemain2 = detail.BillingAmount2 - detail.BillingMatchingAmount2 + (useReceipt ? detail.MatchingAmount2 - detail.ReceiptAmount2 : 0M);
                detail.MonthlyRemain3 = detail.BillingAmount3 - detail.BillingMatchingAmount3 + (useReceipt ? detail.MatchingAmount3 - detail.ReceiptAmount3 : 0M)
                                      + detail.BillingAmount4 - detail.BillingMatchingAmount4 + (useReceipt ? detail.MatchingAmount4 - detail.ReceiptAmount4 : 0M);

                var skip = searchOption.ConsiderCustomerGroup
                    && searchOption.BillingRemainType > 0
                    && detail.ParentCustomerId == detail.CustomerId
                    && detail.CurrentMonthSales == 0M
                    && detail.CurrentMonthMatching == 0M;

                if (detail.LastMonthRemain == 0M
                    && detail.CurrentMonthReceipt == 0M
                    && detail.CurrentMonthRemain == 0M) continue;

                if (pcusIndexBuf.HasValue
                    && RequireCustomerGroupSubtotal(opt, detailBuf, detail))
                {
                    SetSubtotal(list[pcusIndexBuf.Value], pcusSub, unit);
                    ResetSubtotal(pcusSub);
                    pcusIndexBuf = null;
                }

                if (RequireStaffSubtotal(opt, detailBuf, detail))
                {
                    list.Add(GetSubtotal(detailBuf, stafSub, staffSubtotalType, unit));
                    ResetSubtotal(stafSub);
                }

                if (RequireDepartmentSubtotal(opt, detailBuf, detail))
                {
                    list.Add(GetSubtotal(detailBuf, deptSub, departmentSubtotalType, unit));
                    ResetSubtotal(deptSub);
                }

                if (opt.ConsiderCustomerGroup
                    && detail.ParentCustomerId.HasValue
                    && (detailBuf?.ParentCustomerId != detail.ParentCustomerId
                        || detailBuf?.StaffId != detail.StaffId
                        || detailBuf?.DepartmentId != detail.DepartmentId))
                {
                    var parent = GetSubtotal(detail, null, 0, unit);
                    parent.ParentCustomerId = detail.ParentCustomerId;
                    parent.CustomerId = detail.ParentCustomerId.Value;
                    parent.CustomerCode = detail.ParentCustomerCode;
                    parent.CustomerName = detail.ParentCustomerName;
                    parent.ParentCustomerFlag = 1;
                    list.Add(parent);
                    pcusIndexBuf = list.IndexOf(parent);
                }

                if (opt.RequireStaffSubtotal)
                    AddSubtotal(stafSub, detail);
                if (opt.RequireDepartmentSubtotal)
                    AddSubtotal(deptSub, detail);
                if (opt.ConsiderCustomerGroup && pcusIndexBuf.HasValue)
                    AddSubtotal(pcusSub, detail);
                AddSubtotal(grndSub, detail);

                if (opt.ConsiderCustomerGroup
                    && opt.BillingRemainType > 0
                    && detail.ParentCustomerId.HasValue)
                {
                    detail.LastMonthRemain = null;
                    detail.CurrentMonthReceipt = null;
                    detail.CurrentMonthRemain = null;
                    detail.MonthlyRemain0 = null;
                    detail.MonthlyRemain1 = null;
                    detail.MonthlyRemain2 = null;
                    detail.MonthlyRemain3 = null;
                }

                if (!skip)
                {
                    list.Add(detail);
                    TruncateValue(detail, unit);
                }

                detailBuf = detail;
            }

            if (pcusIndexBuf.HasValue
                && RequireCustomerGroupSubtotal(opt, detailBuf, null))
                SetSubtotal(list[pcusIndexBuf.Value], pcusSub, unit);

            if (RequireStaffSubtotal(opt, detailBuf, null))
                list.Add(GetSubtotal(detailBuf, stafSub, staffSubtotalType, unit));

            if (RequireDepartmentSubtotal(opt, detailBuf, null))
                list.Add(GetSubtotal(detailBuf, deptSub, departmentSubtotalType, unit));

            if (detailBuf != null)
                list.Add(GetSubtotal(detailBuf, grndSub, grandTotalType, unit));

            notifier?.UpdateState();

            return list;
        }

        private bool RequireCustomerGroupSubtotal(BillingAgingListSearch opt, BillingAgingList detailBuf, BillingAgingList detail)
        {
            return opt.ConsiderCustomerGroup
                    && detailBuf != null
                    && detailBuf.ParentCustomerId.HasValue
                    && (detail == null
                        || (detailBuf.ParentCustomerId != detail.ParentCustomerId
                            || opt.RequireStaffSubtotal && detailBuf.StaffId != detail.StaffId
                            || opt.RequireDepartmentSubtotal && detailBuf.DepartmentId != detail.DepartmentId)
                        );
        }

        private bool RequireStaffSubtotal(BillingAgingListSearch opt, BillingAgingList detailBuf, BillingAgingList detail)
        {
            return opt.RequireStaffSubtotal
                    && detailBuf != null
                    && (detail == null
                        || (opt.RequireStaffSubtotal && detailBuf.StaffId != detail.StaffId
                            || opt.RequireDepartmentSubtotal && detailBuf.DepartmentId != detail.DepartmentId)
                        );
        }
        private bool RequireDepartmentSubtotal(BillingAgingListSearch opt, BillingAgingList detailBuf, BillingAgingList detail)
        {
            return opt.RequireDepartmentSubtotal
                    && detailBuf != null
                    && (detail == null
                        || (opt.RequireDepartmentSubtotal && detailBuf.DepartmentId != detail.DepartmentId)
                        );
        }

        private BillingAgingList GetSubtotal(BillingAgingList detail, List<decimal> subtotal, int subtotalType, decimal unit)
        {
            var result = new BillingAgingList();
            result.RecordType           = subtotalType;
            result.StaffId              = detail.StaffId;
            result.StaffCode            = detail.StaffCode;
            result.StaffName            = detail.StaffName;
            result.DepartmentId         = detail.DepartmentId;
            result.DepartmentCode       = detail.DepartmentCode;
            result.DepartmentName       = detail.DepartmentName;
            result.CurrencyCode         = detail.CurrencyCode;
            SetSubtotal(result, subtotal, unit);
            return result;
        }

        private void SetSubtotal(BillingAgingList detail, List<decimal> subtotal, decimal unit)
        {
            detail.LastMonthRemain      = subtotal?[(int)SubtotalField.LastRemain]      ?? 0M;
            detail.CurrentMonthSales    = subtotal?[(int)SubtotalField.CurrentBilling]  ?? 0M;
            detail.CurrentMonthReceipt  = subtotal?[(int)SubtotalField.CurrentReceipt]  ?? 0M;
            detail.CurrentMonthMatching = subtotal?[(int)SubtotalField.CurrentMatching] ?? 0M;
            detail.CurrentMonthRemain   = subtotal?[(int)SubtotalField.CurrentRemain]   ?? 0M;
            detail.MonthlyRemain0       = subtotal?[(int)SubtotalField.MonthlyRemain0]  ?? 0M;
            detail.MonthlyRemain1       = subtotal?[(int)SubtotalField.MonthlyRemain1]  ?? 0M;
            detail.MonthlyRemain2       = subtotal?[(int)SubtotalField.MonthlyRemain2]  ?? 0M;
            detail.MonthlyRemain3       = subtotal?[(int)SubtotalField.MonthlyRemain3]  ?? 0M;
            TruncateValue(detail, unit);
        }

        private void TruncateValue(BillingAgingList detail, decimal unit)
        {
            if (unit <= 1M) return;
            if (detail.LastMonthRemain.HasValue)
                detail.LastMonthRemain = detail.LastMonthRemain.Value / unit;
            detail.CurrentMonthSales = detail.CurrentMonthSales / unit;
            if (detail.CurrentMonthReceipt.HasValue)
                detail.CurrentMonthReceipt = detail.CurrentMonthReceipt.Value / unit;
            detail.CurrentMonthMatching = detail.CurrentMonthMatching / unit;
            if (detail.CurrentMonthRemain.HasValue)
                detail.CurrentMonthRemain = detail.CurrentMonthRemain.Value / unit;
            if (detail.MonthlyRemain0.HasValue)
                detail.MonthlyRemain0 = detail.MonthlyRemain0.Value / unit;
            if (detail.MonthlyRemain1.HasValue)
                detail.MonthlyRemain1 = detail.MonthlyRemain1.Value / unit;
            if (detail.MonthlyRemain2.HasValue)
                detail.MonthlyRemain2 = detail.MonthlyRemain2.Value / unit;
            if (detail.MonthlyRemain3.HasValue)
                detail.MonthlyRemain3 = detail.MonthlyRemain3.Value / unit;
        }

        private List<decimal> GetSubtotalList()
        {
            var count = Enum.GetValues(typeof(SubtotalField)).Length;
            return Enumerable.Repeat(0M, count).ToList();
        }
        private void ResetSubtotal(List<decimal> list)
        {
            for (var i = 0; i < list.Count; i++)
                list[i] = 0M;
        }

        private void AddSubtotal(List<decimal> subtotal, BillingAgingList detail)
        {
            subtotal[(int)SubtotalField.LastRemain]      += detail.LastMonthRemain ?? 0M;
            subtotal[(int)SubtotalField.CurrentBilling]  += detail.CurrentMonthSales;
            subtotal[(int)SubtotalField.CurrentReceipt]  += detail.CurrentMonthReceipt ?? 0M;
            subtotal[(int)SubtotalField.CurrentMatching] += detail.CurrentMonthMatching;
            subtotal[(int)SubtotalField.CurrentRemain]   += detail.CurrentMonthRemain ?? 0M;
            subtotal[(int)SubtotalField.MonthlyRemain0]  += detail.MonthlyRemain0 ?? 0M;
            subtotal[(int)SubtotalField.MonthlyRemain1]  += detail.MonthlyRemain1 ?? 0M;
            subtotal[(int)SubtotalField.MonthlyRemain2]  += detail.MonthlyRemain2 ?? 0M;
            subtotal[(int)SubtotalField.MonthlyRemain3]  += detail.MonthlyRemain3 ?? 0M;
        }

        private enum SubtotalField
        {
            LastRemain = 0,
            CurrentBilling,
            CurrentReceipt,
            CurrentMatching,
            CurrentRemain,
            MonthlyRemain0,
            MonthlyRemain1,
            MonthlyRemain2,
            MonthlyRemain3,
        }


        public async Task<IEnumerable<BillingAgingListDetail>> GetDetailsAsync(BillingAgingListSearch option, CancellationToken token = default(CancellationToken))
        {
            option.InitializeYearMonthConditions();
            return await billingAgingListQueryProcessor.GetDetailsAsync(option, token);
        }
    }
}
