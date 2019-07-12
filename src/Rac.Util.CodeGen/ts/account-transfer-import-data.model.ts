export class AccountTransferImportData {
    public billingIdList: number;
    public resultCode: number;
    public dueDateOffset: number | null;
    public collectCategoryId: number | null;
    public customerIds: number[];
    public doUpdateAccountTransferLogId: Boolean;
    public dueDate: string | null;
    public updateBy: number;
}

