import {MatchingReceipt} from './matching-receipt.model';
import {ProcessResult} from './process-result.model';
export class MatchingReceiptsResult {
    public processResult: ProcessResult;
    public get ProcessResult() : ProcessResult {
        return this.processResult;
    }
    public set ProcessResult(processResult: ProcessResult) {
        this.processResult = processResult;
    }
    public matchingReceipts: MatchingReceipt[];
    public get MatchingReceipts() : MatchingReceipt[] {
        return this.matchingReceipts;
    }
    public set MatchingReceipts(matchingReceipts: MatchingReceipt[]) {
        this.matchingReceipts = matchingReceipts;
    }
}

