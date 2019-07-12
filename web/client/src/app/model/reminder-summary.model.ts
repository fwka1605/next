export class ReminderSummary {
    public id: number;
    public companyId: number;
    public currencyId: number;
    public currencyCode: string | null;
    public customerId: number;
    public customerCode: string | null;
    public customerName: string | null;
    public reminderCount: number;
    public billingCount: number;
    public remainAmount: number;
    public reminderAmount: number;
    public memo: string | null;
    public destinationId: number | null;
    public destinationCode: string | null;
    public destinationDepartmentName: string | null;
    public customerStaffName: string | null;
    public customerNote: string | null;
    public customerTel: string | null;
    public customerFax: string | null;
    public excludeReminderPublish: number;
    public customerIds: number[];
    public destinationIds: number[];
    public destinationIdInput: number | null;
    public noDestination: Boolean;
}

