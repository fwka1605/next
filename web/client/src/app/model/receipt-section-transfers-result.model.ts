import {Receipt} from './receipt.model';
import {ReceiptSectionTransfer} from './receipt-section-transfer.model';
import {ProcessResult} from './process-result.model';
export class ReceiptSectionTransfersResult {
    public processResult: ProcessResult;
    public notClearFlag: Boolean;
    public transferFlag: Boolean;
    public receiptSectionTransfers: ReceiptSectionTransfer[];
    public insertReceipts: Receipt[];
    public updateReceipts: Receipt[];
    public deleteReceipts: Receipt[];
}

