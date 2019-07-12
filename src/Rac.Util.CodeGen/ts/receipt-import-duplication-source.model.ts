import {ImporterSettingDetail} from './importer-setting-detail.model';
import {ReceiptImportDuplication} from './receipt-import-duplication.model';
export class ReceiptImportDuplicationSource {
    public companyId: number;
    public importerSettingId: number;
    public items: ReceiptImportDuplication[];
    public details: ImporterSettingDetail[];
}

