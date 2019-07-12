import {ReceiptInput} from './receipt-input.model';
export class ReceiptSaveItem {
    public receipts: ReceiptInput[];
    public parentCustomerId: number | null;
    public clientKey: string | null;
}

