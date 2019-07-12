import { ExporterBase } from 'src/app/model/account-transfer-export/exporter-base'
import { AccountTransferDetail } from 'src/app/model/account-transfer-detail.model'

export class MUFJFactorExporter extends ExporterBase
{
    public GetHeaderRecord():string{
        return this.JoinData([
            this.DivisionHeader,
            91,
            0,
            this.PaymentAgency.consigneeCode,
            this.Company.kana,
            this.GetMMdd(this.DueAt),
            this.PaymentAgency.bankCode,
            this.PaymentAgency.bankName,
            this.PaymentAgency.branchCode,
            this.PaymentAgency.branchName,
            this.PaymentAgency.accountTypeId,
            this.PaymentAgency.accountNumber,
            this.Dummy()]);
    }

    public GetDataRecord(detail:AccountTransferDetail):string{
        return this.JoinData([
            this.DivisionData,
            detail.transferBankCode,
            detail.transferBankName,
            detail.transferBranchCode,
            detail.transferBranchName,
            this.Dummy(),
            detail.transferAccountTypeId,
            detail.transferAccountNumber,
            detail.transferAccountName,
            parseInt(detail.billingAmount.toString()),
            detail.transferNewCode,
            this.PaymentAgency.consigneeCode,
            detail.transferCustomerCode,
            0,
            this.Dummy()]);
    }

    public GetTrailerRecord():string{
        return this.JoinData([
            this.DivisionTrailer,
            this.TotalCount,
            parseInt(this.TotalAmount.toString()),
            0,
            0,
            0,
            0,
            this.Dummy()]);
    }

    public GetEndRecord():string{
        return this.JoinData([
            this.DivisionEnd,
            this.Dummy()]);
    }
}
