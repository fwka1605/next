import {MatchingBillingDiscount} from './matching-billing-discount.model';
import {ProcessResult} from './process-result.model';
export class MatchingBillingDiscountsResult {
    public processResult: ProcessResult;
    public matchingHistorys: MatchingBillingDiscount[];
}

