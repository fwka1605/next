export class BillingAgingList {
    public recordType: number;
    public currencyId: number;
    public parentCustomerId: number | null;
    public customerId: number;
    public parentCustomerFlag: number;
    public currencyCode: string | null;
    public customerCode: string | null;
    public customerName: string | null;
    public departmentCode: string | null;
    public departmentName: string | null;
    public staffCode: string | null;
    public staffName: string | null;
    public parentCustomerCode: string | null;
    public parentCustomerName: string | null;
    public lastMonthRemain: number | null;
    public currentMonthSales: number;
    public currentMonthReceipt: number | null;
    public currentMonthMatching: number;
    public currentMonthRemain: number | null;
    public monthlyRemain0: number | null;
    public monthlyRemain1: number | null;
    public monthlyRemain2: number | null;
    public monthlyRemain3: number | null;
}

