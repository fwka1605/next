export class CustomerLedger {
    public recordedAt: string | null;
    public yearMonth: string | null;
    public sectionName: string | null;
    public departmentName: string | null;
    public invoiceCode: string | null;
    public categoryName: string | null;
    public accountTitleName: string | null;
    public currencyCode: string | null;
    public matchingSymbolBilling: string | null;
    public billingAmount: number | null;
    public slipTotal: number | null;
    public receiptAmount: number | null;
    public matchingSymbolReceipt: string | null;
    public matchingAmount: number | null;
    public remainAmount: number | null;
    public customerCode: string | null;
    public customerName: string | null;
    public parentCustomerId: number;
    public parentCustomerCode: string | null;
    public parentCustomerName: string | null;
    public recordType: number;
    public dataType: number;
    public recordTypeName: string | null;
}

