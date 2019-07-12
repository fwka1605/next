import {KanaHistory} from './kana-history.model';
import {ProcessResult} from './process-result.model';
export class KanaHistorysResult {
    private processResult: ProcessResult;
    public get ProcessResult() : ProcessResult {
        return this.processResult;
    }
    public set ProcessResult(processResult: ProcessResult) {
        this.processResult = processResult;
    }
    private kanaHistorys: KanaHistory[];
    public get KanaHistorys() : KanaHistory[] {
        return this.kanaHistorys;
    }
    public set KanaHistorys(kanaHistorys: KanaHistory[]) {
        this.kanaHistorys = kanaHistorys;
    }
}

