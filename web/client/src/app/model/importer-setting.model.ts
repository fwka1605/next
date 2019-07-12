import {ImporterSettingDetail} from './importer-setting-detail.model';
export class ImporterSetting {
    public id: number;
    public companyId: number;
    public formatId: number;
    public code: string | null;
    public name: string | null;
    public initialDirectory: string | null;
    public encodingCodePage: number;
    public startLineCount: number;
    public ignoreLastLine: number;
    public autoCreationCustomer: number;
    public postAction: number;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
    public fieldName: string | null;
    public importDivisions: number;
    public sequence: number;
    public fieldIndex: number;
    public caption: string | null;
    public attributeDivisions: number;
    public itemPriority: number;
    public detailUpdateAt: string;
    public isUnique: number;
    public fixedValue: string | null;
    public details: ImporterSettingDetail[];
}

