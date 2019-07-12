export class BillingInvoiceSearch {
    public clientKey: string | null;
    public companyId: number;
    public billedAtFrom: string | null;
    public billedAtTo: string | null;
    public closingAt: string | null;
    public collectCategoryId: number;
    public departmentCodeFrom: string | null;
    public departmentCodeTo: string | null;
    public customerCodeFrom: string | null;
    public customerCodeTo: string | null;
    public isPublished: Boolean;
    public reportId: string | null;
    public reportInvoiceAmount: number;
    public publishAtFrom: string | null;
    public publishAtTo: string | null;
    public publishAtFirstFrom: string | null;
    public publishAtFirstTo: string | null;
    public invoiceCodeFrom: string | null;
    public invoiceCodeTo: string | null;
    public invoiceCode: string | null;
    public staffCodeFrom: string | null;
    public staffCodeTo: string | null;
    public assignmentFlg: number;
    public connectionId: string | null;
    public billingIds: number[];
    public billingInputIds: number[];
}

