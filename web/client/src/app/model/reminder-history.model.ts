export class ReminderHistory {
    public id: number;
    public reminderId: number;
    public companyId: number;
    public statusId: number;
    public statusCode: string | null;
    public statusName: string | null;
    public memo: string | null;
    public outputFlag: number;
    public inputType: number;
    public createAt: string;
    public createBy: number;
    public createByName: string | null;
    public customerCode: string | null;
    public customerName: string | null;
    public calculateBaseDate: string | null;
    public billingCount: number | null;
    public currencyCode: string | null;
    public billingAmount: number | null;
    public reminderAmount: number | null;
    public isUpdateStatusMemo: Boolean;
    public arrearsDays: number;
}

