using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Common;
using Rac.VOne.Common.Importer.Billing;

namespace Rac.VOne.Web.Models
{
    [DataContract]
    public class BillingImport
    {
        [DataMember] public int CompanyId { get; set; }
        [DataMember] public string CurrencyCode { get; set; }
        [DataMember] public string CustomerCode { get; set; }
        [DataMember] public string DepartmentCode { get; set; }
        [DataMember] public int DepartmentId { get; set; }
        [DataMember] public string StaffCode { get; set; }
        [DataMember] public int StaffId { get; set; }
        [DataMember] public string BillingCategoryCode { get; set; }
        [DataMember] public string InputType { get; set; }
        [DataMember] public string CollectCategoryCode { get; set; }
        [DataMember] public int CollectCategoryId { get; set; }
        [DataMember] public DateTime BilledAt { get; set; }
        [DataMember] public DateTime ClosingAt { get; set; }
        [DataMember] public DateTime SalesAt { get; set; }
        [DataMember] public DateTime DueAt { get; set; }
        [DataMember] public decimal BillingAmount { get; set; }
        [DataMember] public decimal TaxAmount { get; set; }
        [DataMember] public string DebitAccountTitleCode { get; set; }
        [DataMember] public string InvoiceCode { get; set; }
        [DataMember] public int TaxClassId { get; set; }
        [DataMember] public string Note1 { get; set; }
        [DataMember] public string Note2 { get; set; }
        [DataMember] public string Note3 { get; set; }
        [DataMember] public string Note4 { get; set; }
        [DataMember] public string Note5 { get; set; }
        [DataMember] public string Note6 { get; set; }
        [DataMember] public string Note7 { get; set; }
        [DataMember] public string Note8 { get; set; }
        [DataMember] public decimal Price { get; set; }
        [DataMember] public string ExclusiveBankCode { get; set; }
        [DataMember] public string ExclusiveBranchCode { get; set; }
        [DataMember] public string ExclusiveVirtualBranchCode { get; set; }
        [DataMember] public string ExclusiveAccountNumber { get; set; }
        [DataMember] public long? BillingId { get; set; }
        [DataMember] public int DiscountType { get; set; }
        [DataMember] public decimal DiscountAmount { get; set; }
        [DataMember] public int AssignmentFlag { get; set; }
        [DataMember] public int AutoCreationCustomerFlag {get;set;}
        [DataMember] public string ContractNumber { get; set; }
        [DataMember] public string CompanyCode { get; set; }
        [DataMember] public string CustomerName { get; set; }
        [DataMember] public string CustomerKana { get; set; }
        [DataMember] public string CollationKey { get; set; } = string.Empty;
        [DataMember] public int UseDiscount { get; set; }
        [DataMember] public int UseLongTermAdvanceReceived { get; set; }
        [DataMember] public int RegisterContractInAdvance { get; set; }
        public int LineNo { get; set; }
        [DataMember] public decimal TaxRate { get; set; }

        [DataMember] public string BilledAtForPrint { get; set; }
        [DataMember] public string ClosingAtForPrint { get; set; }
        [DataMember] public string DueAtForPrint { get; set; }
        [DataMember] public string SaleAtForPrint { get; set; }
        [DataMember] public string BillingAmountForPrint { get; set; }
        [DataMember] public string TaxAmountForPrint { get; set; }
        [DataMember] public string PriceForPrint { get; set; }
        [DataMember] public string UseDiscounForPrint { get; set; }
        [DataMember] public string TaxClassIdForPrint { get; set; }

        [DataMember] public int CustomerId { get; set; }
        [DataMember] public int DebitAccountTitleId { get; set; }
        [DataMember] public int BillingCategoryId { get; set; }
        [DataMember] public int CurrencyId { get; set; }


        public bool IsCurrencyIdEmpty() => CurrencyId == 0 && !string.IsNullOrEmpty(CurrencyCode);
        public bool IsCustomerIdEmpty() => CustomerId == 0 && !string.IsNullOrEmpty(CustomerCode);
        public bool IsDepartmentIdEmpty() => DepartmentId == 0 && !string.IsNullOrEmpty(DepartmentCode);
        public bool IsStaffIdEmpty() => StaffId == 0 && !string.IsNullOrEmpty(StaffCode);
        public bool IsAccountTitleIdEmpty() => DebitAccountTitleId == 0 && !string.IsNullOrEmpty(DebitAccountTitleCode);
        public bool IsBillingCategoryIdEmpty() => BillingCategoryId == 0 && !string.IsNullOrEmpty(BillingCategoryCode);
        public bool IsCollectCategoryIdEmpty() => CollectCategoryId == 0 && !string.IsNullOrEmpty(CollectCategoryCode);

