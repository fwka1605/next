using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rac.VOne.Web.Models;


namespace Rac.VOne.AccountTransfer.Export
{
    public class InternetJPBankExporter : ExporterBase
    {
        public DateTime DueAt2nd { get; set; }

        public override string GetHeaderRecord() => Join(
             GetStrField( 1, DivisionHeader),
             GetStrField( 2, PaymentAgency.ContractCode),
             GetStrField( 1, Dummy()),
             GetNmbField(10, PaymentAgency.ConsigneeCode),
             GetStrField(40, Company.Kana),
             GetStrField( 4, DueAt.ToString("MMdd")),
             GetNmbField( 4, PaymentAgency.BankCode),
             GetStrField(15, PaymentAgency.BankName),
             GetNmbField( 3, PaymentAgency.BranchCode),
             GetStrField(16, Dummy()),
             GetNmbField( 7, PaymentAgency.AccountNumber),
             GetStrField( 4, DueAt2nd.ToString("MMdd")),
             GetStrField( 1, Dummy()),
             GetStrField(12, Dummy()));

        public override string GetDataRecord(AccountTransferDetail detail) => Join(
            GetStrField( 1, DivisionData),
            GetNmbField( 4, detail.TransferBankCode),
            GetStrField(15, detail.TransferBankName),
            GetNmbField( 3, detail.TransferBranchCode),
            GetStrField(20, Dummy()),
            GetNmbField( 7, detail.TransferAccountNumber),
            GetStrField(30, detail.TransferAccountName),
            GetNmbField(10, decimal.Truncate(detail.BillingAmount)),
            GetStrField( 1, detail.TransferNewCode),
            GetStrField(20, detail.TransferCustomerCode),
            GetStrField( 1, 0),
            GetStrField( 4, Dummy()),
            GetStrField( 2, Dummy()),
            GetStrField( 2, Dummy()));

        public override string GetTrailerRecord() => Join(
            GetStrField( 1, DivisionTrailer),
            GetNmbField( 6, TotalCount),
            GetNmbField(12, decimal.Truncate(TotalAmount)),
            GetNmbField( 6, Dummy()),
            GetNmbField(12, Dummy()),
            GetNmbField( 6, Dummy()),
            GetNmbField(12, Dummy()),
            GetNmbField( 6, Dummy()),
            GetNmbField( 6, Dummy()),
            GetNmbField( 6, Dummy()),
            GetNmbField( 6, Dummy()),
            GetNmbField(12, Dummy()),
            GetStrField(29, Dummy()));

        public override string GetEndRecord() => Join(
            GetStrField(  1, DivisionEnd),
            GetStrField(119, Dummy()));
    }
}
