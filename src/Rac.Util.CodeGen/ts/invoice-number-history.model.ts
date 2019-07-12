export class InvoiceNumberHistory {
    public id: number;
    public companyId: number;
    public numberingYear: string | null;
    public numberingMonth: string | null;
    public fixedString: string | null;
    public lastNumber: number;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
}

