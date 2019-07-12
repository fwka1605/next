export class CreditAgingList {
    public recordType: number;
    public customerId: number;
    public parentCustomerId: number | null;
    public parentCustomerFlag: number;
    public departmentCode: string | null;
    public departmentName: string | null;
    public staffCode: string | null;
    public staffName: string | null;
    public customerCode: string | null;
    public customerName: string | null;
    public parentCustomerCode: string | null;
    public parentCustomerName: string | null;
    public collectCategory: string | null;
    public creditAmount: number;
    public unsettledRemain: number;
    public billingRemain: number;
    public creditLimit: number | null;
    public creditBalance: number | null;
    public arrivalDueDate1: number;
    public arrivalDueDate2: number;
    public arrivalDueDate3: number;
    public arrivalDueDate4: number;
}

