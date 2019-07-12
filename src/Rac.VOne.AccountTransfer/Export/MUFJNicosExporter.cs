using Rac.VOne.Web.Models;

namespace Rac.VOne.AccountTransfer.Export
{
    public class MUFJNicosExporter : ExporterBase
    {
        public override string GetHeaderRecord() => Join(
            "不能理由サイン",
            "顧客番号",
            "引落依頼金額",
            "銀行コード",
            "支店コード",
            "預金種別",
            "口座番号",
            "口座名義人",
            "新規サイン",
            "制度／商品コード",
            "口座合算有無",
            "会員番号",
            "送付サイン");

        public override string GetDataRecord(AccountTransferDetail detail) => Join(
            Dummy(),
            detail.TransferCustomerCode,
            decimal.Truncate(detail.BillingAmount),
            detail.TransferBankCode,
            detail.TransferBranchCode,
            detail.TransferAccountTypeId,
            detail.TransferAccountNumber,
            detail.TransferAccountName,
            detail.TransferNewCode,
            Dummy(),
            0,
            Dummy(),
            Dummy());

        public override string GetTrailerRecord() => Join(
            Dummy(),
            TotalCount,
            decimal.Truncate(TotalAmount),
            0,
            0,
            0,
            0,
            Dummy(),
            Dummy(),
            Dummy(),
            Dummy(),
            Dummy(),
            Dummy());
    }

}
