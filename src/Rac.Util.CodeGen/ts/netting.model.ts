export class Netting {
    public id: number;
    public companyId: number;
    public currencyId: number;
    public customerId: number;
    public receiptCategoryId: number;
    public sectionId: number | null;
    public receiptId: number | null;
    public recordedAt: string;
    public dueAt: string | null;
    public amount: number;
    public assignmentFlag: number;
    public note: string | null;
    public receiptMemo: string | null;
    public categoryCode: string | null;
    public currencyCode: string | null;
    public customerCode: string | null;
    public customerKana: string | null;
    public customerName: string | null;
    public useAdvanceReceived: string | null;
}

