import {BillingDiscount} from './billing-discount.model';
import {ProcessResult} from './process-result.model';
export class BillingDiscountsResult {
    public processResult: ProcessResult;
    public billingDiscounts: BillingDiscount[];
}

