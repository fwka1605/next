export class MfAggrTransaction {
    public id: number;
    public receiptId: number | null;
    public companyId: number;
    public currencyId: number;
    public amount: number;
    public accountId: number;
    public subAccountId: number;
    public content: string;
    public payerCode: string;
    public payerName: string;
    public payerNameRaw: string;
    public recordedAt: string;
    public mfCreatedAt: string;
    public rate: number;
    public convertedAmount: number;
    public toCurrencyId: number;
    public excludeCategoryId: number | null;
    public createBy: number;
    public createAt: string;

    public IsIncome(): boolean {
        return this.amount > 0;
    }

    public IncomeAmount(): any {
        return this.IsIncome() ? this.amount : null;
    }

    public OutgoingsAmount() : any {
        return this.IsIncome() ? null : (this.amount * -1);
    }

}