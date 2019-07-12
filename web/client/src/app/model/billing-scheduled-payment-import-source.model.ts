import {ScheduledPaymentImport} from './scheduled-payment-import.model';
import {ImporterSettingDetail} from './importer-setting-detail.model';
export class BillingScheduledPaymentImportSource {
    public companyId: number;
    public loginUserId: number;
    public importerSettingId: number;
    public details: ImporterSettingDetail[];
    public items: ScheduledPaymentImport[];
}

