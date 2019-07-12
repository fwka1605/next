import {ReceiptApportion} from './receipt-apportion.model';
import {ReceiptHeader} from './receipt-header.model';
export class ReceiptApportionForReportSource {
    public companyId: number;
    public header: ReceiptHeader;
    public apportions: ReceiptApportion[];
    public categoryName: string | null;
}

