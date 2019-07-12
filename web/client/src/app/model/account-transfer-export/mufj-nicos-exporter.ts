import { ExporterBase } from 'src/app/model/account-transfer-export/exporter-base'
import { AccountTransferDetail } from 'src/app/model/account-transfer-detail.model'

export class MUFJNicosExporter extends ExporterBase
{
    public GetHeaderRecord():string{
        return this.JoinData([
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
            "送付サイン"]);
    }

    public GetDataRecord(detail:AccountTransferDetail):string{
        return this.JoinData([
            this.Dummy(),
            detail.transferCustomerCode,            
            parseInt(detail.billingAmount.toString()),
            detail.transferBankCode,
            detail.transferBranchCode,
            detail.transferAccountTypeId,
            detail.transferAccountNumber,
            detail.transferAccountName,
            detail.transferNewCode,
            this.Dummy(),
            0,
            this.Dummy(),
            this.Dummy()]);
    }  

    public GetTrailerRecord():string{
        return this.JoinData([
            this.Dummy(),
            this.TotalCount,
            parseInt(this.TotalAmount.toString()),
            0,
            0,
            0,
            0,
            this.Dummy(),
            this.Dummy(),
            this.Dummy(),
            this.Dummy(),
            this.Dummy(),
            this.Dummy()]);
    }
}
