export class BillingDueAtModify {
    public id: number | null;
    public billingInputId: number | null;
    public companyId: number;
    public currencyId: number;
    public customerId: number;
    public invoiceCode: string | null;
    public targetAmount: number;
    public billedAt: string;
    public closingAt: string;
    public dueAt: string;
    public originalDueAt: string | null;
    public modifiedDueAt: string | null;
    public collectCategoryId: number;
    public originalCollectCategoryId: number | null;
    public customerCode: string | null;
    public customerName: string | null;
    public currencyCode: string | null;
    public collectCategoryCode: string | null;
    public collectCategoryName: string | null;
    public originalCollectCategoryCode: string | null;
    public originalCollectCategoryName: string | null;
    public updateBy: number;
    public updateAt: string;
    public newDueAt: string | null;
    public newCollectCategoryId: number | null;
}

