import {PeriodicBillingSettingDetail} from './periodic-billing-setting-detail.model';
export class PeriodicBillingSetting {
    public id: number;
    public companyId: number;
    public name: string | null;
    public currencyId: number;
    public customerId: number;
    public destinationId: number | null;
    public departmentId: number;
    public staffId: number;
    public collectCategoryId: number;
    public billedCycle: number;
    public billedDay: number;
    public startMonth: string;
    public endMonth: string | null;
    public invoiceCode: string | null;
    public setBillingNote1: number;
    public setBillingNote2: number;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
    public customerCode: string | null;
    public customerName: string | null;
    public destinationCode: string | null;
    public addressee: string | null;
    public collectCategoryName: string | null;
    public departmentCode: string | null;
    public departmentName: string | null;
    public staffCode: string | null;
    public staffName: string | null;
    public lastUpdateAt: string;
    public lastUpdatedBy: string | null;
    public billedAt: string;
    public baseDate: string;
    public lastCreateYearMonth: string | null;
    public details: PeriodicBillingSettingDetail[];
    public destinationName: string | null;
}
