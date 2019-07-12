export class ReminderOutputed {
    public outputNo: number;
    public billingId: number;
    public reminderId: number;
    public reminderTemplateId: number;
    public outputAt: string;
    public customerId: number;
    public customerCode: string | null;
    public customerName: string | null;
    public billingCount: number;
    public currencyCode: string | null;
    public billingAmount: number;
    public remainAmount: number;
    public destinationId: number | null;
    public destinationCode: string | null;
    public destinationDisplay: string | null;
    public outputNos: number[];
    public destinationIds: number[];
}

