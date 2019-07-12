import {ImportDataDetail} from './import-data-detail.model';
export class ImportData {
    public id: number;
    public companyId: number;
    public fileName: string | null;
    public fileSize: number;
    public createBy: number;
    public createAt: string;
    public details: ImportDataDetail[];
}

