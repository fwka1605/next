export class BillingJournalizingOption {
    public companyId: number;
    public currencyId: number | null;
    public loginUserId: number | null;
    public isOutuptted: Boolean;
    public billedAtFrom: string | null;
    public billedAtTo: string | null;
    public outputAt: string[];
    public precision: number;
}

