using Rac.VOne.AccountTransfer.Import.Reader;
using Rac.VOne.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Rac.VOne.Common.Constants;

namespace Rac.VOne.AccountTransfer.Import
{
    public class Helper
    {
        /// <summary>照合対象請求データを照合単位で小分けにする。</summary>
        /// <param name="billings"></param>
        /// <param name="aggregateBillings">同一得意先で纏めた請求データと照合するか否か</param>
        /// <returns></returns>
        public IEnumerable<AccountTransferTarget> ConvertToAccountTransferTargets(IEnumerable<Billing> billings, Dictionary<int, Customer> customerDictionary, bool aggregateBillings)
        {
            Func<IEnumerable<Billing>, IEnumerable<AccountTransferTarget>> converter =
                source => source.Select(x => {
                    var customer = customerDictionary[x.CustomerId];
                    return new AccountTransferTarget(new[] { x }) {
                        TransferBankCode        = customer.TransferBankCode,
                        TransferBranchCode      = customer.TransferBranchCode,
                        TransferAccountTypeId   = customer.TransferAccountTypeId ?? 0,
                        TransferAccountNumber   = customer.TransferAccountNumber,
                        TransferCustomerCode    = customer.TransferCustomerCode,
                    };
                });
            if (aggregateBillings) converter =
                source => source.Select(x => {
                    var customer = customerDictionary[x.CustomerId];
                    return new {
                        billing                 =   x,
                                                    customer.TransferBankCode,
                                                    customer.TransferBranchCode,
                        TransferAccountTypeId   =   customer.TransferAccountTypeId ?? 0,
                                                    customer.TransferAccountNumber,
                                                    customer.TransferCustomerCode,
                    };
                }).ToLookup(x => new {
                    x.TransferBankCode,
                    x.TransferBranchCode,
                    x.TransferAccountTypeId,
                    x.TransferAccountNumber,
                    x.TransferCustomerCode
                }).Select(g => new AccountTransferTarget(g.Select(x => x.billing).ToArray()) {
                    TransferBankCode        = g.Key.TransferBankCode,
                    TransferBranchCode      = g.Key.TransferBranchCode,
                    TransferAccountTypeId   = g.Key.TransferAccountTypeId,
                    TransferAccountNumber   = g.Key.TransferAccountNumber,
                    TransferCustomerCode    = g.Key.TransferCustomerCode,
                });

            return converter(billings);
        }

        /// <summary>CompanyId, PaymentAgencyId, TransferDate(DueAt) から請求データを取得 同期処理</summary>
        public Func<int, int, DateTime, List<Billing>> GetBillings { get; set; }
        /// <summary>CompanyId, PaymentAgencyId, TransferDate(DueAt) から請求データを取得 非同期処理</summary>
        public Func<int, int, DateTime, Task<List<Billing>>> GetBillingsAsync { get; set; }

        /// <summary>CustomerId の配列から 得意先の Dictionary を取得 同期処理</summary>
        public Func<int[], Dictionary<int, Customer>> GetCustomers { get; set; }
        /// <summary>CustomerId の配列から 得意先の Dictionary を取得 非同期処理</summary>
        public Func<int[], Task<Dictionary<int, Customer>>> GetCustomersAsync { get; set; }


        public IReader CreateReader(AccountTransferFileFormatId formatId)
        {
            IReader reader = null;
            switch (formatId)
            {
                case AccountTransferFileFormatId.ZenginCsv:                 reader = new ZenginDsvFileReader();                 break;
                case AccountTransferFileFormatId.ZenginFixed:               reader = new ZenginFixedFileReader();               break;
                case AccountTransferFileFormatId.MizuhoFactorWebFixed:      reader = new MizuhoFactorWebFixedFileReader();      break;
                case AccountTransferFileFormatId.MitsubishiUfjFactorCsv:    reader = new MitsubishiUfjFactorDsvFileReader();    break;
                case AccountTransferFileFormatId.SMBCFixed:                 reader = new SmbcFixedFileReader();                 break;
                case AccountTransferFileFormatId.MitsubishiUfjNicosCsv:     reader = new MitsubishiUfjNicosDsvFileReader();     break;
                case AccountTransferFileFormatId.MizuhoFactorAspCsv:        reader = new MizuhoFactorAspDsvFileReader();        break;
                case AccountTransferFileFormatId.RicohLeaseCollectCsv:      reader = new RicohLeaseCollectDsvFileReader();      break;
                case AccountTransferFileFormatId.InternetJPBankFixed:       reader = new InternetJPBankFixedFileReader();       break;
                case AccountTransferFileFormatId.RisonaNetCsv:              reader = new RisonaNetDsvFileReader();              break;
            }
            reader.Helper = this;
            return reader;
        }

        public AccountTransferSource CreateTransferSource(IEnumerable<Billing> billings,
            int transferResultCode, decimal transferAmount, ConcreteRecord.IAccountTransferInformation information)
        {
            var source = new AccountTransferSource(billings, transferResultCode, transferAmount) {
                TransferBankName        = information?.TransferBankName,
                TransferBranchName      = information?.TransferBranchName,
                TransferCustomerCode    = information?.TransferCustomerCode,
                TransferAccountName     = information?.TransferAccountName,
            };
            return source;
        }
    }
}
