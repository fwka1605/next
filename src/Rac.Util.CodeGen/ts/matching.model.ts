export class Matching {
    public id: number;
    public receiptId: number;
    public billingId: number;
    public matchingHeaderId: number;
    public bankTransferFee: number;
    public amount: number;
    public billingRemain: number;
    public receiptRemain: number;
    public advanceReceivedOccured: number;
    public recordedAt: string;
    public taxDifference: number;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
    public currencyId: number;
    public customerId: number;
    public memo: string | null;
    public useCashOnDueDates: number;
    public companyId: number;
    public payerName: string | null;
    public sourceBankName: string | null;
    public sourceBranchName: string | null;
    public discountAmount: number | null;
    public discountAmount1: number;
    public discountAmount2: number;
    public discountAmount3: number;
    public discountAmount4: number;
    public discountAmount5: number;
    public isNetting: Boolean;
    public offsetAmount: number;
    public outputAt: string | null;
    public receiptHeaderId: number | null;
}

