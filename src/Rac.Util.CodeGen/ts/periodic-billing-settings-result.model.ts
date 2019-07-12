import {PeriodicBillingSetting} from './periodic-billing-setting.model';
import {ProcessResult} from './process-result.model';
export class PeriodicBillingSettingsResult {
    public processResult: ProcessResult;
    public periodicBillingSettings: PeriodicBillingSetting[];
}

