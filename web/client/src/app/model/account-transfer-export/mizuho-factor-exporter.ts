import { ExporterBase } from 'src/app/model/account-transfer-export/exporter-base'
import { AccountTransferDetail } from 'src/app/model/account-transfer-detail.model'

export class MizuhoFactorExporter extends ExporterBase
{
    public GetHeaderRecord():string{
        return this.JoinFixedData([
            this.GetStrField(1,  this.DivisionHeader),
            this.GetStrField(3,  911),
            this.GetNmbField(10, this.PaymentAgency.consigneeCode),
            this.GetStrField(40, this.Company.kana),
            this.GetStrField(4,  this.GetMMdd(this.DueAt)),
            this.GetStrField(62, this.Dummy())]);
    }

    public GetDataRecord(detail:AccountTransferDetail):string{
        return this.JoinFixedData([
            this.GetStrField(1,  this.DivisionData),
            this.GetNmbField(4,  detail.transferBankCode),
            this.GetStrField(15, detail.transferBankName),
            this.GetNmbField(3,  detail.transferBranchCode),
            this.GetStrField(15, detail.transferBranchName),
            this.GetStrField(2,  "00"),
            this.GetStrField(2,  this.Dummy()),
            this.GetStrField(1,  detail.transferAccountTypeId),
            this.GetNmbField(7,  detail.transferAccountNumber),
            this.GetStrField(30, detail.transferAccountName),
            this.GetNmbField(10, parseInt(detail.billingAmount.toString())),
            this.GetStrField(1,  detail.transferNewCode),
            this.GetStrField(20, detail.transferCustomerCode),
            this.GetStrField(1,  0),
            this.GetStrField(8,  this.Dummy())]);
    }

    public GetTrailerRecord():string{
        return this.JoinFixedData([
            this.GetStrField(1,  this.DivisionTrailer),
            this.GetNmbField(6,  this.TotalCount),
            this.GetNmbField(12, parseInt(this.TotalAmount.toString())),
            this.GetNmbField(6,  0),
            this.GetNmbField(12, 0),
            this.GetNmbField(6,  0),
            this.GetNmbField(12, 0),
            this.GetStrField(65, this.Dummy())]);
    }

    public GetEndRecord():string{
        return this.JoinFixedData([
            this.GetStrField(1,   this.DivisionEnd),
            this.GetStrField(119, this.Dummy())]);
    }
}
