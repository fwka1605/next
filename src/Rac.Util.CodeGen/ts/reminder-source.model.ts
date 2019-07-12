import {ReminderOutputed} from './reminder-outputed.model';
import {ReminderSummarySetting} from './reminder-summary-setting.model';
import {ReminderCommonSetting} from './reminder-common-setting.model';
import {ReminderSummary} from './reminder-summary.model';
import {Reminder} from './reminder.model';
export class ReminderSource {
    public companyId: number;
    public loginUserId: number;
    public useForeignCurrency: number;
    public items: Reminder[];
    public summaries: ReminderSummary[];
    public reminderIds: number[];
    public customerIds: number[];
    public setting: ReminderCommonSetting;
    public summarySettings: ReminderSummarySetting[];
    public reminderOutputed: ReminderOutputed;
    public reminderOutputeds: ReminderOutputed[];
}

