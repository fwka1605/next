import {ReceiptExclude} from './receipt-exclude.model';
export class ReceiptApportion {
    public id: number;
    public receiptHeaderId: number;
    public companyId: number;
    public currencyId: number;
    public excludeFlag: number;
    public excludeCategoryId: number | null;
    public excludeAmount: number | null;
    public sectionId: number | null;
    public customerId: number | null;
    public payerName: string | null;
    public payerNameRaw: string | null;
    public receiptAmount: number;
    public recordedAt: string;
    public workday: string;
    public sourceBankName: string | null;
    public sourceBranchName: string | null;
    public apportioned: number;
    public updateBy: number;
    public updateAt: string;
    public excludeVirtualBranchCode: string | null;
    public excludeAccountNumber: string | null;
    public currencyCode: string | null;
    public sectionCode: string | null;
    public sectionName: string | null;
    public customerCode: string | null;
    public customerName: string | null;
    public refCustomerId: number | null;
    public refCustomerCode: string | null;
    public refCustomerName: string | null;
    public learnIgnoreKana: number;
    public doDelete: number;
    public receiptExcludes: ReceiptExclude[];
    public learnKanaHistory: number;
}

