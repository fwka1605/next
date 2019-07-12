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
    public class CreditAgingListProcessor :
        ICreditAgingListProcessor
    {
        private readonly ICreditAgingListQueryProcessor creditAgingListQueryProcessor;

        public CreditAgingListProcessor(
            ICreditAgingListQueryProcessor creditAgingListQueryProcessor)
        {
            this.creditAgingListQueryProcessor = creditAgingListQueryProcessor;
        }

        public async Task< IEnumerable<CreditAgingList>> GetAsync(CreditAgingListSearch SearchOption,
            IProgressNotifier notifier = null, CancellationToken token = default(CancellationToken))
        {
            var opt = SearchOption;
            opt.InitializeYearMonthValue();
            var details = await creditAgingListQueryProcessor.GetAsync(opt, notifier, token);
            var unit = SearchOption.UnitPrice;

            // key break subtotal
            const int staffTotal = 1;
            const int departmentTotal = 2;
            const int grandTotal = 3;

            var list = new List<CreditAgingList>();
            var pcusIndexBuf = (int?)null;
            var stafSub = GetSubtotalList();
            var deptSub = GetSubtotalList();
            var pcusSub = GetSubtotalList();
            var grndSub = GetSubtotalList();
            var anyBalanceMinus = false;
            CreditAgingList detailBuf = null;

            foreach (var detail in details)
            {
                detail.UnsettledRemain = detail.ReceivableAmount;
                detail.BillingRemain = detail.BillingAmount
                    - (opt.ConsiderReceiptAmount ? detail.ReceiptAmount : detail.MatchingAmount);
                detail.CreditAmount = detail.ReceivableAmount + detail.BillingRemain;
                detail.ArrivalDueDate1 = detail.ReceivableAmount_1;
                detail.ArrivalDueDate2 = detail.ReceivableAmount_2;
                detail.ArrivalDueDate3 = detail.ReceivableAmount_3;
                detail.ArrivalDueDate4 = detail.ReceivableAmount_4;

                var doCalcCreditLimitSingleLine
                    = !opt.UseParentCustomerCredit
                    || !detail.ParentCustomerId.HasValue;

                if (!opt.CalculateCreditLimitRegistered || detail.CreditLimit != 0M)
                    detail.CreditBalance
                        = (doCalcCreditLimitSingleLine ? detail.CreditLimit : 0M)
                        - detail.CreditAmount;

                var skip = opt.ConsiderCustomerGroup
                && opt.ConsiderReceiptAmount
                && detail.ParentCustomerId == detail.CustomerId
                && detail.CreditAmount == 0M
                && detail.CreditLimit == 0M;


                if (detail.CreditAmount == 0M
                    && detail.UnsettledRemain == 0M) continue;

                if (pcusIndexBuf.HasValue
                    && RequireCustomerGroupSubtotal(opt, detailBuf, detail))
                {
                    if (opt.UseParentCustomerCredit)
                        pcusSub[(int)SubtotalField.CreditBalance] += pcusSub[(int)SubtotalField.CreditLimit];

                    if (RequireFilterPositiveCreditBalance(opt, pcusSub, anyBalanceMinus))
                        RemovePostiveCustomerGroup(opt, list, pcusIndexBuf.Value, pcusSub, deptSub, stafSub, grndSub);
                    else
                    {
                        SetSubtotal(list[pcusIndexBuf.Value], pcusSub, unit);

                        if (opt.UseParentCustomerCredit)
                        {
                            if (opt.RequireStaffTotal)
                                AddCreditLimitSubtotal(stafSub, list[pcusIndexBuf.Value]);
                            if (opt.RequireDepartmentTotal)
                                AddCreditLimitSubtotal(deptSub, list[pcusIndexBuf.Value]);
                            AddCreditLimitSubtotal(grndSub, list[pcusIndexBuf.Value]);
                        }
                    }

                    ResetSubtotal(pcusSub);
                    pcusIndexBuf = null;
                }

                if (RequireStaffSubtotal(opt, detailBuf, detail))
                {
                    if (list.Any())
                        list.Add(GetSubtotal(detailBuf, stafSub, staffTotal, unit));
                    ResetSubtotal(stafSub);
                }

                if (RequireDepartmentSubtotal(opt, detailBuf, detail))
                {
                    if (list.Any())
                        list.Add(GetSubtotal(detailBuf, deptSub, departmentTotal, unit));
                    ResetSubtotal(deptSub);
                }

                if (opt.ConsiderCustomerGroup
                    && detail.ParentCustomerId.HasValue
                    && (
                            detailBuf?.ParentCustomerId != detail.ParentCustomerId
                         || detailBuf?.StaffId          != detail.StaffId
                         || detailBuf?.DepartmentId     != detail.DepartmentId
                       ))
                {
                    anyBalanceMinus = false;
                    var parent = GetSubtotal(detail, null, 0, unit);
                    parent.ParentCustomerId = detail.ParentCustomerId;
                    parent.CustomerId = detail.ParentCustomerId.Value;
                    parent.CustomerCode = detail.ParentCustomerCode;
                    parent.CustomerName = detail.ParentCustomerName;
                    parent.ParentCustomerFlag = 1;
                    parent.CollectCategory = detail.ParentCollectCategoryName;
                    list.Add(parent);
                    pcusIndexBuf = list.IndexOf(parent);
                }

                if (DoSkipWhenSingleCustomerAndPositiveCreditBalance(opt, detail)) continue;
                if (IsCustomerGroupNegativeCreditBalance(opt, detail)) anyBalanceMinus = true;


                if (opt.ConsiderCustomerGroup && pcusIndexBuf.HasValue)
                {
                    AddSubtotal(pcusSub, detail, !opt.UseParentCustomerCredit);

                    if (opt.UseParentCustomerCredit)
                    {
                        pcusSub[(int)SubtotalField.CreditLimit]     = detail.CreditLimit ?? 0M;
                        pcusSub[(int)SubtotalField.CreditBalance]  += detail.CreditBalance ?? 0M;
                        detail.CreditLimit = null;
                        detail.CreditBalance = null;
                    }
                }

                if (opt.RequireStaffTotal)
                    AddSubtotal(stafSub, detail, doCalcCreditLimitSingleLine);

                if (opt.RequireDepartmentTotal)
                    AddSubtotal(deptSub, detail, doCalcCreditLimitSingleLine);

                AddSubtotal(grndSub, detail, doCalcCreditLimitSingleLine);

                if (!skip)
                {
                    list.Add(detail);
                    TruncateValue(detail, unit);
                }
                detailBuf = detail;
            }

            if (pcusIndexBuf.HasValue
                && RequireCustomerGroupSubtotal(opt, detailBuf, null))
            {
                if (opt.UseParentCustomerCredit)
                    pcusSub[(int)SubtotalField.CreditBalance] += pcusSub[(int)SubtotalField.CreditLimit];

                if (RequireFilterPositiveCreditBalance(opt, pcusSub, anyBalanceMinus))
                    RemovePostiveCustomerGroup(opt, list, pcusIndexBuf.Value, pcusSub, deptSub, stafSub, grndSub);
                else
                {
                    SetSubtotal(list[pcusIndexBuf.Value], pcusSub, unit);

                    if (opt.UseParentCustomerCredit)
                    {
                        if (opt.RequireStaffTotal)
                            AddCreditLimitSubtotal(stafSub, list[pcusIndexBuf.Value]);
                        if (opt.RequireDepartmentTotal)
                            AddCreditLimitSubtotal(deptSub, list[pcusIndexBuf.Value]);
                        AddCreditLimitSubtotal(grndSub, list[pcusIndexBuf.Value]);
                    }
                }
            }

            if (RequireStaffSubtotal(opt, detailBuf, null) && list.Any())
                list.Add(GetSubtotal(detailBuf, stafSub, staffTotal, unit));

            if (RequireDepartmentSubtotal(opt, detailBuf, null) && list.Any())
                list.Add(GetSubtotal(detailBuf, deptSub, departmentTotal, unit));

            if (detailBuf != null && list.Any())
                list.Add(GetSubtotal(detailBuf, grndSub, grandTotal, unit));

            notifier?.UpdateState();

            return list;
        }

        #region key break condition

        private bool RequireFilterPositiveCreditBalance(CreditAgingListSearch opt,
            List<decimal> pcusSub, bool anyBalanceMinus)
            => opt.FilterPositiveCreditBalance
                && (
                    opt.ConsiderGroupWithCalculate && pcusSub[(int)SubtotalField.CreditBalance] > 0M
                || !opt.ConsiderGroupWithCalculate && !anyBalanceMinus );

        private bool DoSkipWhenSingleCustomerAndPositiveCreditBalance(CreditAgingListSearch opt, CreditAgingList detail)
            => (opt.FilterPositiveCreditBalance
                    && !detail.ParentCustomerId.HasValue
                    && !detail.IsMinusCreditBalance);

        private bool IsCustomerGroupNegativeCreditBalance(CreditAgingListSearch opt, CreditAgingList detail)
            => (opt.FilterPositiveCreditBalance
                    && detail.ParentCustomerId.HasValue
                    && detail.IsMinusCreditBalance);

        private bool RequireCustomerGroupSubtotal(CreditAgingListSearch opt,
            CreditAgingList detailBuf, CreditAgingList detail)
            => opt.ConsiderCustomerGroup
                && detailBuf != null
                && detailBuf.ParentCustomerId.HasValue
                && (detail == null
                    || (                               detailBuf.ParentCustomerId  != detail.ParentCustomerId
                    || opt.RequireDepartmentTotal   && detailBuf.DepartmentId      != detail.DepartmentId
                    || opt.RequireStaffTotal        && detailBuf.StaffId           != detail.StaffId)
                    );

        private bool RequireStaffSubtotal(CreditAgingListSearch opt,
            CreditAgingList detailBuf, CreditAgingList detail)
            => opt.RequireStaffTotal
                && detailBuf != null
                && (detail == null
                    || (                               detailBuf.StaffId       !=  detail.StaffId
                    || opt.RequireDepartmentTotal   && detailBuf.DepartmentId  != detail.DepartmentId)
                    );

        private bool RequireDepartmentSubtotal(CreditAgingListSearch opt,
            CreditAgingList detailBuf, CreditAgingList detail)
            => opt.RequireDepartmentTotal
                && detailBuf != null
                && (detail == null
                    || (detailBuf.DepartmentId != detail.DepartmentId)
                    );
        #endregion

        private void RemovePostiveCustomerGroup(CreditAgingListSearch opt,
            List<CreditAgingList> list,
            int index,
            List<decimal> pcusSub,
            List<decimal> deptSub,
            List<decimal> stafSub,
            List<decimal> grndSub)
        {
            for (var i = list.Count - 1; i > index; i--)
                list.RemoveAt(i);
            if (opt.RequireDepartmentTotal)
                RemoveSubtotal(deptSub, pcusSub, !opt.UseParentCustomerCredit);
            if (opt.RequireStaffTotal)
                RemoveSubtotal(stafSub, pcusSub, !opt.UseParentCustomerCredit);
            RemoveSubtotal(grndSub, pcusSub, !opt.UseParentCustomerCredit);
            list.RemoveAt(index);

        }

        private enum SubtotalField
        {
            CreditAmount = 0,
            UnsettledRemain,
            BillingRemain,
            CreditLimit,
            CreditBalance,
            ArrivalDueDate1,
            ArrivalDueDate2,
            ArrivalDueDate3,
            ArrivalDueDate4,
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
        private void AddSubtotal(List<decimal> subtotal, CreditAgingList detail, bool doCalcLimitSingleLine = true)
        {
            subtotal[(int)SubtotalField.CreditAmount]    += detail.CreditAmount;
            subtotal[(int)SubtotalField.UnsettledRemain] += detail.UnsettledRemain;
            subtotal[(int)SubtotalField.BillingRemain]   += detail.BillingRemain;
            if (doCalcLimitSingleLine)
            {
                subtotal[(int)SubtotalField.CreditLimit]     += detail.CreditLimit ?? 0M;
                subtotal[(int)SubtotalField.CreditBalance]   += detail.CreditBalance ?? 0M;
            }
            subtotal[(int)SubtotalField.ArrivalDueDate1] += detail.ArrivalDueDate1;
            subtotal[(int)SubtotalField.ArrivalDueDate2] += detail.ArrivalDueDate2;
            subtotal[(int)SubtotalField.ArrivalDueDate3] += detail.ArrivalDueDate3;
            subtotal[(int)SubtotalField.ArrivalDueDate4] += detail.ArrivalDueDate4;
        }
        private void AddCreditLimitSubtotal(List<decimal> subtotal, CreditAgingList detail)
        {
            subtotal[(int)SubtotalField.CreditLimit]    += detail.CreditLimit ?? 0M;
            subtotal[(int)SubtotalField.CreditBalance]  += detail.CreditBalance ?? 0M;
        }

        private void RemoveSubtotal(List<decimal> subtotal, List<decimal> detail, bool doCaclLimitSingleLine = true)
        {
            subtotal[(int)SubtotalField.CreditAmount]       -= detail[(int)SubtotalField.CreditAmount];
            subtotal[(int)SubtotalField.UnsettledRemain]    -= detail[(int)SubtotalField.UnsettledRemain];
            subtotal[(int)SubtotalField.BillingRemain]      -= detail[(int)SubtotalField.BillingRemain];
            if (doCaclLimitSingleLine)
            {
                subtotal[(int)SubtotalField.CreditLimit]        -= detail[(int)SubtotalField.CreditLimit];
                subtotal[(int)SubtotalField.CreditBalance]      -= detail[(int)SubtotalField.CreditBalance];
            }
            subtotal[(int)SubtotalField.ArrivalDueDate1]    -= detail[(int)SubtotalField.ArrivalDueDate1];
            subtotal[(int)SubtotalField.ArrivalDueDate2]    -= detail[(int)SubtotalField.ArrivalDueDate2];
            subtotal[(int)SubtotalField.ArrivalDueDate3]    -= detail[(int)SubtotalField.ArrivalDueDate3];
            subtotal[(int)SubtotalField.ArrivalDueDate4]    -= detail[(int)SubtotalField.ArrivalDueDate4];
        }

        private CreditAgingList GetSubtotal(CreditAgingList detail, List<decimal> subtotal, int subtotalType, decimal unit)
        {
            var result = new CreditAgingList();
            result.RecordType       = subtotalType;
            result.StaffId          = detail.StaffId;
            result.StaffCode        = detail.StaffCode;
            result.StaffName        = detail.StaffName;
            result.DepartmentId     = detail.DepartmentId;
            result.DepartmentCode   = detail.DepartmentCode;
            result.DepartmentName   = detail.DepartmentName;
            SetSubtotal(result, subtotal, unit);
            return result;
        }

        private void SetSubtotal(CreditAgingList detail, List<decimal> subtotal, decimal unit)
        {
            detail.CreditAmount     = subtotal?[(int)SubtotalField.CreditAmount]    ?? 0M;
            detail.UnsettledRemain  = subtotal?[(int)SubtotalField.UnsettledRemain] ?? 0M;
            detail.BillingRemain    = subtotal?[(int)SubtotalField.BillingRemain]   ?? 0M;
            detail.CreditLimit      = subtotal?[(int)SubtotalField.CreditLimit]     ?? 0M;
            detail.CreditBalance    = subtotal?[(int)SubtotalField.CreditBalance]   ?? 0M;
            detail.ArrivalDueDate1  = subtotal?[(int)SubtotalField.ArrivalDueDate1] ?? 0M;
            detail.ArrivalDueDate2  = subtotal?[(int)SubtotalField.ArrivalDueDate2] ?? 0M;
            detail.ArrivalDueDate3  = subtotal?[(int)SubtotalField.ArrivalDueDate3] ?? 0M;
            detail.ArrivalDueDate4  = subtotal?[(int)SubtotalField.ArrivalDueDate4] ?? 0M;
            TruncateValue(detail, unit);
        }

        private void TruncateValue(CreditAgingList detail, decimal unit)
        {
            if (unit <= 1M) return;
            detail.CreditAmount     = detail.CreditAmount    / unit;
            detail.UnsettledRemain  = detail.UnsettledRemain / unit;
            detail.BillingRemain    = detail.BillingRemain   / unit;
            detail.CreditLimit      = detail.CreditLimit     / unit;
            detail.CreditBalance    = detail.CreditBalance   / unit;
            detail.ArrivalDueDate1  = detail.ArrivalDueDate1 / unit;
            detail.ArrivalDueDate2  = detail.ArrivalDueDate2 / unit;
            detail.ArrivalDueDate3  = detail.ArrivalDueDate3 / unit;
            detail.ArrivalDueDate4  = detail.ArrivalDueDate4 / unit; 
        }


    }
}
