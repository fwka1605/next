import {MatchingBilling} from './matching-billing.model';
import {ProcessResult} from './process-result.model';
export class MatchingBillingsResult {
    public processResult: ProcessResult;
    public get ProcessResult() : ProcessResult {
        return this.processResult;
    }
    public set ProcessResult(processResult: ProcessResult) {
        this.processResult = processResult;
    }
    public matchingBillings: MatchingBilling[];
    public get MatchingBillings() : MatchingBilling[] {
        return this.matchingBillings;
    }
    public set MatchingBillings(matchingBillings: MatchingBilling[]) {
        this.matchingBillings = matchingBillings;
    }
}

