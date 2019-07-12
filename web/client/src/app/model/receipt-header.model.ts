import {ReceiptExclude} from './receipt-exclude.model';
import {Receipt} from './receipt.model';
export class ReceiptHeader {
    public id: number;
    public companyId: number;
    public fileType: number;
    public currencyId: number;
    public importFileLogId: number;
    public workday: string;
    public bankCode: string | null;
    public bankName: string | null;
    public branchCode: string | null;
    public branchName: string | null;
    public accountTypeId: number;
    public accountNumber: string | null;
    public accountName: string | null;
    public assignmentFlag: number;
    public importCount: number;
    public importAmount: number;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
    public accountTypeName: string | null;
    public currencyCode: string | null;
    public existApportioned: number;
    public isAllApportioned: number;
    public receipts: Receipt[];
    public receiptExcludes: ReceiptExclude[];
}

