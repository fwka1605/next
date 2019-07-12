export class ArrearagesList {
    public id: number;
    public customerCode: string | null;
    public customerName: string | null;
    public currencyCode: string | null;
    public billedAt: string;
    public salesAt: string;
    public closingAt: string;
    public dueAt: string;
    public originalDueAt: string;
    public remainAmount: number;
    public baseDate: string;
    public arrearagesDayCount: number;
    public collectCategoryCode: string | null;
    public collectCategoryName: string | null;
    public invoiceCode: string | null;
    public note1: string | null;
    public note2: string | null;
    public note3: string | null;
    public note4: string | null;
    public memo: string | null;
    public customerStaffName: string | null;
    public customerNote: string | null;
    public tel: string | null;
    public departmentCode: string | null;
    public departmentName: string | null;
    public staffCode: string | null;
    public staffName: string | null;
    public offsetAmount: number;
    public totalAmount: number;
}

