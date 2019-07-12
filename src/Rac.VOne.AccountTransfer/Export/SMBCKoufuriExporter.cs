using Rac.VOne.Web.Models;

namespace Rac.VOne.AccountTransfer.Export
{
    public class SMBCKoufuriExporter : ExporterBase
    {
        public override string GetHeaderRecord() => Join(
            GetStrField( 1, DivisionHeader),
            GetStrField( 2, 91),
            GetStrField( 1, 1),
            GetStrField( 2, "00"),
            GetNmbField( 8, PaymentAgency.ConsigneeCode), /* 右8桁切取 */
            GetStrField(40, Company.Kana),
            GetStrField( 4, DueAt.ToString("MMdd")),
            GetNmbField( 4, PaymentAgency.BankCode),
            GetStrField(15, PaymentAgency.BankName),
            GetNmbField( 3, PaymentAgency.BranchCode),
            GetStrField(15, PaymentAgency.BranchName),
            GetStrField( 1, PaymentAgency.AccountTypeId),
            GetNmbField( 7, PaymentAgency.AccountNumber),
            GetStrField(17, Dummy()));
        public override string GetDataRecord(AccountTransferDetail detail) => Join(
            GetStrField( 1, DivisionData),
            GetNmbField( 4, detail.TransferBankCode),
            GetStrField(15, detail.TransferBankName),
            GetNmbField( 3, detail.TransferBranchCode),
            GetStrField(15, detail.TransferBranchName),
            GetStrField( 4, Dummy()),
            GetStrField( 1, detail.TransferAccountTypeId),
            GetNmbField( 7, detail.TransferAccountNumber),
            GetStrField(30, detail.TransferAccountName),
            GetNmbField(10, decimal.Truncate(detail.BillingAmount)),
            GetStrField( 1, detail.TransferNewCode),
            GetNmbField( 8, PaymentAgency.ConsigneeCode), /* 右8桁切取 */
            GetNmbField(12, detail.TransferCustomerCode), /* 左12桁切取 検証実施済  */
            GetStrField( 1, 0),
            GetStrField( 8, Dummy())
            );

        public override string GetTrailerRecord() => Join(
            GetStrField( 1, DivisionTrailer),
            GetNmbField( 6, TotalCount),
            GetNmbField(12, decimal.Truncate(TotalAmount)),
            GetNmbField( 6, 0),
            GetNmbField(12, 0),
            GetNmbField( 6, 0),
            GetNmbField(12, 0),
            GetStrField(65, Dummy()));
        public override string GetEndRecord() => Join(
            GetStrField(  1, DivisionEnd),
            GetStrField(119, Dummy()));
    }
}
