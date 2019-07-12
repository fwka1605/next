import {Receipt} from './receipt.model';
import {AdvanceReceived} from './advance-received.model';
import {Matching} from './matching.model';
import {ProcessResult} from './process-result.model';
import { MatchingErrorType } from '../common/const/matching.const';
export class MatchingResult {
    public processResult: ProcessResult;
    public matching: Matching;
    public matchingErrorType: MatchingErrorType;
    public matchings: Matching[];
    public advanceReceiveds: AdvanceReceived[];
    public deleteReceipts: Receipt[];
    public errorIndex: number | null;
    public nettingReceipts: Receipt[];
}

