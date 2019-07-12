export class CollationSearch {
    public clientKey: string | null;
    public companyId: number;
    public currencyId: number | null;
    public recordedAtFrom: string | null;
    public recordedAtTo: string | null;
    public dueAtFrom: string | null;
    public dueAtTo: string | null;
    public billingType: number;
    public limitDateType: number;
    public amountType: number;
    public useDepartmentWork: Boolean;
    public useSectionWork: Boolean;
    public approved: Boolean;
    public loginUserId: number;
    public doTransferAdvanceReceived: Boolean;
    public recordedAtType: number;
    public advanceReceivedRecordedAt: string | null;
    public useAdvanceReceived: Boolean;
    public createAtFrom: string | null;
    public createAtTo: string | null;
}

