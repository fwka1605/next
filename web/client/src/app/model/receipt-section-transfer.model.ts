export class ReceiptSectionTransfer {
    public sourceReceiptId: number;
    public destinationReceiptId: number;
    public sourceSectionId: number;
    public destinationSectionId: number;
    public sourceAmount: number;
    public destinationAmount: number;
    public printFlag: number;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
    public transferMemo: string | null;
    public cancelFlag: Boolean;
    public receiptId: number;
    public recordedAt: string;
    public dueAt: string | null;
    public receiptCategoryCode: string | null;
    public receiptCategoryName: string | null;
    public inputType: number;
    public payerName: string | null;
    public note1: string | null;
    public sourceSectionCode: string | null;
    public sourceSectionName: string | null;
    public destinationSectionCode: string | null;
    public destinationSectionName: string | null;
    public memo: string | null;
    public loginUserCode: string | null;
    public loginUserName: string | null;
    public currencyCode: string | null;
    public precision: number;
}

