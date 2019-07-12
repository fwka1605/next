export class MfAggrSubAccount {
    public id: number;
    public accountId: number;
    public name: string;
    public accountTypeName: string;
    public accountTypeId: number | null;
    public accountNumber: string;
    public branchCode: string;
    public receiptCategoryId: number;
    public sectionId: number | null;
    public tagIds: number[];
}