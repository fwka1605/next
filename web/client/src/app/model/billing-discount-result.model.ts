import {BillingDiscount} from './billing-discount.model';
import {ProcessResult} from './process-result.model';
export class BillingDiscountResult {
    public processResult: ProcessResult;
    public billingDiscount: BillingDiscount[];
}

