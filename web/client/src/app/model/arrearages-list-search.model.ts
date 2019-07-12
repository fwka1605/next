import {ReportSetting} from './report-setting.model';
export class ArrearagesListSearch {
    public companyId: number;
    public baseDate: string;
    public existsMemo: Boolean;
    public billingMemo: string | null;
    public currencyCode: string | null;
    public departmentCodeFrom: string | null;
    public departmentCodeTo: string | null;
    public staffCodeFrom: string | null;
    public staffCodeTo: string | null;
    public customerSummaryFlag: Boolean;
    public reportSettings: ReportSetting[];
    public precision: number;
}

