import { PaymentAgency } from 'src/app/model/payment-agency.model';
import { Company } from 'src/app/model/company.model';
import { StringUtil } from "src/app/common/util/string-util";
import { AccountTransferDetail } from 'src/app/model/account-transfer-detail.model';
import { Observable } from 'rxjs';
import { FixedField } from 'src/app/model/account-transfer-export/fixed-field';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';

export class ExporterBase{

    public DivisionHeader:number = 1;
    public DivisionData:number = 2;
    public DivisionTrailer:number = 8;
    public DivisionEnd:number = 9;

    public PaymentAgency:PaymentAgency;
    public Company:Company;
    public DueAt:string;
    public DueAt2nd:string;

    public TotalCount:number=0;
    public TotalAmount:number=0;

    public Export(source:AccountTransferDetail[]): string{

        let exportData:Array<any> = [];
        exportData.push(this.GetHeaderRecord());

        source.forEach(detail =>{
            this.TotalCount++;
            this.TotalAmount += detail.billingAmount;
            exportData.push(this.GetDataRecord(detail));
        });

        exportData.push(this.GetTrailerRecord());
        exportData.push(this.GetEndRecord());

        return exportData.join("");
    }

    public GetHeaderRecord():string{
        return "";
    }

    public GetDataRecord(detail:AccountTransferDetail):string{
        return "";
    }

    public GetTrailerRecord():string{
        return "";
    }

    public GetEndRecord():string{
        return "";
    }

    protected JoinData(dataItems: Array<any>):string{
        return dataItems.join(",") + LINE_FEED_CODE;
    }

    protected JoinFixedData(dataItems: Array<any>):string{
        return dataItems.join("") + LINE_FEED_CODE;
    }

    protected GetStrField(length:number, value:any, padding:string = ' ', rightPadding:boolean = true):string{
        let fixedField:FixedField = new FixedField(length, value, padding, rightPadding);
        return fixedField.ToString();
    }

    protected GetNmbField(length:number, value:any):string{
        let fixedField: FixedField = new FixedField(length, value, '0', false);
        return fixedField.ToString();
    }

    protected Dummy(length:number = 0):string{
        return length == 0 ? "" : String(' ').repeat(length);
    }

    protected GetMMdd(dueDate:string):string{
        let tDate = new Date(dueDate);
        let month:string = ""+ (tDate.getMonth()+1);
        let day:string = ""+ tDate.getDate();
        return StringUtil.setPaddingFrontZero(month, 2, true) + StringUtil.setPaddingFrontZero(day, 2, true);
    }


}