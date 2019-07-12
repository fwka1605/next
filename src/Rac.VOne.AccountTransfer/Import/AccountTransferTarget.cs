using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rac.VOne.AccountTransfer.Import
{
    /// <summary>
    /// 照合対象。
    /// 単一BillingAmountと照合する場合と同一CustomerIdで合算したBillingAmountと照合する場合があるため、
    /// 当クラスで透過的に扱えるようにする。
    /// </summary>
    public class AccountTransferTarget
    {
        //public Customer Customer { get; private set; }
        public IEnumerable<Billing> Billings { get; private set; }
        public decimal TotalBillingAmount { get; private set; } // BillingAmountの合算値
        public string TransferBankCode { get; set; }
        public string TransferBranchCode { get; set; }
        public int TransferAccountTypeId { get; set; }
        public string TransferAccountNumber { get; set; }
        public string TransferCustomerCode { get; set; }

        public AccountTransferTarget(IEnumerable<Billing> billings)
        {
            Billings = billings ?? throw new ArgumentNullException(nameof(billings));

            var billingCount = billings.Count();
            if (billingCount == 0)
            {
                throw new ArgumentException("billingCount == 0");
            }
            TotalBillingAmount = billings.Sum(b => b.BillingAmount);
        }
    }
}
