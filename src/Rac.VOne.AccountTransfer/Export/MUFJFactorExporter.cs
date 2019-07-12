using Rac.VOne.Web.Models;

namespace Rac.VOne.AccountTransfer.Export
{
    public class MUFJFactorExporter : ExporterBase
    {
        public override string GetHeaderRecord() => Join(
            DivisionHeader,
            91,
            0,
            PaymentAgency.ConsigneeCode,
            Company.Kana,
            DueAt.ToString("MMdd"),
            PaymentAgency.BankCode,
            PaymentAgency.BankName,
            PaymentAgency.BranchCode,
            PaymentAgency.BranchName,
            PaymentAgency.AccountTypeId,
            PaymentAgency.AccountNumber,
            Dummy());

        public override string GetDataRecord(AccountTransferDetail detail) => Join(
            DivisionData,
            detail.TransferBankCode,
            detail.TransferBankName,
            detail.TransferBranchCode,
            detail.TransferBranchName,
            Dummy(),
            detail.TransferAccountTypeId,
            detail.TransferAccountNumber,
            detail.TransferAccountName,
            decimal.Truncate(detail.BillingAmount),
            detail.TransferNewCode,
            detail.TransferCustomerCode,
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
