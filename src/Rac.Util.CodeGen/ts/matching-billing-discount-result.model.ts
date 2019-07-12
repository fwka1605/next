import {MatchingBillingDiscount} from './matching-billing-discount.model';
import {ProcessResult} from './process-result.model';
export class MatchingBillingDiscountResult {
    public processResult: ProcessResult;
    public matchingHistory: MatchingBillingDiscount[];
}

