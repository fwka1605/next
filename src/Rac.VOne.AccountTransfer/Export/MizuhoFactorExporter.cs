using Rac.VOne.Web.Models;

namespace Rac.VOne.AccountTransfer.Export
{
    public class MizuhoFactorExporter : ExporterBase
    {
        public override string GetHeaderRecord() => Join(
            GetStrField( 1, DivisionHeader),
            GetStrField( 3, 911),
            GetNmbField(10, PaymentAgency.ConsigneeCode),
            GetStrField(40, Company.Kana),
            GetStrField( 4, DueAt.ToString("MMdd")),
            GetStrField(62, Dummy()));

        public override string GetDataRecord(AccountTransferDetail detail) => Join(
            GetStrField( 1, DivisionData),
            GetNmbField( 4, detail.TransferBankCode),
            GetStrField(15, detail.TransferBankName),
            GetNmbField( 3, detail.TransferBranchCode),
            GetStrField(15, detail.TransferBranchName),
            GetStrField( 2, "00"),
            GetStrField( 2, Dummy()),
            GetStrField( 1, detail.TransferAccountTypeId),
            GetNmbField( 7, detail.TransferAccountNumber),
            GetStrField(30, detail.TransferAccountName),
            GetNmbField(10, decimal.Truncate(detail.BillingAmount)),
            GetStrField( 1, detail.TransferNewCode),
            GetStrField(20, detail.TransferCustomerCode),
            GetStrField( 1, 0),
            GetStrField( 8, Dummy()));

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
