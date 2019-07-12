import {ReceiptHeader} from './receipt-header.model';
export class ImportFileLog {
    public id: number;
    public companyId: number;
    public fileName: string | null;
    public fileSize: number;
    public readCount: number;
    public importCount: number;
    public importAmount: number;
    public createBy: number;
    public createAt: string;
    public apportioned: number;
    public receiptHeaders: ReceiptHeader[];
}

