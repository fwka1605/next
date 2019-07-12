import {ReportSetting} from './report-setting.model';
export class ScheduledPaymentListSearch {
    public companyId: number;
    public baseDate: string;
    public billedAtFrom: string | null;
    public billedAtTo: string | null;
    public dueAtFrom: string | null;
    public dueAtTo: string | null;
    public closingAtFrom: string | null;
    public closingAtTo: string | null;
    public invoiceCodeFrom: string | null;
    public invoiceCodeTo: string | null;
    public invoiceCode: string | null;
    public categoryCode: string | null;
    public currencyCode: string | null;
    public departmentCodeFrom: string | null;
    public departmentCodeTo: string | null;
    public staffCodeFrom: string | null;
    public staffCodeTo: string | null;
    public customerCodeFrom: string | null;
    public customerCodeTo: string | null;
    public customerSummaryFlag: Boolean;
    public precision: number;
    public reportSettings: ReportSetting[];
}

