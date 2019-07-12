import {ReminderSummarySetting} from './reminder-summary-setting.model';
import {ReminderCommonSetting} from './reminder-common-setting.model';
export class ReminderSearch {
    public companyId: number;
    public calculateBaseDate: string;
    public containReminderAmountZero: Boolean;
    public existsMemo: Boolean;
    public billingMemo: string | null;
    public arrearDaysFrom: number | null;
    public arrearDaysTo: number | null;
    public currencyCode: string | null;
    public departmentCodeFrom: string | null;
    public departmentCodeTo: string | null;
    public staffCodeFrom: string | null;
    public staffCodeTo: string | null;
    public customerCodeFrom: string | null;
    public customerCodeTo: string | null;
    public customerName: string | null;
    public reminderMemo: string | null;
    public status: number;
    public outputFlag: number | null;
    public reminderManaged: Boolean;
    public removeExcludeReminderPublishCustomer: Boolean;
    public assignmentFlg: number;
    public createAtFrom: string | null;
    public createAtTo: string | null;
    public createByCode: string | null;
    public setting: ReminderCommonSetting;
    public summarySettings: ReminderSummarySetting[];
}

