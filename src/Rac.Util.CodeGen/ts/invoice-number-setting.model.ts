export class InvoiceNumberSetting {
    public companyId: number;
    public useNumbering: number;
    public length: number;
    public zeroPadding: number;
    public resetType: number;
    public resetMonth: number | null;
    public formatType: number;
    public dateType: number | null;
    public dateFormat: number | null;
    public fixedStringType: number | null;
    public fixedString: string | null;
    public displayFormat: number;
    public delimiter: string | null;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
}

