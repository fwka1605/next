using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;

namespace Rac.VOne.Web.Models.Files
{
    public class PaymentSchedule
    {
        public PaymentSchedule()
        {
        }

        /// <summary>入金予定額</summary>
        public string DueAmount { get; set; }
        /// <summary>会社コード</summary>
        public string CompanyCode { get; set; }
        /// <summary>債権代表者コード</summary>
        public string CustomerGroupCode { get; set; }
        /// <summary>得意先コード</summary>
        public string CustomerCode { get; set; }
        /// <summary>請求日</summary>
        public string BilledAt { get; set; }
        /// <summary>請求金額</summary>
        public string BillingAmount { get; set; }
        /// <summary>消費税</summary>
        public string TaxAmount { get; set; }
        /// <summary>入金予定日</summary>
        public string DueAt { get; set; }
        /// <summary>売上部門コード</summary>
        public string DepartmentCode { get; set; }
        /// <summary>債権科目</summary>
        public string DebitAccountTitleCode { get; set; }
        /// <summary>売上日</summary>
        public string SalesAt { get; set; }
        /// <summary>請求書番号</summary>
        public string InvoiceCode { get; set; }
        /// <summary>請求締日</summary>
        public string ClosingAt { get; set; }
        /// <summary>担当者コード</summary>
        public string StaffCode { get; set; }
        /// <summary>備考</summary>
        public string Note1 { get; set; }
        /// <summary>請求区分コード</summary>
        public string BillingCategoryCode { get; set; }
        /// <summary>備考2</summary>
        public string Note2 { get; set; }
        /// <summary>備考3</summary>
        public string Note3 { get; set; }
        /// <summary>備考4</summary>
        public string Note4 { get; set; }
        /// <summary>備考5</summary>
        public string Note5 { get; set; }
        /// <summary>備考6</summary>
        public string Note6 { get; set; }
        /// <summary>備考7</summary>
        public string Note7 { get; set; }
        /// <summary>備考8</summary>
        public string Note8 { get; set; }
        /// <summary>通貨コード</summary>
        public string CurrencyCode { get; set; }
        /// <summary>入金予定キー</summary>
        public string DueKey { get; set; }
    }
}
