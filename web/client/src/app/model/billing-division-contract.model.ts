export class BillingDivisionContract {
    public id: number;
    public companyId: number;
    public customerId: number;
    public contractNumber: string | null;
    public firstDateType: number;
    public monthly: number;
    public basisDay: number;
    public divisionCount: number;
    public roundingMode: number;
    public remainsApportionment: number;
    public billingId: number | null;
    public billingAmount: number;
    public comfirm: number;
    public cancelDate: string | null;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
}

