export class MatchingHistorySearch {
    public companyId: number;
    public loginUserId: number;
    public outputOrder: number;
    public inputAtFrom: string | null;
    public inputAtTo: string | null;
    public recordedAtFrom: string | null;
    public recordedAtTo: string | null;
    public receiptIdFrom: number | null;
    public receiptIdTo: number | null;
    public bankCode: string | null;
    public branchCode: string | null;
    public accountType: number | null;
    public accountNumber: string | null;
    public currencyCode: string | null;
    public createAtFrom: string | null;
    public createAtTo: string | null;
    public customerCodeFrom: string | null;
    public customerCodeTo: string | null;
    public departmentCodeFrom: string | null;
    public departmentCodeTo: string | null;
    public sectionCodeFrom: string | null;
    public sectionCodeTo: string | null;
    public useSectionMaster: Boolean;
    public loginUserFrom: string | null;
    public loginUserTo: string | null;
    public existsMemo: Boolean;
    public memo: string | null;
    public billingAmountFrom: number | null;
    public billingAmountTo: number | null;
    public receiptAmountFrom: number | null;
    public receiptAmountTo: number | null;
    public billingCategoryId: number | null;
    public onlyNonOutput: Boolean;
    public matchingProcessType: number | null;
    public payerName: string | null;
    public requireSubtotal: Boolean;
}

