import {ImportFileLog} from './import-file-log.model';
export class EbFileInformation {
    public filePath: string | null;
    public file: string | null;
    public format: number;
    public fileFieldType: number;
    public bankCode: string | null;
    public importableValue: string | null;
    public useValueDate: Boolean;
    public result: number;
    public index: number;
    public importFileLog: ImportFileLog;
    public bankInformation: string | null;
    public companyId: number;
    public loginUserId: number;
    public year: number;
}

