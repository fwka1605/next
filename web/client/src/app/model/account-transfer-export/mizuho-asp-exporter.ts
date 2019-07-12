import { ExporterBase } from 'src/app/model/account-transfer-export/exporter-base'
import { AccountTransferDetail } from 'src/app/model/account-transfer-detail.model'

export class MizuhoAspExporter extends ExporterBase
{
    public GetHeaderRecord():string{
        return this.JoinData([
            this.DivisionHeader,
            911,
            this.PaymentAgency.consigneeCode, /* 右6桁 */
            "0000",
            this.Company.kana,
            this.GetMMdd(this.DueAt),
            this.Dummy()]);
    }

    public GetDataRecord(detail:AccountTransferDetail):string{
        return this.JoinData([
            this.DivisionData,
            detail.transferBankCode,
            detail.transferBankName,
            detail.transferBranchCode,
            detail.transferBranchName,
            0,
            detail.transferAccountTypeId,
            detail.transferAccountNumber,
            detail.transferAccountName,
            parseInt(detail.billingAmount.toString()),
            detail.transferNewCode,
            this.PaymentAgency.consigneeCode, /* 右6桁 */
            detail.transferCustomerCode, /* 左14桁 */
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
