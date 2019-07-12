export class Currency {
    public id: number;
    public companyId: number;
    public code: string | null;
    public name: string | null;
    public symbol: string | null;
    public precision: number;
    public note: string | null;
    public displayOrder: number;
    public tolerance: number;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
}

