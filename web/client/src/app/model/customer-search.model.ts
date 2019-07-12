export class CustomerSearch {
    public companyId: number | null;
    public ids: number[];
    public codes: string[];
    public name: string | null;
    public customerCodeFrom: string | null;
    public customerCodeTo: string | null;
    public closingDay: number | null;
    public shareTransferFee: number | null;
    public staffCodeFrom: string | null;
    public staffCodeTo: string | null;
    public updateAtFrom: string | null;
    public updateAtTo: string | null;
    public isParent: number | null;
    public parentCustomerId: number | null;
    public xorParentCustomerId: number | null;
    public exclusiveBankCode: string | null;
    public exclusiveBranchCode: string | null;
    public exclusiveAccountNumber: string | null;
}

