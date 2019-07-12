import {ImporterSettingDetail} from './importer-setting-detail.model';
import {BillingImportDuplication} from './billing-import-duplication.model';
export class BillingImportDuplicationSource {
    public companyId: number;
    public importerSettingId: number;
    public items: BillingImportDuplication[];
    public details: ImporterSettingDetail[];
}

