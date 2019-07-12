export class CustomerLedgerSearch {
    public customerCodeFrom: string | null;
    public customerCodeTo: string | null;
    public yearMonthFrom: string;
    public yearMonthTo: string;
    public closingDay: number | null;
    public companyId: number;
    public currencyId: number;
    public useBilledAt: Boolean;
    public remainType: number;
    public displayDepartment: Boolean;
    public displaySection: Boolean;
    public doGroupReceipt: Boolean;
    public displayMatchingSymbol: Boolean;
    public groupBillingType: number;
    public billingSlipType: number;
    public requireMonthlyBreak: Boolean;
    public unitPrice: number;
    public isPrint: Boolean;
    public precision: number;
}

