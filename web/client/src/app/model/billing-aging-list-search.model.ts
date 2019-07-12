export class BillingAgingListSearch {
    public companyId: number;
    public loginUserId: number;
    public yearMonth: string;
    public targetDate: number;
    public closingDay: number | null;
    public currencyId: number | null;
    public departmentCodeFrom: string | null;
    public departmentCodeTo: string | null;
    public staffCodeFrom: string | null;
    public staffCodeTo: string | null;
    public customerCodeFrom: string | null;
    public customerCodeTo: string | null;
    public departmentNameFrom: string | null;
    public departmentNameTo: string | null;
    public staffNameFrom: string | null;
    public staffNameTo: string | null;
    public isMasterStaff: Boolean;
    public requireStaffSubtotal: Boolean;
    public requireDepartmentSubtotal: Boolean;
    public considerCustomerGroup: Boolean;
    public billingRemainType: number;
    public unitValue: number;
    public precision: number;
    public customerId: number;
    public monthOffset: number;
}

