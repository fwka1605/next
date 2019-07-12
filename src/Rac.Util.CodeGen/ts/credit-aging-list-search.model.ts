export class CreditAgingListSearch {
    public companyId: number;
    public yearMonth: string;
    public closingDay: number | null;
    public departmentCodeFrom: string | null;
    public departmentCodeTo: string | null;
    public staffCodeFrom: string | null;
    public staffCodeTo: string | null;
    public customerCodeFrom: string | null;
    public customerCodeTo: string | null;
    public filterPositiveCreditBalance: Boolean;
    public considerGroupWithCalculate: Boolean;
    public calculateCreditLimitRegistered: Boolean;
    public requireDepartmentTotal: Boolean;
    public requireStaffTotal: Boolean;
    public useMasterStaff: Boolean;
    public considerCustomerGroup: Boolean;
    public considerReceiptAmount: Boolean;
    public useBilledAt: Boolean;
    public unitPrice: number;
    public useParentCustomerCredit: Boolean;
    public displayCustomerCode: Boolean;
}

