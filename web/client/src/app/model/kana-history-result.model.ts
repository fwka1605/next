import {KanaHistory} from './kana-history.model';
import {ProcessResult} from './process-result.model';
export class KanaHistoryResult {
    private processResult: ProcessResult;
    public get ProcessResult() : ProcessResult {
        return this.processResult;
    }
    public set ProcessResult(processResult: ProcessResult) {
        this.processResult = processResult;
    }
    private kanaHistory: KanaHistory;
    public get KanaHistory() : KanaHistory {
        return this.kanaHistory;
    }
    public set KanaHistory(kanaHistory: KanaHistory) {
        this.kanaHistory = kanaHistory;
    }
}

