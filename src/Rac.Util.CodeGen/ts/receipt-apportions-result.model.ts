import {ReceiptApportion} from './receipt-apportion.model';
import {ProcessResult} from './process-result.model';
export class ReceiptApportionsResult {
    public processResult: ProcessResult;
    public receiptApportion: ReceiptApportion[];
    public exceptionMessage: string | null;
}

