import {Billing} from './billing.model';
import {ProcessResult} from './process-result.model';
export class BillingsResult {
    public processResult: ProcessResult;
    public billings: Billing[];
}

