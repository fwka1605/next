import {Setting} from './setting.model';
export class ReportSetting {
    public companyId: number;
    public reportId: string | null;
    public displayOrder: number;
    public isText: number;
    public caption: string | null;
    public itemId: string | null;
    public itemKey: string | null;
    public itemValue: string | null;
    public settingList: Setting[];
}

