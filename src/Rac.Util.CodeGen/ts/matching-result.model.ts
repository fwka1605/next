import {Receipt} from './receipt.model';
import {AdvanceReceived} from './advance-received.model';
import {MatchingErrorType} from './matching-error-type.model';
import {Matching} from './matching.model';
import {ProcessResult} from './process-result.model';
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

