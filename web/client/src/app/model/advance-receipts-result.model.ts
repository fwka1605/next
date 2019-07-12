import {Receipt} from './receipt.model';
import {ProcessResult} from './process-result.model';
export class AdvanceReceiptsResult {
    public processResult: ProcessResult;
    public originalReceipt: Receipt;
    public advanceReceipts: Receipt[];
}

