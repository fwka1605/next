export class BankAccountSearch {
    public useCommonSearch: Boolean;
    public companyId: number | null;
    public bankCodes: string[];
    public branchCodes: string[];
    public accountTypeId: number;
    public accountNumber: string | null;
    public bankName: string | null;
    public branchName: string | null;
    public receiptCategoryId: number | null;
    public sectionId: number | null;
    public importSkipping: number | null;
    public searchWord: string | null;
    public ids: number[];
}

