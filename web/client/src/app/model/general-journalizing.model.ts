export class GeneralJournalizing {
    public journalizingType: number;
    public companyId: number;
    public currencyId: number;
    public currencyCode: string | null;
    public precision: number;
    public amount: number;
    public approved: Boolean;
    public createAt: string;
    public receiptId: number;
    public billingId: number | null;
    public matchingId: number | null;
    public scheduledIncomeReceiptId: number | null;
    public scheduledIncomeBillingId: number | null;
    public scheduledIncomeMatchingHeaderId: number | null;
    public advanceReceivedBackupId: number | null;
    public receiptExcludeId: number | null;
    public receiptCustomerId: number | null;
    public receiptParentCustomerId: number | null;
    public billingCustomerId: number | null;
    public billingParentCustomerId: number | null;
    public advanceReceivedCustomerId: number | null;
    public advanceReceivedParentCustomerId: number | null;
    public billingDepartmentId: number | null;
    public billingDepartmentStaffId: number | null;
    public billingStaffId: number | null;
    public billingStaffDepartmentId: number | null;
}

