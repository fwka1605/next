import {ImporterSettingDetail} from './importer-setting-detail.model';
import {ReceiptImportDuplication} from './receipt-import-duplication.model';
export class ReceiptImportDuplicationSearch {
    public companyId: number;
    public items: ReceiptImportDuplication[];
    public details: ImporterSettingDetail[];
}

