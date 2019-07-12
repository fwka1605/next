export class BillingJournalizing {
    public companyId: number;
    public slipNumber: number;
    public debitDepartmentCode: string | null;
    public debitDepartmentName: string | null;
    public debitAccountTitleCode: string | null;
    public debitAccountTitleName: string | null;
    public debitSubCode: string | null;
    public debitSubName: string | null;
    public creditDepartmentCode: string | null;
    public creditDepartmentName: string | null;
    public creditAccountTitleCode: string | null;
    public creditAccountTitleName: string | null;
    public creditSubCode: string | null;
    public creditSubName: string | null;
    public currencyCode: string | null;
    public billingAmount: number;
    public billedAt: string;
    public note: string | null;
    public customerCode: string | null;
    public customerName: string | null;
    public invoiceCode: string | null;
    public staffCode: string | null;
    public payerCode: string | null;
    public payerName: string | null;
    public sourceBankName: string | null;
    public sourceBranchName: string | null;
    public dueAt: string;
    public bankCode: string | null;
    public bankName: string | null;
    public branchCode: string | null;
    public branchName: string | null;
    public accountType: number | null;
    public accountNumber: string | null;
}

