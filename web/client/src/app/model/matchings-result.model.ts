import {Matching} from './matching.model';
import {ProcessResult} from './process-result.model';
export class MatchingsResult {
    public processResult: ProcessResult;
    public remainType: number;
    public matchings: Matching[];
    public billingRemainTotal: number;
    public receiptRemainTotal: number;
    public billingDiscountTotal: number;
}

