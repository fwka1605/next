export class CustomerGroup {
    public parentCustomerId: number;
    public childCustomerId: number;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
    public parentCustomerCode: string | null;
    public parentCustomerName: string | null;
    public parentCustomerKana: string | null;
    public childCustomerCode: string | null;
    public childCustomerName: string | null;
}

