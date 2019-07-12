using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;
using Rac.VOne.Common.DataHandling;

namespace Rac.VOne.Web.Models.MFModels
{

    public class billing
    {
        #region メンバー
        public string id { get; set; }
        public string partner_id { get; set; }
        public string partner_name { get; set; }

        public string department_id { get; set; }
        public DateTime billing_date { get; set; }
        public DateTime sales_date { get; set; }
        public DateTime due_date { get; set; }
        public decimal total_price { get; set; }
        public string billing_number { get; set; }
        public string title { get; set; }
        public string memo { get; set; }
        public string payment_condition { get; set; }
        public string note { get; set; }
        public List<string> tags { get; set; }
        public status status { get; set; }
        public bool Selected { get; set; } = true;
        public Customer Customer { get; set; }
        public string CustomerCode { get; set; }
        private const string dummyKana = "DUMMY";
        #endregion

        #region メソッド
        public Billing CreateVOneBilling(
            int departmentId,
            int companyId,
            int loginUserId,
            int currencyId ,
            WebApiMFExtractSetting extractSetting,
            department department)
        {
            var billing = new Billing {
                CompanyId           = companyId,
                CurrencyId          = currencyId,
                BillingCategoryId   = extractSetting.BillingCategoryId.Value,
                InputType           = 1,
                BilledAt            = billing_date,
                ClosingAt           = billing_date,
                SalesAt             = sales_date,
                DueAt               = due_date,
                BillingAmount       = total_price,
                RemainAmount        = total_price,
                Approved            = 1,
                InvoiceCode         = GetTriming(billing_number, 100),
                TaxClassId          = (int)TaxClassId.NotCovered,
                Note1               = GetTriming($"No.{billing_number} {partner_name}", 100),
                Note2               = GetTriming(memo, 100),
                Note3               = department == null
                    ? string.Empty
                    : GetTriming(department.name,100),
                Note4               = department == null
                ? string.Empty
                : GetTriming($"{department.person_name} {department.email}",100),

                Note5               = GetTriming(string.Join(" ", tags), 100),
                Note6               = GetTriming(partner_name, 100),
                Note7               = string.Empty,
                Note8               = string.Empty,
                Price               = 0M,
                CreateBy            = loginUserId,
                UpdateBy            = loginUserId,

                //便宜的にCustomerKanaにidを入れる。MFBillingテーブルへデータ登録時、利用するため
                CustomerKana = id,
            };

            if (Customer == null)
            {
                billing.CustomerCode = CustomerCode;
            }
            else
            {
                billing.CustomerId = Customer.Id;
                billing.DepartmentId = departmentId;
                billing.StaffId = Customer.StaffId;
                billing.CollectCategoryId = Customer.CollectCategoryId;
            }

            return billing;
        }

        public Customer BuildNewCustomer(WebApiMFExtractSetting setting,
            partner partner,
            int companyId,
            int loginUserId,
            IEnumerable<string> legalPersonalities)
        {
            var department = partner == null
                ? null
                : partner.departments.FirstOrDefault();

            var customer = new Customer {
                CompanyId = companyId,
                Code = CustomerCode,
                Name = GetTriming(partner_name, 140),
                Kana = partner == null
                ? dummyKana
                : GetTriming(GetCustomerNameKana(partner.name_kana, legalPersonalities), 140),

                PostalCode = department == null
                ? string.Empty
                : GetTriming(department.zip, 10),

                Address1 = department == null
                ? string.Empty
                : GetTriming(department.address1, 40),

                Address2 = department == null
                ? string.Empty
                : GetTriming(department.address2, 40),

                Tel = department == null
                ? string.Empty
                : GetTriming(department.tel, 20),

                CustomerStaffName = partner == null
                ? string.Empty
                : GetTriming(partner.departments.FirstOrDefault().person_name, 40),

                Note = partner == null
                ? string.Empty
                : GetTriming(partner.memo, 100),

                ClosingDay = setting.ClosingDay.Value,
                CollectCategoryId = setting.CollectCategoryId.Value,
                CollectOffsetMonth = setting.CollectOffsetMonth.Value,
                CollectOffsetDay = setting.CollectOffsetDay.Value,
                StaffId = setting.StaffId.Value,
                ReceiveAccountId1 = 1,
                ReceiveAccountId2 = 1,
                ReceiveAccountId3 = 1,
                UseKanaLearning = 1,
                CollationKey = department == null
                ? string.Empty
                : GetTriming(department.tel, 48),

                CreateBy = loginUserId,
                UpdateBy = loginUserId,
            };
            return customer;
        }

        private string GetTriming(string value, int length)
        {
           if (string.IsNullOrWhiteSpace(value)) return string.Empty;
           return value.Length > length ? value.Substring(0, length) : value;
        }

        private string GetCustomerNameKana(string value, IEnumerable<string> legalPersonalities)
        {
            if (string.IsNullOrWhiteSpace(value)) return dummyKana;

            value = EbDataHelper.ConvertToHankakuKatakana(value);
            value = EbDataHelper.ConvertProhibitCharacter(value);
            value = EbDataHelper.RemoveProhibitCharacter(value);
            value = RemoveChars(value);
            value = EbDataHelper.RemoveLegalPersonality(value, legalPersonalities);
            if (string.IsNullOrWhiteSpace(value)) value = dummyKana;
            return value;
        }
        private string RemoveChars(string value)
        {
            return new string(value.Where(x => EbDataHelper.IsValidEBChars(x.ToString())).ToArray());
        }

        public string GetPosting
            => status.posting;

        public string GetEmail
            => status.email;

        public string GetDownload
            => status.download;

        public string GetPeyment
            => status.payment;

        public string GetCustomerName
            => Customer == null ? partner_name : Customer.Name;
        #endregion
    }
}
