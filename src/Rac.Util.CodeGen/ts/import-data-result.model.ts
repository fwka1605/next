import {ImportData} from './import-data.model';
export class ImportDataResult {
    public importData: ImportData;
    public logs: string[];
    public readCount: number;
    public validCount: number;
    public invalidCount: number;
    public saveCount: number;
    public saveAmount: number;
    public newCustomerCreationCount: number;
}

