export class AccountTransferLog {
    public id: number;
    public companyId: number;
    public collectCategoryId: number;
    public paymentAgencyId: number;
    public requestDate: string;
    public dueAt: string;
    public outputCount: number;
    public outputAmount: number;
    public createBy: number;
    public createAt: string;
    public collectCategoryCode: string | null;
    public collectCategoryName: string | null;
    public paymentAgencyCode: string | null;
    public paymentAgencyName: string | null;
}

