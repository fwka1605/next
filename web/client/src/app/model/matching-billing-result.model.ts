import {MatchingBilling} from './matching-billing.model';
import {ProcessResult} from './process-result.model';
export class MatchingBillingResult {
    public processResult: ProcessResult;
    public get ProcessResult() : ProcessResult {
        return this.processResult;
    }
    public set ProcessResult(processResult: ProcessResult) {
        this.processResult = processResult;
    }
    public matchingBilling: MatchingBilling;
    public get MatchingBilling() : MatchingBilling {
        return this.matchingBilling;
    }
    public set MatchingBilling(matchingBilling: MatchingBilling) {
        this.matchingBilling = matchingBilling;
    }
}