        public Billing ConvertToBilling(int loginUserId) => new Billing
        {
            CompanyId           = this.CompanyId,
            CompanyCode         = this.CompanyCode,
            CurrencyId          = this.CurrencyId,
            CurrencyCode        = this.CurrencyCode,
            CustomerId          = this.CustomerId,
            CustomerCode        = this.CustomerCode,
            DepartmentId        = this.DepartmentId,
            DepartmentCode      = this.DepartmentCode,
            StaffId             = this.StaffId,
            StaffCode           = this.StaffCode,
            BillingCategoryId   = this.BillingCategoryId,
            BillingCategoryCode = this.BillingCategoryCode,
            InputType           = (int)Constants.BillingInputType.Importer, /* 1 : フリーインポーター  */
            BilledAt            = this.BilledAt,
            ClosingAt           = this.ClosingAt,
            SalesAt             = this.SalesAt,
            DueAt               = this.DueAt,
            BillingAmount       = this.BillingAmount,
            TaxAmount           = this.TaxAmount,
            RemainAmount        = this.BillingAmount,
            Approved            = 1,
            CollectCategoryId   = this.CollectCategoryId,
            CollectCategoryCode = this.CollectCategoryCode,
            DebitAccountTitleId = DebitAccountTitleId == 0 ? (int?)null : this.DebitAccountTitleId,
            InvoiceCode         = this.InvoiceCode ?? string.Empty,
            TaxClassId          = this.TaxClassId,
            Note1               = this.Note1 ?? string.Empty,
            Note2               = this.Note2 ?? string.Empty,
            Note3               = this.Note3 ?? string.Empty,
            Note4               = this.Note4 ?? string.Empty,
            Note5               = this.Note5 ?? string.Empty,
            Note6               = this.Note6 ?? string.Empty,
            Note7               = this.Note7 ?? string.Empty,
            Note8               = this.Note8 ?? string.Empty,
            Price               = this.Price,
            ScheduledPaymentKey = string.Empty,
            CreateBy            = loginUserId,
            UpdateBy            = loginUserId
        };

        public Customer ConvertToCustomer(List<ImporterSettingDetail> details, int loginUserId, Category collectCategory) => new Customer
        {
            CompanyId               = this.CompanyId,
            Code                    = this.CustomerCode,
            Name                    = this.CustomerName,
            Kana                    = this.CustomerKana,
            CollationKey            = this.CollationKey,
            ExclusiveBankCode       = this.ExclusiveBankCode ?? string.Empty,
            ExclusiveBranchCode     = this.ExclusiveBranchCode ?? string.Empty,
            ExclusiveAccountNumber  = this.ExclusiveVirtualBranchCode + this.ExclusiveAccountNumber,
            CreditLimit             = 0M,
            ClosingDay              = int.Parse(details.FirstOrDefault(c => c.Sequence == (int)Fields.ClosingAt).FixedValue),
            CollectCategoryId       = this.CollectCategoryId,
            CollectCategoryCode     = this.CollectCategoryCode,
            CollectOffsetMonth      = int.Parse(details.FirstOrDefault(c => c.Sequence == (int)Fields.DueAt).FixedValue.Substring(0, 1)),
            CollectOffsetDay        = int.Parse(details.FirstOrDefault(c => c.Sequence == (int)Fields.DueAt).FixedValue.Substring(1, 2)),
            StaffId                 = this.StaffId,
            StaffCode               = this.StaffCode,
            IsParent                = 0,
            SightOfBill             = collectCategory?.UseLimitDate == 1 ? 90 : (int?)null,
            ExclusiveAccountTypeId  = null,
            ShareTransferFee        = 0,
            UseFeeTolerance         = 0,
            UseFeeLearning          = 0,
            UseKanaLearning         = 1,
            HolidayFlag             = 0,
            CreateBy                = loginUserId,
            UpdateBy                = loginUserId
        };

    }
    public static class BillingImportExtension
    {
        public static IEnumerable<BillingImport> ToLookupCustomerCode(this IEnumerable<BillingImport> billings)
            => billings.Where(x => x.AutoCreationCustomerFlag == 1)
                .ToLookup(x => x.CustomerCode)
                .Select(x => x.First());

        public static IEnumerable<Customer> ConvertToCustomers(this IEnumerable<BillingImport> billings,
            List<ImporterSettingDetail> details,
            int loginUserId,
            Dictionary<string, Category> collectCategoryDictionary)
        {
            return billings.ToLookupCustomerCode()
                .Select(x => x.ConvertToCustomer(details,
                    loginUserId,
                    (collectCategoryDictionary?.ContainsKey(x.CollectCategoryCode) ?? false) ? collectCategoryDictionary[x.CollectCategoryCode] : null
                )).ToArray();
        }
    }
    [DataContract]
    public class BillingImportResult : IProcessResult
    {
        [DataMember] public ProcessResult ProcessResult { get; set; }
    }
}
