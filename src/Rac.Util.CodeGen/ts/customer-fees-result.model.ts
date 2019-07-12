import {CustomerFee} from './customer-fee.model';
import {ProcessResult} from './process-result.model';
export class CustomerFeesResult {
    public processResult: ProcessResult;
    public customerFees: CustomerFee[];
    public customerFeePrint: CustomerFee[];
}

