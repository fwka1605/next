import {MatchingReceipt} from './matching-receipt.model';
import {ProcessResult} from './process-result.model';
export class MatchingReceiptResult {
    public processResult: ProcessResult;
    public get ProcessResult() : ProcessResult {
        return this.processResult;
    }
    public set ProcessResult(processResult: ProcessResult) {
        this.processResult = processResult;
    }
    public matchingReceipt: MatchingReceipt[];
    public get MatchingReceipt() : MatchingReceipt[] {
        return this.matchingReceipt;
    }
    public set MatchingReceipt(matchingReceipt: MatchingReceipt[]) {
        this.matchingReceipt = matchingReceipt;
    }
}

