using Rac.VOne.Web.Models;

namespace Rac.VOne.AccountTransfer.Export
{
    public class MizuhoAspExporter : ExporterBase
    {
        public override string GetHeaderRecord() => Join(
            DivisionHeader,
            911,
            PaymentAgency.ConsigneeCode, /* 右6桁 */
            "0000",
            Company.Kana,
            DueAt.ToString("MMdd"),
            Dummy());

        public override string GetDataRecord(AccountTransferDetail detail) => Join(
            DivisionData,
            detail.TransferBankCode,
            detail.TransferBankName,
            detail.TransferBranchCode,
            detail.TransferBranchName,
            0,
            detail.TransferAccountTypeId,
            detail.TransferAccountNumber,
            detail.TransferAccountName,
            decimal.Truncate(detail.BillingAmount),
            detail.TransferNewCode,
            PaymentAgency.ConsigneeCode, /* 右6桁 */
            detail.TransferCustomerCode, /* 左14桁 */
            0,
            Dummy());

        public override string GetTrailerRecord() => Join(
            DivisionTrailer,
            TotalCount,
            decimal.Truncate(TotalAmount),
            0,
            0,
            0,
            0,
            Dummy());

        public override string GetEndRecord() => Join(
            DivisionEnd,
            Dummy());
    }
}
