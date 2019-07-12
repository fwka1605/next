import {ImporterSettingDetail} from './importer-setting-detail.model';
import {BillingImportDuplicationWithCode} from './billing-import-duplication-with-code.model';
export class BillingImportDuplicationSearch {
    public companyId: number;
    public items: BillingImportDuplicationWithCode[];
    public details: ImporterSettingDetail[];
}

